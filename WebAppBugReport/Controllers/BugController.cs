using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebAppBugReport.Data;
using WebAppBugReport.Data.Models;
using System.Data.Entity;
using System.IO;
using System.Threading.Tasks;
using System.Net;

namespace WebAppBugReport.Controllers
{
    public class BugController : Controller
    {
        AppDbContext db = new AppDbContext();

        public ActionResult Index(int id, int? bugStatus, int? assignedDev, int page = 1)
        {
            ViewBag.ProjectId = id;
            int pageSize = 3;
            IEnumerable<Bug> bugsPerPages = db.Bugs
                .Include(p => p.Priority)
                .Include(p => p.Result)
                .Include(p => p.Status)
                .Include(p => p.User)
                .Where(p => p.ProjectId == id)
                .OrderByDescending(x => x.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();


            if (bugStatus != null && bugStatus != 0)
            {
                bugsPerPages = bugsPerPages.Where(p => p.StatusId == bugStatus);
            }
            if (assignedDev != null && assignedDev != 0)
            {
                bugsPerPages = bugsPerPages.Where(p => p.UserId == assignedDev);
            }
            List<Status> statuses = db.Statuses.ToList();
            statuses.Insert(0, new Status { Name = "Все", Id = 0 });

            List<User> users = db.Users.ToList();
            users.Insert(0, new User { Name = "Все", Id = 0 });


            PageInfo pageInfo = new PageInfo
            {
                PageNumber = page,
                PageSize = pageSize,
                TotalItems = db.Bugs.Count()
            };

            BugPagingViewModel ivm = new BugPagingViewModel
            {
                PageInfo = pageInfo,
                Bugs = bugsPerPages,
                Statuses = new SelectList(statuses, "Id", "Name"),
                Users = new SelectList(users, "Id", "Name")
            };

            ViewBag.UserName = User.Identity.Name;

            return View(ivm);
        }

        public ActionResult GetPagination()
        {
            return PartialView();
        }

        [HttpGet]
        public ActionResult Create(int? id)
        {
            ViewBag.PriorityId = new SelectList(db.Priorities, "Id", "Name");
            ViewBag.ResultId = new SelectList(db.Results, "Id", "Name");
            ViewBag.StatusId = new SelectList(db.Statuses, "Id", "Name");
            ViewBag.UserId = new SelectList(db.Users, "Id", "Name");
            ViewBag.ProjectId = id;
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "tester")]
        public ActionResult Create(Bug bug)
        {
            ViewBag.PriorityId = new SelectList(db.Priorities, "Id", "Name");
            ViewBag.ResultId = new SelectList(db.Results, "Id", "Name");
            ViewBag.StatusId = new SelectList(db.Statuses, "Id", "Name");
            ViewBag.UserId = new SelectList(db.Users, "Id", "Name");
            bug.Date = DateTime.Now;

            string strDateTime = DateTime.Now.ToString("ddMMyyyyHHMMss");
            string finalPath = "\\UploadedFile\\" + strDateTime + bug.UploadFile.FileName;

            bug.UploadFile.SaveAs(Server.MapPath("~") + finalPath);
            bug.BugImg = finalPath;




            db.Bugs.Add(bug);
            db.SaveChanges();
            return RedirectToAction("Index", new { id = bug.ProjectId });
        }

        [HttpGet]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }
            Bug bug = db.Bugs.Find(id);
            if (bug == null)
            {
                return HttpNotFound();
            }


            ViewBag.PriorityId = new SelectList(db.Priorities, "Id", "Name");
            ViewBag.ResultId = new SelectList(db.Results, "Id", "Name");
            ViewBag.StatusId = new SelectList(db.Statuses, "Id", "Name");
            ViewBag.UserId = new SelectList(db.Users, "Id", "Name");


            return View(bug);
        }

        public ActionResult TestCard()
        {

            IEnumerable<Bug> bug = db.Bugs
                .Include(p=>p.Status)
                .ToList();

            return View(bug);
        }


        [HttpPost]
        public ActionResult Edit(int id, int statusId)
        {
            Bug bug = db.Bugs.Find(id);

            bug.StatusId = statusId;



            if (ModelState.IsValid)
            {
                db.Entry(bug).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(bug);
        }





    }
}