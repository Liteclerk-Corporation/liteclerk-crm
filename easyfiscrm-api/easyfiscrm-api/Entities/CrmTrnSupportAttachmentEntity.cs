using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace easyfiscrm_api.Entities
{
    public class CrmTrnSupportAttachmentEntity
    {
        public Int32 Id { get; set; }
        public Int32 SPId { get; set; }
        public String Attachment { get; set; }
        public String AttachmentURL { get; set; }
        public String AttachmentType { get; set; }
        public String Particulars { get; set; }
    }
}