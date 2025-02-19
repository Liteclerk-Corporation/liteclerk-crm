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
    [Authorize, RoutePrefix("api/crm/trn/activity")]
    public class ApiCrmTrnActivityController : ApiController
    {
        public Data.easyfiscrmdbDataContext db = new Data.easyfiscrmdbDataContext();

        public String FillLeadingZeroes(Int32 number, Int32 length)
        {
            String result = number.ToString();
            Int32 pad = length - result.Length;
            while (pad > 0) { result = '0' + result; pad--; }

            return result;
        }

        [HttpGet, Route("list/{document}/{id}")]
        public List<Entities.CrmTrnActivityEntity> ListActivity(String id, String document)
        {
            if (document == "LEAD")
            {
                var activities = from d in db.CrmTrnActivities
                                 where d.LDId == Convert.ToInt32(id)
                                 && d.CrmTrnLead.IsLocked == true
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

                return activities.OrderByDescending(d => d.Id).ToList();
            }
            else if (document == "SALES DELIVERY")
            {
                var activities = from d in db.CrmTrnActivities
                                 where d.SDId == Convert.ToInt32(id)
                                 && d.CrmTrnSalesDelivery.IsLocked == true
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

                return activities.OrderByDescending(d => d.Id).ToList();
            }
            else if (document == "SUPPORT")
            {
                var activities = from d in db.CrmTrnActivities
                                 where d.SPId == Convert.ToInt32(id)
                                 && d.CrmTrnSupport.IsLocked == true
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

                return activities.OrderByDescending(d => d.Id).ToList();
            }
            else
            {
                return new List<Entities.CrmTrnActivityEntity>();
            }


        }

        [HttpGet, Route("lead/list/{LDId}")]
        public List<Entities.CrmTrnActivityEntity> ListLeadActivity(String LDId)
        {
            var activities = from d in db.CrmTrnActivities
                             where d.LDId == Convert.ToInt32(LDId)
                             && d.CrmTrnLead.IsLocked == true
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

            return activities.OrderByDescending(d => d.Id).ToList();
        }

        [HttpGet, Route("sales/list/{SDId}")]
        public List<Entities.CrmTrnActivityEntity> ListSalesDeliveryActivity(String SDId)
        {
            var activities = from d in db.CrmTrnActivities
                             where d.SDId == Convert.ToInt32(SDId)
                             && d.CrmTrnSalesDelivery.IsLocked == true
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

            return activities.OrderByDescending(d => d.Id).ToList();
        }

        [HttpGet, Route("support/list/{SPId}")]
        public List<Entities.CrmTrnActivityEntity> ListSupportActivity(String SPId)
        {
            var activities = from d in db.CrmTrnActivities
                             where d.SPId == Convert.ToInt32(SPId)
                             && d.CrmTrnSupport.IsLocked == true
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

            return activities.OrderByDescending(d => d.Id).ToList();
        }

        [HttpGet, Route("functional")]
        public List<Entities.CrmMstUserEntity> ListFunctional()
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

        [HttpGet, Route("users")]
        public List<Entities.CrmMstUserEntity> ListUsers()
        {
            var currentUser = from d in db.MstUsers
                              where d.UserId == User.Identity.GetUserId()
                              select d;


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

        [HttpGet, Route("status")]
        public List<Entities.CrmMstStatusEntity> ListStatus()
        {
            var statuses = from d in db.MstStatus
                           where d.Category.Equals("AC")
                           select new Entities.CrmMstStatusEntity
                           {
                               Id = d.Id,
                               Status = d.Status
                           };

            return statuses.ToList();
        }

        [HttpPost, Route("add")]
        public HttpResponseMessage AddActivity(Entities.CrmTrnActivityEntity objActivity)
        {
            try
            {
                Boolean isValid = true;
                String invalidMessage = "";

                if (objActivity.LDId != null)
                {
                    var currentLead = from d in db.CrmTrnLeads where d.Id == objActivity.LDId select d;
                    if (currentLead.Any())
                    {
                        if (!currentLead.FirstOrDefault().IsLocked)
                        {
                            isValid = false;
                            invalidMessage = "Current lead is not locked!";
                        }
                    }
                }

                if (objActivity.SDId != null)
                {
                    var currentSalesDelivery = from d in db.CrmTrnSalesDeliveries where d.Id == objActivity.SDId select d;
                    if (currentSalesDelivery.Any())
                    {
                        if (!currentSalesDelivery.FirstOrDefault().IsLocked)
                        {
                            isValid = false;
                            invalidMessage = "Current sales is not locked!";
                        }
                    }
                }

                if (objActivity.SPId != null)
                {
                    var currentSupport = from d in db.CrmTrnSupports where d.Id == objActivity.SPId select d;
                    if (currentSupport.Any())
                    {
                        if (!currentSupport.FirstOrDefault().IsLocked)
                        {
                            isValid = false;
                            invalidMessage = "Current Support is not locked!";
                        }
                    }
                }

                if (objActivity.SPId == null && objActivity.LDId == null && objActivity.SDId == null)
                {
                    isValid = false;
                    invalidMessage = "Null SPId, SDId and LDID!";

                }

                if (isValid)
                {
                    String ACNumber = "0000000001";

                    var lastActivity = from d in db.CrmTrnActivities.OrderByDescending(d => d.Id) select d;
                    if (lastActivity.Any())
                    {
                        Int32 newACNumber = Convert.ToInt32(lastActivity.FirstOrDefault().ACNumber) + 0000000001;
                        ACNumber = FillLeadingZeroes(newACNumber, 10);
                    }

                    var currentUser = from d in db.MstUsers where d.UserId == User.Identity.GetUserId() select d;
                    Int32 currentUserId = currentUser.FirstOrDefault().Id;

                    String startDateTime = Convert.ToDateTime(objActivity.StartDate).ToShortDateString() + " " + Convert.ToDateTime(objActivity.StartTime).ToShortTimeString();
                    String endDateTime = Convert.ToDateTime(objActivity.EndDate).ToShortDateString() + " " + Convert.ToDateTime(objActivity.EndTime).ToShortTimeString();

                    Data.CrmTrnActivity newActivity = new Data.CrmTrnActivity
                    {
                        ACNumber = ACNumber,
                        ACDate = Convert.ToDateTime(objActivity.ACDate),
                        UserId = currentUserId,
                        FunctionalUserId = objActivity.FunctionalUserId,
                        TechnicalUserId = objActivity.TechnicalUserId,
                        CRMStatus = objActivity.CRMStatus,
                        Activity = objActivity.Activity,
                        StartDateTime = Convert.ToDateTime(startDateTime),
                        EndDateTime = Convert.ToDateTime(endDateTime),
                        TransportationCost = objActivity.TransportationCost,
                        OnSiteCost = objActivity.OnSiteCost,
                        LDId = objActivity.LDId,
                        SDId = objActivity.SDId,
                        SPId = objActivity.SPId,
                        Status = objActivity.Status,
                        IsLocked = false,
                        CreatedByUserId = currentUserId,
                        CreatedDateTime = DateTime.Now,
                        UpdatedByUserId = currentUserId,
                        UpdatedDateTime = DateTime.Now
                    };

                    db.CrmTrnActivities.InsertOnSubmit(newActivity);
                    db.SubmitChanges();

                    return Request.CreateResponse(HttpStatusCode.OK);
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, invalidMessage);
                }
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpPut, Route("update/{id}")]
        public HttpResponseMessage UpdateActivity(String id, Entities.CrmTrnActivityEntity objActivity)
        {
            try
            {
                var currentActivity = from d in db.CrmTrnActivities
                                      where d.Id == Convert.ToInt32(id)
                                      select d;

                if (currentActivity.Any())
                {
                    Boolean isValid = true;
                    String invalidMessage = "";

                    if (currentActivity.FirstOrDefault().LDId != null)
                    {
                        if (!currentActivity.FirstOrDefault().CrmTrnLead.IsLocked)
                        {
                            isValid = false;
                            invalidMessage = "Current lead is not locked!";
                        }
                    }

                    if (currentActivity.FirstOrDefault().SDId != null)
                    {

                        if (!currentActivity.FirstOrDefault().CrmTrnSalesDelivery.IsLocked)
                        {
                            isValid = false;
                            invalidMessage = "Current sales is not locked!";
                        }

                    }

                    if (currentActivity.FirstOrDefault().SPId != null)
                    {

                        if (!currentActivity.FirstOrDefault().CrmTrnSupport.IsLocked)
                        {
                            isValid = false;
                            invalidMessage = "Current Support is not locked!";
                        }
                    }

                    if (objActivity.SPId == null && objActivity.LDId == null && objActivity.SDId == null)
                    {
                        isValid = false;
                        invalidMessage = "Null SPId, SDId and LDID!";

                    }

                    if (isValid)
                    {
                        var currentUser = from d in db.MstUsers where d.UserId == User.Identity.GetUserId() select d;
                        Int32 currentUserId = currentUser.FirstOrDefault().Id;

                        String startDateTime = Convert.ToDateTime(objActivity.StartDate).ToShortDateString() + " " + Convert.ToDateTime(objActivity.StartTime).ToShortTimeString();
                        String endDateTime = Convert.ToDateTime(objActivity.EndDate).ToShortDateString() + " " + Convert.ToDateTime(objActivity.EndTime).ToShortTimeString();

                        var updateCurrentActivity = currentActivity.FirstOrDefault();
                        updateCurrentActivity.ACDate = Convert.ToDateTime(objActivity.ACDate);
                        updateCurrentActivity.FunctionalUserId = objActivity.FunctionalUserId;
                        updateCurrentActivity.TechnicalUserId = objActivity.TechnicalUserId;
                        updateCurrentActivity.CRMStatus = objActivity.CRMStatus;
                        updateCurrentActivity.Activity = objActivity.Activity;
                        updateCurrentActivity.StartDateTime = Convert.ToDateTime(startDateTime);
                        updateCurrentActivity.EndDateTime = Convert.ToDateTime(endDateTime);
                        updateCurrentActivity.TransportationCost = objActivity.TransportationCost;
                        updateCurrentActivity.OnSiteCost = objActivity.OnSiteCost;
                        updateCurrentActivity.LDId = objActivity.LDId;
                        updateCurrentActivity.SDId = objActivity.SDId;
                        updateCurrentActivity.SPId = objActivity.SPId;
                        updateCurrentActivity.Status = objActivity.Status;
                        updateCurrentActivity.UpdatedByUserId = currentUserId;
                        updateCurrentActivity.UpdatedDateTime = DateTime.Now;
                        db.SubmitChanges();

                        return Request.CreateResponse(HttpStatusCode.OK);
                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.BadRequest, invalidMessage);
                    }
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Reference not found!");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpDelete, Route("delete/{id}")]
        public HttpResponseMessage DeleteLead(String id)
        {
            try
            {
                var currentActivity = from d in db.CrmTrnActivities
                                      where d.Id == Convert.ToInt32(id)
                                      select d;

                if (currentActivity.Any())
                {
                    Boolean isValid = true;
                    String invalidMessage = "";

                    if (currentActivity.FirstOrDefault().LDId != null)
                    {
                        if (!currentActivity.FirstOrDefault().CrmTrnLead.IsLocked)
                        {
                            isValid = false;
                            invalidMessage = "Current lead is not locked!";
                        }
                    }

                    if (isValid)
                    {
                        db.CrmTrnActivities.DeleteOnSubmit(currentActivity.FirstOrDefault());
                        db.SubmitChanges();

                        return Request.CreateResponse(HttpStatusCode.OK);
                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.BadRequest, invalidMessage);
                    }
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Reference not found!");
                }
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpGet, Route("list/status/activity")]
        public List<Entities.CrmMstStatusEntity> ListActivityStatus()
        {
            var status = from d in db.MstStatus
                         where d.Category.Equals("AC")
                         select new Entities.CrmMstStatusEntity
                         {
                             Id = d.Id,
                             Status = d.Status
                         };
            return status.ToList();
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

        public String GetLastActivity(String document, Int32 documentId)
        {
            if (document == "LEAD")
            {
                var activities = from d in db.CrmTrnActivities.OrderByDescending(d => d.Id)
                                 where d.LDId == documentId
                                 select d;

                if (activities.Any())
                {
                    return activities.FirstOrDefault().Activity;
                }

            }

            if (document == "SALES DELIVERY")
            {
                var activities = from d in db.CrmTrnActivities.OrderByDescending(d => d.Id)
                                 where d.SDId == documentId
                                 select d;

                if (activities.Any())
                {
                    return activities.FirstOrDefault().Activity;
                }
            }

            if (document == "SUPPORT")
            {
                var activities = from d in db.CrmTrnActivities.OrderByDescending(d => d.Id)
                                 where d.SPId == documentId
                                 select d;

                if (activities.Any())
                {
                    return activities.FirstOrDefault().Activity;
                }

            }
            return "";
        }

        [HttpGet, Route("summary/list/{startDate}/{endDate}/{docType}/{status}/{functionalId}")]
        public List<Entities.CrmActivityDetailSummaryEntity> ListActivitySummary(String startDate, String endDate, String docType, String status, Int32 functionalId)
        {

            if (docType == "LEAD")
            {
                var leadActivities = from d in db.CrmTrnActivities
                                     where d.ACDate >= Convert.ToDateTime(startDate)
                                     && d.ACDate <= Convert.ToDateTime(endDate)
                                     && d.FunctionalUserId == Convert.ToInt32(functionalId)
                                     && d.LDId != null
                                     && d.CrmTrnLead.IsLocked == true
                                     select new Entities.CrmActivityDetailSummaryEntity
                                     {
                                         Id = d.Id,
                                         ACNumber = d.ACNumber,
                                         ACDate = d.ACDate.ToShortDateString(),
                                         DocumentId = d.CrmTrnLead.Id,
                                         DocumentNumber = "LD-" + d.CrmTrnLead.LDNumber,
                                         Document = "LEAD",
                                         DocumentStatus = d.CrmTrnLead.Status,
                                         Customer = d.CrmTrnLead.Name,
                                         Product = d.CrmTrnLead.CrmMstProduct.ProductDescription,
                                         AssignedToId = d.CrmTrnLead.MstUser.Id,
                                         AssignedTo = d.CrmTrnLead.MstUser.FullName,
                                         CreatedBy = d.CrmTrnLead.MstUser1.FullName,
                                         Particulars = d.CrmTrnLead.Remarks,
                                         Activity = d.Activity,
                                         StartDate = d.StartDateTime.ToShortDateString(),
                                         EndDate = d.EndDateTime.ToShortDateString(),
                                         FunctionalStaff = d.MstUser1.FullName,
                                         TechnicalStaff = d.MstUser2.FullName,
                                         Status = d.Status,
                                         TotalAmount = d.TransportationCost + d.OnSiteCost,
                                         CreatedByUserId = d.CreatedByUserId,
                                         CreatedByUser = d.MstUser3.FullName,
                                         CreatedDateTime = d.CreatedDateTime.ToShortDateString(),
                                         UpdatedByUserId = d.UpdatedByUserId,
                                         UpdatedByUser = d.MstUser4.FullName,
                                         UpdatedDateTime = d.UpdatedDateTime.ToShortDateString()
                                     };

                if (status.Equals("ALL"))
                {
                    return leadActivities.OrderByDescending(d => d.Id).ToList();
                }
                else
                {
                    return leadActivities.Where(d => d.Status.Equals(status)).OrderByDescending(d => d.Id).ToList();
                }

            }
            else if (docType == "SALES DELIVERY")
            {
                var salesDeliveryActivities = from d in db.CrmTrnActivities
                                              where d.ACDate >= Convert.ToDateTime(startDate)
                                              && d.ACDate <= Convert.ToDateTime(endDate)
                                              && d.FunctionalUserId == Convert.ToInt32(functionalId)
                                              && d.SDId != null
                                              && d.CrmTrnSalesDelivery.IsLocked == true
                                              select new Entities.CrmActivityDetailSummaryEntity
                                              {
                                                  Id = d.Id,
                                                  ACNumber = d.ACNumber,
                                                  ACDate = d.ACDate.ToShortDateString(),
                                                  DocumentId = d.CrmTrnSalesDelivery.Id,
                                                  DocumentNumber = "SD-" + d.CrmTrnSalesDelivery.SDNumber,
                                                  Document = "SALES DELIVERY",
                                                  DocumentStatus = d.CrmTrnSalesDelivery.Status,
                                                  Customer = d.CrmTrnSalesDelivery.MstArticle.Article,
                                                  Product = d.CrmTrnSalesDelivery.CrmMstProduct.ProductDescription,
                                                  AssignedToId = d.CrmTrnSalesDelivery.MstUser.Id,
                                                  AssignedTo = d.CrmTrnSalesDelivery.MstUser.FullName,
                                                  CreatedBy = d.CrmTrnSalesDelivery.MstUser1.FullName,
                                                  Particulars = d.CrmTrnSalesDelivery.Particulars,
                                                  Activity = d.Activity,
                                                  StartDate = d.StartDateTime.ToShortDateString(),
                                                  EndDate = d.EndDateTime.ToShortDateString(),
                                                  FunctionalStaff = d.MstUser1.FullName,
                                                  TechnicalStaff = d.MstUser2.FullName,
                                                  Status = d.Status,
                                                  TotalAmount = d.TransportationCost + d.OnSiteCost,
                                                  CreatedByUserId = d.CreatedByUserId,
                                                  CreatedByUser = d.MstUser3.FullName,
                                                  CreatedDateTime = d.CreatedDateTime.ToShortDateString(),
                                                  UpdatedByUserId = d.UpdatedByUserId,
                                                  UpdatedByUser = d.MstUser4.FullName,
                                                  UpdatedDateTime = d.UpdatedDateTime.ToShortDateString()
                                              };

                if (status.Equals("ALL"))
                {
                    return salesDeliveryActivities.OrderByDescending(d => d.Id).ToList();
                }
                else
                {
                    return salesDeliveryActivities.Where(d => d.Status.Equals(status)).OrderByDescending(d => d.Id).ToList();
                }
            }
            else if (docType == "SUPPORT")
            {
                var supportActivities = from d in db.CrmTrnActivities
                                        where d.ACDate >= Convert.ToDateTime(startDate)
                                        && d.ACDate <= Convert.ToDateTime(endDate)
                                        && d.FunctionalUserId == Convert.ToInt32(functionalId)
                                        && d.SPId != null
                                        && d.CrmTrnSupport.IsLocked == true
                                        select new Entities.CrmActivityDetailSummaryEntity
                                        {
                                            Id = d.Id,
                                            ACNumber = d.ACNumber,
                                            ACDate = d.ACDate.ToShortDateString(),
                                            DocumentId = d.CrmTrnSupport.Id,
                                            DocumentNumber = "SP-" + d.CrmTrnSupport.SPNumber,
                                            Document = "SUPPORT",
                                            DocumentStatus = d.CrmTrnSupport.Status,
                                            Customer = d.CrmTrnSupport.MstArticle.Article,
                                            Product = d.CrmTrnSupport.CrmTrnSalesDelivery.CrmMstProduct.ProductDescription,
                                            AssignedToId = d.CrmTrnSupport.MstUser.Id,
                                            AssignedTo = d.CrmTrnSupport.MstUser.FullName,
                                            CreatedBy = d.CrmTrnSupport.MstUser1.FullName,
                                            Particulars = d.CrmTrnSupport.Issue,
                                            Activity = d.Activity,
                                            StartDate = d.StartDateTime.ToShortDateString(),
                                            EndDate = d.EndDateTime.ToShortDateString(),
                                            FunctionalStaff = d.MstUser1.FullName,
                                            TechnicalStaff = d.MstUser2.FullName,
                                            Status = d.Status,
                                            TotalAmount = d.TransportationCost + d.OnSiteCost,
                                            CreatedByUserId = d.CreatedByUserId,
                                            CreatedByUser = d.MstUser3.FullName,
                                            CreatedDateTime = d.CreatedDateTime.ToShortDateString(),
                                            UpdatedByUserId = d.UpdatedByUserId,
                                            UpdatedByUser = d.MstUser4.FullName,
                                            UpdatedDateTime = d.UpdatedDateTime.ToShortDateString()

                                        };

                if (status.Equals("ALL"))
                {
                    return supportActivities.OrderByDescending(d => d.Id).ToList();
                }
                else
                {
                    return supportActivities.Where(d => d.Status.Equals(status)).OrderByDescending(d => d.Id).ToList();
                }
            }
            else
            {

                List<Entities.CrmActivityDetailSummaryEntity> crmActivitySummaries = new List<Entities.CrmActivityDetailSummaryEntity>();

                var leadActivities = from d in db.CrmTrnActivities
                                     where d.ACDate >= Convert.ToDateTime(startDate)
                                     && d.ACDate <= Convert.ToDateTime(endDate)
                                     && d.FunctionalUserId == Convert.ToInt32(functionalId)
                                     && d.LDId != null
                                     && d.CrmTrnLead.IsLocked == true
                                     select new Entities.CrmActivityDetailSummaryEntity
                                     {
                                         Id = d.Id,
                                         ACNumber = d.ACNumber,
                                         ACDate = d.ACDate.ToShortDateString(),
                                         DocumentId = d.CrmTrnLead.Id,
                                         DocumentNumber = "LD-" + d.CrmTrnLead.LDNumber,
                                         Document = "LEAD",
                                         DocumentStatus = d.CrmTrnLead.Status,
                                         Customer = d.CrmTrnLead.Name,
                                         Product = d.CrmTrnLead.CrmMstProduct.ProductDescription,
                                         AssignedToId = d.CrmTrnLead.MstUser.Id,
                                         AssignedTo = d.CrmTrnLead.MstUser.FullName,
                                         CreatedBy = d.CrmTrnLead.MstUser1.FullName,
                                         Particulars = d.CrmTrnLead.Remarks,
                                         Activity = d.Activity,
                                         StartDate = d.StartDateTime.ToShortDateString(),
                                         EndDate = d.EndDateTime.ToShortDateString(),
                                         FunctionalStaff = d.MstUser1.FullName,
                                         TechnicalStaff = d.MstUser2.FullName,
                                         Status = d.Status,
                                         TotalAmount = d.TransportationCost + d.OnSiteCost,
                                         CreatedByUserId = d.CreatedByUserId,
                                         CreatedByUser = d.MstUser3.FullName,
                                         CreatedDateTime = d.CreatedDateTime.ToShortDateString(),
                                         UpdatedByUserId = d.UpdatedByUserId,
                                         UpdatedByUser = d.MstUser4.FullName,
                                         UpdatedDateTime = d.UpdatedDateTime.ToShortDateString()
                                     };

                crmActivitySummaries.AddRange(leadActivities);

                var salesDeliveryActivities = from d in db.CrmTrnActivities
                                              where d.ACDate >= Convert.ToDateTime(startDate)
                                              && d.ACDate <= Convert.ToDateTime(endDate)
                                              && d.FunctionalUserId == Convert.ToInt32(functionalId)
                                              && d.SDId != null
                                              && d.CrmTrnSalesDelivery.IsLocked == true
                                              select new Entities.CrmActivityDetailSummaryEntity
                                              {
                                                  Id = d.Id,
                                                  ACNumber = d.ACNumber,
                                                  ACDate = d.ACDate.ToShortDateString(),
                                                  DocumentId = d.CrmTrnSalesDelivery.Id,
                                                  DocumentNumber = "SD-" + d.CrmTrnSalesDelivery.SDNumber,
                                                  Document = "SALES DELIVERY",
                                                  DocumentStatus = d.CrmTrnSalesDelivery.Status,
                                                  Customer = d.CrmTrnSalesDelivery.MstArticle.Article,
                                                  Product = d.CrmTrnSalesDelivery.CrmMstProduct.ProductDescription,
                                                  AssignedTo = d.CrmTrnSalesDelivery.MstUser.FullName,
                                                  AssignedToId = d.CrmTrnSalesDelivery.MstUser.Id,
                                                  CreatedBy = d.CrmTrnSalesDelivery.MstUser1.FullName,
                                                  Particulars = d.CrmTrnSalesDelivery.Particulars,
                                                  Activity = d.Activity,
                                                  StartDate = d.StartDateTime.ToShortDateString(),
                                                  EndDate = d.EndDateTime.ToShortDateString(),
                                                  FunctionalStaff = d.MstUser1.FullName,
                                                  TechnicalStaff = d.MstUser2.FullName,
                                                  Status = d.Status,
                                                  TotalAmount = d.TransportationCost + d.OnSiteCost,
                                                  CreatedByUserId = d.CreatedByUserId,
                                                  CreatedByUser = d.MstUser3.FullName,
                                                  CreatedDateTime = d.CreatedDateTime.ToShortDateString(),
                                                  UpdatedByUserId = d.UpdatedByUserId,
                                                  UpdatedByUser = d.MstUser4.FullName,
                                                  UpdatedDateTime = d.UpdatedDateTime.ToShortDateString()
                                              };

                crmActivitySummaries.AddRange(salesDeliveryActivities);

                var supportActivities = from d in db.CrmTrnActivities
                                        where d.ACDate >= Convert.ToDateTime(startDate)
                                        && d.ACDate <= Convert.ToDateTime(endDate)
                                        && d.FunctionalUserId == Convert.ToInt32(functionalId)
                                        && d.SPId != null
                                        && d.CrmTrnSupport.IsLocked == true
                                        select new Entities.CrmActivityDetailSummaryEntity
                                        {
                                            Id = d.Id,
                                            ACNumber = d.ACNumber,
                                            ACDate = d.ACDate.ToShortDateString(),
                                            DocumentId = d.CrmTrnSupport.Id,
                                            DocumentNumber = "SP-" + d.CrmTrnSupport.SPNumber,
                                            Document = "SUPPORT",
                                            DocumentStatus = d.CrmTrnSupport.Status,
                                            Customer = d.CrmTrnSupport.MstArticle.Article,
                                            Product = d.CrmTrnSupport.CrmTrnSalesDelivery.CrmMstProduct.ProductDescription,
                                            AssignedToId = d.CrmTrnSupport.MstUser.Id,
                                            AssignedTo = d.CrmTrnSupport.MstUser.FullName,
                                            CreatedBy = d.CrmTrnSupport.MstUser1.FullName,
                                            Particulars = d.CrmTrnSupport.Issue,
                                            Activity = d.Activity,
                                            StartDate = d.StartDateTime.ToShortDateString(),
                                            EndDate = d.EndDateTime.ToShortDateString(),
                                            FunctionalStaff = d.MstUser1.FullName,
                                            TechnicalStaff = d.MstUser2.FullName,
                                            Status = d.Status,
                                            TotalAmount = d.TransportationCost + d.OnSiteCost,
                                            CreatedByUserId = d.CreatedByUserId,
                                            CreatedByUser = d.MstUser3.FullName,
                                            CreatedDateTime = d.CreatedDateTime.ToShortDateString(),
                                            UpdatedByUserId = d.UpdatedByUserId,
                                            UpdatedByUser = d.MstUser4.FullName,
                                            UpdatedDateTime = d.UpdatedDateTime.ToShortDateString()
                                        };

                crmActivitySummaries.AddRange(supportActivities);

                if (status.Equals("ALL"))
                {
                    return crmActivitySummaries.OrderByDescending(d => d.Id).ToList();
                }
                else
                {
                    return crmActivitySummaries.Where(d => d.Status.Equals(status)).OrderByDescending(d => d.Id).ToList();
                }
            }
        }
    }
}
