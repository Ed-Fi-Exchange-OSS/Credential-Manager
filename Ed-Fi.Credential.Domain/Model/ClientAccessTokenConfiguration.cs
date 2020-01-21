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

    // ClientAccessTokens
    [System.CodeDom.Compiler.GeneratedCode("EF.Reverse.POCO.Generator", "2.32.0.0")]
    public partial class ClientAccessTokenConfiguration : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<ClientAccessToken>
    {
        public ClientAccessTokenConfiguration()
            : this("dbo")
        {
        }

        public ClientAccessTokenConfiguration(string schema)
        {
            ToTable("ClientAccessTokens", schema);
            HasKey(x => x.Id);

            Property(x => x.Id).HasColumnName(@"Id").HasColumnType("uniqueidentifier").IsRequired().HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
            Property(x => x.Expiration).HasColumnName(@"Expiration").HasColumnType("datetime").IsRequired();
            Property(x => x.Scope).HasColumnName(@"Scope").HasColumnType("nvarchar(max)").IsOptional();
            Property(x => x.ApiClientApiClientId).HasColumnName(@"ApiClient_ApiClientId").HasColumnType("int").IsOptional();

            // Foreign keys
            HasOptional(a => a.ApiClient).WithMany(b => b.ClientAccessTokens).HasForeignKey(c => c.ApiClientApiClientId).WillCascadeOnDelete(false); // FK_dbo.ClientAccessTokens_dbo.ApiClients_ApiClient_ApiClientId
            InitializePartial();
        }
        partial void InitializePartial();
    }

}
// </auto-generated>