using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace easyfiscrm_api.ApiControllers
{
    [Authorize, RoutePrefix("api/crm/trn/software/development")]
    public class ApiCrmTrnSoftwareDevelopmentController : ApiController
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

        [HttpGet, Route("list/product")]
        public List<Entities.CrmMstProductEntity> ListProduct()
        {
            var currentUser = from d in db.MstUsers where d.UserId == User.Identity.GetUserId() select d;

            if (currentUser.Any())
            {

                var customer = from d in db.CrmMstProducts
                               select new Entities.CrmMstProductEntity
                               {
                                   Id = d.Id,
                                   ProductDescription = d.ProductDescription
                               };

                return customer.OrderBy(d => d.ProductDescription).ToList();
            }
            else
            {
                return new List<Entities.CrmMstProductEntity>();
            }
        }

        [HttpGet, Route("list/users")]
        public List<Entities.CrmMstUserEntity> ListSalesUsers()
        {

            var currentUser = from d in db.MstUsers where d.UserId == User.Identity.GetUserId() select d;

            if (currentUser.Any())
            {
                var users = from d in db.MstUsers
                            where d.CRMUserGroup != "CRMUserGroup"
                            select new Entities.CrmMstUserEntity
                            {
                                Id = d.Id,
                                UserName = d.UserName,
                                FullName = d.FullName
                            };

                return users.OrderBy(d => d.FullName).ToList();
            }
            else
            {
                return new List<Entities.CrmMstUserEntity>();
            }
        }

        [HttpGet, Route("list/status")]
        public List<Entities.CrmMstStatusEntity> ListLeadStatus()
        {
            var currentUser = from d in db.MstUsers where d.UserId == User.Identity.GetUserId() select d;

            if (currentUser.Any())
            {
                var statuses = from d in db.MstStatus
                               where d.Category.Equals("SWD")
                               select new Entities.CrmMstStatusEntity
                               {
                                   Id = d.Id,
                                   Status = d.Status
                               };

                return statuses.OrderBy(d=>d.Status).ToList();
            }
            else
            {
                return new List<Entities.CrmMstStatusEntity>();
            }
        }

        [HttpGet, Route("list/issue/type")]
        public List<Entities.SoftwareDevelopmentIssueTypeEntity> listGroup()
        {
            var currentUser = from d in db.MstUsers where d.UserId == User.Identity.GetUserId() select d;

            if (currentUser.Any())
            {
                List<Entities.SoftwareDevelopmentIssueTypeEntity> IssueTypes = new List<Entities.SoftwareDevelopmentIssueTypeEntity>();
                IssueTypes.Add(new Entities.SoftwareDevelopmentIssueTypeEntity { IssueType = "BUGS" });
                IssueTypes.Add(new Entities.SoftwareDevelopmentIssueTypeEntity { IssueType = "NEW FEATURE" });
                IssueTypes.Add(new Entities.SoftwareDevelopmentIssueTypeEntity { IssueType = "UPDATE FEATURE" });

                return IssueTypes.OrderBy(d => d.IssueType).ToList();
            }
            else
            {
                return new List<Entities.SoftwareDevelopmentIssueTypeEntity>();
            }

        }

        [Authorize, HttpGet, Route("list/{productId}")]
        public List<Entities.CrmTrnSoftwareDevelopmentEntity> ListSoftwareDevelopment(String productId)
        {
            var currentUser = from d in db.MstUsers where d.UserId == User.Identity.GetUserId() select d;

            if (currentUser.Any())
            {
                var softwareDevelopments = from d in db.CrmTrnSoftwareDevelopments
                                           where d.ProductId == Convert.ToInt32(productId)
                                           select new Entities.CrmTrnSoftwareDevelopmentEntity
                                           {
                                               Id = d.Id,
                                               SDNumber = d.SDNumber,
                                               SDDate = d.SDDate.ToShortDateString(),
                                               ProductId = d.ProductId,
                                               ProductDescription = d.CrmMstProduct.ProductCode,
                                               Issue = d.Issue,
                                               IssueType = d.IssueType,
                                               Remarks = d.Remarks,
                                               AssignedToUserId = d.AssignedToUserId,
                                               AssignedToUserFullname = d.MstUser.FullName,
                                               TargetDateTime = d.TargetDateTime.ToShortDateString(),
                                               CloseDateTime = d.CloseDateTime.ToShortDateString(),
                                               Status = d.Status,
                                               IsLocked = d.IsLocked,
                                               CreatedByUserId = d.CreatedByUserId,
                                               CreatedByUserFullname = d.MstUser1.FullName,
                                               CreatedDateTime = d.CreatedDateTime.ToShortDateString(),
                                               UpdatedByUserId = d.UpdatedByUserId,
                                               UpdatedByUserFullname = d.MstUser2.FullName,
                                               UpdatedDateTime = d.UpdatedDateTime.ToShortDateString()
                                           };

                return softwareDevelopments.OrderByDescending(d => d.Id).ToList();
            }
            else
            {
                return new List<Entities.CrmTrnSoftwareDevelopmentEntity>();
            }
        }

        [Authorize, HttpGet, Route("detail/{id}")]
        public Entities.CrmTrnSoftwareDevelopmentEntity DetailSoftwareDevelopment(String id)
        {
            var currentUser = from d in db.MstUsers where d.UserId == User.Identity.GetUserId() select d;

            if (currentUser.Any())
            {
                var softwareDevelopment = from d in db.CrmTrnSoftwareDevelopments
                                          where d.Id == Convert.ToInt32(id)
                                          select new Entities.CrmTrnSoftwareDevelopmentEntity
                                          {
                                              Id = d.Id,
                                              SDNumber = d.SDNumber,
                                              SDDate = d.SDDate.ToShortDateString(),
                                              ProductId = d.ProductId,
                                              ProductDescription = d.CrmMstProduct.ProductCode,
                                              Issue = d.Issue,
                                              IssueType = d.IssueType,
                                              Remarks = d.Remarks,
                                              AssignedToUserId = d.AssignedToUserId,
                                              AssignedToUserFullname = d.MstUser.FullName,
                                              TargetDateTime = d.TargetDateTime.ToShortDateString(),
                                              CloseDateTime = d.CloseDateTime.ToShortDateString(),
                                              Status = d.Status,
                                              IsLocked = d.IsLocked,
                                              CreatedByUserId = d.CreatedByUserId,
                                              CreatedByUserFullname = d.MstUser1.FullName,
                                              CreatedDateTime = d.CreatedDateTime.ToShortDateString(),
                                              UpdatedByUserId = d.UpdatedByUserId,
                                              UpdatedByUserFullname = d.MstUser2.FullName,
                                              UpdatedDateTime = d.UpdatedDateTime.ToShortDateString()
                                          };

                return softwareDevelopment.FirstOrDefault();
            }
            else
            {
                return new Entities.CrmTrnSoftwareDevelopmentEntity();
            }
        }

        [Authorize, HttpPost, Route("add/{productId}")]
        public HttpResponseMessage AddSoftwareDevelopment(String productId)
        {
            try
            {
                var currentUser = from d in db.MstUsers where d.UserId == User.Identity.GetUserId() select d;

                if (currentUser.Any())
                {
                    String SDNumber = "0000000001";

                    var softwareDevelopment = from d in db.CrmTrnSoftwareDevelopments.OrderByDescending(d => d.Id) select d;
                    if (softwareDevelopment.Any())
                    {
                        Int32 newLDNumber = Convert.ToInt32(softwareDevelopment.FirstOrDefault().SDNumber) + 0000000001;
                        SDNumber = FillLeadingZeroes(newLDNumber, 10);
                    }

                    var product = from d in db.CrmMstProducts
                                  where d.Id == Convert.ToInt32(productId)
                                  select d;

                    Int32 product_Id = 0;
                    if (product.Any())
                    {
                        product_Id = product.FirstOrDefault().Id;
                    }

                    Data.CrmTrnSoftwareDevelopment newSoftwareDevelopment = new Data.CrmTrnSoftwareDevelopment
                    {
                        SDNumber = SDNumber,
                        SDDate = DateTime.Today,
                        ProductId = product_Id,
                        Issue = "NA",
                        IssueType = "BUGS",
                        Remarks = "NA",
                        AssignedToUserId = currentUser.FirstOrDefault().Id,
                        TargetDateTime = DateTime.Today,
                        CloseDateTime = DateTime.Today,
                        Status = "1-OPEN",
                        IsLocked = false,
                        CreatedByUserId = currentUser.FirstOrDefault().Id,
                        CreatedDateTime = DateTime.Now,
                        UpdatedByUserId = currentUser.FirstOrDefault().Id,
                        UpdatedDateTime = DateTime.Now,
                    };

                    db.CrmTrnSoftwareDevelopments.InsertOnSubmit(newSoftwareDevelopment);
                    db.SubmitChanges();

                    return Request.CreateResponse(HttpStatusCode.OK, newSoftwareDevelopment.Id);
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "User not found!");
                }
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [Authorize, HttpPut, Route("Update/{id}")]
        public HttpResponseMessage UpdateSoftwareDevelopment(Entities.CrmTrnSoftwareDevelopmentEntity objSoftwareDevelopment, String id)
        {
            try
            {
                var currentUser = from d in db.MstUsers where d.UserId == User.Identity.GetUserId() select d;

                if (currentUser.Any())
                {
                    var softwareDevelopment = from d in db.CrmTrnSoftwareDevelopments
                                              where d.Id == Convert.ToInt32(id)
                                              select d;
                    if (softwareDevelopment.Any())
                    {
                        var product = from d in db.CrmMstProducts
                                      where d.Id == Convert.ToInt32(objSoftwareDevelopment.ProductId)
                                      select d;

                        if (!product.Any())
                        {
                            return Request.CreateResponse(HttpStatusCode.Conflict, "Product not available!");
                        }

                        var updateSoftwareDevelopment = softwareDevelopment.FirstOrDefault();
                        updateSoftwareDevelopment.SDDate = Convert.ToDateTime(objSoftwareDevelopment.SDDate);
                        updateSoftwareDevelopment.ProductId = product.FirstOrDefault().Id;
                        updateSoftwareDevelopment.Issue = objSoftwareDevelopment.Issue;
                        updateSoftwareDevelopment.Remarks = objSoftwareDevelopment.Remarks;
                        updateSoftwareDevelopment.Issue = objSoftwareDevelopment.Issue;
                        updateSoftwareDevelopment.IssueType = objSoftwareDevelopment.IssueType;
                        updateSoftwareDevelopment.AssignedToUserId = objSoftwareDevelopment.AssignedToUserId;
                        updateSoftwareDevelopment.TargetDateTime = Convert.ToDateTime(objSoftwareDevelopment.TargetDateTime);
                        updateSoftwareDevelopment.CloseDateTime = Convert.ToDateTime(objSoftwareDevelopment.CloseDateTime);
                        updateSoftwareDevelopment.Status = objSoftwareDevelopment.Status;
                        updateSoftwareDevelopment.IsLocked = objSoftwareDevelopment.IsLocked;
                        updateSoftwareDevelopment.UpdatedByUserId = currentUser.FirstOrDefault().Id;
                        updateSoftwareDevelopment.UpdatedDateTime = DateTime.Now;
                        db.SubmitChanges();

                        return Request.CreateResponse(HttpStatusCode.OK);
                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.NotFound, "Software Development not found!");
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

        [Authorize, HttpPut, Route("lock/{id}")]
        public HttpResponseMessage LockSoftwareDevelopment(Entities.CrmTrnSoftwareDevelopmentEntity objSoftwareDevelopment, String id)
        {
            try
            {
                var currentUser = from d in db.MstUsers where d.UserId == User.Identity.GetUserId() select d;

                if (currentUser.Any())
                {
                    var softwareDevelopment = from d in db.CrmTrnSoftwareDevelopments
                                              where d.Id == Convert.ToInt32(id)
                                              select d;
                    if (softwareDevelopment.Any())
                    {
                        var product = from d in db.CrmMstProducts
                                      where d.Id == Convert.ToInt32(objSoftwareDevelopment.ProductId)
                                      select d;

                        if (!product.Any())
                        {
                            return Request.CreateResponse(HttpStatusCode.Conflict, "Product not available!");
                        }

                        var loclSoftwareDevelopment = softwareDevelopment.FirstOrDefault();
                        loclSoftwareDevelopment.SDDate = Convert.ToDateTime(objSoftwareDevelopment.SDDate);
                        loclSoftwareDevelopment.ProductId = product.FirstOrDefault().Id;
                        loclSoftwareDevelopment.Issue = objSoftwareDevelopment.Issue;
                        loclSoftwareDevelopment.Remarks = objSoftwareDevelopment.Remarks;
                        loclSoftwareDevelopment.Issue = objSoftwareDevelopment.Issue;
                        loclSoftwareDevelopment.IssueType = objSoftwareDevelopment.IssueType;
                        loclSoftwareDevelopment.AssignedToUserId = objSoftwareDevelopment.AssignedToUserId;
                        loclSoftwareDevelopment.TargetDateTime = Convert.ToDateTime(objSoftwareDevelopment.TargetDateTime);
                        loclSoftwareDevelopment.CloseDateTime = Convert.ToDateTime(objSoftwareDevelopment.CloseDateTime);
                        loclSoftwareDevelopment.Status = objSoftwareDevelopment.Status;
                        loclSoftwareDevelopment.IsLocked = true;
                        loclSoftwareDevelopment.UpdatedByUserId = currentUser.FirstOrDefault().Id;
                        loclSoftwareDevelopment.UpdatedDateTime = DateTime.Now;
                        db.SubmitChanges();

                        return Request.CreateResponse(HttpStatusCode.OK);
                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.NotFound, "Software Development not found!");
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

        [Authorize, HttpPut, Route("unlock/{id}")]
        public HttpResponseMessage UnLockSoftwareDevelopment(String id)
        {
            try
            {
                var currentUser = from d in db.MstUsers where d.UserId == User.Identity.GetUserId() select d;

                if (currentUser.Any())
                {
                    var softwareDevelopment = from d in db.CrmTrnSoftwareDevelopments
                                              where d.Id == Convert.ToInt32(id)
                                              select d;
                    if (softwareDevelopment.Any())
                    {
                        var unlockSoftwareDevelopment = softwareDevelopment.FirstOrDefault();
                        unlockSoftwareDevelopment.IsLocked = false;
                        unlockSoftwareDevelopment.UpdatedByUserId = currentUser.FirstOrDefault().Id;
                        unlockSoftwareDevelopment.UpdatedDateTime = DateTime.Now;
                        db.SubmitChanges();

                        return Request.CreateResponse(HttpStatusCode.OK);
                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.NotFound, "Software Development not found!");
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

        [Authorize, HttpDelete, Route("delete/{id}")]
        public HttpResponseMessage DeleteSoftwareDevelopment(String id)
        {
            try
            {
                var currentUser = from d in db.MstUsers where d.UserId == User.Identity.GetUserId() select d;

                if (currentUser.Any())
                {
                    var softwareDevelopment = from d in db.CrmTrnSoftwareDevelopments
                                              where d.Id == Convert.ToInt32(id)
                                              select d;

                    if (softwareDevelopment.Any())
                    {
                        if (!softwareDevelopment.FirstOrDefault().IsLocked)
                        {
                            db.CrmTrnSoftwareDevelopments.DeleteOnSubmit(softwareDevelopment.FirstOrDefault());
                            db.SubmitChanges();

                            return Request.CreateResponse(HttpStatusCode.OK);
                        }
                        else
                        {
                            return Request.CreateResponse(HttpStatusCode.BadRequest, "Software Development is lock!");
                        }
                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.NotFound, "Software Development not found!");
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
