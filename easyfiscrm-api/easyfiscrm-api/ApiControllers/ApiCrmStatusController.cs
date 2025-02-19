using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace easyfiscrm_api.ApiControllers
{
    [Authorize, RoutePrefix("api/crm/mst/status")]
    public class ApiCrmStatusController : ApiController
    {
        public Data.easyfiscrmdbDataContext db = new Data.easyfiscrmdbDataContext();

        [HttpGet, Route("category/list")]
        public List<Entities.CategoryEntity> ListCategory()
        {
            List<Entities.CategoryEntity> categories = new List<Entities.CategoryEntity>();
            categories.Add(new Entities.CategoryEntity { Id = 2, Code = "AC", Name = "ACTIVITY" });
            categories.Add(new Entities.CategoryEntity { Id = 2, Code = "LD",Name = "LEAD" });
            categories.Add(new Entities.CategoryEntity { Id = 3, Code = "SD", Name = "SALES DELIVERY" });
            categories.Add(new Entities.CategoryEntity { Id = 4, Code = "SP", Name = "SUPPORT" });
            categories.Add(new Entities.CategoryEntity { Id = 4, Code = "SWD", Name = "Software Development" });
            return categories.ToList();
        }

        [HttpGet, Route("list")]
        public List<Entities.CrmMstStatusEntity> ListStatus()
        {
            var statuses = from d in db.MstStatus
                           select new Entities.CrmMstStatusEntity
                           {
                               Id = d.Id,
                               Status = d.Status,
                               Category = d.Category,
                               IsLocked = d.IsLocked,
                               CreatedById = d.CreatedById,
                               CreatedDateTime = d.CreatedDateTime.ToShortDateString(),
                               UpdatedById = d.UpdatedById,
                               UpdatedDateTime = d.UpdatedDateTime.ToShortDateString()
                           };

            return statuses.OrderByDescending(d => d.Id).ToList();
        }

        [HttpGet, Route("detail/{id}")]
        public Entities.CrmMstStatusEntity DetailStatus(String id)
        {
            var status = from d in db.MstStatus
                         where d.Id == Convert.ToInt32(id)
                         select new Entities.CrmMstStatusEntity
                         {
                             Id = d.Id,
                             Status = d.Status,
                             Category = d.Category,
                             IsLocked = d.IsLocked,
                             CreatedById = d.CreatedById,
                             CreatedDateTime = d.CreatedDateTime.ToShortDateString(),
                             UpdatedById = d.UpdatedById,
                             UpdatedDateTime = d.UpdatedDateTime.ToShortDateString()
                         };

            return status.FirstOrDefault();
        }

        [HttpPost, Route("add")]
        public HttpResponseMessage AddStatus(Entities.CrmMstStatusEntity objStatus)
        {
            try
            {
                var currentUser = from d in db.MstUsers where d.UserId == User.Identity.GetUserId() select d;
                Int32 currentUserId = currentUser.FirstOrDefault().Id;

                Data.MstStatus newStatus = new Data.MstStatus
                {
                    Status = objStatus.Status,
                    Category = objStatus.Category,
                    IsLocked = true,
                    CreatedById = currentUserId,
                    CreatedDateTime = DateTime.Today,
                    UpdatedById = currentUserId,
                    UpdatedDateTime = DateTime.Today
                };

                db.MstStatus.InsertOnSubmit(newStatus);
                db.SubmitChanges();

                return Request.CreateResponse(HttpStatusCode.OK, newStatus.Id);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpPut, Route("update")]
        public HttpResponseMessage UpdateStatus(Entities.CrmMstStatusEntity objStatus)
        {
            try
            {
                var currentUser = from d in db.MstUsers where d.UserId == User.Identity.GetUserId() select d;
                Int32 currentUserId = currentUser.FirstOrDefault().Id;

                var status = from d in db.MstStatus
                              where d.Id == Convert.ToInt32(objStatus.Id)
                              select d;

                if (status.Any())
                {

                    var updateStatus = status.FirstOrDefault();
                    updateStatus.Status = objStatus.Status;
                    updateStatus.Category = objStatus.Category;
                    updateStatus.UpdatedById = currentUser.FirstOrDefault().Id;
                    updateStatus.UpdatedDateTime = DateTime.Today;
                    db.SubmitChanges();

                    return Request.CreateResponse(HttpStatusCode.OK);
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "Status not found.");
                }
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpDelete, Route("delete/{id}")]
        public HttpResponseMessage DeleteStatus(String id)
        {
            try
            {
                var status = from d in db.MstStatus
                             where d.Id == Convert.ToInt32(id)
                             select d;

                if (status.Any())
                {
                    db.MstStatus.DeleteOnSubmit(status.FirstOrDefault());
                    db.SubmitChanges();

                    return Request.CreateResponse(HttpStatusCode.OK);
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "Status not found.");
                }
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
    }
}
