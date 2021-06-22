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
    public class ProjectsController : Controller
    {
        
        AppDbContext db = new AppDbContext();

        [Authorize]
        public ActionResult Index(int page = 1)
        {

            User user = db.Users.Where(p => p.Email == User.Identity.Name).FirstOrDefault();
            ViewBag.Users = user;




            int pageSize = 7;
            IEnumerable<Project> projectsPerPages = db.Projects
                .OrderBy(x => x.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            PageInfo pageInfo = new PageInfo { PageNumber = page, PageSize = pageSize, TotalItems = db.Projects.Count() };
            ProjectPagingViewModel ivm = new ProjectPagingViewModel { PageInfo = pageInfo, Projects = projectsPerPages, User = user };

            return View(ivm);
        }




        public ActionResult Create()
        {
            
            return View();
        }




        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,ProjectName")] Project project)
        {
            User user = db.Users.First(p => p.Email == User.Identity.Name);

            if (ModelState.IsValid)
            {
                user.Projects.Add(project);

                //db.Projects.Add(project, User = new User);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(project);
        }



        [HttpPost]
        public ActionResult Edit(string projectName, int projectId)
        {

            Project project = db.Projects.Find(projectId);
            if(project != null)
            {
                project.ProjectName = projectName;
            }
            
            if (ModelState.IsValid)
            {
                db.Entry(project).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Details", new { id = projectId });

            }

          
            return View(project);
        }





        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Project project = db.Projects.Find(id);
            db.Projects.Remove(project);
            db.SaveChanges();
            return RedirectToAction("Index");

        }


        public ActionResult DeleteUserFromProject( int projectId, int id)
        {
            Project project = db.Projects.Find(projectId);
            User user = project.Users.First(p => p.Id == id);

            if (project != null && user != null)
            {
                project.Users.Remove(user);
                db.SaveChanges();
            }
            return RedirectToAction("Details", new { id = projectId });
        }



        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project project = db.Projects.Find(id);
            if (project == null)
            {
                return HttpNotFound();
            }

            ViewBag.Name = project.ProjectName;

            List<User> users = db.Users
                .Include(p => p.Role)
                .ToList();

            ViewBag.ProjectId = id;
            ViewBag.Project = project;
            ViewBag.Users = users;

            return View(project);
        }


        [HttpPost]
        public ActionResult AddUserToProject(string email, int projectId)
        {
            
            User user = db.Users.Where(p => p.Email == email).FirstOrDefault();
            if(user != null)
            {
                Project project = db.Projects.Find(projectId);


                if (!project.Users.Contains(user))
                {
                    project.Users.Add(user);
                    db.SaveChanges();
                   
                }
            }

            return RedirectToAction("Details", new { id = projectId });



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
