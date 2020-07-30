using SchoolWeb.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Dynamic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StudentManager.Controllers
{
    /// <summary>
    /// 课程信息
    /// 作者:刘冉旭
    /// 创建时间:2020-7-08 17:17:30
    /// </summary>
    public class ClassManagerController : Controller
    {
        
        
        SchoolManageEntities6 sm = new SchoolManageEntities6();
        // GET: ClassManager

        /// <summary>
        /// 课程信息修改
        /// </summary>
        /// <returns></returns>
        
        public ActionResult ClassUpd(int? couid)
        {
            //下拉列表的绑定
            List<SelectListItem> teID = new List<SelectListItem>();
            foreach (var item in sm.Staff.Where(a => a.IsDel.Equals(0)))
            {
                teID.Add(new SelectListItem() { Text = item.Sta_No.ToString(), Value = item.Sta_id.ToString() });
            }
            ViewData["Cou_Tid"] = teID;
            //绑定数据
            var couinfo=from a in sm.CourseInfo
                        where a.Cou_id==couid
                        select a;
            if (couinfo.ToList().Count > 0)
            {
                CourseInfo ci = couinfo.ToList().LastOrDefault();

                ViewBag.data = ci;
            }
            return View();
        }
        /// <summary>
        /// 课程信息修改
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ClassUpd(string ctype)
        {
            //下拉列表的绑定
            List<SelectListItem> teID = new List<SelectListItem>();
            foreach (var item in sm.Staff.Where(a => a.IsDel.Equals(0)))
            {
                teID.Add(new SelectListItem() { Text = item.Sta_No.ToString(), Value = item.Sta_id.ToString() });
            }
            ViewData["Cou_Tid"] = teID;
            string courseid = Request.Form["courseid"];
            string coursename = Request.Form["coursename"];
            int Cou_Tid = Convert.ToInt32(Request.Form["Cou_Tid"]);
            string remark = Request.Form["remark"];
            var stano = from a in sm.Staff
                        where a.Sta_id == Cou_Tid
                        select a;
            string stanum = stano.ToList().LastOrDefault().Sta_No;
            //var couid = from a in sm.CourseInfo
            //            where a.Cou_No == courseid
            //            select a;
            //int iid = couid.ToList().LastOrDefault().Cou_id;
            
                var dInfo = new CourseInfo();
                dInfo.Cou_No = courseid;
                dInfo.Cou_Name = coursename;
                dInfo.Cou_Teacher = stanum;
                dInfo.Cou_Type = ctype;
                dInfo.IsDel = 0;
                dInfo.Remark = remark;
                sm.Entry(dInfo).State = EntityState.Modified;
                sm.SaveChanges();

                //Content("<script>alert('课程已经修改完成');history.go(-1);</script>");

            return RedirectToAction("ClassM");
        }

        /// <summary>
        /// 课程信息增加
        /// </summary>
        /// <returns></returns>
        public ActionResult ClassAdd()
        {
            //下拉列表的绑定
            List<SelectListItem> teID = new List<SelectListItem>();
            foreach (var item in sm.Staff.Where(a => a.IsDel.Equals(0)))
            {
                teID.Add(new SelectListItem() { Text = item.Sta_No.ToString(), Value = item.Sta_id.ToString() });
            }
            ViewData["Cou_Tid"] = teID;
            return View();
        }

        /// <summary>
        /// 课程信息增加
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ClassAdd(string ctype)
        {
            //下拉列表的绑定
            List<SelectListItem> teID = new List<SelectListItem>();
            foreach (var item in sm.Staff.Where(a => a.IsDel.Equals(0)))
            {
                teID.Add(new SelectListItem() { Text = item.Sta_No.ToString(), Value = item.Sta_id.ToString() });
            }
            ViewData["Cou_Tid"] = teID;
            string courseid = Request.Form["courseid"];
            string coursename = Request.Form["coursename"];
            int Cou_Tid = Convert.ToInt32(Request.Form["Cou_Tid"]);
            string remark = Request.Form["remark"];
            var stano = from a in sm.Staff
                        where a.Sta_id == Cou_Tid
                        select a;
            string stanum = stano.ToList().LastOrDefault().Sta_No;
            var couid = from a in sm.CourseInfo
                        where a.Cou_No == courseid
                        select a;
            if (couid.ToList().Count>0)
            {
                return Content("<script>alert('存在相同的课程号');history.go(-1);</script>");
            }
            else if (couid.ToList().Count == 0)
            {
                var dInfo = new CourseInfo();
                dInfo.Cou_No = courseid;
                dInfo.Cou_Name = coursename;
                dInfo.Cou_Teacher = stanum;
                dInfo.Cou_Type = ctype;
                dInfo.IsDel = 0;
                dInfo.Remark = remark;
                sm.Entry<CourseInfo>(dInfo).State = System.Data.Entity.EntityState.Added;
                sm.SaveChanges();
                Response.Write("<script>alert('课程已经添加完成');</script>");
            }
            
            return View();
        }

        /// <summary>
        /// 课程信息查询
        /// </summary>
        /// <returns></returns>
        public ActionResult ClassM()
        {
            var results = from a in sm.CourseInfo
                          join b in sm.Staff on a.Cou_Teacher equals b.Sta_No
                          select new
                          {
                              Cou_id = a.Cou_id,
                              Cou_No = a.Cou_No,
                              Cou_Name = a.Cou_Name,
                              Cou_Teacher = b.Sta_Name,
                              Cou_Type = a.Cou_Type,
                              IsDel = a.IsDel,
                              Remark = a.Remark
                          };
            //多表查询
            List<dynamic> dnc = new List<dynamic>();
            foreach (var item in results.ToList())
            {
                dynamic d = new ExpandoObject();
                d.Cou_id = item.Cou_id;
                d.Cou_No = item.Cou_No;
                d.Cou_Name = item.Cou_Name;
                d.Cou_Teacher = item.Cou_Teacher;
                d.Cou_Type = item.Cou_Type;
                d.IsDel = item.IsDel;
                d.Remark = item.Remark;
                dnc.Add(d);
            }
            ViewBag.Data = dnc;
            return View();
        }

        /// <summary>
        /// 课程信息查询
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ClassM(string userInfo)
        {
            var results = from a in sm.CourseInfo
                          join b in sm.Staff on a.Cou_Teacher equals b.Sta_No
                          select new
                          {
                              Cou_id = a.Cou_id,
                              Cou_No = a.Cou_No,
                              Cou_Name = a.Cou_Name,
                              Cou_Teacher=b.Sta_Name,
                              Cou_Type=a.Cou_Type,
                              IsDel=a.IsDel,
                              Remark=a.Remark
                          };
            results = results.Where(a => a.Cou_No.Contains(userInfo) || a.Cou_Name.Contains(userInfo));
            //多表查询
            List<dynamic> dnc = new List<dynamic>();
            foreach (var item in results.ToList())
            {
                dynamic d = new ExpandoObject();
                d.Cou_id = item.Cou_id;
                d.Cou_No = item.Cou_No;
                d.Cou_Name = item.Cou_Name;
                d.Cou_Teacher = item.Cou_Teacher;
                d.Cou_Type = item.Cou_Type;
                d.IsDel = item.IsDel;
                d.Remark = item.Remark;
                dnc.Add(d);
            }
            ViewBag.Data = dnc;

            return View();
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <returns></returns>
        public ActionResult delete(int? id)
        {
            try
            {
                var ss = from a in sm.CourseInfo
                         where a.Cou_id == id
                         select a;
                CourseInfo ci = ss.ToList().LastOrDefault();

                sm.CourseInfo.Remove(ci);
                sm.SaveChanges();
                return Content("<script>alert('删除成功');history.go(-1);</script>");
            }
            catch (Exception)
            {

                return null;
            }
        }
    }
}