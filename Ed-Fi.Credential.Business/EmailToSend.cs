using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ed_Fi.Credential.Business
{
    public class EmailToSend
    {
        public string From { get; set; }
        public IEnumerable<string>To { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
       public string ApplicationName { get; set; }
    }
}
