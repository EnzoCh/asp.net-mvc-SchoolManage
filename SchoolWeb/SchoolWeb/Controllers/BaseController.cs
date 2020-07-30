using SchoolWeb.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;

namespace StudentManager.Controllers
{
    /// <summary>
    /// 登录
    /// 作者:刘冉旭
    /// 创建时间:2020-7-08 17:17:30
    /// </summary>
    public class BaseController : Controller
    {
        
        SchoolManageEntities6 sm = new SchoolManageEntities6();
        #region
        [OutputCache(Location = OutputCacheLocation.None, Duration = 0, NoStore = false)]
        
         public ActionResult SecurityCode()
         {
         string oldcode = Session["SecurityCode"] as string;
         string code = CreateRandomCode(5);
         Session["SecurityCode"] = code;
         return File(CreateValidateGraphic(code), "imgs/Jpeg");
        }
        public string GetSession()
        {
            //Response.Write("<script>alert('"+Session["SecurityCode"] +"');</script>");

            return Session["SecurityCode"] as string;
        }

    private byte[] CreateImage(string checkCode)
    {
        int iwidth = (int)(checkCode.Length * 12);
        System.Drawing.Bitmap image = new System.Drawing.Bitmap(iwidth, 20);
        Graphics g = Graphics.FromImage(image);
        Font f = new System.Drawing.Font("Arial", 10, System.Drawing.FontStyle.Bold);
        Brush b = new System.Drawing.SolidBrush(Color.White);
        g.Clear(Color.Blue);
        g.DrawString(checkCode, f, b, 3, 3);
        Pen blackPen = new Pen(Color.Black, 0);
        Random rand = new Random();
        for (int i = 0; i < 5; i++)
        {
            int x1 = rand.Next(image.Width);
            int x2 = rand.Next(image.Width);
            int y1 = rand.Next(image.Height);
            int y2 = rand.Next(image.Height);
            g.DrawLine(new Pen(Color.Silver, 1), x1, y1, x2, y2);
        }
        System.IO.MemoryStream ms = new System.IO.MemoryStream();
        image.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
        return ms.ToArray();
    }

    private string CreateRandomCode(int codeCount)
    {
        string allChar = "0,1,2,3,4,5,6,7,8,9,a,b,c,d,e,f,g,h,i,j,k,l,m,n,o,p,q,r,s,t,u,v,w,x,y,z";
        string[] allCharArray = allChar.Split(',');
        string randomCode = "";
        int temp = -1;
        Random rand = new Random();
        for (int i = 0; i < codeCount; i++)
        {
            if (temp != -1)
            {
                rand = new Random(i * temp * ((int)DateTime.Now.Ticks));
            }
            int t = rand.Next(35);
            if (temp == t)
            {
                    return CreateRandomCode(codeCount);
            }
            temp = t;
            randomCode += allCharArray[t];
        }
            Session["ssdd"] = randomCode;
        return randomCode;
    }
    /// <summary>
    /// 创建验证码的图片
    /// </summary>
    public byte[] CreateValidateGraphic(string validateCode)
    {
        Bitmap image = new Bitmap((int)Math.Ceiling(validateCode.Length * 16.0), 27);
        Graphics g = Graphics.FromImage(image);
        try
        {
            //生成随机生成器
            Random random = new Random();
            //清空图片背景色
            g.Clear(Color.White);
            //画图片的干扰线
            for (int i = 0; i < 25; i++)
            {
                int x1 = random.Next(image.Width);
                int x2 = random.Next(image.Width);
                int y1 = random.Next(image.Height);
                int y2 = random.Next(image.Height);
                g.DrawLine(new Pen(Color.Silver), x1, y1, x2, y2);
            }
            Font font = new Font("Arial", 13, (FontStyle.Bold | FontStyle.Italic));
            LinearGradientBrush brush = new LinearGradientBrush(new Rectangle(0, 0, image.Width, image.Height),
             Color.Blue, Color.DarkRed, 1.2f, true);
            g.DrawString(validateCode, font, brush, 3, 2);
            //画图片的前景干扰点
            for (int i = 0; i < 100; i++)
            {
                int x = random.Next(image.Width);
                int y = random.Next(image.Height);
                image.SetPixel(x, y, Color.FromArgb(random.Next()));
            }
            //画图片的边框线
            g.DrawRectangle(new Pen(Color.Silver), 0, 0, image.Width - 1, image.Height - 1);
            //保存图片数据
            MemoryStream stream = new MemoryStream();
            image.Save(stream, ImageFormat.Jpeg);
            //输出图片流
            return stream.ToArray();
        }
        finally
        {
            g.Dispose();
            image.Dispose();
        }
    }
    #endregion

        // GET: Base
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Views()
        {
            return View();
        }
        // GET: Admin
        public ActionResult AdminLogin()
        {
            try
            {
                List<SelectListItem> roletype = new List<SelectListItem>();
                SelectListItem sli0 = new SelectListItem() { Text = "请选择", Value = "0" };
                roletype.Add(sli0);
                foreach (var item in sm.LoginType)
                {
                    SelectListItem sli = new SelectListItem() { Text = item.Name, Value = item.id.ToString() };
                    roletype.Add(sli);
                }
                ViewData["roletype"] = roletype;
                return View();
            }
            catch
            {

                return View();
            }
          
        }

        /// <summary>
        /// 通过用户名密码登陆
        /// </summary>
        /// <param name="loginName"></param>
        /// <param name="loginPwd"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AdminLogin(string loginName, string loginPwd)
        {
            try
            {
                int rd = Convert.ToInt32(Request.Form["roletype"]);
                List<SelectListItem> roletype = new List<SelectListItem>();
                SelectListItem sli0 = new SelectListItem() { Text = "请选择", Value = "0" };
                roletype.Add(sli0);
                foreach (var item in sm.LoginType)
                {
                    SelectListItem sli = new SelectListItem() { Text = item.Name, Value = item.id.ToString() };
                    roletype.Add(sli);
                }
                ViewData["roletype"] = roletype;
                var admins = from a in sm.LoginInfo
                             where a.LoginName == loginName && a.Password == loginPwd && a.Type == rd
                             select a;

                if (admins.ToList().Count > 0)
                {
                    LoginInfo li = admins.ToList().LastOrDefault();
                    Session["LoginUser"] = li;
                    Session["logintype"] = li.LoginType.id.ToString();
                    return RedirectToAction("../Main/Index");
                }
                else
                {
                    Response.Write("<script>alert('登陆失败,找不到数据');</script>");
                }

                return View();
            }
            catch
            {

                return View();
            }
            
        }

    }
}