MERGE INTO [dbo].[ClaimsetType] AS Target 
USING (VALUES 
	  (1, 'Public SIS')
	 ,(2, 'Choice SIS')
	 ,(3, 'SPED')
	 ,(4, 'DISC')
	 ,(5, 'Finance')
	 ,(6, 'ReadOnly')
	 ,(7, 'Assessment')
) 
AS Source (Id, Name) 
ON Target.Id = Source.Id 
WHEN MATCHED THEN 
UPDATE SET
	Name = Source.Name
WHEN NOT MATCHED BY TARGET THEN 
	INSERT (Id, Name) 
	VALUES (Id, Name)
WHEN NOT MATCHED BY SOURCE THEN 
DELETE;