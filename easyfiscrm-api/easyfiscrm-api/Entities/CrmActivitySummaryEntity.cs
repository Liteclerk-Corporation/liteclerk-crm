using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace easyfiscrm_api.Entities
{
    public class CrmActivitySummaryEntity
    {
        public Int32 Id { get; set; }
        public String Date { get; set; }
        public String DocType { get; set; }
        public String Reference { get; set; }
        public String Customer { get; set; }
        public String Product { get; set; }
        public String Particular { get; set; }
        public String LastActivity { get; set; }
        public String Status { get; set; }
        public Int32 AssignedToId { get; set; }
        public String AssignedTo { get; set; }
        public String CreatedBy { get; set; }
    }
}