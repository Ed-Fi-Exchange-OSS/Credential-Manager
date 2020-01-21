using Ed_Fi.Credential.Business.Common.Interfaces;
using Ed_Fi.Credential.Domain.Ods;
using System.Collections.Generic;
using System.Linq;

namespace Ed_Fi.Credential.Business
{
    public class OdsBusiness: IOdsBusiness
    {
        private readonly IOdsDbContext _context;

        public OdsBusiness(IOdsDbContext context)
        {
            _context = context;
        }
        
        public Agency GetEducationOrganization(int educationOrganizationId)
        {
            var educationOrganization = _context.Agencies
                .OrderByDescending(e => e.SchoolYear)
                .FirstOrDefault(e => e.EducationOrganizationId == educationOrganizationId);
            return educationOrganization;
        }
        
        public IEnumerable<AgencySchool> GetLeaSchools(int leaId)
        {
            var edOrgs = _context.AgencySchools
                .Where(e => e.LocalEducationAgencyId==leaId)
                .GroupBy(g => g.EducationOrganizationId)
                .Select(g => g.OrderByDescending(y => y.SchoolYear).FirstOrDefault())
                .AsQueryable().AsEnumerable();

            return edOrgs;
        }


        public IEnumerable<AgencySchool> GetSchools(List<int> orgs)
        {
            var edOrgs = _context.AgencySchools
                .Where(e => orgs.Contains(e.EducationOrganizationId))
                .GroupBy(g => g.EducationOrganizationId)
                .Select(g => g.OrderByDescending(y => y.SchoolYear).FirstOrDefault())
                .AsQueryable().AsEnumerable();

            return edOrgs;
        }
        
     }
}
