using InTimeCourier.App_Start;
using InTimeCourier.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace InTimeCourier.Controllers
{
    [SessionAuthorize]
    public class SourceController : Controller
    {
        InLogisticModel db = new InLogisticModel();
        public string ValidationMessage()
        {
            string errMsg = string.Empty;
            foreach (var state in ModelState)
            {
                if (state.Value.Errors.Count > 0)
                    errMsg += state.Value.Errors.FirstOrDefault().ErrorMessage + "\n";
            }
            return errMsg;
        }
        // GET: Source
        public ActionResult Source()
        {
            var list = db.SourceMasters.OrderBy(x => x.SourceId).ToList();
            return View(list);
        }

        public ActionResult AddSource()
        {
            ViewBag.State = new SelectList(db.StateMasters.Where(x => x.CountryId == 1).OrderBy(x => x.StateName), "StateId", "StateName");
            return View();
        }
        [HttpPost]
        public ActionResult AddSource(SourceMaster source)
        {
            ViewBag.State = new SelectList(db.StateMasters.Where(x => x.CountryId == 1).OrderBy(x => x.StateName), "StateId", "StateName");
            var list = db.SourceMasters.Where(x => x.SourceName == source.SourceName && x.StateId == source.StateId).ToList();
            if (ModelState.IsValid)
            {
                if (list.Count > 0)
                    ViewBag.ErrorMsg = "This source alredy exists";
                else
                {
                    source.IsActive = true;
                    source.CreatedBy = 1;
                    source.CreatedDt = DateTime.Now;
                    db.SourceMasters.Add(source);
                    db.SaveChanges();
                    ViewBag.SuccessMsg = "Your new source added successfully";
                }
            }
            else
                ViewBag.ErrorMsg = "Please fill all mandatory fields";

            return View();
        }
        [HttpGet]
        public ActionResult Edit(long? id)
        {
            ViewBag.State = new SelectList(db.StateMasters.Where(x => x.CountryId == 1).OrderBy(x => x.StateName), "StateId", "StateName");
            var list = db.SourceMasters.Where(x => x.SourceId == id).ToList();
            return View(list[0]);
        }
        [HttpPost]
        public ActionResult Edit(SourceMaster source)
        {
            ViewBag.State = new SelectList(db.StateMasters.Where(x => x.CountryId == 1).OrderBy(x => x.StateName), "StateId", "StateName");
            if (ModelState.IsValid)
            {
                if (source != null)
                {
                    source.ModifyBy = 1;
                    source.ModifyDt = DateTime.Now;
                    db.Entry(source).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                    ViewBag.SuccessMsg = "Your source modification has been done successfully!!!";
                }
                else
                    ViewBag.ErrorMsg = "Some error occurred. Please try again later.";
            }
            else ViewBag.ErrorMsg = "Some error occurred. Please try again later.";
            return RedirectToAction("Source");
        }
        public ActionResult AcivateDeativate(long? id)
        {
            if (id > 0)
            {
                SqlConnection connString = new SqlConnection(db.Database.Connection.ConnectionString);
                if (connString.State == ConnectionState.Closed)
                    connString.Open();
                SqlCommand cmd = new SqlCommand("uspDeleteSourceMaster", connString);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SourceId", id);
                int a = cmd.ExecuteNonQuery();
            }
            return RedirectToAction("Source");
        }

        // Add rate mapping with party and source
        public ActionResult MapRateSourceAndParty(int? Id)
        {
            try
            {
                //ViewBag.Source = new SelectList(db.SourceMasters.Where(x => x.IsActive == true).OrderBy(x => x.SourceName), "SourceId", "SourceName");
                ViewBag.CourrierMode = new SelectList(db.CourrierModes.Where(x => x.IsActive == true).OrderBy(x => x.CourrierModeId), "CourrierModeId", "CourrierModeName");
                ViewBag.Party = new SelectList(db.PartyMasters.Where(x => x.IsActive == true).OrderBy(x => x.PartyName), "PartyId", "PartyName");
                if(Id > 0)
                {
                    RateMapping objratemapping = new RateMapping();
                    objratemapping = db.RateMapping.Where(x => x.Id == Id).FirstOrDefault();
                    return View(objratemapping);
                }
            }
            catch (Exception ex)
            {

            }
            return View();
        }
      
        [HttpPost]
        // Add rate mapping with party and source
        public ActionResult MapRateSourceAndParty(RateMapping mapping)
        {
            try
            {
                DataSet ds = new DataSet();
                string errMsg = ValidationMessage();
                //ViewBag.Source = new SelectList(db.SourceMasters.Where(x => x.IsActive == true).OrderBy(x => x.SourceName), "SourceId", "SourceName");
                ViewBag.CourrierMode = new SelectList(db.CourrierModes.Where(x => x.IsActive == true).OrderBy(x => x.CourrierModeId), "CourrierModeId", "CourrierModeName");
                ViewBag.Party = new SelectList(db.PartyMasters.Where(x => x.IsActive == true).OrderBy(x => x.PartyName), "PartyId", "PartyName");
                var checkexists = db.RateMapping.Where(x => x.PartyId == mapping.PartyId && x.ModeId==mapping.ModeId).FirstOrDefault();
                if(checkexists != null && mapping.Id==0 && (checkexists.FromWt==mapping.FromWt || checkexists.ToWt==mapping.ToWt))
                {
                    ViewBag.ErrorMsg = "This Party and Mode Rate Already Exists";
                    return View();
                }
                else
                {
                    var checkidexists = db.RateMapping.Where(x => x.PartyId == mapping.PartyId && x.ModeId == mapping.ModeId && x.Id!=mapping.Id && (x.FromWt == mapping.FromWt || x.ToWt == mapping.ToWt)).FirstOrDefault();
                    if(checkidexists!=null)
                    {
                        ViewBag.ErrorMsg = "This Party and Mode Rate Already Exists";
                        return View();
                    }
                    SqlConnection connString = new SqlConnection(db.Database.Connection.ConnectionString);
                    if (connString.State == ConnectionState.Closed)
                        connString.Open();
                    SqlCommand cmd = new SqlCommand("uspSaveRateMapping", connString);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Id", mapping.Id);
                    cmd.Parameters.AddWithValue("@PartyId", mapping.PartyId);
                    cmd.Parameters.AddWithValue("@ModeId", mapping.ModeId);
                    cmd.Parameters.AddWithValue("@NetworkModeId", mapping.NetworkModeId);
                    cmd.Parameters.AddWithValue("@FromWt", mapping.FromWt);
                    cmd.Parameters.AddWithValue("@ToWt", mapping.ToWt);
                    cmd.Parameters.AddWithValue("@Rate", mapping.Rate);
                    SqlDataAdapter sda = new SqlDataAdapter(cmd);
                    sda.Fill(ds);
                    if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            if (int.Parse("0" + dr["StatusCode"]) == 0)
                            {
                                ViewBag.ErrorMsg = Convert.ToString(dr["Message"]);
                            }
                            else
                            {
                                ViewBag.SuccessMsg = Convert.ToString(dr["Message"]);
                            }
                        }
                    }
                }

               
            }
            catch (Exception ex) { }
            return View();
        }

        [HttpGet]
        public JsonResult FetchRateMapping(string modeId,string networkModeId,string partyId)
        {
            try
            {
                string rate = string.Empty;
                DataSet ds = new DataSet();
                SqlConnection connString = new SqlConnection(db.Database.Connection.ConnectionString);
                if (connString.State == ConnectionState.Closed)
                    connString.Open();
                SqlCommand cmd = new SqlCommand("uspGetRateDetails", connString);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@PartyId", partyId);
                cmd.Parameters.AddWithValue("@ModeId", modeId);
                cmd.Parameters.AddWithValue("@NetworkModeId", networkModeId);
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                sda.Fill(ds);
                //List< RateMappingData> RateMappingDatalst=new List<RateMappingData>();
                //foreach (DataTable table in ds.Tables)
                //{
                //    foreach (DataRow dr in table.Rows)
                //    {
                //        RateMappingData ratedetails = new RateMappingData
                //        {
                //            FromWt = Convert.ToDecimal(dr["FromWt"]),
                //            ToWt = Convert.ToDecimal(dr["ToWt"]),
                //            Rate = Convert.ToDecimal(dr["Rate"])
                //        };
                //        RateMappingDatalst.Add(ratedetails);
                //    }
                //}
                //if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                //{
                //    foreach (DataRow dr in ds.Tables[0].Rows)
                //    {
                //        rate = Convert.ToString(dr["Rate"]);
                //    }
                //}
                return Json(JsonConvert.SerializeObject(ds), JsonRequestBehavior.AllowGet);
                //return Json(new { RateMappingDatalst }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { rate = "", status = false }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public ActionResult GetRateMappingList()
        {
            ViewBag.Party = new SelectList(db.PartyMasters.Where(x => x.IsActive == true).OrderBy(x => x.PartyName).ToList(), "PartyId", "PartyName");
            //ViewBag.SourceMode = new SelectList(db.SourceMasters.Where(x => x.IsActive == true).OrderBy(x => x.SourceName).ToList(), "SourceId", "SourceName");
            ViewBag.CourrierMode = new SelectList(db.CourrierModes.Where(x => x.IsActive == true).OrderBy(x => x.CourrierModeName).ToList(), "CourrierModeId", "CourrierModeName");
            try
            {
                return View();
            }
            catch (Exception Ex)
            {
                return View();
            }
        }

        [HttpGet]
        public JsonResult GetRateMappingData(string modeId, string partyId)
        {
            //ViewBag.Party = new SelectList(db.PartyMasters.Where(x => x.IsActive == true).OrderBy(x => x.PartyName).ToList(), "PartyId", "PartyName");
            List<RateMappingDetails> rateMappingList = new List<RateMappingDetails>();
            try
            {
                DataSet ds = new DataSet();
                SqlConnection connString = new SqlConnection(db.Database.Connection.ConnectionString);
                if (connString.State == ConnectionState.Closed)
                    connString.Open();
                SqlCommand cmd = new SqlCommand("uspSelectRateDetails", connString);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@PartyId", partyId==""?null:partyId);
                cmd.Parameters.AddWithValue("@ModeId", modeId==""?null:modeId);
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                sda.Fill(ds);
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        RateMappingDetails ratedetails = new RateMappingDetails();
                        ratedetails.Rate = Convert.ToDecimal(dr["Rate"]);
                        ratedetails.FromWt = Convert.ToDecimal(dr["FromWt"]);
                        ratedetails.ToWt = Convert.ToDecimal(dr["ToWt"]);
                        ratedetails.PartyName = Convert.ToString(dr["PartyName"]);
                        //ratedetails.PartyId = Convert.ToInt32(dr["PartyId"]);
                        ratedetails.CourrierModeName = Convert.ToString(dr["CourrierModeName"]);
                        //ratedetails.CourrierModeId = Convert.ToInt32(dr["CourrierModeId"]);
                        ratedetails.NetworkModeName = Convert.ToString(dr["NetworkModeName"]);
                        ratedetails.Id = Convert.ToInt32(dr["Id"]);
                        rateMappingList.Add(ratedetails);
                    }
                }
                return Json(rateMappingList, JsonRequestBehavior.AllowGet);
            }
            catch (Exception Ex)
            {
                return Json(rateMappingList, JsonRequestBehavior.AllowGet);
            }
        }


        [HttpGet]
        public JsonResult FilteredGetRateMappingData(int? modeId,int? partyId)
        {
            ViewBag.Party = new SelectList(db.PartyMasters.Where(x => x.IsActive == true).OrderBy(x => x.PartyName).ToList(), "PartyId", "PartyName");
            List<RateMappingDetails> rateMappingList = new List<RateMappingDetails>();
            try
            {
                DataSet ds = new DataSet();
                SqlConnection connString = new SqlConnection(db.Database.Connection.ConnectionString);
                if (connString.State == ConnectionState.Closed)
                    connString.Open();
                SqlCommand cmd = new SqlCommand("uspSelectRateDetails", connString);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                sda.Fill(ds);
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        RateMappingDetails ratedetails = new RateMappingDetails();
                        ratedetails.Rate = decimal.Parse("0" + dr["Rate"]);
                        ratedetails.FromWt = decimal.Parse("0" + dr["FromWt"]);
                        ratedetails.ToWt = decimal.Parse("0" + dr["ToWt"]);
                        ratedetails.PartyName = Convert.ToString(dr["PartyName"]);
                        ratedetails.PartyId = int.Parse("0" + dr["PartyId"]);
                        ratedetails.CourrierModeName = Convert.ToString(dr["CourrierModeName"]);
                        ratedetails.CourrierModeId = int.Parse("0" + dr["CourrierModeId"]);
                        ratedetails.Id = int.Parse("0" + dr["Id"]);
                        rateMappingList.Add(ratedetails);
                    }
                }
                return Json(rateMappingList, JsonRequestBehavior.AllowGet);
            }
            catch (Exception Ex)
            {
                return Json(rateMappingList, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult RateMapping()
        {
            return View();
        }
    }
    public class RateMappingData
    {
        public decimal FromWt { get; set; }
        public decimal ToWt { get; set; }
        public decimal Rate { get; set; }
    }
}