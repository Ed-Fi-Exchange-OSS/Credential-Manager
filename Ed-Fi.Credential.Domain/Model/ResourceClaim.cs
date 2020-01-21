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

    // ResourceClaims
    [System.CodeDom.Compiler.GeneratedCode("EF.Reverse.POCO.Generator", "2.32.0.0")]
    public partial class ResourceClaim
    {
        public int ResourceClaimId { get; set; } // ResourceClaimId (Primary key)
        public string DisplayName { get; set; } // DisplayName (length: 255)
        public string ResourceName { get; set; } // ResourceName (length: 2048)
        public string ClaimName { get; set; } // ClaimName (length: 2048)
        public int? ParentResourceClaimId { get; set; } // ParentResourceClaimId
        public int ApplicationApplicationId { get; set; } // Application_ApplicationId

        // Reverse navigation

        /// <summary>
        /// Child ClaimSetResourceClaims where [ClaimSetResourceClaims].[ResourceClaim_ResourceClaimId] point to this entity (FK_dbo.ClaimSetResourceClaims_dbo.ResourceClaims_ResourceClaim_ResourceClaimId)
        /// </summary>
        public virtual System.Collections.Generic.ICollection<ClaimSetResourceClaim> ClaimSetResourceClaims { get; set; } // ClaimSetResourceClaims.FK_dbo.ClaimSetResourceClaims_dbo.ResourceClaims_ResourceClaim_ResourceClaimId
        /// <summary>
        /// Child ResourceClaims where [ResourceClaims].[ParentResourceClaimId] point to this entity (FK_dbo.ResourceClaims_dbo.ResourceClaims_ParentResourceClaimId)
        /// </summary>
        public virtual System.Collections.Generic.ICollection<ResourceClaim> ResourceClaims { get; set; } // ResourceClaims.FK_dbo.ResourceClaims_dbo.ResourceClaims_ParentResourceClaimId

        // Foreign keys

        /// <summary>
        /// Parent ResourceClaim pointed by [ResourceClaims].([ParentResourceClaimId]) (FK_dbo.ResourceClaims_dbo.ResourceClaims_ParentResourceClaimId)
        /// </summary>
        public virtual ResourceClaim ParentResourceClaim { get; set; } // FK_dbo.ResourceClaims_dbo.ResourceClaims_ParentResourceClaimId

        public ResourceClaim()
        {
            ClaimSetResourceClaims = new System.Collections.Generic.List<ClaimSetResourceClaim>();
            ResourceClaims = new System.Collections.Generic.List<ResourceClaim>();
            InitializePartial();
        }

        partial void InitializePartial();
    }

}
// </auto-generated>