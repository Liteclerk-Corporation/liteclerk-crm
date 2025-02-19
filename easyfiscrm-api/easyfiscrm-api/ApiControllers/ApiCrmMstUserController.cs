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
    [RoutePrefix("api/crm/user")]
    public class ApiCrmMstUserController : ApiController
    {
        public Data.easyfiscrmdbDataContext db = new Data.easyfiscrmdbDataContext();

        // ==================
        // Get User Full Name
        // ==================
        public String GetCurrentUserFullName(Int32? id)
        {
            if (id != null)
            {
                var mstUser = from d in db.MstUsers
                              where d.Id == id
                              select d;

                if (mstUser.Any())
                {
                    return mstUser.FirstOrDefault().FullName;
                }
                else
                {
                    return "";
                }
            }
            else
            {
                return "";
            }
        }

        // =========
        // List User
        // =========
        [Authorize, HttpGet, Route("list/user")]
        public List<Entities.CrmMstUserEntity> ListUser()
        {
            var currentUser = from d in db.MstUsers
                              where d.UserId == User.Identity.GetUserId()
                              select d;

            var currentUserName = currentUser.FirstOrDefault().UserName;

            if (currentUserName.Equals("admin"))
            {
                var users = from d in db.MstUsers
                            select new Entities.CrmMstUserEntity
                            {
                                Id = d.Id,
                                UserName = d.UserName,
                                FullName = d.FullName,
                                Email = d.AspNetUser.Email,
                                IsLocked = d.IsLocked,
                                CRMUserGroup = d.CRMUserGroup,
                                CreatedBy = GetCurrentUserFullName(d.CreatedById),
                                CreatedDateTime = d.CreatedDateTime.ToShortDateString(),
                                UpdatedBy = GetCurrentUserFullName(d.UpdatedById),
                                UpdatedDateTime = d.UpdatedDateTime.ToShortDateString()
                            };

                return users.ToList();
            }
            else
            {
                var users = from d in db.MstUsers
                            where !d.UserName.Equals("admin")
                            select new Entities.CrmMstUserEntity
                            {
                                Id = d.Id,
                                UserName = d.UserName,
                                FullName = d.FullName,
                                Email = d.AspNetUser.Email,
                                IsLocked = d.IsLocked,
                                CRMUserGroup = d.CRMUserGroup,
                                CreatedBy = GetCurrentUserFullName(d.CreatedById),
                                CreatedDateTime = d.CreatedDateTime.ToShortDateString(),
                                UpdatedBy = GetCurrentUserFullName(d.UpdatedById),
                                UpdatedDateTime = d.UpdatedDateTime.ToShortDateString()
                            };

                return users.ToList();
            }
        }

        [HttpGet, Route("userGroup/{username}")]
        public String userGroup(String username)
        {
            var currentUser = from d in db.MstUsers where d.UserName == username select d;

            return currentUser.FirstOrDefault().CRMUserGroup;
        }

        [HttpGet, Route("list/group")]
        public List<Entities.CrmGroupEntity> listGroup()
        {
            List<Entities.CrmGroupEntity> grouList = new List<Entities.CrmGroupEntity>();
            grouList.Add(new Entities.CrmGroupEntity { Group = "Sales" });
            grouList.Add(new Entities.CrmGroupEntity { Group = "Sales Manager" });
            grouList.Add(new Entities.CrmGroupEntity { Group = "Delivery" });
            grouList.Add(new Entities.CrmGroupEntity { Group = "Delivery Manager" });
            grouList.Add(new Entities.CrmGroupEntity { Group = "Customer" });
            grouList.Add(new Entities.CrmGroupEntity { Group = "Support" });
            grouList.Add(new Entities.CrmGroupEntity { Group = "Support Manager" });
            grouList.Add(new Entities.CrmGroupEntity { Group = "Easyfis Staff" });
            grouList.Add(new Entities.CrmGroupEntity { Group = "Admin" });

            return grouList.OrderBy(d => d.Group).ToList();
        }

        // ===========
        // Detail User
        // ===========
        [Authorize, HttpGet, Route("api/user/detail/{id}")]
        public Entities.CrmMstUserEntity DetailUser(String id)
        {
            var user = from d in db.MstUsers
                       where d.Id == Convert.ToInt32(id)
                       select new Entities.CrmMstUserEntity
                       {
                           Id = d.Id,
                           UserId = d.UserId,
                           UserName = d.UserName,
                           Password = d.Password,
                           FullName = d.FullName,
                           Email = d.AspNetUser.Email,
                           CompanyId = d.CompanyId,
                           BranchId = d.BranchId,
                           IncomeAccountId = d.IncomeAccountId,
                           SupplierAdvancesAccountId = d.SupplierAdvancesAccountId,
                           CustomerAdvancesAccountId = d.CustomerAdvancesAccountId,
                           InventoryType = d.InventoryType,
                           DefaultSalesInvoiceDiscountId = d.DefaultSalesInvoiceDiscountId,
                           SalesInvoiceName = d.SalesInvoiceName,
                           SalesInvoiceCheckedById = d.SalesInvoiceCheckedById,
                           SalesInvoiceApprovedById = d.SalesInvoiceApprovedById,
                           OfficialReceiptName = d.OfficialReceiptName,
                           IsIncludeCostStockReports = d.IsIncludeCostStockReports,
                           ActivateAuditTrail = d.ActivateAuditTrail,
                           IsSIVATAnalysisIncluded = d.IsSIVATAnalysisIncluded,
                           IsSIDuplicateDocumentReferenceAllowed = d.IsSIDuplicateDocumentReferenceAllowed,
                           CustomerReturnAccountId = d.CustomerReturnAccountId,
                           SupplierReturnAccountId = d.SupplierReturnAccountId,
                           IsLocked = d.IsLocked,
                           CreatedBy = GetCurrentUserFullName(d.CreatedById),
                           CreatedDateTime = d.CreatedDateTime.ToShortDateString(),
                           UpdatedBy = GetCurrentUserFullName(d.UpdatedById),
                           UpdatedDateTime = d.UpdatedDateTime.ToShortDateString()
                       };

            if (user.Any())
            {
                return user.FirstOrDefault();
            }
            else
            {
                return null;
            }
        }

        // ===============================
        // Dropdown List - Company (Field)
        // ===============================
        [Authorize, HttpGet, Route("api/user/dropdown/list/company")]
        public List<Entities.MstCompanyEntity> DropdownListUserListCompany()
        {
            var companies = from d in db.MstCompanies.OrderBy(d => d.Company)
                            select new Entities.MstCompanyEntity
                            {
                                Id = d.Id,
                                Company = d.Company
                            };

            return companies.ToList();
        }

        // ==============================
        // Dropdown List - Branch (Field)
        // ==============================
        [Authorize, HttpGet, Route("api/user/dropdown/list/branch/{companyId}")]
        public List<Entities.MstBranchEntity> DropdownListUserBranch(String companyId)
        {
            var branches = from d in db.MstBranches.OrderBy(d => d.Branch)
                           where d.CompanyId == Convert.ToInt32(companyId)
                           select new Entities.MstBranchEntity
                           {
                               Id = d.Id,
                               Branch = d.Branch
                           };

            return branches.ToList();
        }

        // =========
        // Save User
        // =========
        [Authorize, HttpPut, Route("update/{id}")]
        public HttpResponseMessage SaveUser(Entities.CrmMstUserEntity objUser, String id)
        {
            try
            {
                var currentUser = from d in db.MstUsers where d.UserId == User.Identity.GetUserId() select d;
                if (currentUser.Any())
                {
                    var currentUserId = currentUser.FirstOrDefault().Id;

                    var user = from d in db.MstUsers where d.Id == Convert.ToInt32(id) select d;
                    if (user.Any())
                    {
                        var saveUser = user.FirstOrDefault();
                        saveUser.FullName = objUser.FullName;
                        //saveUser.CompanyId = objUser.CompanyId;
                        saveUser.CRMUserGroup = objUser.CRMUserGroup;
                        saveUser.UpdatedById = currentUserId;
                        saveUser.UpdatedDateTime = DateTime.Now;
                        db.SubmitChanges();

                        return Request.CreateResponse(HttpStatusCode.OK);
                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.NotFound, "Data not found. These user details are not found in the server.");
                    }
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "Theres no current user logged in.");
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, "Something's went wrong from the server.");
            }
        }
    }
}
