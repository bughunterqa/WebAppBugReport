using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using WebAppBugReport.Data.Models;

namespace WebAppBugReport.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext() : base("DefaultConnection") { }



        public DbSet<User> Users { get; set; }
        public DbSet<Bug> Bugs { get; set; }
        public DbSet<Priority> Priorities { get; set; }

        public DbSet<Status> Statuses { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Project> Projects { get; set; }






    }


    public class DbInitializer : DropCreateDatabaseAlways<AppDbContext>
    {
        protected override void Seed(AppDbContext db)
        {
            db.Priorities.Add(new Priority { Name = "Low" });
            db.Priorities.Add(new Priority { Name = "Medium" });
            db.Priorities.Add(new Priority { Name = "Hight" });



            db.Statuses.Add(new Status { Name = "Open" });
            db.Statuses.Add(new Status { Name = "Queue" });
            db.Statuses.Add(new Status { Name = "In Progress" });
            db.Statuses.Add(new Status { Name = "In Review" });
            db.Statuses.Add(new Status { Name = "Not a Bug" });
            db.Statuses.Add(new Status { Name = "Close" });
            

            db.Roles.Add(new Role { Name = "tester" });
            db.Roles.Add(new Role { Name = "developer" });



            User s1 = new User { Id = 1, Name = "Егор", Email = "testet@tester.com", Password = "Test123!", RoleId = 1 };
            User s2 = new User { Id = 2, Name = "Мария", Email = "dev@dev.com", Password = "Test123!", RoleId = 2 };
            User s3 = new User { Id = 3, Name = "Олег", Email = "random@random.com", Password = "Test123!", RoleId = 2 };
            User s4 = new User { Id = 4, Name = "Ольга", Email = "initial@initial.com", Password = "Test123!", RoleId = 2 };

            db.Users.Add(s1);
            db.Users.Add(s2);
            db.Users.Add(s3);
            db.Users.Add(s4);

            Project c1 = new Project
            {
                Id = 1,
                ProjectName = "Операционные системы",
                Users = new List<User>() { s1, s2, s3 }
            };
            Project c2 = new Project
            {
                Id = 2,
                ProjectName = "Алгоритмы и структуры данных",
                Users = new List<User>() { s2, s4 }
            };
            Project c3 = new Project
            {
                Id = 3,
                ProjectName = "Основы HTML и CSS",
                Users = new List<User>() { s3, s4, s1 }
            };

            db.Projects.Add(c1);
            db.Projects.Add(c2);
            db.Projects.Add(c3);




            base.Seed(db);
        }
    }
}