using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SLADashboard.Infrastructure
{
   

    public class ReportInfo
    {
        public string ClientName { get; set; }
        public int ProfileID { get; set; }
        public string Profile { get; set; }
        public string ProfileDescription { get; set; }
        public string ProfilePrefix { get; set; }
        public string Date { get; set; }
        public string SlaName { get; set; }
        public string SlaDescription { get; set; }
        public double? Target { get; set; }
        public string TargetAchieved { get; set; }
        public string SLAIndicator { get; set; }
        
    }
    public class TableReport
    {
        public string ReportName { get; set; }
        public string Period { get; set; }
        public List<ReportInfo> ReportInfo { get; set; }
    }

    public class ReportConfig
    {
        public int ProfileID { get; set; }
        public int ClientID { get; set; }
        public int SelectedYear { get; set; }
        public int SelectedMonth { get; set; }
    }
}