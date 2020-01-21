CREATE TABLE [dbo].[SiteContent] (
    [Id]                INT            IDENTITY (1, 1) NOT NULL,
    [SiteContentTypeId] INT            NOT NULL,
    [Body]              VARCHAR (8000) NOT NULL,
    CONSTRAINT [PK_SiteContent_Id] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_SiteContent_Type] FOREIGN KEY ([SiteContentTypeId]) REFERENCES [dbo].[Lookup] ([Id])
);

