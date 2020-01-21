using System.Collections.Generic;
using System.Linq;
using Ed_Fi.Credential.Business.Common;
using Ed_Fi.Credential.Domain.Model;

namespace Ed_Fi.Credential.Business
{
    public interface IApplicationEducationOrganizationBusiness : IBaseBusiness<ApplicationEducationOrganization>
    {
        ApplicationEducationOrganization GetOrCreateApplicationEducationOrganization(string wamsId, int educationOrganizationId, int vendorId, int applicationId);
    }

    public class ApplicationEducationOrganizationBusiness : BaseEdFiAdminBusiness<ApplicationEducationOrganization>,
        IApplicationEducationOrganizationBusiness
    {
     
        public ApplicationEducationOrganizationBusiness(IEdFiAdminDbContext context) 
            : base(context)
        {
         }
      
         public ApplicationEducationOrganization GetOrCreateApplicationEducationOrganization(string wamsId, int educationOrganizationId, int vendorId, int applicationId)
        {
            var applicationLocalEducationAgency = Context.Query<ApplicationEducationOrganization>()
                .FirstOrDefault(
                    a =>
                    a.EducationOrganizationId == educationOrganizationId &&
                   (a.Application != null && a.Application.VendorVendorId == vendorId));

            if (applicationLocalEducationAgency == null)
            {
                applicationLocalEducationAgency = new ApplicationEducationOrganization
                {
                    ApplicationApplicationId = applicationId,
                    EducationOrganizationId = educationOrganizationId
                };

                Create(wamsId,applicationLocalEducationAgency);
            }

            return applicationLocalEducationAgency;
        }

    }

}
