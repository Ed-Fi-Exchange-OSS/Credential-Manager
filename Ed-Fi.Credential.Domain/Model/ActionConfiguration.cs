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

    // Actions
    [System.CodeDom.Compiler.GeneratedCode("EF.Reverse.POCO.Generator", "2.32.0.0")]
    public partial class ActionConfiguration : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<Action>
    {
        public ActionConfiguration()
            : this("dbo")
        {
        }

        public ActionConfiguration(string schema)
        {
            ToTable("Actions", schema);
            HasKey(x => x.ActionId);

            Property(x => x.ActionId).HasColumnName(@"ActionId").HasColumnType("int").IsRequired().HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity);
            Property(x => x.ActionName).HasColumnName(@"ActionName").HasColumnType("nvarchar").IsRequired().HasMaxLength(255);
            Property(x => x.ActionUri).HasColumnName(@"ActionUri").HasColumnType("nvarchar").IsRequired().HasMaxLength(2048);
            InitializePartial();
        }
        partial void InitializePartial();
    }

}
// </auto-generated>