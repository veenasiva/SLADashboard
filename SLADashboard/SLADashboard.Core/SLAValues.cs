using System;
using System.Configuration;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SLADashboard.Core
{
    [Table("SLAValues")]
    public class SLAValues
    {
        //private double _quantity;
        //private double _quantityoutside;
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int ID { get; set; }
        public int SLAID { get; set; }
        public DateTime ReportingDate { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "Error-QuantityProcessed must be a positive number and not zero")]
        public int QuantityProcessed { get; set; }
        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "Error-QuantityOutsideofSLA must be a positive integer number")]
        [QuantityOutsideSLAAttribute("QuantityProcessed",ErrorMessage ="Error-Invalid Value")]
        public int QuantityOutsideofSLA { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public virtual SLA Sla { get; set; }
    }
    public class SLAValuesForSLA
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int? ID { get; set; }
        public int ProfileID { get; set; }
        public int SLAID { get; set; }
        public DateTime? ReportingDate { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "Error-QuantityProcessed must be a positive integer number and not zero")]
        public int? QuantityProcessed { get; set; }
        [Range(0, int.MaxValue, ErrorMessage = "Error-QuantityOutsideofSLA must be a positive integer number")]
        public int? QuantityOutsideofSLA { get; set; }
        public string  SlaName { get; set; }
        public string SlaDescription { get; set; }
        public double? SlaTarget { get; set; }
    }
    public class QuantityOutsideSLAAttribute : ValidationAttribute
    {
        private readonly string _comparisonProperty;

        public QuantityOutsideSLAAttribute(string comparisonProperty)
        {
            _comparisonProperty = comparisonProperty;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            ErrorMessage = ErrorMessageString;
            var currentValue = (int)value;

            var property = validationContext.ObjectType.GetProperty(_comparisonProperty);

            if (property == null)
                throw new ArgumentException("Property with this name not found");

            var comparisonValue = (int)property.GetValue(validationContext.ObjectInstance);

            if (currentValue< 0 || currentValue > comparisonValue)
                return new ValidationResult(ErrorMessage);

            return ValidationResult.Success;
        }
    }
}
