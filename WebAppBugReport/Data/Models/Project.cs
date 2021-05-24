using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAppBugReport.Data.Models
{
    public class Project
    {
        public int Id { get; set; }
        public string ProjectName { get; set; }

    }

    public class ProjectPagingViewModel
    {
        public IEnumerable<Project> Projects { get; set; }
        public PageInfo PageInfo { get; set; }
    }
}