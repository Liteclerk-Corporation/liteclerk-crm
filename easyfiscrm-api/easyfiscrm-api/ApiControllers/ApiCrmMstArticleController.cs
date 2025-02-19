using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace easyfiscrm_api.ApiControllers
{
    [Authorize, RoutePrefix("api/crm/mst/customer")]
    public class ApiCrmMstArticleController : ApiController
    {
        //public Data.easyfiscrmdbDataContext db = new Data.easyfiscrmdbDataContext();

        //public String FillLeadingZeroes(Int32 number, Int32 length)
        //{
        //    String result = number.ToString();
        //    Int32 pad = length - result.Length;
        //    while (pad > 0) { result = '0' + result; pad--; }

        //    return result;
        //}

        //[HttpGet, Route("list")]
        //public List<Entities.MstArticle> ListCustomer()
        //{
        //    var customers = from d in db.MstArticles
        //                    where d.IsLocked == true
        //                    select new Entities.MstArticle
        //                    {
        //                        Id = d.Id,
        //                        ArticleCode = d.ArticleCode,
        //                        ManualArticleCode = d.ManualArticleCode,
        //                        Article = d.Article,
        //                        Category = d.Category,
        //                        ArticleTypeId = d.ArticleGroupId,
        //                        ArticleGroupId = d.ArticleGroupId,
        //                        AccountId = d.AccountId,
        //                        SalesAccountId = d.SalesAccountId,
        //                        CostAccountId = d.CostAccountId,
        //                        AssetAccountId = d.AssetAccountId,
        //                        ExpenseAccountId = d.ExpenseAccountId,
        //                        UnitId = d.UnitId,
        //                        OutputTaxId = d.OutputTaxId,
        //                        InputTaxId = d.InputTaxId,
        //                        WTaxTypeId = d.WTaxTypeId,
        //                        Price = d.Price,
        //                        Cost = d.Cost,
        //                        IsInventory = d.IsInventory,
        //                        IsConsignment = d.IsConsignment,
        //                        ConsignmentCostPercentage = d.ConsignmentCostPercentage,
        //                        ConsignmentCostValue = d.ConsignmentCostValue,
        //                        Particulars = d.Particulars,
        //                        Address = d.Address,
        //                        TermId = d.TermId,
        //                        ContactNumber = d.ContactNumber,
        //                        ContactPerson = d.ContactPerson,
        //                        EmailAddress = d.EmailAddress,
        //                        TaxNumber = d.TaxNumber,
        //                        CreditLimit = d.CreditLimit,
        //                        DateAcquired = d.DateAcquired,
        //                        UsefulLife = d.UsefulLife,
        //                        SalvageValue = d.SalvageValue,
        //                        ManualArticleOldCode = d.ManualArticleCode,
        //                        Kitting = d.Kitting,
        //                        DefaultSupplierId = d.DefaultSupplierId,
        //                        StockLevelQuantity = d.StockLevelQuantity,
        //                        ReorderQuantity = d.ReorderQuantity,
        //                        IsLocked = d.IsLocked,
        //                        CreatedById = d.CreatedById,
        //                        CreatedDateTime = d.CreatedDateTime,
        //                        UpdatedById = d.UpdatedById,
        //                        UpdatedDateTime = d.UpdatedDateTime
        //                    };

        //    return customers.ToList();
        //}

        //[HttpPost, Route("Add")]
        //public HttpResponseMessage AddArticle()
        //{
        //    try
        //    {
        //        var currentUser = from d in db.MstUsers where d.UserId == User.Identity.GetUserId() select d;
        //        if (currentUser.Any())
        //        {
        //            String articleCode = "0000000001";

        //            var lastArticleCode = from d in db.MstArticles.OrderByDescending(d => d.Id) select d;
        //            if (lastArticleCode.Any())
        //            {
        //                Int32 newLDNumber = Convert.ToInt32(lastArticleCode.FirstOrDefault().ArticleCode) + 0000000001;
        //                articleCode = FillLeadingZeroes(newLDNumber, 10);
        //            }

        //            Int32 articleTypeId = 0, articleGroupId = 0, accountId = 0, salesAccountId = 0, costAccountId = 0, assetAccountId = 0, expenseAccountId = 0, unitId = 0, outputTaxId = 0, inputTaxId = 0, wTaxTypeId = 0, termId = 0, defaultSupplierId = 0;

        //            var articleType = from d in db.MstArticleTypes
        //                              where d.Id == 2
        //                              select d;
        //            if (articleType.Any())
        //            {
        //                articleTypeId = articleType.FirstOrDefault().Id;
        //            }

        //            var articleGroup = from d in db.MstArticleGroups
        //                               where d.ArticleTypeId == 2
        //                               select d;

        //            if (articleGroup.Any())
        //            {
        //                articleGroupId = articleGroup.FirstOrDefault().Id;
        //            }

        //            var account = from d in db.MstArticleGroups
        //                          where d.ArticleTypeId == 2
        //                          select d;

        //            if (account.Any())
        //            {
        //                accountId = account.FirstOrDefault().AccountId;
        //                salesAccountId = account.FirstOrDefault().SalesAccountId;
        //                costAccountId = account.FirstOrDefault().CostAccountId;
        //                assetAccountId = account.FirstOrDefault().AssetAccountId;
        //                expenseAccountId = account.FirstOrDefault().ExpenseAccountId;
        //            }

        //            var unit = from d in db.MstUnits
        //                       select d;

        //            if (unit.Any())
        //            {
        //                unitId = unit.FirstOrDefault().Id;
        //            }

        //            var outputTax = from d in db.MstTaxTypes
        //                            where d.TaxType.Equals("VAT Output")
        //                            select d;

        //            if (outputTax.Any())
        //            {
        //                outputTaxId = outputTax.FirstOrDefault().Id;
        //            }

        //            var inputTax = from d in db.MstTaxTypes
        //                           where d.TaxType.Equals("VAT Input")
        //                           select d;

        //            if (inputTax.Any())
        //            {
        //                inputTaxId = inputTax.FirstOrDefault().Id;
        //            }

        //            var wTaxType = from d in db.MstTaxTypes
        //                           where d.TaxType.Equals("WITHHOLDING TAX")
        //                           select d;

        //            if (wTaxType.Any())
        //            {
        //                wTaxTypeId = wTaxType.FirstOrDefault().Id;
        //            }

        //            var term = from d in db.MstTerms
        //                       select d;
        //            if (term.Any())
        //            {
        //                termId = term.FirstOrDefault().Id;
        //            }

        //            Data.MstArticle newArticle = new Data.MstArticle()
        //            {
        //                ArticleCode = articleCode,
        //                ManualArticleCode = "NA",
        //                Article = "NA",
        //                Category = "NA",
        //                ArticleTypeId = articleTypeId,
        //                ArticleGroupId = articleGroupId,
        //                AccountId = accountId,
        //                SalesAccountId = salesAccountId,
        //                CostAccountId = costAccountId,
        //                AssetAccountId = assetAccountId,
        //                ExpenseAccountId = expenseAccountId,
        //                UnitId = unitId,
        //                OutputTaxId = outputTaxId,
        //                InputTaxId = inputTaxId,
        //                WTaxTypeId = wTaxTypeId,
        //                Price = 0,
        //                Cost = 0,
        //                IsInventory = false,
        //                IsConsignment = false,
        //                ConsignmentCostPercentage = 0,
        //                ConsignmentCostValue = 0,
        //                Particulars = "NA",
        //                Address = "NA",
        //                TermId = termId,
        //                ContactNumber = "NA",
        //                ContactPerson = "NA",
        //                EmailAddress = "NA",
        //                TaxNumber = "NA",
        //                CreditLimit = 0,
        //                DateAcquired = DateTime.Today,
        //                UsefulLife = 0,
        //                SalvageValue = 0,
        //                ManualArticleOldCode = "NA",
        //                Kitting = 0,
        //                DefaultSupplierId = null,
        //                StockLevelQuantity = 0,
        //                ReorderQuantity = 0,
        //                IsLocked = false,
        //                CreatedById = currentUser.FirstOrDefault().Id,
        //                CreatedDateTime = DateTime.Today,
        //                UpdatedById = currentUser.FirstOrDefault().Id,
        //                UpdatedDateTime = DateTime.Today
        //            };

        //            db.MstArticles.InsertOnSubmit(newArticle);
        //            db.SubmitChanges();

        //            return Request.CreateResponse(HttpStatusCode.OK, newArticle.Id);
        //        }
        //        else
        //        {
        //            return Request.CreateResponse(HttpStatusCode.NotFound, "User not found!");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
        //    }
        //}

        //[HttpGet, Route("save/{id}")]
        //// ============
        //// Add Customer
        //// ============
        //[Authorize, HttpPost, Route("api/customer/add")]
        //public HttpResponseMessage AddCustomer()
        //{
        //    try
        //    {
        //        var currentUser = from d in db.MstUsers
        //                          where d.UserId == User.Identity.GetUserId()
        //                          select d;

        //        if (currentUser.Any())
        //        {
        //            var currentUserId = currentUser.FirstOrDefault().Id;


        //            var defaultCustomerCode = "0000000001";
        //            var lastCustomer = from d in db.MstArticles.OrderByDescending(d => d.Id)
        //                               where d.ArticleTypeId == 2
        //                               select d;

        //            if (lastCustomer.Any())
        //            {
        //                var customerCode = Convert.ToInt32(lastCustomer.FirstOrDefault().ArticleCode) + 0000000001;
        //                defaultCustomerCode = FillLeadingZeroes(customerCode, 10);
        //            }

        //            var articleGroups = from d in db.MstArticleGroups
        //                                where d.ArticleTypeId == 2
        //                                select d;

        //            if (articleGroups.Any())
        //            {
        //                var units = from d in db.MstUnits
        //                            select d;

        //                if (units.Any())
        //                {
        //                    var taxTypes = from d in db.MstTaxTypes
        //                                   select d;

        //                    if (taxTypes.Any())
        //                    {
        //                        var terms = from d in db.MstTerms
        //                                    select d;

        //                        if (terms.Any())
        //                        {
        //                            Data.MstArticle newCustomer = new Data.MstArticle
        //                            {
        //                                ArticleCode = defaultCustomerCode,
        //                                ManualArticleCode = defaultCustomerCode,
        //                                Article = "NA",
        //                                Category = "NA",
        //                                ArticleTypeId = 2,
        //                                ArticleGroupId = articleGroups.FirstOrDefault().Id,
        //                                AccountId = articleGroups.FirstOrDefault().AccountId,
        //                                SalesAccountId = articleGroups.FirstOrDefault().SalesAccountId,
        //                                CostAccountId = articleGroups.FirstOrDefault().CostAccountId,
        //                                AssetAccountId = articleGroups.FirstOrDefault().AssetAccountId,
        //                                ExpenseAccountId = articleGroups.FirstOrDefault().ExpenseAccountId,
        //                                UnitId = units.FirstOrDefault().Id,
        //                                OutputTaxId = db.MstTaxTypes.FirstOrDefault().Id,
        //                                InputTaxId = db.MstTaxTypes.FirstOrDefault().Id,
        //                                WTaxTypeId = db.MstTaxTypes.FirstOrDefault().Id,
        //                                Price = 0,
        //                                Cost = 0,
        //                                IsInventory = false,
        //                                Particulars = "NA",
        //                                Address = "NA",
        //                                TermId = terms.FirstOrDefault().Id,
        //                                ContactNumber = "NA",
        //                                ContactPerson = "NA",
        //                                EmailAddress = "NA",
        //                                TaxNumber = "NA",
        //                                CreditLimit = 0,
        //                                DateAcquired = DateTime.Now,
        //                                UsefulLife = 0,
        //                                SalvageValue = 0,
        //                                ManualArticleOldCode = "NA",
        //                                Kitting = 0,
        //                                DefaultSupplierId = null,
        //                                StockLevelQuantity = 0,
        //                                ReorderQuantity = 0,
        //                                IsLocked = false,
        //                                CreatedById = currentUserId,
        //                                CreatedDateTime = DateTime.Now,
        //                                UpdatedById = currentUserId,
        //                                UpdatedDateTime = DateTime.Now
        //                            };

        //                            db.MstArticles.InsertOnSubmit(newCustomer);
        //                            db.SubmitChanges();

        //                            return Request.CreateResponse(HttpStatusCode.OK, newCustomer.Id);
        //                        }
        //                        else
        //                        {
        //                            return Request.CreateResponse(HttpStatusCode.NotFound, "No term found. Please setup more terms for all master tables.");
        //                        }
        //                    }
        //                    else
        //                    {
        //                        return Request.CreateResponse(HttpStatusCode.NotFound, "No tax type found. Please setup more tax types for all master tables.");
        //                    }
        //                }
        //                else
        //                {
        //                    return Request.CreateResponse(HttpStatusCode.NotFound, "No unit found. Please setup more units for all master tables.");
        //                }
        //            }
        //            else
        //            {
        //                return Request.CreateResponse(HttpStatusCode.NotFound, "No article group found. Please setup at least one article group for customers.");
        //            }
        //        }
        //        else
        //        {
        //            return Request.CreateResponse(HttpStatusCode.BadRequest, "Theres no current user logged in.");
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        Debug.WriteLine(e);
        //        return Request.CreateResponse(HttpStatusCode.InternalServerError, "Something's went wrong from the server.");
        //    }
        //}

        //// =============
        //// Save Customer
        //// =============
        //[HttpPut, Route("api/customer/save/{id}")]
        //public HttpResponseMessage SaveCustomer(Entities.MstArticle objCustomer, String id)
        //{
        //    try
        //    {
        //        var currentUser = from d in db.MstUsers
        //                          where d.UserId == User.Identity.GetUserId()
        //                          select d;

        //        if (currentUser.Any())
        //        {
        //            var currentUserId = currentUser.FirstOrDefault().Id;

        //            var customer = from d in db.MstArticles
        //                           where d.Id == Convert.ToInt32(id)
        //                           && d.ArticleTypeId == 2
        //                           select d;

        //            if (customer.Any())
        //            {
        //                if (!customer.FirstOrDefault().IsLocked)
        //                {
        //                    var saveCustomer = customer.FirstOrDefault();
        //                    saveCustomer.ManualArticleCode = objCustomer.ManualArticleCode;
        //                    saveCustomer.Article = objCustomer.Article;
        //                    saveCustomer.ArticleGroupId = objCustomer.ArticleGroupId;
        //                    saveCustomer.AccountId = objCustomer.AccountId;
        //                    saveCustomer.SalesAccountId = objCustomer.SalesAccountId;
        //                    saveCustomer.CostAccountId = objCustomer.CostAccountId;
        //                    saveCustomer.AssetAccountId = objCustomer.AssetAccountId;
        //                    saveCustomer.ExpenseAccountId = objCustomer.ExpenseAccountId;
        //                    saveCustomer.TermId = objCustomer.TermId;
        //                    saveCustomer.Address = objCustomer.Address;
        //                    saveCustomer.CreditLimit = objCustomer.CreditLimit;
        //                    saveCustomer.ContactNumber = objCustomer.ContactNumber;
        //                    saveCustomer.ContactPerson = objCustomer.ContactPerson;
        //                    saveCustomer.TaxNumber = objCustomer.TaxNumber;
        //                    saveCustomer.Particulars = objCustomer.Particulars;
        //                    saveCustomer.EmailAddress = objCustomer.EmailAddress;
        //                    saveCustomer.UpdatedById = currentUserId;
        //                    saveCustomer.UpdatedDateTime = DateTime.Now;
        //                    db.SubmitChanges();

        //                    return Request.CreateResponse(HttpStatusCode.OK);
        //                }
        //                else
        //                {
        //                    return Request.CreateResponse(HttpStatusCode.BadRequest, "Saving Error. These customer details are already locked.");
        //                }
        //            }
        //            else
        //            {
        //                return Request.CreateResponse(HttpStatusCode.NotFound, "Data not found. These customer details are not found in the server.");
        //            }
        //        }
        //        else
        //        {
        //            return Request.CreateResponse(HttpStatusCode.BadRequest, "Theres no current user logged in.");
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        Debug.WriteLine(e);
        //        return Request.CreateResponse(HttpStatusCode.InternalServerError, "Something's went wrong from the server.");
        //    }
        //}

        //// =============
        //// Lock Customer
        //// =============
        //[HttpPut, Route("api/customer/lock/{id}")]
        //public HttpResponseMessage LockCustomer(Entities.MstArticle objCustomer, String id)
        //{
        //    try
        //    {
        //        var currentUser = from d in db.MstUsers
        //                          where d.UserId == User.Identity.GetUserId()
        //                          select d;

        //        if (currentUser.Any())
        //        {
        //            var currentUserId = currentUser.FirstOrDefault().Id;

        //            var customer = from d in db.MstArticles
        //                           where d.Id == Convert.ToInt32(id)
        //                           && d.ArticleTypeId == 2
        //                           select d;

        //            if (customer.Any())
        //            {
        //                if (!customer.FirstOrDefault().IsLocked)
        //                {
        //                    var customerByManualCode = from d in db.MstArticles
        //                                               where d.ArticleTypeId == 2
        //                                               && d.ManualArticleCode.Equals(objCustomer.ManualArticleCode)
        //                                               && d.IsLocked == true
        //                                               select d;

        //                    if (!customerByManualCode.Any())
        //                    {
        //                        var lockCustomer = customer.FirstOrDefault();
        //                        lockCustomer.ManualArticleCode = objCustomer.ManualArticleCode;
        //                        lockCustomer.Article = objCustomer.Article;
        //                        lockCustomer.ArticleGroupId = objCustomer.ArticleGroupId;
        //                        lockCustomer.AccountId = objCustomer.AccountId;
        //                        lockCustomer.SalesAccountId = objCustomer.SalesAccountId;
        //                        lockCustomer.CostAccountId = objCustomer.CostAccountId;
        //                        lockCustomer.AssetAccountId = objCustomer.AssetAccountId;
        //                        lockCustomer.ExpenseAccountId = objCustomer.ExpenseAccountId;
        //                        lockCustomer.TermId = objCustomer.TermId;
        //                        lockCustomer.Address = objCustomer.Address;
        //                        lockCustomer.CreditLimit = objCustomer.CreditLimit;
        //                        lockCustomer.ContactNumber = objCustomer.ContactNumber;
        //                        lockCustomer.ContactPerson = objCustomer.ContactPerson;
        //                        lockCustomer.TaxNumber = objCustomer.TaxNumber;
        //                        lockCustomer.Particulars = objCustomer.Particulars;
        //                        lockCustomer.EmailAddress = objCustomer.EmailAddress;
        //                        lockCustomer.IsLocked = true;
        //                        lockCustomer.UpdatedById = currentUserId;
        //                        lockCustomer.UpdatedDateTime = DateTime.Now;

        //                        db.SubmitChanges();

        //                        return Request.CreateResponse(HttpStatusCode.OK);
        //                    }
        //                    else
        //                    {
        //                        return Request.CreateResponse(HttpStatusCode.BadRequest, "Manual Code is already taken.");
        //                    }
        //                }
        //                else
        //                {
        //                    return Request.CreateResponse(HttpStatusCode.BadRequest, "Locking Error. These customer details are already locked.");
        //                }
        //            }
        //            else
        //            {
        //                return Request.CreateResponse(HttpStatusCode.NotFound, "Data not found. These customer details are not found in the server.");
        //            }
        //        }
        //        else
        //        {
        //            return Request.CreateResponse(HttpStatusCode.BadRequest, "Theres no current user logged in.");
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        Debug.WriteLine(e);
        //        return Request.CreateResponse(HttpStatusCode.InternalServerError, "Something's went wrong from the server.");
        //    }
        //}

        //// ===============
        //// Unlock Customer
        //// ===============
        //[HttpPut, Route("api/customer/unlock/{id}")]
        //public HttpResponseMessage UnlockCustomer(String id)
        //{
        //    try
        //    {
        //        var currentUser = from d in db.MstUsers
        //                          where d.UserId == User.Identity.GetUserId()
        //                          select d;

        //        if (currentUser.Any())
        //        {
        //            var currentUserId = currentUser.FirstOrDefault().Id;

        //            var customer = from d in db.MstArticles
        //                           where d.Id == Convert.ToInt32(id)
        //                           && d.ArticleTypeId == 2
        //                           select d;

        //            if (customer.Any())
        //            {
        //                if (customer.FirstOrDefault().IsLocked)
        //                {
        //                    var unlockCustomer = customer.FirstOrDefault();
        //                    unlockCustomer.IsLocked = false;
        //                    unlockCustomer.UpdatedById = currentUserId;
        //                    unlockCustomer.UpdatedDateTime = DateTime.Now;

        //                    db.SubmitChanges();

        //                    return Request.CreateResponse(HttpStatusCode.OK);
        //                }
        //                else
        //                {
        //                    return Request.CreateResponse(HttpStatusCode.BadRequest, "Unlocking Error. These customer details are already unlocked.");
        //                }
        //            }
        //            else
        //            {
        //                return Request.CreateResponse(HttpStatusCode.NotFound, "Data not found. These customer details are not found in the server.");
        //            }

        //        }
        //        else
        //        {
        //            return Request.CreateResponse(HttpStatusCode.BadRequest, "Theres no current user logged in.");
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        Debug.WriteLine(e);
        //        return Request.CreateResponse(HttpStatusCode.InternalServerError, "Something's went wrong from the server.");
        //    }
        //}

        //// ===============
        //// Delete Customer
        //// ===============
        //[Authorize, HttpDelete, Route("api/customer/delete/{id}")]
        //public HttpResponseMessage DeleteCustomer(String id)
        //{
        //    try
        //    {
        //        var currentUser = from d in db.MstUsers
        //                          where d.UserId == User.Identity.GetUserId()
        //                          select d;

        //        if (currentUser.Any())
        //        {
        //            var currentUserId = currentUser.FirstOrDefault().Id;

        //            var customer = from d in db.MstArticles
        //                           where d.Id == Convert.ToInt32(id)
        //                           && d.ArticleTypeId == 2
        //                           select d;

        //            if (customer.Any())
        //            {
        //                db.MstArticles.DeleteOnSubmit(customer.First());
        //                db.SubmitChanges();

        //                return Request.CreateResponse(HttpStatusCode.OK);
        //            }
        //            else
        //            {
        //                return Request.CreateResponse(HttpStatusCode.NotFound, "Data not found. This selected customer is not found in the server.");
        //            }

        //        }
        //        else
        //        {
        //            return Request.CreateResponse(HttpStatusCode.BadRequest, "Theres no current user logged in.");
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        Debug.WriteLine(e);
        //        return Request.CreateResponse(HttpStatusCode.InternalServerError, "Something's went wrong from the server.");
        //    }
        //}
    }
}
