MERGE INTO [dbo].[ClaimsetDetails] AS Target 
USING (VALUES 
	 ('SIS Vendor', 'Student/Enrollment', 1,1,1,0,0,0,0,2020,null, 'Read/write on all student related data')
	 ,('Ed-Fi Sandbox', 'Student/Enrollment',1, 1,1,0,0,0,0,2020,null, 'Relationship and namespaced access to certain data')
	 ,('Roster Vendor', 'Roster',6, 0,1,0,0,0,0,null,null, 'Read access to courses and education organizations')
	 ,('Assessment Vendor', 'Assessment',7, 0,1,0,0,0,0,null,null, 'Namespace based access to certain descriptors')
	 ,('Assessment Read', 'Assessment',7, 0,1,0,0,0,1,null,null, 'Read assessment metadata')
	 ,('District Hosted SIS Vendor', 'Student/Enrollment',1, 0,0,0,0,0,1,null,null, 'Read/write on all student related data')
	 
AS Source (ClaimSetName, LogicalDomain,  ClaimsetTypeId, PrimarySis, PublicClaimset, ChoiceClaimset, SchoolLevelClaimset, ReadOnlyClaimset, Active, RequirementYear, ProfileId, PlainEnglish) 
ON Target.ClaimSetName = Source.ClaimSetName 
WHEN MATCHED THEN 
UPDATE SET
	PlainEnglish = Source.PlainEnglish, 
	LogicalDomain = Source.LogicalDomain,
	ClaimsetTypeId = Source.ClaimsetTypeId, 
	PrimarySis = Source.PrimarySis,
	PublicClaimset = Source.PublicClaimset,
	ChoiceClaimset = Source.ChoiceClaimset,
	SchoolLevelClaimset = Source.SchoolLevelClaimset,
	ReadOnlyClaimset = Source.ReadOnlyClaimset,
	Active = Source.Active,
	RequirementYear = Source.RequirementYear,
	ProfileId = Source.ProfileId
WHEN NOT MATCHED BY TARGET THEN 
	INSERT (ClaimSetName, PlainEnglish, LogicalDomain,  ClaimsetTypeId, 
	PrimarySis, PublicClaimset, ChoiceClaimset, SchoolLevelClaimset, ReadOnlyClaimset, Active, RequirementYear, ProfileId) 
	VALUES (ClaimSetName, PlainEnglish, LogicalDomain,  ClaimsetTypeId, 
	PrimarySis, PublicClaimset, ChoiceClaimset, SchoolLevelClaimset, ReadOnlyClaimset, Active, RequirementYear, ProfileId)
WHEN NOT MATCHED BY SOURCE THEN 
DELETE;

