using Ed_Fi.Credential.Domain.Model;
using System.Data.Entity;
using System.Linq;
using Ed_Fi.Credential.Business.Common;
using Ed_Fi.Credential.Business.Common.Extensions;

namespace Ed_Fi.Credential.Business
{
    public interface IVendorBusiness : IBaseBusiness<Vendor>
    {
        Vendor GetVendorByName(string vendorName);
        Vendor GetVendorById(int vendorId);
        Vendor GetOrCreateVendorByPrefix(string wamsId, string prefix);
        Vendor GetFullVendorById(int vendorId);
         void Remove(Vendor vendor);
        void DeletePrefix(int id);
        void DeleteDomain(int id);

        IQueryable<Vendor> GetVendors();
        IQueryable<Vendor> GetVendorsWithUsers();
         IQueryable<VendorEmailDomain> GetDomains(int vendorId);
        bool EmailDomainExists(string emailDomain);
        Vendor GetVendorIdByDomain(string domain);
    }

    public class VendorBusiness : BaseEdFiAdminBusiness<Vendor>, IVendorBusiness
    {
         public VendorBusiness(IEdFiAdminDbContext context)
            : base(context)
        {
           
        }

     
        public IQueryable<Vendor> GetVendors()
        {
            return Context.Query<Vendor>().Include(v=>v.Users).Include(v => v.Applications.Select(c => c.ClaimsetDetail));
        }

        public IQueryable<Vendor> GetVendorsWithUsers()
        {
            return Context.Query<Vendor>()
                .Include(v => v.VendorNamespacePrefixes)
                .Include(v => v.VendorEmailDomains)
                .Include(v => v.Users);
        }

    

        public Vendor GetVendorById(int vendorId)
        {
            var vendor =
                Context.Query<Vendor>()
                    .Include(p => p.Applications)
                    .Include(p => p.VendorNamespacePrefixes)
                    .FirstOrDefault(
                        v =>
                            v.VendorId == vendorId);

            return vendor;
        }

        public Vendor GetVendorByName(string vendorName)
        {
            var vendor =
                Context.Query<Vendor>()
                    .FirstOrDefault(
                        v =>
                            v.VendorName == vendorName);

            return vendor;
        }

        public Vendor GetVendorByPrefix(string prefix)
        {
            var vendor =
                Context.Query<Vendor>()
                    .FirstOrDefault(
                        v =>
                            v.VendorNamespacePrefixes.Any(p=>p.NamespacePrefix == prefix));

            return vendor;
        }

        public Vendor GetOrCreateVendorByPrefix(string wamsId, string prefix)
        {
            var vendor = GetVendorByPrefix(prefix);

            if (vendor != null)
                return vendor;

            var prefixParts = prefix.Split('.');
            var vendorName = prefixParts[0].FirstCharToUpper();

            vendor = new Vendor
            {
                VendorName = vendorName
            };

            Create(wamsId, vendor);

            return vendor;
        }

        public Vendor GetFullVendorById(int vendorId)
        {
            var vendor =
                Context.Query<Vendor>()
                    .Include(p => p.VendorNamespacePrefixes)
                    .Include(p => p.VendorEmailDomains)
                    .Include(p => p.Users)
                    .Include(p => p.Applications.Select(a => a.ClaimsetDetail))
                    .Include(p => p.Applications.Select(a => a.Profiles))
                    .FirstOrDefault(
                        v =>
                            v.VendorId == vendorId);

            return vendor;
        }

        public void Remove(Vendor vendor)
        {
            var application = vendor.Applications.FirstOrDefault();

            if (application != null)
            {
                Context.RemoveRange(application.ApplicationEducationOrganizations);
                Context.RemoveEntity(application);
            }
            Context.RemoveEntity(vendor);
        }


        public void DeletePrefix(int id)
        {
            var prefix = Context.Query<VendorNamespacePrefix>().FirstOrDefault(p=>p.VendorNamespacePrefixId==id);
                       
            Context.RemoveEntity(prefix);
        }
        public void DeleteDomain(int id)
        {
            var prefix = Context.Query<VendorEmailDomain>().FirstOrDefault(p => p.VendorEmailDomainId == id);

            Context.RemoveEntity(prefix);
        }

        public IQueryable<VendorEmailDomain> GetDomains(int vendorId)
        {
            return Context.Query<VendorEmailDomain>().Where(v => v.VendorVendorId == vendorId);
        }

        public bool EmailDomainExists(string emailDomain)
        {
            return Context.Query<VendorEmailDomain>().Any(v => v.EmailDomain==emailDomain);
        }

        public Vendor GetVendorIdByDomain(string domain)
        {
            Vendor ret = null;
            if (Context.Query<VendorEmailDomain>().Count(v => v.EmailDomain == domain) == 1)
            {
                ret = Context.Query<VendorEmailDomain>().First(v => v.EmailDomain == domain).Vendor;
            }
            return ret;
            
        }
    }
}
