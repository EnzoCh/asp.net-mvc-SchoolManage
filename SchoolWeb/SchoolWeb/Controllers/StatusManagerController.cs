using SchoolWeb.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace jiaowuguanliMVC.Controllers
{
    public class StatusManagerController : Controller
    {
        /// <summary>
        /// 学籍管理
        /// 作者:曾梓豪
        /// 创建时间:2020-7-08 17:17:30
        /// </summary>
        SchoolManageEntities6 sm = new SchoolManageEntities6();
        // GET: StatusManager
        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="Index"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Index(string Index)
        {


            var result = from a in sm.Status
                         select a;
            result = result.Where(a => a.Name.Contains(Index));
            return View(result);
        }
        /// <summary>
        /// 加载
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            var result = from a in sm.Status
                         select a;
            return View(result);
        }


        /// <summary>
        /// 获得一个编码 准考证号
        /// </summary>
        /// <returns></returns>
        public string newNum()
        {
            var results = from a in sm.Status
                          select a;
            Status sr = results.ToList().LastOrDefault();
            int num = Convert.ToInt32(sr.AdmissionNumber);
            num += 1;
            if (num < 10)
            {
                return "00000" + num;
            }
            else if (num >= 10 && num < 100)
            {
                return "0000" + num;
            }
            else if (num >= 100 && num < 1000)
            {
                return "000" + num;
            }
            else if (num >= 1000 && num < 10000)
            {
                return "00" + num;
            }
            else if (num >= 10000 && num < 100000)
            {
                return "0" + num;
            }
            else
            {
                return num.ToString();
            }
        }

        /// <summary>
        /// 获得一个编码  考生号
        /// </summary>
        /// <returns></returns>
        public string newNum2()
        {
            var results = from a in sm.Status
                          select a;
            Status sr = results.ToList().LastOrDefault();
            int num = Convert.ToInt32(sr.CandidateNumber);
            num += 1;
            if (num < 10)
            {
                return "185100000" + num;
            }
            else if (num >= 10 && num < 100)
            {
                return "18510000" + num;
            }
            else if (num >= 100 && num < 1000)
            {
                return "1851000" + num;
            }
            else if (num >= 1000 && num < 10000)
            {
                return "185100" + num;
            }
            else if (num >= 10000 && num < 100000)
            {
                return "18510" + num;
            }
            else
            {
                return num.ToString();
            }
        }


        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Add(Status m, string numA, string numB, string StuNo)
        {
            List<SelectListItem> departs = new List<SelectListItem>();
            SelectListItem sli0 = new SelectListItem() { Text = "请选择", Value = "0" };
            departs.Add(sli0);
            foreach (var item in sm.Student)
            {
                SelectListItem sli = new SelectListItem() { Value = item.S_No.ToString(), Text = item.S_Name };
                departs.Add(sli);

            }
            ViewData["StuNo"] = departs;

            //判断姓名是否为空
            if (StuNo == "0")
            {
                return Content("<script>alert('名称不能为空');history.go(-1);</script>");
            }
            //判断信息是否完整
            if (string.IsNullOrEmpty(m.Sex) || string.IsNullOrEmpty(m.LandScape) || string.IsNullOrEmpty(m.ContactPerson) || string.IsNullOrEmpty(m.ContactAddress) || string.IsNullOrEmpty(m.Type) || string.IsNullOrEmpty(m.Province) || string.IsNullOrEmpty(m.Year) || string.IsNullOrEmpty(m.ModeAdmission))
            {
                return Content("<script>alert('请填写完整信息');history.go(-1);</script>");
            }
            //身份证正则验证  (15位、18位数字)
            Regex rgx = new Regex(@"(^\d{15}|\d{18}$)");
            if (!rgx.IsMatch(m.IdCard))
            {
                return Content("<script> alert('请输入正确的身份证号');history.go(-1);</script>");
            }
            //手机号正则验证  (11位数字)
            Regex rgxx = new Regex(@"(^\d{11}$)");
            if (!rgxx.IsMatch(m.ContactNumber))
            {
                return Content("<script> alert('请输入正确的手机号');history.go(-1);</script>");
            }
            //姓名正则验证
            Regex rgxxs = new Regex(@"(^[\u4e00-\u9fa5]{0,}$)");
            if (!rgxxs.IsMatch(m.Sex))
            {
                return Content("<script> alert('请输入正确的性别');history.go(-1);</script>");
            }
            //姓名正则验证

            if (!rgxxs.IsMatch(m.LandScape))
            {
                return Content("<script> alert('请输入正确的政治面貌');history.go(-1);</script>");
            }
            //姓名正则验证

            if (!rgxxs.IsMatch(m.ContactPerson))
            {
                return Content("<script> alert('请输入正确的联系人');history.go(-1);</script>");
            }

            //姓名正则验证

            if (!rgxxs.IsMatch(m.Type))
            {
                return Content("<script> alert('请输入正确的考生类别');history.go(-1);</script>");
            }
            //姓名正则验证

            if (!rgxxs.IsMatch(m.Province))
            {
                return Content("<script> alert('请输入正确的生源省份');history.go(-1);</script>");
            }
            //姓名正则验证

            if (!rgxxs.IsMatch(m.ModeAdmission))
            {
                return Content("<script> alert('请输入正确的入学方式');history.go(-1);</script>");
            }

            var result = from a in sm.Student
                         where a.S_No == StuNo
                         select a.S_Name;
            string sname = result.ToList().Last();
            

            //考生号
            numA = newNum();
            numB = newNum2();




            var sta = new Status();
            sta.Name = sname;
            sta.Sex = m.Sex;
            sta.IdCard = m.IdCard;
            sta.LandScape = m.LandScape;
            sta.ContactAddress = m.ContactAddress;
            sta.ContactNumber = m.ContactNumber;
            sta.ContactPerson = m.ContactPerson;
            sta.AdmissionNumber = numA;
            sta.CandidateNumber = numB;
            sta.Type = m.Type;
            sta.Province = m.Province;
            sta.Year = m.Year;
            sta.ModeAdmission = m.ModeAdmission;
            sta.Reamrk = m.Reamrk;
            sta.ImagePath = "/images/NoPic.jpg";
            sta.S_No = StuNo;
            sm.Entry<Status>(sta).State = System.Data.Entity.EntityState.Added;
            if (sm.SaveChanges() > 0)
            {
                Response.Write("<script>alert('成功')</script>");
            }
            else
            {
                Response.Write("<script>alert('失败')</script>");
            }

            return View();
        }

        public ActionResult Add()
        {
            List<SelectListItem> departs = new List<SelectListItem>();
            SelectListItem sli0 = new SelectListItem() { Text = "请选择", Value = "0" };
            departs.Add(sli0);
            foreach (var item in sm.Student)
            {
                SelectListItem sli = new SelectListItem() { Value = item.S_No.ToString(), Text = item.S_Name };
                departs.Add(sli);
            }
            ViewData["StuNo"] = departs;

            
            return View();
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        public ActionResult delete(int id)
        {
            var dinfo = sm.Status.Find(id);

            sm.Entry<Status>(dinfo).State = System.Data.Entity.EntityState.Deleted;
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
        public ActionResult update(Status m)
        {

            HttpPostedFileBase files = Request.Files[0];
            var sta = new Status();
            var fileName = files.FileName;
            var filePath = Server.MapPath("~/images/");
            if (fileName == "")
            {
                fileName = m.ImagePath.Substring(8);
            }
            files.SaveAs(Path.Combine(filePath, fileName));
            //判断信息是否完整
            if (string.IsNullOrEmpty(m.Sex) || string.IsNullOrEmpty(m.LandScape) || string.IsNullOrEmpty(m.ContactPerson) || string.IsNullOrEmpty(m.ContactAddress) || string.IsNullOrEmpty(m.Type) || string.IsNullOrEmpty(m.Province) || string.IsNullOrEmpty(m.Year) || string.IsNullOrEmpty(m.ModeAdmission) || string.IsNullOrEmpty(m.ContactNumber) || string.IsNullOrEmpty(m.IdCard))
            {
                return Content("<script>alert('请填写完整信息');history.go(-1);</script>");
            }
            //身份证正则验证  (15位、18位数字)
            Regex rgx = new Regex(@"(^\d{15}|\d{18}$)");
            if (!rgx.IsMatch(m.IdCard))
            {
                return Content("<script> alert('请输入正确的身份证号');history.go(-1);</script>");
            }
            //手机号正则验证  (11位数字)
            Regex rgxx = new Regex(@"(^\d{11}$)");
            if (!rgxx.IsMatch(m.ContactNumber))
            {
                return Content("<script> alert('请输入正确的手机号');history.go(-1);</script>");
            }

            sta.ID = m.ID;
            sta.Name = m.Name;
            sta.Sex = m.Sex;
            sta.IdCard = m.IdCard;
            sta.LandScape = m.LandScape;
            sta.ContactAddress = m.ContactAddress;
            sta.ContactNumber = m.ContactNumber;
            sta.ContactPerson = m.ContactPerson;
            sta.AdmissionNumber = m.AdmissionNumber;
            sta.CandidateNumber = m.CandidateNumber;
            sta.Type = m.Type;
            sta.Province = m.Province;
            sta.Year = m.Year;
            sta.ModeAdmission = m.ModeAdmission;
            sta.Reamrk = m.Reamrk;
            sta.ImagePath = "/images/" + fileName;
            sta.S_No = m.S_No;
            sm.Entry<Status>(sta).State = System.Data.Entity.EntityState.Modified;
            if (sm.SaveChanges() > 0)
            {
                Response.Write("<script>alert('修改成功')</script>");
                return Content("<script>window.close()</script>");
            }
            else
            {
                return Content("<script>alert('失败');</script>");
            }
            
        }

        public ActionResult update(int id)
        {
            var depart = from a in sm.Status
                         where a.ID == id
                         select a;
            Status sd = depart.ToList().LastOrDefault();
            return View(sd);
        }
    }
}