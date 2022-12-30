using InTimeCourier.App_Start;
using InTimeCourier.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace InTimeCourier.Controllers
{
    //[SessionAuthorize]
    public class AdminController : Controller
    {
        InLogisticModel db = new InLogisticModel();
        // GET: Admin
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(Login login)
        {
            if (!ModelState.IsValid)
            {
                return View(login);
            }
            var log = db.AdminUsers.Where(x => (x.Username == login.Username && x.Password == login.Password)).ToList();
            if (log.Count > 0)
            {
                Session["Name"] = log[0].Name;
                Session["Email"] = log[0].EmailId;
                Session["UserId"] = log[0].UserId;
                Session["LoggedInUser"] = log.FirstOrDefault();
                CourierHelper.UserId = int.Parse("0"+log[0].UserId);
                CourierHelper.Name = log[0].Name;
                CourierHelper.EmailId= log[0].EmailId;
                return RedirectToAction("Index","Home");
            }
            else
            {
                ViewBag.Error = "You have entered wrong username/password.";
                ModelState.AddModelError("You have entered wrong username/password.","Message");
                return View();
            }
        }
        public ActionResult LogOut()
        {
            Session.Abandon();
            return RedirectToAction("Login");
        }
        [SessionAuthorize]
        
        public ActionResult AddUser()
        {
            return View();
        }
        [SessionAuthorize]
        [HttpPost]
        public ActionResult AddUser(AdminUser user)
        {
            string msg = string.Empty;
            if (ModelState.IsValid)
            {
                SqlConnection connString = new SqlConnection(db.Database.Connection.ConnectionString);
                if (connString.State == ConnectionState.Closed)
                    connString.Open();
                SqlCommand cmd = new SqlCommand("uspRegisterAdminUser", connString);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Name", user.Name);
                cmd.Parameters.AddWithValue("@MobileNo", user.MobileNo);
                cmd.Parameters.AddWithValue("@EmailId", user.EmailId);
                cmd.Parameters.AddWithValue("@Username", user.Username);
                cmd.Parameters.AddWithValue("@Password", user.Password);
                cmd.Parameters.AddWithValue("@RoleId", user.RoleId);
                cmd.Parameters.AddWithValue("@UserId", 0);
                cmd.Parameters.Add("@ErrorMsg", SqlDbType.VarChar, 50);
                cmd.Parameters["@ErrorMsg"].Direction = ParameterDirection.Output;
                cmd.Parameters.Add("@ReturnUserId", SqlDbType.BigInt);
                cmd.Parameters["@ReturnUserId"].Direction = ParameterDirection.Output;
                msg = Convert.ToString(cmd.Parameters["@ErrorMsg"].Value);
                if(msg==string.Empty)
                {
                    msg = "Your user registered successfully with user id " + Convert.ToString(cmd.Parameters["@ReturnUserId"].Value);
                }
            }
            return new JavaScriptResult() { Script="<script>alert('"+msg+"')</script>" };
        }
    }
}