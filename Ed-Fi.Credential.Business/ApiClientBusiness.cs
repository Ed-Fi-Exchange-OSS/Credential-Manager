using Ed_Fi.Credential.Business.Common;
using Ed_Fi.Credential.Domain.Enums;
using Ed_Fi.Credential.Domain.Model;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Ed_Fi.Credential.Business
{
    public interface IApiClientBusiness : IBaseBusiness<ApiClient>
    {
        ApiClient GetApiClient(int apiClientId);
        IEnumerable<ApiClient> GetApiClients(int edOrgId);
        IEnumerable<ApiClient> GetApiClients(string wamsId);
        IEnumerable<ApiClient> GetApiClientsWithDetails(int edOrgId);
        ApiClient GetApiClientWithDetails(int apiClientId);
        IEnumerable<ApiClient> GetApiClientsByApplication(int applicationId);
        IEnumerable<ApiClient> GetApiClientsByVendor(int vendorId);
        void AddEducationOrganiztion(string wamsId, int educationOrganizationId, int apiClientId);
        void RemoveEducationOrganiztion(string wamsId, int educationOrganizationId, int apiClientId);
        void TryDestroy(string wamsId, int apiClientId);
        bool DoesKeyExist(int apiClientId, string key);
        bool DoesNameExist(int apiClientId, string name);
        IEnumerable<Crosstab> GetCrosstab();
        bool ChangeSecret(string wamsId, int apiClientId, string secret);
    }

    public class ApiClientBusiness : BaseEdFiAdminBusiness<ApiClient>, IApiClientBusiness
    {
      public ApiClientBusiness(IEdFiAdminDbContext context)
            : base(context)
        {
         }

        public ApiClient GetApiClient(int apiClientId)
        {
            return Context.Query<ApiClient>()
                .Include(a => a.Application.Profiles)
                .Include(a=>a.ApplicationEducationOrganizations.Select(e=>e.Application.Vendor))
                .FirstOrDefault(a => a.ApiClientId == apiClientId);
        }

        public IEnumerable<ApiClient> GetApiClients(int edOrgId)
        {
            var apiClients = Context.Query<ApiClient>()
                .Include(a => a.Application.Profiles)
                .Include(a => a.Application.Vendor)
                .Where(a => a.ApplicationEducationOrganizations.Any(ae => ae.EducationOrganizationId == edOrgId)).ToList();

            return apiClients;
        }

        public IEnumerable<ApiClient> GetApiClientsWithDetails(int edOrgId)
        {
            var apiClients = Context.Query<ApiClient>()
                .Include(a => a.Application.Profiles)
                .Include(a => a.Application.Vendor)
                .Include(a => a.Application.ClaimsetDetail.ClaimsetType)
                .Include(a=>a.ApplicationEducationOrganizations.Select(e=>e.EdOrg))
                .Where(a => (a.ApplicationEducationOrganizations.Any(ae => ae.EducationOrganizationId == edOrgId) || a.ApplicationEducationOrganizations.Any(ae=>ae.EdOrg.LocalEducationAgencyId==edOrgId))
                            && a.Application.Vendor.VendorName != "DPI").ToList();

            return apiClients;
        }

        public ApiClient GetApiClientWithDetails(int apiClientId)
        {
            var apiClient = Context.Query<ApiClient>()
                .Include(a => a.Application.Profiles)
                .Include(a => a.Application.Vendor)
                .Include(a => a.Application.ClaimsetDetail.ClaimsetType)
                .Include(a => a.ApplicationEducationOrganizations.Select(e => e.EdOrg))
                .FirstOrDefault(a => a.ApiClientId==apiClientId);

            return apiClient;
        }

        public IEnumerable<ApiClient> GetApiClients(string wamsId)
        {
            var result = Enumerable.Empty<ApiClient>();

            var asmUser = Context.Query<UserAsmUser>()
                .Include(a => a.User)
                .FirstOrDefault(a => a.WamsId == wamsId);

            if (asmUser == null || asmUser.User == null)
                return result;

            var apiClients = Context.Query<ApiClient>()
                .Where(a => a.Application != null && a.Application.VendorVendorId == asmUser.User.VendorVendorId).ToList();

            return apiClients;
        }
        
    

        public IEnumerable<ApiClient> GetApiClientsByApplication(int applicationId)
        {
            var apiClients = Context.Query<ApiClient>().Include(a => a.Application)
                .Where(a => a.ApplicationApplicationId == applicationId);
            return apiClients;
        }

        public IEnumerable<ApiClient> GetApiClientsByVendor(int vendorId)
        {
            var apiClients = Context.Query<ApiClient>()
                .Include(a => a.Application)
                .Include(a=>a.Application.Profiles)
                .Include(a=>a.ApplicationEducationOrganizations.Select(e=>e.EdOrg))
                .Where(a => a.Application.VendorVendorId == vendorId);
            return apiClients;
        }


        public void AddEducationOrganiztion(string wamsId, int educationOrganizationId, int apiClientId)
        {
            var apiClient = Context.Query<ApiClient>()
                .FirstOrDefault(a => (a.ApiClientId == apiClientId));

            if (apiClient == null)
            {
                return;
            }

            if (!apiClient.ApplicationEducationOrganizations.Any(
                    appEdOrg => appEdOrg.EducationOrganizationId == educationOrganizationId))
            {
                apiClient.ApplicationEducationOrganizations.Add(
                    new ApplicationEducationOrganization
                    {
                        Application = apiClient.Application,
                        EducationOrganizationId = educationOrganizationId
                    });

                Context.SaveChanges(wamsId);
            }
        }

        public void RemoveEducationOrganiztion(string wamsId, int educationOrganizationId, int apiClientId)
        {
            var apiClient = Context.Query<ApiClient>()
                .FirstOrDefault(a => a.ApiClientId == apiClientId);

            if (apiClient == null)
            {
                return;
            }

            var applicationEducationOrganization = apiClient.ApplicationEducationOrganizations.FirstOrDefault(
                a => a.EducationOrganizationId == educationOrganizationId);
            if (applicationEducationOrganization != null)
            {
                apiClient.ApplicationEducationOrganizations.Remove(applicationEducationOrganization);
                apiClient.Application.ApplicationEducationOrganizations.Remove(applicationEducationOrganization);
                Context.SaveChanges(wamsId);
            }
        }

        public void TryDestroy(string wamsId, int apiClientId)
        {
            var apiClient = Context.Query<ApiClient>()
                .Include(a => a.ApplicationEducationOrganizations)
                .FirstOrDefault(a => a.ApiClientId == apiClientId);

            if (apiClient == null)
            {
                return;
            }

            var aoes = apiClient.ApplicationEducationOrganizations.ToList();

            foreach (var aeo in aoes)
            {
                apiClient.ApplicationEducationOrganizations.Remove(aeo);
                apiClient.Application.ApplicationEducationOrganizations.Remove(aeo);
            }

            var tokens = Context.Query<ClientAccessToken>().Where(c => c.ApiClientApiClientId == apiClientId);
            Context.RemoveRange(tokens);

            Destroy(wamsId,apiClient);
        }

        public bool DoesKeyExist(int apiClientId, string key)
        {
            var query = Context.Query<ApiClient>();
            if (apiClientId != 0)
                query = query.Where(a => a.ApiClientId != apiClientId);

            var exist =
                query
                    .Any(client => client.Key == key);
            return exist;
        }

        public bool DoesNameExist(int apiClientId, string name)
        {
            var query = Context.Query<ApiClient>();
            if (apiClientId != 0)
                query = query.Where(a => a.ApiClientId != apiClientId);

            var exist =
                query
                    .Any(client => client.Name == name);
            return exist;
        }

        public IEnumerable<Crosstab> GetCrosstab()
        {
            return Context.Query<Crosstab>().AsEnumerable();
        }

        public bool ChangeSecret(string wamsId, int apiClientId, string secret)
        {
            if (string.IsNullOrWhiteSpace(secret) || secret.Length<16)
            {
                return false;
            }

            var apiClient = Context.Query<ApiClient>()
                .FirstOrDefault(a => a.ApiClientId == apiClientId);

            if (apiClient == null)
            {
                return false;
            }

            apiClient.Secret = secret;
            apiClient.SecretIsHashed = false;
            Context.SaveChanges(wamsId);
            return true;
        }


    }
}
