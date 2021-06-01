using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebAppBugReport.Data.Models
{
    public class Project
    {
        public int Id { get; set; }
        [Display(Name = "Проект")]
        public string ProjectName { get; set; }


        public virtual ICollection<User> Users { get; set; }
        public Project()
        {
            Users = new List<User>();
        }

    }

    public class ProjectPagingViewModel
    {
        public IEnumerable<Project> Projects { get; set; }
        public PageInfo PageInfo { get; set; }

        public User User { get; set; }
    }
}