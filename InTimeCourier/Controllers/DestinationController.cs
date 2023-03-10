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
    [SessionAuthorize]
    public class DestinationController : Controller
    {
        InLogisticModel db = new InLogisticModel();
        // GET: Network
        public ActionResult Index()
        {
            var data = db.DestinationMaster.OrderBy(x => x.Id).ToList();
            return View(data);
        }

        public ActionResult AddDestination(int? id)
        {
            if(id!=null && id>0)
            {
                return View(db.DestinationMaster.Where(x=>x.Id==id).FirstOrDefault());
            }
            return View();
        }
        [HttpPost]
        public ActionResult AddDestination(DestinationMaster network)
        {
            var response = db.Database.SqlQuery<Responsedetails>("exec uspDestinationInsertUpdate @Id,@Name,@Description,@UserId",
                new SqlParameter("@Id", network.Id),
                new SqlParameter("@Name", network.Name),
                new SqlParameter("@Description", network.Description),
                new SqlParameter("@UserId", int.Parse("0" + Session["UserId"]))).FirstOrDefault();
            return RedirectToAction("Index");
        }

        public ActionResult AcivateDeativate(int? id)
        {
            if (id > 0)
            {
                SqlConnection connString = new SqlConnection(db.Database.Connection.ConnectionString);
                if (connString.State == ConnectionState.Closed)
                    connString.Open();
                SqlCommand cmd = new SqlCommand("uspActivateDeactivateDestination", connString);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Id", id);
                int a = cmd.ExecuteNonQuery();
            }
            return RedirectToAction("Index");
        }
    }
}