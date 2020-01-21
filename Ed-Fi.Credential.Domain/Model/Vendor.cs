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

    // Vendors
    [System.CodeDom.Compiler.GeneratedCode("EF.Reverse.POCO.Generator", "2.32.0.0")]
    public partial class Vendor
    {
        public int VendorId { get; set; } // VendorId (Primary key)
        public string VendorName { get; set; } // VendorName

        // Reverse navigation

        /// <summary>
        /// Child Agreements where [Agreement].[VendorId] point to this entity (FK_Agreement_Vendor)
        /// </summary>
        public virtual System.Collections.Generic.ICollection<Agreement> Agreements { get; set; } // Agreement.FK_Agreement_Vendor
        /// <summary>
        /// Child Applications where [Applications].[Vendor_VendorId] point to this entity (FK_dbo.Applications_dbo.Vendors_Vendor_VendorId)
        /// </summary>
        public virtual System.Collections.Generic.ICollection<Application> Applications { get; set; } // Applications.FK_dbo.Applications_dbo.Vendors_Vendor_VendorId
        /// <summary>
        /// Child Subscriptions where [Subscription].[VendorId] point to this entity (FK_Subscription_Vendor)
        /// </summary>
        public virtual System.Collections.Generic.ICollection<Subscription> Subscriptions { get; set; } // Subscription.FK_Subscription_Vendor
        /// <summary>
        /// Child Users where [Users].[Vendor_VendorId] point to this entity (FK_dbo.Users_dbo.Vendors_Vendor_VendorId)
        /// </summary>
        public virtual System.Collections.Generic.ICollection<User> Users { get; set; } // Users.FK_dbo.Users_dbo.Vendors_Vendor_VendorId
        /// <summary>
        /// Child VendorEmailDomains where [VendorEmailDomains].[Vendor_VendorId] point to this entity (FK_dbo.VendorEmailDomains_dbo.Vendors_Vendor_VendorId)
        /// </summary>
        public virtual System.Collections.Generic.ICollection<VendorEmailDomain> VendorEmailDomains { get; set; } // VendorEmailDomains.FK_dbo.VendorEmailDomains_dbo.Vendors_Vendor_VendorId
        /// <summary>
        /// Child VendorNamespacePrefixes where [VendorNamespacePrefixes].[Vendor_VendorId] point to this entity (FK_dbo.VendorNamespacePrefixes_dbo.Vendors_Vendor_VendorId)
        /// </summary>
        public virtual System.Collections.Generic.ICollection<VendorNamespacePrefix> VendorNamespacePrefixes { get; set; } // VendorNamespacePrefixes.FK_dbo.VendorNamespacePrefixes_dbo.Vendors_Vendor_VendorId

        public Vendor()
        {
            Agreements = new System.Collections.Generic.List<Agreement>();
            Applications = new System.Collections.Generic.List<Application>();
            Subscriptions = new System.Collections.Generic.List<Subscription>();
            Users = new System.Collections.Generic.List<User>();
            VendorEmailDomains = new System.Collections.Generic.List<VendorEmailDomain>();
            VendorNamespacePrefixes = new System.Collections.Generic.List<VendorNamespacePrefix>();
            InitializePartial();
        }

        partial void InitializePartial();
    }

}
// </auto-generated>
