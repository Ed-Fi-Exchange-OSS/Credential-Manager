namespace Ed_Fi.Credential.MVC.ImplementationSpecific
{
    public interface ISessionInfo
    {
        void SetCurrentAgency(int edOrgId);
        int? CurrentAgencyId { get; }
         WamsPrincipal User { get; set; }
    }

    public class SessionInfo : ISessionInfo
    {
        public void SetCurrentAgency(int edOrgId)
        {
            CurrentAgencyId = edOrgId;
        }

        public int? CurrentAgencyId { get; set; }
        public WamsPrincipal User { get; set; }
    }
}