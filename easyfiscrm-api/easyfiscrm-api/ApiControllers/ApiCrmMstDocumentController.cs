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
    [Authorize, RoutePrefix("api/crm/mst/document")]
    public class ApiCrmMstDocumentController : ApiController
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

        [HttpGet, Route("list/{document}")]
        public List<Entities.CrmMstDocumentEntity> ListDocument(String document)
        {
            var documents = from d in db.CrmMstDocuments
                            where d.DocumentGroup == document
                            select new Entities.CrmMstDocumentEntity
                            {
                                Id = d.Id,
                                DocumentName = d.DocumentName,
                                DocumentType = d.DocumentType,
                                DocumentURL = d.DocumentURL,
                                DocumentGroup = d.DocumentGroup,
                                DateUploaded = d.DateUploaded.ToShortDateString(),
                                Particulars = d.Particulars,
                                CreatedByUserId = d.CreatedByUserId,
                                CreatedByUser = d.MstUser.FullName,
                                CreatedDateTime = d.CreatedDateTime.ToShortDateString(),
                                UpdatedByUserId = d.UpdatedByUserId,
                                UpdatedByUser = d.MstUser1.FullName,
                                UpdatedDateTime = d.UpdatedDateTime.ToShortDateString()
                            };

            return documents.ToList();
        }

        [HttpPost, Route("add/{documentGroup}")]
        public HttpResponseMessage AddDocument(Entities.CrmMstDocumentEntity objDocument, String documentGroup)
        {
            try
            {
                var currentUser = from d in db.MstUsers where d.UserId == User.Identity.GetUserId() select d;

                if (currentUser.Any())
                {
                    if (currentUser.FirstOrDefault().CRMUserGroup == "Admin" || currentUser.FirstOrDefault().CRMUserGroup == "Easyfis Staff")
                    {

                        Data.CrmMstDocument newDocument = new Data.CrmMstDocument
                        {
                            DocumentName = objDocument.DocumentName,
                            DocumentType = objDocument.DocumentType,
                            DocumentURL = objDocument.DocumentURL,
                            DocumentGroup = documentGroup,
                            DateUploaded = DateTime.Today,
                            Particulars = objDocument.Particulars,
                            CreatedByUserId = currentUser.FirstOrDefault().Id,
                            CreatedDateTime = DateTime.Today,
                            UpdatedByUserId = currentUser.FirstOrDefault().Id,
                            UpdatedDateTime = DateTime.Today
                        };

                        db.CrmMstDocuments.InsertOnSubmit(newDocument);
                        db.SubmitChanges();

                        return Request.CreateResponse(HttpStatusCode.OK);
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

        [HttpPut, Route("update/{documentId}")]
        public HttpResponseMessage UpdateDocument(Entities.CrmMstDocumentEntity objDocument, String documentId)
        {
            try
            {
                var currentUser = from d in db.MstUsers where d.UserId == User.Identity.GetUserId() select d;

                if (currentUser.Any())
                {
                    if (currentUser.FirstOrDefault().CRMUserGroup == "Admin" || currentUser.FirstOrDefault().CRMUserGroup == "Easyfis Staff")
                    {
                        var currentDocument = from d in db.CrmMstDocuments
                                              where d.Id == Convert.ToInt32(documentId)
                                              select d;

                        if (currentDocument.Any())
                        {
                            var updateDocument = currentDocument.FirstOrDefault();
                            updateDocument.DocumentName = objDocument.DocumentName;
                            updateDocument.DocumentType = objDocument.DocumentType;
                            updateDocument.DocumentURL = objDocument.DocumentURL;
                            updateDocument.DocumentGroup = objDocument.DocumentGroup;
                            updateDocument.DateUploaded = DateTime.Today;
                            updateDocument.Particulars = objDocument.Particulars;
                            updateDocument.UpdatedByUserId = currentUser.FirstOrDefault().Id;
                            updateDocument.UpdatedDateTime = DateTime.Today;
                            db.SubmitChanges();

                            return Request.CreateResponse(HttpStatusCode.OK);
                        }
                        else
                        {
                            return Request.CreateResponse(HttpStatusCode.NotFound, "Document not found!");
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

        [HttpDelete, Route("delete/{documentId}")]
        public HttpResponseMessage DeleteDocument(String documentId)
        {
            try
            {
                var currentUser = from d in db.MstUsers where d.UserId == User.Identity.GetUserId() select d;

                if (currentUser.Any())
                {
                    if (currentUser.FirstOrDefault().CRMUserGroup == "Admin" || currentUser.FirstOrDefault().CRMUserGroup == "Easyfis Staff")
                    {
                        var currentDocument = from d in db.CrmMstDocuments
                                              where d.Id == Convert.ToInt32(documentId)
                                              select d;

                        if (currentDocument.Any())
                        {
                            db.CrmMstDocuments.DeleteOnSubmit(currentDocument.FirstOrDefault());
                            db.SubmitChanges();

                            return Request.CreateResponse(HttpStatusCode.OK);
                        }
                        else
                        {
                            return Request.CreateResponse(HttpStatusCode.NotFound, "Document not found!");
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

        [HttpPost, Route("uploadFile")]
        public async Task<IHttpActionResult> UploadFile()
        {
            try
            {
                var currentUser = from d in db.MstUsers where d.UserId == User.Identity.GetUserId() select d;

                if (currentUser.Any())
                {
                    if (currentUser.FirstOrDefault().CRMUserGroup == "Admin" || currentUser.FirstOrDefault().CRMUserGroup == "Easyfis Staff")
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
                    else
                    {
                        return BadRequest("Unauthorized Upload.");
                    }
                }
                else
                {
                    return BadRequest("Unauthorized Upload.");
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"An error has occured. Details: {ex.Message}");
            }
        }
    }
}
