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
    public class ClassesController : Controller
    {
        private DataContext db = new DataContext();

        // GET: Classes
        public ActionResult Index()
        {
            return View(db.Classes.ToList());
        }

        // GET: Classes/Details/5
        [Authorize]
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Class calss = db.Classes.Find(id);
            if (calss == null)
            {
                return HttpNotFound();
            }
            return View(calss);
        }

        // GET: Classes/Create
        [Authorize]
        public ActionResult Create()
        {
            Class model = new Class();
            model.ClassName = String.Format("Class - {0}", DateTime.Now.Ticks);
            ViewBag.Students = new MultiSelectList(db.Students.ToList(), "StudentId", "StudentName", model.Students.Select(x => x.StudentId).ToArray());
            return View(model);
        }

        // POST: Classes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Create([Bind(Include = "ClassName,ClassDescription,Studentsids")] Class calss, String[] Studentsids)
        {
            if (ModelState.IsValid)
            {
                Class checkclassame = db.Classes.SingleOrDefault(x => x.ClassName == calss.ClassName);
                if (checkclassame == null)
                {
                    calss.ClassId = Guid.NewGuid().ToString();
                    calss.CreateDate = DateTime.Now;
                    calss.EditDate = calss.CreateDate;
                    db.Classes.Add(calss);
                    db.SaveChanges();
                    if (Studentsids != null)
                    {
                        foreach (string studentsid in Studentsids)
                        {
                            Signup signup = new Signup();
                            signup.SignupId = Guid.NewGuid().ToString();
                            signup.CreateDate = DateTime.Now;
                            signup.EditDate = signup.CreateDate;
                            signup.StudentId = studentsid;
                            signup.ClassId = calss.ClassId;
                            db.Signups.Add(signup);
                        }
                    }
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("", "Duplicate student detected");
                }

            }
            ViewBag.Students = new MultiSelectList(db.Students.ToList(), "StudentId", "StudentName", Studentsids);

            return View(calss);
        }

        // GET: Classes/Edit/5
        [Authorize]
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Class calss = db.Classes.Find(id);
            if (calss == null)
            {
                return HttpNotFound();
            }
            ViewBag.Students = new MultiSelectList(db.Students.ToList(), "StudentId", "StudentName", calss.Students.Select(x => x.StudentId).ToArray());
            return View(calss);
        }

        // POST: Classes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Edit([Bind(Include = "ClassId,ClassName,ClassDescription,Studentsids")] Class calss, String[] Studentsids)
        {
            if (ModelState.IsValid)
            {
                Class classstmpobject = db.Classes.Find(calss.ClassId);
                if (classstmpobject != null)
                {
                    Class checkclassname = db.Classes.SingleOrDefault(x => x.ClassName == calss.ClassName && x.ClassId != calss.ClassId);
                    if (checkclassname == null)
                    {
                        classstmpobject.ClassName = calss.ClassName;
                        classstmpobject.ClassDescription = calss.ClassDescription;

                        db.Entry(classstmpobject).State = EntityState.Modified;

                        var removeitems = classstmpobject.Students.Where(x => !Studentsids.Contains(x.StudentId)).ToList();
                        foreach (var removeitem in removeitems)
                        {
                            db.Entry(removeitem).State = EntityState.Deleted;
                        }
                        if (Studentsids != null)
                        {
                            var additems = Studentsids.Where(x => !classstmpobject.Students.Select(y => y.StudentId).Contains(x));

                            foreach (string additem in additems)
                            {
                                Signup signup = new Signup();
                                signup.SignupId = Guid.NewGuid().ToString();
                                signup.CreateDate = DateTime.Now;
                                signup.EditDate = signup.CreateDate;
                                signup.StudentId = additem;
                                signup.ClassId = classstmpobject.ClassId;
                                db.Signups.Add(signup);
                            }
                        }
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Duplicate game detected");
                    }

                }
            }
            ViewBag.Games = new MultiSelectList(db.Students.ToList(), "StudentId", "StudentName", Studentsids);
            return View(calss);
        }

        // GET: Classes/Delete/5
        [Authorize]
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Class calss = db.Classes.Find(id);
            if (calss == null)
            {
                return HttpNotFound();
            }
            return View(calss);
        }

        // POST: Classes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult DeleteConfirmed(string id)
        {
            Class calss = db.Classes.Find(id);
            if (calss == null)
            {
                return HttpNotFound();
            }
            foreach (var item in calss.Students.ToList())
            {
                db.Signups.Remove(item);
            }
            db.Classes.Remove(calss);
            var deleted = db.ChangeTracker.Entries().Where(e => e.State == EntityState.Added);
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
