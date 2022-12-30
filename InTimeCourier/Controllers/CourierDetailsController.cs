using InTimeCourier.App_Start;
using InTimeCourier.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Configuration;
using System.Globalization;
using System.Reflection;

namespace InTimeCourier.Controllers
{
    [SessionAuthorize]
    public class CourierDetailsController : Controller
    {
        SqlConnection connString = new SqlConnection(ConfigurationManager.ConnectionStrings["InLogisticModel"].ToString());
        InLogisticModel db = new InLogisticModel();

        // GET: CourierDetails
        public ActionResult AllCourier()
        {
            return View();
        }
        public ActionResult CourrierList()
        {
            return View();
        }
        public ActionResult SearchList()
        {
            return View();
        }
        public JsonResult Search(long? partyId, string trackingNo, string fromDate, string toDate)
        {
            TotalDetails dtTotal = new TotalDetails();
            PartyAddressDetails dtPartyDetails = new PartyAddressDetails();

            List<CorrierDetails> list = GetCourierList(ref dtTotal, ref dtPartyDetails, partyId, trackingNo, fromDate, toDate);
            CorrierDetails objCorrierDetails = new CorrierDetails();
            objCorrierDetails.CorrierDetails1 = new List<Models.CorrierDetails>();
            objCorrierDetails.CorrierDetails2 = new List<Models.CorrierDetails>();
            for (int i = 1; i <= list.Count; i++)
            {
                if (i % 2 == 0)
                {
                    objCorrierDetails.CorrierDetails2.Add(list[i - 1]);
                }
                else
                {
                    objCorrierDetails.CorrierDetails1.Add(list[i - 1]);
                }
            }

            var html = RenderPartialViewToString("SearchList", objCorrierDetails);
            string totalInWord = ConvertInWord.ConvertToWords(dtTotal.GrandTotal.ToString());
            var v = new { html = html, TotalRecord = dtTotal, PartyDetails = dtPartyDetails, InWord = totalInWord };
            return this.Json(v, JsonRequestBehavior.AllowGet);
        }
        public ActionResult SearchCorier()
        {
            ViewBag.Party = new SelectList(db.PartyMasters.Where(x => x.IsActive == true).OrderBy(x => x.PartyName).ToList(), "PartyId", "PartyName");
            return View();
        }
        public ActionResult AddCourrier()
        {
            try
            {
                ViewBag.Location = new SelectList(db.SourceMasters.Where(x => x.IsActive == true).OrderBy(x => x.SourceId).ToList(), "SourceId", "SourceName");
                ViewBag.Party = new SelectList(db.PartyMasters.Where(x => x.IsActive == true).OrderBy(x => x.PartyName).ToList(), "PartyId", "PartyName");
                var data = db.CourrierModes.Where(x => x.IsActive == true).OrderBy(x => x.CourrierModeId);
                ViewBag.CourrierMode = new SelectList(db.CourrierModes.Where(x => x.IsActive == true).OrderBy(x => x.CourrierModeId), "CourrierModeId", "CourrierModeName");
                ViewBag.Networks = new SelectList(db.NetworkMaster.Where(x => x.IsActive == true).Select(item => new { NetworkModeId = item.NetworkId, NetworkName = item.NetworkName }).OrderBy(x => x.NetworkName), "NetworkModeId", "NetworkName");
                var list = db.CourrierMasters.OrderByDescending(x => x.CourrierId).ToList();
                ViewBag.Courrier = list.Take(12).ToList();
                return View();
            }
            catch (Exception ex)
            {
                Response.Write(ex);
                return null;
            }
        }
        [HttpPost]
        public ActionResult AddCourrier(CourrierMaster courier)
        {
            ViewBag.CourrierMode = new SelectList(db.CourrierModes.Where(x => x.IsActive == true).OrderBy(x => x.CourrierModeId), "CourrierModeId", "CourrierModeName");
            ViewBag.Location = new SelectList(db.SourceMasters.Where(x => x.IsActive == true).OrderBy(x => x.SourceId).ToList(), "SourceId", "SourceName");
            ViewBag.Party = new SelectList(db.PartyMasters.Where(x => x.IsActive == true).OrderBy(x => x.PartyName).ToList(), "PartyId", "PartyName");
            ViewBag.Networks = new SelectList(db.NetworkMaster.Where(x => x.IsActive == true).Select(item => new { NetworkModeId = item.NetworkId, NetworkName = item.NetworkName }).OrderBy(x => x.NetworkName), "NetworkModeId", "NetworkName");
            try
            {

                var response = db.Database.SqlQuery<CorierResponse>("exec uspInsertCourrierDetails @PartyId,@Amount,@CreatedBy,@TrackingNo,@CNNo,@Weight,@DepartureDt,@Rate,@Location,@CourrierModeId,@ODACharges,@NetworModeId,@Discount,@Qty",
                new SqlParameter("@PartyId", courier.PartyId),
                new SqlParameter("@CourrierModeId", courier.CourrierModeId),
                 new SqlParameter("@NetworModeId", courier.NetworkModeId),
                new SqlParameter("@ODACharges", courier.ODACharges),
                new SqlParameter("@Qty", courier.Qty),
                new SqlParameter("@Amount", courier.Amount),
                new SqlParameter("@CreatedBy", CourierHelper.UserId),
                new SqlParameter("@TrackingNo", string.Empty),
                new SqlParameter("@CNNo", courier.CNNo),
                new SqlParameter("@Weight", courier.Weight),
                new SqlParameter("@Discount", courier.Discount),
                new SqlParameter("@DepartureDt", courier.DepartureDt),
                new SqlParameter("@Location", courier.Location),
                new SqlParameter("@Rate", courier.Rate)).ToList();
                ViewBag.Message = response[0].Message; //"Your courier registered successfully with courier tracking no: " + response[0].TrackingNo;
                ViewBag.Status = response[0].Status;
                courier.TrackingNo = response[0].TrackingNo;
                ViewBag.TrackingNo = response[0].TrackingNo;

                var list = db.CourrierMasters.OrderByDescending(x => x.CourrierId).ToList();
                ViewBag.Courrier = list.Take(10).ToList();
            }
            catch (Exception ex)
            {
                throw;
            }
            return View();
        }
        public ActionResult Edit(long? id)
        {
            List<CourrierMaster> courrierList = new List<CourrierMaster>();
            try
            {
                //ViewBag.Location = new SelectList(db.SourceMasters.OrderBy(x => x.SourceId).ToList(), "SourceId", "SourceName");
                ViewBag.CourrierMode = new SelectList(db.CourrierModes.Where(x => x.IsActive == true).OrderBy(x => x.CourrierModeId), "CourrierModeId", "CourrierModeName");
                ViewBag.Party = new SelectList(db.PartyMasters.OrderBy(x => x.PartyName).ToList(), "PartyId", "PartyName");
                ViewBag.Networks = new SelectList(db.NetworkMaster.Where(x => x.IsActive == true).Select(item=>new { NetworkModeId = item.NetworkId,NetworkName=item.NetworkName }).OrderBy(x => x.NetworkName), "NetworkModeId", "NetworkName");
                var list = db.CourrierMasters.Where(x => x.CourrierId == id).ToList();
                courrierList = list;
            }
            catch (Exception ex)
            {

            }
            return View(courrierList[0]);
        }
        [HttpPost]
        public ActionResult Edit(CourrierMaster courrier)
        {
            //if (ModelState.IsValid)
            //{
            if (courrier != null)
            {
                courrier.ModifyBy = 1;
                courrier.ModifyDate = DateTime.Now;
                db.Entry(courrier).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }
            //}
            return View("CourrierList");
        }
        [HttpPost]
        public ActionResult UpdateEdit(CourrierMaster courrier)
        {
            if (courrier != null)
            {
                courrier.ModifyBy = 1;
                courrier.ModifyDate = DateTime.Now;
                courrier.IsActive = true;
                courrier.StatusId = 1;
                db.Entry(courrier).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }
            return Json("Success", JsonRequestBehavior.AllowGet);
        }
        public ActionResult CourrierDetails(long? id)
        {
            var list = (from CM in db.CourrierMasters
                        join P in db.PartyMasters on CM.PartyId equals P.PartyId
                        join N in db.NetworkMaster on CM.NetworkModeId equals N.NetworkId
                        join C in db.CourrierModes on CM.CourrierModeId equals C.CourrierModeId
                        select new CorrierDetails
                        {
                            Amount = CM.Amount,
                            CNNo = CM.CNNo,
                            Weight = CM.Weight,
                            TrackingNo = CM.TrackingNo,
                            StatusId = CM.StatusId,
                            CourrierId = CM.CourrierId,
                            CreatedBy = CM.CreatedBy,
                            CreatedDt = CM.CreatedDt,
                            BookingDate = "" + CM.DepartureDt,
                            Location = CM.Location,
                            IsActive = CM.IsActive,
                            ModifyBy = CM.ModifyBy,
                            ModifyDate = CM.ModifyDate,
                            PartyId = CM.PartyId,
                            PartyName = P.PartyName,
                            NetworkName = N.NetworkName,
                            CourierMode =C.CourrierModeName,
                            ODACharges = CM.ODACharges,
                            DiscountAmount = CM.Discount
                        }).ToList();
            return View(list[0]);
        }
        public ActionResult ActiveDeative(long? id)
        {
            if (id > 0)
            {
                SqlConnection connString = new SqlConnection(db.Database.Connection.ConnectionString);
                if (connString.State == ConnectionState.Closed)
                    connString.Open();
                SqlCommand cmd = new SqlCommand("uspDeleteCourrierDetails", connString);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@CourrierId", id);
                int a = cmd.ExecuteNonQuery();
            }
            return RedirectToAction("CourrierList");
        }

