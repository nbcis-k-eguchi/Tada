USE [Tada]
GO

/****** オブジェクト: Table [dbo].[ProjectMember] スクリプト日付: 2023/08/01 15:47:16 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ProjectMember] (
    [ProjectId]       INT           NOT NULL,
    [Seq]             INT           NOT NULL,
    [EmployeeNumber]  INT           NOT NULL,
    [EMail]           VARCHAR (255) NULL,
    [Password]        VARCHAR (255) NULL,
    [Name]            VARCHAR (30)  NULL,
    [Birthday]        DATE          NULL,
    [Position]        VARCHAR (255) NULL,
    [JoiningDate]     DATETIME      NULL,
    [ResignationDate] DATETIME      NULL,
    [IsLock]          BIT           NULL,
    [CreateUserId]    INT           NULL,
    [CreateDate]      DATETIME      NULL,
    [UpdateUserId]    INT           NULL,
    [UpdateDate]      DATETIME      NULL
);




