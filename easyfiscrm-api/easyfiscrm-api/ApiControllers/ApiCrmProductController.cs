using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace easyfiscrm_api.ApiControllers
{
    [Authorize, RoutePrefix("api/crm/product")]
    public class ApiCrmProductController : ApiController
    {
        public Data.easyfiscrmdbDataContext db = new Data.easyfiscrmdbDataContext();

        [HttpGet, Route("list")]
        public List<Entities.CrmMstProductEntity> ProductList()
        {
            var products = from d in db.CrmMstProducts
                           select new Entities.CrmMstProductEntity
                           {
                               Id = d.Id,
                               ProductCode = d.ProductCode,
                               ProductDescription = d.ProductDescription,
                               CreatedByUserId = d.CreatedByUserId,
                               CreatedByUser = d.MstUser.FullName,
                               CreatedDateTime = d.CreatedDateTime.ToShortDateString(),
                               UpdatedByUserId = d.UpdatedByUserId,
                               UpdatedByUser = d.MstUser1.FullName,
                               UpdatedDateTime = d.UpdatedDateTime.ToShortDateString()
                           };

            return products.OrderByDescending(d => d.Id).ToList();
        }

        [HttpGet, Route("detail/{id}")]
        public Entities.CrmMstProductEntity DetailProduct(String id)
        {
            var products = from d in db.CrmMstProducts
                           where d.Id == Convert.ToInt32(id)
                           select new Entities.CrmMstProductEntity
                           {
                               Id = d.Id,
                               ProductCode = d.ProductCode,
                               ProductDescription = d.ProductDescription,
                               CreatedByUserId = d.CreatedByUserId,
                               CreatedByUser = d.MstUser.FullName,
                               CreatedDateTime = d.CreatedDateTime.ToShortDateString(),
                               UpdatedByUserId = d.UpdatedByUserId,
                               UpdatedByUser = d.MstUser1.FullName,
                               UpdatedDateTime = d.UpdatedDateTime.ToShortDateString()
                           };

            return products.FirstOrDefault();
        }

        [HttpPost, Route("add")]
        public HttpResponseMessage AddProduct()
        {
            try
            {
                var currentUser = from d in db.MstUsers where d.UserId == User.Identity.GetUserId() select d;
                if (currentUser.Any())
                {
                    Int32 currentUserId = currentUser.FirstOrDefault().Id;

                    Data.CrmMstProduct newProduct = new Data.CrmMstProduct
                    {
                        ProductCode = "NA",
                        ProductDescription = "NA",
                        CreatedByUserId = currentUser.FirstOrDefault().Id,
                        CreatedDateTime = DateTime.Today,
                        UpdatedByUserId = currentUser.FirstOrDefault().Id,
                        UpdatedDateTime = DateTime.Today
                    };

                    db.CrmMstProducts.InsertOnSubmit(newProduct);
                    db.SubmitChanges();

                    return Request.CreateResponse(HttpStatusCode.OK, newProduct.Id);
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "User not found");
                }
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpPut, Route("update/{id}")]
        public HttpResponseMessage UpdateProduct(Entities.CrmMstProductEntity objProduct, String id)
        {
            try
            {
                var currentUser = from d in db.MstUsers where d.UserId == User.Identity.GetUserId() select d;
                Int32 currentUserId = currentUser.FirstOrDefault().Id;

                var product = from d in db.CrmMstProducts
                              where d.Id == Convert.ToInt32(id)
                              select d;

                if (product.Any())
                {
                    var updateProduct = product.FirstOrDefault();
                    updateProduct.ProductCode = objProduct.ProductCode;
                    updateProduct.ProductDescription = objProduct.ProductDescription;
                    updateProduct.UpdatedByUserId = currentUser.FirstOrDefault().Id;
                    updateProduct.UpdatedDateTime = DateTime.Today;
                    db.SubmitChanges();

                    return Request.CreateResponse(HttpStatusCode.OK);
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "Product not found.");
                }
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpDelete, Route("delete/{id}")]
        public HttpResponseMessage DeleteProduct(String id)
        {
            try
            {
                var product = from d in db.CrmMstProducts
                              where d.Id == Convert.ToInt32(id)
                              select d;

                if (product.Any())
                {
                    db.CrmMstProducts.DeleteOnSubmit(product.FirstOrDefault());
                    db.SubmitChanges();

                    return Request.CreateResponse(HttpStatusCode.OK);
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "Product not found.");
                }
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
    }
}
