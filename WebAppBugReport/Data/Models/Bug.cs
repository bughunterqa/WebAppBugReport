using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebAppBugReport.Data.Models
{
    public class Bug
    {
        public int Id { get; set; }
        public string Summary { get; set; }
        public string Description { get; set; }
        public string ExpectedResult { get; set; }
        public string ActualResult { get; set; }
        public string StepToReproduce { get; set; }
        public string Attachment { get; set; }
        public string Comment { get; set; }
        public string Link { get; set; }
        public DateTime Date { get; set; }


        public int PriorityId { get; set; }
        public Priority Priority { get; set; }

        public int StatusId { get; set; }
        public Status Status { get; set; }

        public int? ResultId { get; set; }
        public Result Result { get; set; }

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

    public class Result
    {
        public int Id { get; set; }
        [Display(Name = "Result")]
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
}