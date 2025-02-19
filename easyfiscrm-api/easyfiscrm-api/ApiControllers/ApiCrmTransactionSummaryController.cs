using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace easyfiscrm_api.ApiControllers
{
    [Authorize, RoutePrefix("api/crm/dashboard")]
    public class ApiCrmTransactionSummaryController : ApiController
    {
        public Data.easyfiscrmdbDataContext db = new Data.easyfiscrmdbDataContext();

        [HttpGet, Route("users")]
        public List<Entities.CrmMstUserEntity> ListUser()
        {
            var currentUser = from d in db.MstUsers
                              where d.UserId == User.Identity.GetUserId()
                              select d;

            if (currentUser.FirstOrDefault().UserName.Equals("admin"))
            {
                var users = from d in db.MstUsers
                            where d.IsLocked == true
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

        [HttpGet, Route("trnSummary/status/{startDate}/{endDate}/{userId}")]
        public List<Entities.CrmTransactionSummaryEntity> TrnSummaryFilterByStatus(String startDate, String endDate, String userId)
        {
            List<Entities.CrmTransactionSummaryEntity> crmTransactions = new List<Entities.CrmTransactionSummaryEntity>();

            if (userId == "0")
            {
                var leads = from d in db.CrmTrnLeads
                            where d.LDDate >= Convert.ToDateTime(startDate)
                            && d.LDDate <= Convert.ToDateTime(endDate)
                            && d.IsLocked == true
                            select new Entities.CrmTrnLeadEntity
                            {
                                Id = d.Id,
                                Status = d.Status,
                            };

                var salesDeliveries = from d in db.CrmTrnSalesDeliveries
                                      where d.SDDate >= Convert.ToDateTime(startDate)
                                      && d.SDDate <= Convert.ToDateTime(endDate)
                                      && d.IsLocked == true
                                      select new Entities.CrmTrnSalesDeliveryEntity
                                      {
                                          Id = d.Id,
                                          Status = d.Status,
                                      };

                var supports = from d in db.CrmTrnSupports
                               where d.SPDate >= Convert.ToDateTime(startDate)
                               && d.SPDate <= Convert.ToDateTime(endDate)
                               && d.IsLocked == true
                               select new Entities.CrmTrnSupportEntity
                               {
                                   Id = d.Id,
                                   Status = d.Status,
                               };

                var groupLeadByStatus = from d in leads
                                        group d by new
                                        {
                                            d.Status
                                        }
                                        into g
                                        select new Entities.CrmTransactionSummaryEntity
                                        {
                                            Document = "LD",
                                            Status = g.Key.Status,
                                            NoOfTransaction = g.Count()
                                        };
                if (leads.Any())
                {
                    crmTransactions.AddRange(groupLeadByStatus);
                }


                var groupSalesDeliveryByStatus = from d in salesDeliveries
                                                 group d by new
                                                 {
                                                     d.Status
                                                 }
                                                 into g
                                                 select new Entities.CrmTransactionSummaryEntity
                                                 {
                                                     Document = "SD",
                                                     Status = g.Key.Status,
                                                     NoOfTransaction = g.Count()
                                                 };
                if (salesDeliveries.Any())
                {
                    crmTransactions.AddRange(groupSalesDeliveryByStatus);
                }

                var groupSupportsByStatus = from d in supports
                                            group d by new
                                            {
                                                d.Status
                                            }
                                            into g
                                            select new Entities.CrmTransactionSummaryEntity
                                            {
                                                Document = "SP",
                                                Status = g.Key.Status,
                                                NoOfTransaction = g.Count()
                                            };

                if (supports.Any())
                {
                    crmTransactions.AddRange(groupSupportsByStatus);
                }

                return crmTransactions.ToList();
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
                                Status = d.Status,
                            };

                var salesDeliveries = from d in db.CrmTrnSalesDeliveries
                                      where d.SDDate >= Convert.ToDateTime(startDate)
                                      && d.SDDate <= Convert.ToDateTime(endDate)
                                      && d.AssignedToUserId == Convert.ToInt32(userId)
                                      && d.IsLocked == true
                                      select new Entities.CrmTrnSalesDeliveryEntity
                                      {
                                          Id = d.Id,
                                          Status = d.Status,
                                      };

                var supports = from d in db.CrmTrnSupports
                               where d.SPDate >= Convert.ToDateTime(startDate)
                               && d.SPDate <= Convert.ToDateTime(endDate)
                               && d.AssignedToUserId == Convert.ToInt32(userId)
                               && d.IsLocked == true
                               select new Entities.CrmTrnSupportEntity
                               {
                                   Id = d.Id,
                                   Status = d.Status,
                               };

                var groupLeadByStatus = from d in leads
                                        group d by new
                                        {
                                            d.Status
                                        }
                                        into g
                                        select new Entities.CrmTransactionSummaryEntity
                                        {
                                            Document = "LD",
                                            Status = g.Key.Status,
                                            NoOfTransaction = g.Count()
                                        };
                if (leads.Any())
                {
                    crmTransactions.AddRange(groupLeadByStatus);
                }


                var groupSalesDeliveryByStatus = from d in salesDeliveries
                                                 group d by new
                                                 {
                                                     d.Status
                                                 }
                                                 into g
                                                 select new Entities.CrmTransactionSummaryEntity
                                                 {
                                                     Document = "SD",
                                                     Status = g.Key.Status,
                                                     NoOfTransaction = g.Count()
                                                 };

                if (salesDeliveries.Any())
                {
                    crmTransactions.AddRange(groupSalesDeliveryByStatus);
                }


                var groupSupportsByStatus = from d in supports
                                            group d by new
                                            {
                                                d.Status
                                            }
                                            into g
                                            select new Entities.CrmTransactionSummaryEntity
                                            {
                                                Document = "SP",
                                                Status = g.Key.Status,
                                                NoOfTransaction = g.Count()
                                            };

                if (supports.Any())
                {
                    crmTransactions.AddRange(groupSupportsByStatus);
                }

                return crmTransactions.ToList();
            }
        }

        [HttpGet, Route("trnSummary/{startDate}/{endDate}/{userId}")]
        public List<Entities.CrmTransactionSummaryEntity> TrnSummary(String startDate, String endDate, String userId)
        {
            List<Entities.CrmTransactionSummaryEntity> crmTransactions = new List<Entities.CrmTransactionSummaryEntity>();

            if (userId == "0")
            {
                var leads = from d in db.CrmTrnLeads
                            where d.LDDate >= Convert.ToDateTime(startDate)
                            && d.LDDate <= Convert.ToDateTime(endDate)
                            && d.IsLocked == true
                            select new Entities.CrmTransactionSummaryEntity
                            {
                                Id = d.Id,
                                Document = "Leads",
                            };

                var salesDeliveries = from d in db.CrmTrnSalesDeliveries
                                      where d.SDDate >= Convert.ToDateTime(startDate)
                                      && d.SDDate <= Convert.ToDateTime(endDate)
                                      && d.IsLocked == true
                                      select new Entities.CrmTransactionSummaryEntity
                                      {
                                          Id = d.Id,
                                          Document = "Sales Delivery",
                                      };

                var supports = from d in db.CrmTrnSupports
                               where d.SPDate >= Convert.ToDateTime(startDate)
                               && d.SPDate <= Convert.ToDateTime(endDate)
                               && d.IsLocked == true
                               select new Entities.CrmTransactionSummaryEntity
                               {
                                   Id = d.Id,
                                   Document = "Support",
                               };

                if (leads.Any())
                {
                    crmTransactions.AddRange(leads);
                }

                if (salesDeliveries.Any())
                {
                    crmTransactions.AddRange(salesDeliveries);
                }

                if (supports.Any())
                {
                    crmTransactions.AddRange(supports);
                }

                var groupTransactionByDocument = from d in crmTransactions
                                                 group d by new
                                                 {
                                                     d.Document
                                                 }
                                                 into g
                                                 select new Entities.CrmTransactionSummaryEntity
                                                 {
                                                     Document = g.Key.Document,
                                                     NoOfTransaction = g.Count()
                                                 };

                return groupTransactionByDocument.ToList();
            }
            else
            {
                var leads = from d in db.CrmTrnLeads
                            where d.LDDate >= Convert.ToDateTime(startDate)
                            && d.LDDate <= Convert.ToDateTime(endDate)
                            && d.AssignedToUserId == Convert.ToInt32(userId)
                            && d.IsLocked == true
                            select new Entities.CrmTransactionSummaryEntity
                            {
                                Id = d.Id,
                                Document = "Leads",
                            };

                var salesDeliveries = from d in db.CrmTrnSalesDeliveries
                                      where d.SDDate >= Convert.ToDateTime(startDate)
                                      && d.SDDate <= Convert.ToDateTime(endDate)
                                      && d.AssignedToUserId == Convert.ToInt32(userId)
                                      && d.IsLocked == true
                                      select new Entities.CrmTransactionSummaryEntity
                                      {
                                          Id = d.Id,
                                          Document = "Sales Delivery",
                                      };

                var supports = from d in db.CrmTrnSupports
                               where d.SPDate >= Convert.ToDateTime(startDate)
                               && d.SPDate <= Convert.ToDateTime(endDate)
                               && d.AssignedToUserId == Convert.ToInt32(userId)
                               && d.IsLocked == true
                               select new Entities.CrmTransactionSummaryEntity
                               {
                                   Id = d.Id,
                                   Document = "Support",
                               };

                if (leads.Any())
                {
                    crmTransactions.AddRange(leads);
                }

                if (salesDeliveries.Any())
                {
                    crmTransactions.AddRange(salesDeliveries);
                }

                if (supports.Any())
                {
                    crmTransactions.AddRange(supports);
                }

                var groupTransactionByDocument = from d in crmTransactions
                                                 group d by new
                                                 {
                                                     d.Document
                                                 }
                                                 into g
                                                 select new Entities.CrmTransactionSummaryEntity
                                                 {
                                                     Document = g.Key.Document,
                                                     NoOfTransaction = g.Count()
                                                 };

                return groupTransactionByDocument.ToList();
            }
        }
    }
}
