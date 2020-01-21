CREATE VIEW [dbo].[AgencySchool]
	AS 

	Select eo.EducationOrganizationId,
			lea.LocalEducationAgencyId, 
			eo.NameOfInstitution, 
			cast(0 as bit) IsChoice, 
			cast(eo.EducationOrganizationId as varchar) SchoolCode, 
			a.City, 
			year(getdate()) SchoolYear,
			st.CodeValue SchoolAgencyType, 
			sc.SchoolCategoryDescriptorId SchoolType, 
			sct.Description SchoolTypeDescription,
			sct.CodeValue SchoolTypeCode
	from  edfi.School sch
	inner join edfi.LocalEducationAgency lea  on lea.LocalEducationAgencyId=sch.LocalEducationAgencyId 
	inner join edfi.EducationOrganization eo
		on eo.EducationOrganizationId = sch.SchoolId
	left outer join edfi.Descriptor os 
		on eo.OperationalStatusDescriptorId = os.DescriptorId
    inner join edfi.SchoolCategory sc on sc.SchoolId = sch.SchoolId
	inner join edfi.Descriptor sct on sct.DescriptorId=sc.SchoolCategoryDescriptorId
	left outer join edfi.Descriptor st 
		on sch.SchoolTypeDescriptorId = st.DescriptorId
	left outer join edfi.EducationOrganizationAddress a 
		on eo.EducationOrganizationId = a.EducationOrganizationId
	WHERE os.CodeValue='Active'

GO

