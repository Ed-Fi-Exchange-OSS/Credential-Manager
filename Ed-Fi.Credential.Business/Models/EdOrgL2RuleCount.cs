using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ed_Fi.Credential.Business.Models
{
    public class EdOrgL2RuleCount
    {
        public short SchoolYear { get; set; }
        public int EdOrgId { get; set; }

        public int RuleCode { get; set; }
        public string KbaUrl { get; set; }
        public string ShortDescription { get; set; }
        public string ValidationCategories { get; set; }

        public int Count { get; set; }

       
    }
}
