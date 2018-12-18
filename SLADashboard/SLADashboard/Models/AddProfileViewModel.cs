using SLADashboard.Core;
using System.Collections.Generic;

namespace SLADashboard.Models
{
    public class AddProfileViewModel
    {
        public Client Client { get; set; }
        public List<Profile> Profiles { get; set;}
        public string NewProfileName { get; set; }
        public string NewProfileDescription { get; set; }
        public string NewProfileIDPrefix { get; set; }
    }
}