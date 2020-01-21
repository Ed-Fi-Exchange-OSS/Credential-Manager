CREATE TABLE [dbo].[AuditEntry] (
    [AuditEntryId]		INT NOT NULL IDENTITY,
    [EntityTypeName]	VARCHAR(255) NOT NULL,
    [State]				INT NOT NULL,
    [StateName]			VARCHAR(255) NOT NULL,
    [CreatedBy]			VARCHAR(255) NOT NULL,
    [CreatedDate]		DATETIME2 NOT NULL,
    CONSTRAINT [PK_AuditEntry] PRIMARY KEY ([AuditEntryId])
)
GO