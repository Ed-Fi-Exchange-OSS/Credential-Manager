SET IDENTITY_INSERT [dbo].[SiteContent] ON 

MERGE INTO [dbo].[SiteContent] AS Target 
USING (VALUES 
	  (1, 3, 'Verbage for Education Organization agreement with SIS vendor.')
	 ,(2, 4, 'Verbage for SIS vendor agreement with Education Organization.')
	 ,(3, 6, 'Verbage for VendorSubscriptionAgreement.')
) 
AS Source (Id, SiteContentTypeId, Body) 
ON Target.Id = Source.Id 
WHEN MATCHED THEN 
UPDATE SET
	SiteContentTypeId = Source.SiteContentTypeId, 
	Body = Source.Body
WHEN NOT MATCHED BY TARGET THEN 
	INSERT (Id, SiteContentTypeId, Body) 
	VALUES (Id, SiteContentTypeId, Body)
WHEN NOT MATCHED BY SOURCE THEN 
DELETE;

SET IDENTITY_INSERT [dbo].[SiteContent] OFF 
