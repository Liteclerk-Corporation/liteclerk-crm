using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace easyfiscrm_api.ApiControllers
{
    [RoutePrefix("api/crm/lead/marketing")]
    public class ApiCrmLeadMarketingController : ApiController
    {
        public Data.easyfiscrmdbDataContext db = new Data.easyfiscrmdbDataContext();

        public String FillLeadingZeroes(Int32 number, Int32 length)
        {
            String result = number.ToString();
            Int32 pad = length - result.Length;
            while (pad > 0) { result = '0' + result; pad--; }

            return result;
        }

        [HttpPost, Route("add/{productFilter}")]
        public HttpResponseMessage AddLead(Entities.CrmTrnLeadEntity objLead, String productFilter)
        {
            try
            {
                var currentUser = from d in db.MstUsers where d.UserName.ToLower() == "admin" select d;

                String LDNumber = "0000000001";

                var lastLead = from d in db.CrmTrnLeads.OrderByDescending(d => d.Id) select d;
                if (lastLead.Any())
                {
                    Int32 newLDNumber = Convert.ToInt32(lastLead.FirstOrDefault().LDNumber) + 0000000001;
                    LDNumber = FillLeadingZeroes(newLDNumber, 10);
                }

                var product = from d in db.CrmMstProducts where d.ProductCode.ToLower().Contains(productFilter.ToLower()) select d;
                Int32 productId = product.FirstOrDefault().Id;

                var status = from d in db.MstStatus
                             where d.Category.Equals("LD")
                             && d.Status.Contains("1")
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
                    Name = objLead.Name,
                    ProductId = productId,
                    Address = objLead.Address,
                    ContactPerson = objLead.ContactPerson,
                    ContactPosition = objLead.ContactPosition,
                    ContactEmail = objLead.ContactEmail,
                    ContactPhoneNumber = objLead.ContactPhoneNumber,
                    ReferredBy = objLead.ReferredBy,
                    Remarks = objLead.Remarks,
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

                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
    }
}
