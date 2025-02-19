using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace easyfiscrm_api.Entities
{
    public class CrmMstUserEntity
    {
        public Int32 Id { get; set; }
        public String UserId { get; set; }
        public String UserName { get; set; }
        public String Password { get; set; }
        public String FullName { get; set; }
        public String Email { get; set; }
        public Int32 CompanyId { get; set; }
        public Int32 BranchId { get; set; }
        public Int32 IncomeAccountId { get; set; }
        public Int32 SupplierAdvancesAccountId { get; set; }
        public Int32 CustomerAdvancesAccountId { get; set; }
        public String InventoryType { get; set; }
        public Int32 DefaultSalesInvoiceDiscountId { get; set; }
        public String SalesInvoiceName { get; set; }
        public String SalesInvoicePrefix { get; set; }
        public Int32 SalesInvoiceCheckedById { get; set; }
        public Int32 SalesInvoiceApprovedById { get; set; }
        public String OfficialReceiptName { get; set; }
        public String OfficialReceiptPrefix { get; set; }
        public Boolean IsIncludeCostStockReports { get; set; }
        public Boolean IsIncludeBranchRRItem { get; set; }
        public Boolean ActivateAuditTrail { get; set; }
        public Boolean IsSIVATAnalysisIncluded { get; set; }
        public Boolean IsSIDuplicateDocumentReferenceAllowed { get; set; }
        public Int32 CustomerReturnAccountId { get; set; }
        public Int32 SupplierReturnAccountId { get; set; }
        public Boolean UsePriceStockTransfer { get; set; }
        public Boolean IsPriceBelowCostSellingAllowed { get; set; }
        public Boolean IsNegativeInventorySellingAllowed { get; set; }
        public Boolean IsIncludeTotalQuantityPerUnit { get; set; }
        public Boolean ORDuplicateSIReferenceAllowed { get; set; }
        public Boolean CVDuplicateRRReferenceAllowed { get; set; }
        public Boolean IsLocked { get; set; }
        public String CRMUserGroup { get; set; }
        public Int32? CreatedById { get; set; }
        public String CreatedBy { get; set; }
        public String CreatedDateTime { get; set; }
        public Int32? UpdatedById { get; set; }
        public String UpdatedBy { get; set; }
        public String UpdatedDateTime { get; set; }
    }
}