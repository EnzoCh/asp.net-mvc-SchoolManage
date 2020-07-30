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
    public class StudentController : Controller
    {
        /// <summary>
        /// 学生信息管理
        /// 作者:陈宇恒
        /// 创建时间:2020-7-08 17:17:30
        /// </summary>
        private SchoolManageEntities6 db = new SchoolManageEntities6();

        // GET: Student
        public ActionResult Index()
        {
            var student = from a in db.Student
                          select a;

            return View(student.ToList());
        }

        // GET: Student/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = db.Student.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }

        // GET: Student/Create
        public ActionResult Create()
        {
            ViewBag.S_No = new SelectList(db.Student, "S_No", "S_Name");
            ViewBag.S_No = new SelectList(db.Student, "S_No", "S_Name");

            return View();
        }

        // POST: Student/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "S_id,S_No,S_Name,S_Sex,S_Birthday,S_ClassNo,S_Telephone,S_QQ,S_Address,S_Indate,S_Email,IsDel,Remark")] Student student)
        {
            if (ModelState.IsValid)
            {
                db.Student.Add(student);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.S_No = new SelectList(db.Student, "S_No", "S_Name", student.S_No);
            ViewBag.S_No = new SelectList(db.Student, "S_No", "S_Name", student.S_No);
            return View(student);
        }

        // GET: Student/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = db.Student.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            ViewBag.S_No = new SelectList(db.Student, "S_No", "S_Name", student.S_No);
            ViewBag.S_No = new SelectList(db.Student, "S_No", "S_Name", student.S_No);
            return View(student);
        }

        // POST: Student/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "S_id,S_No,S_Name,S_Sex,S_Birthday,S_ClassNo,S_Telephone,S_QQ,S_Address,S_Indate,S_Email,IsDel,Remark")] Student student)
        {
            if (ModelState.IsValid)
            {
                db.Entry(student).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.S_No = new SelectList(db.Student, "S_No", "S_Name", student.S_No);
            ViewBag.S_No = new SelectList(db.Student, "S_No", "S_Name", student.S_No);
            return View(student);
        }

        // GET: Student/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = db.Student.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }

        // POST: Student/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Student student = db.Student.Find(id);
            db.Student.Remove(student);
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
