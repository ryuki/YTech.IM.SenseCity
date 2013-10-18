USE [DB_IM_SENSECITY]
GO
/****** Object:  Table [dbo].[T_SHIFT]    Script Date: 10/19/2013 02:32:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[T_SHIFT](
	[SHIFT_ID] [nvarchar](50) NOT NULL,
	[EMPLOYEE_ID] [nvarchar](50) NULL,
	[SHIFT_DATE] [datetime] NULL,
	[SHIFT_NO] [int] NULL,
	[SHIFT_DATE_FROM] [datetime] NULL,
	[SHIFT_DATE_TO] [datetime] NULL,
	[SHIFT_STATUS] [nvarchar](50) NULL,
	[SHIFT_DESC] [nvarchar](max) NULL,
	[DATA_STATUS] [nvarchar](50) NULL,
	[CREATED_BY] [nvarchar](50) NULL,
	[CREATED_DATE] [datetime] NULL,
	[MODIFIED_BY] [nvarchar](50) NULL,
	[MODIFIED_DATE] [datetime] NULL,
	[ROW_VERSION] [timestamp] NULL,
 CONSTRAINT [PK_T_SHIFT] PRIMARY KEY CLUSTERED 
(
	[SHIFT_ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[T_SHIFT]  WITH CHECK ADD  CONSTRAINT [FK_T_SHIFT_M_EMPLOYEE] FOREIGN KEY([EMPLOYEE_ID])
REFERENCES [dbo].[M_EMPLOYEE] ([EMPLOYEE_ID])
GO
ALTER TABLE [dbo].[T_SHIFT] CHECK CONSTRAINT [FK_T_SHIFT_M_EMPLOYEE]
GO
