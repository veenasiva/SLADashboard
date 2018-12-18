using System;
using System.Collections.Generic;
using SLADashboard.Core;

namespace SLADashboard.Models
{
    public class SLAValuesTableModel
    {
        public List<SLAValuesForSLA> SlaValues { get; set; }
        public int ProfileID { get; set; }
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