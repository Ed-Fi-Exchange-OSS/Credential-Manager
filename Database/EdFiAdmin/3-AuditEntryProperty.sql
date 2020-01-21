CREATE TABLE [dbo].[AuditEntryProperty] (
    [AuditEntryPropertyId]		INT NOT NULL IDENTITY,
    [AuditEntryId]				INT NOT NULL,
    [PropertyName]				NVARCHAR(255),
    [OldValue]					NVARCHAR(max),
    [NewValue]					NVARCHAR(max),
    CONSTRAINT [PK_AuditEntryProperty] PRIMARY KEY ([AuditEntryPropertyId])
)
GO

CREATE INDEX [IX_AuditEntryProperty_AuditEntryId] ON [dbo].[AuditEntryProperty]([AuditEntryId])
GO

ALTER TABLE [dbo].[AuditEntryProperty] ADD CONSTRAINT [FK_AuditEntryProperty_AuditEntry_AuditEntryId] 
	FOREIGN KEY ([AuditEntryId]) REFERENCES [dbo].[AuditEntry] ([AuditEntryId])
	ON DELETE CASCADE
GO