namespace Ed_Fi.Credential.Domain.Model
{
    public partial class ClaimsetDetail
    {
        public virtual System.Collections.Generic.ICollection<Application> Applications { get; set; }
      
        public string ProfileName {
            get { return Profile != null ? Profile.ProfileName : string.Empty; }
        }
    }
}
