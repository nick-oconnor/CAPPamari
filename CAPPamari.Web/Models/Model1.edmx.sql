
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 10/19/2014 18:59:29
-- Generated from EDMX file: C:\Users\ngpitt\Documents\GitHub\CAPPamari\CAPPamari.Web\Models\Model1.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [Nicholas];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_CAPPReports_ApplicationUser]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CAPPReports] DROP CONSTRAINT [FK_CAPPReports_ApplicationUser];
GO
IF OBJECT_ID(N'[dbo].[FK_UserSession_ApplicationUser]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[UserSessions] DROP CONSTRAINT [FK_UserSession_ApplicationUser];
GO
IF OBJECT_ID(N'[dbo].[FK_Advises_Advisor]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Advises] DROP CONSTRAINT [FK_Advises_Advisor];
GO
IF OBJECT_ID(N'[dbo].[FK_Advises_ApplicationUser]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Advises] DROP CONSTRAINT [FK_Advises_ApplicationUser];
GO
IF OBJECT_ID(N'[dbo].[FK_HasA_CAPPReport]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[HasA] DROP CONSTRAINT [FK_HasA_CAPPReport];
GO
IF OBJECT_ID(N'[dbo].[FK_HasA_RequirementSet]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[HasA] DROP CONSTRAINT [FK_HasA_RequirementSet];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[Advisors]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Advisors];
GO
IF OBJECT_ID(N'[dbo].[ApplicationUsers]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ApplicationUsers];
GO
IF OBJECT_ID(N'[dbo].[CAPPReports]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CAPPReports];
GO
IF OBJECT_ID(N'[dbo].[Courses]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Courses];
GO
IF OBJECT_ID(N'[dbo].[UserSessions]', 'U') IS NOT NULL
    DROP TABLE [dbo].[UserSessions];
GO
IF OBJECT_ID(N'[dbo].[RequirementSets]', 'U') IS NOT NULL
    DROP TABLE [dbo].[RequirementSets];
GO
IF OBJECT_ID(N'[dbo].[Advises]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Advises];
GO
IF OBJECT_ID(N'[dbo].[HasA]', 'U') IS NOT NULL
    DROP TABLE [dbo].[HasA];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'Advisors'
CREATE TABLE [dbo].[Advisors] (
    [AdvisorID] int IDENTITY(1,1) NOT NULL,
    [Name] varchar(50)  NOT NULL,
    [EMailAddress] varchar(50)  NOT NULL
);
GO

-- Creating table 'ApplicationUsers'
CREATE TABLE [dbo].[ApplicationUsers] (
    [UserName] varchar(25)  NOT NULL,
    [Password] varchar(25)  NOT NULL,
    [Major] varchar(50)  NOT NULL
);
GO

-- Creating table 'CAPPReports'
CREATE TABLE [dbo].[CAPPReports] (
    [ReportID] int IDENTITY(1,1) NOT NULL,
    [UserName] varchar(25)  NOT NULL,
    [Name] varchar(25)  NOT NULL
);
GO

-- Creating table 'Courses'
CREATE TABLE [dbo].[Courses] (
    [CourseID] int IDENTITY(1,1) NOT NULL,
    [Department] nvarchar(max)  NOT NULL,
    [Number] nvarchar(max)  NOT NULL,
    [Semester] nvarchar(max)  NOT NULL,
    [PassNC] nvarchar(max)  NOT NULL,
    [Grade] nvarchar(max)  NOT NULL,
    [Credits] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'UserSessions'
CREATE TABLE [dbo].[UserSessions] (
    [SessionID] int IDENTITY(1,1) NOT NULL,
    [UserName] varchar(25)  NOT NULL,
    [Expiration] datetime  NOT NULL
);
GO

-- Creating table 'RequirementSets'
CREATE TABLE [dbo].[RequirementSets] (
    [RequirementSetID] int IDENTITY(1,1) NOT NULL,
    [Credits] nvarchar(max)  NOT NULL,
    [PassNCCredits] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'Advises'
CREATE TABLE [dbo].[Advises] (
    [Advisors_AdvisorID] int  NOT NULL,
    [ApplicationUsers_UserName] varchar(25)  NOT NULL
);
GO

-- Creating table 'HasA'
CREATE TABLE [dbo].[HasA] (
    [CAPPReports_ReportID] int  NOT NULL,
    [RequirementSets_RequirementSetID] int  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [AdvisorID] in table 'Advisors'
ALTER TABLE [dbo].[Advisors]
ADD CONSTRAINT [PK_Advisors]
    PRIMARY KEY CLUSTERED ([AdvisorID] ASC);
GO

-- Creating primary key on [UserName] in table 'ApplicationUsers'
ALTER TABLE [dbo].[ApplicationUsers]
ADD CONSTRAINT [PK_ApplicationUsers]
    PRIMARY KEY CLUSTERED ([UserName] ASC);
GO

-- Creating primary key on [ReportID] in table 'CAPPReports'
ALTER TABLE [dbo].[CAPPReports]
ADD CONSTRAINT [PK_CAPPReports]
    PRIMARY KEY CLUSTERED ([ReportID] ASC);
GO

-- Creating primary key on [CourseID] in table 'Courses'
ALTER TABLE [dbo].[Courses]
ADD CONSTRAINT [PK_Courses]
    PRIMARY KEY CLUSTERED ([CourseID] ASC);
GO

-- Creating primary key on [SessionID] in table 'UserSessions'
ALTER TABLE [dbo].[UserSessions]
ADD CONSTRAINT [PK_UserSessions]
    PRIMARY KEY CLUSTERED ([SessionID] ASC);
GO

-- Creating primary key on [RequirementSetID] in table 'RequirementSets'
ALTER TABLE [dbo].[RequirementSets]
ADD CONSTRAINT [PK_RequirementSets]
    PRIMARY KEY CLUSTERED ([RequirementSetID] ASC);
GO

-- Creating primary key on [Advisors_AdvisorID], [ApplicationUsers_UserName] in table 'Advises'
ALTER TABLE [dbo].[Advises]
ADD CONSTRAINT [PK_Advises]
    PRIMARY KEY CLUSTERED ([Advisors_AdvisorID], [ApplicationUsers_UserName] ASC);
GO

-- Creating primary key on [CAPPReports_ReportID], [RequirementSets_RequirementSetID] in table 'HasA'
ALTER TABLE [dbo].[HasA]
ADD CONSTRAINT [PK_HasA]
    PRIMARY KEY CLUSTERED ([CAPPReports_ReportID], [RequirementSets_RequirementSetID] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [UserName] in table 'CAPPReports'
ALTER TABLE [dbo].[CAPPReports]
ADD CONSTRAINT [FK_CAPPReports_ApplicationUser]
    FOREIGN KEY ([UserName])
    REFERENCES [dbo].[ApplicationUsers]
        ([UserName])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_CAPPReports_ApplicationUser'
CREATE INDEX [IX_FK_CAPPReports_ApplicationUser]
ON [dbo].[CAPPReports]
    ([UserName]);
GO

-- Creating foreign key on [UserName] in table 'UserSessions'
ALTER TABLE [dbo].[UserSessions]
ADD CONSTRAINT [FK_UserSession_ApplicationUser]
    FOREIGN KEY ([UserName])
    REFERENCES [dbo].[ApplicationUsers]
        ([UserName])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_UserSession_ApplicationUser'
CREATE INDEX [IX_FK_UserSession_ApplicationUser]
ON [dbo].[UserSessions]
    ([UserName]);
GO

-- Creating foreign key on [Advisors_AdvisorID] in table 'Advises'
ALTER TABLE [dbo].[Advises]
ADD CONSTRAINT [FK_Advises_Advisor]
    FOREIGN KEY ([Advisors_AdvisorID])
    REFERENCES [dbo].[Advisors]
        ([AdvisorID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [ApplicationUsers_UserName] in table 'Advises'
ALTER TABLE [dbo].[Advises]
ADD CONSTRAINT [FK_Advises_ApplicationUser]
    FOREIGN KEY ([ApplicationUsers_UserName])
    REFERENCES [dbo].[ApplicationUsers]
        ([UserName])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_Advises_ApplicationUser'
CREATE INDEX [IX_FK_Advises_ApplicationUser]
ON [dbo].[Advises]
    ([ApplicationUsers_UserName]);
GO

-- Creating foreign key on [CAPPReports_ReportID] in table 'HasA'
ALTER TABLE [dbo].[HasA]
ADD CONSTRAINT [FK_HasA_CAPPReport]
    FOREIGN KEY ([CAPPReports_ReportID])
    REFERENCES [dbo].[CAPPReports]
        ([ReportID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [RequirementSets_RequirementSetID] in table 'HasA'
ALTER TABLE [dbo].[HasA]
ADD CONSTRAINT [FK_HasA_RequirementSet]
    FOREIGN KEY ([RequirementSets_RequirementSetID])
    REFERENCES [dbo].[RequirementSets]
        ([RequirementSetID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_HasA_RequirementSet'
CREATE INDEX [IX_FK_HasA_RequirementSet]
ON [dbo].[HasA]
    ([RequirementSets_RequirementSetID]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------