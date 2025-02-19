using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace easyfiscrm_api.Entities
{
    public class CrmMstDocumentEntity
    {
        public Int32 Id { get; set; }
        public String DocumentName { get; set; }
        public String DocumentType { get; set; }
        public String DocumentURL { get; set; }
        public String DocumentGroup { get; set; }
        public String DateUploaded { get; set; }
        public String Particulars { get; set; }
        public Int32 CreatedByUserId { get; set; }
        public String CreatedByUser { get; set; }
        public String CreatedDateTime { get; set; }
        public Int32 UpdatedByUserId { get; set; }
        public String UpdatedByUser { get; set; }
        public String UpdatedDateTime { get; set; }
    }
}