using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace InTimeCourier.Controllers
{
    public class NetworkController : Controller
    {
        // GET: Network
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult AddNetwork(int? id)
        {
            return View();
        }
    }
}