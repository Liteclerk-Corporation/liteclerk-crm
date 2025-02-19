using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace easyfiscrm_api.Entities
{
    public class CrmTrnSalesInvoiceEntity
    {
        public Int32 Id { get; set; }
        public Int32 BranchId { get; set; }
        public String SINumber { get; set; }
        public String SalesInvoice { get; set; }
        public String SIDate { get; set; }
        public Int32 CustomerId { get; set; }
        public String Customer { get; set; }
        public Int32 TermId { get; set; }
        public String Term { get; set; }
        public String DocumentReference { get; set; }
        public String ManualSINumber { get; set; }
        public String Remarks { get; set; }
        public Decimal Amount { get; set; }
        public Decimal PaidAmount { get; set; }
        public Decimal AdjustmentAmount { get; set; }
        public Decimal BalanceAmount { get; set; }
        public Int32 SoldById { get; set; }
        public String SoldBy { get; set; }
        public Int32 PreparedById { get; set; }
        public Int32 CheckedById { get; set; }
        public Int32 ApprovedById { get; set; }
        public String Status { get; set; }
        public Boolean IsCancelled { get; set; }
        public Boolean IsPrinted { get; set; }
        public Boolean IsLocked { get; set; }
    }
}