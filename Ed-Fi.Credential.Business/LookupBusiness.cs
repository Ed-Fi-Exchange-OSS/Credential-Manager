using System.Collections.Generic;
using System.Linq;
using Ed_Fi.Credential.Business.Common;
using Ed_Fi.Credential.Domain.Model;

namespace Ed_Fi.Credential.Business
{
    public interface ILookupBusiness : IBaseBusiness<Lookup>
    {
        IEnumerable<Lookup> GetByType(string type);
    }

    public class LookupBusiness : BaseEdFiAdminBusiness<Lookup>, ILookupBusiness
    {
        public LookupBusiness(IEdFiAdminDbContext context)
            : base(context)
        {
        }

        public IEnumerable<Lookup> GetByType(string type)
        {
            var lookups = Query().Where(l => l.Type == type).AsEnumerable();

            return lookups;
        }
    }
}
