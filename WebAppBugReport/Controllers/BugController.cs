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

/*        public ActionResult Index(int id, int? bugStatus, int? assignedDev, int page = 1)
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
        }*/

/*        public ActionResult GetPagination()
        {
            return PartialView();
        }*/

        [HttpGet]
        public ActionResult Create(int? id)
        {



            ViewBag.PriorityId = new SelectList(db.Priorities, "Id", "Name");
            ViewBag.ProjectId = id;



            User use = db.Users.Where(p => p.Email == User.Identity.Name).FirstOrDefault();
            List<Project> projects = db.Projects.ToList();
            List<Project> finalListProj = new List<Project>();
            foreach (Project finalProj in projects)
            {
                if (finalProj.Users.Contains(use))
                {
                    finalListProj.Add(finalProj);
                }
            }
            ViewBag.Projects = finalListProj;








            Project project = db.Projects.Find(id);
            List<User> users = db.Users
                .Where(p=>p.RoleId == 2)
                .ToList();
            List<User> finalList = new List<User>();
            foreach(User user in users)
            {
                if (user.Projects.Contains(project))
                {
                    finalList.Add(user);
                }
            }
            ViewBag.Users = finalList;
            




            return View();
        
        
        }

        [HttpPost]
        [Authorize]
        public ActionResult Create(Bug bug)
        {
            ViewBag.PriorityId = new SelectList(db.Priorities, "Id", "Name");
            ViewBag.ProjectId = new SelectList(db.Projects, "Id", "ProjectName");
            ViewBag.StatusId = new SelectList(db.Statuses, "Id", "Name");
            ViewBag.UserId = new SelectList(db.Users, "Id", "Name");
            bug.Date = DateTime.Now;
            bug.Updated = DateTime.Now;
            bug.StatusId = 1;

            string strDateTime = DateTime.Now.ToString("ddMMyyyyHHMMss");
            string finalPath = "\\UploadedFile\\" + strDateTime + bug.UploadFile.FileName;

            bug.UploadFile.SaveAs(Server.MapPath("~") + finalPath);
            bug.BugImg = finalPath;


            db.Bugs.Add(bug);
            db.SaveChanges();
            return RedirectToAction("ListBugs", new { id = bug.ProjectId });
        }







        public ActionResult ListBugs(int id)
        {
            List<Status> statuses = db.Statuses.ToList();

            ViewBag.Users = db.Users.ToList();
            ViewBag.Priority = db.Priorities.ToList();



            List<Bug> bug = db.Bugs
                .Include(p => p.Status)
                .Include(p => p.Priority)
                .Include(p=>p.User)
                .Where(p=>p.ProjectId == id)
                .OrderByDescending(x => x.Updated)
                .ToList();

            ViewBag.ProjectId = id;

            BugList bugList = new BugList
            {
                Statuses = statuses,
                Bugs = bug
            };

            return View(bugList);
        }


        [HttpPost]
        public ActionResult Edit(int id, int statusId)
        {
            Bug bug = db.Bugs.Find(id);

            bug.StatusId = statusId;
            bug.Updated = DateTime.Now;

            if (ModelState.IsValid)
            {
                db.Entry(bug).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("TestCard");
            }
            return RedirectToAction("TestCard");
        }


        [HttpPost]
        public ActionResult ChangeBug(Bug bug)
        {          
            bug.Updated = DateTime.Now;
            string strDateTime = DateTime.Now.ToString("ddMMyyyyHHMMss");
            string finalPath;

            if(bug.UploadFile != null)
            {
                finalPath = "\\UploadedFile\\" + strDateTime + bug.UploadFile.FileName;
                bug.UploadFile.SaveAs(Server.MapPath("~") + finalPath);
                bug.BugImg = finalPath;
            }
   

            if (ModelState.IsValid)
            {
                db.Entry(bug).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("ListBugs", new { id = bug.ProjectId });
            }

            return View(bug);

        }

        [HttpPost]
        public ActionResult DeleteBug(int id)
        {
            Bug bug = db.Bugs.Find(id);
            db.Bugs.Remove(bug);
            db.SaveChanges();
            return RedirectToAction("ListBugs", new { id = bug.ProjectId });
        }


    }
}