        [HttpPost]
        public JsonResult SaveBillDetails(long partyId, string fromDate, string toDate, decimal grandTotal, decimal TotalAmount, decimal fullCharges, decimal CGST, decimal SGST, int BillId)
        {
            SqlConnection connString = new SqlConnection(db.Database.Connection.ConnectionString);
            if (connString.State == ConnectionState.Closed)
                connString.Open();
            SqlCommand cmd = new SqlCommand("uspInsertBillDetails", connString);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@BillId", BillId);
            cmd.Parameters.AddWithValue("@PartyId", partyId);
            cmd.Parameters.AddWithValue("@PeriodFrom", fromDate);
            cmd.Parameters.AddWithValue("@PeriodTo", toDate);
            cmd.Parameters.AddWithValue("@TotalAmount", TotalAmount);
            cmd.Parameters.AddWithValue("@FullCharges", fullCharges);
            cmd.Parameters.AddWithValue("@CGST", CGST);
            cmd.Parameters.AddWithValue("@SGST", SGST);
            cmd.Parameters.AddWithValue("@GrandTotal", grandTotal);
            cmd.Parameters.AddWithValue("@UserId", int.Parse("0" + Session["UserId"]));
            cmd.Parameters.Add("@GeneratedBillId", SqlDbType.VarChar, 50);
            cmd.Parameters["@GeneratedBillId"].Direction = ParameterDirection.Output;
            cmd.Parameters.Add("@BillDate", SqlDbType.Date);
            cmd.Parameters["@BillDate"].Direction = ParameterDirection.Output;
            cmd.Parameters.Add("@FuelCharges", SqlDbType.Decimal);
            cmd.Parameters["@FuelCharges"].Direction = ParameterDirection.Output;

            cmd.ExecuteNonQuery();
            string billId = Convert.ToString(cmd.Parameters["@GeneratedBillId"].Value);
            string fuelCharges = Convert.ToString(cmd.Parameters["@FuelCharges"].Value);
            DateTime date = Convert.ToDateTime(cmd.Parameters["@BillDate"].Value);
            var v = new { BillId = billId, CurrentDate = date.ToString("dd-MMM-yyyy"), FuelCharges = fuelCharges };
            return Json(v, JsonRequestBehavior.AllowGet);
        }
        public List<CorrierDetails> GetCourierList(ref TotalDetails dtTotal, ref PartyAddressDetails dtPartydetails, long? partyId, string trackingNo, string fromDate, string toDate)
        {
            DataSet ds = new DataSet();
            List<CorrierDetails> list = new List<CorrierDetails>();
            try
            {
                SqlConnection connString = new SqlConnection(db.Database.Connection.ConnectionString);
                if (connString.State == ConnectionState.Closed)
                    connString.Open();
                SqlCommand cmd = new SqlCommand("uspSearchCourier", connString);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@PartyId", partyId);
                cmd.Parameters.AddWithValue("@CourrierId", 0);
                cmd.Parameters.AddWithValue("@UserId", 0);
                cmd.Parameters.AddWithValue("@TrackingNo", trackingNo);
                cmd.Parameters.AddWithValue("@FromDate", fromDate);
                cmd.Parameters.AddWithValue("@ToDate", toDate);
                cmd.Parameters.AddWithValue("@START_INDEX", 1);
                cmd.Parameters.AddWithValue("@MAX_ROWS", 1000000);
                cmd.Parameters.AddWithValue("@SORT_EXPRESSION", string.Empty);
                cmd.Parameters.AddWithValue("@SORT_DIRECTION", string.Empty);
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                ds = new DataSet();
                sda.Fill(ds);
                sda.Dispose();
                connString.Close();

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    CorrierDetails courier = new CorrierDetails();
                    courier.Amount = decimal.Parse("0" + dr["Amount"]);
                    courier.CNNo = Convert.ToString(dr["CNNo"]);
                    courier.Weight = decimal.Parse("0" + dr["Weight"]);
                    courier.TrackingNo = Convert.ToString(dr["TrackingNo"]);
                    courier.StatusId = int.Parse("0" + dr["StatusId"]);
                    courier.CourrierId = int.Parse("0" + dr["CourrierId"]);
                    courier.CreatedBy = int.Parse("0" + dr["CreatedBy"]);
                    courier.CreatedDt = DateTime.Parse("" + dr["CreatedDt"]);
                    courier.BookingDate = Convert.ToString(dr["BookingDate"]);
                    courier.Location = Convert.ToString(dr["Location"]);
                    courier.NetworkName = Convert.ToString(dr["NetworkName"]);
                    courier.IsActive = bool.Parse("" + dr["IsActive"]);
                    courier.ModifyBy = int.Parse("0" + dr["ModifyBy"]);
                    courier.Rate = decimal.Parse("0" + dr["Rate"]);
                    courier.PartyId = int.Parse("0" + dr["PartyId"]);
                    courier.CourrierModeId = int.Parse("0" + dr["CourrierModeId"]);
                    courier.NetworkModeId = int.Parse("0" + dr["NetworkModeId"]);
                    courier.PartyName = Convert.ToString(dr["PartyName"]);
                    courier.CourierMode = Convert.ToString(dr["CourierMode"]);
                    courier.CalculatedAmount = decimal.Parse("0" + dr["CalculatedAmount"]);
                    courier.ODACharges = decimal.Parse("0" + dr["ODACharges"]);
                    courier.DiscountAmount = decimal.Parse("0" + dr["DiscountAmount"]);
                    list.Add(courier);
                }
                if (ds.Tables.Count > 1 && ds.Tables[1].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[1].Rows)
                    {
                        dtTotal.TotalAmount = decimal.Parse("0" + dr["TotalAmount"]);
                        dtTotal.RecordCount = int.Parse("0" + dr["RecordCount"]);
                        dtTotal.CGST = decimal.Parse("0" + dr["CGST"]);
                        dtTotal.SGST = decimal.Parse("0" + dr["SGST"]);
                        dtTotal.FullCharges = decimal.Parse("0" + dr["Full Charges"]);

                        dtTotal.GrandTotal = dtTotal.TotalAmount + dtTotal.CGST + dtTotal.SGST + dtTotal.FullCharges;
                    }
                }
                if (ds.Tables.Count > 2 && ds.Tables[2].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[2].Rows)
                    {
                        dtPartydetails.PartyId = int.Parse("0" + dr["PartyId"]);
                        dtPartydetails.PartyName = Convert.ToString(dr["PartyName"]);
                        dtPartydetails.Address = Convert.ToString(dr["Address"]);
                        dtPartydetails.GSTNumber = Convert.ToString(dr["GSTNumber"]);
                        dtTotal.FuelChargesLabel = Convert.ToString(dr["FuelCharges"]);
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return list;
        }
        protected string RenderPartialViewToString(string viewName, object model)
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
        public static List<List<CorrierDetails>> SplitList(List<CorrierDetails> locations, int nSize)
        {
            List<List<CorrierDetails>> list = new List<List<CorrierDetails>>();
            for (int i = 0; i < locations.Count; i += nSize)
            {
                list.Add(locations.GetRange(i, Math.Min(nSize, locations.Count - i)));
            }
            return list;
        }
        public ActionResult GetUpdatedetails(long courrierId)
        {
            UpdateCourrier courrierUpdate = new UpdateCourrier();
            try
            {
                courrierUpdate.PartyList = db.PartyMasters.Where(x => x.IsActive == true).OrderBy(x => x.PartyName).ToList();
                courrierUpdate.Courrier = db.CourrierMasters.Where(c => c.CourrierId == courrierId).FirstOrDefault();
            }
            catch (Exception ex)
            {

            }
            return Json(courrierUpdate, JsonRequestBehavior.AllowGet);
        }
        //public JsonResult SearchJsonData(long? partyId, string trackingNo, string fromDate, string toDate)
        //{
        //    int count = 0;
        //    List<CorrierDetails> list = GetCourierList(ref count, partyId, trackingNo, fromDate.Replace("/", ""), toDate.Replace("/", ""));
        //    return Json(list, JsonRequestBehavior.AllowGet);
        //}
        public ActionResult ResultExportToExcel()
        {
            var grdReport = new System.Web.UI.WebControls.GridView();
            grdReport.DataSource = db.Database.SqlQuery<DailyCourierManifesto>("exec uspDailyCourierManiFesto").ToList();
            grdReport.DataBind();
            System.IO.StringWriter sw = new System.IO.StringWriter();
            System.Web.UI.HtmlTextWriter htw = new System.Web.UI.HtmlTextWriter(sw);
            grdReport.RenderControl(htw);
            byte[] bindData=System.Text.Encoding.ASCII.GetBytes(sw.ToString());
            return File(bindData, "application/ms-excel","DailyManifesto"+DateTime.Now+".xls");
        }
        [HttpGet]
        public JsonResult GetLoggedInUser()
        {
            long userId = (long)Session["UserId"];
            var loggedInUser = db.AdminUsers.Where(admin => admin.UserId == userId).FirstOrDefault();
            return Json(new { loggedInUser = loggedInUser }, JsonRequestBehavior.AllowGet);
        }
    }

    public class UpdateCourrier
    {
        public List<PartyMaster> PartyList { get; set; }
        public CourrierMaster Courrier { get; set; }
    }
    public class PartyAddressDetails
    {
        public int PartyId { get; set; }
        public string PartyName { get; set; }
        public string Address { get; set; }
        public string GSTNumber { get; set; }
    }
    public class CorierResponse
    {
        public string TrackingNo { get; set; }
        public long PartyId { get; set; }
        public string Message { get; set; }
        public int Status { get; set; }
    }
    public class TotalDetails
    {
        public decimal TotalAmount { get; set; }
        public int RecordCount { get; set; }
        public decimal CGST { get; set; }
        public decimal SGST { get; set; }
        public decimal FullCharges { get; set; }
        public decimal GrandTotal { get; set; }
        public string FuelChargesLabel { get; set; }
    }
}