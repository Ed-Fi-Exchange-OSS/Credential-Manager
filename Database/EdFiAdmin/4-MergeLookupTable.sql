SET IDENTITY_INSERT [dbo].[Lookup] ON 

MERGE INTO [dbo].[Lookup] AS Target 
USING (VALUES 
	  (1, 'AgreementType', 'Agency', 'Agency', 1, 1)
	 ,(2, 'AgreementType', 'Vendor', 'Vendor', 2, 1)
	 ,(3, 'SiteContentType', 'AgencyAgreement', 'AgencyAgreement', 1, 1)
	 ,(4, 'SiteContentType', 'VendorAgreement', 'VendorAgreement', 2, 1)
	 ,(5, 'AgreementType', 'VendorSubscription', 'VendorSubscription', 3, 1)
	 ,(6, 'SiteContentType', 'VendorSubscriptionAgreement', 'VendorSubscriptionAgreement', 3, 1)
) 
AS Source (Id, Type, Description, Code, SortOrder, Active) 
ON Target.Id = Source.Id 
WHEN MATCHED THEN 
UPDATE SET
	Type = Source.Type, 
	Description = Source.Description,
	Code = Source.Code,
	SortOrder = Source.SortOrder,
	Active = Source.Active
WHEN NOT MATCHED BY TARGET THEN 
	INSERT (Id, Type, Description, Code, SortOrder, Active) 
	VALUES (Id, Type, Description, Code, SortOrder, Active)
WHEN NOT MATCHED BY SOURCE THEN 
DELETE;

SET IDENTITY_INSERT [dbo].[Lookup] OFF 