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
    public class RoleTypeController : Controller
    {
        /// <summary>
        /// 角色管理
        /// 作者:曾梓豪
        /// 创建时间:2020-7-08 17:17:30
        /// </summary>
        SchoolManageEntities6 sm = new SchoolManageEntities6();
        // GET: RoleType
        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="userInfo"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Index(string userInfo)
        {
            //绑定下拉列表
            //用户类型
            List<SelectListItem> departs = new List<SelectListItem>();
            SelectListItem sli0 = new SelectListItem() { Text = "请选择用户类型", Value = "0" };
            departs.Add(sli0);
            foreach (var item in sm.LoginType)//原UserType
            {
                SelectListItem sli = new SelectListItem() { Value = item.id.ToString(), Text = item.Name };
                departs.Add(sli);
            }
            ViewData["UserType"] = departs;

            //学生
            List<SelectListItem> departss = new List<SelectListItem>();
            SelectListItem slii0 = new SelectListItem() { Text = "请选择", Value = "0" };
            departss.Add(slii0);
            foreach (var item in sm.Student)
            {
                SelectListItem slii = new SelectListItem() { Value = item.S_No.ToString(), Text = item.S_Name };
                departss.Add(slii);
            }
            ViewData["StuName"] = departss;

            //老师
            List<SelectListItem> eeeesssa = new List<SelectListItem>();
            SelectListItem sli00 = new SelectListItem() { Text = "请选择", Value = "0" };
            eeeesssa.Add(sli00);
            foreach (var item in sm.Staff.Where(a => a.Sta_Type == "ST001"))
            {
                SelectListItem sli1 = new SelectListItem() { Value = item.Sta_No.ToString(), Text = item.Sta_Name };
                eeeesssa.Add(sli1);
            }
            ViewData["TeaName"] = eeeesssa;

            var result = from a in sm.LoginInfo
                         join b in sm.LoginType on a.Type equals b.id //原UserType
                         where a.IsDel == 0
                         select new
                         {
                             ID = a.L_id,
                             RealName = a.RealName,
                             Password = a.Password,
                             Type = a.Type,
                             UserType = b.Name,
                             Remark = a.Remark,
                             CreateTime=a.L_CreateTime,
                             LoginName=a.LoginName,

                         };
            //单独赋值给一个变量
            //string str = Request.Form["UserType"].ToString();
            ////判断下拉列表是否选中
            //if (Request.Form["UserType"].ToString() != "0")
            //{
            //    result = result.Where(a => a.Type.ToString().Contains(str));
            //}
            result = result.Where(a => a.LoginName.Contains(userInfo) || a.RealName.Contains(userInfo));
            //多表查询
            List<dynamic> dnc = new List<dynamic>();
            foreach (var item in result.ToList())
            {
                dynamic d = new ExpandoObject();
                d.ID = item.ID;
                d.RealName = item.RealName;
                d.LoginName = item.LoginName;
                d.Password = item.Password;
                d.Type = item.UserType;
                d.CreateTime = item.CreateTime;
                d.Remark = item.Remark;
                dnc.Add(d);
            }
            ViewBag.Data = dnc;
            return View();
            
            
        }

        public ActionResult Index()
        {
            //绑定下拉列表
            //用户类型
            List<SelectListItem> departs = new List<SelectListItem>();
            SelectListItem sli0 = new SelectListItem() { Text = "请选择用户类型", Value = "0" };
            departs.Add(sli0);
            foreach (var item in sm.LoginType)//原UserType
            {
                SelectListItem sli = new SelectListItem() { Value = item.id.ToString(), Text = item.Name };
                departs.Add(sli);
            }
            ViewData["UserType"] = departs;

            //学生
            List<SelectListItem> asdasdas = new List<SelectListItem>();
            SelectListItem sli01 = new SelectListItem() { Text = "请选择", Value = "0" };
            asdasdas.Add(sli01);
            foreach (var item in sm.Student)
            {
                SelectListItem sliii = new SelectListItem() { Value = item.S_No.ToString(), Text = item.S_Name };
                asdasdas.Add(sliii);
            }
            ViewData["StuName"] = asdasdas;

            //老师
            List<SelectListItem> eeeesssa = new List<SelectListItem>();
            SelectListItem sli00 = new SelectListItem() { Text = "请选择", Value = "0" };
            eeeesssa.Add(sli00);
            foreach (var item in sm.Staff.Where(a => a.Sta_Type == "ST001"))
            {
                SelectListItem sli1 = new SelectListItem() { Value = item.Sta_No.ToString(), Text = item.Sta_Name };
                eeeesssa.Add(sli1);
            }
            ViewData["TeaName"] = eeeesssa;
           
            var result = from a in sm.LoginInfo
                         join b in sm.LoginType on a.Type equals b.id //原UserType
                         where a.IsDel == 0
                         select new
                         {
                             ID = a.L_id,
                             RealName = a.RealName,
                             Password = a.Password,
                             Type = a.Type,
                             UserType = b.Name,
                             Remark = a.Remark,
                             CreateTime = a.L_CreateTime,
                             LoginName = a.LoginName,

                         };
            List<dynamic> dnc = new List<dynamic>();
            foreach (var item in result.ToList())
            {
                dynamic d = new ExpandoObject();
                d.ID = item.ID;
                d.RealName = item.RealName;
                d.LoginName = item.LoginName;
                d.Password = item.Password;
                d.Type = item.UserType;
                d.CreateTime = item.CreateTime;
                d.Remark = item.Remark;

                dnc.Add(d);
            }
            ViewBag.Data = dnc;
            return View();
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="LoginName"></param>
        /// <param name="Password"></param>
        /// <param name="RealName"></param>
        /// <param name="UserType"></param>
        /// <param name="Remark"></param>
        /// <returns></returns>
        public ActionResult add(string LoginName, string Password, string RealName, string UserType, string Remark, string ClassName, string StuName, string TeaName)
        {
            //学生
            List<SelectListItem> departs = new List<SelectListItem>();
            SelectListItem sli0 = new SelectListItem() { Text = "请选择", Value = "0" };
            departs.Add(sli0);
            foreach (var item in sm.Student)
            {
                SelectListItem sli = new SelectListItem() { Value = item.S_No.ToString(), Text = item.S_Name };
                departs.Add(sli);
            }
            ViewData["StuName"] = departs;

            //老师
            List<SelectListItem> eeeesssa = new List<SelectListItem>();
            SelectListItem sli00 = new SelectListItem() { Text = "请选择", Value = "0" };
            eeeesssa.Add(sli00);
            foreach (var item in sm.Staff.Where(a => a.Sta_Type == "ST001"))
            {
                SelectListItem sli1 = new SelectListItem() { Value = item.Sta_No.ToString(), Text = item.Sta_Name };
                eeeesssa.Add(sli1);
            }
            ViewData["TeaName"] = eeeesssa;






            var exists = from a in sm.LoginInfo
                         where a.LoginName == LoginName
                         select a;
            //用户名或密码是否为空
            if (string.IsNullOrEmpty(LoginName) || string.IsNullOrEmpty(Password))
            {
                return Content("<script> alert('用户名或密码不能为空');history.go(-1);</script>");
            }
            //用户名正则验证 (字母开头，允许5-16字节，允许字母数字下划线)
            //Regex rgx = new Regex(@"(^[a-zA-Z][a-zA-Z0-9_]{4,15}$)");
            //if (!rgx.IsMatch(LoginName))
            //{
            //    return Content("<script> alert('用户名请以字母开头且不少于5位');history.go(-1);</script>");
            //}
            //密码正则验证 （长度在6~18之间）
            //Regex rgxx = new Regex(@"(^{5,17}$)");
            //if (!rgxx.IsMatch(Password))
            //{
            //    return Content("<script> alert('密码不少于6位');history.go(-1);</script>");
            //}
            //姓名是否为空
            if (string.IsNullOrEmpty(RealName) && StuName == "0" && TeaName == "0")
            {
                return Content("<script> alert('姓名不能为空');history.go(-1);</script>");
            }
            //姓名正则验证
            Regex rgxxs = new Regex(@"(^[\u4e00-\u9fa5]{0,}$)");
            if (RealName != null && !rgxxs.IsMatch(RealName))
            {
                return Content("<script> alert('请输入正确的名字');history.go(-1);</script>");
            }
            //用户类型是否为空验证
            if (UserType == "0")
            {
                return Content("<script> alert('请重新选择用户类型');history.go(-1);</script>");
            }
            //验证是否重复
            if (exists.ToList().Count > 0)
            {
                return Content("<script>alert('已经存在相同数据');history.go(-1);</script>");
            }
            else
            {
                //新增
                var login = new LoginInfo();

                //判断姓名
                if (RealName != null)
                {
                    login.RealName = RealName;
                }
                if (StuName != null)
                {
                    var result1 = from a in sm.Student
                                  where a.S_No == StuName
                                  select a.S_Name;
                    string stuname = result1.ToList().Last();

                    login.RealName = stuname;
                    login.StuNo = StuName;
                }
                if (TeaName != null)
                {
                    var result2 = from a in sm.Staff
                                  where a.Sta_No == TeaName
                                  select a.Sta_Name;
                    string teaname = result2.ToList().Last();

                    login.RealName = teaname;
                    login.StaNo = TeaName;
                }

                login.LoginName = LoginName;
                login.Password = Password;
                login.Type = Convert.ToInt32(UserType);
                login.Remark = Remark;
                login.IsDel = 0;
                login.L_CreateTime = DateTime.Now;
                sm.Entry<LoginInfo>(login).State = System.Data.Entity.EntityState.Added;
                sm.SaveChanges();
            }
            return RedirectToAction("Index");


        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        public ActionResult delete(int id)
        {
            LoginInfo login = sm.LoginInfo.Where(c => c.L_id == id).FirstOrDefault();//先查找出要修改的对象
            login.IsDel = 1;//修改数据

            if (sm.SaveChanges() > 0)
            {
                Response.Write("<script>alert('删除成功')</script>");
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
        public ActionResult update(LoginInfo lg, string UserType)
        {
            //绑定下拉列表
            //教师
            List<SelectListItem> departs = new List<SelectListItem>();
            SelectListItem sli0 = new SelectListItem() { Text = "请选择", Value = "0" };
            departs.Add(sli0);
            foreach (var item in sm.LoginType)//原UserType
            {
                SelectListItem sli = new SelectListItem() { Value = item.id.ToString(), Text = item.Name };
                departs.Add(sli);
            }
            ViewData["UserType"] = departs;

            //密码正则验证 （以字母开头，长度在6~18之间，只能包含字母、数字和下划线）
            //Regex rgxx = new Regex(@"(^[a-zA-Z]\w{5,17}$)");
            //if (!rgxx.IsMatch(lg.Password))
            //{
            //    return Content("<script> alert('密码不少于6位且以字母开头');history.go(-1);</script>");
            //}

            var login = new LoginInfo ();
            login.L_id = lg.L_id;
            login.LoginName = lg.LoginName;
            login.Password = lg.Password;
            login.RealName = lg.RealName;
            login.Type = lg.Type;
            login.Remark = lg.Remark;
            login.IsDel = 0;
            login.L_CreateTime = DateTime.Now;
            
            sm.Entry<LoginInfo>(login).State = System.Data.Entity.EntityState.Modified;
            
            if (sm.SaveChanges() > 0)
            {
                Response.Write("<script>alert('修改成功')</script>");
            }
            else
            {
                Response.Write("<script>alert('修改失败')</script>");
            }
            return Content("<script>window.close()</script>");

        }

        public ActionResult update(int id)
        {
            var depart = from a in sm.LoginInfo
                         where a.L_id == id
                         select a;
            LoginInfo sd = depart.ToList().LastOrDefault();

            //绑定下拉列表
            //教师
            List<SelectListItem> departs = new List<SelectListItem>();
            SelectListItem sli0 = new SelectListItem() { Text = "请选择", Value = "0" };
            departs.Add(sli0);
            foreach (var item in sm.LoginType)//原UserType
            {
                
                SelectListItem sli = new SelectListItem() { Value = item.id.ToString(), Text = item.Name };
                departs.Add(sli);
            }
            ViewData["UserType"] = departs;
            return View(sd);
        }
    }
}