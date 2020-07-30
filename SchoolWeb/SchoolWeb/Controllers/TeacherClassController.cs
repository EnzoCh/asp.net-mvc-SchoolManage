using SchoolWeb.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Drawing;
using System.Drawing.Imaging;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.DataVisualization.Charting;

namespace czwshixun.Controllers
{
    public class TeacherClassController : Controller
    {
        /// <summary>
        /// 打分
        /// 作者:陈忠文
        /// 创建时间:2020-7-08 17:17:30
        /// </summary>
        SchoolManageEntities6 sme = new SchoolManageEntities6();

        // GET: TeacherClass

        /// 添加成绩
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult update(Score sco)
        {
            string studentNo = Request.Form["studentNo"];
            var chage = from a in sme.Score
                        where a.Sco_StudentNo == studentNo
                        select a;

            Score sc = chage.AsNoTracking().ToList().LastOrDefault();

            string stuscore = Request.Form["stuscore"];
            var score = new Score();
            score.Sco_id = sc.Sco_id;
            score.Sco_StudentNo = sc.Sco_StudentNo;
            score.Sco_score =Convert.ToDouble(stuscore);
            score.Sco_CourseNo = sc.Sco_CourseNo;
            score.Sco_ExamNo = sc.Sco_ExamNo;
            score.IsDel = 0;

            sme.Entry<Score>(score).State = System.Data.Entity.EntityState.Modified;
            if (sme.SaveChanges() > 0)
            {
                Response.Write("<script>alert('修改成功');</script>");
            }
            else
            {
                Response.Write("<script>alert('修改失败');</script>");
            }
            return View();
        }

        public ActionResult update(string Sco_StudentNo)
        {
            var res = from a in sme.Score
                         select a;

            var result = from a in sme.Score
                         join b in sme.Student on a.Sco_StudentNo equals b.S_No
                         join c in sme.ExamInfo on a.Sco_ExamNo equals c.Em_No
                         where a.IsDel == 0 && a.Sco_StudentNo==Sco_StudentNo
                         select new
                         {
                             Sco_StudentNo = a.Sco_StudentNo,
                             Sco_Score = a.Sco_score,
                             Sco_Name = b.S_Name,
                             Exam_Name = c.Em_Name
                         };

            List<dynamic> dnc = new List<dynamic>();
            foreach (var item in result.ToList())
            {
                dynamic d = new ExpandoObject();
                d.Sco_StudentNo = item.Sco_StudentNo;
                d.Sco_Score = item.Sco_Score.ToString();
                d.Sco_Name = item.Sco_Name;
                d.Exam_Name = item.Exam_Name;
                dnc.Add(d);
            }
            ViewBag.Data = dnc;
            return View(res);
        }


        /// <summary>
        /// 已选该课程学生名单查询
        /// </summary>
        /// <param name="Cou_No"></param>
        /// <returns></returns>
        public ActionResult ClassStudent(string Cou_No)
        {
            var result = from a in sme.Score
                         join b in sme.Student on a.Sco_StudentNo equals b.S_No
                         join c in sme.ExamInfo on a.Sco_ExamNo equals c.Em_No
                         where a.IsDel == 0 && a.Sco_CourseNo == Cou_No
                         select new
                         {
                             Sco_StudentNo = a.Sco_StudentNo,
                             Sco_Score = a.Sco_score,
                             Sco_Name = b.S_Name,
                             Exam_Name = c.Em_Name
                         };

            List<dynamic> dnc = new List<dynamic>();
            foreach (var item in result.ToList())
            {
                dynamic d = new ExpandoObject();
                d.Sco_StudentNo = item.Sco_StudentNo;
                d.Sco_Score= item.Sco_Score.ToString();
                d.Sco_Name = item.Sco_Name;
                d.Exam_Name = item.Exam_Name;
                dnc.Add(d);
            }
            ViewBag.Data = dnc;
            return View();
        }


        /// <summary>
        /// 我教授的课程查询
        /// </summary>
        /// <param name="Cou_Name"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Index(string Cou_Name)
        {
            string couNo = (Session["LoginUser"] as LoginInfo).StaNo;
            var result = from a in sme.CourseInfo
                         where a.IsDel == 0 && a.Cou_Teacher == couNo
                         select a;
            result = result.Where(a => a.Cou_Name.Contains(Cou_Name));
            return View(result);
        }

        public ActionResult Index()
        {
            string couNo = (Session["LoginUser"] as LoginInfo).StaNo;
            var result = from a in sme.CourseInfo
                         where a.IsDel == 0 && a.Cou_Teacher==couNo
                         select a;
            return View(result);
        }

    }
}