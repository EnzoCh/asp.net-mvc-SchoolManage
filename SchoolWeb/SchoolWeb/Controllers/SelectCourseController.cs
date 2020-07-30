using Microsoft.Office.Interop.Excel;
using PagedList;
using SchoolWeb.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace SchoolManage.Controllers
{
    public class SelectCourseController : Controller
    {
        /// <summary>
        /// 课程查询
        /// 作者:刘洋
        /// 创建时间:2020-7-08 17:17:30
        /// </summary>
        SchoolManageEntities6 db = new SchoolManageEntities6();

        public static List<SelectCourse> source = new List<SelectCourse>();

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
            ExportExcel(ListToDataTable(SelectCourseController.source), "已修课程");
            return Redirect("Index");
        }


        /// <summary>
        /// 已选删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult delete(string CouNo)
        {
            try
            {
                var result = from a in db.SelectCourse
                             where a.SC_CourseNo == CouNo
                             select a;
                SelectCourse sc = result.AsNoTracking().ToList().LastOrDefault();
                SelectCourse scInfo = new SelectCourse();
                scInfo.SC_id = sc.SC_id;
                scInfo.SC_StudentNo = sc.SC_StudentNo;
                scInfo.SC_Time = sc.SC_Time;
                scInfo.SC_CourseNo = sc.SC_CourseNo;
                scInfo.IsDel = sc.IsDel;
                db.Entry<SelectCourse>(scInfo).State = System.Data.Entity.EntityState.Deleted;
                //db.SaveChanges();
                if (db.SaveChanges() != 0)
                {
                    Response.Write("<script>alert('成功')</script>");
                }
                else
                {
                    Response.Write("<script>alert('失败')</script>");
                }
            }
            catch
            {

                return RedirectToAction("Index");
            }

            return RedirectToAction("Index");
        }

        /// <summary>
        /// 查询数据
        /// </summary>
        /// <param name="scoreInfo"></param>
        /// <param name="PageNum"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Index(string selectcourseInfo)
        {
            try
            {
                var result = from a in db.SelectCourse
                             join b in db.CourseInfo on a.SC_CourseNo equals b.Cou_No
                             join c in db.Staff on b.Cou_Teacher equals c.Sta_No
                             select new
                             {
                                 CouNo = a.SC_CourseNo,
                                 SCouTime = a.SC_Time,
                                 CouName = b.Cou_Name,
                                 CouTeacher = c.Sta_Name,
                                 CouType = b.Cou_Type
                             };
                result = result.Where(a => a.CouName.Contains(selectcourseInfo) || a.CouNo.Contains(selectcourseInfo));
                List<dynamic> dnc = new List<dynamic>();
                foreach (var item in result.ToList())
                {
                    dynamic d = new ExpandoObject();
                    d.CouNo = item.CouNo;
                    d.SCouTime = item.SCouTime;
                    d.CouName = item.CouName;
                    d.CouTeacher = item.CouTeacher;
                    d.CouType = item.CouType;
                    dnc.Add(d);
                }
                ViewBag.Data = dnc;
                return View();
            }
            catch
            {
                return View();

            }
            
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            try
            {
                string stuno = Session["stuNo"].ToString();
                var result = from a in db.SelectCourse
                             join b in db.CourseInfo on a.SC_CourseNo equals b.Cou_No
                             join c in db.Staff on b.Cou_Teacher equals c.Sta_No
                             where a.SC_StudentNo == stuno
                             select new
                             {
                                 CouNo = a.SC_CourseNo,
                                 SCouTime = a.SC_Time,
                                 CouName = b.Cou_Name,
                                 CouTeacher = c.Sta_Name,
                                 CouType = b.Cou_Type
                             };
                List<dynamic> dnc = new List<dynamic>();
                foreach (var item in result.ToList())
                {
                    dynamic d = new ExpandoObject();
                    d.CouNo = item.CouNo;
                    d.SCouTime = item.SCouTime;
                    d.CouName = item.CouName;
                    d.CouTeacher = item.CouTeacher;
                    d.CouType = item.CouType;
                    dnc.Add(d);
                }
                ViewBag.Data = dnc;
                return View();
            }
            catch
            {
                return View();
            }
           

        }

    }
}