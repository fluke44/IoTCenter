USE [IoTCenter]
GO

CREATE TABLE [devices].[Device](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](255) NOT NULL,
	[Mac] [nvarchar](17) NOT NULL,
	[Ip] [nvarchar](15) NOT NULL,
	[Type] [nvarchar](100) NOT NULL,
	[Registered] [bit] NOT NULL,
	[DateRegistered] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [devices].[Device] ADD  DEFAULT ((0)) FOR [Registered]
GO


