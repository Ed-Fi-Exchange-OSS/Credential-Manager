CREATE VIEW [dbo].[LatestEdOrgs]
	AS 
	Select latest.EducationOrganizationId, year(getdate()) SchoolYear, 
		coalesce(e2.NameOfInstitution, e3.NameOfInstitution) NameOfInstitution, 
		coalesce(s2.LocalEducationAgencyId, s3.LocalEducationAgencyId) LocalEducationAgencyId,
		coalesce(l2.NameOfInstitution, l3.NameOfInstitution) Lea,
		coalesce(a2.City, a3.City) City
	from (	
		Select EducationOrganizationId, max(year(getdate())) SchoolYear
		from 
			(select EducationOrganizationId
			from [EdFi_Ods].edfi.EducationOrganization 
			left outer join edfi.Descriptor os 
			on OperationalStatusDescriptorId = DescriptorId
			where CodeValue='Active'
			) c
		group by EducationOrganizationId
		) latest 
left outer join [EdFi_Ods].edfi.EducationOrganization e2 on latest.EducationOrganizationId = e2.EducationOrganizationId 
left outer join [EdFi_Ods].edfi.School s2 on e2.EducationOrganizationId = s2.SchoolId 
left outer join [EdFi_Ods].edfi.EducationOrganization l2 on l2.EducationOrganizationId = s2.LocalEducationAgencyId 
left outer join [EdFi_Ods].edfi.EducationOrganizationAddress a2 on a2.EducationOrganizationId = e2.EducationOrganizationId 
GO

