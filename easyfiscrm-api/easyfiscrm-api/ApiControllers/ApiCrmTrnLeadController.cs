using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Microsoft.AspNet.Identity;

namespace easyfiscrm_api.ApiControllers
{
    [Authorize, RoutePrefix("api/crm/trn/lead")]
    public class ApiCrmTrnLeadController : ApiController
    {
        public Data.easyfiscrmdbDataContext db = new Data.easyfiscrmdbDataContext();

        public String FillLeadingZeroes(Int32 number, Int32 length)
        {
            String result = number.ToString();
            Int32 pad = length - result.Length;
            while (pad > 0) { result = '0' + result; pad--; }

            return result;
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

        [HttpGet, Route("list/{startDate}/{endDate}/{status}")]
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
            else {
                return new List<Entities.CrmTrnLeadEntity>();
            }
        }

        // Dashboard lead list
        [HttpGet, Route("list/{startDate}/{endDate}/{status}/{userId}")]
        public List<Entities.CrmTrnLeadEntity> ListLeadFilteredByUser(String startDate, String endDate, String status, String userId)
        {
            var leads = from d in db.CrmTrnLeads
                        where d.LDDate >= Convert.ToDateTime(startDate)
                        && d.LDDate <= Convert.ToDateTime(endDate)
                        && d.AssignedToUserId == Convert.ToInt32(userId)
                        && d.IsLocked == true
                        select new Entities.CrmTrnLeadEntity
                        {
                            Id = d.Id,
                            LDNumber = d.LDNumber,
                            LDDate = d.LDDate.ToShortDateString(),
                            Name = d.Name,
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

            return leads.Where(d => d.Status.Equals(status)).OrderByDescending(d => d.Id).ToList();
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
                           where d.Category.Equals("LD")
                           select new Entities.CrmMstStatusEntity
                           {
                               Id = d.Id,
                               Status = d.Status
                           };

            return statuses.ToList();
        }

        [HttpGet, Route("detail/{id}")]
        public Entities.CrmTrnLeadEntity DetailLead(String id)
        {
            var lead = from d in db.CrmTrnLeads
                       where d.Id == Convert.ToInt32(id)
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
                           LastActivity = "",
                           Status = d.Status,
                           IsLocked = d.IsLocked,
                           CreatedByUserId = d.CreatedByUserId,
                           CreatedByUser = d.MstUser1.FullName,
                           CreatedDateTime = d.CreatedDateTime.ToShortDateString(),
                           UpdatedByUserId = d.UpdatedByUserId,
                           UpdatedByUser = d.MstUser2.FullName,
                           UpdatedDateTime = d.UpdatedDateTime.ToShortDateString()
                       };

            return lead.FirstOrDefault();
        }

