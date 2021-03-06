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

    // SiteContent
    [System.CodeDom.Compiler.GeneratedCode("EF.Reverse.POCO.Generator", "2.32.0.0")]
    public partial class SiteContentConfiguration : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<SiteContent>
    {
        public SiteContentConfiguration()
            : this("dbo")
        {
        }

        public SiteContentConfiguration(string schema)
        {
            ToTable("SiteContent", schema);
            HasKey(x => x.Id);

            Property(x => x.Id).HasColumnName(@"Id").HasColumnType("int").IsRequired().HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity);
            Property(x => x.SiteContentTypeId).HasColumnName(@"SiteContentTypeId").HasColumnType("int").IsRequired();
            Property(x => x.Body).HasColumnName(@"Body").HasColumnType("varchar").IsRequired().IsUnicode(false).HasMaxLength(8000);

            // Foreign keys
            HasRequired(a => a.Lookup).WithMany(b => b.SiteContents).HasForeignKey(c => c.SiteContentTypeId).WillCascadeOnDelete(false); // FK_SiteContent_Type
            InitializePartial();
        }
        partial void InitializePartial();
    }

}
// </auto-generated>
