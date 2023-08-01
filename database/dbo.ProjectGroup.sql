USE [Tada]
GO

/****** オブジェクト: Table [dbo].[ProjectGroup] スクリプト日付: 2023/08/01 15:47:27 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ProjectGroup] (
    [Id]           INT           NOT NULL,
    [Name]         VARCHAR (50)  NULL,
    [Description]  VARCHAR (200) NULL,
    [CreateUserId] INT           NULL,
    [CreateDate]   DATETIME      NULL,
    [UpdateUserId] INT           NULL,
    [UpdateDate]   DATETIME      NULL
);



