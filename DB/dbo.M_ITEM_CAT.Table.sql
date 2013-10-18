USE [DB_IM_SENSECITY]
GO
/****** Object:  Table [dbo].[M_ITEM_CAT]    Script Date: 10/19/2013 02:32:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[M_ITEM_CAT](
	[ITEM_CAT_ID] [nvarchar](50) NOT NULL,
	[ITEM_CAT_NAME] [nvarchar](50) NOT NULL,
	[ITEM_CAT_DESC] [nvarchar](50) NULL,
	[DATA_STATUS] [nvarchar](50) NULL,
	[CREATED_BY] [nvarchar](50) NULL,
	[CREATED_DATE] [datetime] NULL,
	[MODIFIED_BY] [nvarchar](50) NULL,
	[MODIFIED_DATE] [datetime] NULL,
	[ROW_VERSION] [timestamp] NULL,
 CONSTRAINT [PK_M_ITEM_CAT] PRIMARY KEY CLUSTERED 
(
	[ITEM_CAT_ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
