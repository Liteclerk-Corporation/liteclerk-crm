﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace easyfiscrm_api.Entities
{
    public class MstArticleEntity
    {
        public Int32 Id { get; set; }
        public String ArticleCode { get; set; }
        public String ManualArticleCode { get; set; }
        public String Article { get; set; }
        public String Category { get; set; }
        public Int32 ArticleTypeId { get; set; }
        public Int32 ArticleGroupId { get; set; }
        public Int32 AccountId { get; set; }
        public Int32 SalesAccountId { get; set; }
        public Int32 CostAccountId { get; set; }
        public Int32 AssetAccountId { get; set; }
        public Int32 ExpenseAccountId { get; set; }
        public Int32 UnitId { get; set; }
        public Int32 OutputTaxId { get; set; }
        public Int32 InputTaxId { get; set; }
        public Int32 WTaxTypeId { get; set; }
        public Decimal Price { get; set; }
        public Decimal Cost { get; set; }
        public Boolean IsInventory { get; set; }
        public Boolean IsConsignment { get; set; }
        public Decimal ConsignmentCostPercentage { get; set; }
        public Decimal ConsignmentCostValue { get; set; }
        public String Particulars { get; set; }
        public String Address { get; set; }
        public Int32 TermId { get; set; }
        public String ContactNumber { get; set; }
        public String ContactPerson { get; set; }
        public String EmailAddress { get; set; }
        public String TaxNumber { get; set; }
        public Decimal CreditLimit { get; set; }
        public DateTime DateAcquired { get; set; }
        public Decimal UsefulLife { get; set; }
        public Decimal SalvageValue { get; set; }
        public String ManualArticleOldCode { get; set; }
        public Int32 Kitting { get; set; }
        public Int32? DefaultSupplierId { get; set; }
        public Decimal StockLevelQuantity { get; set; }
        public Decimal ReorderQuantity { get; set; }
        public Boolean IsLocked { get; set; }
        public Int32 CreatedById { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public Int32 UpdatedById { get; set; }
        public DateTime UpdatedDateTime { get; set; }
    }
}