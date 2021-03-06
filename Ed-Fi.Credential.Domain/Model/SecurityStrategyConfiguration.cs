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

    // SecurityStrategies
    [System.CodeDom.Compiler.GeneratedCode("EF.Reverse.POCO.Generator", "2.32.0.0")]
    public partial class SecurityStrategyConfiguration : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<SecurityStrategy>
    {
        public SecurityStrategyConfiguration()
            : this("dbo")
        {
        }

        public SecurityStrategyConfiguration(string schema)
        {
            ToTable("SecurityStrategies", schema);
            HasKey(x => new { x.ResourceClaimId, x.ClaimsetId, x.ResourceName, x.ClaimSetName });

            Property(x => x.ResourceClaimId).HasColumnName(@"ResourceClaimId").HasColumnType("int").IsRequired().HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
            Property(x => x.ClaimsetId).HasColumnName(@"ClaimsetId").HasColumnType("int").IsRequired().HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
            Property(x => x.ResourceName).HasColumnName(@"ResourceName").HasColumnType("nvarchar").IsRequired().HasMaxLength(2048).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
            Property(x => x.ClaimSetName).HasColumnName(@"ClaimSetName").HasColumnType("nvarchar").IsRequired().HasMaxLength(255).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
            Property(x => x.ReadStrategy).HasColumnName(@"ReadStrategy").HasColumnType("nvarchar").IsOptional().HasMaxLength(255);
            Property(x => x.CreateStrategy).HasColumnName(@"CreateStrategy").HasColumnType("nvarchar").IsOptional().HasMaxLength(255);
            Property(x => x.UpdateStrategy).HasColumnName(@"UpdateStrategy").HasColumnType("nvarchar").IsOptional().HasMaxLength(255);
            Property(x => x.DeleteStrategy).HasColumnName(@"DeleteStrategy").HasColumnType("nvarchar").IsOptional().HasMaxLength(255);
            InitializePartial();
        }
        partial void InitializePartial();
    }

}
// </auto-generated>
