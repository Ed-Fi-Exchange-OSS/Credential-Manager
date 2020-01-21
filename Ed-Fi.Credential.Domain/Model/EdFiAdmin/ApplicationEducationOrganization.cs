using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ed_Fi.Credential.Domain.Model
{
    public partial class ApplicationEducationOrganization
    {
        public LatestEdOrg EdOrg { get; set; }
    }

    public partial class ApplicationEducationOrganizationConfiguration
    {
        partial void InitializePartial()
        {
            this.HasRequired(s => s.EdOrg).WithMany().HasForeignKey(s => s.EducationOrganizationId);
        }
    }
}
