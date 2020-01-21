using Ed_Fi.Credential.Domain.Model;

namespace Ed_Fi.Credential.Business
{
    public abstract class BaseEdFiSecurityBusiness<TEntity> : BaseBusiness<TEntity>
        where TEntity : class
    {
        protected BaseEdFiSecurityBusiness(IEdFiSecurityDbContext context)
            : base(context)
        {
        }
    }
}
