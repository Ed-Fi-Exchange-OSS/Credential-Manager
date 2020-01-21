CREATE VIEW [dbo].[AsmUser]
	AS 
	SELECT 
		WamsId,
		FirstName,
		LastName,
		UserName
	FROM  
	(values (1234567890, 'demo', 'user', 'demouser'),
	(1122334455, 'demo2', 'user', 'demouser2')) as ExternalUserTable
	(WamsId,
		FirstName,
		LastName,
		UserName)

	WHERE	WamsId IS NOT NULL
	AND		UserName IS NOT NULL
GO

