using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using eyemusic45.Models.ViewModels;

namespace eyemusic45.Controllers
{
    public class AccountController : Controller
    {
        public eyemusic45.Models.ViewModels.eyeMusicModel _eyeMusicModel;
        //
        // GET: /Account/

        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// show view of login
        /// </summary>
        /// <param name="ReturnUrl">The Url that the user try to enter for return after login</param>
        /// <returns></returns>
        public ActionResult Login(string ReturnUrl)
        {
            _eyeMusicModel =
                (eyemusic45.Models.ViewModels.eyeMusicModel)System.Web.HttpContext.Current.Session["Themodel"];
            ViewBag.ReturnUrl = ReturnUrl;

            if (_eyeMusicModel != null)
                ViewBag.len = _eyeMusicModel.len;

            return View("../Home/Login");
        }



    }
}
