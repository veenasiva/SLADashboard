using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace SLADashboard.Infrastructure
{
    public class JsonResultReturn
    {
        public int ErrorNumber { get; set; }
        public string ErrorKey { get; set; }
        public string ErrorMessage { get; set; }
        public bool Success { get; set; }
        public string SuccessMessage { get; set; }
    }
}
