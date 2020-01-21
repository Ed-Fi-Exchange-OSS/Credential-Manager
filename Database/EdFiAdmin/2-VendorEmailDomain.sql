CREATE TABLE [dbo].[VendorEmailDomains](
	[VendorEmailDomainId] [int] IDENTITY(1,1) NOT NULL,
	[EmailDomain] [nvarchar](255) NOT NULL,
	[Vendor_VendorId] [int] NOT NULL,
 CONSTRAINT [PK_dbo.VendorEmailDomains] PRIMARY KEY CLUSTERED 
(
	[VendorEmailDomainId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[VendorEmailDomains]  WITH CHECK ADD  CONSTRAINT [FK_dbo.VendorEmailDomains_dbo.Vendors_Vendor_VendorId] FOREIGN KEY([Vendor_VendorId])
REFERENCES [dbo].[Vendors] ([VendorId])
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[VendorEmailDomains] CHECK CONSTRAINT [FK_dbo.VendorEmailDomains_dbo.Vendors_Vendor_VendorId]
GO

