CREATE TABLE [dbo].[ClaimsetDetails](
	[ClaimSetName] [nvarchar](255) NOT NULL,
	[PlainEnglish] [varchar](1000) NOT NULL,
	[LogicalDomain] [varchar](100) NOT NULL,
	[PrimarySis] [bit] NOT NULL,
	[PublicClaimset] [bit] NOT NULL,
	[ChoiceClaimset] [bit] NOT NULL,
	[SchoolLevelClaimset] [bit] NOT NULL,
	[ReadOnlyClaimset] [bit] NOT NULL,
	[Active] [bit] NOT NULL,
	[RequirementYear] [int] NULL,
	[ProfileId] [int] NULL,
	[ClaimsetTypeId] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[ClaimSetName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[ClaimsetDetails] ADD  DEFAULT ((0)) FOR [PrimarySis]
GO

ALTER TABLE [dbo].[ClaimsetDetails] ADD  DEFAULT ((0)) FOR [PublicClaimset]
GO

ALTER TABLE [dbo].[ClaimsetDetails] ADD  DEFAULT ((0)) FOR [ChoiceClaimset]
GO

ALTER TABLE [dbo].[ClaimsetDetails] ADD  DEFAULT ((0)) FOR [SchoolLevelClaimset]
GO

ALTER TABLE [dbo].[ClaimsetDetails] ADD  DEFAULT ((0)) FOR [BoundaryYearAware]
GO

ALTER TABLE [dbo].[ClaimsetDetails] ADD  DEFAULT ((0)) FOR [DisciplineBoundaryYearAware]
GO

ALTER TABLE [dbo].[ClaimsetDetails] ADD  DEFAULT ((0)) FOR [ReadOnlyClaimset]
GO

ALTER TABLE [dbo].[ClaimsetDetails] ADD  DEFAULT ((0)) FOR [Active]
GO

ALTER TABLE [dbo].[ClaimsetDetails]  WITH CHECK ADD  CONSTRAINT [FK_ClaimsetDetail_ClaimsetType] FOREIGN KEY([ClaimsetTypeId])
REFERENCES [dbo].[ClaimsetType] ([Id])
GO

ALTER TABLE [dbo].[ClaimsetDetails] CHECK CONSTRAINT [FK_ClaimsetDetail_ClaimsetType]
GO

ALTER TABLE [dbo].[ClaimsetDetails]  WITH CHECK ADD  CONSTRAINT [FK_ClaimsetDetail_Profile] FOREIGN KEY([ProfileId])
REFERENCES [dbo].[Profiles] ([ProfileId])
GO

ALTER TABLE [dbo].[ClaimsetDetails] CHECK CONSTRAINT [FK_ClaimsetDetail_Profile]
GO

