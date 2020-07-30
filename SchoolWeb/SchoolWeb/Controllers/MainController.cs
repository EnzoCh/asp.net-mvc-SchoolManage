using SchoolWeb.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace SchoolWeb.Controllers
{
    public class MainController : Controller
    {
        /// <summary>
        /// 主页
        /// 作者:吴江帆
        /// 创建时间:2020-7-08 17:17:30
        /// </summary>
        SchoolManageEntities6 db = new SchoolManageEntities6();
        // GET: Main


        public ActionResult main()
        {
            return View();
        }


        /// <summary>
        /// 查询
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            string type = Session["logintype"].ToString();
            if (Session["LoginUser"] == null)
            {
                return RedirectToAction("Login");
            }
            List<dynamic> dnc = new List<dynamic>();
            if (type != "2")
            {
                string stano = (Session["LoginUser"] as LoginInfo).StaNo;
                var result = from a in db.Staff
                             join b in db.StaffType on a.Sta_Type equals b.ST_No
                             where a.Sta_No == stano
                             select new
                             {
                                 StaNo = a.Sta_No,
                                 Name = a.Sta_Name,
                                 StaSex = a.Sta_Sex,
                                 StaBirday = a.Sta_Birthday,
                                 StaTele = a.Sta_Telephone,
                                 StaQQ = a.Sta_QQ,
                                 StaAddress = a.Sta_Address,
                                 StaInDate = a.Sta_Indate,
                                 StaEmail = a.Sta_Email,
                                 StaType = b.ST_Name
                             };


                foreach (var item in result.ToList())
                {
                    dynamic d = new ExpandoObject();
                    d.No = item.StaNo;
                    d.Name = item.Name;
                    d.Sex = item.StaSex;
                    d.Birday = item.StaBirday;
                    d.Tele = item.StaTele;
                    d.QQ = item.StaQQ;
                    d.Address = item.StaAddress;
                    d.InDate = item.StaInDate;
                    d.Email = item.StaEmail;
                    d.Type = item.StaType;
                    dnc.Add(d);
                }
            }
            else
            {
                string stuno = (Session["LoginUser"] as LoginInfo).StuNo;
                var result = from a in db.Student
                             join b in db.ClassInfo on a.S_ClassNo equals b.Cla_No
                             join c in db.Staff on b.Cla_Man equals c.Sta_No
                             where a.S_No == stuno
                             select new
                             {
                                 StuNo = a.S_No,
                                 Name = a.S_Name,
                                 StuSex = a.S_Sex,
                                 StuBirday = a.S_Birthday,
                                 StuTele = a.S_Telephone,
                                 StuQQ = a.S_QQ,
                                 StuAddress = a.S_Address,
                                 StuInDate = a.S_Indate,
                                 StuEmail = a.S_Email,
                                 StuClassName = b.Cla_Name,
                                 StuColName = c.Sta_Name
                             };
                foreach (var item in result.ToList())
                {
                    dynamic d = new ExpandoObject();
                    d.No = item.StuNo;
                    d.Name = item.Name;
                    d.Sex = item.StuSex;
                    d.Birday = item.StuBirday;
                    d.Tele = item.StuTele;
                    d.QQ = item.StuQQ;
                    d.Address = item.StuAddress;
                    d.InDate = item.StuInDate;
                    d.Email = item.StuEmail;
                    d.ClassName = item.StuClassName;
                    d.ColName = item.StuColName;
                    dnc.Add(d);

                }
            }
            Session["stuNo"] = (Session["LoginUser"] as LoginInfo).StuNo;
            ViewBag.loginInfo = dnc.ToList().LastOrDefault();
            ViewBag.Type = type;
            return View();
        }

    }
}