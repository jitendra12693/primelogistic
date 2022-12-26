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
    public class NetworkController : Controller
    {
        InLogisticModel db = new InLogisticModel();
        // GET: Network
        public ActionResult Index()
        {
            var data = db.NetworkMaster.OrderBy(x => x.NetworkId).ToList();
            return View(data);
        }

        public ActionResult AddNetwork(int? id)
        {
            if(id!=null && id>0)
            {
                return View(db.NetworkMaster.Where(x=>x.NetworkId==id).FirstOrDefault());
            }
            return View();
        }
        [HttpPost]
        public ActionResult AddNetwork(NetworkMaster network)
        {
            var response = db.Database.SqlQuery<Responsedetails>("exec uspNetworkInsertUpdate @NetworkId,@NetworkName,@Address,@Description,@UserId",
                new SqlParameter("@NetworkId", network.NetworkId),
                new SqlParameter("@NetworkName", network.NetworkName),
                new SqlParameter("@Address", network.Address),
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
                SqlCommand cmd = new SqlCommand("uspActivateDeactivateNetwork", connString);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@NetworkId", id);
                int a = cmd.ExecuteNonQuery();
            }
            return RedirectToAction("Index");
        }
    }
}