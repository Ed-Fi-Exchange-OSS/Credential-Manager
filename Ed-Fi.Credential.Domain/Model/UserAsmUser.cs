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
    public partial class UserAsmUser
    {
        public int UserUserId { get; set; } // User_UserId (Primary key)
        public string WamsId { get; set; } // WamsId (length: 30)
        public string WamsUser { get; set; } // WamsUser (length: 20)

        // Foreign keys

        /// <summary>
        /// Parent User pointed by [UserAsmUser].([UserUserId]) (FK_UserAsmUser_Users)
        /// </summary>
        public virtual User User { get; set; } // FK_UserAsmUser_Users

        public UserAsmUser()
        {
            InitializePartial();
        }

        partial void InitializePartial();
    }

}
// </auto-generated>
