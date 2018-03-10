USE [IoTCenter]
GO

CREATE TABLE [devices].[DataLog](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[DeviceId] [int] NULL,
	[Data] [nvarchar](max) NULL,
	[DateLogged] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

ALTER TABLE [devices].[DataLog]  WITH CHECK ADD FOREIGN KEY([DeviceId])
REFERENCES [devices].[Device] ([Id])
GO


