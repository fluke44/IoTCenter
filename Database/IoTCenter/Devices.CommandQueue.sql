USE [IoTCenter]
GO
CREATE TABLE [devices].[CommandQueue](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[DeviceId] [int] NOT NULL,
	[Command] [nvarchar](50) NOT NULL,
	[Url] [nvarchar](1000) NULL,
	[Status] [nvarchar](50) NOT NULL,
	[DateAdded] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [devices].[CommandQueue]  WITH CHECK ADD FOREIGN KEY([DeviceId])
REFERENCES [devices].[Device] ([Id])
GO


