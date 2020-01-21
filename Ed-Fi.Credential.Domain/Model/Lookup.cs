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

    // Lookup
    [System.CodeDom.Compiler.GeneratedCode("EF.Reverse.POCO.Generator", "2.32.0.0")]
    public partial class Lookup
    {
        public int Id { get; set; } // Id (Primary key)
        public string Type { get; set; } // Type (length: 50)
        public string Description { get; set; } // Description (length: 80)
        public string Code { get; set; } // Code (length: 100)
        public int SortOrder { get; set; } // SortOrder
        public bool Active { get; set; } // Active

        // Reverse navigation

        /// <summary>
        /// Child Agreements where [Agreement].[AgreementTypeId] point to this entity (FK_Agreement_Type)
        /// </summary>
        public virtual System.Collections.Generic.ICollection<Agreement> Agreements { get; set; } // Agreement.FK_Agreement_Type
        /// <summary>
        /// Child SiteContents where [SiteContent].[SiteContentTypeId] point to this entity (FK_SiteContent_Type)
        /// </summary>
        public virtual System.Collections.Generic.ICollection<SiteContent> SiteContents { get; set; } // SiteContent.FK_SiteContent_Type

        public Lookup()
        {
            Active = true;
            Agreements = new System.Collections.Generic.List<Agreement>();
            SiteContents = new System.Collections.Generic.List<SiteContent>();
            InitializePartial();
        }

        partial void InitializePartial();
    }

}
// </auto-generated>