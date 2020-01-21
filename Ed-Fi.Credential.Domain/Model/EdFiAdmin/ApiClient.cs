using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ed_Fi.Credential.Domain.Model
{
    public partial class ApiClient
    {
        public string SecretDisplay { get { return SecretIsHashed ? SecretDisplayConstant.SecretDisplayHashed : Secret; } }
    }
}
