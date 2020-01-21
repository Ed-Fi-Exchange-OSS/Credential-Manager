using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;

namespace Ed_Fi.Credential.MVC.ImplementationSpecific
{

    public class WamsPrincipal : IPrincipal
    {

        public WamsPrincipal(string wamsId, string firstName, string lastName, string email,
            System.Collections.Generic.IEnumerable<WamsRole> wamsroles)
        {
            this.Email = email;
            this.FirstName = firstName;
            this.FullName = firstName + " " + lastName;
            this.LastName = lastName;
            this.WamsId = wamsId;
            this.Roles = wamsroles;
        }

        public string WamsId { get; }
        public string Email { get; }
        public string FirstName { get; }
        public string FullName { get; }
        public string LastName { get; }
        public System.Collections.Generic.IEnumerable<WamsRole> Roles { get; }

        public IIdentity Identity => new GenericIdentity(WamsId);

        public bool HasPrivilege(int edOrgId, object toString)
        {
            var priv = (WisePrivilege)Enum.Parse(typeof(WisePrivilege), toString.ToString());
           return Roles.Any(r => r.EducationOrganizationId == edOrgId && r.Privileges.Contains(priv));
           
        }

         bool IPrincipal.IsInRole(string role)
        {
            return Roles.Any(r => r.Role == role);
        }

        public List<WisePrivilege> GetPrivileges(int edOrgId)
        {
            return Roles.FirstOrDefault(r => r.EducationOrganizationId == edOrgId)?.Privileges;

        }
    }
}
