using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace easyfiscrm_api.Entities
{
    public class CrmTrnActivityEntity
    {
        public Int32 Id { get; set; }
        public String ACNumber { get; set; }
        public String ACDate { get; set; }
        public Int32 UserId { get; set; }
        public String User { get; set; }
        public Int32? FunctionalUserId { get; set; }
        public String FunctionalUser { get; set; }
        public Int32? TechnicalUserId { get; set; }
        public String TechnicalUser { get; set; }
        public String CRMStatus { get; set; }
        public String Activity { get; set; }
        public String StartDate { get; set; }
        public String StartTime { get; set; }
        public String EndDate { get; set; }
        public String EndTime { get; set; }
        public Decimal TransportationCost { get; set; }
        public Decimal OnSiteCost { get; set; }
        public Int32? LDId { get; set; }
        public Int32? SDId { get; set; }
        public Int32? SPId { get; set; }
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