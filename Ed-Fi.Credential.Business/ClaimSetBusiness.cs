using Ed_Fi.Credential.Domain.Model;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Ed_Fi.Credential.Business
{
    public interface IClaimSetBusiness : IBaseBusiness<ClaimSet>
    {
        IEnumerable<ClaimsetDetail> GetClaimSetsWithDetails();
        IEnumerable<ClaimsetDetail> GetClaimSetDetailsWithProfiles();
         ClaimsetDetail GetClaimSetDetails(string claimSetName);
        IEnumerable<SecurityStrategy> GetSecurityStrategies();
        IEnumerable<AllowedActionsByClaimset> GetAllowedActionsByClaimset(string claimsetName);

    }

    public class ClaimSetBusiness : BaseEdFiSecurityBusiness<ClaimSet>, IClaimSetBusiness
    {
        public IEdFiAdminDbContext DetailContext { get; private set; }


        public ClaimSetBusiness(IEdFiSecurityDbContext context, IEdFiAdminDbContext detailContext)
            : base(context)
        {
            DetailContext = detailContext;
        }

        public IEnumerable<ClaimsetDetail> GetClaimSetsWithDetails()
        {
            var details = DetailContext.Query<ClaimsetDetail>().ToList();

            var claimsets = Context.Query<ClaimSet>().ToList();


            return details.Where(d => claimsets.Any(c => c.ClaimSetName == d.ClaimSetName)).ToList();

        }


        public IEnumerable<ClaimsetDetail> GetClaimSetDetailsWithProfiles()
        {
            var details = DetailContext.Query<ClaimsetDetail>().Include(d=>d.ClaimsetType).Include(d=>d.Profile).ToList();
            var claimsets = Context.Query<ClaimSet>().ToList();
         
            return details.Where(d => claimsets.Any(c => c.ClaimSetName == d.ClaimSetName)).ToList();

        }

       
        
        public ClaimsetDetail GetClaimSetDetails(string claimSetName)
        {
            var claimsetDetail = DetailContext.Query<ClaimsetDetail>().FirstOrDefault(c => c.ClaimSetName == claimSetName);
            return claimsetDetail;
        }

        public IEnumerable<SecurityStrategy> GetSecurityStrategies()
        {
            var details = Context.Query<SecurityStrategy>().ToList();
            
            return details;

        }

        public IEnumerable<AllowedActionsByClaimset> GetAllowedActionsByClaimset(string claimsetName)
        {
            var details = Context.Query<AllowedActionsByClaimset>().Where(c=>c.ClaimSetName== claimsetName).ToList();

            return details;

        }

    }
}
