using InTimeCourier.App_Start;
using InTimeCourier.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace InTimeCourier.Controllers
{
    [SessionAuthorize]
    public class BillStatementController : Controller
    {
        SqlConnection connString = new SqlConnection(ConfigurationManager.ConnectionStrings["InLogisticModel"].ToString());
        InLogisticModel db = new InLogisticModel();
        // GET: BillStatement
        public ActionResult Index()
        {
            ViewBag.Party = new SelectList(db.PartyMasters.Where(x => x.IsActive == true)
                .OrderBy(x => x.PartyName).ToList(), "PartyId", "PartyName");
            return View();
        }

        public IEnumerable<BillDetails> GetBillListDapper(BillDetails obj, int type)
        {
            DapperClass objDb = new DapperClass();
            string sprcname = "uspSelectBillStatement";
            object Paramenters = new
            {
                PartyId = obj.PartyId,
                SearchTerm = String.IsNullOrEmpty(obj.SearchTerm) ? "" : obj.SearchTerm,
                BillDate = String.IsNullOrEmpty(obj.BillDate) ? "" : obj.BillDate,
                FromDate = String.IsNullOrEmpty(obj.PeriodFrom) ? "" : obj.PeriodFrom,
                ToDate = String.IsNullOrEmpty(obj.PeriodTo) ? "" : obj.PeriodTo,
                BillId = obj.BillId,
                LoadType=type
            };
            return objDb.Exec_SPrc<BillDetails>(sprcname, Paramenters, "InLogisticModel", "intimelogisticjeetu", true);
        }

        [HttpPost]
        public JsonResult SearchStatement()
        {
            BillDetails obj = new BillDetails();
            JsonResult result = new JsonResult();
            try
            {
                var draw = Request.Form.GetValues("draw").FirstOrDefault();
                var start = Request.Form.GetValues("start").FirstOrDefault();
                var length = Request.Form.GetValues("length").FirstOrDefault();
                var sortColumn = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault()
                                        + "][name]").FirstOrDefault();
                var sortColumnDir = Request.Form.GetValues("order[0][dir]").FirstOrDefault();
                var partyId = Request.Form.GetValues("columns[4][search][value]").FirstOrDefault();
                var fromDate = Request.Form.GetValues("columns[5][search][value]").FirstOrDefault();
                var toDate = Request.Form.GetValues("columns[6][search][value]").FirstOrDefault();
                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt16(start) : 0;
                obj.PeriodFrom = fromDate;
                obj.PeriodTo = toDate;
                obj.PartyId = int.Parse("0" + partyId);
                int recordsTotal = 0;


                var v = GetBillListDapper(obj, partyId == ""?0:1);

                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
                {
                    v = v.OrderBy(a=> sortColumn + " " + sortColumnDir);
                }
                recordsTotal = v.Count();
                var data = v.Skip(skip).Take(pageSize).ToList();
                return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data },
                    JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                // Info     
                Console.Write(ex);
            }
            // Return info.     
            return result;
        }
    }
}