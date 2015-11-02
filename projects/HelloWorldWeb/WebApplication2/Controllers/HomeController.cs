using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication2.Models;

namespace WebApplication2.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            return View(new UserModel());
        }

        /// <summary>
        /// 名前をpostされたときの処理
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Index([Bind(Include = "Name")] UserModel user)
        {
            return RedirectToAction("Hello", user);
        }

        /// <summary>
        /// あいさつページの表示
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public ActionResult Hello(UserModel user)
        {
            if (string.IsNullOrEmpty(user.Name))
            {
                return RedirectToAction("Index");
            }
            return View(user);
        }
    }
}