using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MajorProject.Models;

namespace MajorProject.Controllers
{
    public class SignupsController : Controller
    {
        private DataContext db = new DataContext();

        // GET: Signups
        public ActionResult Index()
        {
            var signups = db.Signups.Include(s => s.Class).Include(s => s.Student);
            return View(signups.ToList());
        }

        // GET: Signups/Details/5
        [Authorize]
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Signup signup = db.Signups.Find(id);
            if (signup == null)
            {
                return HttpNotFound();
            }
            return View(signup);
        }

        // GET: Signups/Create
        [Authorize]
        public ActionResult Create()
        {
            ViewBag.ClassId = new SelectList(db.Classes, "ClassId", "ClassName");
            ViewBag.StudentId = new SelectList(db.Students, "StudentId", "StudentName");
            return View();
        }

        // POST: Signups/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Create([Bind(Include = "StudentId,ClassId")] Signup signup)
        {
            if (ModelState.IsValid)
            {
                Signup tmpsignup = db.Signups.SingleOrDefault(x => x.StudentId == signup.StudentId && x.ClassId == signup.ClassId);

                if (tmpsignup == null)
                {
                    signup.SignupId = Guid.NewGuid().ToString();
                    signup.CreateDate = DateTime.Now;
                    signup.EditDate = signup.CreateDate;
                    db.Signups.Add(signup);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("", "Duplicate key found!");
                }

            }

            ViewBag.StudentId = new SelectList(db.Students, "StudentId", "StudentName", signup.StudentId);
            ViewBag.ClassId = new SelectList(db.Classes, "ClassId", "ClassName", signup.ClassId);
            return View(signup);
        }

        // GET: Signups/Edit/5
        [Authorize]
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Signup signup = db.Signups.Find(id);
            if (signup == null)
            {
                return HttpNotFound();
            }
            ViewBag.ClassId = new SelectList(db.Classes, "ClassId", "ClassName", signup.ClassId);
            ViewBag.StudentId = new SelectList(db.Students, "StudentId", "StudentName", signup.StudentId);
            return View(signup);
        }

        // POST: Signups/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Edit([Bind(Include = "SignupId,StudentId,ClassId")] Signup signup)
        {
            if (ModelState.IsValid)
            {
                Signup signups = db.Signups.Find(signup.SignupId);
                if (signup != null)
                {
                    signups.StudentId = signup.StudentId;
                    signups.ClassId = signup.ClassId;
                    signups.EditDate = DateTime.Now;
                    db.Entry(signups).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");

                }

            }
            ViewBag.ClassId = new SelectList(db.Classes, "ClassId", "ClassName", signup.ClassId);
            ViewBag.StudentId = new SelectList(db.Students, "StudentId", "StudentName", signup.StudentId);
            return View(signup);
        }

        // GET: Signups/Delete/5
        [Authorize]
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Signup signup = db.Signups.Find(id);
            if (signup == null)
            {
                return HttpNotFound();
            }
            return View(signup);
        }

        // POST: Signups/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult DeleteConfirmed(string id)
        {
            Signup signup = db.Signups.Find(id);
            db.Signups.Remove(signup);
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
