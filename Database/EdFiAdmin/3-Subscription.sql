CREATE TABLE [dbo].[Subscription]
(
	[SubscriptionId]			INT NOT NULL IDENTITY,
	[SubscriptionActionId]		INT NOT NULL,
	[EducationOrganizationId]	INT NOT NULL,
	[VendorId]					INT NOT NULL,
	[WamsId]					VARCHAR (30) NOT NULL,
    [CreatedDate]				DATETIME2 NOT NULL,
    CONSTRAINT [PK_Subscription] PRIMARY KEY ([SubscriptionId]),
    CONSTRAINT [FK_Subscription_SubscriptionAction] FOREIGN KEY ([SubscriptionActionId]) REFERENCES [dbo].[SubscriptionAction] ([SubscriptionActionId]),
    CONSTRAINT [FK_Subscription_Vendor] FOREIGN KEY ([VendorId]) REFERENCES [dbo].[Vendors] ([VendorId]) 
)
