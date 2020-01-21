using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Ed_Fi.Credential.Business.Common;
using Ed_Fi.Credential.Domain.Enums;
using Ed_Fi.Credential.Domain.Model;

namespace Ed_Fi.Credential.Business
{
    public interface IApplicationBusiness : IBaseBusiness<Application>
    {
        List<Application> GetApplicationsByVendor(int vendorId);
        Application GetApplicationById(int applicationId);
        Application GetApplication(int vendorId);
        Application GetOrCreateApplication(string wamsId, int vendorId, string claimsetName, int? profileId, string applicationName = null);
      }

    public class ApplicationBusiness : BaseEdFiAdminBusiness<Application>, IApplicationBusiness
    {
        public ApplicationBusiness(IEdFiAdminDbContext context)
            : base(context)
        {
        }

        public List<Application> GetApplicationsByVendor(int vendorId)
        {
            var applications = Context.Query<Application>().Include(a => a.ClaimsetDetail.ClaimsetType)
                .Where(a => a.VendorVendorId == vendorId).ToList();
            return applications;
        }

        public Application GetApplicationById(int applicationId)
        {
            var application =
                Context.Query<Application>().Include(p => p.Profiles).Include(p => p.Vendor)
                    .FirstOrDefault(
                        app =>
                            app.ApplicationId == applicationId);

            return application;
        }

        public Application GetApplication(int vendorId)
        {
            var application = Context.Query<Application>()
                .FirstOrDefault(a => a.VendorVendorId == vendorId);

            return application;
        }

        public Application GetOrCreateApplication(string wamsId, int vendorId, string claimsetName, int? profileId, string applicationName = null)
        {
            var application = Context.Query<Application>()
                .FirstOrDefault(a => a.VendorVendorId == vendorId && a.ClaimSetName == claimsetName );

            if (application == null)
            {
                application = new Application
                {
                    ApplicationName = applicationName ?? ApplicationName.DEFAULT,
                    ClaimSetName = claimsetName,
                    VendorVendorId = vendorId
                };
                if (profileId.GetValueOrDefault() > 0)
                {
                    var profile = Context.Query<Profile>().FirstOrDefault(p => p.ProfileId == profileId);
                    application.Profiles = new List<Profile> { profile };
                }

                Create(wamsId, application);
            }

            return application;
        }
  

       
    }
}
