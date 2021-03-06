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

    // UserAsmUser
    [System.CodeDom.Compiler.GeneratedCode("EF.Reverse.POCO.Generator", "2.32.0.0")]
    public partial class UserAsmUserConfiguration : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<UserAsmUser>
    {
        public UserAsmUserConfiguration()
            : this("dbo")
        {
        }

        public UserAsmUserConfiguration(string schema)
        {
            ToTable("UserAsmUser", schema);
            HasKey(x => x.UserUserId);

            Property(x => x.UserUserId).HasColumnName(@"User_UserId").HasColumnType("int").IsRequired().HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
            Property(x => x.WamsId).HasColumnName(@"WamsId").HasColumnType("varchar").IsRequired().IsUnicode(false).HasMaxLength(30);
            Property(x => x.WamsUser).HasColumnName(@"WamsUser").HasColumnType("varchar").IsRequired().IsUnicode(false).HasMaxLength(20);

            // Foreign keys
            HasRequired(a => a.User).WithOptional(b => b.UserAsmUser).WillCascadeOnDelete(false); // FK_UserAsmUser_Users
            InitializePartial();
        }
        partial void InitializePartial();
    }

}
// </auto-generated>
