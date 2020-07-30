using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace czwshixun.Controllers
{
    public class ImageResult : ActionResult
    {
        public ImageResult()
        { }

        public ImageResult(Image image)
        {
            Image = image;
        }

        public ImageResult(Image image, ImageFormat format)
        {
            Image = image;
            ImageFormat = format;
        }

        /// <summary> 
        /// 
        /// </summary> 
        public Image Image { get; set; }

        /// <summary> 
        /// 指定图像的文件格式 
        /// </summary> 
        public ImageFormat ImageFormat { get; set; }

        public override void ExecuteResult(ControllerContext context)
        {
            if (Image == null)
                throw new ArgumentNullException("Image");

            if (ImageFormat == null)
                throw new ArgumentNullException("ImageFormat");

            context.HttpContext.Response.Clear();

            if (ImageFormat.Equals(ImageFormat.Gif))
                context.HttpContext.Response.ContentType = "image/gif";
            else if (ImageFormat.Equals(ImageFormat.Jpeg))
                context.HttpContext.Response.ContentType = "image/jpeg";
            else if (ImageFormat.Equals(ImageFormat.Png))
                context.HttpContext.Response.ContentType = "image/png";
            else if (ImageFormat.Equals(ImageFormat.Bmp))
                context.HttpContext.Response.ContentType = "image/bmp";
            else if (ImageFormat.Equals(ImageFormat.Tiff))
                context.HttpContext.Response.ContentType = "image/tiff";
            else if (ImageFormat.Equals(ImageFormat.Icon))
                context.HttpContext.Response.ContentType = "image/vnd.microsoft.icon";
            else if (ImageFormat.Equals(ImageFormat.Wmf))
                context.HttpContext.Response.ContentType = "image/wmf";

            Image.Save(context.HttpContext.Response.OutputStream, ImageFormat);
        }
    }
}