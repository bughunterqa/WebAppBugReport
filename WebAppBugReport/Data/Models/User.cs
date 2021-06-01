using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebAppBugReport.Data.Models
{
    public class User
    {
        public int Id { get; set; }
             
        [Display(Name = "Пользователь")]
        public string Name { get; set; }
        public string Email { get; set; }    
        public string Password { get; set; }


        public int RoleId { get; set; }
        public Role Role { get; set; }


        public virtual ICollection<Project> Projects { get; set; }
        public User()
        {
            Projects = new List<Project>();
        }




    }


    public class Role
    {
        public int Id { get; set; }
        public string Name { get; set; }

    }
}