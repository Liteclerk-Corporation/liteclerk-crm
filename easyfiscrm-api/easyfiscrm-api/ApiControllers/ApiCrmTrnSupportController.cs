using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace easyfiscrm_api.ApiControllers
{
    [Authorize, RoutePrefix("api/crm/trn/support")]
    public class ApiCrmTrnSupportController : ApiController
    {
        public Data.easyfiscrmdbDataContext db = new Data.easyfiscrmdbDataContext();

        public String FillLeadingZeroes(Int32 number, Int32 length)
        {
            String result = number.ToString();
            Int32 pad = length - result.Length;
            while (pad > 0) { result = '0' + result; pad--; }

            return result;
        }

        public String GetLastSupportctivity(Int32 SPId)
        {
            var activities = from d in db.CrmTrnActivities.OrderByDescending(d => d.Id)
                             where d.SPId == SPId
                             select d;

            if (activities.Any())
            {
                return activities.FirstOrDefault().Activity;
            }

            return "";
        }

        public String GetLastSupportActivityDate(Int32 SPId)
        {
            var activities = from d in db.CrmTrnActivities.OrderByDescending(d => d.Id)
                             where d.SPId == SPId
                             select d;

            if (activities.Any())
            {
                return activities.FirstOrDefault().ACDate.ToShortDateString();
            }

            return "";
        }

        [HttpGet, Route("list/pointOfContactEntity")]
        public List<Entities.PointOfContactEntity> ListDocument()
        {
            List<Entities.PointOfContactEntity> pointOfContactEntity = new List<Entities.PointOfContactEntity>();
            pointOfContactEntity.Add(new Entities.PointOfContactEntity { Id = 1, PointOfContact = "PHONE" });
            pointOfContactEntity.Add(new Entities.PointOfContactEntity { Id = 2, PointOfContact = "EMAIL" });
            pointOfContactEntity.Add(new Entities.PointOfContactEntity { Id = 3, PointOfContact = "PORTAL" });
            return pointOfContactEntity.ToList();
        }

        [HttpGet, Route("list/{startDate}/{endDate}/{status}")]
        public List<Entities.CrmTrnSupportEntity> ListSupport(String startDate, String endDate, String status)
        {
            var currentUser = from d in db.MstUsers where d.UserId == User.Identity.GetUserId() select d;

            if (currentUser.FirstOrDefault().CRMUserGroup == "Support Manager" || currentUser.FirstOrDefault().CRMUserGroup == "Support" || currentUser.FirstOrDefault().CRMUserGroup == "Easyfis Staff" || currentUser.FirstOrDefault().CRMUserGroup == "Admin")
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
                                   Product = d.CrmTrnSalesDelivery.CrmMstProduct.ProductDescription,
                                   SDNumber = d.CrmTrnSalesDelivery.SDNumber + " - " + d.CrmTrnSalesDelivery.SDDate + " - " + d.CrmTrnSalesDelivery.CrmMstProduct.ProductDescription + " - " + d.CrmTrnSalesDelivery.MstUser.FullName,
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
            else if (currentUser.FirstOrDefault().CRMUserGroup == "Customer")
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
                                   Product = d.CrmTrnSalesDelivery.CrmMstProduct.ProductDescription,
                                   SDNumber = d.CrmTrnSalesDelivery.SDNumber + " - " + d.CrmTrnSalesDelivery.SDDate + " - " + d.CrmTrnSalesDelivery.CrmMstProduct.ProductDescription + " - " + d.CrmTrnSalesDelivery.MstUser.FullName,
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

        [HttpGet, Route("list/{startDate}/{endDate}/{status}/{userId}")]
        public List<Entities.CrmTrnSupportEntity> ListSupportFilteredByUser(String startDate, String endDate, String status, String userId)
        {
            var supports = from d in db.CrmTrnSupports
                           where d.SPDate >= Convert.ToDateTime(startDate)
                           && d.SPDate <= Convert.ToDateTime(endDate)
                           && d.AssignedToUserId == Convert.ToInt32(userId)
                           && d.IsLocked == true
                           select new Entities.CrmTrnSupportEntity
                           {
                               Id = d.Id,
                               SPNumber = d.SPNumber,
                               SPDate = d.SPDate.ToShortDateString(),
                               CustomerId = d.CustomerId,
                               Customer = d.MstArticle.Article,
                               SDId = d.SDId,
                               Product = d.CrmTrnSalesDelivery.CrmMstProduct.ProductDescription,
                               SDNumber = d.CrmTrnSalesDelivery.SDNumber + " - " + d.CrmTrnSalesDelivery.SDDate + " - " + d.CrmTrnSalesDelivery.CrmMstProduct.ProductDescription + " - " + d.CrmTrnSalesDelivery.MstUser.FullName,
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

            return supports.Where(d => d.Status.Equals(status)).OrderByDescending(d => d.Id).ToList();
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

            return customer.OrderByDescending(d => d.Id).ToList();
        }

        [HttpGet, Route("list/sales/delivery/{customerId}")]
        public List<Entities.CrmTrnSalesDeliveryEntity> ListSalesDelivery(String customerId)
        {
            var salesDeliveries = from d in db.CrmTrnSalesDeliveries
                                  where d.CustomerId == Convert.ToInt32(customerId)
                                  select new Entities.CrmTrnSalesDeliveryEntity
                                  {
                                      Id = d.Id,
                                      SDNumber = d.SDNumber,
                                      SDDate = d.SDDate.ToShortDateString(),
                                      ProductDescription = d.CrmMstProduct.ProductDescription,
                                      AssignedToUser = d.MstUser.FullName,
                                      ContactPerson = d.ContactPerson,
                                      ContactPosition = d.ContactPosition,
                                      ContactEmail = d.ContactEmail,
                                      ContactPhoneNumber = d.ContactPhoneNumber
                                  };

            return salesDeliveries.OrderByDescending(d => d.Id).ToList();
        }

        [HttpGet, Route("list/users")]
        public List<Entities.CrmMstUserEntity> ListLeadUsers()
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
                           where d.Category.Equals("SP")
                           select new Entities.CrmMstStatusEntity
                           {
                               Id = d.Id,
                               Status = d.Status
                           };

            return statuses.ToList();
        }

        [HttpGet, Route("detail/{id}")]
        public Entities.CrmTrnSupportEntity DetailSupport(String id)
        {

            var support = from d in db.CrmTrnSupports
                          where d.Id == Convert.ToInt32(id)
                          select new Entities.CrmTrnSupportEntity

                          {
                              Id = d.Id,
                              SPNumber = d.SPNumber,
                              SPDate = d.SPDate.ToShortDateString(),
                              CustomerId = d.CustomerId,
                              Customer = d.MstArticle.Article,
                              SDId = d.SDId,
                              ContactPerson = d.ContactPerson,
                              ContactPosition = d.ContactPosition,
                              ContactEmail = d.ContactEmail,
                              ContactPhoneNumber = d.ContactPhoneNumber,
                              PointOfContact = d.PointOfContact,
                              Issue = d.Issue,
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

            return support.FirstOrDefault();
        }

        [HttpPost, Route("add")]
        public HttpResponseMessage AddSupport()
        {
            try
            {
                var currentUser = from d in db.MstUsers where d.UserId == User.Identity.GetUserId() select d;

                if (currentUser.Any())
                {
                    if (currentUser.FirstOrDefault().CRMUserGroup == "Support Manager" || currentUser.FirstOrDefault().CRMUserGroup == "Support" || currentUser.FirstOrDefault().CRMUserGroup == "Customer" || currentUser.FirstOrDefault().CRMUserGroup == "Easyfis Staff" || currentUser.FirstOrDefault().CRMUserGroup == "Admin")
                    {
                        String SPNumber = "0000000001";

                        var lastSupport = from d in db.CrmTrnSupports.OrderByDescending(d => d.Id) select d;
                        if (lastSupport.Any())
                        {
                            Int32 newSPNumber = Convert.ToInt32(lastSupport.FirstOrDefault().SPNumber) + 0000000001;
                            SPNumber = FillLeadingZeroes(newSPNumber, 10);
                        }

                        var customer = from d in db.MstArticles
                                       select d;

                        Int32 customerId = 0;
                        if (customer.Any())
                        {
                            customerId = customer.FirstOrDefault().Id;
                        }

                        var salesDelivery = from d in db.CrmTrnSalesDeliveries
                                            select d;

                        Int32 sDId = 0;
                        if (salesDelivery.Any())
                        {
                            sDId = salesDelivery.FirstOrDefault().Id;
                        }

                        var product = from d in db.CrmMstProducts
                                      select d;

                        Int32 productId = 0;
                        if (product.Any())
                        {
                            productId = product.FirstOrDefault().Id;
                        }

                        var status = from d in db.MstStatus
                                     where d.Category.Equals("SP")
                                     select d;

                        String supportStatus = "";
                        if (status.Any())
                        {
                            supportStatus = status.FirstOrDefault().Status;
                        }

                        Data.CrmTrnSupport newSupport = new Data.CrmTrnSupport
                        {
                            SPNumber = SPNumber,
                            SPDate = DateTime.Today,
                            CustomerId = customerId,
                            SDId = sDId,
                            ContactPerson = "NA",
                            ContactPosition = "NA",
                            ContactEmail = "NA",
                            ContactPhoneNumber = "NA",
                            PointOfContact = "PHONE",
                            Issue = "NA",
                            AssignedToUserId = currentUser.FirstOrDefault().Id,
                            Status = supportStatus,
                            IsLocked = false,
                            CreatedByUserId = currentUser.FirstOrDefault().Id,
                            CreatedDateTime = DateTime.Today,
                            UpdatedByUserId = currentUser.FirstOrDefault().Id,
                            UpdatedDateTime = DateTime.Today,
                        };

                        db.CrmTrnSupports.InsertOnSubmit(newSupport);
                        db.SubmitChanges();

                        return Request.CreateResponse(HttpStatusCode.OK, newSupport.Id);
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
        public HttpResponseMessage SaveSupport(String id, Entities.CrmTrnSupportEntity objSupport)
        {
            try
            {
                var currentUser = from d in db.MstUsers where d.UserId == User.Identity.GetUserId() select d;

                if (currentUser.Any())
                {
                    var userRights = from d in db.MstUserForms
                                     where d.UserId == currentUser.FirstOrDefault().Id
                                     && d.SysForm.FormName == "CRMSupport"
                                     select d;

                    if (currentUser.FirstOrDefault().CRMUserGroup == "Support Manager" || currentUser.FirstOrDefault().CRMUserGroup == "Support" || currentUser.FirstOrDefault().CRMUserGroup == "Customer" || currentUser.FirstOrDefault().CRMUserGroup == "Easyfis Staff" || currentUser.FirstOrDefault().CRMUserGroup == "Admin")
                    {
                        var currentSupport = from d in db.CrmTrnSupports
                                             where d.Id == Convert.ToInt32(id)
                                             select d;

                        if (currentSupport.Any())
                        {
                            if (!currentSupport.FirstOrDefault().IsLocked)
                            {
                                var updateSupport = currentSupport.FirstOrDefault();
                                updateSupport.SPNumber = objSupport.SPNumber;
                                updateSupport.SPDate = Convert.ToDateTime(objSupport.SPDate).Date;
                                updateSupport.CustomerId = objSupport.CustomerId;
                                updateSupport.SDId = objSupport.SDId;
                                updateSupport.ContactPerson = objSupport.ContactPerson;
                                updateSupport.ContactPosition = objSupport.ContactPosition;
                                updateSupport.ContactPerson = objSupport.ContactPerson;
                                updateSupport.ContactEmail = objSupport.ContactEmail;
                                updateSupport.ContactPhoneNumber = objSupport.ContactPhoneNumber;
                                updateSupport.PointOfContact = objSupport.PointOfContact;
                                updateSupport.Issue = objSupport.Issue;
                                updateSupport.AssignedToUserId = objSupport.AssignedToUserId;
                                updateSupport.Status = objSupport.Status;
                                updateSupport.UpdatedByUserId = currentUser.FirstOrDefault().Id;
                                updateSupport.UpdatedDateTime = DateTime.Today;
                                db.SubmitChanges();

                                return Request.CreateResponse(HttpStatusCode.OK);
                            }
                            else
                            {
                                return Request.CreateResponse(HttpStatusCode.BadRequest, "Current support is locked!");
                            }
                        }
                        else
                        {
                            return Request.CreateResponse(HttpStatusCode.NotFound, "Support data not found!");
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
        public HttpResponseMessage LockSupport(String id, Entities.CrmTrnSupportEntity objSupport)
        {
            try
            {
                var currentUser = from d in db.MstUsers where d.UserId == User.Identity.GetUserId() select d;

                if (currentUser.Any())
                {
                    if (currentUser.FirstOrDefault().CRMUserGroup == "Support Manager" || currentUser.FirstOrDefault().CRMUserGroup == "Support" || currentUser.FirstOrDefault().CRMUserGroup == "Customer" || currentUser.FirstOrDefault().CRMUserGroup == "Easyfis Staff" || currentUser.FirstOrDefault().CRMUserGroup == "Admin")
                    {
                        var currentSupport = from d in db.CrmTrnSupports
                                             where d.Id == Convert.ToInt32(id)
                                             select d;

                        if (currentSupport.Any())
                        {
                            if (!currentSupport.FirstOrDefault().IsLocked)
                            {
                                var lockSupport = currentSupport.FirstOrDefault();
                                lockSupport.SPNumber = objSupport.SPNumber;
                                lockSupport.SPDate = Convert.ToDateTime(objSupport.SPDate).Date;
                                lockSupport.CustomerId = objSupport.CustomerId;
                                lockSupport.SDId = objSupport.SDId;
                                lockSupport.ContactPerson = objSupport.ContactPerson;
                                lockSupport.ContactPosition = objSupport.ContactPosition;
                                lockSupport.ContactPerson = objSupport.ContactPerson;
                                lockSupport.ContactEmail = objSupport.ContactEmail;
                                lockSupport.ContactPhoneNumber = objSupport.ContactPhoneNumber;
                                lockSupport.PointOfContact = objSupport.PointOfContact;
                                lockSupport.Issue = objSupport.Issue;
                                lockSupport.AssignedToUserId = objSupport.AssignedToUserId;
                                lockSupport.Status = objSupport.Status;
                                lockSupport.IsLocked = true;
                                lockSupport.UpdatedByUserId = currentUser.FirstOrDefault().Id;
                                lockSupport.UpdatedDateTime = DateTime.Today;
                                db.SubmitChanges();

                                return Request.CreateResponse(HttpStatusCode.OK);
                            }
                            else
                            {
                                return Request.CreateResponse(HttpStatusCode.BadRequest, "Current support is locked!");
                            }
                        }
                        else
                        {
                            return Request.CreateResponse(HttpStatusCode.NotFound, "Support data not found!");
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
        public HttpResponseMessage UnlockSupport(String id, Entities.CrmTrnSupportEntity objSupport)
        {
            try
            {
                var currentUser = from d in db.MstUsers where d.UserId == User.Identity.GetUserId() select d;
                if (currentUser.Any())
                {
                    var userRights = from d in db.MstUserForms
                                     where d.UserId == currentUser.FirstOrDefault().Id
                                     && d.SysForm.FormName == "CRMSupport"
                                     select d;

                    if (currentUser.FirstOrDefault().CRMUserGroup == "Support Manager" || currentUser.FirstOrDefault().CRMUserGroup == "Support" || currentUser.FirstOrDefault().CRMUserGroup == "Customer" || currentUser.FirstOrDefault().CRMUserGroup == "Easyfis Staff" || currentUser.FirstOrDefault().CRMUserGroup == "Admin")
                    {
                        var currentSupport = from d in db.CrmTrnSupports
                                             where d.Id == Convert.ToInt32(id)
                                             select d;

                        if (currentSupport.Any())
                        {
                            if (currentSupport.FirstOrDefault().IsLocked)
                            {
                                var unlockSupport = currentSupport.FirstOrDefault();
                                unlockSupport.IsLocked = false;
                                unlockSupport.UpdatedByUserId = currentUser.FirstOrDefault().Id;
                                unlockSupport.UpdatedDateTime = DateTime.Today;
                                db.SubmitChanges();

                                return Request.CreateResponse(HttpStatusCode.OK);
                            }
                            else
                            {
                                return Request.CreateResponse(HttpStatusCode.BadRequest, "Current support is unlocked!");
                            }
                        }
                        else
                        {
                            return Request.CreateResponse(HttpStatusCode.NotFound, "Support data not found!");
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
        public HttpResponseMessage DeleteSupport(String id)
        {
            try
            {
                var currentUser = from d in db.MstUsers where d.UserId == User.Identity.GetUserId() select d;

                if (currentUser.Any())
                {
                    var userRights = from d in db.MstUserForms
                                     where d.UserId == currentUser.FirstOrDefault().Id
                                     && d.SysForm.FormName == "CRMSupport"
                                     select d;

                    if (currentUser.FirstOrDefault().CRMUserGroup == "Support Manager" || currentUser.FirstOrDefault().CRMUserGroup == "Support" || currentUser.FirstOrDefault().CRMUserGroup == "Customer" || currentUser.FirstOrDefault().CRMUserGroup == "Easyfis Staff" || currentUser.FirstOrDefault().CRMUserGroup == "Admin")
                    {
                        var currentSupport = from d in db.CrmTrnSupports
                                             where d.Id == Convert.ToInt32(id)
                                             select d;

                        if (currentSupport.Any())
                        {
                            if (!currentSupport.FirstOrDefault().IsLocked)
                            {
                                db.CrmTrnSupports.DeleteOnSubmit(currentSupport.FirstOrDefault());
                                db.SubmitChanges();

                                return Request.CreateResponse(HttpStatusCode.OK);
                            }
                            else
                            {
                                return Request.CreateResponse(HttpStatusCode.BadRequest, "Current support is Locked!");
                            }
                        }
                        else
                        {
                            return Request.CreateResponse(HttpStatusCode.NotFound, "Support data not found!");
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
