using Ed_Fi.Credential.Domain.Ods;
using System;
using System.Linq;

namespace Ed_Fi.Credential.Business
{
    public class ImpersonatableAgency
    {
        public int Year { get; set; }
        public int EducationOrganizationId { get; set; }
        public string AgencyCode { get; set; }
        public string Name { get; set; }
        public string AgencyType { get; set; }
        public string AgencyTypeDescription { get; set; }
        public string City { get; set; }
    }
    
    public interface IAgencyBusiness
    {
        ImpersonatableAgency GetCurrentYearImpersonatableAgencyByKey(int key);
    }

    public class AgencyBusiness : IAgencyBusiness
    {
        private readonly IOdsDbContext _context;

        public AgencyBusiness(IOdsDbContext context)
        {
            _context = context;
        }

        public ImpersonatableAgency GetCurrentYearImpersonatableAgencyByKey(int key)
        {
            var schoolYear = GetMaxYear();
            var agency = GetImpersonatableAgenciesImplementation().FirstOrDefault(a => a.Year == schoolYear && a.EducationOrganizationId == key);
            return agency;
        }
        
        private IQueryable<ImpersonatableAgency> GetImpersonatableAgenciesImplementation()
        {
            var agencies = GetAgencies()
              .Select(x => new ImpersonatableAgency
              {
                  AgencyCode = x.AgencyCode,
                  EducationOrganizationId = x.EducationOrganizationId,
                  AgencyType = x.AgencyType,
                  Name = x.NameOfInstitution,
                  Year = x.SchoolYear,
                  AgencyTypeDescription = x.AgencyTypeDescription,
                  City = x.City
              });

            var schools = _context.AgencySchools
                .Where(x => x.IsChoice)
                .Select(x => new ImpersonatableAgency
                {
                    AgencyCode = x.SchoolCode,
                    EducationOrganizationId = x.EducationOrganizationId,
                    AgencyType = x.SchoolAgencyType,
                    Name = x.NameOfInstitution,
                    Year = x.SchoolYear,
                    AgencyTypeDescription = x.SchoolTypeDescription,
                    City = x.City
                });
            agencies = agencies.Union(schools);

            return agencies;
        }

        private IQueryable<Agency> GetAgencies()
        {
            //This is in the CurrentAgency View
            var agencyTypes1 = new[] { "01", "03", "49", "00", "49M", "61" };
            var agencyTypes2 = new[] { "05" };
            var agencyTypes3 = new[] { "10" };

            var agencyCodes1 = new[] { "7300", "7000", "7100" };
            var agencyCodes2 = new[] { "7302", "7301" };

            var agencies = _context.Agencies
                .Where(x =>
                    (
                        agencyTypes1.Contains(x.AgencyType)
                        || (agencyTypes2.Contains(x.AgencyType) && agencyCodes1.Contains(x.AgencyCode))
                        || (agencyTypes3.Contains(x.AgencyType) && agencyCodes2.Contains(x.AgencyCode))
                    ));

            return agencies;
        }

        private int GetMaxYear()
        {
            var maxyear = GetAgencies().Max(m => m.SchoolYear);
            var schoolYear = DateTime.Now.GetSchoolYear();

            if (schoolYear < maxyear)//if we have loaded the upcoming school year before it starts, keep using the current one
            {
                return schoolYear;
            }
            else { return maxyear; }
        }

    }
}
