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

        [Display(Name = "Заглавие")]
        public string Summary { get; set; }

        [Display(Name = "Описание")]
        public string Description { get; set; }

        [Display(Name = "Ожидаемый результат")]
        public string ExpectedResult { get; set; }

        [Display(Name = "Фактический результат")]
        public string ActualResult { get; set; }

        [Display(Name = "Шаги для воспроизведения")]
        public string StepToReproduce { get; set; }

        [Display(Name = "Скриншот")]
        public string BugImg { get; set; }
        [NotMapped]
        public HttpPostedFileBase UploadFile { get; set; }

        [Display(Name = "Комментарий")]
        public string Comment { get; set; }

        [Display(Name = "Ссылка")]
        public string Link { get; set; }

        [Display(Name = "Дата")]
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
        [Display(Name = "Приоритет")]
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
        [Display(Name = "Статус")]
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