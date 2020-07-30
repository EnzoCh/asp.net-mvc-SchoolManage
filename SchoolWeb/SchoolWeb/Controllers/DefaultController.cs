using SchoolWeb.Models;
using System;
using System.Collections.Generic;
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
    /// <summary>
    /// 报表
    /// 作者:陈忠文
    /// 创建时间:2020-7-08 17:17:30
    /// </summary>
    public class DefaultController : Controller
    {
        SchoolManageEntities6 sme = new SchoolManageEntities6();
        public ActionResult MVCChart(string Cou_No, string Cou_Name)
        {
            ViewData["Cou_No"] = Cou_No;
            ViewData["Cou_Name"] = Cou_Name;
            return View();
        }

        public ActionResult ChartView()
        {
            return PartialView();
        }

        /// <summary>
        /// 柱状图片
        /// </summary>
        /// <returns></returns>
        public ActionResult ZhuChart(string Cou_No, string Cou_Name)
        {
            System.Web.UI.DataVisualization.Charting.Chart chart = new System.Web.UI.DataVisualization.Charting.Chart();
            chart.Width = 1000;
            chart.Height = 400;
            chart.RenderType = RenderType.ImageTag;
            chart.Palette = ChartColorPalette.BrightPastel;
            

            chart.ChartAreas.Add("Subject");
            chart.ChartAreas[0].AxisX.Interval = 1;   //设置X轴坐标的间隔为1
            chart.ChartAreas[0].AxisX.IntervalOffset = 1;  //设置X轴坐标偏移为1
            chart.ChartAreas[0].AxisX.LabelStyle.IsStaggered = false;   //设置是否交错显示,比如数据多的时间分成两行来显示

            chart.Series.Add("Subject");

            chart.Series["Subject"].Label = "#VAL";
            chart.Series["Subject"].LegendText = "成绩";

            //从数据库读取数据
            var result = from a in sme.Score
                         join b in sme.Student on a.Sco_StudentNo equals b.S_No
                         join c in sme.CourseInfo on a.Sco_CourseNo equals c.Cou_No
                         where a.Sco_CourseNo == "Cou001"
                         select new
                         {
                             CourseName = c.Cou_Name,
                             StudentName = b.S_Name,
                             Score = a.Sco_score
                         };
            List<dynamic> dnc = new List<dynamic>();
            foreach (var item in result.ToList())
            {
                dynamic d = new ExpandoObject();
                d.CourseName = item.CourseName;
                d.Score = item.Score;
                d.StudentName = item.StudentName;
                dnc.Add(d);
            }
            ViewBag.Data = dnc;

            //标题
            Title t = new Title(Cou_Name, Docking.Top, new System.Drawing.Font("Trebuchet MS", 14, System.Drawing.FontStyle.Bold), System.Drawing.Color.FromArgb(26, 59, 105));
            chart.Titles.Add(t);

            //遍历姓名和成绩
            foreach (var item in ViewBag.Data)
            {
                if (item.Score == null)
                {
                    item.Score = 0;
                }
                chart.Series["Subject"].Points.AddXY(item.StudentName, item.Score);
            }


            chart.BorderSkin.SkinStyle = BorderSkinStyle.Emboss;
            chart.BorderColor = System.Drawing.Color.FromArgb(26, 59, 105);
            chart.BorderlineDashStyle = ChartDashStyle.Solid;
            chart.BorderWidth = 2;

            chart.Legends.Add("Legend1");

            MemoryStream stream = new MemoryStream();
            chart.SaveImage(stream, ChartImageFormat.Jpeg);

            return new ImageResult(Image.FromStream(stream), ImageFormat.Jpeg);
        }
    }
}