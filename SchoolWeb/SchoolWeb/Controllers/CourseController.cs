using Microsoft.Office.Interop.Excel;
using PagedList;
using SchoolWeb.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace SchoolManage.Controllers
{
    public class CourseController : Controller
    {
        SchoolManageEntities6 db = new SchoolManageEntities6();

        /// <summary>
        /// 选课
        /// 作者:刘洋
        /// 创建时间:2020-7-08 17:17:30
        /// </summary>

        public static List<CourseInfo> source = new List<CourseInfo>();

        /// <summary>
        /// 泛型转为table
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        private static System.Data.DataTable ListToDataTable(IList list)
        {
            //定义一个table
            System.Data.DataTable result = new System.Data.DataTable();
            //判断传入的泛型有没有数据
            if (list.Count > 0)
            {
                //获得第一个泛型的所有公共属性集合
                PropertyInfo[] propertys = list[0].GetType().GetProperties();
                //通过公共属性设置table的表结构
                foreach (PropertyInfo pi in propertys)
                {
                    result.Columns.Add(pi.Name, pi.PropertyType);
                }
                //遍历泛型
                foreach (object t in list)
                {
                    //遍历属性
                    ArrayList templist = new ArrayList();
                    foreach (PropertyInfo pi in propertys)
                    {
                        //把值放在arraylist
                        object obj = pi.GetValue(t, null);
                        templist.Add(obj);
                    }
                    //把arraylist的数据弄成一个object数组
                    object[] array = templist.ToArray();
                    //然后把object数组里面的数据放在table里
                    result.LoadDataRow(array, true);
                }
            }
            return result;//返回table
        }

        /// <summary>
        /// 导出excel
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="fileName"></param>
        public void ExportExcel(System.Data.DataTable dt, string fileName)
        {

            Microsoft.Office.Interop.Excel.Application excelApp = new Microsoft.Office.Interop.Excel.Application();
            Workbook excelIWB = excelApp.Workbooks.Add(System.Type.Missing);//创建工作簿
            Worksheet excelWS = (Worksheet)excelIWB.Worksheets[1];//创建工作表

            for (int k = 0; k < dt.Columns.Count; k++)
            {
                excelWS.Cells[1, k + 1] = dt.Columns[k].Caption;
            }
            //将数据导入到工作表的单元格
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    excelWS.Cells[i + 2, j + 1] = dt.Rows[i][j].ToString();//excel单元格第一个从索引1开始
                }
            }
            excelIWB.SaveAs("D:\\" + fileName + ".xlsx");//保存到指定路径
            excelIWB.Close();
            excelApp.Quit();

        }
        public ActionResult excelOut()
        {
            ExportExcel(ListToDataTable(CourseController.source), "课程信息");
            return Redirect("Index");
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult delete(int? id)
        {
            var cInfo = db.CourseInfo.Find(id);
            cInfo.IsDel = 1;
            db.Entry<CourseInfo>(cInfo).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        /// <summary>
        /// 查询数据
        /// </summary>
        /// <param name="courseInfo"></param>
        /// <param name="PageNum"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Index(string courseInfo)
        {
            var results = from a in db.CourseInfo
                          join b in db.Staff on a.Cou_Teacher equals b.Sta_No
                          where a.IsDel == 0
                          select new
                          {
                              CouId = a.Cou_id,
                              StaNO = a.Cou_Type,
                              StaName = b.Sta_Name,
                              CouNo = a.Cou_No,
                              CouName = a.Cou_Name,
                              CouType = a.Cou_Type
                          };
            results = results.Where(a => a.CouName.Contains(courseInfo));
            List<dynamic> dnc = new List<dynamic>();
            foreach (var item in results.ToList())
            {
                dynamic d = new ExpandoObject();
                d.CouId = item.CouId;
                d.StaNO = item.StaNO;
                d.StaName = item.StaName;
                d.CouNo = item.CouNo;
                d.CouName = item.CouName;
                d.CouType = item.CouType;
                dnc.Add(d);
            };
            ViewBag.Data = dnc;
            return View();
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            var results = from a in db.CourseInfo
                          join b in db.Staff on a.Cou_Teacher equals b.Sta_No
                          where a.IsDel == 0
                          select new
                          {
                              CouId = a.Cou_id,
                              StaNO = a.Cou_Type,
                              StaName = b.Sta_Name,
                              CouNo = a.Cou_No,
                              CouName = a.Cou_Name,
                              CouType = a.Cou_Type
                          };
            List<dynamic> dnc = new List<dynamic>();
            foreach (var item in results.ToList())
            {
                dynamic d = new ExpandoObject();
                d.CouId = item.CouId;
                d.StaNO = item.StaNO;
                d.StaName = item.StaName;
                d.CouNo = item.CouNo;
                d.CouName = item.CouName;
                d.CouType = item.CouType;
                dnc.Add(d);
            }
            ViewBag.Data = dnc;
            return View();

        }

        /// <summary>
        /// 选课
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult selectcourse()
        {
            string stuno = Session["stuNo"].ToString();
            string courseid = Request.Form["courseNo"];
            var oldselcourse = from a in db.SelectCourse
                               where a.SC_StudentNo == stuno && a.SC_CourseNo == courseid
                               select a;
            if (oldselcourse.ToList().Count()==0)
            {
                SelectCourse scou = new SelectCourse();
                scou.SC_CourseNo = courseid;
                scou.SC_StudentNo = stuno;
                scou.SC_Time = DateTime.Now;
                scou.IsDel = 0;
                db.Entry<SelectCourse>(scou).State = System.Data.Entity.EntityState.Added;
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        public ActionResult selectcourse(string id)
        {
            
              var results = from a in db.CourseInfo
                          join b in db.Staff on a.Cou_Teacher equals b.Sta_No
                          where a.IsDel == 0
                          select new
                          {
                              CouId = a.Cou_id,
                              StaNO = a.Cou_Type,
                              StaName = b.Sta_Name,
                              CouNo = a.Cou_No,
                              CouName = a.Cou_Name,
                              CouType = a.Cou_Type
                          };
            results = results.Where(a => a.CouNo.Contains(id));
            List<dynamic> dnc = new List<dynamic>();
            foreach (var item in results.ToList())
            {
                dynamic d = new ExpandoObject();
                d.CouId = item.CouId;
                d.StaNO = item.StaNO;
                d.StaName = item.StaName;
                d.CouNo = item.CouNo;
                d.CouName = item.CouName;
                d.CouType = item.CouType;
                dnc.Add(d);
            }
            ViewBag.Data = dnc;
            return View();
        }
    }
}