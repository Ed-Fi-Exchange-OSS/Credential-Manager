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

    // Subscription
    [System.CodeDom.Compiler.GeneratedCode("EF.Reverse.POCO.Generator", "2.32.0.0")]
    public partial class Subscription
    {
        public int SubscriptionId { get; set; } // SubscriptionId (Primary key)
        public int SubscriptionActionId { get; set; } // SubscriptionActionId
        public int EducationOrganizationId { get; set; } // EducationOrganizationId
        public int VendorId { get; set; } // VendorId
        public string WamsId { get; set; } // WamsId (length: 30)
        public System.DateTime CreatedDate { get; set; } // CreatedDate

        // Foreign keys

        /// <summary>
        /// Parent SubscriptionAction pointed by [Subscription].([SubscriptionActionId]) (FK_Subscription_SubscriptionAction)
        /// </summary>
        public virtual SubscriptionAction SubscriptionAction { get; set; } // FK_Subscription_SubscriptionAction

        /// <summary>
        /// Parent Vendor pointed by [Subscription].([VendorId]) (FK_Subscription_Vendor)
        /// </summary>
        public virtual Vendor Vendor { get; set; } // FK_Subscription_Vendor

        public Subscription()
        {
            InitializePartial();
        }

        partial void InitializePartial();
    }

}
// </auto-generated>
