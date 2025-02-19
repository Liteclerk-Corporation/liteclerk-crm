using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace easyfiscrm_api.ApiControllers
{
    [Authorize, RoutePrefix("api/crm/trn/sales")]
    public class ApiCrmSalesDeliveryController : ApiController
    {
        public Data.easyfiscrmdbDataContext db = new Data.easyfiscrmdbDataContext();

        public String FillLeadingZeroes(Int32 number, Int32 length)
        {
            String result = number.ToString();
            Int32 pad = length - result.Length;
            while (pad > 0) { result = '0' + result; pad--; }

            return result;
        }

        public String GetLastSalesActivity(Int32 SDId)
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

        public String GetLastLeadActivityDate(Int32 SDId)
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

        [HttpGet, Route("list/{startDate}/{endDate}/{status}")]
        public List<Entities.CrmTrnSalesDeliveryEntity> ListSalesDelivery(String startDate, String endDate, String status)
        {
            var currentUser = from d in db.MstUsers where d.UserId == User.Identity.GetUserId() select d;

            if (currentUser.FirstOrDefault().CRMUserGroup == "Delivery Manager" || currentUser.FirstOrDefault().CRMUserGroup == "Easyfis Staff" || currentUser.FirstOrDefault().CRMUserGroup == "Admin")
            {
                var salesDeliveries = from d in db.CrmTrnSalesDeliveries
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
                                          LDName = d.CrmTrnLead.Name,
                                          ContactPerson = d.ContactPerson,
                                          ContactPosition = d.ContactPosition,
                                          ContactEmail = d.ContactEmail,
                                          ContactPhoneNumber = d.ContactPhoneNumber,
                                          Particulars = d.Particulars,
                                          AssignedToUserId = d.AssignedToUserId,
                                          AssignedToUser = d.MstUser.FullName,
                                          LastActivity = GetLastSalesActivity(d.Id),
                                          LastActivityDate = GetLastLeadActivityDate(d.Id),
                                          Status = d.Status,
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
                    return salesDeliveries.OrderByDescending(d => d.Id).ToList();
                }
                else
                {
                    return salesDeliveries.Where(d => d.Status.Equals(status)).OrderByDescending(d => d.Id).ToList();
                }
            }
            else if (currentUser.FirstOrDefault().CRMUserGroup == "Delivery")
            {
                var salesDeliveries = from d in db.CrmTrnSalesDeliveries
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
                                          LDName = d.CrmTrnLead.Name,
                                          ContactPerson = d.ContactPerson,
                                          ContactPosition = d.ContactPosition,
                                          ContactEmail = d.ContactEmail,
                                          ContactPhoneNumber = d.ContactPhoneNumber,
                                          Particulars = d.Particulars,
                                          AssignedToUserId = d.AssignedToUserId,
                                          AssignedToUser = d.MstUser.FullName,
                                          LastActivity = GetLastSalesActivity(d.Id),
                                          LastActivityDate = GetLastLeadActivityDate(d.Id),
                                          Status = d.Status,
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
                    return salesDeliveries.OrderByDescending(d => d.Id).ToList();
                }
                else
                {
                    return salesDeliveries.Where(d => d.Status.Equals(status)).OrderByDescending(d => d.Id).ToList();
                }
            }
            else
            {
                return new List<Entities.CrmTrnSalesDeliveryEntity>();
            }
        }

        [HttpGet, Route("list/{startDate}/{endDate}/{status}/{userId}")]
        public List<Entities.CrmTrnSalesDeliveryEntity> ListSalesDeliveryFilterByUser(String startDate, String endDate, String status, String userId)
        {
            var salesDeliveries = from d in db.CrmTrnSalesDeliveries
                                  where d.SDDate >= Convert.ToDateTime(startDate)
                                  && d.SDDate <= Convert.ToDateTime(endDate)
                                  && d.AssignedToUserId == Convert.ToInt32(userId)
                                  && d.IsLocked == true
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
                                      LDName = d.CrmTrnLead.Name,
                                      ContactPerson = d.ContactPerson,
                                      ContactPosition = d.ContactPosition,
                                      ContactEmail = d.ContactEmail,
                                      ContactPhoneNumber = d.ContactPhoneNumber,
                                      Particulars = d.Particulars,
                                      AssignedToUserId = d.AssignedToUserId,
                                      AssignedToUser = d.MstUser.FullName,
                                      LastActivity = GetLastSalesActivity(d.Id),
                                      LastActivityDate = GetLastLeadActivityDate(d.Id),
                                      Status = d.Status,
                                      IsLocked = d.IsLocked,
                                      CreatedByUserId = d.CreatedByUserId,
                                      CreatedByUser = d.MstUser1.FullName,
                                      CreatedDateTime = d.CreatedDateTime.ToShortDateString(),
                                      UpdatedByUserId = d.UpdatedByUserId,
                                      UpdatedByUser = d.MstUser2.FullName,
                                      UpdatedDateTime = d.UpdatedDateTime.ToShortDateString()
                                  };

            return salesDeliveries.Where(d => d.Status.Equals(status)).OrderByDescending(d => d.Id).ToList();
        }

        [HttpGet, Route("list/sales/invoice/{id}")]
        public List<Entities.CrmTrnSalesInvoiceEntity> ListSalesInvoice(string id)
        {
            var salesInvoice = from d in db.TrnSalesInvoices
                               where d.CustomerId == Convert.ToInt32(id)
                               && d.IsLocked == true
                               select new Entities.CrmTrnSalesInvoiceEntity
                               {
                                   Id = d.Id,
                                   SINumber = d.SINumber,
                                   SIDate = d.SIDate.ToShortDateString(),
                                   Amount = d.Amount,
                                   Remarks = d.Remarks
                               };

            return salesInvoice.OrderByDescending(d => d.Id).ToList();
        }

        [HttpGet, Route("list/customer")]
        public List<Entities.MstArticleEntity> ListCustomer()
        {
            var customer = from d in db.MstArticles
                           where d.MstArticleType.ArticleType == "Customer"
                           select new Entities.MstArticleEntity
                           {

                               Id = d.Id,
                               ArticleCode = d.ArticleCode,
                               Article = d.Article,
                               ContactPerson = d.ContactPerson
                           };

            return customer.OrderByDescending(d => d.Article).ToList();
        }

        [HttpGet, Route("list/product")]
        public List<Entities.CrmMstProductEntity> ListProduct()
        {
            var customer = from d in db.CrmMstProducts
                           select new Entities.CrmMstProductEntity
                           {
                               Id = d.Id,
                               ProductDescription = d.ProductDescription
                           };

            return customer.OrderBy(d => d.ProductDescription).ToList();
        }

        [HttpGet, Route("list/lead")]
        public List<Entities.CrmTrnLeadEntity> ListLead()
        {
            var leads = from d in db.CrmTrnLeads
                        where d.IsLocked == true
                        select new Entities.CrmTrnLeadEntity
                        {
                            Id = d.Id,
                            LDNumber = d.LDNumber,
                            LDDate = d.LDDate.ToShortDateString(),
                            Name = d.Name,
                            ContactPerson = d.ContactPerson,
                            ContactPosition = d.ContactPosition,
                            ContactEmail = d.ContactEmail,
                            ContactPhoneNumber = d.ContactPhoneNumber,
                            Remarks = d.Remarks
                        };

            return leads.OrderByDescending(d => d.Id).ToList();
        }

        [HttpGet, Route("list/users")]
        public List<Entities.CrmMstUserEntity> ListSalesUsers()
        {
            var users = from d in db.MstUsers
                        select new Entities.CrmMstUserEntity
                        {
                            Id = d.Id,
                            UserName = d.UserName,
                            FullName = d.FullName
                        };

            return users.OrderBy(d => d.FullName).ToList();
        }

        [HttpGet, Route("list/status")]
        public List<Entities.CrmMstStatusEntity> ListLeadStatus()
        {
            var statuses = from d in db.MstStatus
                           where d.Category.Equals("SD")
                           select new Entities.CrmMstStatusEntity
                           {
                               Id = d.Id,
                               Status = d.Status
                           };

            return statuses.ToList();
        }

        [HttpGet, Route("detail/{id}")]
        public Entities.CrmTrnSalesDeliveryEntity DetailSalesDelivery(String id)
        {
            var salesDelivery = from d in db.CrmTrnSalesDeliveries
                                where d.Id == Convert.ToInt32(id)
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
                                    LDId = d.LDId,
                                    LDName = d.CrmTrnLead.Name,
                                    LDNumber = d.CrmTrnLead.LDNumber,
                                    ContactPerson = d.ContactPerson,
                                    ContactPosition = d.ContactPosition,
                                    ContactEmail = d.ContactEmail,
                                    ContactPhoneNumber = d.ContactPhoneNumber,
                                    Particulars = d.Particulars,
                                    AssignedToUserId = d.AssignedToUserId,
                                    Status = d.Status,
                                    IsLocked = d.IsLocked,
                                    CreatedByUserId = d.CreatedByUserId,
                                    CreatedByUser = d.MstUser1.FullName,
                                    CreatedDateTime = d.CreatedDateTime.ToShortDateString(),
                                    UpdatedByUserId = d.UpdatedByUserId,
                                    UpdatedByUser = d.MstUser2.FullName,
                                    UpdatedDateTime = d.UpdatedDateTime.ToShortDateString()
                                };

            return salesDelivery.FirstOrDefault();
        }

        [HttpPost, Route("add")]
        public HttpResponseMessage AddSalesDelivery()
        {
            try
            {
                var currentUser = from d in db.MstUsers where d.UserId == User.Identity.GetUserId() select d;

                if (currentUser.Any())
                {
                    if (currentUser.FirstOrDefault().CRMUserGroup == "Delivery Manager" || currentUser.FirstOrDefault().CRMUserGroup == "Delivery" || currentUser.FirstOrDefault().CRMUserGroup == "Easyfis Staff" || currentUser.FirstOrDefault().CRMUserGroup == "Admin")
                    {
                        String SDNumber = "0000000001";

                        var lastLead = from d in db.CrmTrnSalesDeliveries.OrderByDescending(d => d.Id) select d;
                        if (lastLead.Any())
                        {
                            Int32 newLDNumber = Convert.ToInt32(lastLead.FirstOrDefault().SDNumber) + 0000000001;
                            SDNumber = FillLeadingZeroes(newLDNumber, 10);
                        }

                        var customer = from d in db.MstArticles
                                       where d.IsLocked == true
                                       && d.MstArticleType.ArticleType == "Customer"
                                       select d;

                        Int32 customerId = 0;
                        if (customer.Any())
                        {
                            customerId = customer.FirstOrDefault().Id;
                        }

                        var salesInvoice = from d in db.TrnSalesInvoices
                                           select d;

                        Int32? sIId = 0;
                        if (salesInvoice.Any())
                        {
                            sIId = salesInvoice.FirstOrDefault().Id;
                        }
                        else
                        {
                            sIId = null;
                        }

                        var lead = from d in db.CrmTrnLeads
                                   where d.IsLocked == true
                                   select d;

                        Int32 lDId = 0;
                        if (lead.Any())
                        {
                            lDId = lead.FirstOrDefault().Id;
                        }

                        var product = from d in db.CrmMstProducts
                                      select d;

                        Int32 productId = 0;
                        if (product.Any())
                        {
                            productId = product.FirstOrDefault().Id;
                        }

                        var status = from d in db.MstStatus
                                     where d.Category.Equals("SD")
                                     select d;

                        String salesDeliveryStatus = "";
                        if (status.Any())
                        {
                            salesDeliveryStatus = status.FirstOrDefault().Status;
                        }

                        Data.CrmTrnSalesDelivery newSalesDelivery = new Data.CrmTrnSalesDelivery
                        {
                            SDNumber = SDNumber,
                            SDDate = DateTime.Today,
                            RenewalDate = DateTime.Today,
                            CustomerId = customerId,
                            SIId = sIId,
                            ProductId = productId,
                            LDId = lDId,
                            ContactPerson = "NA",
                            ContactPosition = "NA",
                            ContactEmail = "NA",
                            ContactPhoneNumber = "NA",
                            Particulars = "NA",
                            AssignedToUserId = currentUser.FirstOrDefault().Id,
                            Status = salesDeliveryStatus,
                            IsLocked = false,
                            CreatedByUserId = currentUser.FirstOrDefault().Id,
                            CreatedDateTime = DateTime.Today,
                            UpdatedByUserId = currentUser.FirstOrDefault().Id,
                            UpdatedDateTime = DateTime.Today
                        };

                        db.CrmTrnSalesDeliveries.InsertOnSubmit(newSalesDelivery);
                        db.SubmitChanges();

                        return Request.CreateResponse(HttpStatusCode.OK, newSalesDelivery.Id);
                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.Unauthorized, "Unauthorized!");
                    }
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "User not found!");
                }
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpPut, Route("save/{id}")]
        public HttpResponseMessage SaveSalesDelivery(String id, Entities.CrmTrnSalesDeliveryEntity objSalesDelivery)
        {
            try
            {
                var currentUser = from d in db.MstUsers where d.UserId == User.Identity.GetUserId() select d;

                if (currentUser.Any())
                {
                    if (currentUser.FirstOrDefault().CRMUserGroup == "Delivery Manager" || currentUser.FirstOrDefault().CRMUserGroup == "Delivery" || currentUser.FirstOrDefault().CRMUserGroup == "Easyfis Staff" || currentUser.FirstOrDefault().CRMUserGroup == "Admin")
                    {
                        var currentSalesDelivery = from d in db.CrmTrnSalesDeliveries
                                                   where d.Id == Convert.ToInt32(id)
                                                   select d;

                        if (currentSalesDelivery.Any())
                        {
                            if (!currentSalesDelivery.FirstOrDefault().IsLocked)
                            {
                                var updateCurrentSalesDelivery = currentSalesDelivery.FirstOrDefault();
                                updateCurrentSalesDelivery.SDDate = Convert.ToDateTime(objSalesDelivery.SDDate);
                                updateCurrentSalesDelivery.RenewalDate = Convert.ToDateTime(objSalesDelivery.RenewalDate);
                                updateCurrentSalesDelivery.CustomerId = objSalesDelivery.CustomerId;
                                updateCurrentSalesDelivery.SIId = objSalesDelivery.SIId;
                                updateCurrentSalesDelivery.ProductId = objSalesDelivery.ProductId;
                                updateCurrentSalesDelivery.LDId = objSalesDelivery.LDId;
                                updateCurrentSalesDelivery.ContactPerson = objSalesDelivery.ContactPerson;
                                updateCurrentSalesDelivery.ContactPosition = objSalesDelivery.ContactPosition;
                                updateCurrentSalesDelivery.ContactPhoneNumber = objSalesDelivery.ContactPhoneNumber;
                                updateCurrentSalesDelivery.ContactEmail = objSalesDelivery.ContactEmail;
                                updateCurrentSalesDelivery.Particulars = objSalesDelivery.Particulars;
                                updateCurrentSalesDelivery.AssignedToUserId = objSalesDelivery.AssignedToUserId;
                                updateCurrentSalesDelivery.Status = objSalesDelivery.Status;
                                updateCurrentSalesDelivery.UpdatedByUserId = currentUser.FirstOrDefault().Id;
                                updateCurrentSalesDelivery.UpdatedDateTime = DateTime.Today;
                                db.SubmitChanges();

                                return Request.CreateResponse(HttpStatusCode.OK);
                            }
                            else
                            {
                                return Request.CreateResponse(HttpStatusCode.BadRequest, "Current sales delivery is already locked!");
                            }
                        }
                        else
                        {
                            return Request.CreateResponse(HttpStatusCode.NotFound, "Sales Delivery data not found!");
                        }
                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.Unauthorized, "Unauthorized!");
                    }
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "User not found!");
                }
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpPut, Route("lock/{id}")]
        public HttpResponseMessage LockSalesDelivry(String id, Entities.CrmTrnSalesDeliveryEntity objSalesDelivery)
        {
            try
            {
                var currentUser = from d in db.MstUsers where d.UserId == User.Identity.GetUserId() select d;

                if (currentUser.Any())
                {
                    if (currentUser.FirstOrDefault().CRMUserGroup == "Delivery Manager" || currentUser.FirstOrDefault().CRMUserGroup == "Delivery" || currentUser.FirstOrDefault().CRMUserGroup == "Easyfis Staff" || currentUser.FirstOrDefault().CRMUserGroup == "Admin")
                    {
                        var currentSalesDelivery = from d in db.CrmTrnSalesDeliveries
                                                   where d.Id == Convert.ToInt32(id)
                                                   select d;

                        if (currentSalesDelivery.Any())
                        {
                            if (!currentSalesDelivery.FirstOrDefault().IsLocked)
                            {
                                var lockCurrentSalesDelivery = currentSalesDelivery.FirstOrDefault();
                                lockCurrentSalesDelivery.SDDate = Convert.ToDateTime(objSalesDelivery.SDDate);
                                lockCurrentSalesDelivery.RenewalDate = Convert.ToDateTime(objSalesDelivery.RenewalDate);
                                lockCurrentSalesDelivery.CustomerId = objSalesDelivery.CustomerId;
                                lockCurrentSalesDelivery.SIId = objSalesDelivery.SIId;
                                lockCurrentSalesDelivery.ProductId = objSalesDelivery.ProductId;
                                lockCurrentSalesDelivery.LDId = objSalesDelivery.LDId;
                                lockCurrentSalesDelivery.ContactPerson = objSalesDelivery.ContactPerson;
                                lockCurrentSalesDelivery.Particulars = objSalesDelivery.Particulars;
                                lockCurrentSalesDelivery.AssignedToUserId = objSalesDelivery.AssignedToUserId;
                                lockCurrentSalesDelivery.Status = objSalesDelivery.Status;
                                lockCurrentSalesDelivery.IsLocked = true;
                                lockCurrentSalesDelivery.UpdatedByUserId = currentUser.FirstOrDefault().Id;
                                lockCurrentSalesDelivery.UpdatedDateTime = DateTime.Today;
                                db.SubmitChanges();

                                return Request.CreateResponse(HttpStatusCode.OK);
                            }
                            else
                            {
                                return Request.CreateResponse(HttpStatusCode.BadRequest, "Current sales delivery is already locked!");
                            }
                        }
                        else
                        {
                            return Request.CreateResponse(HttpStatusCode.NotFound, "Sales Delivery data not found!");
                        }
                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.Unauthorized, "Unauthorized!");
                    }
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "User not found!");
                }
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpPut, Route("unlock/{id}")]
        public HttpResponseMessage UnlockSalesDelivry(String id, Entities.CrmTrnSalesDeliveryEntity objSalesDelivery)
        {
            try
            {
                var currentUser = from d in db.MstUsers where d.UserId == User.Identity.GetUserId() select d;

                if (currentUser.Any())
                {
                    if (currentUser.FirstOrDefault().CRMUserGroup == "Delivery Manager" || currentUser.FirstOrDefault().CRMUserGroup == "Delivery" || currentUser.FirstOrDefault().CRMUserGroup == "Easyfis Staff" || currentUser.FirstOrDefault().CRMUserGroup == "Admin")
                    {
                        var currentSalesDelivery = from d in db.CrmTrnSalesDeliveries
                                                   where d.Id == Convert.ToInt32(id)
                                                   select d;

                        if (currentSalesDelivery.Any())
                        {
                            if (currentSalesDelivery.FirstOrDefault().IsLocked)
                            {
                                var unlockCurrentSalesDelivery = currentSalesDelivery.FirstOrDefault();
                                unlockCurrentSalesDelivery.IsLocked = false;
                                unlockCurrentSalesDelivery.UpdatedByUserId = currentUser.FirstOrDefault().Id;
                                unlockCurrentSalesDelivery.UpdatedDateTime = DateTime.Today;
                                db.SubmitChanges();

                                return Request.CreateResponse(HttpStatusCode.OK);
                            }
                            else
                            {
                                return Request.CreateResponse(HttpStatusCode.BadRequest, "Current sales delivery is already unlocked!");
                            }
                        }
                        else
                        {
                            return Request.CreateResponse(HttpStatusCode.NotFound, "Sales Delivery data not found!");
                        }
                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.Unauthorized, "Unauthorized!");
                    }
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "User not found!");
                }

            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpDelete, Route("delete/{id}")]
        public HttpResponseMessage DeleteSalesDelivery(String id)
        {
            try
            {
                var currentUser = from d in db.MstUsers where d.UserId == User.Identity.GetUserId() select d;

                if (currentUser.Any())
                {
                    if (currentUser.FirstOrDefault().CRMUserGroup == "Delivery Manager" || currentUser.FirstOrDefault().CRMUserGroup == "Delivery" || currentUser.FirstOrDefault().CRMUserGroup == "Easyfis Staff" || currentUser.FirstOrDefault().CRMUserGroup == "Admin")
                    {
                        var currentSalesDelivery = from d in db.CrmTrnSalesDeliveries
                                                   where d.Id == Convert.ToInt32(id)
                                                   select d;

                        if (currentSalesDelivery.Any())
                        {
                            if (!currentSalesDelivery.FirstOrDefault().IsLocked)
                            {
                                db.CrmTrnSalesDeliveries.DeleteOnSubmit(currentSalesDelivery.FirstOrDefault());
                                db.SubmitChanges();

                                return Request.CreateResponse(HttpStatusCode.OK);
                            }
                            else
                            {
                                return Request.CreateResponse(HttpStatusCode.BadRequest, "Delete is not allowed when the current sales delivery is locked!");
                            }
                        }
                        else
                        {
                            return Request.CreateResponse(HttpStatusCode.NotFound, "Sales delivery data not found!");
                        }
                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.Unauthorized, "Unauthorized!");
                    }
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "User not found!");
                }
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

    }
}
