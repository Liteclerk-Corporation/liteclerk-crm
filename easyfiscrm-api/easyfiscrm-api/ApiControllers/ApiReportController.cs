using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace easyfiscrm_api.ApiControllers
{
    [Authorize, RoutePrefix("api/crm/report")]
    public class ApiReportController : ApiController
    {
        public Data.easyfiscrmdbDataContext db = new Data.easyfiscrmdbDataContext();

        [HttpGet, Route("list/status/{document}")]
        public List<Entities.CrmMstStatusEntity> ListLeadStatus(String document)
        {
            String category = "";

            if (document == "LEAD")
            {
                category = "LD";
            }
            else if (document == "SALES DELIVERY")
            {
                category = "SD";
            }
            else if (document == "SUPPORT")
            {
                category = "SP";
            }

            var statuses = from d in db.MstStatus
                           where d.Category.Equals(category)
                           select new Entities.CrmMstStatusEntity
                           {
                               Id = d.Id,
                               Status = d.Status
                           };

            return statuses.ToList();
        }

        [HttpGet, Route("list/document")]
        public List<Entities.CrmMstStatusEntity> ListDocument()
        {
            List<Entities.CrmMstStatusEntity> document = new List<Entities.CrmMstStatusEntity>();
            document.Add(new Entities.CrmMstStatusEntity { Id = 1, Category = "ALL" });
            document.Add(new Entities.CrmMstStatusEntity { Id = 2, Category = "LEAD" });
            document.Add(new Entities.CrmMstStatusEntity { Id = 3, Category = "SALES DELIVERY" });
            document.Add(new Entities.CrmMstStatusEntity { Id = 4, Category = "SUPPORT" });
            return document.ToList();
        }

        public String GetLastLeadActivity(Int32 LDId)
        {
            var activities = from d in db.CrmTrnActivities.OrderByDescending(d => d.Id)
                             where d.LDId == LDId
                             select d;

            if (activities.Any())
            {
                return activities.FirstOrDefault().Activity;
            }

            return "";
        }

        public String GetLastLeadActivityDate(Int32 LDId)
        {
            var activities = from d in db.CrmTrnActivities.OrderByDescending(d => d.Id)
                             where d.LDId == LDId
                             select d;

            if (activities.Any())
            {
                return activities.FirstOrDefault().ACDate.ToShortDateString();
            }

            return "";
        }

        [HttpGet, Route("lead/list/{startDate}/{endDate}/{status}")]
        public List<Entities.CrmTrnLeadEntity> ListLead(String startDate, String endDate, String status)
        {
            var currentUser = from d in db.MstUsers where d.UserId == User.Identity.GetUserId() select d;

            if (currentUser.FirstOrDefault().CRMUserGroup == "Sales Manager" || currentUser.FirstOrDefault().CRMUserGroup == "Easyfis Staff" || currentUser.FirstOrDefault().CRMUserGroup == "Admin")
            {
                var leads = from d in db.CrmTrnLeads
                            where d.LDDate >= Convert.ToDateTime(startDate)
                            && d.LDDate <= Convert.ToDateTime(endDate)
                            select new Entities.CrmTrnLeadEntity
                            {
                                Id = d.Id,
                                LDNumber = d.LDNumber,
                                LDDate = d.LDDate.ToShortDateString(),
                                Name = d.Name,
                                ProductId = d.ProductId,
                                ProductDescription = d.CrmMstProduct.ProductDescription,
                                TotalAmount = d.TotalAmount,
                                Address = d.Address,
                                ContactPerson = d.ContactPerson,
                                ContactPosition = d.ContactPosition,
                                ContactEmail = d.ContactEmail,
                                ContactPhoneNumber = d.ContactPhoneNumber,
                                ReferredBy = d.ReferredBy,
                                Remarks = d.Remarks,
                                AssignedToUserId = d.AssignedToUserId,
                                AssignedToUser = d.MstUser.FullName,
                                Status = d.Status,
                                LastActivity = GetLastLeadActivity(d.Id),
                                LastActivityDate = GetLastLeadActivityDate(d.Id),
                                IsLocked = d.IsLocked,
                                CreatedByUserId = d.CreatedByUserId,
                                CreatedByUser = d.MstUser1.FullName,
                                CreatedDateTime = d.CreatedDateTime.ToShortDateString(),
                                UpdatedByUserId = d.UpdatedByUserId,
                                UpdatedByUser = d.MstUser2.FullName,
                                UpdatedDateTime = d.UpdatedDateTime.ToShortDateString()
                            };

                if (status.Equals("ALL"))
                {
                    return leads.OrderByDescending(d => d.Id).ToList();
                }
                else
                {
                    return leads.Where(d => d.Status.Equals(status)).OrderByDescending(d => d.Id).ToList();
                }
            }
            else if (currentUser.FirstOrDefault().CRMUserGroup == "Sales")
            {
                var leads = from d in db.CrmTrnLeads
                            where d.LDDate >= Convert.ToDateTime(startDate)
                            && d.LDDate <= Convert.ToDateTime(endDate)
                            && d.CreatedByUserId == currentUser.FirstOrDefault().Id
                            select new Entities.CrmTrnLeadEntity
                            {
                                Id = d.Id,
                                LDNumber = d.LDNumber,
                                LDDate = d.LDDate.ToShortDateString(),
                                Name = d.Name,
                                ProductId = d.ProductId,
                                TotalAmount = d.TotalAmount,
                                Address = d.Address,
                                ContactPerson = d.ContactPerson,
                                ContactPosition = d.ContactPosition,
                                ContactEmail = d.ContactEmail,
                                ContactPhoneNumber = d.ContactPhoneNumber,
                                ReferredBy = d.ReferredBy,
                                Remarks = d.Remarks,
                                AssignedToUserId = d.AssignedToUserId,
                                AssignedToUser = d.MstUser.FullName,
                                Status = d.Status,
                                LastActivity = GetLastLeadActivity(d.Id),
                                LastActivityDate = GetLastLeadActivityDate(d.Id),
                                IsLocked = d.IsLocked,
                                CreatedByUserId = d.CreatedByUserId,
                                CreatedByUser = d.MstUser1.FullName,
                                CreatedDateTime = d.CreatedDateTime.ToShortDateString(),
                                UpdatedByUserId = d.UpdatedByUserId,
                                UpdatedByUser = d.MstUser2.FullName,
                                UpdatedDateTime = d.UpdatedDateTime.ToShortDateString()
                            };

                if (status.Equals("ALL"))
                {
                    return leads.OrderByDescending(d => d.Id).ToList();
                }
                else
                {
                    return leads.Where(d => d.Status.Equals(status)).OrderByDescending(d => d.Id).ToList();
                }
            }
            else
            {
                return new List<Entities.CrmTrnLeadEntity>();
            }

        }

        public String GetLastSalesDeliveryActivity(Int32 SDId)
        {
            var activities = from d in db.CrmTrnActivities.OrderByDescending(d => d.Id)
                             where d.SDId == SDId
                             select d;

            if (activities.Any())
            {
                return activities.FirstOrDefault().Activity;
            }

            return "";
        }

        public String GetLastSalesDeliveryActivityDate(Int32 SDId)
        {
            var activities = from d in db.CrmTrnActivities.OrderByDescending(d => d.Id)
                             where d.SDId == SDId
                             select d;

            if (activities.Any())
            {
                return activities.FirstOrDefault().ACDate.ToShortDateString();
            }

            return "";
        }

        [HttpGet, Route("salesdelivery/list/{startDate}/{endDate}/{status}")]
        public List<Entities.CrmTrnSalesDeliveryEntity> ListSalesDelivery(String startDate, String endDate, String status)
        {
            var currentUser = from d in db.MstUsers where d.UserId == User.Identity.GetUserId() select d;

            if (currentUser.FirstOrDefault().CRMUserGroup == "Delivery Manager" || currentUser.FirstOrDefault().CRMUserGroup == "Easyfis Staff" || currentUser.FirstOrDefault().CRMUserGroup == "Admin")
            {
                var sales = from d in db.CrmTrnSalesDeliveries
                            where d.SDDate >= Convert.ToDateTime(startDate)
                            && d.SDDate <= Convert.ToDateTime(endDate)
                            select new Entities.CrmTrnSalesDeliveryEntity
                            {
                                Id = d.Id,
                                SDNumber = d.SDNumber,
                                SDDate = d.SDDate.ToShortDateString(),
                                RenewalDate = d.RenewalDate.ToShortDateString(),
                                CustomerId = d.CustomerId,
                                Customer = d.MstArticle.Article,
                                SIId = d.SIId,
                                ProductId = d.ProductId,
                                ProductDescription = d.CrmMstProduct.ProductDescription,
                                LDId = d.LDId,
                                LDNumber = d.CrmTrnLead.LDNumber,
                                ContactPerson = d.ContactPerson,
                                ContactPosition = d.ContactPosition,
                                ContactEmail = d.ContactEmail,
                                ContactPhoneNumber = d.ContactPhoneNumber,
                                Particulars = d.Particulars,
                                AssignedToUserId = d.AssignedToUserId,
                                AssignedToUser = d.MstUser.FullName,
                                Status = d.Status,
                                LastActivity = GetLastSalesDeliveryActivity(d.Id),
                                LastActivityDate = GetLastSalesDeliveryActivityDate(d.Id),
                                IsLocked = d.IsLocked,
                                CreatedByUserId = d.CreatedByUserId,
                                CreatedByUser = d.MstUser1.FullName,
                                CreatedDateTime = d.CreatedDateTime.ToShortDateString(),
                                UpdatedByUserId = d.UpdatedByUserId,
                                UpdatedByUser = d.MstUser2.FullName,
                                UpdatedDateTime = d.UpdatedDateTime.ToShortDateString()
                            };

                if (status.Equals("ALL"))
                {
                    return sales.OrderByDescending(d => d.Id).ToList();
                }
                else
                {
                    return sales.Where(d => d.Status.Equals(status)).OrderByDescending(d => d.Id).ToList();
                }
            }
            else if (currentUser.FirstOrDefault().CRMUserGroup == "Delivery")
            {
                var sales = from d in db.CrmTrnSalesDeliveries
                            where d.SDDate >= Convert.ToDateTime(startDate)
                            && d.SDDate <= Convert.ToDateTime(endDate)
                            && d.CreatedByUserId == currentUser.FirstOrDefault().Id
                            select new Entities.CrmTrnSalesDeliveryEntity
                            {
                                Id = d.Id,
                                SDNumber = d.SDNumber,
                                SDDate = d.SDDate.ToShortDateString(),
                                RenewalDate = d.RenewalDate.ToShortDateString(),
                                CustomerId = d.CustomerId,
                                Customer = d.MstArticle.Article,
                                SIId = d.SIId,
                                ProductId = d.ProductId,
                                ProductDescription = d.CrmMstProduct.ProductDescription,
                                LDId = d.LDId,
                                LDNumber = d.CrmTrnLead.LDNumber,
                                ContactPerson = d.ContactPerson,
                                ContactPosition = d.ContactPosition,
                                ContactEmail = d.ContactEmail,
                                ContactPhoneNumber = d.ContactPhoneNumber,
                                Particulars = d.Particulars,
                                AssignedToUserId = d.AssignedToUserId,
                                AssignedToUser = d.MstUser.FullName,
                                Status = d.Status,
                                LastActivity = GetLastSalesDeliveryActivity(d.Id),
                                LastActivityDate = GetLastSalesDeliveryActivityDate(d.Id),
                                IsLocked = d.IsLocked,
                                CreatedByUserId = d.CreatedByUserId,
                                CreatedByUser = d.MstUser1.FullName,
                                CreatedDateTime = d.CreatedDateTime.ToShortDateString(),
                                UpdatedByUserId = d.UpdatedByUserId,
                                UpdatedByUser = d.MstUser2.FullName,
                                UpdatedDateTime = d.UpdatedDateTime.ToShortDateString()
                            };

                if (status.Equals("ALL"))
                {
                    return sales.OrderByDescending(d => d.Id).ToList();
                }
                else
                {
                    return sales.Where(d => d.Status.Equals(status)).OrderByDescending(d => d.Id).ToList();
                }
            }
            else
            {
                return new List<Entities.CrmTrnSalesDeliveryEntity>();
            }

        }

        public String GetLastSupportctivity(Int32 sPId)
        {
            var activities = from d in db.CrmTrnActivities.OrderByDescending(d => d.Id)
                             where d.SPId == sPId
                             select d;

            if (activities.Any())
            {
                return activities.FirstOrDefault().Activity;
            }

            return "";
        }

        public String GetLastSupportActivityDate(Int32 sPId)
        {
            var activities = from d in db.CrmTrnActivities.OrderByDescending(d => d.Id)
                             where d.SPId == sPId
                             select d;

            if (activities.Any())
            {
                return activities.FirstOrDefault().ACDate.ToShortDateString();
            }

            return "";
        }

        [HttpGet, Route("support/list/{startDate}/{endDate}/{status}")]
        public List<Entities.CrmTrnSupportEntity> ListSupport(String startDate, String endDate, String status)
        {
            var currentUser = from d in db.MstUsers where d.UserId == User.Identity.GetUserId() select d;

            if (currentUser.FirstOrDefault().CRMUserGroup == "Support Manager" || currentUser.FirstOrDefault().CRMUserGroup == "Easyfis Staff" || currentUser.FirstOrDefault().CRMUserGroup == "Admin")
            {
                var supports = from d in db.CrmTrnSupports
                               where d.SPDate >= Convert.ToDateTime(startDate)
                               && d.SPDate <= Convert.ToDateTime(endDate)
                               select new Entities.CrmTrnSupportEntity
                               {
                                   Id = d.Id,
                                   SPNumber = d.SPNumber,
                                   SPDate = d.SPDate.ToShortDateString(),
                                   CustomerId = d.CustomerId,
                                   Customer = d.MstArticle.Article,
                                   SDId = d.SDId,
                                   SDNumber = d.CrmTrnSalesDelivery.SDNumber,
                                   ContactPerson = d.ContactPerson,
                                   ContactPosition = d.ContactPosition,
                                   ContactEmail = d.ContactEmail,
                                   ContactPhoneNumber = d.ContactPhoneNumber,
                                   PointOfContact = d.PointOfContact,
                                   Issue = d.Issue,
                                   AssignedToUserId = d.AssignedToUserId,
                                   AssignedToUser = d.MstUser.FullName,
                                   Status = d.Status,
                                   LastActivity = GetLastSupportctivity(d.Id),
                                   LastActivityDate = GetLastSupportActivityDate(d.Id),
                                   IsLocked = d.IsLocked,
                                   CreatedByUserId = d.CreatedByUserId,
                                   CreatedByUser = d.MstUser1.FullName,
                                   CreatedDateTime = d.CreatedDateTime.ToShortDateString(),
                                   UpdatedByUserId = d.UpdatedByUserId,
                                   UpdatedByUser = d.MstUser2.FullName,
                                   UpdatedDateTime = d.UpdatedDateTime.ToShortDateString()
                               };

                if (status.Equals("ALL"))
                {
                    return supports.OrderByDescending(d => d.Id).ToList();
                }
                else
                {
                    return supports.Where(d => d.Status.Equals(status)).OrderByDescending(d => d.Id).ToList();
                }
            }
            else if (currentUser.FirstOrDefault().CRMUserGroup == "Support")
            {
                var supports = from d in db.CrmTrnSupports
                               where d.SPDate >= Convert.ToDateTime(startDate)
                               && d.SPDate <= Convert.ToDateTime(endDate)
                               && d.CreatedByUserId == currentUser.FirstOrDefault().Id
                               select new Entities.CrmTrnSupportEntity
                               {
                                   Id = d.Id,
                                   SPNumber = d.SPNumber,
                                   SPDate = d.SPDate.ToShortDateString(),
                                   CustomerId = d.CustomerId,
                                   Customer = d.MstArticle.Article,
                                   SDId = d.SDId,
                                   SDNumber = d.CrmTrnSalesDelivery.SDNumber,
                                   ContactPerson = d.ContactPerson,
                                   ContactPosition = d.ContactPosition,
                                   ContactEmail = d.ContactEmail,
                                   ContactPhoneNumber = d.ContactPhoneNumber,
                                   PointOfContact = d.PointOfContact,
                                   Issue = d.Issue,
                                   AssignedToUserId = d.AssignedToUserId,
                                   AssignedToUser = d.MstUser.FullName,
                                   Status = d.Status,
                                   LastActivity = GetLastSupportctivity(d.Id),
                                   LastActivityDate = GetLastSupportActivityDate(d.Id),
                                   IsLocked = d.IsLocked,
                                   CreatedByUserId = d.CreatedByUserId,
                                   CreatedByUser = d.MstUser1.FullName,
                                   CreatedDateTime = d.CreatedDateTime.ToShortDateString(),
                                   UpdatedByUserId = d.UpdatedByUserId,
                                   UpdatedByUser = d.MstUser2.FullName,
                                   UpdatedDateTime = d.UpdatedDateTime.ToShortDateString()
                               };

                if (status.Equals("ALL"))
                {
                    return supports.OrderByDescending(d => d.Id).ToList();
                }
                else
                {
                    return supports.Where(d => d.Status.Equals(status)).OrderByDescending(d => d.Id).ToList();
                }
            }
            else
            {
                return new List<Entities.CrmTrnSupportEntity>();
            }

        }

        [HttpGet, Route("activity/list/{startDate}/{endDate}/{document}/{status}")]
        public List<Entities.CrmAcitivityReportEntity> ListActivity(String startDate, String endDate, String document, String status)
        {
            var currentUser = from d in db.MstUsers where d.UserId == User.Identity.GetUserId() select d;


            if (document == "LEAD")
            {
                if (currentUser.FirstOrDefault().CRMUserGroup == "Sales Manager" || currentUser.FirstOrDefault().CRMUserGroup == "Easyfis Staff" || currentUser.FirstOrDefault().CRMUserGroup == "Admin")
                {
                    List<Entities.CrmTrnLeadEntity> trnLeads = new List<Entities.CrmTrnLeadEntity>();

                    var leads = from d in db.CrmTrnLeads
                                where d.LDDate.Date >= Convert.ToDateTime(startDate)
                                && d.LDDate.Date <= Convert.ToDateTime(endDate)
                                select new Entities.CrmTrnLeadEntity
                                {
                                    Id = d.Id,
                                    LDNumber = d.LDNumber,
                                    Status = d.Status,
                                };

                    if (status.Equals("ALL"))
                    {
                        trnLeads = leads.OrderByDescending(d => d.Id).ToList();
                    }
                    else
                    {
                        trnLeads = leads.Where(d => d.Status.Equals(status)).OrderByDescending(d => d.Id).ToList();
                    }

                    var activities = from d in db.CrmTrnActivities
                                     where d.ACDate.Date >= Convert.ToDateTime(startDate)
                                     && d.ACDate.Date <= Convert.ToDateTime(endDate)
                                     select new Entities.CrmTrnActivityEntity
                                     {
                                         Id = d.Id,
                                         ACNumber = d.ACNumber,
                                         ACDate = d.ACDate.ToShortDateString(),
                                         UserId = d.UserId,
                                         User = d.MstUser.FullName,
                                         FunctionalUserId = d.FunctionalUserId,
                                         FunctionalUser = d.MstUser1.FullName,
                                         TechnicalUserId = d.TechnicalUserId,
                                         TechnicalUser = d.MstUser2.FullName,
                                         CRMStatus = d.CRMStatus,
                                         Activity = d.Activity,
                                         StartDate = d.StartDateTime.ToShortDateString(),
                                         StartTime = d.StartDateTime.ToShortTimeString(),
                                         EndDate = d.EndDateTime.ToShortDateString(),
                                         EndTime = d.EndDateTime.ToShortTimeString(),
                                         TransportationCost = d.TransportationCost,
                                         OnSiteCost = d.OnSiteCost,
                                         LDId = d.LDId,
                                         SDId = d.SDId,
                                         SPId = d.SPId,
                                         Status = d.Status,
                                         IsLocked = d.IsLocked,
                                         CreatedByUserId = d.CreatedByUserId,
                                         CreatedByUser = d.MstUser3.FullName,
                                         CreatedDateTime = d.CreatedDateTime.ToShortDateString(),
                                         UpdatedByUserId = d.UpdatedByUserId,
                                         UpdatedByUser = d.MstUser4.FullName,
                                         UpdatedDateTime = d.UpdatedDateTime.ToShortDateString()
                                     };

                    var leadActivityReport = from d in trnLeads
                                             join e in activities
                                             on d.Id equals e.LDId
                                             select new Entities.CrmAcitivityReportEntity
                                             {
                                                 Id = d.Id,
                                                 Document = "LD-" + e.ACNumber,
                                                 ACNumber = e.ACNumber,
                                                 ACDate = e.ACNumber,
                                                 User = e.User,
                                                 Functional = e.FunctionalUser,
                                                 Technical = e.TechnicalUser,
                                                 CRMStatus = e.CRMStatus,
                                                 Activity = e.Activity,
                                                 StartDateTime = e.StartDate,
                                                 EndDateTime = e.EndDate,
                                                 TransportationCost = e.TransportationCost,
                                                 OnSiteCost = e.OnSiteCost,
                                                 Status = e.ACNumber,
                                                 IsLocked = e.IsLocked,
                                                 CreatedByUser = e.CreatedByUser,
                                                 CreatedDateTime = e.CreatedDateTime,
                                                 UpdatedByUser = e.UpdatedByUser,
                                                 UpdatedDateTime = e.UpdatedDateTime
                                             };

                    return leadActivityReport.OrderByDescending(d => d.Id).ToList();
                }
                else
                {
                    return new List<Entities.CrmAcitivityReportEntity>();
                }
            }
            else if (document == "SALES DELIVERY")
            {
                if (currentUser.FirstOrDefault().CRMUserGroup == "Delivery Manager" || currentUser.FirstOrDefault().CRMUserGroup == "Easyfis Staff" || currentUser.FirstOrDefault().CRMUserGroup == "Admin")
                {
                    List<Entities.CrmTrnSalesDeliveryEntity> trnSalesDelivery = new List<Entities.CrmTrnSalesDeliveryEntity>();

                    var salesDeliveries = from d in db.CrmTrnSalesDeliveries
                                          where d.SDDate.Date >= Convert.ToDateTime(startDate)
                                          && d.SDDate.Date <= Convert.ToDateTime(endDate)
                                          select new Entities.CrmTrnSalesDeliveryEntity
                                          {
                                              Id = d.Id,
                                              LDNumber = d.SDNumber,
                                              Status = d.Status
                                          };

                    if (status.Equals("ALL"))
                    {
                        trnSalesDelivery = salesDeliveries.OrderByDescending(d => d.Id).ToList();
                    }
                    else
                    {
                        trnSalesDelivery = salesDeliveries.Where(d => d.Status.Equals(status)).OrderByDescending(d => d.Id).ToList();
                    }

                    var activities = from d in db.CrmTrnActivities
                                     where d.ACDate.Date >= Convert.ToDateTime(startDate)
                                     && d.ACDate.Date <= Convert.ToDateTime(endDate)
                                     select new Entities.CrmTrnActivityEntity
                                     {
                                         Id = d.Id,
                                         ACNumber = d.ACNumber,
                                         ACDate = d.ACDate.ToShortDateString(),
                                         UserId = d.UserId,
                                         User = d.MstUser.FullName,
                                         FunctionalUserId = d.FunctionalUserId,
                                         FunctionalUser = d.MstUser1.FullName,
                                         TechnicalUserId = d.TechnicalUserId,
                                         TechnicalUser = d.MstUser2.FullName,
                                         CRMStatus = d.CRMStatus,
                                         Activity = d.Activity,
                                         StartDate = d.StartDateTime.ToShortDateString(),
                                         StartTime = d.StartDateTime.ToShortTimeString(),
                                         EndDate = d.EndDateTime.ToShortDateString(),
                                         EndTime = d.EndDateTime.ToShortTimeString(),
                                         TransportationCost = d.TransportationCost,
                                         OnSiteCost = d.OnSiteCost,
                                         LDId = d.LDId,
                                         SDId = d.SDId,
                                         SPId = d.SPId,
                                         Status = d.Status,
                                         IsLocked = d.IsLocked,
                                         CreatedByUserId = d.CreatedByUserId,
                                         CreatedByUser = d.MstUser3.FullName,
                                         CreatedDateTime = d.CreatedDateTime.ToShortDateString(),
                                         UpdatedByUserId = d.UpdatedByUserId,
                                         UpdatedByUser = d.MstUser4.FullName,
                                         UpdatedDateTime = d.UpdatedDateTime.ToShortDateString()
                                     };

                    var salesDeliveryActivityReport = from d in trnSalesDelivery
                                                      join e in activities
                                                      on d.Id equals e.SDId
                                                      select new Entities.CrmAcitivityReportEntity
                                                      {
                                                          Id = d.Id,
                                                          Document = "SD-" + e.ACNumber,
                                                          ACNumber = e.ACNumber,
                                                          ACDate = e.ACNumber,
                                                          User = e.User,
                                                          Functional = e.FunctionalUser,
                                                          Technical = e.TechnicalUser,
                                                          CRMStatus = e.CRMStatus,
                                                          Activity = e.Activity,
                                                          StartDateTime = e.StartDate,
                                                          EndDateTime = e.EndDate,
                                                          TransportationCost = e.TransportationCost,
                                                          OnSiteCost = e.OnSiteCost,
                                                          Status = e.ACNumber,
                                                          IsLocked = e.IsLocked,
                                                          CreatedByUser = e.CreatedByUser,
                                                          CreatedDateTime = e.CreatedDateTime,
                                                          UpdatedByUser = e.UpdatedByUser,
                                                          UpdatedDateTime = e.UpdatedDateTime
                                                      };

                    return salesDeliveryActivityReport.OrderByDescending(d => d.Id).ToList();
                }
                else
                {
                    return new List<Entities.CrmAcitivityReportEntity>();
                }

            }
            else if (document == "SUPPORT")
            {
                if (currentUser.FirstOrDefault().CRMUserGroup == "Support Manager" || currentUser.FirstOrDefault().CRMUserGroup == "Easyfis Staff" || currentUser.FirstOrDefault().CRMUserGroup == "Admin")
                {
                    List<Entities.CrmTrnSupportEntity> trnSupports = new List<Entities.CrmTrnSupportEntity>();

                    var supports = from d in db.CrmTrnSupports
                                   where d.SPDate.Date >= Convert.ToDateTime(startDate)
                                   && d.SPDate.Date <= Convert.ToDateTime(endDate)
                                   select new Entities.CrmTrnSupportEntity
                                   {
                                       Id = d.Id,
                                       SPNumber = d.SPNumber,
                                       Status = d.Status
                                   };

                    if (status.Equals("ALL"))
                    {
                        trnSupports = supports.OrderByDescending(d => d.Id).ToList();
                    }
                    else
                    {
                        trnSupports = supports.Where(d => d.Status.Equals(status)).OrderByDescending(d => d.Id).ToList();
                    }

                    var activities = from d in db.CrmTrnActivities
                                     where d.ACDate.Date >= Convert.ToDateTime(startDate)
                                     && d.ACDate.Date <= Convert.ToDateTime(endDate)
                                     select new Entities.CrmTrnActivityEntity
                                     {
                                         Id = d.Id,
                                         ACNumber = d.ACNumber,
                                         ACDate = d.ACDate.ToShortDateString(),
                                         UserId = d.UserId,
                                         User = d.MstUser.FullName,
                                         FunctionalUserId = d.FunctionalUserId,
                                         FunctionalUser = d.MstUser1.FullName,
                                         TechnicalUserId = d.TechnicalUserId,
                                         TechnicalUser = d.MstUser2.FullName,
                                         CRMStatus = d.CRMStatus,
                                         Activity = d.Activity,
                                         StartDate = d.StartDateTime.ToShortDateString(),
                                         StartTime = d.StartDateTime.ToShortTimeString(),
                                         EndDate = d.EndDateTime.ToShortDateString(),
                                         EndTime = d.EndDateTime.ToShortTimeString(),
                                         TransportationCost = d.TransportationCost,
                                         OnSiteCost = d.OnSiteCost,
                                         LDId = d.LDId,
                                         SDId = d.SDId,
                                         SPId = d.SPId,
                                         Status = d.Status,
                                         IsLocked = d.IsLocked,
                                         CreatedByUserId = d.CreatedByUserId,
                                         CreatedByUser = d.MstUser3.FullName,
                                         CreatedDateTime = d.CreatedDateTime.ToShortDateString(),
                                         UpdatedByUserId = d.UpdatedByUserId,
                                         UpdatedByUser = d.MstUser4.FullName,
                                         UpdatedDateTime = d.UpdatedDateTime.ToShortDateString()
                                     };

                    var supportActivityReport = from d in trnSupports
                                                join e in activities
                                                on d.Id equals e.SPId
                                                select new Entities.CrmAcitivityReportEntity
                                                {
                                                    Id = d.Id,
                                                    Document = "SP-" + e.ACNumber,
                                                    ACNumber = e.ACNumber,
                                                    ACDate = e.ACNumber,
                                                    User = e.User,
                                                    Functional = e.FunctionalUser,
                                                    Technical = e.TechnicalUser,
                                                    CRMStatus = e.CRMStatus,
                                                    Activity = e.Activity,
                                                    StartDateTime = e.StartDate,
                                                    EndDateTime = e.EndDate,
                                                    TransportationCost = e.TransportationCost,
                                                    OnSiteCost = e.OnSiteCost,
                                                    Status = e.ACNumber,
                                                    IsLocked = e.IsLocked,
                                                    CreatedByUser = e.CreatedByUser,
                                                    CreatedDateTime = e.CreatedDateTime,
                                                    UpdatedByUser = e.UpdatedByUser,
                                                    UpdatedDateTime = e.UpdatedDateTime
                                                };

                    return supportActivityReport.OrderByDescending(d => d.Id).ToList();
                }
                else
                {
                    return new List<Entities.CrmAcitivityReportEntity>();
                }
            }
            else
            {
                if (currentUser.FirstOrDefault().CRMUserGroup == "Easyfis Staff" || currentUser.FirstOrDefault().CRMUserGroup == "Admin")
                {
                    List<Entities.CrmTrnLeadEntity> trnLeads = new List<Entities.CrmTrnLeadEntity>();
                    List<Entities.CrmTrnSupportEntity> trnSupports = new List<Entities.CrmTrnSupportEntity>();
                    List<Entities.CrmTrnSalesDeliveryEntity> trnSalesDelivery = new List<Entities.CrmTrnSalesDeliveryEntity>();
                    List<Entities.CrmAcitivityReportEntity> crmAcitivityReports = new List<Entities.CrmAcitivityReportEntity>();

                    var activities = from d in db.CrmTrnActivities
                                     where d.ACDate.Date >= Convert.ToDateTime(startDate)
                                     && d.ACDate.Date <= Convert.ToDateTime(endDate)
                                     select new Entities.CrmTrnActivityEntity
                                     {
                                         Id = d.Id,
                                         ACNumber = d.ACNumber,
                                         ACDate = d.ACDate.ToShortDateString(),
                                         UserId = d.UserId,
                                         User = d.MstUser.FullName,
                                         FunctionalUserId = d.FunctionalUserId,
                                         FunctionalUser = d.MstUser1.FullName,
                                         TechnicalUserId = d.TechnicalUserId,
                                         TechnicalUser = d.MstUser2.FullName,
                                         CRMStatus = d.CRMStatus,
                                         Activity = d.Activity,
                                         StartDate = d.StartDateTime.ToShortDateString(),
                                         StartTime = d.StartDateTime.ToShortTimeString(),
                                         EndDate = d.EndDateTime.ToShortDateString(),
                                         EndTime = d.EndDateTime.ToShortTimeString(),
                                         TransportationCost = d.TransportationCost,
                                         OnSiteCost = d.OnSiteCost,
                                         LDId = d.LDId,
                                         SDId = d.SDId,
                                         SPId = d.SPId,
                                         Status = d.Status,
                                         IsLocked = d.IsLocked,
                                         CreatedByUserId = d.CreatedByUserId,
                                         CreatedByUser = d.MstUser3.FullName,
                                         CreatedDateTime = d.CreatedDateTime.ToShortDateString(),
                                         UpdatedByUserId = d.UpdatedByUserId,
                                         UpdatedByUser = d.MstUser4.FullName,
                                         UpdatedDateTime = d.UpdatedDateTime.ToShortDateString()
                                     };

                    var leads = from d in db.CrmTrnLeads
                                where d.LDDate.Date >= Convert.ToDateTime(startDate)
                                && d.LDDate.Date <= Convert.ToDateTime(endDate)
                                select new Entities.CrmTrnLeadEntity
                                {
                                    Id = d.Id,
                                    LDNumber = d.LDNumber,
                                };

                    if (status.Equals("ALL"))
                    {
                        trnLeads = leads.OrderByDescending(d => d.Id).ToList();
                    }
                    else
                    {
                        trnLeads = leads.Where(d => d.Status.Equals(status)).OrderByDescending(d => d.Id).ToList();
                    }


                    var supports = from d in db.CrmTrnSupports
                                   where d.SPDate.Date >= Convert.ToDateTime(startDate)
                                   && d.SPDate.Date <= Convert.ToDateTime(endDate)
                                   select new Entities.CrmTrnSupportEntity
                                   {
                                       Id = d.Id,
                                       SPNumber = d.SPNumber,
                                   };

                    if (status.Equals("ALL"))
                    {
                        trnSupports = supports.OrderByDescending(d => d.Id).ToList();
                    }
                    else
                    {
                        trnSupports = supports.Where(d => d.Status.Equals(status)).OrderByDescending(d => d.Id).ToList();
                    }


                    var salesDeliveries = from d in db.CrmTrnSalesDeliveries
                                          where d.SDDate.Date >= Convert.ToDateTime(startDate)
                                          && d.SDDate.Date <= Convert.ToDateTime(endDate)
                                          select new Entities.CrmTrnSalesDeliveryEntity
                                          {
                                              Id = d.Id,
                                              LDNumber = d.SDNumber,
                                          };

                    if (status.Equals("ALL"))
                    {
                        trnSalesDelivery = salesDeliveries.OrderByDescending(d => d.Id).ToList();
                    }
                    else
                    {
                        trnSalesDelivery = salesDeliveries.Where(d => d.Status.Equals(status)).OrderByDescending(d => d.Id).ToList();
                    }

                    var leadActivityReport = from d in trnLeads
                                             join e in activities
                                             on d.Id equals e.LDId
                                             select new Entities.CrmAcitivityReportEntity
                                             {
                                                 Id = d.Id,
                                                 Document = "LD-" + e.ACNumber,
                                                 ACNumber = e.ACNumber,
                                                 ACDate = e.ACDate,
                                                 User = e.User,
                                                 Functional = e.FunctionalUser,
                                                 Technical = e.TechnicalUser,
                                                 CRMStatus = e.CRMStatus,
                                                 Activity = e.Activity,
                                                 StartDateTime = e.StartDate,
                                                 EndDateTime = e.EndDate,
                                                 TransportationCost = e.TransportationCost,
                                                 OnSiteCost = e.OnSiteCost,
                                                 Status = e.ACNumber,
                                                 IsLocked = e.IsLocked,
                                                 CreatedByUser = e.CreatedByUser,
                                                 CreatedDateTime = e.CreatedDateTime,
                                                 UpdatedByUser = e.UpdatedByUser,
                                                 UpdatedDateTime = e.UpdatedDateTime
                                             };

                    crmAcitivityReports.AddRange(leadActivityReport);

                    var salesDeliveryActivityReport = from d in trnSalesDelivery
                                                      join e in activities
                                                      on d.Id equals e.SDId
                                                      select new Entities.CrmAcitivityReportEntity
                                                      {
                                                          Id = d.Id,
                                                          Document = "SD-" + e.ACNumber,
                                                          ACNumber = e.ACNumber,
                                                          ACDate = e.ACDate,
                                                          User = e.User,
                                                          Functional = e.FunctionalUser,
                                                          Technical = e.TechnicalUser,
                                                          CRMStatus = e.CRMStatus,
                                                          Activity = e.Activity,
                                                          StartDateTime = e.StartDate,
                                                          EndDateTime = e.EndDate,
                                                          TransportationCost = e.TransportationCost,
                                                          OnSiteCost = e.OnSiteCost,
                                                          Status = e.ACNumber,
                                                          IsLocked = e.IsLocked,
                                                          CreatedByUser = e.CreatedByUser,
                                                          CreatedDateTime = e.CreatedDateTime,
                                                          UpdatedByUser = e.UpdatedByUser,
                                                          UpdatedDateTime = e.UpdatedDateTime
                                                      };

                    crmAcitivityReports.AddRange(salesDeliveryActivityReport);

                    var supportActivityReport = from d in trnSalesDelivery
                                                join e in activities
                                                on d.Id equals e.SPId
                                                select new Entities.CrmAcitivityReportEntity
                                                {
                                                    Id = d.Id,
                                                    Document = "SP-" + e.ACNumber,
                                                    ACNumber = e.ACNumber,
                                                    ACDate = e.ACNumber,
                                                    User = e.User,
                                                    Functional = e.FunctionalUser,
                                                    Technical = e.TechnicalUser,
                                                    CRMStatus = e.CRMStatus,
                                                    Activity = e.Activity,
                                                    StartDateTime = e.StartDate,
                                                    EndDateTime = e.EndDate,
                                                    TransportationCost = e.TransportationCost,
                                                    OnSiteCost = e.OnSiteCost,
                                                    Status = e.ACNumber,
                                                    IsLocked = e.IsLocked,
                                                    CreatedByUser = e.CreatedByUser,
                                                    CreatedDateTime = e.CreatedDateTime,
                                                    UpdatedByUser = e.UpdatedByUser,
                                                    UpdatedDateTime = e.UpdatedDateTime
                                                };

                    crmAcitivityReports.AddRange(supportActivityReport);

                    return crmAcitivityReports.OrderByDescending(d => d.Id).ToList();
                }
                else
                {
                    return new List<Entities.CrmAcitivityReportEntity>();
                }
            }
        }

        public String GetLastLeadActivityStaff(Int32 LDId)
        {
            var activities = from d in db.CrmTrnActivities.OrderByDescending(d => d.Id)
                             where d.LDId == LDId
                             select new Entities.CrmTrnActivityEntity
                             {
                                 FunctionalUserId = d.FunctionalUserId != null ? d.FunctionalUserId : 0,
                                 FunctionalUser = d.FunctionalUserId != null ? d.MstUser1.FullName : ""
                             };

            if (activities.Any())
            {
                return activities.FirstOrDefault().FunctionalUser;
            }

            return "";
        }

        public String GetLastSalesDeliveryActivityStaff(Int32 SDId)
        {
            var activities = from d in db.CrmTrnActivities.OrderByDescending(d => d.Id)
                             where d.SDId == SDId
                             select new Entities.CrmTrnActivityEntity
                             {
                                 FunctionalUserId = d.FunctionalUserId != null ? d.FunctionalUserId : 0,
                                 FunctionalUser = d.FunctionalUserId != null ? d.MstUser1.FullName : ""
                             };

            if (activities.Any())
            {
                return activities.FirstOrDefault().FunctionalUser;
            }

            return "";
        }

        public String GetLastSupportActivityStaff(Int32 SPId)
        {
            var activities = from d in db.CrmTrnActivities.OrderByDescending(d => d.Id)
                             where d.SPId == SPId
                             select new Entities.CrmTrnActivityEntity
                             {
                                 FunctionalUserId = d.FunctionalUserId != null ? d.FunctionalUserId : 0,
                                 FunctionalUser = d.FunctionalUserId != null ? d.MstUser1.FullName : ""
                             };

            if (activities.Any())
            {
                return activities.FirstOrDefault().FunctionalUser;
            }

            return "";
        }

        [HttpGet, Route("users")]
        public List<Entities.CrmMstUserEntity> ListUser()
        {
            var currentUser = from d in db.MstUsers
                              where d.UserId == User.Identity.GetUserId()
                              select d;

            if (currentUser.FirstOrDefault().CRMUserGroup == "Easyfis Staff" || currentUser.FirstOrDefault().CRMUserGroup == "Admin")
            {
                var users = from d in db.MstUsers
                            where d.IsLocked == true
                            && d.CRMUserGroup != "Customer"
                            select new Entities.CrmMstUserEntity
                            {
                                Id = d.Id,
                                FullName = d.FullName,
                                UserName = d.UserName
                            };

                return users.OrderBy(d => d.FullName).ToList();
            }
            else if (currentUser.FirstOrDefault().CRMUserGroup == "Sales Manager")
            {
                var users = from d in db.MstUsers
                            where d.IsLocked == true
                            && d.CRMUserGroup == "Sales Manager"
                            || d.CRMUserGroup == "Sales"
                            select new Entities.CrmMstUserEntity
                            {
                                Id = d.Id,
                                FullName = d.FullName,
                                UserName = d.UserName
                            };

                return users.OrderBy(d => d.FullName).ToList();
            }
            else if (currentUser.FirstOrDefault().CRMUserGroup == "Delivery Manager")
            {
                var users = from d in db.MstUsers
                            where d.IsLocked == true
                            && d.CRMUserGroup == "Delivery Manager"
                            || d.CRMUserGroup == "Delivery"
                            select new Entities.CrmMstUserEntity
                            {
                                Id = d.Id,
                                FullName = d.FullName,
                                UserName = d.UserName
                            };

                return users.OrderBy(d => d.FullName).ToList();
            }
            else if (currentUser.FirstOrDefault().CRMUserGroup == "Support Manager")
            {
                var users = from d in db.MstUsers
                            where d.IsLocked == true
                            && d.CRMUserGroup == "Support Manager"
                            || d.CRMUserGroup == "Support"
                            select new Entities.CrmMstUserEntity
                            {
                                Id = d.Id,
                                FullName = d.FullName,
                                UserName = d.UserName
                            };

                return users.OrderBy(d => d.FullName).ToList();
            }
            else
            {
                var users = from d in db.MstUsers
                            where d.IsLocked == true
                            && d.Id == currentUser.FirstOrDefault().Id
                            select new Entities.CrmMstUserEntity
                            {
                                Id = d.Id,
                                FullName = d.FullName,
                                UserName = d.UserName
                            };

                return users.OrderBy(d => d.FullName).ToList();
            }
        }

        [HttpGet, Route("sales/staff/{startDate}/{endDate}/{userId}")]
        public List<Entities.ReportSalesStaffEntity> StaffSalesReport(String startDate, String endDate, String userId)
        {
            var currentUser = from d in db.MstUsers where d.UserId == User.Identity.GetUserId() select d;

            if (currentUser.FirstOrDefault().CRMUserGroup == "Sales Manager" || currentUser.FirstOrDefault().CRMUserGroup == "Easyfis Staff" || currentUser.FirstOrDefault().CRMUserGroup == "Admin")
            {
                if (Convert.ToInt32(userId) == 0)
                {
                    var leads = from d in db.CrmTrnLeads
                                where d.LDDate >= Convert.ToDateTime(startDate)
                                && d.LDDate <= Convert.ToDateTime(endDate)
                                && d.IsLocked == true
                                select new Entities.CrmTrnLeadEntity
                                {
                                    Id = d.Id,
                                    AssignedToUserId = d.AssignedToUserId,
                                    AssignedToUser = d.MstUser.FullName,
                                    Status = d.Status,
                                };

                    var groupLeadByStaffByStatus = from d in leads
                                                   group d by new
                                                   {
                                                       d.AssignedToUserId,
                                                       d.AssignedToUser,
                                                   }
                                                   into g
                                                   select new Entities.ReportSalesStaffEntity
                                                   {
                                                       StaffId = g.Key.AssignedToUserId,
                                                       Staff = g.Key.AssignedToUser,
                                                       Introduction = g.Count(lead => lead.Status == "1-INTRODUCTION"),
                                                       Presentation = g.Count(lead => lead.Status == "2-PRESENTATION"),
                                                       Quotation = g.Count(lead => lead.Status == "3-QUOTATION"),
                                                       Invoiced = g.Count(lead => lead.Status == "4-INVOICED"),
                                                       Cancelled = g.Count(lead => lead.Status == "5-CANCELLED"),
                                                       Total = g.Count(),
                                                   };

                    return groupLeadByStaffByStatus.ToList();
                }
                else
                {
                    var leads = from d in db.CrmTrnLeads
                                where d.LDDate >= Convert.ToDateTime(startDate)
                                && d.LDDate <= Convert.ToDateTime(endDate)
                                && d.AssignedToUserId == Convert.ToInt32(userId)
                                && d.IsLocked == true
                                select new Entities.CrmTrnLeadEntity
                                {
                                    Id = d.Id,
                                    AssignedToUserId = d.AssignedToUserId,
                                    AssignedToUser = d.MstUser.FullName,
                                    Status = d.Status,
                                };

                    var groupLeadByStaffByStatus = from d in leads
                                                   group d by new
                                                   {
                                                       d.AssignedToUserId,
                                                       d.AssignedToUser,
                                                   }
                                                   into g
                                                   select new Entities.ReportSalesStaffEntity
                                                   {
                                                       StaffId = g.Key.AssignedToUserId,
                                                       Staff = g.Key.AssignedToUser,
                                                       Introduction = g.Count(lead => lead.Status == "1-INTRODUCTION"),
                                                       Presentation = g.Count(lead => lead.Status == "2-PRESENTATION"),
                                                       Quotation = g.Count(lead => lead.Status == "3-QUOTATION"),
                                                       Invoiced = g.Count(lead => lead.Status == "4-INVOICED"),
                                                       Cancelled = g.Count(lead => lead.Status == "5-CANCELLED"),
                                                       Total = g.Count(),
                                                   };

                    return groupLeadByStaffByStatus.ToList();
                }
            }
            else
            {
                var leads = from d in db.CrmTrnLeads
                            where d.LDDate >= Convert.ToDateTime(startDate)
                            && d.LDDate <= Convert.ToDateTime(endDate)
                            && d.AssignedToUserId == Convert.ToInt32(userId)
                            && d.IsLocked == true
                            select new Entities.CrmTrnLeadEntity
                            {
                                Id = d.Id,
                                AssignedToUserId = d.AssignedToUserId,
                                AssignedToUser = d.MstUser.FullName,
                                Status = d.Status,
                            };

                var groupLeadByStaffByStatus = from d in leads
                                               group d by new
                                               {
                                                   d.AssignedToUserId,
                                                   d.AssignedToUser,
                                               }
                                               into g
                                               select new Entities.ReportSalesStaffEntity
                                               {
                                                   StaffId = g.Key.AssignedToUserId,
                                                   Staff = g.Key.AssignedToUser,
                                                   Introduction = g.Count(lead => lead.Status == "1-INTRODUCTION"),
                                                   Presentation = g.Count(lead => lead.Status == "2-PRESENTATION"),
                                                   Quotation = g.Count(lead => lead.Status == "3-QUOTATION"),
                                                   Invoiced = g.Count(lead => lead.Status == "4-INVOICED"),
                                                   Cancelled = g.Count(lead => lead.Status == "5-CANCELLED"),
                                                   Total = g.Count(),
                                               };

                return groupLeadByStaffByStatus.ToList();
            }

        }

        [HttpGet, Route("sales/staff/quotation/{startDate}/{endDate}/{userId}")]
        public List<Entities.ReportQuotationSalesEntity> StaffSalesQuotationReport(String startDate, String endDate, String userId)
        {
            var currentUser = from d in db.MstUsers where d.UserId == User.Identity.GetUserId() select d;

            if (currentUser.FirstOrDefault().CRMUserGroup == "Sales Manager" || currentUser.FirstOrDefault().CRMUserGroup == "Easyfis Staff" || currentUser.FirstOrDefault().CRMUserGroup == "Admin")
            {
                if (Convert.ToInt32(userId) == 0)
                {
                    var quotationSales = from d in db.CrmTrnLeads
                                         where d.LDDate >= Convert.ToDateTime(startDate)
                                         && d.LDDate <= Convert.ToDateTime(endDate)
                                         && d.Status == "3-QUOTATION"
                                         && d.IsLocked == true
                                         select new Entities.ReportQuotationSalesEntity
                                         {
                                             LDNumber = d.LDNumber,
                                             Customer = d.Name,
                                             LDQuotationDate = d.LDDate.ToShortDateString(),
                                             ProductDescription = d.CrmMstProduct.ProductDescription,
                                             TotalAmount = d.TotalAmount,
                                             ExpectedInvoicedDate = "",
                                             AssignedToUser = d.MstUser.FullName,
                                             LastActivity = GetLastLeadActivity(d.Id),
                                             LastActivityDate = GetLastLeadActivityDate(d.Id),
                                             LastActivityStaff = GetLastLeadActivityStaff(d.Id)
                                         };

                    return quotationSales.OrderByDescending(d => d.LDNumber).ToList();
                }
                else
                {
                    var quotationSales = from d in db.CrmTrnLeads
                                         where d.LDDate >= Convert.ToDateTime(startDate)
                                         && d.LDDate <= Convert.ToDateTime(endDate)
                                         && d.AssignedToUserId == Convert.ToInt32(userId)
                                         && d.Status == "3-QUOTATION"
                                         && d.IsLocked == true
                                         select new Entities.ReportQuotationSalesEntity
                                         {
                                             LDNumber = d.LDNumber,
                                             Customer = d.Name,
                                             LDQuotationDate = d.LDDate.ToShortDateString(),
                                             ProductDescription = d.CrmMstProduct.ProductDescription,
                                             TotalAmount = d.TotalAmount,
                                             ExpectedInvoicedDate = "",
                                             AssignedToUser = d.MstUser.FullName,
                                             LastActivity = GetLastLeadActivity(d.Id),
                                             LastActivityDate = GetLastLeadActivityDate(d.Id),
                                             LastActivityStaff = GetLastLeadActivityStaff(d.Id)
                                         };

                    return quotationSales.OrderByDescending(d => d.LDNumber).ToList();
                }
            }
            else
            {
                var quotationSales = from d in db.CrmTrnLeads
                                     where d.LDDate >= Convert.ToDateTime(startDate)
                                     && d.LDDate <= Convert.ToDateTime(endDate)
                                     && d.AssignedToUserId == Convert.ToInt32(userId)
                                     && d.Status == "3-QUOTATION"
                                     && d.IsLocked == true
                                     select new Entities.ReportQuotationSalesEntity
                                     {
                                         LDNumber = d.LDNumber,
                                         Customer = d.Name,
                                         LDQuotationDate = d.LDDate.ToShortDateString(),
                                         ProductDescription = d.CrmMstProduct.ProductDescription,
                                         TotalAmount = d.TotalAmount,
                                         ExpectedInvoicedDate = "",
                                         AssignedToUser = d.MstUser.FullName,
                                         LastActivity = GetLastLeadActivity(d.Id),
                                         LastActivityDate = GetLastLeadActivityDate(d.Id),
                                         LastActivityStaff = GetLastLeadActivityStaff(d.Id)
                                     };

                return quotationSales.OrderByDescending(d => d.LDNumber).ToList();
            }
        }

        [HttpGet, Route("sales-delivery/staff/{startDate}/{endDate}/{userId}")]
        public List<Entities.ReportSalesDeliveryStaffEntity> StaffSalesDeliveryReport(String startDate, String endDate, String userId)
        {
            var currentUser = from d in db.MstUsers where d.UserId == User.Identity.GetUserId() select d;

            if (currentUser.FirstOrDefault().CRMUserGroup == "Delivery Manager" || currentUser.FirstOrDefault().CRMUserGroup == "Easyfis Staff" || currentUser.FirstOrDefault().CRMUserGroup == "Admin")
            {
                if (Convert.ToInt32(userId) == 0)
                {
                    var salesDelivery = from d in db.CrmTrnSalesDeliveries
                                        where d.SDDate >= Convert.ToDateTime(startDate)
                                        && d.SDDate <= Convert.ToDateTime(endDate)
                                        && d.IsLocked == true
                                        select new Entities.CrmTrnSalesDeliveryEntity
                                        {
                                            Id = d.Id,
                                            AssignedToUserId = d.AssignedToUserId,
                                            AssignedToUser = d.MstUser.FullName,
                                            Status = d.Status,
                                        };

                    var groupSalesDeliveryByStaffByStatus = from d in salesDelivery
                                                            group d by new
                                                            {
                                                                d.AssignedToUserId,
                                                                d.AssignedToUser,
                                                            }
                                                            into g
                                                            select new Entities.ReportSalesDeliveryStaffEntity
                                                            {
                                                                StaffId = g.Key.AssignedToUserId,
                                                                Staff = g.Key.AssignedToUser,
                                                                Open = g.Count(delivery => delivery.Status == "1-OPEN"),
                                                                ForAcceptance = g.Count(delivery => delivery.Status == "2-FOR ACCEPTANCE"),
                                                                Close = g.Count(delivery => delivery.Status == "3-CLOSE"),
                                                                Cancelled = g.Count(delivery => delivery.Status == "4-CANCELLED"),
                                                                Total = g.Count(),
                                                            };

                    return groupSalesDeliveryByStaffByStatus.ToList();
                }
                else
                {
                    var salesDelivery = from d in db.CrmTrnSalesDeliveries
                                        where d.SDDate >= Convert.ToDateTime(startDate)
                                        && d.SDDate <= Convert.ToDateTime(endDate)
                                        && d.AssignedToUserId == Convert.ToInt32(userId)
                                        && d.IsLocked == true
                                        select new Entities.CrmTrnSalesDeliveryEntity
                                        {
                                            Id = d.Id,
                                            AssignedToUserId = d.AssignedToUserId,
                                            AssignedToUser = d.MstUser.FullName,
                                            Status = d.Status,
                                        };

                    var groupSalesDeliveryByStaffByStatus = from d in salesDelivery
                                                            group d by new
                                                            {
                                                                d.AssignedToUserId,
                                                                d.AssignedToUser,
                                                            }
                                                            into g
                                                            select new Entities.ReportSalesDeliveryStaffEntity
                                                            {
                                                                StaffId = g.Key.AssignedToUserId,
                                                                Staff = g.Key.AssignedToUser,
                                                                Open = g.Count(delivery => delivery.Status == "1-OPEN"),
                                                                ForAcceptance = g.Count(delivery => delivery.Status == "2-FOR ACCEPTANCE"),
                                                                Close = g.Count(delivery => delivery.Status == "3-CLOSE"),
                                                                Cancelled = g.Count(delivery => delivery.Status == "4-CANCELLED"),
                                                                Total = g.Count(),
                                                            };

                    return groupSalesDeliveryByStaffByStatus.ToList();
                }
            }
            else
            {
                var salesDelivery = from d in db.CrmTrnSalesDeliveries
                                    where d.SDDate >= Convert.ToDateTime(startDate)
                                    && d.SDDate <= Convert.ToDateTime(endDate)
                                    && d.AssignedToUserId == Convert.ToInt32(userId)
                                    && d.IsLocked == true
                                    select new Entities.CrmTrnSalesDeliveryEntity
                                    {
                                        Id = d.Id,
                                        AssignedToUserId = d.AssignedToUserId,
                                        AssignedToUser = d.MstUser.FullName,
                                        Status = d.Status,
                                    };

                var groupSalesDeliveryByStaffByStatus = from d in salesDelivery
                                                        group d by new
                                                        {
                                                            d.AssignedToUserId,
                                                            d.AssignedToUser,
                                                        }
                                                        into g
                                                        select new Entities.ReportSalesDeliveryStaffEntity
                                                        {
                                                            StaffId = g.Key.AssignedToUserId,
                                                            Staff = g.Key.AssignedToUser,
                                                            Open = g.Count(delivery => delivery.Status == "1-OPEN"),
                                                            ForAcceptance = g.Count(delivery => delivery.Status == "2-FOR ACCEPTANCE"),
                                                            Close = g.Count(delivery => delivery.Status == "3-CLOSE"),
                                                            Cancelled = g.Count(delivery => delivery.Status == "4-CANCELLED"),
                                                            Total = g.Count(),
                                                        };

                return groupSalesDeliveryByStaffByStatus.ToList();
            }

        }

        [HttpGet, Route("sales-delivery/staff/open/{startDate}/{endDate}/{userId}")]
        public List<Entities.ReportOpenSalesDeliveryEntity> StaffSalesDeliveryOpeReport(String startDate, String endDate, String userId)
        {
            var currentUser = from d in db.MstUsers where d.UserId == User.Identity.GetUserId() select d;

            if (currentUser.FirstOrDefault().CRMUserGroup == "Delivery Manager" || currentUser.FirstOrDefault().CRMUserGroup == "Easyfis Staff" || currentUser.FirstOrDefault().CRMUserGroup == "Admin")
            {
                if (Convert.ToInt32(userId) == 0)
                {

                    var openSalesDelivery = from d in db.CrmTrnSalesDeliveries
                                            where d.SDDate >= Convert.ToDateTime(startDate)
                                            && d.SDDate <= Convert.ToDateTime(endDate)
                                            && d.Status == "1-OPEN"
                                            && d.IsLocked == true
                                            select new Entities.ReportOpenSalesDeliveryEntity
                                            {
                                                SDNumber = d.SDNumber,
                                                Customer = d.MstArticle.Article,
                                                SDDeliveryDate = d.SDDate.ToShortDateString(),
                                                ProductDescription = d.CrmMstProduct.ProductDescription,
                                                Amount = "",
                                                ExpectedAcceptanceDate = "",
                                                AssignedToUser = d.MstUser.FullName,
                                                LastActivity = GetLastSalesDeliveryActivity(d.Id),
                                                LastActivityDate = GetLastSalesDeliveryActivityDate(d.Id),
                                                LastActivityStaff = GetLastSalesDeliveryActivityStaff(d.Id)
                                            };

                    return openSalesDelivery.OrderByDescending(d => d.SDNumber).ToList();
                }
                else
                {
                    var openSalesDelivery = from d in db.CrmTrnSalesDeliveries
                                            where d.SDDate >= Convert.ToDateTime(startDate)
                                            && d.SDDate <= Convert.ToDateTime(endDate)
                                            && d.AssignedToUserId == Convert.ToInt32(userId)
                                            && d.Status == "1-OPEN"
                                            && d.IsLocked == true
                                            select new Entities.ReportOpenSalesDeliveryEntity
                                            {
                                                SDNumber = d.SDNumber,
                                                Customer = d.MstArticle.Article,
                                                SDDeliveryDate = d.SDDate.ToShortDateString(),
                                                ProductDescription = d.CrmMstProduct.ProductDescription,
                                                Amount = "",
                                                ExpectedAcceptanceDate = "",
                                                AssignedToUser = d.MstUser.FullName,
                                                LastActivity = GetLastSalesDeliveryActivity(d.Id),
                                                LastActivityDate = GetLastSalesDeliveryActivityDate(d.Id),
                                                LastActivityStaff = GetLastSalesDeliveryActivityStaff(d.Id)
                                            };

                    return openSalesDelivery.OrderByDescending(d => d.SDNumber).ToList();
                }
            }
            else
            {

                var openSalesDelivery = from d in db.CrmTrnSalesDeliveries
                                        where d.SDDate >= Convert.ToDateTime(startDate)
                                        && d.SDDate <= Convert.ToDateTime(endDate)
                                        && d.AssignedToUserId == Convert.ToInt32(userId)
                                        && d.Status == "1-OPEN"
                                        && d.IsLocked == true
                                        select new Entities.ReportOpenSalesDeliveryEntity
                                        {
                                            SDNumber = d.SDNumber,
                                            Customer = d.MstArticle.Article,
                                            SDDeliveryDate = d.SDDate.ToShortDateString(),
                                            ProductDescription = d.CrmMstProduct.ProductDescription,
                                            Amount = "",
                                            ExpectedAcceptanceDate = "",
                                            AssignedToUser = d.MstUser.FullName,
                                            LastActivity = GetLastSalesDeliveryActivity(d.Id),
                                            LastActivityDate = GetLastSalesDeliveryActivityDate(d.Id),
                                            LastActivityStaff = GetLastSalesDeliveryActivityStaff(d.Id)
                                        };

                return openSalesDelivery.OrderByDescending(d => d.SDNumber).ToList();
            }
        }

        [HttpGet, Route("support/staff/{startDate}/{endDate}/{userId}")]
        public List<Entities.ReportSupportStaffEntity> StaffSupportReport(String startDate, String endDate, String userId)
        {
            var currentUser = from d in db.MstUsers where d.UserId == User.Identity.GetUserId() select d;

            if (currentUser.FirstOrDefault().CRMUserGroup == "Support Manager" || currentUser.FirstOrDefault().CRMUserGroup == "Easyfis Staff" || currentUser.FirstOrDefault().CRMUserGroup == "Admin")
            {
                if (Convert.ToInt32(userId) == 0)
                {
                    var support = from d in db.CrmTrnSupports
                                  where d.SPDate >= Convert.ToDateTime(startDate)
                                  && d.SPDate <= Convert.ToDateTime(endDate)
                                  && d.IsLocked == true
                                  select new Entities.CrmTrnSupportEntity
                                  {
                                      Id = d.Id,
                                      AssignedToUserId = d.AssignedToUserId,
                                      AssignedToUser = d.MstUser.FullName,
                                      Status = d.Status,
                                  };

                    var groupSupportByStaffByStatus = from d in support
                                                      group d by new
                                                      {
                                                          d.AssignedToUserId,
                                                          d.AssignedToUser,
                                                      }
                                                            into g
                                                      select new Entities.ReportSupportStaffEntity
                                                      {
                                                          StaffId = g.Key.AssignedToUserId,
                                                          Staff = g.Key.AssignedToUser,
                                                          Open = g.Count(lead => lead.Status == "1-OPEN"),
                                                          ForClosing = g.Count(lead => lead.Status == "2-FOR CLOSING"),
                                                          Close = g.Count(lead => lead.Status == "3-CLOSE"),
                                                          Cancelled = g.Count(lead => lead.Status == "4-CANCELLED"),
                                                          Total = g.Count(),
                                                      };

                    return groupSupportByStaffByStatus.ToList();
                }
                else
                {
                    var support = from d in db.CrmTrnSupports
                                  where d.SPDate >= Convert.ToDateTime(startDate)
                                  && d.SPDate <= Convert.ToDateTime(endDate)
                                  && d.AssignedToUserId == Convert.ToInt32(userId)
                                  && d.IsLocked == true
                                  select new Entities.CrmTrnSupportEntity
                                  {
                                      Id = d.Id,
                                      AssignedToUserId = d.AssignedToUserId,
                                      AssignedToUser = d.MstUser.FullName,
                                      Status = d.Status,
                                  };

                    var groupSupportByStaffByStatus = from d in support
                                                      group d by new
                                                      {
                                                          d.AssignedToUserId,
                                                          d.AssignedToUser,
                                                      }
                                                            into g
                                                      select new Entities.ReportSupportStaffEntity
                                                      {
                                                          StaffId = g.Key.AssignedToUserId,
                                                          Staff = g.Key.AssignedToUser,
                                                          Open = g.Count(lead => lead.Status == "1-OPEN"),
                                                          ForClosing = g.Count(lead => lead.Status == "2-FOR CLOSING"),
                                                          Close = g.Count(lead => lead.Status == "3-CLOSE"),
                                                          Cancelled = g.Count(lead => lead.Status == "4-CANCELLED"),
                                                          Total = g.Count(),
                                                      };

                    return groupSupportByStaffByStatus.ToList();
                }
            }
            else
            {
                var support = from d in db.CrmTrnSupports
                              where d.SPDate >= Convert.ToDateTime(startDate)
                              && d.SPDate <= Convert.ToDateTime(endDate)
                              && d.AssignedToUserId == Convert.ToInt32(userId)
                              && d.IsLocked == true
                              select new Entities.CrmTrnSupportEntity
                              {
                                  Id = d.Id,
                                  AssignedToUserId = d.AssignedToUserId,
                                  AssignedToUser = d.MstUser.FullName,
                                  Status = d.Status,
                              };

                var groupSupportByStaffByStatus = from d in support
                                                  group d by new
                                                  {
                                                      d.AssignedToUserId,
                                                      d.AssignedToUser,
                                                  }
                                                        into g
                                                  select new Entities.ReportSupportStaffEntity
                                                  {
                                                      StaffId = g.Key.AssignedToUserId,
                                                      Staff = g.Key.AssignedToUser,
                                                      Open = g.Count(lead => lead.Status == "1-OPEN"),
                                                      ForClosing = g.Count(lead => lead.Status == "2-FOR CLOSING"),
                                                      Close = g.Count(lead => lead.Status == "3-CLOSE"),
                                                      Cancelled = g.Count(lead => lead.Status == "4-CANCELLED"),
                                                      Total = g.Count(),
                                                  };

                return groupSupportByStaffByStatus.ToList();
            }
        }

        [HttpGet, Route("support/staff/open/{startDate}/{endDate}/{userId}")]
        public List<Entities.ReportOpenSupportEntity> StaffSupportOpeReport(String startDate, String endDate, String userId)
        {
            var currentUser = from d in db.MstUsers where d.UserId == User.Identity.GetUserId() select d;

            if (currentUser.FirstOrDefault().CRMUserGroup == "Support Manager" || currentUser.FirstOrDefault().CRMUserGroup == "Easyfis Staff" || currentUser.FirstOrDefault().CRMUserGroup == "Admin")
            {
                if (Convert.ToInt32(userId) == 0)
                {
                    var openSupport = from d in db.CrmTrnSupports
                                      where d.SPDate >= Convert.ToDateTime(startDate)
                                      && d.SPDate <= Convert.ToDateTime(endDate)
                                      && d.Status == "1-OPEN"
                                      && d.IsLocked == true
                                      select new Entities.ReportOpenSupportEntity
                                      {
                                          SPNumber = d.SPNumber,
                                          Customer = d.MstArticle.Article,
                                          SupportDate = d.SPDate.ToShortDateString(),
                                          ProductDescription = d.CrmTrnSalesDelivery.CrmMstProduct.ProductDescription,
                                          Amount = "",
                                          PointOfContact = d.PointOfContact,
                                          ExpectedCloseDate = "",
                                          AssignedToUser = d.MstUser.FullName,
                                          LastActivity = GetLastSupportctivity(d.Id),
                                          LastActivityDate = GetLastSupportActivityDate(d.Id),
                                          LastActivityStaff = GetLastSupportActivityStaff(d.Id)
                                      };

                    return openSupport.OrderByDescending(d => d.SPNumber).ToList();
                }
                else
                {
                    var openSupport = from d in db.CrmTrnSupports
                                      where d.SPDate >= Convert.ToDateTime(startDate)
                                      && d.SPDate <= Convert.ToDateTime(endDate)
                                      && d.AssignedToUserId == Convert.ToInt32(userId)
                                      && d.Status == "1-OPEN"
                                      && d.IsLocked == true
                                      select new Entities.ReportOpenSupportEntity
                                      {
                                          SPNumber = d.SPNumber,
                                          Customer = d.MstArticle.Article,
                                          SupportDate = d.SPDate.ToShortDateString(),
                                          ProductDescription = d.CrmTrnSalesDelivery.CrmMstProduct.ProductDescription,
                                          Amount = "",
                                          PointOfContact = d.PointOfContact,
                                          ExpectedCloseDate = "",
                                          AssignedToUser = d.MstUser.FullName,
                                          LastActivity = GetLastSupportctivity(d.Id),
                                          LastActivityDate = GetLastSupportActivityDate(d.Id),
                                          LastActivityStaff = GetLastSupportActivityStaff(d.Id)
                                      };

                    return openSupport.OrderByDescending(d => d.SPNumber).ToList();
                }
            }
            else
            {
                var openSupport = from d in db.CrmTrnSupports
                                  where d.SPDate >= Convert.ToDateTime(startDate)
                                  && d.SPDate <= Convert.ToDateTime(endDate)
                                  && d.AssignedToUserId == Convert.ToInt32(userId)
                                  && d.Status == "1-OPEN"
                                  && d.IsLocked == true
                                  select new Entities.ReportOpenSupportEntity
                                  {
                                      SPNumber = d.SPNumber,
                                      Customer = d.MstArticle.Article,
                                      SupportDate = d.SPDate.ToShortDateString(),
                                      ProductDescription = d.CrmTrnSalesDelivery.CrmMstProduct.ProductDescription,
                                      Amount = "",
                                      PointOfContact = d.PointOfContact,
                                      ExpectedCloseDate = "",
                                      AssignedToUser = d.MstUser.FullName,
                                      LastActivity = GetLastSupportctivity(d.Id),
                                      LastActivityDate = GetLastSupportActivityDate(d.Id),
                                      LastActivityStaff = GetLastSupportActivityStaff(d.Id)
                                  };

                return openSupport.OrderByDescending(d => d.SPNumber).ToList();
            }
        }

        [HttpGet, Route("software/development/staff/{startDate}/{endDate}/{userId}")]
        public List<Entities.ReportSoftwareDevelopmentStaffEntity> StaffSoftwareDevelopmentReport(String startDate, String endDate, String userId)
        {
            var currentUser = from d in db.MstUsers where d.UserId == User.Identity.GetUserId() select d;

            if (currentUser.FirstOrDefault().CRMUserGroup == "Easyfis Staff" || currentUser.FirstOrDefault().CRMUserGroup == "Admin")
            {
                if (Convert.ToInt32(userId) == 0)
                {
                    var softwareDevelopment = from d in db.CrmTrnSoftwareDevelopments
                                              where d.SDDate >= Convert.ToDateTime(startDate)
                                              && d.SDDate <= Convert.ToDateTime(endDate)
                                              && d.IsLocked == true
                                              select new Entities.CrmTrnSoftwareDevelopmentEntity
                                              {
                                                  Id = d.Id,
                                                  AssignedToUserId = d.AssignedToUserId,
                                                  AssignedToUserFullname = d.MstUser.FullName,
                                                  Status = d.Status
                                              };

                    var groupSoftwareDevelopmentByStaffByStatus = from d in softwareDevelopment
                                                                  group d by new
                                                                  {
                                                                      d.AssignedToUserId,
                                                                      d.AssignedToUserFullname,
                                                                  }
                                                                    into g
                                                                  select new Entities.ReportSoftwareDevelopmentStaffEntity
                                                                  {
                                                                      StaffId = g.Key.AssignedToUserId,
                                                                      Staff = g.Key.AssignedToUserFullname,
                                                                      Open = g.Count(delivery => delivery.Status == "1-OPEN"),
                                                                      Close = g.Count(delivery => delivery.Status == "2-CLOSE"),
                                                                      Cancelled = g.Count(delivery => delivery.Status == "3-CANCELLED"),
                                                                      Total = g.Count(),
                                                                  };


                    return groupSoftwareDevelopmentByStaffByStatus.ToList();
                }
                else
                {
                    var softwareDevelopment = from d in db.CrmTrnSoftwareDevelopments
                                              where d.SDDate >= Convert.ToDateTime(startDate)
                                              && d.SDDate <= Convert.ToDateTime(endDate)
                                              && d.AssignedToUserId == Convert.ToInt32(userId)
                                              && d.IsLocked == true
                                              select new Entities.CrmTrnSoftwareDevelopmentEntity
                                              {
                                                  Id = d.Id,
                                                  AssignedToUserId = d.AssignedToUserId,
                                                  AssignedToUserFullname = d.MstUser.FullName,
                                                  Status = d.Status
                                              };

                    var groupSoftwareDevelopmentByStaffByStatus = from d in softwareDevelopment
                                                                  group d by new
                                                                  {
                                                                      d.AssignedToUserId,
                                                                      d.AssignedToUserFullname,
                                                                  }
                                                                    into g
                                                                  select new Entities.ReportSoftwareDevelopmentStaffEntity
                                                                  {
                                                                      StaffId = g.Key.AssignedToUserId,
                                                                      Staff = g.Key.AssignedToUserFullname,
                                                                      Open = g.Count(delivery => delivery.Status == "1-OPEN"),
                                                                      Close = g.Count(delivery => delivery.Status == "2-CLOSE"),
                                                                      Cancelled = g.Count(delivery => delivery.Status == "3-CANCELLED"),
                                                                      Total = g.Count(),
                                                                  };


                    return groupSoftwareDevelopmentByStaffByStatus.ToList();
                }
            }
            else
            {
                return new List<Entities.ReportSoftwareDevelopmentStaffEntity>();
            }
        }

        [HttpGet, Route("software/development/staff/open/{startDate}/{endDate}/{userId}")]
        public List<Entities.ReportOpenSoftwareDevelopmentEntity> StaffSoftwareDevelopmentOpenReport(String startDate, String endDate, String userId)
        {
            var currentUser = from d in db.MstUsers where d.UserId == User.Identity.GetUserId() select d;

            if (currentUser.FirstOrDefault().CRMUserGroup == "Easyfis Staff" || currentUser.FirstOrDefault().CRMUserGroup == "Admin")
            {
                if (Convert.ToInt32(userId) == 0)
                {
                    var softwareDevelopment = from d in db.CrmTrnSoftwareDevelopments
                                              where d.SDDate >= Convert.ToDateTime(startDate)
                                              && d.SDDate <= Convert.ToDateTime(endDate)
                                              && d.Status == "1-OPEN"
                                              && d.IsLocked == true
                                              select new Entities.ReportOpenSoftwareDevelopmentEntity
                                              {
                                                  SDNumber = d.SDNumber,
                                                  SDDate = d.SDDate.ToShortDateString(),
                                                  ProductDescription = d.CrmMstProduct.ProductDescription,
                                                  Issue = d.Issue,
                                                  IssueType = d.IssueType,
                                                  AssignedToUserFullname = d.MstUser.FullName,
                                                  TargetDateTime = d.TargetDateTime.ToShortDateString(),
                                                  CloseDateTime = d.CloseDateTime.ToShortDateString(),
                                                  Status = d.Status,
                                              };

                    return softwareDevelopment.OrderBy(d => d.SDNumber).ToList();
                }
                else
                {
                    var softwareDevelopment = from d in db.CrmTrnSoftwareDevelopments
                                              where d.SDDate >= Convert.ToDateTime(startDate)
                                              && d.SDDate <= Convert.ToDateTime(endDate)
                                              && d.AssignedToUserId == Convert.ToInt32(userId)
                                              && d.Status == "1-OPEN"
                                              && d.IsLocked == true
                                              select new Entities.ReportOpenSoftwareDevelopmentEntity
                                              {
                                                  SDNumber = d.SDNumber,
                                                  SDDate = d.SDDate.ToShortDateString(),
                                                  ProductDescription = d.CrmMstProduct.ProductDescription,
                                                  Issue = d.Issue,
                                                  IssueType = d.IssueType,
                                                  AssignedToUserFullname = d.MstUser.FullName,
                                                  TargetDateTime = d.TargetDateTime.ToShortDateString(),
                                                  CloseDateTime = d.CloseDateTime.ToShortDateString(),
                                                  Status = d.Status,
                                              };

                    return softwareDevelopment.OrderBy(d => d.SDNumber).ToList();
                }
            }
            else
            {
                return new List<Entities.ReportOpenSoftwareDevelopmentEntity>();
            }
        }
    }
}
