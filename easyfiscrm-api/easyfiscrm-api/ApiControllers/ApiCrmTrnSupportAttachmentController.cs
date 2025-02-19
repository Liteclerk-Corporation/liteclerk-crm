using easyfiscrm_api.Azure.AzureStorage;
using Microsoft.AspNet.Identity;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace easyfiscrm_api.ApiControllers
{
    [Authorize, RoutePrefix("api/crm/trn/support/attachment")]
    public class ApiCrmTrnSupportAttachmentController : ApiController
    {
        public Data.easyfiscrmdbDataContext db = new Data.easyfiscrmdbDataContext();

        [HttpGet, Route("list/doctype")]
        public List<Entities.CrmDocumentTypeEntity> listGroup()
        {
            List<Entities.CrmDocumentTypeEntity> documentTypeList = new List<Entities.CrmDocumentTypeEntity>();
            documentTypeList.Add(new Entities.CrmDocumentTypeEntity { DocumentType = "JPEG" });
            documentTypeList.Add(new Entities.CrmDocumentTypeEntity { DocumentType = "PNG" });
            documentTypeList.Add(new Entities.CrmDocumentTypeEntity { DocumentType = "DOC" });
            documentTypeList.Add(new Entities.CrmDocumentTypeEntity { DocumentType = "XLS" });
            documentTypeList.Add(new Entities.CrmDocumentTypeEntity { DocumentType = "YOUTUBE" });
            documentTypeList.Add(new Entities.CrmDocumentTypeEntity { DocumentType = "PPT" });
            documentTypeList.Add(new Entities.CrmDocumentTypeEntity { DocumentType = "PDF" });

            return documentTypeList.OrderBy(d => d.DocumentType).ToList();
        }

        [HttpGet, Route("list/{supportId}")]
        public List<Entities.CrmTrnSupportAttachmentEntity> ListDocument(String supportId)
        {
            var attachments = from d in db.CrmTrnSupportAttachments
                              where d.SPId == Convert.ToInt32(supportId)
                              select new Entities.CrmTrnSupportAttachmentEntity
                              {
                                  Id = d.Id,
                                  SPId = d.SPId,
                                  Attachment = d.Attachment,
                                  AttachmentURL = d.AttachmentURL,
                                  AttachmentType = d.AttachmentType,
                                  Particulars = d.Particulars
                              };

            return attachments.ToList();
        }

        [HttpPost, Route("add")]
        public HttpResponseMessage AddDocument(Entities.CrmTrnSupportAttachmentEntity objAttachment)
        {
            try
            {
                var currentUser = from d in db.MstUsers where d.UserId == User.Identity.GetUserId() select d;

                if (currentUser.Any())
                {
                    var support = from d in db.CrmTrnSupports
                                  where d.Id == objAttachment.SPId
                                  select d;

                    if (support.Any())
                    {
                        Data.CrmTrnSupportAttachment newAttachment = new Data.CrmTrnSupportAttachment
                        {
                            SPId = support.FirstOrDefault().Id,
                            Attachment = objAttachment.Attachment,
                            AttachmentURL = objAttachment.AttachmentURL,
                            AttachmentType = objAttachment.AttachmentType,
                            Particulars = objAttachment.Particulars,
                        };

                        db.CrmTrnSupportAttachments.InsertOnSubmit(newAttachment);
                        db.SubmitChanges();

                        return Request.CreateResponse(HttpStatusCode.OK);
                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.NotFound, "Support not found!");
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

        [HttpPut, Route("update/{attachmentId}")]
        public HttpResponseMessage UpdateDocument(Entities.CrmTrnSupportAttachmentEntity objAttachment, String attachmentId)
        {
            try
            {
                var currentUser = from d in db.MstUsers where d.UserId == User.Identity.GetUserId() select d;

                if (currentUser.Any())
                {
                    var currentAttachement = from d in db.CrmTrnSupportAttachments
                                             where d.Id == Convert.ToInt32(attachmentId)
                                             select d;

                    if (currentAttachement.Any())
                    {
                        var updateAttachment = currentAttachement.FirstOrDefault();
                        updateAttachment.Attachment = objAttachment.Attachment;
                        updateAttachment.AttachmentURL = objAttachment.AttachmentURL;
                        updateAttachment.AttachmentType = objAttachment.AttachmentType;
                        updateAttachment.Particulars = objAttachment.Particulars;
                        db.SubmitChanges();

                        return Request.CreateResponse(HttpStatusCode.OK);
                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.NotFound, "Attachment not found!");
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

        [HttpDelete, Route("delete/{attachmentId}")]
        public HttpResponseMessage DeleteDocument(String attachmentId)
        {
            try
            {
                var currentUser = from d in db.MstUsers where d.UserId == User.Identity.GetUserId() select d;

                if (currentUser.Any())
                {

                    var currentAttachement = from d in db.CrmTrnSupportAttachments
                                             where d.Id == Convert.ToInt32(attachmentId)
                                             select d;

                    if (currentAttachement.Any())
                    {
                        db.CrmTrnSupportAttachments.DeleteOnSubmit(currentAttachement.FirstOrDefault());
                        db.SubmitChanges();

                        return Request.CreateResponse(HttpStatusCode.OK);
                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.NotFound, "Attachment not found!");
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

        [HttpPost, Route("uploadFile")]
        public async Task<IHttpActionResult> UploadFile()
        {
            try
            {
                if (!Request.Content.IsMimeMultipartContent("form-data"))
                {
                    throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
                }

                String cloudStorageConnectionString = ConfigurationManager.AppSettings["CloudStorageConnectionString"];
                CloudStorageAccount cloudStorageAccount = CloudStorageAccount.Parse(cloudStorageConnectionString);

                String cloudStorageContainerName = ConfigurationManager.AppSettings["CloudStorageContainerName"];
                CloudBlobClient cloudBlobClient = cloudStorageAccount.CreateCloudBlobClient();
                CloudBlobContainer cloudBlobContainer = cloudBlobClient.GetContainerReference(cloudStorageContainerName);

                AzureStorageMultipartFormDataStreamProvider provider = new AzureStorageMultipartFormDataStreamProvider(cloudBlobContainer);
                await Request.Content.ReadAsMultipartAsync(provider);

                String fileName = provider.FileData.FirstOrDefault()?.LocalFileName;
                if (String.IsNullOrEmpty(fileName))
                {
                    return BadRequest("An error has occured while uploading your file. Please try again.");
                }

                String fileURI = Azure.BlobStorage.BlobContainer.GetCloudBlockBlobImageURI(fileName);

                return Ok(fileURI);
            }
            catch (Exception ex)
            {
                return BadRequest($"An error has occured. Details: {ex.Message}");
            }
        }
    }
}
