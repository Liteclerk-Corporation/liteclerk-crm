using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace easyfiscrm_api.Entities
{
    public class ReportSupportStaffEntity
    {
        public Int32 StaffId { get; set; }
        public String Staff { get; set; }
        public Int32 Open { get; set; }
        public Int32 ForClosing { get; set; }
        public Int32 Close { get; set; }
        public Int32 Cancelled { get; set; }
        public Int32 Total { get; set; }
    }
}