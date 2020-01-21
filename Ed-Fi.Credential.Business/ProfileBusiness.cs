using System.Collections.Generic;
using System.Linq;
using Ed_Fi.Credential.Business.Common;
using Ed_Fi.Credential.Domain.Model;

namespace Ed_Fi.Credential.Business
{
    public interface IProfileBusiness : IBaseBusiness<Profile>
    {
        Profile GetById(int profileId);
     
        List<Profile> Get(bool dpiOnly = true);
     
    }

    public class ProfileBusiness : BaseEdFiAdminBusiness<Profile>, IProfileBusiness
    {
        public ProfileBusiness(IEdFiAdminDbContext context) 
            : base(context)
        {
        }

        public Profile GetById(int profileId)
        {
            var profile = Context.Query<Profile>()
                .FirstOrDefault(p => p.ProfileId == profileId);
            return profile;
        }

      
        public List<Profile> Get(bool dpiOnly=true)
        {
            const string wiDpi = "WiDpi";
            var profiles = Context.Query<Profile>();
            if (dpiOnly)
                profiles = profiles.Where(p => p.ProfileName.Contains(wiDpi));
            return profiles.ToList();
        }

       
    }
}
