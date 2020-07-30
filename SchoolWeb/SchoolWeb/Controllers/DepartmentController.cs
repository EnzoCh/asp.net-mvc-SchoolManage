using SchoolWeb.Models;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace jiaowuguanliMVC.Controllers
{
    public class DepartmentController : Controller
    {
        /// <summary>
        /// 系别管理
        /// 作者:曾梓豪
        /// 创建时间:2020-7-08 17:17:30
        /// </summary>
        SchoolManageEntities6 sm = new SchoolManageEntities6();

        // GET: Department
        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="Index"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Index(string Index)
        {
            
            //绑定下拉列表
            //系主任
            List<SelectListItem> departs = new List<SelectListItem>();
            SelectListItem sli0 = new SelectListItem() { Text = "请选择", Value = "0" };
            departs.Add(sli0);
            foreach (var item in sm.Staff.Where(a => a.Sta_Type == "ST001"))
            {
                SelectListItem sli = new SelectListItem() { Value = item.Sta_No.ToString(), Text = item.Sta_Name };
                departs.Add(sli);
            }
            ViewData["ColMan"] = departs;


            //查询数据库
            var result = from a in sm.CollegeInfo
                         join b in sm.Staff on a.Col_Man equals b.Sta_No
                         where a.IsDel == 0
                         select new
                         {
                             ID = a.Col_id,
                             ColNo=a.Col_No,
                             ColName = a.Col_Name,
                             CreateTime = a.Col_CreateTime,
                             ColMna = a.Col_Man,
                             ColeMan = b.Sta_Name,
                             Remark = a.Remark,
                             ColNum=a.Col_No
                         };

            result = result.Where(a => a.ColName.Contains(Index));
            //多表查询
            List<dynamic> dnc = new List<dynamic>();
            foreach (var item in result.ToList())
            {
                dynamic d = new ExpandoObject();
                d.ID = item.ID;
                d.ColNo = item.ColNo;
                d.ColName = item.ColName;
                d.CreateTime = item.CreateTime;
                d.ColMan = item.ColeMan;
                d.Remark = item.Remark;
                dnc.Add(d);
            }
            ViewBag.Data = dnc;
            return View(result);

        }

        public ActionResult Index()
        {
            
            //绑定下拉列表
            //系主任
            List<SelectListItem> departs = new List<SelectListItem>();
            SelectListItem sli0 = new SelectListItem() { Text = "请选择", Value = "0" };
            departs.Add(sli0);
            foreach (var item in sm.Staff.Where(a=>a.Sta_Type== "ST001"))
            {
                SelectListItem sli = new SelectListItem() { Value = item.Sta_No.ToString(), Text = item.Sta_Name };
                departs.Add(sli);
            }
            ViewData["ColMan"] = departs;
            var result = from a in sm.CollegeInfo
                         join b in sm.Staff on a.Col_Man equals b.Sta_No
                         where a.IsDel == 0
                         select new
                         {
                             ID = a.Col_id,
                             ColNo = a.Col_No,
                             ColName = a.Col_Name,
                             CreateTime = a.Col_CreateTime,
                             ColMna = a.Col_Man,
                             ColeMan = b.Sta_Name,
                             Remark = a.Remark,
                             ColNum = a.Col_No
                         };
            //多表查询
            List<dynamic> dnc = new List<dynamic>();
            foreach (var item in result.ToList())
            {
                dynamic d = new ExpandoObject();
                d.ID = item.ID;
                d.ColNo = item.ColNo;
                d.ColName = item.ColName;
                d.CreateTime = item.CreateTime;
                d.ColMan = item.ColeMan;
                d.Remark = item.Remark;
                dnc.Add(d);
            }
            ViewBag.Data = dnc;
            return View();
        }

        /// <summary>
        /// 获得一个新编码
        /// </summary>
        /// <returns></returns>
        public string newNum()
        {
            var results = from a in sm.CollegeInfo
                          select a;
            CollegeInfo sr = results.ToList().LastOrDefault();
            string num = sr.Col_No;

            var hounum = num.Substring(3, 3);
            var qiannum = num.Substring(0, 3);
            if (qiannum == "Col" && Convert.ToInt32(hounum) < 9)
            {
                hounum = "00" + (Convert.ToInt32(hounum) + 1).ToString();
                return qiannum + hounum;
            }
            else if (qiannum == "Col" && Convert.ToInt32(hounum) > 8 && Convert.ToInt32(hounum) < 99)
            {
                hounum = "0" + (Convert.ToInt32(hounum) + 1).ToString();
                return qiannum + hounum;
            }
            else if (qiannum == "Col" && Convert.ToInt32(hounum) > 98)
            {
                hounum = (Convert.ToInt32(hounum) + 1).ToString();
                return qiannum + hounum;
            }
            else
            {
                return num.ToString();
            }
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult addone(string ColName, string ColMan, string RemarkA, string ColNo)
        {
            try
            {
                //获得生成编码
                ColNo = newNum();
                //验证是否重复
                var exists = from a in sm.CollegeInfo
                             where a.Col_Name == ColName
                             select a;
                //验证
                //系名是否为空验证
                if (string.IsNullOrEmpty(ColName))
                {
                    return Content("<script> alert('系名不能为空');history.go(-1);</script>");
                }
                //系名正则验证
                Regex rgx = new Regex(@"(^[\u4e00-\u9fa5]{0,}$)");
                if (!rgx.IsMatch(ColName))
                {
                    return Content("<script> alert('系名只能为汉字');history.go(-1);</script>");
                }
                //系主任是否为空验证
                if (ColMan == "0")
                {
                    return Content("<script> alert('请重新选择系主任');history.go(-1);</script>");
                }
                //系名是否重复验证
                if (exists.ToList().Count > 0)
                {
                    return Content("<script> alert('已经存在相同数据');history.go(-1);</script>");
                }
                else
                {
                    //新增
                    var colInfo = new CollegeInfo();
                    colInfo.Col_No = ColNo;
                    colInfo.Col_Name = ColName;
                    colInfo.Col_Man = ColMan;
                    colInfo.Col_CreateTime = DateTime.Now;
                    colInfo.Remark = RemarkA;
                    colInfo.IsDel = 0;
                    sm.Entry<CollegeInfo>(colInfo).State = System.Data.Entity.EntityState.Added;
                    sm.SaveChanges();
                }
            }
            catch
            {

                return RedirectToAction("Index");
            }

            return RedirectToAction("Index");
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        public ActionResult delete(int id)
        {
            CollegeInfo college = sm.CollegeInfo.Where(c => c.Col_id == id).FirstOrDefault();//先查找出要修改的对象
            college.IsDel = 1;//修改数据

            if (sm.SaveChanges() > 0)
            {
                Response.Write("<script>alert('删除成功')</script>");
                this.Index();
            }
            else
            {
                Response.Write("<script>alert('删除失败')</script>");
            }
            return RedirectToAction("Index");

        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult update(CollegeInfo cg, string ColMan)
        {
            try
            {

                //绑定下拉列表
                //系主任
                List<SelectListItem> departs = new List<SelectListItem>();

                foreach (var item in sm.Staff.Where(a => a.Sta_Type == "ST001"))
                {
                    SelectListItem sli = new SelectListItem() { Value = item.Sta_No.ToString(), Text = item.Sta_Name };
                    departs.Add(sli);
                }
                ViewData["ColMan"] = departs;
                //修改
                var collegeinfo = new CollegeInfo();
                collegeinfo.Col_id = cg.Col_id;
                collegeinfo.Col_No = cg.Col_No;
                collegeinfo.Col_Name = cg.Col_Name;
                collegeinfo.Col_Man = cg.Col_Man;
                collegeinfo.Col_CreateTime = DateTime.Now;
                collegeinfo.IsDel = 0;
                collegeinfo.Remark = cg.Remark;
                sm.Entry<CollegeInfo>(collegeinfo).State = System.Data.Entity.EntityState.Modified;

                if (sm.SaveChanges() > 0)
                {
                    Response.Write("<script>alert('修改成功')</script>");
                }
                else
                {
                    Response.Write("<script>alert('修改失败')</script>");
                }
            }
            catch
            {
                return Content("<script>window.close()</script>");

            }

            return Content("<script>window.close()</script>");
        }

        public ActionResult update(int id)
        {
            var depart = from a in sm.CollegeInfo
                         where a.Col_id == id
                         select a;
            CollegeInfo sd = depart.ToList().LastOrDefault();

            //绑定下拉列表
            //系主任
            List<SelectListItem> departs = new List<SelectListItem>();

            foreach (var item in sm.Staff.Where(a => a.Sta_Type == "ST001"))
            {
                SelectListItem sli = new SelectListItem() { Value = item.Sta_No.ToString(), Text = item.Sta_Name };
                departs.Add(sli);
            }
            ViewData["ColMan"] = departs;
            return View(sd);
        }
    }
}