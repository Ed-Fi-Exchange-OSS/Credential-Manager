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

    // VendorEmailDomains
    [System.CodeDom.Compiler.GeneratedCode("EF.Reverse.POCO.Generator", "2.32.0.0")]
    public partial class VendorEmailDomainConfiguration : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<VendorEmailDomain>
    {
        public VendorEmailDomainConfiguration()
            : this("dbo")
        {
        }

        public VendorEmailDomainConfiguration(string schema)
        {
            ToTable("VendorEmailDomains", schema);
            HasKey(x => x.VendorEmailDomainId);

            Property(x => x.VendorEmailDomainId).HasColumnName(@"VendorEmailDomainId").HasColumnType("int").IsRequired().HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity);
            Property(x => x.EmailDomain).HasColumnName(@"EmailDomain").HasColumnType("nvarchar").IsRequired().HasMaxLength(255);
            Property(x => x.VendorVendorId).HasColumnName(@"Vendor_VendorId").HasColumnType("int").IsRequired();

            // Foreign keys
            HasRequired(a => a.Vendor).WithMany(b => b.VendorEmailDomains).HasForeignKey(c => c.VendorVendorId); // FK_dbo.VendorEmailDomains_dbo.Vendors_Vendor_VendorId
            InitializePartial();
        }
        partial void InitializePartial();
    }

}
// </auto-generated>