using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace easyfiscrm_api.Entities
{
    public class CrmActivityDetailSummaryEntity
    {
        public Int32 Id { get; set; }
        public String ACNumber { get; set; }
        public String ACDate { get; set; }
        public Int32 DocumentId { get; set; }
        public String DocumentNumber { get; set; }
        public String Document { get; set; }
        public String DocumentStatus { get; set; }
        public String Customer { get; set; }
        public String Product { get; set; }
        public Int32 AssignedToId { get; set; }
        public String AssignedTo { get; set; }
        public String CreatedBy { get; set; }
        public String Particulars { get; set; }
        public String Activity { get; set; }
        public String StartDate { get; set; }
        public String EndDate { get; set; }
        public String TechnicalStaff { get; set; }
        public String FunctionalStaff { get; set; }
        public String Status { get; set; }
        public Decimal TotalAmount { get; set; }
        public Int32 CreatedByUserId { get; set; }
        public String CreatedByUser { get; set; }
        public String CreatedDateTime { get; set; }
        public Int32 UpdatedByUserId { get; set; }
        public String UpdatedByUser { get; set; }
        public String UpdatedDateTime { get; set; }
    }
}