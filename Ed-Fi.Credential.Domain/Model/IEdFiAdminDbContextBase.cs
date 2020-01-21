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

    public interface IEdFiAdminDbContextBase : System.IDisposable
    {
        System.Data.Entity.DbSet<Agreement> Agreements { get; set; } // Agreement
        System.Data.Entity.DbSet<ApiClient> ApiClients { get; set; } // ApiClients
        System.Data.Entity.DbSet<Application> Applications { get; set; } // Applications
        System.Data.Entity.DbSet<ApplicationEducationOrganization> ApplicationEducationOrganizations { get; set; } // ApplicationEducationOrganizations
        System.Data.Entity.DbSet<AsmUser> AsmUsers { get; set; } // AsmUser
        System.Data.Entity.DbSet<AuditEntry> AuditEntries { get; set; } // AuditEntry
        System.Data.Entity.DbSet<AuditEntryProperty> AuditEntryProperties { get; set; } // AuditEntryProperty
        System.Data.Entity.DbSet<ClaimsetDetail> ClaimsetDetails { get; set; } // ClaimsetDetails
        System.Data.Entity.DbSet<ClaimsetType> ClaimsetTypes { get; set; } // ClaimsetType
        System.Data.Entity.DbSet<ClientAccessToken> ClientAccessTokens { get; set; } // ClientAccessTokens
        System.Data.Entity.DbSet<Crosstab> Crosstabs { get; set; } // Crosstab
        System.Data.Entity.DbSet<LatestEdOrg> LatestEdOrgs { get; set; } // LatestEdOrgs
        System.Data.Entity.DbSet<Lookup> Lookups { get; set; } // Lookup
        System.Data.Entity.DbSet<Profile> Profiles { get; set; } // Profiles
        System.Data.Entity.DbSet<SiteContent> SiteContents { get; set; } // SiteContent
        System.Data.Entity.DbSet<Subscription> Subscriptions { get; set; } // Subscription
        System.Data.Entity.DbSet<SubscriptionAction> SubscriptionActions { get; set; } // SubscriptionAction
        System.Data.Entity.DbSet<User> Users { get; set; } // Users
        System.Data.Entity.DbSet<UserAsmUser> UserAsmUsers { get; set; } // UserAsmUser
        System.Data.Entity.DbSet<Vendor> Vendors { get; set; } // Vendors
        System.Data.Entity.DbSet<VendorEmailDomain> VendorEmailDomains { get; set; } // VendorEmailDomains
        System.Data.Entity.DbSet<VendorNamespacePrefix> VendorNamespacePrefixes { get; set; } // VendorNamespacePrefixes

        int SaveChanges();
        System.Threading.Tasks.Task<int> SaveChangesAsync();
        System.Threading.Tasks.Task<int> SaveChangesAsync(System.Threading.CancellationToken cancellationToken);
        System.Data.Entity.Infrastructure.DbChangeTracker ChangeTracker { get; }
        System.Data.Entity.Infrastructure.DbContextConfiguration Configuration { get; }
        System.Data.Entity.Database Database { get; }
        System.Data.Entity.Infrastructure.DbEntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class;
        System.Data.Entity.Infrastructure.DbEntityEntry Entry(object entity);
        System.Collections.Generic.IEnumerable<System.Data.Entity.Validation.DbEntityValidationResult> GetValidationErrors();
        System.Data.Entity.DbSet Set(System.Type entityType);
        System.Data.Entity.DbSet<TEntity> Set<TEntity>() where TEntity : class;
        string ToString();
    }

}
// </auto-generated>
