using SchoolWeb.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;


namespace Web.Controllers
{
    /// <summary>
    /// 班级
    /// 作者:陈宇恒
    /// 创建时间:2020-7-08 17:17:30
    /// </summary>
    public class ClassInfoController : Controller
    {
        
        private SchoolManageEntities6 db = new SchoolManageEntities6();

        // GET: ClassInfo
        public ActionResult Index()
        {
            var result = from a in db.ClassInfo
                         select a;

            return View(result);
        }

        [HttpPost]
        public ActionResult Index(string classInfo)
        {
            //var classInfo = db.ClassInfo.Include(c => c.Staff);
            var result = from a in db.ClassInfo
                         select a;
            result = result.Where(a => a.Cla_No.Contains(classInfo) || a.Cla_Name.Contains(classInfo));
            return View(result);
        }
        // GET: ClassInfo/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ClassInfo classInfo = db.ClassInfo.Find(id);
            if (classInfo == null)
            {
                return HttpNotFound();
            }
            return View(classInfo);
        }

        // GET: ClassInfo/Create
        public ActionResult Create()
        {
            ViewBag.Cla_Man = new SelectList(db.Staff, "Sta_No", "Sta_Name");
            return View();
        }

        // POST: ClassInfo/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Cla_id,Cla_No,Cla_Name,Cla_CreateTime,Cla_Man,IsDel,Remark")] ClassInfo classInfo)
        {
            if (ModelState.IsValid)
            {
                db.ClassInfo.Add(classInfo);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Cla_Man = new SelectList(db.Staff, "Sta_No", "Sta_Name", classInfo.Cla_Man);
            return View(classInfo);
        }

        // GET: ClassInfo/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ClassInfo classInfo = db.ClassInfo.Find(id);
            if (classInfo == null)
            {
                return HttpNotFound();
            }
            ViewBag.Cla_Man = new SelectList(db.Staff, "Sta_No", "Sta_Name", classInfo.Cla_Man);
            return View(classInfo);
        }

        // POST: ClassInfo/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Cla_id,Cla_No,Cla_Name,Cla_CreateTime,Cla_Man,IsDel,Remark")] ClassInfo classInfo)
        {
            if (ModelState.IsValid)
            {
                db.Entry(classInfo).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Cla_Man = new SelectList(db.Staff, "Sta_No", "Sta_Name", classInfo.Cla_Man);
            return View(classInfo);
        }

        // GET: ClassInfo/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ClassInfo classInfo = db.ClassInfo.Find(id);
            if (classInfo == null)
            {
                return HttpNotFound();
            }
            return View(classInfo);
        }

        // POST: ClassInfo/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            ClassInfo classInfo = db.ClassInfo.Find(id);
            db.ClassInfo.Remove(classInfo);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
