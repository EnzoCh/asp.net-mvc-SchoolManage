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
    public class ScoreController : Controller
    {
        /// <summary>
        /// 成绩信息
        /// 作者:刘洋
        /// 创建时间:2020-7-08 17:17:30
        /// </summary>
        SchoolManageEntities6 db = new SchoolManageEntities6();

        public static List<Score> source = new List<Score>();

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
            ExportExcel(ListToDataTable(ScoreController.source), "学生成绩");
            return Redirect("Index");
        }


        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult delete(int? id)
        {
            var sInfo = db.Score.Find(id);
            sInfo.IsDel = 1;
            db.Entry<Score>(sInfo).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }


        /// <summary>
        /// 查询数据
        /// </summary>
        /// <param name="scoreInfo"></param>
        /// <param name="PageNum"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Index(string scoreInfo)
        {
            string stuno = Session["stuNo"].ToString();
            var result = from a in db.Score
                         join b in db.Student on a.Sco_StudentNo equals b.S_No
                         join c in db.CourseInfo on a.Sco_CourseNo equals c.Cou_No
                         join d in db.Staff on c.Cou_Teacher equals d.Sta_No
                         where a.Sco_StudentNo == stuno
                         select new
                         {
                             CouNo = a.Sco_CourseNo,
                             CouName = c.Cou_Name,
                             StuNo = b.S_No,
                             StuName = b.S_Name,
                             StuScore = a.Sco_score,
                             CouTeacher = d.Sta_Name,
                             CouType = c.Cou_Type,
                         };
            result = result.Where(a => a.CouName.Contains(scoreInfo) || a.CouNo.Contains(scoreInfo));
            List<dynamic> dnc = new List<dynamic>();
            foreach (var item in result.ToList())
            {
                dynamic d = new ExpandoObject();
                d.CouNo = item.CouNo;
                d.CouName = item.CouName;
                d.StuNo = item.StuNo;
                d.StuName = item.StuName;
                d.StuScore = item.StuScore;
                d.CouTeacher = item.CouTeacher;
                d.CouType = item.CouType;
                dnc.Add(d);
            }
            ViewBag.Data = dnc;
            return View();
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            string stuno = Session["stuNo"].ToString();
            var result = from a in db.Score
                         join b in db.Student on a.Sco_StudentNo equals b.S_No
                         join c in db.CourseInfo on a.Sco_CourseNo equals c.Cou_No
                         join d in db.Staff on c.Cou_Teacher equals d.Sta_No
                         where a.Sco_StudentNo == stuno
                         select new
                         {
                             CouNo = a.Sco_CourseNo,
                             CouName = c.Cou_Name,
                             StuNo = b.S_No,
                             StuName = b.S_Name,
                             StuScore = a.Sco_score,
                             CouTeacher = d.Sta_Name,
                             CouType=c.Cou_Type,
                         };
            List<dynamic> dnc = new List<dynamic>();
            foreach (var item in result.ToList())
            {
                dynamic d = new ExpandoObject();
                d.CouNo = item.CouNo;
                d.CouName = item.CouName;
                d.StuNo = item.StuNo;
                d.StuName = item.StuName;
                d.StuScore = item.StuScore;
                d.CouTeacher = item.CouTeacher;
                d.CouType = item.CouType;
                dnc.Add(d);
            }
            ViewBag.Data = dnc;
            return View();

        }

        
    }
}