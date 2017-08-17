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
    public class StudentsController : Controller
    {
        private DataContext db = new DataContext();

        // GET: Students
        public ActionResult Index()
        {
            return View(db.Students.ToList());
        }

        // GET: Students/Details/5
        [Authorize]
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = db.Students.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }

        // GET: Students/Create
        [Authorize]
        public ActionResult Create()
        {
            Student model = new Student();
            model.StudentName = String.Format("Student - {0}", DateTime.Now.Ticks);
            ViewBag.Classes = new MultiSelectList(db.Classes.ToList(), "ClassId", "ClassName", model.Classes.Select(x => x.ClassId).ToArray());
            return View(model);
        }

        // POST: Students/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Create([Bind(Include = "StudentName,StudentPhone,StudentEmail,Classesids")] Student student, String[] Classesids)
        {
            if (ModelState.IsValid)
            {
                Student checkstudentname = db.Students.SingleOrDefault(x => x.StudentName == student.StudentName);
                if (checkstudentname == null)
                {
                   
                    db.Students.Add(student);
                    db.SaveChanges();
                    if (Classesids != null)
                    {
                        foreach (string classid in Classesids)
                        {
                            Signup signup = new Signup();

                            signup.StudentId = student.StudentId;
                            signup.ClassId = classid;

                            student.Classes.Add(signup);
                        }
                        db.Entry(student).State = EntityState.Modified;

                        db.SaveChanges();
                    }

                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("", "Duplicate Student detected");
                }
            }
           
            ViewBag.Classes = new MultiSelectList(db.Classes.ToList(), "ClassId", "ClassName", Classesids);
            return View(student);
        }

        // GET: Students/Edit/5
        [Authorize]
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = db.Students.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            ViewBag.Classes = new MultiSelectList(db.Classes.ToList(), "ClassId", "ClassName", student.Classes.Select(x => x.ClassId).ToArray());
            return View(student);
        }

        // POST: Students/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Edit([Bind(Include = "StudentId,StudentName,StudentPhone,StudentEmail,Classesids")] Student student, String[] Classesids)
        {
            if (ModelState.IsValid)
            {
                Student studenttmpobject = db.Students.Find(student.StudentId);
                if (studenttmpobject != null)
                {
                    Student checkstudentname = db.Students.SingleOrDefault(x => x.StudentName == student.StudentName && x.StudentId != student.StudentId);
                    if (checkstudentname == null)
                    {


                        studenttmpobject.StudentName = student.StudentName;
                        studenttmpobject.StudentPhone = student.StudentPhone;
                        studenttmpobject.StudentEmail = student.StudentEmail;
                       
                        db.Entry(studenttmpobject).State = EntityState.Modified;

                        var removeitems = studenttmpobject.Classes.Where(x => !Classesids.Contains(x.ClassId)).ToList();
                        foreach (var removeitem in removeitems)
                        {
                            db.Entry(removeitem).State = EntityState.Deleted;
                        }
                        if (Classesids != null)
                        {
                            var additems = Classesids.Where(x => !studenttmpobject.Classes.Select(y => y.ClassId).Contains(x));
                            foreach (string additem in additems)
                            {
                                Signup singup = new Signup();
                                singup.SignupId= Guid.NewGuid().ToString();
                                singup.CreateDate = DateTime.Now;
                                singup.EditDate = singup.CreateDate;
                                singup.StudentId = studenttmpobject.StudentId;
                                singup.ClassId = additem;
                                db.Signups.Add(singup);
                            }
                        }
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Duplicate Student detected");
                    }

                }


            }
            ViewBag.Classes = new MultiSelectList(db.Classes.ToList(), "ClassId", "ClassName", Classesids);
            return View(student);
        }

        // GET: Students/Delete/5
        [Authorize]
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = db.Students.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }

        // POST: Students/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
      public ActionResult DeleteConfirmed(string id)
        {
            Student student = db.Students.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            foreach (var item in student.Classes.ToList())
            {
                db.Signups.Remove(item);
            }
            db.Students.Remove(student);
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
