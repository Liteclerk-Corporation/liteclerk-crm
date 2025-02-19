using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace easyfiscrm_api.Entities
{
    public class ReportSalesStaffEntity
    {
        public Int32 StaffId { get; set; }
        public String Staff { get; set; }
        public Int32 Introduction { get; set; }
        public Int32 Presentation { get; set; }
        public Int32 Quotation { get; set; }
        public Int32 Invoiced { get; set; }
        public Int32 Cancelled { get; set; }
        public Int32 Total { get; set; }
    }
}