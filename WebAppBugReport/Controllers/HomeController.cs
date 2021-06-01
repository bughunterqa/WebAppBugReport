using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebAppBugReport.Data;
using WebAppBugReport.Data.Models;

namespace WebAppBugReport.Controllers
{
    public class HomeController : Controller
    {
        AppDbContext db = new AppDbContext();

        [Authorize]
        public ActionResult Index(int page = 1)
        {

            User user = db.Users.Where(p => p.Email == User.Identity.Name).FirstOrDefault();
            ViewBag.Users = user;

            int pageSize = 3;
            IEnumerable<Project> projectsPerPages = db.Projects
                .OrderBy(x => x.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            PageInfo pageInfo = new PageInfo { PageNumber = page, PageSize = pageSize, TotalItems = db.Projects.Count() };
            ProjectPagingViewModel ivm = new ProjectPagingViewModel { PageInfo = pageInfo, Projects = projectsPerPages, User = user };

                return View(ivm);
        }


       


    }
}