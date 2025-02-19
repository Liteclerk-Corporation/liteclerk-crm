using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace easyfiscrm_api.Entities
{
    public class CrmTrnSupportEntity
    {
        public Int32 Id { get; set; }
        public String SPNumber { get; set; }
        public String SPDate { get; set; }
        public Int32 CustomerId { get; set; }
        public String Customer { get; set; }
        public String Product { get; set; }
        public Int32 SDId { get; set; }
        public String SDNumber { get; set; }
        public String ContactPerson { get; set; }
        public String ContactPosition { get; set; }
        public String ContactEmail { get; set; }
        public String ContactPhoneNumber { get; set; }
        public String PointOfContact { get; set; }
        public String Issue { get; set; }
        public Int32 AssignedToUserId { get; set; }
        public String AssignedToUser { get; set; }
        public String Status { get; set; }
        public String LastActivity { get; set; }
        public String LastActivityDate { get; set; }
        public Boolean IsLocked { get; set; }
        public Int32 CreatedByUserId { get; set; }
        public String CreatedByUser { get; set; }
        public String CreatedDateTime { get; set; }
        public Int32 UpdatedByUserId { get; set; }
        public String UpdatedByUser { get; set; }
        public String UpdatedDateTime { get; set; }
    }
}