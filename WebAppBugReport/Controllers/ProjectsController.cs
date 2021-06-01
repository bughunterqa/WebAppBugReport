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


        public ActionResult Edit(int? id)
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

            List<User> users = db.Users
                .Include(p=>p.Role)
                .ToList();

            ViewBag.ProjectId = id;
            ViewBag.Project = project;
            ViewBag.Users = users;

            return View(project);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,ProjectName")] Project project)
        {

            Project newProject = db.Projects.Find(project.Id);
            newProject.ProjectName = project.ProjectName;

            if (ModelState.IsValid)
            {
                db.Entry(newProject).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", "Projects");

            }

          
            return View(project);
        }




        public ActionResult Delete(int? id)
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
            return View(project);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Project project = db.Projects.Find(id);
            db.Projects.Remove(project);
            db.SaveChanges();
            return RedirectToAction("Index");

        }


     /*   public ActionResult DeleteUser(int? id, int projectId)
        {
            Project project = db.Projects.Find(projectId);
            User user = project.Users.First(p => p.Id == id);

            ViewBag.Id = id;
            ViewBag.ProjectId = projectId;
            if (project != null)
                return View(project);


            
            return HttpNotFound();
        }*/

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteUser1(int id, int projectId)
        {
            Project project = db.Projects.Find(projectId);
            User user = project.Users.First(p => p.Id == id);
            user.Projects.Remove(project);
            db.SaveChanges();
            return PartialView(user);
        }


        public ActionResult Details(int id)
        {
            Project project = db.Projects.Find(id);
            if (project != null)
                return PartialView(project);
            return HttpNotFound();
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
