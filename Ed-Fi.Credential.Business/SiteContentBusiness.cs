using System.Linq;
using Ed_Fi.Credential.Business.Common;
using Ed_Fi.Credential.Domain.Model;

namespace Ed_Fi.Credential.Business
{
    public interface ISiteContentBusiness : IBaseBusiness<SiteContent>
    {
        SiteContent GetByContentTypeId(int contentTypeId);
    }

    public class SiteContentBusiness : BaseEdFiAdminBusiness<SiteContent>, ISiteContentBusiness
    {
        public SiteContentBusiness(IEdFiAdminDbContext context)
            : base(context)
        {
        }

        public SiteContent GetByContentTypeId(int contentTypeId)
        {
            var content = Query().FirstOrDefault(c => c.SiteContentTypeId == contentTypeId);

            return content;
        }
    }
}
