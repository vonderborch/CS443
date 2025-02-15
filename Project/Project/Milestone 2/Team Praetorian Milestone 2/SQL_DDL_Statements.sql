USE [master]
GO
CREATE DATABASE [CPTS451PROJECT]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'CPTS451PROJECT', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL11.MSSQLSERVER\MSSQL\DATA\CPTS451PROJECT.mdf' , SIZE = 375808KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'CPTS451PROJECT_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL11.MSSQLSERVER\MSSQL\DATA\CPTS451PROJECT_log.ldf' , SIZE = 32448KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
GO
ALTER DATABASE [CPTS451PROJECT] SET COMPATIBILITY_LEVEL = 110
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [CPTS451PROJECT].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
USE [CPTS451PROJECT]
GO
CREATE TABLE [dbo].[Business] (
    [b_id]      CHAR (64)     NOT NULL,
    [name]      VARCHAR (512) NOT NULL,
    [stars]     FLOAT (53)    DEFAULT ((0)) NOT NULL,
    [r_count]   INT           DEFAULT ((0)) NOT NULL,
    [is_open]   BIT           NOT NULL,
    [address]   VARCHAR (512) NOT NULL,
    [city]      CHAR (16)     NOT NULL,
    [us_state]  CHAR (16)     NOT NULL,
    [latitude]  FLOAT (53)    NOT NULL,
    [longitude] FLOAT (53)    NOT NULL,
    PRIMARY KEY CLUSTERED ([b_id] ASC)
) ON [PRIMARY]
GO
CREATE TABLE [dbo].[Category] (
    [b_id] CHAR (64)     NOT NULL,
    [name] VARCHAR (256) NOT NULL,
    PRIMARY KEY CLUSTERED ([b_id] ASC, [name] ASC),
    FOREIGN KEY ([b_id]) REFERENCES [dbo].[Business] ([b_id])
) ON [PRIMARY]
GO
CREATE TABLE [dbo].[CheckIn] (
    [b_id]  CHAR (64) NOT NULL,
    [time]  CHAR (6)  NOT NULL,
    [count] INT       DEFAULT ((0)) NOT NULL,
    PRIMARY KEY CLUSTERED ([b_id] ASC, [time] ASC),
    FOREIGN KEY ([b_id]) REFERENCES [dbo].[Business] ([b_id])
) ON [PRIMARY]
GO
CREATE TABLE [dbo].[Users] (
    [u_id]        CHAR (64)     NOT NULL,
    [name]        VARCHAR (512) NOT NULL,
    [stars]       FLOAT (53)    DEFAULT ((0)) NOT NULL,
    [r_count]     INT           DEFAULT ((0)) NOT NULL,
    [funnyVotes]  INT           DEFAULT ((0)) NOT NULL,
    [usefulVotes] INT           DEFAULT ((0)) NOT NULL,
    [coolVotes]   INT           DEFAULT ((0)) NOT NULL,
    PRIMARY KEY CLUSTERED ([u_id] ASC)
) ON [PRIMARY]
GO
CREATE TABLE [dbo].[Reviews] (
    [r_id]        CHAR (64)     NOT NULL,
    [u_id]        CHAR (64)     NOT NULL,
    [stars]       FLOAT (53)    NOT NULL,
    [text]        VARCHAR (MAX) NOT NULL,
    [date]        DATE          NOT NULL,
    [funnyVotes]  INT           DEFAULT ((0)) NOT NULL,
    [usefulVotes] INT           DEFAULT ((0)) NOT NULL,
    [coolVotes]   INT           DEFAULT ((0)) NOT NULL,
    PRIMARY KEY CLUSTERED ([r_id] ASC),
    FOREIGN KEY ([u_id]) REFERENCES [dbo].[Users] ([u_id])
) ON [PRIMARY]
GO