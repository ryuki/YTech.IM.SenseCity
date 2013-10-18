USE [DB_IM_SENSECITY]
GO
/****** Object:  Table [dbo].[M_ACCOUNT_REF]    Script Date: 10/19/2013 02:32:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[M_ACCOUNT_REF](
	[ACCOUNT_REF_ID] [nvarchar](50) NOT NULL,
	[REFERENCE_TABLE] [nvarchar](50) NOT NULL,
	[REFERENCE_TYPE] [nvarchar](50) NOT NULL,
	[REFERENCE_ID] [nvarchar](50) NOT NULL,
	[ACCOUNT_ID] [nvarchar](50) NOT NULL,
	[DATA_STATUS] [nvarchar](50) NULL,
	[CREATED_BY] [nvarchar](50) NULL,
	[CREATED_DATE] [datetime] NULL,
	[MODIFIED_BY] [nvarchar](50) NULL,
	[MODIFIED_DATE] [datetime] NULL,
	[ROW_VERSION] [timestamp] NULL,
 CONSTRAINT [PK_M_ACCOUNT_REF] PRIMARY KEY CLUSTERED 
(
	[ACCOUNT_REF_ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[M_ACCOUNT_REF]  WITH CHECK ADD  CONSTRAINT [FK_M_ACCOUNT_REF_M_ACCOUNT] FOREIGN KEY([ACCOUNT_ID])
REFERENCES [dbo].[M_ACCOUNT] ([ACCOUNT_ID])
GO
ALTER TABLE [dbo].[M_ACCOUNT_REF] CHECK CONSTRAINT [FK_M_ACCOUNT_REF_M_ACCOUNT]
GO
