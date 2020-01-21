CREATE TABLE [dbo].[Lookup] (
    [Id]          INT           IDENTITY (1, 1) NOT NULL,
    [Type]        VARCHAR (50)  NOT NULL,
    [Description] VARCHAR (80)  NOT NULL,
    [Code]        VARCHAR (100) NULL,
    [SortOrder]   INT           NOT NULL,
    [Active]      BIT           CONSTRAINT [DF_Lookup_Active] DEFAULT ((1)) NOT NULL,
    CONSTRAINT [PK_Lookup] PRIMARY KEY CLUSTERED ([Id] ASC)
);

