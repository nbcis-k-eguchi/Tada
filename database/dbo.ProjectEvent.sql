USE [Tada]
GO

/****** オブジェクト: Table [dbo].[ProjectEvent] スクリプト日付: 2023/08/01 15:47:35 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ProjectEvent] (
    [ProjectId]    INT           NOT NULL,
    [Seq]          INT           NOT NULL,
    [EventDay]     DATE          NOT NULL,
    [EventAdapt]   INT           NOT NULL,
    [StartTime]    DATETIME      NULL,
    [EndTime]      DATETIME      NULL,
    [Description]  VARCHAR (MAX) NULL,
    [Location]     VARCHAR (200) NULL,
    [MemberCount]  INT           NULL,
    [CreateUserId] INT           NULL,
    [CreateDate]   DATETIME      NULL,
    [UpdateUserId] INT           NULL,
    [UpdateDate]   DATETIME      NULL
);


