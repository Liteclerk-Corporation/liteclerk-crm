using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace easyfiscrm_api.ApiControllers
{
    [Authorize, RoutePrefix("api/crm/mst/user/form")]
    public class ApiCrmMstUserFormController : ApiController
    {
        public Data.easyfiscrmdbDataContext db = new Data.easyfiscrmdbDataContext();

        [HttpGet, Route("list/sysForm")]
        public List<Entities.SysFormEntity> listSysForm()
        {
            var sysForms = from d in db.SysForms
                           where d.FormName.Contains("CRM")
                           select new Entities.SysFormEntity
                           {
                               Id = d.Id,
                               FormName = d.FormName,
                               Particulars = d.Particulars
                           };

            return sysForms.ToList();
        }

        [HttpGet, Route("list/UserForm/{userId}")]
        public List<Entities.MstUserFormEntity> listUserForm(String userId)
        {
            var userForms = from d in db.MstUserForms
                            where d.UserId == Convert.ToInt32(userId)
                            && d.SysForm.FormName.Contains("CRM")
                            select new Entities.MstUserFormEntity
                            {
                                Id = d.Id,
                                UserId = d.UserId,
                                FormId = d.FormId,
                                Form = d.SysForm.FormName,
                                CanAdd = d.CanAdd,
                                CanEdit = d.CanEdit,
                                CanDelete = d.CanDelete,
                                CanLock = d.CanLock,
                                CanUnlock = d.CanUnlock,
                                CanCancel = d.CanCancel,
                                CanPrint = d.CanPrint
                            };

            return userForms.ToList();
        }

        [HttpGet, Route("api/listUserFormByUserId/{UserId}")]
        public List<Entities.MstUserFormEntity> listUserFormByUserId(String UserId)
        {
            var userForms = from d in db.MstUserForms
                            where d.UserId == Convert.ToInt32(UserId)
                            select new Entities.MstUserFormEntity
                            {
                                Id = d.Id,
                                UserId = d.UserId,
                                FormId = d.FormId,
                                Form = d.SysForm.FormName,
                                CanAdd = d.CanAdd,
                                CanEdit = d.CanEdit,
                                CanDelete = d.CanDelete,
                                CanLock = d.CanLock,
                                CanUnlock = d.CanUnlock,
                                CanCancel = d.CanCancel,
                                CanPrint = d.CanPrint
                            };

            return userForms.ToList();
        }

        [HttpGet, Route("list/UserFormByUserName/{userName}")]
        public List<Entities.MstUserFormEntity> listUserFormByUserName(String userName)
        {
            var userForms = from d in db.MstUserForms
                            where d.MstUser.UserName == userName
                            && d.SysForm.FormName.Contains("CRM")
                            select new Entities.MstUserFormEntity
                            {
                                Id = d.Id,
                                UserId = d.UserId,
                                FormId = d.FormId,
                                Form = d.SysForm.FormName,
                                CanAdd = d.CanAdd,
                                CanEdit = d.CanEdit,
                                CanDelete = d.CanDelete,
                                CanLock = d.CanLock,
                                CanUnlock = d.CanUnlock,
                                CanCancel = d.CanCancel,
                                CanPrint = d.CanPrint
                            };

            return userForms.ToList();
        }

        [HttpPost, Route("add")]
        public HttpResponseMessage AddUserForm(Entities.MstUserFormEntity objUserForm)
        {
            try
            {
                var user = from d in db.MstUsers
                           where d.Id == objUserForm.UserId
                           select d;
                if (user.Any())
                {
                    var userForm = from d in db.MstUserForms
                                   where d.FormId == Convert.ToInt32(objUserForm.FormId)
                                   && d.UserId == Convert.ToInt32(objUserForm.UserId)
                                   select d;

                    if (userForm.Any())
                    {
                        return Request.CreateResponse(HttpStatusCode.Conflict, "User form already exist!");
                    }
                    else
                    {
                        Data.MstUserForm newUserForm = new Data.MstUserForm()
                        {
                            UserId = objUserForm.UserId,
                            FormId = objUserForm.FormId,
                            CanAdd = objUserForm.CanAdd,
                            CanEdit = objUserForm.CanEdit,
                            CanDelete = objUserForm.CanDelete,
                            CanLock = objUserForm.CanLock,
                            CanUnlock = objUserForm.CanUnlock,
                            CanCancel = objUserForm.CanCancel,
                            CanPrint = objUserForm.CanPrint
                        };

                        db.MstUserForms.InsertOnSubmit(newUserForm);
                        db.SubmitChanges();

                        return Request.CreateResponse(HttpStatusCode.OK);
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

        [HttpPut, Route("update")]
        public HttpResponseMessage UpdateUserForm(Entities.MstUserFormEntity objUserForm)
        {
            try
            {
                var currentUser = from d in db.MstUsers
                                  where d.UserId == User.Identity.GetUserId()
                                  select d;

                if (currentUser.FirstOrDefault().UserName.Equals("admin"))
                {
                    var currentUserForm = from d in db.MstUserForms
                                          where d.Id == objUserForm.Id
                                          select d;
                    if (currentUserForm.Any())
                    {
                        var updateUserForm = currentUserForm.FirstOrDefault();
                        updateUserForm.FormId = objUserForm.FormId;
                        updateUserForm.CanAdd = objUserForm.CanAdd;
                        updateUserForm.CanEdit = objUserForm.CanEdit;
                        updateUserForm.CanDelete = objUserForm.CanDelete;
                        updateUserForm.CanLock = objUserForm.CanLock;
                        updateUserForm.CanUnlock = objUserForm.CanUnlock;
                        updateUserForm.CanCancel = objUserForm.CanCancel;
                        updateUserForm.CanPrint = objUserForm.CanPrint;
                        db.SubmitChanges();

                        return Request.CreateResponse(HttpStatusCode.OK);
                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.NotFound, "User not found!");
                    }
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.Forbidden, "User unauthorized!");
                }
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpDelete, Route("delete/{formId}")]
        public HttpResponseMessage DeleteUserForm(String formId)
        {
            try
            {
                var currentUserForm = from d in db.MstUserForms
                                      where d.Id == Convert.ToInt32(formId)
                                      select d;
                if (currentUserForm.Any())
                {
                    db.MstUserForms.DeleteOnSubmit(currentUserForm.FirstOrDefault());
                    db.SubmitChanges();

                    return Request.CreateResponse(HttpStatusCode.OK);
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "User Form not found!");
                }
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
    }
}
