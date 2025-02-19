using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace easyfiscrm_api.Entities
{
    public class ReportOpenSalesDeliveryEntity
    {
        public String SDNumber { get; set; }
        public String Customer { get; set; }
        public String SDDeliveryDate  { get; set; }
        public String ProductDescription { get; set; }
        public String Amount { get; set; }
        public String ExpectedAcceptanceDate { get; set; }
        public String AssignedToUser { get; set; }
        public String LastActivity { get; set; }
        public String LastActivityDate { get; set; }
        public String LastActivityStaff { get; set; }
    }
}