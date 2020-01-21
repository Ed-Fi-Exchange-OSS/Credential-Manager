using Ed_Fi.Credential.Domain.Model;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Ed_Fi.Credential.Business.Common;

namespace Ed_Fi.Credential.Business
{
    public interface IUserBusiness : IBaseBusiness<User>
    {
        IEnumerable<User> GetUsers();
        IEnumerable<User> GetUsers(int vendorId);
        User GetUser(int userId);
        User GetUserByWamsId(string wamsId);
        User GetNonAsmUserByEmail(string email);
        AsmUser GetAsmUser(string userName);
        AsmUser GetAsmUserByWamsId(string wamsId);
        bool AsmUserInUse(string userName, int? userId = null);
        void Create(User user);
        void Remove(User user);
        void Remove(UserAsmUser userAsmUser);
    }

    public class UserBusiness : BaseEdFiAdminBusiness<User>, IUserBusiness
    {
        public UserBusiness(IEdFiAdminDbContext context)
            : base(context)
        {
        }

        public IEnumerable<User> GetUsers()
        {
            return Context.Query<User>()
                .Include(a => a.UserAsmUser)
                .Include(v=>v.Vendor);
        }

        public IEnumerable<User> GetUsers(int vendorId)
        {
            return Context.Query<User>()
                .Where(u => u.VendorVendorId == vendorId);
        }

        public User GetUser(int userId)
        {
            return Context.Query<User>()
                .Include(a => a.UserAsmUser)
                .SingleOrDefault(a => a.UserId == userId);
        }

        public User GetUserByWamsId(string wamsId)
        {
          return Context.Query<User>()
                .Include(a => a.UserAsmUser)
                .FirstOrDefault(u => u.UserAsmUser != null && u.UserAsmUser.WamsId == wamsId);
        }
        public User GetNonAsmUserByEmail(string email)
        {
            return Context.Query<User>()
                .FirstOrDefault(u => u.Email == email && u.UserAsmUser == null);
        }

        public bool AsmUserInUse(string userName, int? userId = null)
        {
            return Context.Query<UserAsmUser>()
                .Any(a => a.WamsUser == userName && a.UserUserId != userId);
        }

        public AsmUser GetAsmUser(string userName)
        {
            return Context.Query<AsmUser>()
                .SingleOrDefault(a => a.UserName == userName);
        }

        public AsmUser GetAsmUserByWamsId(string wamsId)
        {
            return Context.Query<AsmUser>()
                .SingleOrDefault(a => a.WamsId == wamsId);
        }

        public void Create(User user)
        {
            Context.AddEntity(user);
        }

        public void Remove(User user)
        {
            foreach (var apiClient in Context.Query<ApiClient>().Where(c=>c.UserUserId==user.UserId).ToList())
            {
                apiClient.UserUserId = null;
            }
            Context.RemoveEntity(user);
        }

        public void Remove(UserAsmUser userAsmUser)
        {
            Context.RemoveEntity(userAsmUser);
        }
    }
}
