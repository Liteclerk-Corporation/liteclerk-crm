using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace easyfiscrm_api.Entities
{
    public class CrmTrnLeadEntity
    {
        public Int32 Id { get; set; }
        public String LDNumber { get; set; }
        public String LDDate { get; set; }
        public String Name { get; set; }
        public Int32 ProductId { get; set; }
        public String ProductDescription { get; set; }
        public String Address { get; set; }
        public String ContactPerson { get; set; }
        public String ContactPosition { get; set; }
        public String ContactEmail { get; set; }
        public String ContactPhoneNumber { get; set; }
        public String ReferredBy { get; set; }
        public String Remarks { get; set; }
        public Decimal TotalAmount { get; set; }
        public Int32 AssignedToUserId { get; set; }
        public String AssignedToUser { get; set; }
        public String LastActivity { get; set; }
        public String LastActivityDate { get; set; }
        public String Status { get; set; }
        public Boolean IsLocked { get; set; }
        public Int32 CreatedByUserId { get; set; }
        public String CreatedByUser { get; set; }
        public String CreatedDateTime { get; set; }
        public Int32 UpdatedByUserId { get; set; }
        public String UpdatedByUser { get; set; }
        public String UpdatedDateTime { get; set; }
    }
}