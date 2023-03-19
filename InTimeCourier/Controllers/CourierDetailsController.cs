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
using Newtonsoft.Json;
using Microsoft.Ajax.Utilities;

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
            //objCorrierDetails.CorrierDetails2 = new List<Models.CorrierDetails>();
            //for (int i = 1; i <= list.Count; i++)
            //{
            //    if (i % 2 == 0)
            //    {
            //        objCorrierDetails.CorrierDetails2.Add(list[i - 1]);
            //    }
            //    else
            //    {
            //        objCorrierDetails.CorrierDetails1.Add(list[i - 1]);
            //    }
            //}
            foreach(var item in list)
            {
                objCorrierDetails.CorrierDetails1.Add(item);
            }
            var html = RenderPartialViewToString("SearchList", objCorrierDetails);
            string totalInWord = ConvertInWord.ConvertToWords(dtTotal.GrandTotal.ToString());
            var v = new { html = html, TotalRecord = dtTotal, PartyDetails = dtPartyDetails, InWord = totalInWord };
            return this.Json(v, JsonRequestBehavior.AllowGet);
        }
        public ActionResult SearchCorier()
        {
            //ViewBag.Party = new SelectList(db.PartyMasters.Where(x => x.IsActive == true).OrderBy(x => x.PartyName).ToList(), "PartyId", "PartyName");
            ViewBag.Party = new SelectList(db.PartyMasters.Where(x => x.IsActive == true).Select(item => new { PartyId = item.PartyId, PartyName = item.PartyType == null ? item.PartyName + item.PartyType : item.PartyName + " : " + item.PartyType }).OrderBy(x => x.PartyName).ToList(), "PartyId", "PartyName");
            return View();
        }
        public ActionResult AddCourrier()
        {
            try
            {
                if(!string.IsNullOrEmpty(Request.QueryString["success"]) && !string.IsNullOrEmpty(Request.QueryString["status"]) || !string.IsNullOrEmpty(Request.QueryString["trackingNo"]))
                {
                    ViewBag.Message = Request.QueryString["success"];
                    ViewBag.Status = int.Parse("0"+Request.QueryString["status"]);
                    ViewBag.TrackingNo = Request.QueryString["trackingNo"];
                }
                if (!string.IsNullOrEmpty(Request.QueryString["dateLock"]))
                {
                    ViewBag.DateLock = Request.QueryString["dateLock"];
                    ViewBag.LockDate= Request.QueryString["lockdate"];
                }

               //ViewBag.Location = new SelectList(db.SourceMasters.Where(x => x.IsActive == true).OrderBy(x => x.SourceId).ToList(), "SourceId", "SourceName");
                ViewBag.Party = new SelectList(db.PartyMasters.Where(x => x.IsActive == true).Select(item=>new {PartyId=item.PartyId,PartyName=item.PartyType==null? item.PartyName + item.PartyType  : item.PartyName + " : " + item.PartyType }).OrderBy(x => x.PartyName).ToList(), "PartyId", "PartyName");
                var data = db.CourrierModes.Where(x => x.IsActive == true).OrderBy(x => x.CourrierModeId);
                ViewBag.CourrierMode = new SelectList(db.CourrierModes.Where(x => x.IsActive == true).OrderBy(x => x.CourrierModeName), "CourrierModeId", "CourrierModeName");
                ViewBag.Networks = new SelectList(db.NetworkMaster.Where(x => x.IsActive == true).Select(item => new { NetworkModeId = item.NetworkId, NetworkName = item.NetworkName }).OrderBy(x => x.NetworkName), "NetworkModeId", "NetworkName");
                ViewBag.DestinationList = new SelectList(db.DestinationMaster.Where(x => x.IsActive == true).Select(item => new { DestinationId = item.Id, Name = item.Name }).OrderBy(x => x.Name), "DestinationId", "Name");
                var list = db.CourrierMasters.Where(x=>x.IsActive==true).OrderByDescending(x => x.CourrierId).ToList();
                var listnew=list.Take(12).ToList();
                int cnt = 0;
                foreach (var item in listnew)
                {
                    var Location= db.DestinationMaster.Where(x => x.IsActive == true && x.Id==item.DestinationId).FirstOrDefault();
                    if (Location!=null)
                    {
                        list[cnt].Location = Location.Name;
                        list[cnt].TrackingNo = item.DepartureDt.ToString("dd-MMM-yyyy");
                    }
                    cnt++;
                }
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
            ViewBag.CourrierMode = new SelectList(db.CourrierModes.Where(x => x.IsActive == true).OrderBy(x => x.CourrierModeName), "CourrierModeId", "CourrierModeName");
            ViewBag.Location = new SelectList(db.SourceMasters.Where(x => x.IsActive == true).OrderBy(x => x.SourceId).ToList(), "SourceId", "SourceName");
            ViewBag.Party = new SelectList(db.PartyMasters.Where(x => x.IsActive == true).OrderBy(x => x.PartyName).ToList(), "PartyId", "PartyName");
            ViewBag.Networks = new SelectList(db.NetworkMaster.Where(x => x.IsActive == true).Select(item => new { NetworkModeId = item.NetworkId, NetworkName = item.NetworkName }).OrderBy(x => x.NetworkName), "NetworkModeId", "NetworkName");
            ViewBag.DestinationList = new SelectList(db.DestinationMaster.Where(x => x.IsActive == true).Select(item => new { DestinationId = item.Id, Name = item.Name }).OrderBy(x => x.Name), "DestinationId", "Name");
            try
            {
                if(courier != null && courier.CourrierId>0)
                {
                    if (courier.DestinationId == 0)
                    {
                        //Add Destination if not available
                        DestinationMaster destinationMaster = new DestinationMaster();
                        destinationMaster.Name = courier.Location;
                        destinationMaster.Description = "Added via AWB Entry";
                        destinationMaster.IsActive = true;
                        destinationMaster.CreatedDate = DateTime.Now;
                        destinationMaster.CreatedBy = int.Parse("0" + Session["UserId"]);
                        db.Entry(destinationMaster).State = System.Data.Entity.EntityState.Added;
                        db.SaveChanges();
                        courier.DestinationId = db.DestinationMaster.OrderByDescending(x => x.Id).Select(x => x.Id).FirstOrDefault();

                    }
                    decimal? fuelcharges = db.PartyMasters.Where(sa => sa.PartyId == courier.PartyId).Select(sa => sa.FuelCharges).FirstOrDefault();
                    decimal? finalfuelcharges = 0;
                    if (fuelcharges != null)
                    {
                        finalfuelcharges = (courier.Amount * fuelcharges) / 100;
                    }
                    else if (fuelcharges == Convert.ToDecimal(0.00))
                    {
                        finalfuelcharges = 0;
                    }

                    courier.FuelCharges = finalfuelcharges;
                    courier.ModifyBy = int.Parse("0" + Session["UserId"]);
                        courier.ModifyDate = DateTime.Now;
                        courier.IsActive = true;
                        courier.Distance = db.PartyMasters.Where(sa => sa.PartyId == courier.PartyId).Select(sa => sa.PartyName).FirstOrDefault();
                        db.Entry(courier).State = System.Data.Entity.EntityState.Modified;
                        db.SaveChanges();
                        ViewBag.Message = "Your Courier Updated Successfully"; //"Your courier registered successfully with courier tracking no: " + response[0].TrackingNo;
                        ViewBag.Status = 1;
                }
                else
                {
                    //Add Destination if not available
                    if(courier.DestinationId==0)
                    {
                        DestinationMaster destinationMaster = new DestinationMaster();
                        destinationMaster.Name = courier.Location;
                        destinationMaster.Description = "Added via AWB Entry";
                        destinationMaster.IsActive = true;
                        destinationMaster.CreatedDate = DateTime.Now;
                        destinationMaster.CreatedBy = int.Parse("0" + Session["UserId"]);
                        db.Entry(destinationMaster).State = System.Data.Entity.EntityState.Added;
                        db.SaveChanges();
                        courier.DestinationId=db.DestinationMaster.OrderByDescending(x => x.Id).Select(x=>x.Id).FirstOrDefault();

                    }

                    courier.Distance = db.PartyMasters.Where(sa => sa.PartyId == courier.PartyId).Select(sa => sa.PartyName).FirstOrDefault();
                    var response = db.Database.SqlQuery<CorierResponse>("exec uspInsertCourrierDetails @PartyId,@Amount,@CreatedBy,@TrackingNo,@CNNo,@Weight,@DepartureDt,@Rate,@DestinationId,@CourrierModeId,@ODACharges,@NetworModeId,@Discount,@Qty,@PartyName",
                                    new SqlParameter("@PartyId", courier.PartyId),
                                    new SqlParameter("@CourrierModeId", courier.CourrierModeId),
                                    new SqlParameter("@NetworModeId", courier.NetworkModeId),
                                    new SqlParameter("@ODACharges", courier.ODACharges ?? (object)DBNull.Value),
                                    new SqlParameter("@Qty", courier.Qty),
                                    new SqlParameter("@Amount", courier.Amount),
                                    new SqlParameter("@CreatedBy", CourierHelper.UserId),
                                    new SqlParameter("@TrackingNo", string.Empty),
                                    new SqlParameter("@CNNo", courier.CNNo),
                                    new SqlParameter("@Weight", courier.Weight),
                                    new SqlParameter("@Discount", courier.Discount ?? (object)DBNull.Value),
                                    new SqlParameter("@DepartureDt", courier.DepartureDt),
                                    new SqlParameter("@DestinationId", courier.DestinationId),
                                    new SqlParameter("@PartyName", courier.Distance),
                                    new SqlParameter("@Rate", courier.Rate)).ToList();
                    ViewBag.Message = response[0].Message; //"Your courier registered successfully with courier tracking no: " + response[0].TrackingNo;
                    ViewBag.Status = response[0].Status;
                    courier.TrackingNo = response[0].TrackingNo;
                    ViewBag.TrackingNo = response[0].TrackingNo;
                    ViewBag.DateLock = courier.IsActive;
                    if(courier.IsActive==true)
                    {
                        ViewBag.LockDate = courier.DepartureDt;
                    }

                }

                var list = db.CourrierMasters.Where(x => x.IsActive == true).OrderByDescending(x => x.CourrierId).ToList();
                var listnew = list.Take(12).ToList();
                int cnt = 0;
                foreach (var item in listnew)
                {
                    var Location = db.DestinationMaster.Where(x => x.IsActive == true && x.Id == item.DestinationId).FirstOrDefault();
                    if (Location != null)
                    {
                        list[cnt].Location = Location.Name;
                    }
                    cnt++;
                }
                ViewBag.Courrier = list.Take(10).ToList();
            }
            catch (Exception ex)
            {
                throw;
            }
           // return new EmptyResult(ViewBag);
           // return View(ViewBag);
            return RedirectToAction("AddCourrier",new { success= ViewBag.Message,status= ViewBag.Status,
                trackingNo= ViewBag.TrackingNo,dateLock= ViewBag.DateLock,lockdate=ViewBag.LockDate
            });
        }
        public ActionResult Edit(long? id)
        {
            List<CourrierMaster> courrierList = new List<CourrierMaster>();
            try
            {
                //ViewBag.Location = new SelectList(db.SourceMasters.OrderBy(x => x.SourceId).ToList(), "SourceId", "SourceName");
                ViewBag.CourrierMode = new SelectList(db.CourrierModes.Where(x => x.IsActive == true).OrderBy(x => x.CourrierModeId), "CourrierModeId", "CourrierModeName");
                //ViewBag.Party = new SelectList(db.PartyMasters.OrderBy(x => x.PartyName).ToList(), "PartyId", "PartyName");
                ViewBag.Party = new SelectList(db.PartyMasters.Where(x => x.IsActive == true).Select(item => new { PartyId = item.PartyId, PartyName = item.PartyType == null ? item.PartyName + item.PartyType : item.PartyName + " : " + item.PartyType }).OrderBy(x => x.PartyName).ToList(), "PartyId", "PartyName");
                ViewBag.Networks = new SelectList(db.NetworkMaster.Where(x => x.IsActive == true).Select(item => new { NetworkModeId = item.NetworkId, NetworkName = item.NetworkName }).OrderBy(x => x.NetworkName), "NetworkModeId", "NetworkName");
                ViewBag.DestinationList = new SelectList(db.DestinationMaster.Where(x => x.IsActive == true).Select(item => new { DestinationId = item.Id, Name = item.Name }).OrderBy(x => x.Name), "DestinationId", "Name");
                var list = db.CourrierMasters.Where(x => x.IsActive == true).Where(x => x.CourrierId == id).ToList();
                courrierList = list;
            }
            catch (Exception ex)
            {

            }

            return View(courrierList[0]);
        }

        public ActionResult EditPopup(long? id)
        {
            List<CourrierMaster> courrierList = new List<CourrierMaster>();
            try
            {
                //ViewBag.Location = new SelectList(db.SourceMasters.OrderBy(x => x.SourceId).ToList(), "SourceId", "SourceName");
                ViewBag.CourrierMode = new SelectList(db.CourrierModes.Where(x => x.IsActive == true).OrderBy(x => x.CourrierModeId), "CourrierModeId", "CourrierModeName");
                //ViewBag.Party = new SelectList(db.PartyMasters.OrderBy(x => x.PartyName).ToList(), "PartyId", "PartyName");
                ViewBag.Party = new SelectList(db.PartyMasters.Where(x => x.IsActive == true).Select(item => new { PartyId = item.PartyId, PartyName = item.PartyType == null ? item.PartyName + item.PartyType : item.PartyName + " : " + item.PartyType }).OrderBy(x => x.PartyName).ToList(), "PartyId", "PartyName");
                ViewBag.Networks = new SelectList(db.NetworkMaster.Where(x => x.IsActive == true).Select(item => new { NetworkModeId = item.NetworkId, NetworkName = item.NetworkName }).OrderBy(x => x.NetworkName), "NetworkModeId", "NetworkName");
                ViewBag.DestinationList = new SelectList(db.DestinationMaster.Where(x => x.IsActive == true).Select(item => new { DestinationId = item.Id, Name = item.Name }).OrderBy(x => x.Name), "DestinationId", "Name");
                var list = db.CourrierMasters.Where(x => x.IsActive == true).Where(x => x.CourrierId == id).ToList();
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
                if (courrier.DestinationId == 0)
                {
                    //Add Destination if not available
                    DestinationMaster destinationMaster = new DestinationMaster();
                    destinationMaster.Name = courrier.Location;
                    destinationMaster.Description = "Added via AWB Entry";
                    destinationMaster.IsActive = true;
                    destinationMaster.CreatedDate = DateTime.Now;
                    destinationMaster.CreatedBy = int.Parse("0" + Session["UserId"]);
                    db.Entry(destinationMaster).State = System.Data.Entity.EntityState.Added;
                    db.SaveChanges();
                    courrier.DestinationId = db.DestinationMaster.OrderByDescending(x => x.Id).Select(x => x.Id).FirstOrDefault();

                }
                courrier.ModifyBy = int.Parse("0" + Session["UserId"]);
                courrier.ModifyDate = DateTime.Now;
                courrier.Distance = db.PartyMasters.Where(sa => sa.PartyId == courrier.PartyId).Select(sa => sa.PartyName).FirstOrDefault();
                db.Entry(courrier).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }
            //}
            return View("AddCourrier");
        }
        [HttpPost]
        public ActionResult EditPopup(CourrierMaster courrierMaster)
        {
            //if (ModelState.IsValid)
            //{
            if (courrierMaster != null)
            {
                if (courrierMaster.DestinationId == 0)
                {
                    //Add Destination if not available
                    DestinationMaster destinationMaster = new DestinationMaster();
                    destinationMaster.Name = courrierMaster.Location;
                    destinationMaster.Description = "Added via AWB Entry";
                    destinationMaster.IsActive = true;
                    destinationMaster.CreatedDate = DateTime.Now;
                    destinationMaster.CreatedBy = int.Parse("0" + Session["UserId"]);
                    db.Entry(destinationMaster).State = System.Data.Entity.EntityState.Added;
                    db.SaveChanges();
                    courrierMaster.DestinationId = db.DestinationMaster.OrderByDescending(x => x.Id).Select(x => x.Id).FirstOrDefault();
                }
                decimal? fuelcharges= db.PartyMasters.Where(sa => sa.PartyId == courrierMaster.PartyId).Select(sa => sa.FuelCharges).FirstOrDefault();
                decimal? finalfuelcharges = 0;
                if (fuelcharges!=null)
                {
                    finalfuelcharges = (courrierMaster.Amount * fuelcharges) / 100;
                } 
                else if(fuelcharges==Convert.ToDecimal(0.00))
                {
                    finalfuelcharges = 0;
                }

                courrierMaster.FuelCharges = finalfuelcharges;
                courrierMaster.ModifyBy = int.Parse("0" + Session["UserId"]);
                courrierMaster.ModifyDate = DateTime.Now;
                courrierMaster.Distance = db.PartyMasters.Where(sa => sa.PartyId == courrierMaster.PartyId).Select(sa => sa.PartyName).FirstOrDefault();
                db.Entry(courrierMaster).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }
            //}
            return Json("Success", JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult UpdateEdit(CourrierMaster courrier)
        {
            if (courrier != null)
            {
                courrier.ModifyBy = int.Parse("0" + Session["UserId"]);
                courrier.ModifyDate = DateTime.Now;
                courrier.IsActive = true;
                courrier.StatusId = 1;
                courrier.Distance = db.PartyMasters.Where(sa => sa.PartyId == courrier.PartyId).Select(sa => sa.PartyName).FirstOrDefault();
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
                        join C in db.CourrierModes on CM.CourrierModeId equals C.CourrierModeId where CM.IsActive==true
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
                            CourierMode = C.CourrierModeName,
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
        public JsonResult SaveBillDetails(long partyId, string fromDate, string toDate, decimal grandTotal, 
            decimal TotalAmount, decimal fullCharges, decimal CGST, decimal SGST, string InvoiceNo,int SrNo,string InvoiceDate)
        {
            SqlConnection connString = new SqlConnection(db.Database.Connection.ConnectionString);
            if (connString.State == ConnectionState.Closed)
                connString.Open();
            SqlCommand cmd = new SqlCommand("uspInsertBillDetails", connString);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@InvoiceNo", InvoiceNo);
            cmd.Parameters.AddWithValue("@SrNo", SrNo);
            cmd.Parameters.AddWithValue("@PartyId", partyId);
            cmd.Parameters.AddWithValue("@PeriodFrom", fromDate);
            cmd.Parameters.AddWithValue("@PeriodTo", toDate);
            cmd.Parameters.AddWithValue("@TotalAmount", TotalAmount);
            cmd.Parameters.AddWithValue("@FullCharges", fullCharges);
            cmd.Parameters.AddWithValue("@CGST", CGST);
            cmd.Parameters.AddWithValue("@SGST", SGST);
            cmd.Parameters.AddWithValue("@InvoiceDate", InvoiceDate);
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
                        dtTotal.NetAmount = decimal.Parse("0" + dr["NetAmount"]);
                        dtTotal.Discount = decimal.Parse("0" + dr["Discount"]);
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
                        dtPartydetails.BillId = Convert.ToInt32(dr["BillId"]);
                        dtPartydetails.FuelCharges = Convert.ToString(dr["FuelCharges"]);
                        dtPartydetails.PartyType = Convert.ToString(dr["PartyType"]);
                        dtPartydetails.IsIGST = Convert.ToBoolean(dr["IsIGST"]);
                        if(dtTotal.FuelChargesLabel=="0.00 %")
                        {
                            dtTotal.GrandTotal = dtTotal.TotalAmount + dtTotal.CGST + dtTotal.SGST;
                            var grndttl = dtTotal.GrandTotal.ToString("0.00");
                            var spltgrndttl = grndttl.Split('.');
                            int pr1 = Convert.ToInt32(spltgrndttl[0]);
                            int pr2 = Convert.ToInt32(spltgrndttl[1]);
                            if (pr2 > 0)
                            {
                                dtTotal.GrandTotal = pr1 + 1;
                            }

                        }
                        else
                        {
                            dtTotal.GrandTotal = dtTotal.TotalAmount + dtTotal.CGST + dtTotal.SGST + dtTotal.FullCharges;
                            var grndttl = dtTotal.GrandTotal.ToString("0.00");
                            var spltgrndttl = grndttl.Split('.');
                            int pr1 = Convert.ToInt32(spltgrndttl[0]);
                            int pr2 = Convert.ToInt32(spltgrndttl[1]);
                            if (pr2 > 0)
                            {
                                dtTotal.GrandTotal = pr1 + 1;
                            }
                        }
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
            byte[] bindData = System.Text.Encoding.ASCII.GetBytes(sw.ToString());
            return File(bindData, "application/ms-excel", "DailyManifesto" + DateTime.Now + ".xls");
        }
        [HttpGet]
        public JsonResult GetLoggedInUser()
        {
            long userId = (long)Session["UserId"];
            var loggedInUser = db.AdminUsers.Where(admin => admin.UserId == userId).FirstOrDefault();
            return Json(new { loggedInUser = loggedInUser }, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public JsonResult FillCourierDetails(string awbno)
        {
            //List<RateMappingDetails> rateMappingList = new List<RateMappingDetails>();
            try
            {
                DataSet ds = new DataSet();
                SqlConnection connString = new SqlConnection(db.Database.Connection.ConnectionString);
                SqlCommand cmd = new SqlCommand("usp_GetCourierDetailsbyAWBNo", connString);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Awbno", awbno);
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                sda.Fill(ds);
                //if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                //{
                //    foreach (DataRow dr in ds.Tables[0].Rows)
                //    {
                //        RateMappingDetails ratedetails = new RateMappingDetails();
                //        ratedetails.Rate = Convert.ToDecimal(dr["Rate"]);
                //        ratedetails.FromWt = Convert.ToDecimal(dr["FromWt"]);
                //        ratedetails.ToWt = Convert.ToDecimal(dr["ToWt"]);
                //        ratedetails.PartyName = Convert.ToString(dr["PartyName"]);
                //        //ratedetails.PartyId = Convert.ToInt32(dr["PartyId"]);
                //        ratedetails.CourrierModeName = Convert.ToString(dr["CourrierModeName"]);
                //        //ratedetails.CourrierModeId = Convert.ToInt32(dr["CourrierModeId"]);
                //        ratedetails.NetworkModeName = Convert.ToString(dr["NetworkModeName"]);
                //        ratedetails.Id = Convert.ToInt32(dr["Id"]);
                //        rateMappingList.Add(ratedetails);
                //    }
                //}
                return Json(JsonConvert.SerializeObject(ds), JsonRequestBehavior.AllowGet);
            }
            catch (Exception Ex)
            {
                return Json("", JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public JsonResult Delete(int id)
        {
            if (id > 0)
            {
               //var dataexist= db.CourrierMasters.Where(x => x.CourrierId == id).ToList();
               // if (dataexist != null)
               // {
                    CourrierMaster courrierMaster = new CourrierMaster();
                    courrierMaster.CourrierId = id;
                    db.Entry(courrierMaster).State = System.Data.Entity.EntityState.Deleted;
                    db.SaveChanges();
                //}
            }
            return Json(new { response = "Deleted"}, JsonRequestBehavior.AllowGet);
        }

        //[HttpPost]
        //public JsonResult SearchDestination(string Prefix)
        //{
        //    var DestinationList = (from c in db.DestinationMaster
        //                           where c.Name.StartsWith(Prefix)
        //                     select new { c.Name, c.Id });
        //    return Json(DestinationList, JsonRequestBehavior.AllowGet);
        //}


        //[HttpPost]
        //public JsonResult SearchParty(string Prefix)
        //{
        //    var PartyList = (from c in db.PartyMasters
        //                           where c.PartyName.StartsWith(Prefix)
        //                           select new { c.PartyName, c.PartyId });
        //    return Json(PartyList, JsonRequestBehavior.AllowGet);
        //}




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
        public int? BillId { get; set; }
        public string FuelCharges { get; set; }
        public string PartyType { get; set; }
        public bool IsIGST { get; set; }
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
        public decimal Discount { get; set; }
        public decimal NetAmount { get; set; }
    }
}