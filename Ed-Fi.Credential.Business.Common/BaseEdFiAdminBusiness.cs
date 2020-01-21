using Ed_Fi.Credential.Domain.Model;

namespace Ed_Fi.Credential.Business.Common
{
    public abstract class BaseEdFiAdminBusiness<TEntity> : BaseBusiness<TEntity>
        where TEntity : class
    {
        protected BaseEdFiAdminBusiness(IEdFiAdminDbContext context)
            : base(context)
        {
        }
    }
}
