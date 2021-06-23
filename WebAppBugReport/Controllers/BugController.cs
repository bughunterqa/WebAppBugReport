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

       /* public ActionResult Index(int id, int? bugStatus, int? assignedDev, int page = 1)
        {
            ViewBag.ProjectId = id;
            int pageSize = 3;
            IEnumerable<Bug> bugsPerPages = db.Bugs
                .Include(p => p.Priority)
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



        public ActionResult GetPagination()
        {
            return PartialView();
        }



        [HttpGet]
        public ActionResult Create(int? id)
        {
            if(id != null)
            {
                Project project1 = db.Projects.Find(id);
                ViewBag.ProjectName = project1.ProjectName;
                ViewBag.ProjectId = id;
            }
            else
            {
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
               
                ViewBag.ProjectId = new SelectList(finalListProj, "Id", "ProjectName");
                ViewBag.ProjectName = null;
            }


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

            ViewBag.PriorityId = new SelectList(db.Priorities, "Id", "Name");



            return View();
        
        
        }

        [HttpPost]
        [Authorize]
        public ActionResult Create(Bug bug)
        {
            
                ViewBag.PriorityId = new SelectList(db.Priorities, "Id", "Name");
                ViewBag.StatusId = new SelectList(db.Statuses, "Id", "Name");
                ViewBag.ProjectId = new SelectList(db.Projects, "Id", "ProjectName", bug.ProjectId);
                ViewBag.UserId = new SelectList(db.Users, "Id", "Name");
                bug.Date = DateTime.Now;
                bug.Updated = DateTime.Now;
                bug.StatusId = 1;

                string strDateTime = DateTime.Now.ToString("ddMMyyyyHHMMss");
                string finalPath;

                if (bug.UploadFile != null)
                {
                    finalPath = "\\UploadedFile\\" + strDateTime + bug.UploadFile.FileName;
                    bug.UploadFile.SaveAs(Server.MapPath("~") + finalPath);
                    bug.BugImg = finalPath;
                }
                else
                    bug.BugImg = null;

                if (bug.UserId == null)
                    bug.UserId = null;

                if (!string.IsNullOrEmpty(bug.Summary) && bug.Summary.Length < 200 )
                {
                    db.Bugs.Add(bug);
                    db.SaveChanges();

                    return RedirectToAction("ListBugs", new { id = bug.ProjectId });
                }
            return View(bug);
        }







        public ActionResult ListBugs(int id)
        {
            List<Status> statuses = db.Statuses.ToList();

            Project project = db.Projects.Find(id);
            List<User> users = db.Users
                .Where(p => p.RoleId == 2)
                .ToList();
            List<User> finalList = new List<User>();
            foreach (User user in users)
            {
                if (user.Projects.Contains(project))
                {
                    finalList.Add(user);
                }
            }
            ViewBag.Users = finalList;


            
            ViewBag.Priority = db.Priorities.ToList();



            List<Bug> bug = db.Bugs
                .Include(p => p.Status)
                .Include(p => p.Priority)
                .Include(p=>p.User)
                .Where(p=>p.ProjectId == id)
                .OrderByDescending(x => x.Updated)
                .ToList();





            List<Bug> newListBug = new List<Bug>();

            foreach(Bug b in bug)
            {
                if(b.StatusId == 1 && newListBug.Count < 10)
                {
                    newListBug.Add(b);
                    
                }
                if (b.StatusId == 2 && newListBug.Count < 10)
                {
                    newListBug.Add(b);
                }
                if (b.StatusId == 3 && newListBug.Count < 10)
                {
                    newListBug.Add(b);
                }
                if (b.StatusId == 4 && newListBug.Count < 10)
                {
                    newListBug.Add(b);
                }
                if (b.StatusId == 5 && newListBug.Count < 10)
                {
                    newListBug.Add(b);
                }

            }








            ViewBag.ProjectId = id;

            BugList bugList = new BugList
            {
                Statuses = statuses,
                Bugs = newListBug
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



        public ActionResult MoveToBoard(int id)
        {
            Bug bug = db.Bugs.Find(id);
            bug.Updated = DateTime.Now;
            db.SaveChanges();

            if(bug.StatusId == 1)
                return RedirectToAction("AllOpenBugs", new { id = bug.ProjectId });
            else
                return RedirectToAction("AllCloseBugs", new { id = bug.ProjectId });


        }



        public ActionResult AllOpenBugs(int id, int page = 1)
        {
            ViewBag.ProjectId = id;
            int pageSize = 10;
            IEnumerable<Bug> bugsPerPages = db.Bugs
                .Include(p => p.Priority)
                .Include(p => p.User)
                .Where(p => p.ProjectId == id)
                .Where(b => b.StatusId == 1)
                .OrderByDescending(x => x.Updated)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();


            PageInfo pageInfo = new PageInfo
            {
                PageNumber = page,
                PageSize = pageSize,
                TotalItems = db.Bugs
                .Where(p => p.ProjectId == id)
                .Where(b => b.StatusId == 1)
                .Count()
            };

            BugPagingViewModel ivm = new BugPagingViewModel
            {
                PageInfo = pageInfo,
                Bugs = bugsPerPages,

            };


            return View(ivm);
        }


        public ActionResult AllCloseBugs(int id, int page = 1)
        {
            ViewBag.ProjectId = id;
            int pageSize = 10;
            IEnumerable<Bug> bugsPerPages = db.Bugs
                .Include(p => p.Priority)
                .Include(p => p.User)
                .Where(p => p.ProjectId == id)
                .Where(b => b.StatusId == 5)
                .OrderByDescending(x => x.Updated)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();


            PageInfo pageInfo = new PageInfo
            {
                PageNumber = page,
                PageSize = pageSize,
                TotalItems = db.Bugs
                .Where(p => p.ProjectId == id)
                .Where(b => b.StatusId == 5)
                .Count()
            };

            BugPagingViewModel ivm = new BugPagingViewModel
            {
                PageInfo = pageInfo,
                Bugs = bugsPerPages,

            };


            return View(ivm);
        }

    }
}