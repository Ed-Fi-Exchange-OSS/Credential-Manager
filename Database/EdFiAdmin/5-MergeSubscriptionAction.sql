MERGE INTO [dbo].[SubscriptionAction] AS Target 
USING (VALUES 
	  (1, 'Subscribe')
	 ,(2, 'Unsubscribe')
	 ,(3, 'AdminAdd')
	 ,(4, 'AdminDelete')
) 
AS Source ([SubscriptionActionId], [ActionName]) 
ON Target.[SubscriptionActionId] = Source.[SubscriptionActionId] 
WHEN MATCHED THEN 
UPDATE SET
	[ActionName] = Source.[ActionName]
WHEN NOT MATCHED BY TARGET THEN 
	INSERT ([SubscriptionActionId], [ActionName]) 
	VALUES ([SubscriptionActionId], [ActionName])
WHEN NOT MATCHED BY SOURCE THEN 
DELETE;

