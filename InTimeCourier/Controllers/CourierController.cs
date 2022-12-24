using InTimeCourier.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Configuration;
using InTimeCourier.App_Start;

namespace InTimeCourier.Controllers
{
    [SessionAuthorize]
    public class CourierController : Controller
    {
        InLogisticModel db = new InLogisticModel();
        // GET: Courier
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult AddCourierMode()
        {
            return View();
        }
        [HttpPost]
        public ActionResult AddCourierMode(CourrierMode courrierMode)
        {
            if (ModelState.IsValid)
            {
                var courrierModeList = db.CourrierModes.Where(x => x.CourrierModeName == courrierMode.CourrierModeName).ToList();
                if (courrierModeList.Count == 0 && courrierMode != null)
                {
                    courrierMode.CreatedBy = CourierHelper.UserId;
                    courrierMode.CreatedDate = DateTime.Now;
                    courrierMode.IsActive = true;
                    db.CourrierModes.Add(courrierMode);
                    db.SaveChanges();
                    ViewBag.Message = "Courier Mode added successfully!!!";
                }
                else
                {
                    ViewBag.ErrorMessage = "This courier mode already exists";
                }
            }
            else
            {
                ViewBag.ErrorMessage = "Please fill all required field";
            }
            return View();
        }

        [HttpGet]
        public ActionResult CourierModeDetails(long? id)
        {
            CourrierMode courierModeDetails = db.CourrierModes.Where(x => x.CourrierModeId == id).FirstOrDefault();
            return View(courierModeDetails);
        }

        [HttpGet]
        public ActionResult CourierModeList()
        {
            IEnumerable<CourrierMode> list = db.CourrierModes.OrderByDescending(x => x.CourrierModeId).ToList();
            return View(list);
        }

        [HttpGet]
        public ActionResult EditCourierMode(long? id)
        {
            CourrierMode courrierModeDetails = new CourrierMode();
            var courrierModeDetailslist = db.CourrierModes.Where(x => x.CourrierModeId == id).ToList();
            if (courrierModeDetailslist.Count > 0)
            {
                courrierModeDetails.CourrierModeId = courrierModeDetailslist[0].CourrierModeId;
                courrierModeDetails.CourrierModeName = courrierModeDetailslist[0].CourrierModeName;
                courrierModeDetails.Description = courrierModeDetailslist[0].Description;
            }
            return View(courrierModeDetails);
        }
        [HttpPost]
        public ActionResult EditCourierMode(CourrierMode courrierMode)
        {
            if (ModelState.IsValid)
            {
                courrierMode.ModifiedBy = CourierHelper.UserId;
                courrierMode.ModifiedDate = DateTime.Now;
                courrierMode.IsActive = true;
                db.Entry(courrierMode).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }
            return RedirectToAction("CourierModeList");
        }



        [HttpGet]
        public ActionResult PartyList()
        {
            IEnumerable<PartyMaster> list = db.PartyMasters.OrderByDescending(x => x.PartyId).ToList();
            return View(list);
        }
        [HttpGet]
        public ActionResult AddParty()
        {
            BindDropdown();
            return View();
        }
        [HttpPost]
        public ActionResult AddParty(PartyMaster party)
        {
            BindDropdown();
            if(ModelState.IsValid)
            {
                var partyList = db.PartyMasters.Where(x => x.MobileNo == party.MobileNo).ToList();
                if(partyList.Count==0 &&  party!=null)
                {
                    party.CreatedBy = CourierHelper.UserId;
                    party.CreatedDt = DateTime.Now;
                    party.IsActive = true;
                    db.PartyMasters.Add(party);
                    
                    db.SaveChanges();
                    ViewBag.Message = "Party added successfully!!!";
                }
                else
                {
                    ViewBag.ErrorMessage = "This party already exists";
                }
            }
            else
            {
                ViewBag.ErrorMessage = "Please fill all required field";
            }
            return View();
        }
        public void BindDropdown()
        {
            try
            {
                ViewBag.State = new SelectList(db.StateMasters.Where(x => x.CountryId == 1 && x.IsActive==true).OrderBy(x => x.StateName).ToList(),"StateId","StateName");
                ViewBag.Country = new SelectList(db.CountryMasters.Where(x=>x.IsActive == true).OrderBy(x => x.CountryName).ToList(), "CountryId", "CountryName");
            }
            catch (Exception ex)
            {

            }
        }

