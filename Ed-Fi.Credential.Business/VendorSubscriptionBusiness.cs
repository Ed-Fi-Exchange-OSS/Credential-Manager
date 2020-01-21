using Ed_Fi.Credential.Domain.Enums;
using Ed_Fi.Credential.Domain.Model;
using System;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ed_Fi.Credential.Business
{
    public interface IVendorSubscriptionBusiness
    {
        void Unsubscribe(string wamsId, int edOrgId, int apiClientId);
        EmailToSend Subscribe(string wamsId, int edOrgId, string vendorName, string claimSetName, int?[] selectedSchoolIds = null);
        void Modify(string wamsId, int apiClientId, string claimSetName, int?[] selectedSchoolIds = null);
    }

    public class VendorSubscriptionBusiness : IVendorSubscriptionBusiness
    {
        private readonly IEdFiAdminDbContext _adminDbContext;
        private readonly IClaimSetBusiness _claimSetBusiness;
        private readonly IVendorBusiness _vendorBusiness;
        private readonly IApplicationEducationOrganizationBusiness _applicationEducationOrganizationBusiness;
        private readonly IAgencyBusiness _agencyBusiness;
        private readonly IApiClientBusiness _apiClientBusiness;
        private readonly IUserBusiness _userBusiness;
        private readonly IApplicationBusiness _applicationBusiness;

        public VendorSubscriptionBusiness(
            IEdFiAdminDbContext adminDbContext,
            IVendorBusiness vendorBusiness,
            IApplicationEducationOrganizationBusiness applicationLeaBusiness,
            IApiClientBusiness apiClientBusiness,
            IUserBusiness userBusiness,
            IApplicationBusiness applicationBusiness,
            IAgencyBusiness agencyBusiness,
            IClaimSetBusiness claimSetBusiness)

        {
            _adminDbContext = adminDbContext;
            _claimSetBusiness = claimSetBusiness;
            _applicationBusiness = applicationBusiness;
            _agencyBusiness = agencyBusiness;
            _vendorBusiness = vendorBusiness;
            _applicationEducationOrganizationBusiness = applicationLeaBusiness;
            _apiClientBusiness = apiClientBusiness;
            _userBusiness = userBusiness;
        }

        public void Unsubscribe(string wamsId, int edOrgId, int apiClientId)
        {
            var apiClient = _apiClientBusiness.GetApiClient(apiClientId);
            var vendorId = apiClient?.Application?.VendorVendorId;

            if (!vendorId.HasValue)
                return;

            _adminDbContext.Subscriptions.Add(new Subscription
            {
                EducationOrganizationId = edOrgId,
                SubscriptionActionId = (int)SubscriptionActionEnum.Unsubscribe,
                VendorId = vendorId.Value,
                WamsId = wamsId,
                CreatedDate = DateTime.Now
            });
            _adminDbContext.SaveChanges(wamsId);

            _apiClientBusiness.TryDestroy(wamsId, apiClientId);
        }

        public EmailToSend Subscribe(string wamsId, int edOrgId, string vendorName, string claimSetName, int?[] selectedSchoolIds = null)
        {
           var agency = _agencyBusiness.GetCurrentYearImpersonatableAgencyByKey(edOrgId);
            var vendor = _vendorBusiness.GetVendorByName(vendorName);

            var users = _userBusiness.GetUsers(vendor.VendorId).Where(u => !string.IsNullOrEmpty(u.Email)).ToList();
            int? userId = users.Any() ? users.FirstOrDefault()?.UserId : null;

            var applications = _applicationBusiness.GetApplicationsByVendor(vendor.VendorId);

            if (applications.All(a => a.ClaimSetName != claimSetName))
            {
                throw new Exception(string.Format("User can't subscribe to a vendor '{0}' because no application exists for the vendor '{0}' and claim set '{1}'", vendor.VendorName, claimSetName));
            }
            var claimsetDetail = _claimSetBusiness.GetClaimSetDetails(claimSetName);

              if (claimsetDetail.SchoolLevelClaimset && selectedSchoolIds == null)
            {
                throw new Exception("At least one school must be selected.");
            }


            Application application = applications.FirstOrDefault(a => a.ClaimSetName == claimSetName);
                if (application == null)
                {
                    application = _applicationBusiness.GetOrCreateApplication(wamsId, vendor.VendorId,
                          claimSetName, claimsetDetail.ProfileId.GetValueOrDefault(), $"{vendor.VendorName} {claimSetName}");
                }
            

            var key = $"{vendor.VendorName} - {agency.EducationOrganizationId} - {agency.Name}";
            key = key.Substring(0, Math.Min(key.Length, 50));
            var secret = Guid.NewGuid().ToString().Replace("-", "");
            if (_apiClientBusiness.DoesKeyExist(0, key))
            {
                key = key.Substring(0, Math.Min(key.Length, 50)- (claimSetName.Length+4)) + " - " +claimSetName + " ";
                if (_apiClientBusiness.DoesKeyExist(0, key))
                {
                    int i = 0;
                    while (_apiClientBusiness.DoesKeyExist(0, key) && i<10)
                    {
                        key = key.Substring(0, Math.Min(key.Length, 47));
                        key = key + " x" +i.ToString();
                        i++;
                    }

                    if (i == 10)
                    {
                        throw new Exception("Please contact DPI to subscribe to this vendor again.");
                    }
                }
            }

            var apiClient = new ApiClient
            {
                Key = key,
                Secret = secret,
                Name = key,
                IsApproved = true,
                UseSandbox = false,
                SandboxType = 1,
                ApplicationApplicationId = application?.ApplicationId,
                UserUserId = userId
            };

            if (claimsetDetail.SchoolLevelClaimset && selectedSchoolIds != null && selectedSchoolIds.Length > 0)
            {
                var schoolIds = "";
                foreach (var schoolId in selectedSchoolIds)
                {
                    if (!schoolId.HasValue)
                        continue;
                    schoolIds = schoolIds + schoolId.ToString().PadLeft(6, '0') + ",";
                    var schoolApplicationEdOrg =
                        _applicationEducationOrganizationBusiness.GetOrCreateApplicationEducationOrganization(wamsId, schoolId.Value, vendor.VendorId, application?.ApplicationId ?? 0);
                    apiClient.ApplicationEducationOrganizations.Add(schoolApplicationEdOrg);

                }
                key = $"{vendor.VendorName} - {schoolIds}  {agency.Name}";
                key = key.Substring(0, Math.Min(key.Length, 50));
                apiClient.Key = key;
            }
            else
            {
                var applicationEdOrg = _applicationEducationOrganizationBusiness.GetOrCreateApplicationEducationOrganization(wamsId, edOrgId, vendor.VendorId, application?.ApplicationId ?? 0);
                apiClient.ApplicationEducationOrganizations.Add(applicationEdOrg);
            }

            _apiClientBusiness.Create(wamsId, apiClient);

            _adminDbContext.Subscriptions.Add(new Subscription
            {
                EducationOrganizationId = edOrgId,
                SubscriptionActionId = (int)SubscriptionActionEnum.Subscribe,
                VendorId = vendor.VendorId,
                WamsId = wamsId,
                CreatedDate = DateTime.Now
            });
            _adminDbContext.SaveChanges(wamsId);
            
                var edCredUrl = ConfigurationManager.AppSettings["EdCredUrl"];

                var bodySb = new StringBuilder();

                bodySb.Append("<div>" + agency.Name + " has subscribed to your student information system (SIS).  This subscription provides the security credentials needed for WISEdata Ed-Fi integration between their SIS implementation and the Wisconsin DPI Ed-Fi API resources.</div>");
                bodySb.Append("<br/>");
                bodySb.Append("<div>" + "Login to the <a href='" + edCredUrl +
                              "'>Ed-Fi Credential application</a> to view your credentials. Please <a href='http://dpi.wi.gov/wisedata/vendors/contact-us'>contact us</a> with any issues or questions.</div>");
                bodySb.Append("<br/>");
                bodySb.Append("<div>Thanks!</div>");
                bodySb.Append("<br/>");
                bodySb.Append("<div>System Administrator</div>");
                bodySb.Append("<div>Division For Libraries and Technology</div>");
                bodySb.Append("<div>Wisconsin Department of Public Instruction</div>");

                EmailToSend email= new EmailToSend
                {
                    From = "EdFi.Credential.DoNotReply@dpi.wi.gov",
                    To = users.Select(u => u.Email),
                    Subject = "WISEdata Ed-Fi Integration - Agency Credentials Issued",
                    Body = bodySb.ToString(),
                    ApplicationName = "EdFi.Credential"
                };

                return email;
        }

        public void Modify(string wamsId, int apiClientId, string claimSetName, int?[] selectedSchoolIds = null)
        {
            var client = _apiClientBusiness.GetApiClient(apiClientId);
            int applicationId = client.ApplicationApplicationId.GetValueOrDefault();
            int vendorId = client.Application.VendorVendorId.GetValueOrDefault();
            int profileId = client.Application.Profiles.Select(p => p.ProfileId).FirstOrDefault();
            //claimset change will mean choosing/adding new application
            if (client.Application.ClaimSetName != claimSetName)
            {
                Application app = _applicationBusiness.GetOrCreateApplication(wamsId, vendorId,
                        claimSetName, profileId, $"{client.Application.Vendor.VendorName} {claimSetName}");
                
                applicationId = app.ApplicationId;
                //keep applicationids aligned
                client.ApplicationApplicationId = applicationId;
                foreach (var clientApplicationEducationOrganization in client.ApplicationEducationOrganizations.Where(e => e.Application.VendorVendorId == vendorId))
                {
                    clientApplicationEducationOrganization.ApplicationApplicationId =
                        applicationId;
                }
                _apiClientBusiness.Update(wamsId, client);
                _apiClientBusiness.SaveChanges(wamsId);

            }

            //the only other update would be new or fewer schools
            if (selectedSchoolIds != null && selectedSchoolIds.Length > 0)
            {
                bool changes = false;
                //schools not in the current list of edorgs
                foreach (var schoolId in selectedSchoolIds.AsQueryable().Except(client.ApplicationEducationOrganizations.Select(a => (int?)a.EducationOrganizationId)))
                {
                    if (schoolId > 0)
                    {

                        var schoolApplicationEdOrg =
                            _applicationEducationOrganizationBusiness.GetOrCreateApplicationEducationOrganization(wamsId, schoolId.Value, vendorId, applicationId);
                        client.ApplicationEducationOrganizations.Add(schoolApplicationEdOrg);
                        changes = true;
                    }
                }
                //edorgs not in the current list of schools
                IQueryable<int?> currentOrgs = client.ApplicationEducationOrganizations.Select(a => (int?)a.EducationOrganizationId).ToArray().AsQueryable();
                foreach (var schoolId in currentOrgs.Except(selectedSchoolIds))
                {
                    var org = client.ApplicationEducationOrganizations.FirstOrDefault(e =>
                        e.EducationOrganizationId == schoolId);
                    if (org != null)
                    {
                        client.ApplicationEducationOrganizations.Remove(org);
                        changes = true;
                    }
                }
                if (changes)
                {
                    _apiClientBusiness.Update(wamsId, client);
                    _apiClientBusiness.SaveChanges(wamsId);

                }
            }

        }


    }
}
