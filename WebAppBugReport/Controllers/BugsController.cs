using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebAppBugReport.Data;
using WebAppBugReport.Data.Models;

namespace WebAppBugReport.Controllers
{
    public class BugsController : Controller
    {
        private AppDbContext db = new AppDbContext();

        // GET: Bugs
        public ActionResult TestCard1()
        {

            List<Status> statuses = db.Statuses.ToList();


            

            List<Bug> bug = db.Bugs
                .Include(p => p.Status)
                .Include(p => p.Priority)
                .ToList();

            BugList bugList = new BugList
            {
                Statuses = statuses,
                Bugs = bug
            };

            return View(bugList);
        }


        // GET: Bugs/Details/5
        public ActionResult Details(int? id)
        {
            Bug bug = db.Bugs.Find(id);


            return View(bug);
        }

        // GET: Bugs/Create
        public ActionResult Create()
        {
            ViewBag.PriorityId = new SelectList(db.Priorities, "Id", "Name");
            ViewBag.ProjectId = new SelectList(db.Projects, "Id", "ProjectName");
            ViewBag.StatusId = new SelectList(db.Statuses, "Id", "Name");
            ViewBag.UserId = new SelectList(db.Users, "Id", "Name");
            return View();
        }

        // POST: Bugs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Summary,Description,ExpectedResult,ActualResult,StepToReproduce,BugImg,Comment,Link,Date,PriorityId,StatusId,ResultId,UserId,ProjectId")] Bug bug)
        {
            if (ModelState.IsValid)
            {
                db.Bugs.Add(bug);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.PriorityId = new SelectList(db.Priorities, "Id", "Name", bug.PriorityId);
            ViewBag.ProjectId = new SelectList(db.Projects, "Id", "ProjectName", bug.ProjectId);
            ViewBag.StatusId = new SelectList(db.Statuses, "Id", "Name", bug.StatusId);
            ViewBag.UserId = new SelectList(db.Users, "Id", "Name", bug.UserId);
            return View(bug);
        }

        // GET: Bugs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Bug bug = db.Bugs.Find(id);
            if (bug == null)
            {
                return HttpNotFound();
            }
            ViewBag.PriorityId = new SelectList(db.Priorities, "Id", "Name", bug.PriorityId);
            ViewBag.ProjectId = new SelectList(db.Projects, "Id", "ProjectName", bug.ProjectId);
            ViewBag.StatusId = new SelectList(db.Statuses, "Id", "Name", bug.StatusId);
            ViewBag.UserId = new SelectList(db.Users, "Id", "Name", bug.UserId);
            return View(bug);
        }

        // POST: Bugs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Summary,Description,ExpectedResult,ActualResult,StepToReproduce,BugImg,Comment,Link,Date,PriorityId,StatusId,ResultId,UserId,ProjectId")] Bug bug)
        {
            if (ModelState.IsValid)
            {
                db.Entry(bug).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.PriorityId = new SelectList(db.Priorities, "Id", "Name", bug.PriorityId);
            ViewBag.ProjectId = new SelectList(db.Projects, "Id", "ProjectName", bug.ProjectId);
            ViewBag.StatusId = new SelectList(db.Statuses, "Id", "Name", bug.StatusId);
            ViewBag.UserId = new SelectList(db.Users, "Id", "Name", bug.UserId);
            return View(bug);
        }

        // GET: Bugs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Bug bug = db.Bugs.Find(id);
            if (bug == null)
            {
                return HttpNotFound();
            }
            return View(bug);
        }

        // POST: Bugs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Bug bug = db.Bugs.Find(id);
            db.Bugs.Remove(bug);
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
