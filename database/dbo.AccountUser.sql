USE [Tada]
GO

/****** オブジェクト: Table [dbo].[AccountUser] スクリプト日付: 2023/08/01 15:48:02 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[AccountUser] (
    [Id]           INT           IDENTITY (1, 1) NOT NULL,
    [Name]         VARCHAR (50)  NULL,
    [EMail]        VARCHAR (200) NOT NULL,
    [Password]     VARCHAR (255) NOT NULL,
    [Token]        VARCHAR (255) NOT NULL,
    [Role]         VARCHAR (50)  NOT NULL,
    [CreateUserId] INT           NOT NULL,
    [CreateDate]   DATETIME      NOT NULL,
    [UpdateUserId] INT           NULL,
    [UpdateDate]   DATETIME      NULL
);


