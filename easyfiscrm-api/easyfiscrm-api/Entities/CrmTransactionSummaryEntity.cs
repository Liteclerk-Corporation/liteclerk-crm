using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace easyfiscrm_api.Entities
{
    public class CrmTransactionSummaryEntity
    {
        public Int32 Id { get; set; }
        public String Document { get; set; }
        public String Status { get; set; }
        public Int32 NoOfTransaction { get; set; }
    }
}