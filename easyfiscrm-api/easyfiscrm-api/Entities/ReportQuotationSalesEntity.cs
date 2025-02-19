using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace easyfiscrm_api.Entities
{
    public class ReportQuotationSalesEntity
    {
        public String LDNumber { get; set; }
        public String Customer { get; set; }
        public String LDQuotationDate { get; set; }
        public String ProductDescription { get; set; }
        public Decimal TotalAmount { get; set; }
        public String ExpectedInvoicedDate { get; set; }
        public String AssignedToUser { get; set; }
        public String LastActivity { get; set; }
        public String LastActivityDate { get; set; }
        public String LastActivityStaff { get; set; }
    }
}