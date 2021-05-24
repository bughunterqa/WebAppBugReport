using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebAppBugReport.Data.Models;

namespace WebAppBugReport.Data.ViewModels
{
    public class FilterBugsViewModel
    {
        public IEnumerable<Bug> Bugs { get; set; }
        public SelectList Statuses { get; set; }
        public SelectList Users { get; set; }
    }

    public class TestView
    {
        public IEnumerable<Bug> Bugs { get; set; }
        public FilterBugsViewModel FilterBugsViewModel { get; set; }
        public BugPagingViewModel bugPagingViewModel { get; set; }
    }
}