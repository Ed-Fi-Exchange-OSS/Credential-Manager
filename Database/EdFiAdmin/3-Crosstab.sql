CREATE VIEW [dbo].[Crosstab]
	AS

SELECT concat(ac.ApiClientId,'_',acaeo.ApplicationEducationOrganization_ApplicationEducationOrganizationId) RowId,
ac.ApiClientId,
case when org.LocalEducationAgencyId is not null then org.EducationOrganizationId else null end as SchoolId,
case when org.LocalEducationAgencyId is not null then org.NameOfInstitution else null end as SchoolName,
case when org.LocalEducationAgencyId is not null then org.LocalEducationAgencyId else org.EducationOrganizationId end as LeaId,
case when org.LocalEducationAgencyId is not null then org.Lea else org.NameOfInstitution end as LeaName,
v.VendorName, ac.Name ApiClientName, ac.[Key] ApiKey, case when ac.SecretIsHashed=1 then '***hashed***' else ac.Secret end [Secret],
a.ClaimSetName,  p.ProfileName
FROM dbo.ApiClients ac
inner join dbo.ApiClientApplicationEducationOrganizations acaeo on ac.ApiClientId= acaeo.ApiClient_ApiClientId 
inner join dbo.ApplicationEducationOrganizations aeo on acaeo.ApplicationEducationOrganization_ApplicationEducationOrganizationId = aeo.ApplicationEducationOrganizationId
inner join[dbo].[Applications] a  on a.ApplicationId = ac.Application_ApplicationId
inner join [dbo].[Vendors] v on a.Vendor_VendorId = v.VendorId
left outer join [dbo].[ProfileApplications] pa on pa.Application_ApplicationId = a.ApplicationId
left outer join [dbo].[Profiles] p on pa.Profile_ProfileId = p.ProfileId
inner join dbo.LatestEdOrgs org
	on aeo.EducationOrganizationId = org.EducationOrganizationId
GO

