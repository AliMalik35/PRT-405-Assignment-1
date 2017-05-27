using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using OnlineJobs;
using Microsoft.AspNet.Identity;

namespace OnlineJobs.Controllers
{
    public class JobsController : Controller
    {
        private JobDBEntities2 db = new JobDBEntities2();
        public Boolean CheckCurrentUser()
        {
            var CurrentUserName = User.Identity.GetUserName();
            var CurrentUserID = User.Identity.GetUserId();
            if (CurrentUserID != null && CurrentUserName != "")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public Boolean CheckAdmin()
        {
            if (User.Identity.GetUserName() == "alimalik@gmail.com")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        // GET: Jobs
        [Authorize]
        public ActionResult Index()
        {
            if (CheckCurrentUser() == true)
            {
                if (CheckAdmin() == true)
                {
                    var jobs = db.Jobs.Include(j => j.AspNetUser);
                    return View(jobs);
                }
                else
                {
                    var loginUser = User.Identity.GetUserId();
                    var job2 = db.Jobs.Where(j => j.User_ID ==loginUser);
                    return View(job2.ToList());
                }
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
            
        }

        // GET: Jobs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Job job = db.Jobs.Find(id);
            if (job == null)
            {
                return HttpNotFound();
            }
            return View(job);
        }

        // GET: Jobs/Create
        public ActionResult Create()
        {
            ViewBag.User_ID = new SelectList(db.AspNetUsers, "Id", "Email");
            return View();
        }

        // POST: Jobs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Job_Title,Description,Company,Start_Level,Salary,Post_Date,Last_Date")] Job job)
        {
            job.User_ID = User.Identity.GetUserId();
            if (ModelState.IsValid)
            {
                db.Jobs.Add(job);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.User_ID = new SelectList(db.AspNetUsers, "Id", "Email", job.User_ID);
            return View(job);
        }

        // GET: Jobs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Job job = db.Jobs.Find(id);
            if (job == null)
            {
                return HttpNotFound();
            }
            ViewBag.User_ID = new SelectList(db.AspNetUsers, "Id", "Email", job.User_ID);
            return View(job);
        }

        // POST: Jobs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Job_Title,Description,Company,Start_Level,Salary,Post_Date,Last_Date,User_ID")] Job job)
        {
            if (ModelState.IsValid)
            {
                db.Entry(job).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.User_ID = new SelectList(db.AspNetUsers, "Id", "Email", job.User_ID);
            return View(job);
        }

        // GET: Jobs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Job job = db.Jobs.Find(id);
            if (job == null)
            {
                return HttpNotFound();
            }
            return View(job);
        }

        // POST: Jobs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Job job = db.Jobs.Find(id);
            db.Jobs.Remove(job);
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
