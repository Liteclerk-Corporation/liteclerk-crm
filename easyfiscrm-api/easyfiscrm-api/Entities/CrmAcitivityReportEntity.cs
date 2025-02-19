using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace easyfiscrm_api.Entities
{
    public class CrmAcitivityReportEntity
    {
        public Int32 Id { get; set; }
        public String Document { get; set; }
        public String ACNumber { get; set; }
        public String ACDate { get; set; }
        public String User { get; set; }
        public String Functional { get; set; }
        public Int32 TechnicalUserId { get; set; }
        public String Technical { get; set; }
        public String CRMStatus { get; set; }
        public String Activity { get; set; }
        public String StartDateTime { get; set; }
        public String EndDateTime { get; set; }
        public Decimal TransportationCost { get; set; }
        public Decimal OnSiteCost { get; set; }
        public String Status { get; set; }
        public Boolean IsLocked { get; set; }
        public String CreatedByUser { get; set; }
        public String CreatedDateTime { get; set; }
        public String UpdatedByUser { get; set; }
        public String UpdatedDateTime { get; set; }
    }
}