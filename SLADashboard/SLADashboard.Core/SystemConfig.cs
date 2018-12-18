using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SLADashboard.Core
{
    public enum Mode
    {
        Administrator = 0,
        Operator = 1,
        Test = 2
    }

    [Table("SystemConfig")]
    public class SystemConfig
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int ID { get; set; }
        public string UserName { get; set; }
        public Mode Mode { get; set; }
    }


    [Table("Settings")]
    public class Settings
    {
        [Key]
        public string Name { get; set; }
        public string Value { get; set; }
        public string Comments { get; set; }
    }
    public class Year
    {
        public int key;
        public string value;
    }
    public class Month
    {
        public int key;
        public string value;
    }
}
