using SLADashboard.Core;
using System.Collections.Generic;

namespace SLADashboard.Models
{
    public class AddClientViewModel
    {
        public List<Client> Clients { get; set; }
        public string NewClientName { get; set; }
    }
}