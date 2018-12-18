using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SLADashboard.Core;

namespace SLADashboard.Models
{
    public class SLAValuesModel
    {
        public List<SLAValuesForSLA> SlaValues { get; set; }
        public string ProfileName { get; set; }
        public int ProfileID { get; set; }
        public string ClientName { get; set; }
        public SLAValues SelectedSla { get; set; }
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