        [HttpPost, Route("add")]
        public HttpResponseMessage AddLead()
        {
            try
            {
                var currentUser = from d in db.MstUsers where d.UserId == User.Identity.GetUserId() select d;

                if (currentUser.Any())
                {
                    if (currentUser.FirstOrDefault().CRMUserGroup == "Sales Manager" || currentUser.FirstOrDefault().CRMUserGroup == "Sales" || currentUser.FirstOrDefault().CRMUserGroup == "Easyfis Staff" || currentUser.FirstOrDefault().CRMUserGroup == "Admin")
                    {
                        String LDNumber = "0000000001";

                        var lastLead = from d in db.CrmTrnLeads.OrderByDescending(d => d.Id) select d;
                        if (lastLead.Any())
                        {
                            Int32 newLDNumber = Convert.ToInt32(lastLead.FirstOrDefault().LDNumber) + 0000000001;
                            LDNumber = FillLeadingZeroes(newLDNumber, 10);
                        }

                        var product = from d in db.CrmMstProducts select d;
                        Int32 productId = product.FirstOrDefault().Id;

                        var status = from d in db.MstStatus
                                     where d.Category.Equals("LD")
                                     select d;

                        String leadStatus = "";
                        if (status.Any())
                        {
                            leadStatus = status.FirstOrDefault().Status;
                        }

                        Data.CrmTrnLead newLead = new Data.CrmTrnLead
                        {
                            LDNumber = LDNumber,
                            LDDate = DateTime.Today,
                            Name = "NA",
                            ProductId = productId,
                            TotalAmount = 0,
                            Address = "NA",
                            ContactPerson = "NA",
                            ContactPosition = "NA",
                            ContactEmail = "NA",
                            ContactPhoneNumber = "NA",
                            ReferredBy = "NA",
                            Remarks = "NA",
                            AssignedToUserId = currentUser.FirstOrDefault().Id,
                            Status = leadStatus,
                            IsLocked = false,
                            CreatedByUserId = currentUser.FirstOrDefault().Id,
                            CreatedDateTime = DateTime.Now,
                            UpdatedByUserId = currentUser.FirstOrDefault().Id,
                            UpdatedDateTime = DateTime.Now
                        };

                        db.CrmTrnLeads.InsertOnSubmit(newLead);
                        db.SubmitChanges();

                        return Request.CreateResponse(HttpStatusCode.OK, newLead.Id);
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
        public HttpResponseMessage SaveLead(String id, Entities.CrmTrnLeadEntity objLead)
        {
            try
            {
                var currentUser = from d in db.MstUsers where d.UserId == User.Identity.GetUserId() select d;

                if (currentUser.Any())
                {
                    if (currentUser.FirstOrDefault().CRMUserGroup == "Sales Manager" || currentUser.FirstOrDefault().CRMUserGroup == "Sales" || currentUser.FirstOrDefault().CRMUserGroup == "Easyfis Staff" || currentUser.FirstOrDefault().CRMUserGroup == "Admin")
                    {
                        var currentLead = from d in db.CrmTrnLeads
                                          where d.Id == Convert.ToInt32(id)
                                          select d;

                        if (currentLead.Any())
                        {
                            if (!currentLead.FirstOrDefault().IsLocked)
                            {
                                var saveCurrentLead = currentLead.FirstOrDefault();
                                saveCurrentLead.LDDate = Convert.ToDateTime(objLead.LDDate);
                                saveCurrentLead.Name = objLead.Name;
                                saveCurrentLead.ProductId = objLead.ProductId;
                                saveCurrentLead.TotalAmount = objLead.TotalAmount;
                                saveCurrentLead.Address = objLead.Address;
                                saveCurrentLead.ContactPerson = objLead.ContactPerson;
                                saveCurrentLead.ContactPosition = objLead.ContactPosition;
                                saveCurrentLead.ContactEmail = objLead.ContactEmail;
                                saveCurrentLead.ContactPhoneNumber = objLead.ContactPhoneNumber;
                                saveCurrentLead.ReferredBy = objLead.ReferredBy;
                                saveCurrentLead.Remarks = objLead.Remarks;
                                saveCurrentLead.AssignedToUserId = objLead.AssignedToUserId;
                                saveCurrentLead.Status = objLead.Status;
                                saveCurrentLead.UpdatedByUserId = currentUser.FirstOrDefault().Id;
                                saveCurrentLead.UpdatedDateTime = DateTime.Now;
                                db.SubmitChanges();

                                return Request.CreateResponse(HttpStatusCode.OK);
                            }
                            else
                            {
                                return Request.CreateResponse(HttpStatusCode.BadRequest, "Current lead is locked!");
                            }
                        }
                        else
                        {
                            return Request.CreateResponse(HttpStatusCode.NotFound, "Lead data not found!");
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
        public HttpResponseMessage LockLead(String id, Entities.CrmTrnLeadEntity objLead)
        {
            try
            {
                var currentUser = from d in db.MstUsers where d.UserId == User.Identity.GetUserId() select d;

                if (currentUser.Any())
                {
                    if (currentUser.FirstOrDefault().CRMUserGroup == "Sales Manager" || currentUser.FirstOrDefault().CRMUserGroup == "Sales" || currentUser.FirstOrDefault().CRMUserGroup == "Easyfis Staff" || currentUser.FirstOrDefault().CRMUserGroup == "Admin")
                    {
                        var currentLead = from d in db.CrmTrnLeads
                                          where d.Id == Convert.ToInt32(id)
                                          select d;

                        if (currentLead.Any())
                        {
                            if (!currentLead.FirstOrDefault().IsLocked)
                            {
                                var lockCurrentLead = currentLead.FirstOrDefault();
                                lockCurrentLead.LDDate = Convert.ToDateTime(objLead.LDDate);
                                lockCurrentLead.Name = objLead.Name;
                                lockCurrentLead.ProductId = objLead.ProductId;
                                lockCurrentLead.TotalAmount = objLead.TotalAmount;
                                lockCurrentLead.Address = objLead.Address;
                                lockCurrentLead.ContactPerson = objLead.ContactPerson;
                                lockCurrentLead.ContactPosition = objLead.ContactPosition;
                                lockCurrentLead.ContactEmail = objLead.ContactEmail;
                                lockCurrentLead.ContactPhoneNumber = objLead.ContactPhoneNumber;
                                lockCurrentLead.ReferredBy = objLead.ReferredBy;
                                lockCurrentLead.Remarks = objLead.Remarks;
                                lockCurrentLead.AssignedToUserId = objLead.AssignedToUserId;
                                lockCurrentLead.Status = objLead.Status;
                                lockCurrentLead.IsLocked = true;
                                lockCurrentLead.UpdatedByUserId = currentUser.FirstOrDefault().Id;
                                lockCurrentLead.UpdatedDateTime = DateTime.Now;
                                db.SubmitChanges();

                                return Request.CreateResponse(HttpStatusCode.OK);
                            }
                            else
                            {
                                return Request.CreateResponse(HttpStatusCode.BadRequest, "Current lead is already locked!");
                            }
                        }
                        else
                        {
                            return Request.CreateResponse(HttpStatusCode.NotFound, "Lead data not found!");
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
        public HttpResponseMessage UnlockLead(String id)
        {
            try
            {
                var currentUser = from d in db.MstUsers where d.UserId == User.Identity.GetUserId() select d;

                if (currentUser.Any())
                {
                    if (currentUser.FirstOrDefault().CRMUserGroup == "Sales Manager" || currentUser.FirstOrDefault().CRMUserGroup == "Sales" || currentUser.FirstOrDefault().CRMUserGroup == "Easyfis Staff" || currentUser.FirstOrDefault().CRMUserGroup == "Admin")
                    {
                        var currentLead = from d in db.CrmTrnLeads
                                          where d.Id == Convert.ToInt32(id)
                                          select d;

                        if (currentLead.Any())
                        {
                            if (currentLead.FirstOrDefault().IsLocked)
                            {
                                var unlockCurrentLead = currentLead.FirstOrDefault();
                                unlockCurrentLead.IsLocked = false;
                                unlockCurrentLead.UpdatedByUserId = currentUser.FirstOrDefault().Id;
                                unlockCurrentLead.UpdatedDateTime = DateTime.Now;
                                db.SubmitChanges();

                                return Request.CreateResponse(HttpStatusCode.OK);
                            }
                            else
                            {
                                return Request.CreateResponse(HttpStatusCode.BadRequest, "Current lead is unlocked!");
                            }
                        }
                        else
                        {
                            return Request.CreateResponse(HttpStatusCode.NotFound, "Lead data not found!");
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
        public HttpResponseMessage DeleteLead(String id)
        {
            try
            {
                var currentUser = from d in db.MstUsers where d.UserId == User.Identity.GetUserId() select d;

                if (currentUser.Any())
                {
                    if (currentUser.FirstOrDefault().CRMUserGroup == "Sales Manager" || currentUser.FirstOrDefault().CRMUserGroup == "Sales" || currentUser.FirstOrDefault().CRMUserGroup == "Easyfis Staff" || currentUser.FirstOrDefault().CRMUserGroup == "Admin")
                    {
                        var currentLead = from d in db.CrmTrnLeads
                                          where d.Id == Convert.ToInt32(id)
                                          select d;

                        if (currentLead.Any())
                        {
                            if (!currentLead.FirstOrDefault().IsLocked)
                            {
                                db.CrmTrnLeads.DeleteOnSubmit(currentLead.FirstOrDefault());
                                db.SubmitChanges();

                                return Request.CreateResponse(HttpStatusCode.OK);
                            }
                            else
                            {
                                return Request.CreateResponse(HttpStatusCode.BadRequest, "Current lead is locked!");
                            }
                        }
                        else
                        {
                            return Request.CreateResponse(HttpStatusCode.NotFound, "Lead data not found!");
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