namespace Ed_Fi.Credential.Domain.Model
{
    public partial class Application
    {
        public ClaimsetDetail ClaimsetDetail { get; set; }
    }

    public partial class ApplicationConfiguration
    {
        partial void InitializePartial()
        {
            this.HasOptional(s => s.ClaimsetDetail).WithMany(p => p.Applications).HasForeignKey(s => s.ClaimSetName);
        }
    }
}
