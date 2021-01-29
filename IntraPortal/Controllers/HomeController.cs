using IntraPortal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IntraPortal.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        
        public ActionResult Edit(RegisterViewModel loggedInUser)
        {
            ModelState.Clear();
            loggedInUser.IsAdded = 0;
            return View(loggedInUser);
        }
        [HttpPost]
        public ActionResult Update(RegisterViewModel loggedInUser)
        {
            var reg = new RegisterViewModel();

           

            var status= reg.UpdateUser(loggedInUser);
            if (status == Convert.ToInt16(DisplayMsg.Success))
            {
                ModelState.Clear();
                loggedInUser.IsAdded = Convert.ToInt16(DisplayMsg.Success);
                return RedirectToAction("Index", loggedInUser);
            }
            else if(status==Convert.ToInt16(DisplayMsg.DbError))
            {
                ModelState.Clear();
                loggedInUser.IsAdded = Convert.ToInt16(DisplayMsg.DbError);
                return RedirectToAction("Edit", loggedInUser);
            }
            return RedirectToAction("Index", loggedInUser);
        }

        public ActionResult Index(RegisterViewModel loggedInUser)
        {
            return View(loggedInUser);
        }
        [Authorize]
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }
        [Authorize(Roles = "A")]
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}