USE [Tada]
GO

/****** オブジェクト: Table [dbo].[ActivityReport] スクリプト日付: 2023/08/01 15:47:52 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ActivityReport] (
    [ProjectId]    INT           NOT NULL,
    [Seq]          INT           NOT NULL,
    [ReportDay]    DATETIME      NOT NULL,
    [ReportName]   VARCHAR (255) NULL,
    [FilePath]     VARCHAR (500) NULL,
    [CreateUserId] INT           NULL,
    [CreateDate]   DATETIME      NULL,
    [UpdateUserId] INT           NULL,
    [UpdateDate]   DATETIME      NULL
);