        [HttpGet]
        public ActionResult PartyDetails(long? id)
        {
            PartyDetails party = GetPartyDetailsById(id);
            return View(party);
        }

        [HttpGet]
        public ActionResult Edit(long? id)
        {
            BindDropdown();
            PartyMaster party = new PartyMaster();
            var list = db.PartyMasters.Where(x => x.PartyId == id).ToList();
            if (list.Count > 0)
            {
                party.PartyName = list[0].PartyName;
                party.MobileNo = list[0].MobileNo;
                party.EmailId = list[0].EmailId;
                party.AlternateContact = list[0].AlternateContact;
                party.Ladline = list[0].Ladline;
                party.Landmark = list[0].Landmark;
                party.PartyId = list[0].PartyId;
                party.Address1 = list[0].Address1;
                party.Address2 = list[0].Address2;
                party.City = list[0].City;
                party.CountryId = list[0].CountryId;
                party.StateId = list[0].StateId;
                party.IsActive = list[0].IsActive;
                party.ModifyBy = 1;
                party.ModifyDate = DateTime.Now;
                party.Pincode = list[0].Pincode;
                party.FuelCharges = list[0].FuelCharges;
                party.PartyType=list[0].PartyType;
                party.Discount=list[0].Discount;
                party.IsIGST=list[0].IsIGST;
                party.GSTNumber=list[0].GSTNumber;
            }
            return View(party);
        }
        [HttpPost]
        public ActionResult Edit(PartyMaster party)
        {
            if(ModelState.IsValid)
            {
                party.ModifyBy = CourierHelper.UserId;
                party.ModifyDate = DateTime.Now;
                party.IsActive = true;
                db.Entry(party).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }
            return RedirectToAction("PartyList");
        }
        public ActionResult AcivateDeativate(long ? id)
        {
            if(id>0)
            {
                SqlConnection connString = new SqlConnection(db.Database.Connection.ConnectionString);
                if (connString.State == ConnectionState.Closed)
                    connString.Open();
                SqlCommand cmd = new SqlCommand("uspDeletePartyDetails", connString);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@PartyId", id);
                int a = cmd.ExecuteNonQuery();
            }
            return RedirectToAction("PartyList");
        }
        private PartyDetails GetPartyDetailsById(long? id)
        {
            PartyDetails party = new PartyDetails();
            var list = (from PM in db.PartyMasters
                        join S in db.StateMasters on PM.StateId equals S.StateId
                        join C in db.CountryMasters on PM.CountryId equals C.CountryId
                        select new PartyDetails
                        {
                            PartyName = PM.PartyName,
                            PartyId = PM.PartyId,
                            CountryId = PM.CountryId,
                            CountryName = C.CountryName,
                            StateId = PM.StateId,
                            StateName = S.StateName,
                            GSTNumber = PM.GSTNumber,
                            Address1 = PM.Address1,
                            Address2 = PM.Address2,
                            AlternateContact = PM.AlternateContact,
                            City = PM.City,
                            IsActive = PM.IsActive,
                            EmailId = PM.EmailId,
                            Ladline = PM.Ladline,
                            Landmark = PM.Landmark,
                            MobileNo = PM.MobileNo,
                            Pincode = PM.Pincode
                        }
                       ).Where(x=>x.PartyId==id).ToList();
            if(list.Count>0)
            {
                party.PartyName = list[0].PartyName;
                party.MobileNo = list[0].MobileNo;
                party.EmailId = list[0].EmailId;
                party.AlternateContact = list[0].AlternateContact;
                party.Ladline = list[0].Ladline;
                party.Landmark = list[0].Landmark;
                party.PartyId = list[0].PartyId;
                party.Address1 = list[0].Address1;
                party.Address2 = list[0].Address2;
                party.City = list[0].City;
                party.CountryId = list[0].CountryId;
                party.StateId = list[0].StateId;
                party.IsActive = list[0].IsActive;
                party.ModifyBy = 1;
                party.ModifyDate = DateTime.Now;
                party.StateName = list[0].StateName;
                party.CountryName = list[0].CountryName;
                party.Pincode = list[0].Pincode;
                party.ActivationMode = list[0].IsActive == true ? "Active" : "Deactive" ;
            }
            return party;
        }
    }
}