using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SLADashboard.Core;


namespace SLADashboard.Models
{
    public class ConfigureStatusViewModel
    {
        public List<Client> Clients { get; set; }
        public List<Profile> Profiles { get; set; }
    }
}