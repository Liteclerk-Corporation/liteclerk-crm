using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace easyfiscrm_api.ApiControllers
{
    [Authorize, RoutePrefix("api/crm/mst/sys/form")]
    public class ApiSysFormController : ApiController
    {
        public Data.easyfiscrmdbDataContext db = new Data.easyfiscrmdbDataContext();

        [HttpGet, Route("api/listForm")]
        public List<Entities.SysFormEntity> listForm()
        {
            var forms = from d in db.SysForms.OrderBy(d => d.Particulars)
                        select new Entities.SysFormEntity
                        {
                            Id = d.Id,
                            FormName = d.FormName,
                            Particulars = d.Particulars
                        };

            return forms.ToList();
        }
    }
}
