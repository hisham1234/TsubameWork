using System;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using IntraPortal.Models;
using System.Configuration;
using IntraPortal.Db;
using System.Collections.Generic;
using System.Web.Security;

namespace IntraPortal.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {

        private string strcon = ConfigurationManager.ConnectionStrings["conGps"].ConnectionString;

        public AccountController()
        {
        }

    
       

      
        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            var accntModel = new LoginViewModel { Result = 1 };
            return View(accntModel);
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var db = new clsDb();
            string msg = "";
            string chkValidLoginSql = "Select staffNo from users where staffNo='" + model.StaffNumber + "' and password='"+ model.Password + "'" ;
            db.SetbolConnection(strcon, ref msg);
            var i = db.GetstrSingleColumnSelectData(chkValidLoginSql);
            if (i=="")
            {
                ModelState.Clear();
                model.Result = 2;
                return View(model);
            }
            if (i == model.StaffNumber)
            {
                model.Result = 1;
                FormsAuthentication.SetAuthCookie(model.StaffNumber, false);
                
                var regViewModel= model.GetUserById(Int32.Parse(model.StaffNumber));
                return RedirectToAction("Index","Home",regViewModel);
            }else
            {
                ModelState.Clear();
                model.Result = 0;
                return View(model);
            }
           
           
        }

       
      
        //
        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult Register()
        {
            var regmod = new RegisterViewModel { IsAdded = 0 };
            return View(regmod);
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                
                              
                var ValidStaff = model.IsValidStaffNumber(model.StaffNumber);

                if( ValidStaff== Convert.ToInt16(DisplayMsg.InvalidStaffNumber))
                {
                    ModelState.Clear();
                    model.IsAdded = Convert.ToInt16(DisplayMsg.InvalidStaffNumber);

                    return View(model);
                }else if(ValidStaff== Convert.ToInt16(DisplayMsg.DbError))
                {
                    ModelState.Clear();
                    model.IsAdded = Convert.ToInt16(DisplayMsg.DbError);

                    return View(model);
                }

               


                if (model.RegisterUser(model) == Convert.ToInt16(DisplayMsg.Success))
                {
                    ModelState.Clear();
                    model.IsAdded =   Convert.ToInt16(DisplayMsg.Success);

                    return View(model);
                }
                   
                else
                {
                    ModelState.Clear();
                    model.IsAdded =  Convert.ToInt16(DisplayMsg.DbError);
                    return View(model);
                }
                    
            }
            

            // ここで問題が発生した場合はフォームを再表示します
            return View(model);
        }

       
        //
        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();
            //return RedirectToAction("Index", "Home");
            //AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Index", "Home");
        }

       

        #region ヘルパー
        // 外部ログインの追加時に XSRF の防止に使用します
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        internal class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }
        #endregion
    }
}