using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace easyfiscrm_api.Entities
{
    public class CrmTrnSoftwareDevelopmentEntity
    {
        public Int32 Id { get; set; }
        public String SDNumber { get; set; }
        public String SDDate { get; set; }
        public Int32 ProductId { get; set; }
        public String ProductDescription { get; set; }
        public String Issue { get; set; }
        public String IssueType { get; set; }
        public String Remarks { get; set; }
        public Int32 AssignedToUserId { get; set; }
        public String AssignedToUserFullname { get; set; }
        public String TargetDateTime { get; set; }
        public String CloseDateTime { get; set; }
        public String Status { get; set; }
        public Boolean IsLocked { get; set; }
        public Int32 CreatedByUserId { get; set; }
        public String CreatedByUserFullname { get; set; }
        public String CreatedDateTime { get; set; }
        public Int32 UpdatedByUserId { get; set; }
        public String UpdatedByUserFullname { get; set; }
        public String UpdatedDateTime { get; set; }
    }
}