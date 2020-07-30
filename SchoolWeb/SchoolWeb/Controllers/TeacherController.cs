using SchoolWeb.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Dynamic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace czwshixun.Controllers
{
    public class TeacherController : Controller
    {
        /// <summary>
        /// 教师信息
        /// 作者:陈忠文
        /// 创建时间:2020-7-08 17:17:30
        /// </summary>
        SchoolManageEntities6 sme = new SchoolManageEntities6();

        // GET: Teacher
        public ActionResult Index()
        {
            var result = from a in sme.Staff
                         join b in sme.StaffType on a.Sta_Type equals b.ST_No
                         where b.ST_No != "UT004" && b.ST_No != "ST006" && a.IsDel == 0
                         select new
                         {
                             StaNo = a.Sta_No,
                             StaName = a.Sta_Name,
                             Sex = a.Sta_Sex,
                             BormDate = a.Sta_Birthday,
                             Type = b.ST_Name,
                             Phone = a.Sta_Telephone,
                             QQ = a.Sta_QQ,
                             Address = a.Sta_Address,
                             InDate = a.Sta_Indate,
                             Email = a.Sta_Email,
                             Remark = a.Remark
                         };

            List<dynamic> dnc = new List<dynamic>();
            foreach (var item in result.ToList())
            {
                dynamic d = new ExpandoObject();
                d.StaNo = item.StaNo;
                d.StaName = item.StaName;
                d.Sex = item.Sex;
                d.BornDate = item.BormDate;
                d.Type = item.Type;
                d.Phone = item.Phone;
                d.QQ = item.QQ;
                d.Address = item.Address;
                d.Indate = item.InDate;
                d.Email = item.Email;
                d.Remark = item.Remark;
                dnc.Add(d);
            }

            ViewBag.Data = dnc;
            return View();
        }

        /// <summary>
        /// 修改更新
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult update(Staff st)
        {
            //绑定下拉列表(职称)
            List<SelectListItem> departs = new List<SelectListItem>();
            SelectListItem sli0 = new SelectListItem() { Text = "请选择", Value = "0" };
            departs.Add(sli0);
            foreach (var item in sme.StaffType)
            {
                SelectListItem sli = new SelectListItem() { Value = item.ST_No, Text = item.ST_Name.ToString() };
                departs.Add(sli);
            }
            ViewData["ColMan"] = departs;

            //绑定下拉列表(性别)
            List<SelectListItem> sex = new List<SelectListItem>();
            SelectListItem sex0 = new SelectListItem() { Text = "男", Value = "男" };
            SelectListItem sex1 = new SelectListItem() { Text = "女", Value = "女" };
            sex.Add(sex0);
            sex.Add(sex1);
            ViewData["Sex"] = sex;

            //验证(姓名验证，只能为2-4个中文)
            string xingming = @"^[\u4E00-\u9FA5]{2,4}$";
            Regex regeXM = new Regex(xingming);

            //验证(手机验证)
            //电信手机号码正则表达式
            string dianxin = @"^1[3578][01379]\d{8}$";
            Regex regeDX = new Regex(dianxin);

            //联通手机号码正则表达式
            string liantong = @"^1[34578][01256]\d{8}";
            Regex regexLT = new Regex(liantong);

            //移动手机号码正则表达式
            string yidong = @"^(1[012345678])\d{8}|1[345678][0123456789]\d{8}$";
            Regex regexYD = new Regex(yidong);

            //验证(邮箱验证)
            string email = @"^[A-Za-z0-9\u4e00-\u9fa5]+@[a-zA-Z0-9_-]+(\.[a-zA-Z0-9_-]+)+$";
            Regex regeEM = new Regex(email);

            //验证(QQ)
            string QQ = @"^[1-9][0-9]{4,12}$";
            Regex regexQQ = new Regex(QQ);


            if (string.IsNullOrEmpty(st.Sta_Name))
            {
                Response.Write("<script>alert('姓名不能为空')</script>");

                //return Content("<script>parent.window.close()</script>");
            }
            else if (!regeXM.IsMatch(st.Sta_Name))
            {
                Response.Write("<script>alert('姓名格式不正确,请输入2-4个汉字。')</script>");
                return Content("<script>parent.window.close()</script>");
            }
            else if (st.Sta_Birthday.Year == 1)
            {
                Response.Write("<script>alert('出生年月不能为空')</script>");
                return Content("<script>parent.window.close()</script>");
            }
            else if (st.Sta_Indate.Year == 1)
            {
                Response.Write("<script>alert('入职时间不能为空')</script>");
                return Content("<script>parent.window.close()</script>");
            }
            else if (string.IsNullOrEmpty(st.Sta_Telephone))
            {
                Response.Write("<script>alert('电话不能为空')</script>");
                return Content("<script>parent.window.close()</script>");
            }
            else if (!(regeDX.IsMatch(st.Sta_Telephone) || regexLT.IsMatch(st.Sta_Telephone) || regexYD.IsMatch(st.Sta_Telephone)))
            {
                Response.Write("<script>alert('电话格式不正确')</script>");
            }
            else if (string.IsNullOrEmpty(st.Sta_Address))
            {
                Response.Write("<script>alert('地址不能为空')</script>");
                return Content("<script>parent.window.close()</script>");
            }
            else if (st.Sta_Email != null && !regeEM.IsMatch(st.Sta_Email))
            {
                Response.Write("<script>alert('邮箱格式不正确')</script>");
                return Content("<script>parent.window.close()</script>");
            }
            else if (st.Sta_QQ != null && !regexQQ.IsMatch(st.Sta_QQ))
            {
                Response.Write("<script>alert('QQ格式不正确')</script>");
                return Content("<script>parent.window.close()</script>");
            }
            else
            {
                try
                {
                    var Staff = new Staff();
                    Staff.Sta_No = st.Sta_No;
                    Staff.Sta_Name = st.Sta_Name;
                    Staff.Sta_Sex = st.Sta_Sex;
                    Staff.Sta_Birthday = st.Sta_Birthday;
                    Staff.Sta_Type = st.Sta_Type;
                    Staff.Sta_Telephone = st.Sta_Telephone;
                    Staff.Sta_QQ = st.Sta_QQ;
                    Staff.Sta_Address = st.Sta_Address;
                    Staff.Sta_Indate = st.Sta_Indate;
                    Staff.Sta_Email = st.Sta_Email;
                    Staff.Remark = st.Remark;
                    Staff.IsDel = 0;

                    sme.Entry<Staff>(Staff).State = System.Data.Entity.EntityState.Modified;
                    if (sme.SaveChanges() > 0)
                    {
                        Response.Write("<script>alert('成功');</script>");
                        return Content("<script>parent.window.close()</script>");
                    }
                    else
                    {
                        Response.Write("<script>alert('失败');</script>");
                    }
                }
                catch (Exception)
                {

                    return null;
                }
            }
            return View();
        }

        public ActionResult update(string StaNo)
        {
            this.Index();
            //绑定下拉列表(职称)
            List<SelectListItem> departs = new List<SelectListItem>();
            SelectListItem sli0 = new SelectListItem() { Text = "请选择", Value = "0" };
            departs.Add(sli0);
            foreach (var item in sme.StaffType)
            {
                SelectListItem sli = new SelectListItem() { Value = item.ST_No, Text = item.ST_Name.ToString() };
                departs.Add(sli);
            }
            ViewData["ColMan"] = departs;

            //绑定下拉列表(性别)
            List<SelectListItem> sex = new List<SelectListItem>();
            SelectListItem sex0 = new SelectListItem() { Text = "男", Value = "男" };
            SelectListItem sex1 = new SelectListItem() { Text = "女", Value = "女" };
            sex.Add(sex0);
            sex.Add(sex1);
            ViewData["Sex"] = sex;

            var staff = from a in sme.Staff
                        where a.Sta_No == StaNo
                        select a;
            Staff st = staff.ToList().LastOrDefault();
            return View(st);
        }

        /// <summary>
        /// 获得一个新编码UserCode
        /// </summary>
        /// <returns></returns>
        public string newNum()
        {
            var results = from a in sme.Staff
                          select a;
            Staff ad = results.ToList().LastOrDefault();
            string num = ad.Sta_No;
            int n = Convert.ToInt32(num.Substring(4, 2));
            n += 1;
            if (n < 10)
            {
                return "Sta00" + n;
            }
            else if (n < 100 && n >= 10)
            {
                return "Sta0" + n;
            }
            else if (n >= 100 && n < 1000)
            {
                return "Sta" + n;
            }
            else
            {
                return n.ToString();
            }

        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult add(Staff s,string ColMan,string Sex,string Sta_No)
        {
            Sta_No = newNum();

            //绑定下拉列表(职称)
            List<SelectListItem> departs = new List<SelectListItem>();
            //SelectListItem sli0 = new SelectListItem() { Text = "请选择", Value = "0" };
            //departs.Add(sli0);
            foreach (var item in sme.StaffType)
            {
                SelectListItem sli = new SelectListItem() { Value = item.ST_No, Text = item.ST_Name.ToString() };
                departs.Add(sli);
            }
            ViewData["ColMan"] = departs;

            //绑定下拉列表(性别)
            List<SelectListItem> sex = new List<SelectListItem>();
            SelectListItem sex0 = new SelectListItem() { Text = "男", Value = "男" };
            SelectListItem sex1 = new SelectListItem() { Text = "女", Value = "女" };
            sex.Add(sex0);
            sex.Add(sex1);
            ViewData["Sex"] = sex;

            //验证(姓名验证，只能为2-4个中文)
            string xingming = @"^[\u4E00-\u9FA5]{2,4}$";
            Regex regeXM = new Regex(xingming);

            //验证(手机验证)
            //电信手机号码正则表达式
            string dianxin = @"^1[3578][01379]\d{8}$";
            Regex regeDX = new Regex(dianxin);

            //联通手机号码正则表达式
            string liantong = @"^1[34578][01256]\d{8}";
            Regex regexLT = new Regex(liantong);

            //移动手机号码正则表达式
            string yidong = @"^(1[012345678])\d{8}|1[345678][0123456789]\d{8}$";
            Regex regexYD = new Regex(yidong);

            //验证(邮箱验证)
            string email = @"^[A-Za-z0-9\u4e00-\u9fa5]+@[a-zA-Z0-9_-]+(\.[a-zA-Z0-9_-]+)+$";
            Regex regeEM = new Regex(email);

            //验证(QQ)
            string QQ = @"^[1-9][0-9]{4,12}$";
            Regex regexQQ = new Regex(QQ);

            //记住生日
            if (s.Sta_Birthday.Year != 1)
            {
                Session["SessionBirthday"] = s.Sta_Birthday;
                ViewBag.SessionBirthday = Session["SessionBirthday"];
            }
            else
            {
                Session["SessionBirthday"] = null;
                ViewBag.SessionBirthday = Session["SessionBirthday"];
            }

            //记住入职时间
            if (s.Sta_Indate.Year != 1)
            {
                Session["SessionIndate"] = s.Sta_Indate;
                ViewBag.SessionIndate = Session["SessionIndate"];
            }
            else
            {
                Session["SessionIndate"] = null;
                ViewBag.SessionIndate = Session["SessionIndate"];
            }

            //记住地址
            Session["SessionAddress"] = s.Sta_Address;
            ViewBag.SessionAddress = Session["SessionAddress"];

            if (string.IsNullOrEmpty(s.Sta_Name))
            {
                Response.Write("<script>alert('姓名不能为空')</script>");
            }
            else if (!regeXM.IsMatch(s.Sta_Name))
            {
                Response.Write("<script>alert('姓名格式不正确,请输入2-4个汉字。')</script>");
            }
            else if (s.Sta_Birthday.Year == 1)
            {
                Response.Write("<script>alert('出生年月不能为空')</script>");

            }
            else if (s.Sta_Indate.Year == 1)
            {
                Response.Write("<script>alert('入职时间不能为空')</script>");
            }
            else if (string.IsNullOrEmpty(s.Sta_Telephone))
            {
                Response.Write("<script>alert('电话不能为空')</script>");
            }
            else if (!(regeDX.IsMatch(s.Sta_Telephone) || regexLT.IsMatch(s.Sta_Telephone) || regexYD.IsMatch(s.Sta_Telephone)))
            {
                Response.Write("<script>alert('电话格式不正确')</script>");
            }
            else if (string.IsNullOrEmpty(s.Sta_Address))
            {
                Response.Write("<script>alert('地址不能为空')</script>");
            }
            else if (s.Sta_Email != null && !regeEM.IsMatch(s.Sta_Email))
            {
                Response.Write("<script>alert('邮箱格式不正确')</script>");
            }
            else if (s.Sta_QQ != null && !regexQQ.IsMatch(s.Sta_QQ))
            {
                Response.Write("<script>alert('QQ格式不正确')</script>");
            }
            else
            {
                var staff = new Staff();
                staff.Sta_No = Sta_No;
                staff.Sta_Name = s.Sta_Name;
                staff.Sta_Sex = Sex;
                staff.Sta_Birthday = s.Sta_Birthday;
                staff.Sta_Type = ColMan;
                staff.Sta_Telephone = s.Sta_Telephone;
                staff.Sta_QQ = s.Sta_QQ;
                staff.Sta_Address = s.Sta_Address;
                staff.Sta_Indate = s.Sta_Indate;
                staff.Sta_Email = s.Sta_Email;
                staff.IsDel = 0;
                staff.Remark = s.Remark;

                sme.Entry<Staff>(staff).State = System.Data.Entity.EntityState.Added;
                if (sme.SaveChanges() > 0)
                {
                    Response.Write("<script>alert('成功');</script>");
                    return Content("<script>parent.window.close()</script>");
                }
                else
                {
                    Response.Write("<script>alert('失败');</script>");
                }
            }
            return View();
        }

        public ActionResult add(string ColMan, string Sex)
        {
            //绑定下拉列表(职称)
            List<SelectListItem> departs = new List<SelectListItem>();
            SelectListItem sli0 = new SelectListItem() { Text = "请选择", Value = "0" };
            departs.Add(sli0);
            foreach (var item in sme.StaffType)
            {
                SelectListItem sli = new SelectListItem() { Value = item.ST_No, Text = item.ST_Name.ToString() };
                departs.Add(sli);
            }
            ViewData["ColMan"] = departs;

            //绑定下拉列表(性别)
            List<SelectListItem> sex = new List<SelectListItem>();
            SelectListItem sex0 = new SelectListItem() { Text = "男", Value = "男" };
            SelectListItem sex1 = new SelectListItem() { Text = "女", Value = "女" };
            sex.Add(sex0);
            sex.Add(sex1);
            ViewData["Sex"] = sex;


            var staff = new Staff();

            return View();
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult delete(string StaNo)
        {
            var Sta_No = StaNo;
            var dInfo = sme.Staff.Find(Sta_No);
            dInfo.IsDel = 1;
            sme.Entry<Staff>(dInfo).State = System.Data.Entity.EntityState.Modified;
            if (sme.SaveChanges() > 0)
            {
                sme.SaveChanges();
                Response.Write("<script>alert('删除成功');</script>");
                this.Index();
            }
            else
            {
                Response.Write("<script>alert('删除失败');</script>");
            }
            return RedirectToAction("Index");

        }

        [HttpPost]
        /// <summary>
        /// 查询
        /// </summary>
        /// <returns></returns>
        public ActionResult Index(string name)
        {
            var result = from a in sme.Staff
                         join b in sme.StaffType on a.Sta_Type equals b.ST_No
                         where b.ST_No != "ST004" && b.ST_No != "ST006" && a.IsDel == 0
                         select new
                         {
                             StaNo = a.Sta_No,
                             StaName = a.Sta_Name,
                             Sex = a.Sta_Sex,
                             BormDate = a.Sta_Birthday,
                             Type = b.ST_Name,
                             Phone = a.Sta_Telephone,
                             QQ = a.Sta_QQ,
                             Address = a.Sta_Address,
                             InDate = a.Sta_Indate,
                             Email = a.Sta_Email,
                             Remark = a.Remark
                         };

            result = result.Where(a => a.StaName.Contains(name));
            List<dynamic> dnc = new List<dynamic>();
            foreach (var item in result.ToList())
            {
                dynamic d = new ExpandoObject();
                d.StaNo = item.StaNo;
                d.StaName = item.StaName;
                d.Sex = item.Sex;
                d.BornDate = item.BormDate;
                d.Type = item.Type;
                d.Phone = item.Phone;
                d.QQ = item.QQ;
                d.Address = item.Address;
                d.Indate = item.InDate;
                d.Email = item.Email;
                d.Remark = item.Remark;
                dnc.Add(d);
            }

            ViewBag.Data = dnc;
            return View();
        }
    }
}