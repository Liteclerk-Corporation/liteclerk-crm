using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace easyfiscrm_api.Entities
{
    public class ReportOpenSupportEntity
    {
        public String SPNumber { get; set; }
        public String Customer { get; set; }
        public String SupportDate { get; set; }
        public String ProductDescription { get; set; }
        public String Amount { get; set; }
        public String PointOfContact { get; set; }
        public String ExpectedCloseDate { get; set; }
        public String AssignedToUser { get; set; }
        public String LastActivity { get; set; }
        public String LastActivityDate { get; set; }
        public String LastActivityStaff { get; set; }
    }
}