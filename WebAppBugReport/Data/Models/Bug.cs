using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebAppBugReport.Data.Models
{
    public class Bug
    {
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Summary { get; set; }


        [Display(Name = "Expected Result")]
        public string ExpectedResult { get; set; }

        [Display(Name = "Actual Result")]
        public string ActualResult { get; set; }

        [Display(Name = "Step to Reproduce")]
        public string StepToReproduce { get; set; }

        [Display(Name = "Attachment")]
        public string BugImg { get; set; }
        [NotMapped]
        public HttpPostedFileBase UploadFile { get; set; }


        public string Link { get; set; }

        public DateTime Date { get; set; }

        public DateTime? Updated { get; set; }



        public int PriorityId { get; set; }
        public Priority Priority { get; set; }


        public int StatusId { get; set; }
        public Status Status { get; set; }





        public int? UserId { get; set; }
        public User User { get; set; }


        public int ProjectId { get; set; }
        public Project Project { get; set; }
    }

    public class Priority
    {
        public int Id { get; set; }
        [Display(Name = "Priority")]
        public string Name { get; set; }
    }


    public class Status
    {
        public int Id { get; set; }
        [Display(Name = "Status")]
        public string Name { get; set; }
    }




    public class PageInfo
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalItems { get; set; }

        public int TotalPages
        {
            get { return (int)Math.Ceiling((decimal)TotalItems / PageSize); }
        }
    }

    public class BugPagingViewModel
    {
        public IEnumerable<Bug> Bugs { get; set; }
        public PageInfo PageInfo { get; set; }



        public SelectList Statuses { get; set; }
        public SelectList Users { get; set; }
    }

    public class BugList
    {
        public IEnumerable<Bug> Bugs { get; set; }
        public IEnumerable<Status> Statuses { get; set; }
    }
}