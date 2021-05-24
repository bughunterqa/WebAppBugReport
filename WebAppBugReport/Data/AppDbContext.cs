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
        public DbSet<Result> Results { get; set; }
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

            db.Results.Add(new Result { Name = "Verified" });
            db.Results.Add(new Result { Name = "Unverified" });

            db.Statuses.Add(new Status { Name = "Open" });
            db.Statuses.Add(new Status { Name = "Done" });
            db.Statuses.Add(new Status { Name = "In Progress" });
            db.Statuses.Add(new Status { Name = "Cannot Reproduce" });
            db.Statuses.Add(new Status { Name = "Pause" });
            db.Statuses.Add(new Status { Name = "Not a Bug" });
            db.Statuses.Add(new Status { Name = "Needed more info" });

            db.Roles.Add(new Role { Name = "tester" });
            db.Roles.Add(new Role { Name = "developer" });


            db.Users.Add(new User { Name = "Владик", Email="testet@tester.com", Password="Test123!", RoleId= 1});
            db.Users.Add(new User { Name = "Маша", Email = "dev@dev.com", Password = "Test123!", RoleId = 2 });




            /*db.Users.Add(new User { Name = "Маша" });
            db.Users.Add(new User { Name = "Таня" });
            db.Users.Add(new User { Name = "Саша" });*/



            base.Seed(db);
        }
    }
}