using System;

namespace Ed_Fi.Credential.Business.Models
{
   public class ApiKeyErrorCountAndDate
    {

        public int ApiClientId { get; set; }
        public short SchoolYear { get; set; }
        public string ApiKey { get; set; }

        public int L1RuleId { get; set; }
        public string AggregateName { get; set; } 
        public string HttpMethod { get; set; } 
        public string Message { get; set; } 
        public DateTime LastModified { get; set; }
        public int Count { get; set; }
    }
}
