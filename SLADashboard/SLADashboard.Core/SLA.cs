using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace SLADashboard.Core
{
    [Table("SLA")]
    public class SLA
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int ID { get; set; }
        public int ProfileID { get; set; }
        [Required(ErrorMessage = "Please Enter Name")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Please Enter Description")]
        public string Description { get; set; }
        public double Target { get; set; }
        public string DeletedUser { get; set; }
        public bool ?IsDeleted { get; set; }
        public virtual Profile Profile { get; set; }
        public virtual ICollection<SLAValues> SLAValues { get; set; }
    }
}
