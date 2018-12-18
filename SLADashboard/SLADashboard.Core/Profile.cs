using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SLADashboard.Core
{
    [Table("Profile")]
    public class Profile
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int ID { get; set; }
        public int ClientID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string SLAIDPrefix { get; set; }
        public virtual Client Client { get; set; }
        public virtual ICollection<SLA> SLAs { get; set; }
    }
}