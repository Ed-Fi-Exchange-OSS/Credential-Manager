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

    // Agreement
    [System.CodeDom.Compiler.GeneratedCode("EF.Reverse.POCO.Generator", "2.32.0.0")]
    public partial class Agreement
    {
        public int Id { get; set; } // Id (Primary key)
        public int AgreementTypeId { get; set; } // AgreementTypeId
        public string WamsId { get; set; } // WamsId (length: 30)
        public bool Agree { get; set; } // Agree
        public System.DateTime ModifiedDate { get; set; } // ModifiedDate
        public string ModifierId { get; set; } // ModifierId (length: 30)
        public int? VendorId { get; set; } // VendorId
        public int? EducationOrganizationId { get; set; } // EducationOrganizationId

        // Foreign keys

        /// <summary>
        /// Parent Lookup pointed by [Agreement].([AgreementTypeId]) (FK_Agreement_Type)
        /// </summary>
        public virtual Lookup Lookup { get; set; } // FK_Agreement_Type

        /// <summary>
        /// Parent Vendor pointed by [Agreement].([VendorId]) (FK_Agreement_Vendor)
        /// </summary>
        public virtual Vendor Vendor { get; set; } // FK_Agreement_Vendor

        public Agreement()
        {
            Agree = true;
            InitializePartial();
        }

        partial void InitializePartial();
    }

}
// </auto-generated>
