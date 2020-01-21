CREATE VIEW [dbo].[Agency]
	AS 
	Select  eo.EducationOrganizationId, 
			eo.NameOfInstitution, 
			cast(0 as bit) IsChoice, 
			cast(lea.StateEducationAgencyId as varchar) AgencyCode, 
			a.City, 
			year(getdate()) SchoolYear,
	lt.CodeValue AgencyType, lt.Description AgencyTypeDescription
	from edfi.LocalEducationAgency lea 
	inner join edfi.EducationOrganization eo
		on eo.EducationOrganizationId = lea.LocalEducationAgencyId 	
	left outer join edfi.Descriptor os 
		on eo.OperationalStatusDescriptorId = os.DescriptorId
	left outer join edfi.Descriptor lt 
		on lea.LocalEducationAgencyCategoryDescriptorId = lt.DescriptorId
	left outer join edfi.EducationOrganizationAddress a 
		on eo.EducationOrganizationId = a.EducationOrganizationId 
	WHERE os.CodeValue='Active'
GO

