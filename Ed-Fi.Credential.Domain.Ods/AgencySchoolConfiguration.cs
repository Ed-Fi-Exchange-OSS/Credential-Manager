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


namespace Ed_Fi.Credential.Domain.Ods
{

    // AgencySchool
    [System.CodeDom.Compiler.GeneratedCode("EF.Reverse.POCO.Generator", "2.32.0.0")]
    public partial class AgencySchoolConfiguration : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<AgencySchool>
    {
        public AgencySchoolConfiguration()
            : this("dbo")
        {
        }

        public AgencySchoolConfiguration(string schema)
        {
            ToTable("AgencySchool", schema);
            HasKey(x => new { x.EducationOrganizationId, x.LocalEducationAgencyId, x.NameOfInstitution, x.IsChoice, x.SchoolYear, x.SchoolType, x.SchoolTypeDescription, x.SchoolTypeCode });

            Property(x => x.EducationOrganizationId).HasColumnName(@"EducationOrganizationId").HasColumnType("int").IsRequired().HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
            Property(x => x.LocalEducationAgencyId).HasColumnName(@"LocalEducationAgencyId").HasColumnType("int").IsRequired().HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
            Property(x => x.NameOfInstitution).HasColumnName(@"NameOfInstitution").HasColumnType("nvarchar").IsRequired().HasMaxLength(75).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
            Property(x => x.IsChoice).HasColumnName(@"IsChoice").HasColumnType("bit").IsRequired().HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
            Property(x => x.SchoolCode).HasColumnName(@"SchoolCode").HasColumnType("nvarchar").IsOptional().HasMaxLength(60);
            Property(x => x.City).HasColumnName(@"City").HasColumnType("nvarchar").IsOptional().HasMaxLength(30);
            Property(x => x.SchoolYear).HasColumnName(@"SchoolYear").HasColumnType("smallint").IsRequired().HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
            Property(x => x.SchoolAgencyType).HasColumnName(@"SchoolAgencyType").HasColumnType("nvarchar").IsOptional().HasMaxLength(50);
            Property(x => x.SchoolType).HasColumnName(@"SchoolType").HasColumnType("int").IsRequired().HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
            Property(x => x.SchoolTypeDescription).HasColumnName(@"SchoolTypeDescription").HasColumnType("nvarchar").IsRequired().HasMaxLength(1024).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
            Property(x => x.SchoolTypeCode).HasColumnName(@"SchoolTypeCode").HasColumnType("nvarchar").IsRequired().HasMaxLength(50).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
            InitializePartial();
        }
        partial void InitializePartial();
    }

}
// </auto-generated>
