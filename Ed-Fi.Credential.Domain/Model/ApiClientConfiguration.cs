// <auto-generated>
// ReSharper disable ConvertPropertyToExpressionBody
// ReSharper disable DoNotCallOverridableMethodsInConstructor
// ReSharper disable InconsistentNaming
// ReSharper disable PartialMethodWithSinglePart
// ReSharper disable PartialTypeWithSinglePart
// ReSharper disable RedundantNameQualifier
// ReSharper disable RedundantOverridenMember
// ReSharper disable UseNameofExpression
// TargetFrameworkVersion = 4.7
#pragma warning disable 1591    //  Ignore "Missing XML Comment" warning


namespace Ed_Fi.Credential.Domain.Model
{

    // ApiClients
    [System.CodeDom.Compiler.GeneratedCode("EF.Reverse.POCO.Generator", "2.32.0.0")]
    public partial class ApiClientConfiguration : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<ApiClient>
    {
        public ApiClientConfiguration()
            : this("dbo")
        {
        }

        public ApiClientConfiguration(string schema)
        {
            ToTable("ApiClients", schema);
            HasKey(x => x.ApiClientId);

            Property(x => x.ApiClientId).HasColumnName(@"ApiClientId").HasColumnType("int").IsRequired().HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity);
            Property(x => x.Key).HasColumnName(@"Key").HasColumnType("nvarchar").IsRequired().HasMaxLength(50);
            Property(x => x.Secret).HasColumnName(@"Secret").HasColumnType("nvarchar").IsRequired().HasMaxLength(100);
            Property(x => x.Name).HasColumnName(@"Name").HasColumnType("nvarchar").IsRequired().HasMaxLength(50);
            Property(x => x.IsApproved).HasColumnName(@"IsApproved").HasColumnType("bit").IsRequired();
            Property(x => x.UseSandbox).HasColumnName(@"UseSandbox").HasColumnType("bit").IsRequired();
            Property(x => x.SandboxType).HasColumnName(@"SandboxType").HasColumnType("int").IsRequired();
            Property(x => x.ApplicationApplicationId).HasColumnName(@"Application_ApplicationId").HasColumnType("int").IsOptional();
            Property(x => x.UserUserId).HasColumnName(@"User_UserId").HasColumnType("int").IsOptional();
            Property(x => x.KeyStatus).HasColumnName(@"KeyStatus").HasColumnType("nvarchar(max)").IsOptional();
            Property(x => x.ChallengeId).HasColumnName(@"ChallengeId").HasColumnType("nvarchar(max)").IsOptional();
            Property(x => x.ChallengeExpiry).HasColumnName(@"ChallengeExpiry").HasColumnType("datetime").IsOptional();
            Property(x => x.ActivationCode).HasColumnName(@"ActivationCode").HasColumnType("nvarchar(max)").IsOptional();
            Property(x => x.ActivationRetried).HasColumnName(@"ActivationRetried").HasColumnType("int").IsOptional();
            Property(x => x.SecretIsHashed).HasColumnName(@"SecretIsHashed").HasColumnType("bit").IsRequired();
            Property(x => x.StudentIdentificationSystemDescriptor).HasColumnName(@"StudentIdentificationSystemDescriptor").HasColumnType("nvarchar").IsOptional().HasMaxLength(306);

            // Foreign keys
            HasOptional(a => a.Application).WithMany(b => b.ApiClients).HasForeignKey(c => c.ApplicationApplicationId).WillCascadeOnDelete(false); // FK_dbo.ApiClients_dbo.Applications_Application_ApplicationId
            HasOptional(a => a.User).WithMany(b => b.ApiClients).HasForeignKey(c => c.UserUserId).WillCascadeOnDelete(false); // FK_dbo.ApiClients_dbo.Users_User_UserId
            HasMany(t => t.ApplicationEducationOrganizations).WithMany(t => t.ApiClients).Map(m =>
            {
                m.ToTable("ApiClientApplicationEducationOrganizations", "dbo");
                m.MapLeftKey("ApiClient_ApiClientId");
                m.MapRightKey("ApplicationEducationOrganization_ApplicationEducationOrganizationId");
            });
            InitializePartial();
        }
        partial void InitializePartial();
    }

}
// </auto-generated>
