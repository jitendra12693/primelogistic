using InTimeCourier.App_Start;
using InTimeCourier.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace InTimeCourier.Controllers
{
    [SessionAuthorize]
    public class BillDetailsController : Controller
    {
        InLogisticModel db = new InLogisticModel();
        // GET: BillDetails
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult SearchBill()
        {
            return View();
        }

        public ActionResult BillList()
        {
            return View();
        }

        public JsonResult SearchBillDetails(string SearchTerm, string FromDate, string ToDate, string billDate,long? partyId)
        {
            BillDetails bill = new Models.BillDetails();
            bill.SearchTerm = SearchTerm;
            bill.BillDate = billDate;
            bill.PeriodFrom = FromDate;
            bill.PeriodTo = ToDate;
            bill.PartyId = 0;
            var billList = GetBillListDapper(bill).AsEnumerable().ToList();
            string html = RenderPartialViewToString("BillList", billList);
            return Json(html, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AddBillDetails(long id)
        {
            BillDetails obj = new BillDetails();
            obj.BillId = id;
            List<BillDetails> billList = GetBillListDapper(obj).ToList();
            return PartialView("AddBillDetails",billList[0]);
        }
        public ActionResult SelectedPartyCourrierList()
        {
            Session["PartyId"] = Request.QueryString["PartyId"];
            Session["PeriodFrom"] = Convert.ToDateTime(Request.QueryString["PeriodFrom"]).ToShortDateString();
            Session["PeriodTo"] = Convert.ToDateTime(Request.QueryString["PeriodTo"]).ToShortDateString();
            return View();
        }
        [HttpPost]
        public ActionResult AddBillDetails(BillDetails billDetails)
        {
            DapperClass objDb = new DapperClass();
            string sprcname = "uspUpdateBillDetails";
            object Paramenters = new
            {
                //PartyId = billDetails.PartyId,
                BillDate = String.IsNullOrEmpty(billDetails.BillDate) ? "" : billDetails.BillDate,
                FromDate = String.IsNullOrEmpty(billDetails.PeriodFrom) ? "" : billDetails.PeriodFrom,
                ToDate = String.IsNullOrEmpty(billDetails.PeriodTo) ? "" : billDetails.PeriodTo,
                UpdatedBy =  int.Parse("0"+ Session["UserId"]),
                BillId = billDetails.BillId
            };
            var courrierRes = objDb.Exec_SPrc<CorierResponse>(sprcname, Paramenters, "InLogisticModel", "primelogisticdb", true).FirstOrDefault();
            TempData["Message"] = courrierRes.Message;
            return RedirectToAction("Index");
        }
        public ActionResult BillPayment(long id)
        {
            BillPayment pay = new Models.BillPayment();
            pay.BillId = id;
            var paymentDetails = GetPaymentDetails(pay).AsEnumerable().FirstOrDefault();
            return View(paymentDetails);
        }
        [HttpPost]
        public ActionResult BillPayment(BillPayment payment)
        {
            var response = db.Database.SqlQuery<Responsedetails>("exec uspInsertPayment @PartyId,@BillId,@BillAmount,@BalanceAmount,@PaidAmount,@PaymentAmount,@ChequeNo,@BankName,@ChequeDate,@UserId",
                new SqlParameter("@PartyId",payment.PartyId),
                new SqlParameter("@BillId",payment.BillId),
                new SqlParameter("@BillAmount",payment.BillAmount),
                new SqlParameter("@BalanceAmount",payment.BalanceAmount),
                new SqlParameter("@PaidAmount", payment.PaidAmount),
                new SqlParameter("@PaymentAmount",payment.PaymentAmount),
                new SqlParameter("@ChequeNo",payment.ChequeNo ?? (object)DBNull.Value),
                new SqlParameter("@BankName",payment.BankName ?? (object)DBNull.Value),
                new SqlParameter("@ChequeDate",payment.ChequeDate ?? (object)DBNull.Value),
                new SqlParameter("@UserId",int.Parse("0"+ Session["UserId"]))).FirstOrDefault();
            TempData["PaymentMessage"] = response.Message;
            return RedirectToAction("Index");
        }
        public IEnumerable<BillDetails> GetBillListDapper(BillDetails obj)
        {
            DapperClass objDb = new DapperClass();
            string sprcname = "uspSelectAllBill";
            object Paramenters = new {PartyId = obj.PartyId,
                SearchTerm = String.IsNullOrEmpty(obj.SearchTerm) ? "" : obj.SearchTerm,
                BillDate = String.IsNullOrEmpty(obj.BillDate) ? "" : obj.BillDate,
                FromDate = String.IsNullOrEmpty(obj.PeriodFrom) ? "" : obj.PeriodFrom,
                ToDate = String.IsNullOrEmpty(obj.PeriodTo) ? "" : obj.PeriodTo,
                BillId=obj.BillId
            };
            return objDb.Exec_SPrc<BillDetails>(sprcname, Paramenters, "InLogisticModel", "primelogisticdb", true);
        }
        public IEnumerable<BillPayment> GetPaymentDetails(BillPayment payment)
        {
            DapperClass objDb = new DapperClass();
            string sprcname = "uspSearchPaymentDetails";
            object Paramenters = new
            {
                BillId = payment.BillId
            };
            return objDb.Exec_SPrc<BillPayment>(sprcname, Paramenters, "InLogisticModel", "primelogisticdb", true);
        }
        public string RenderPartialViewToString(string viewName, object model)
        {
            if (string.IsNullOrEmpty(viewName))
                viewName = ControllerContext.RouteData.GetRequiredString("action");
            ViewData.Model = model;
            using (StringWriter sw = new StringWriter())
            {
                ViewEngineResult viewResult = ViewEngines.Engines.FindPartialView(ControllerContext, viewName);
                ViewContext viewContext = new ViewContext(ControllerContext, viewResult.View, ViewData, TempData, sw);
                viewResult.View.Render(viewContext, sw);
                return sw.GetStringBuilder().ToString();
            }
        }

        [HttpPost]
        public JsonResult DeleteBillDetails(int BillId)
        {
            SqlConnection connString = new SqlConnection(db.Database.Connection.ConnectionString);
            if (connString.State == ConnectionState.Closed)
                connString.Open();
            SqlCommand cmd = new SqlCommand("uspDeleteBillDetails", connString);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@BillId", BillId);
            cmd.Parameters.Add("@Result", SqlDbType.Int);
            cmd.Parameters["@Result"].Direction = ParameterDirection.Output;
            cmd.ExecuteNonQuery();
            string response = Convert.ToString(cmd.Parameters["@Result"].Value);
            var v = new { responseId = response };
            return Json(v, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetNewInvoiceNo()
        {
            DataSet ds = new DataSet();
            SqlConnection connString = new SqlConnection(db.Database.Connection.ConnectionString);
            SqlCommand cmd = new SqlCommand("uspGetInvoiceNumber", connString);
            cmd.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            sda.Fill(ds);
            return Json(JsonConvert.SerializeObject(ds), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult CheckInvoiceNo(string InvoiceNo)
        {
            DataSet ds = new DataSet();
            SqlConnection connString = new SqlConnection(db.Database.Connection.ConnectionString);
            SqlCommand cmd = new SqlCommand("uspCheckInvoiceNoDetails", connString);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@InvoiceNo", InvoiceNo);
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            sda.Fill(ds);
            return Json(JsonConvert.SerializeObject(ds), JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult GetInvoiceDetails(string BillId)
        {
            DataSet ds = new DataSet();
            SqlConnection connString = new SqlConnection(db.Database.Connection.ConnectionString);
            SqlCommand cmd = new SqlCommand("uspGetBillDetailsByBillId", connString);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@BillId", BillId);
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            sda.Fill(ds);
            return Json(JsonConvert.SerializeObject(ds), JsonRequestBehavior.AllowGet);
        }
    }
    public class Responsedetails
    {
        public string ResponseId { get; set; }
        public int StatusId { get; set; }
        public string Status { get; set; }
        public string Message { get; set; }
    }
}