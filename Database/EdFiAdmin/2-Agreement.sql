CREATE TABLE [dbo].[Agreement] (
    [Id]              INT          IDENTITY (1, 1) NOT NULL,
    [AgreementTypeId] INT          NOT NULL,
    [WamsId]          CHAR (30)    NULL,
    [Agree]           BIT          CONSTRAINT [DF_Agreement_Agree] DEFAULT ((1)) NOT NULL,
    [ModifiedDate]    DATETIME     NOT NULL,
    [ModifierId]      VARCHAR (30) NOT NULL,
    [VendorId] INT NULL, 
	[EducationOrganizationId] [int] NULL,
    CONSTRAINT [PK_Agreement] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Agreement_Type] FOREIGN KEY ([AgreementTypeId]) REFERENCES [dbo].[Lookup] ([Id]),
    CONSTRAINT [FK_Agreement_Vendor] FOREIGN KEY ([VendorId]) REFERENCES [dbo].[Vendors] ([VendorId])
);

