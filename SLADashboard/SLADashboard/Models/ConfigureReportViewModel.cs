﻿using SLADashboard.Core;
using System.Collections.Generic;

namespace SLADashboard.Models
{
    public class ConfigureReportViewModel
    {
        public List<Client> Clients { get; set; }
        //public List<Profile> Profiles { get; set; }
        public int ProfileID { get; set; }
        public List<Month> Months { get; set; }
        public List<Year> Years { get; set; }
        public int SelectedMonth
        {
            get;
            set;
        }
        public int SelectedYear
        {
            get;
            set;
        }
    }
}