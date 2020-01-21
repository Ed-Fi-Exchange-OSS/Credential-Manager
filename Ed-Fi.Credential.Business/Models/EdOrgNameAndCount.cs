using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ed_Fi.Credential.Business.Models
{
   public class EdOrgNameAndCount
    {
        public short SchoolYear { get; set; }
        public string Name { get; set; }
        public int EdOrgId { get; set; }
        public bool IsChoice { get; set; }
        public string EdOrgType => IsChoice ? "Choice" : "Public";
        public int Count { get; set; }
    }
}
