USE [Tada]
GO

/****** オブジェクト: Table [dbo].[BalanceSheet] スクリプト日付: 2023/08/01 15:47:44 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[BalanceSheet] (
    [ProjectId]    INT           NOT NULL,
    [Seq]          INT           NOT NULL,
    [BalanceDate]  DATETIME      NOT NULL,
    [BalanceType]  INT           NOT NULL,
    [SubjectName]  VARCHAR (100) NOT NULL,
    [Amount]       INT           NOT NULL,
    [IsExpense]    BIT           NOT NULL,
    [Note]         VARCHAR (MAX) NULL,
    [CreateUserId] INT           NULL,
    [CreateDate]   DATETIME      NULL,
    [UpdateUserId] INT           NULL,
    [UpdateDate]   DATETIME      NULL
);




