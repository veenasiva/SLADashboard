using SLADashboard.Core;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SLADashboard.Models
{
    public class SLAViewModel
    {
        public Client Client { get; set; }
        public Profile Profile { get; set; }
        [Required]
        public int ID { get; set; }
        [Required]
        public int ProfileID { get; set; }
        [Required(ErrorMessage = "Please Enter Name")]
        [StringLength(100,ErrorMessage = "Must be under 100 charcters")]
        [RegularExpression(@"^[a-zA-Z0-9\s-(),.]*$", ErrorMessage = "Name is invalid,please remove any special characters")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Please Enter Description")]
        [StringLength(1000, ErrorMessage = "Must be under 1000 charcters")]
        [RegularExpression(@"^[^<>!@#%/?*]+$", ErrorMessage = "Description is invalid,please remove any special characters")]
        public string Description { get; set; }
        [Required(ErrorMessage = "Please Enter Target")]
        [Range(1.0, 100.0, ErrorMessage = "Value not in range")]
        [RegularExpression(@"^\d{1,3}(?:\.\d{0,2})?$", ErrorMessage = "Please enter values between 1-100 with 2 decimal places")]
        public double Target { get; set; }

    }
    public class SLADefinitionListViewModel
    {
        public int ClientID { get; set; }
        public int ProfileID { get; set; }
        public List<SLAViewModel> SLAList { get; set; }
    }
}