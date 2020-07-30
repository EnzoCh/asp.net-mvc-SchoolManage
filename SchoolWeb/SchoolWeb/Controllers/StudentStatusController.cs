using SchoolWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace jiaowuguanliMVC.Controllers
{
    public class StudentStatusController : Controller
    {
        /// <summary>
        /// 学籍查询
        /// 作者:曾梓豪
        /// 创建时间:2020-7-08 17:17:30
        /// </summary>
        SchoolManageEntities6 sm = new SchoolManageEntities6();
        // GET: StudentStatus
        public ActionResult Index()
        {
            if (Session["LoginUser"] == null)
            {
                return RedirectToAction("Login");
            }
            string stuno = (Session["LoginUser"] as LoginInfo).StuNo;
            Session["stuNo"] = (Session["LoginUser"] as LoginInfo).StuNo;
            var result = from a in sm.Status
                         where a.S_No == stuno
                         select a;
            return View(result);

        }

        
    }
}