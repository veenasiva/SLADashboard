using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SLADashboard.Infrastructure
{

    public class StatusInfo
    {
        public string Profile { get; set; }
        public string Date { get; set; }
        public string VolumeAdded { get; set; }
        public string VolumeExported { get; set; }
        public string DeletedQuantity { get; set; }
        public string NumberOfProblems { get; set; }
        public int ProfileID { get; set; }
    }
    public class TableStatus
    {
        public int ClientID { get; set; }
        public string ClientName { get; set; }
        public string Period { get; set; }
        public List<StatusInfo> StatusInfo { get; set; }
    }

    public class StatusConfig
    {
        public int ProfileID { get; set; }
        public int ClientID { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
    }
}