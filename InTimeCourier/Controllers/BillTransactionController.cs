using InTimeCourier.App_Start;
using InTimeCourier.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web.Mvc;

namespace InTimeCourier.Controllers
{
    [SessionAuthorize]
    public class BillTransactionController : Controller
    {
        InLogisticModel db = new InLogisticModel();
        // GET: BillTransaction
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult BillTransactionList()
        {
            return View();
        }

        public ActionResult AddBillTransaction()
        {
            return View();
        }
        public ActionResult SearchBillTransaction()
        {
            return View();
        }
        public JsonResult GetSearchBillTransactionResult(int partyId,string fYear)
        {
            DapperClass objDb = new DapperClass();
            string sprcname = "uspSelectBillTransactionList";
            object Paramenters = new
            {
                PartyId = partyId,
                FYear = fYear
            };
            var response = objDb.Exec_SPrc<BillTransaction>(sprcname, Paramenters, "InLogisticModel", "primelogisticdb", true);

            var v = RenderPartialViewToString("BillTransactionList", response);
            return Json(v,JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetAllPartyAndFinancialYear()
        {
            var partyList = db.PartyMasters.Where(x=>x.IsActive==true).OrderBy(x=>x.PartyName).ToList();
            var fYearList = db.FinancialYears.Where(x=>x.IsActive==true).OrderBy(x => x.FYear).ToList();
            return Json(new {parties=partyList,financialYears=fYearList}, JsonRequestBehavior.AllowGet);
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

    }
}