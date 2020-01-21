using Ed_Fi.Credential.Domain.Ods;
using System.Collections.Generic;

namespace Ed_Fi.Credential.Business.Common.Interfaces
{
   

    public interface IOdsBusiness
    {
        Agency GetEducationOrganization(int educationOrganizationId);
        IEnumerable<AgencySchool> GetLeaSchools(int leaId);
        IEnumerable<AgencySchool> GetSchools(List<int> orgs);


    }
}
