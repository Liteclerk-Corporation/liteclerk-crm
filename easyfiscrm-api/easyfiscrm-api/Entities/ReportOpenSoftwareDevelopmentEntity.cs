using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace easyfiscrm_api.Entities
{
    public class ReportOpenSoftwareDevelopmentEntity
    {
        public String SDNumber { get; set; }
        public String SDDate { get; set; }
        public String ProductDescription { get; set; }
        public String Issue { get; set; }
        public String IssueType { get; set; }
        public String AssignedToUserFullname { get; set; }
        public String TargetDateTime { get; set; }
        public String CloseDateTime { get; set; }
        public String Status { get; set; }
    }
}