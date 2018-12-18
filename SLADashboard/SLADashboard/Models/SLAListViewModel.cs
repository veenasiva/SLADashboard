using SLADashboard.Core;
using System.Collections.Generic;

namespace SLADashboard.Models
{
    public class SLAListViewModel
    {
        public Client Client { get; set; }
        public Profile Profile { get; set; }
        public IEnumerable<SLA> Sla { get; set; }
        public IEnumerable<SLA> SelectedSLA { get; set; }
        public bool Delete { get; set; }
    }
}