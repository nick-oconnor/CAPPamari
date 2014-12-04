
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 12/04/2014 14:52:14
-- Generated from EDMX file: C:\Users\ngpitt\Documents\GitHub\CAPPamari\CAPPamari.Web\Models\CAPPamari.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [CAPPamariRelease];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK__CAPPRepor__CAPPR__0E6E26BF]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CAPPReportRequirement] DROP CONSTRAINT [FK__CAPPRepor__CAPPR__0E6E26BF];
GO
IF OBJECT_ID(N'[dbo].[FK__CAPPRepor__Requi__0F624AF8]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CAPPReportRequirement] DROP CONSTRAINT [FK__CAPPRepor__Requi__0F624AF8];
GO
IF OBJECT_ID(N'[dbo].[FK__Required__Requir__08B54D69]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Required] DROP CONSTRAINT [FK__Required__Requir__08B54D69];
GO
IF OBJECT_ID(N'[dbo].[FK__Required__Requir__09A971A2]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Required] DROP CONSTRAINT [FK__Required__Requir__09A971A2];
GO
IF OBJECT_ID(N'[dbo].[FK__Requireme__Cours__06CD04F7]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[RequirementFulfillment] DROP CONSTRAINT [FK__Requireme__Cours__06CD04F7];
GO
IF OBJECT_ID(N'[dbo].[FK__Requireme__Requi__05D8E0BE]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[RequirementFulfillment] DROP CONSTRAINT [FK__Requireme__Requi__05D8E0BE];
GO
IF OBJECT_ID(N'[dbo].[FK__Requireme__Requi__0B91BA14]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[RequirementSetRequirement] DROP CONSTRAINT [FK__Requireme__Requi__0B91BA14];
GO
IF OBJECT_ID(N'[dbo].[FK__Requireme__Requi__0C85DE4D]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[RequirementSetRequirement] DROP CONSTRAINT [FK__Requireme__Requi__0C85DE4D];
GO
IF OBJECT_ID(N'[dbo].[FK_AdvisorID]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Advises] DROP CONSTRAINT [FK_AdvisorID];
GO
IF OBJECT_ID(N'[dbo].[FK_ApplicationUser]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Advises] DROP CONSTRAINT [FK_ApplicationUser];
GO
IF OBJECT_ID(N'[dbo].[FK_CAPPReport_ApplicationUser]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CAPPReport] DROP CONSTRAINT [FK_CAPPReport_ApplicationUser];
GO
IF OBJECT_ID(N'[dbo].[FK_Course_RequirementSet]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Course] DROP CONSTRAINT [FK_Course_RequirementSet];
GO
IF OBJECT_ID(N'[dbo].[FK_RequirementSet_CAPPRerport]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[RequirementSet] DROP CONSTRAINT [FK_RequirementSet_CAPPRerport];
GO
IF OBJECT_ID(N'[dbo].[FK_UserSession_ApplicationUser]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[UserSession] DROP CONSTRAINT [FK_UserSession_ApplicationUser];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[Advises]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Advises];
GO
IF OBJECT_ID(N'[dbo].[Advisor]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Advisor];
GO
IF OBJECT_ID(N'[dbo].[ApplicationUser]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ApplicationUser];
GO
IF OBJECT_ID(N'[dbo].[CAPPReport]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CAPPReport];
GO
IF OBJECT_ID(N'[dbo].[CAPPReportRequirement]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CAPPReportRequirement];
GO
IF OBJECT_ID(N'[dbo].[Course]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Course];
GO
IF OBJECT_ID(N'[dbo].[CourseFulfillment]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CourseFulfillment];
GO
IF OBJECT_ID(N'[dbo].[Required]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Required];
GO
IF OBJECT_ID(N'[dbo].[Requirement]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Requirement];
GO
IF OBJECT_ID(N'[dbo].[RequirementFulfillment]', 'U') IS NOT NULL
    DROP TABLE [dbo].[RequirementFulfillment];
GO
IF OBJECT_ID(N'[dbo].[RequirementSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[RequirementSet];
GO
IF OBJECT_ID(N'[dbo].[RequirementSetRequirement]', 'U') IS NOT NULL
    DROP TABLE [dbo].[RequirementSetRequirement];
GO
IF OBJECT_ID(N'[dbo].[UserSession]', 'U') IS NOT NULL
    DROP TABLE [dbo].[UserSession];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'Advisors'
CREATE TABLE [dbo].[Advisors] (
    [AdvisorID] int IDENTITY(1,1) NOT NULL,
    [Name] varchar(50)  NOT NULL,
    [EmailAddress] varchar(50)  NOT NULL
);
GO

-- Creating table 'ApplicationUsers'
CREATE TABLE [dbo].[ApplicationUsers] (
    [Username] varchar(25)  NOT NULL,
    [Password] varchar(25)  NOT NULL,
    [Major] varchar(50)  NOT NULL
);
GO

-- Creating table 'CAPPReports'
CREATE TABLE [dbo].[CAPPReports] (
    [ReportID] int IDENTITY(1,1) NOT NULL,
    [Username] varchar(25)  NOT NULL,
    [Name] varchar(25)  NOT NULL
);
GO

-- Creating table 'Courses'
CREATE TABLE [dbo].[Courses] (
    [CourseID] int IDENTITY(1,1) NOT NULL,
    [Department] nvarchar(max)  NOT NULL,
    [Number] nvarchar(max)  NOT NULL,
    [Semester] nvarchar(max)  NOT NULL,
    [PassNC] bit  NOT NULL,
    [Grade] float  NOT NULL,
    [Credits] int  NOT NULL,
    [RequirementSetID] int  NOT NULL,
    [CommunicationIntensive] bit  NOT NULL
);
GO

-- Creating table 'CourseFulfillments'
CREATE TABLE [dbo].[CourseFulfillments] (
    [CourseFulfillmentID] int IDENTITY(1,1) NOT NULL,
    [DepartmentCode] varchar(4)  NOT NULL,
    [CourseNumber] varchar(4)  NOT NULL
);
GO

-- Creating table 'Requirements'
CREATE TABLE [dbo].[Requirements] (
    [RequirementID] int IDENTITY(1,1) NOT NULL,
    [CreditsNeeded] int  NOT NULL,
    [CommunicationIntensive] bit  NOT NULL,
    [Exclusion] bit  NOT NULL,
    [PassNoCreditCreditsAllowed] int  NOT NULL
);
GO

-- Creating table 'UserSessions'
CREATE TABLE [dbo].[UserSessions] (
    [SessionID] int IDENTITY(1,1) NOT NULL,
    [Username] varchar(25)  NOT NULL,
    [Expiration] datetime  NOT NULL
);
GO

-- Creating table 'RequirementSets'
CREATE TABLE [dbo].[RequirementSets] (
    [RequirementSetID] int IDENTITY(1,1) NOT NULL,
    [Credits] int  NOT NULL,
    [PassNCCredits] int  NOT NULL,
    [Name] varchar(50)  NOT NULL,
    [CAPPReportID] int  NOT NULL,
    [DepthRSR] bit  NOT NULL,
    [Description] varchar(4096)  NOT NULL
);
GO

-- Creating table 'CAPPReportRequirement'
CREATE TABLE [dbo].[CAPPReportRequirement] (
    [CAPPReports_ReportID] int  NOT NULL,
    [Requirements_RequirementID] int  NOT NULL
);
GO

-- Creating table 'RequirementFulfillment'
CREATE TABLE [dbo].[RequirementFulfillment] (
    [CourseFulfillments_CourseFulfillmentID] int  NOT NULL,
    [Requirements_RequirementID] int  NOT NULL
);
GO

-- Creating table 'Required'
CREATE TABLE [dbo].[Required] (
    [RequirementSets_RequirementSetID] int  NOT NULL,
    [Requirements_RequirementID] int  NOT NULL
);
GO

-- Creating table 'RequirementSetRequirement'
CREATE TABLE [dbo].[RequirementSetRequirement] (
    [RequirementSets1_RequirementSetID] int  NOT NULL,
    [RequirementSetRequirements_RequirementID] int  NOT NULL
);
GO

-- Creating table 'Advises'
CREATE TABLE [dbo].[Advises] (
    [Advisors_AdvisorID] int  NOT NULL,
    [ApplicationUsers_Username] varchar(25)  NOT NULL
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

-- Creating primary key on [Username] in table 'ApplicationUsers'
ALTER TABLE [dbo].[ApplicationUsers]
ADD CONSTRAINT [PK_ApplicationUsers]
    PRIMARY KEY CLUSTERED ([Username] ASC);
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

-- Creating primary key on [CourseFulfillmentID] in table 'CourseFulfillments'
ALTER TABLE [dbo].[CourseFulfillments]
ADD CONSTRAINT [PK_CourseFulfillments]
    PRIMARY KEY CLUSTERED ([CourseFulfillmentID] ASC);
GO

-- Creating primary key on [RequirementID] in table 'Requirements'
ALTER TABLE [dbo].[Requirements]
ADD CONSTRAINT [PK_Requirements]
    PRIMARY KEY CLUSTERED ([RequirementID] ASC);
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

-- Creating primary key on [CAPPReports_ReportID], [Requirements_RequirementID] in table 'CAPPReportRequirement'
ALTER TABLE [dbo].[CAPPReportRequirement]
ADD CONSTRAINT [PK_CAPPReportRequirement]
    PRIMARY KEY CLUSTERED ([CAPPReports_ReportID], [Requirements_RequirementID] ASC);
GO

-- Creating primary key on [CourseFulfillments_CourseFulfillmentID], [Requirements_RequirementID] in table 'RequirementFulfillment'
ALTER TABLE [dbo].[RequirementFulfillment]
ADD CONSTRAINT [PK_RequirementFulfillment]
    PRIMARY KEY CLUSTERED ([CourseFulfillments_CourseFulfillmentID], [Requirements_RequirementID] ASC);
GO

-- Creating primary key on [RequirementSets_RequirementSetID], [Requirements_RequirementID] in table 'Required'
ALTER TABLE [dbo].[Required]
ADD CONSTRAINT [PK_Required]
    PRIMARY KEY CLUSTERED ([RequirementSets_RequirementSetID], [Requirements_RequirementID] ASC);
GO

-- Creating primary key on [RequirementSets1_RequirementSetID], [RequirementSetRequirements_RequirementID] in table 'RequirementSetRequirement'
ALTER TABLE [dbo].[RequirementSetRequirement]
ADD CONSTRAINT [PK_RequirementSetRequirement]
    PRIMARY KEY CLUSTERED ([RequirementSets1_RequirementSetID], [RequirementSetRequirements_RequirementID] ASC);
GO

-- Creating primary key on [Advisors_AdvisorID], [ApplicationUsers_Username] in table 'Advises'
ALTER TABLE [dbo].[Advises]
ADD CONSTRAINT [PK_Advises]
    PRIMARY KEY CLUSTERED ([Advisors_AdvisorID], [ApplicationUsers_Username] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [Username] in table 'CAPPReports'
ALTER TABLE [dbo].[CAPPReports]
ADD CONSTRAINT [FK_CAPPReport_ApplicationUser]
    FOREIGN KEY ([Username])
    REFERENCES [dbo].[ApplicationUsers]
        ([Username])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_CAPPReport_ApplicationUser'
CREATE INDEX [IX_FK_CAPPReport_ApplicationUser]
ON [dbo].[CAPPReports]
    ([Username]);
GO

-- Creating foreign key on [Username] in table 'UserSessions'
ALTER TABLE [dbo].[UserSessions]
ADD CONSTRAINT [FK_UserSession_ApplicationUser]
    FOREIGN KEY ([Username])
    REFERENCES [dbo].[ApplicationUsers]
        ([Username])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_UserSession_ApplicationUser'
CREATE INDEX [IX_FK_UserSession_ApplicationUser]
ON [dbo].[UserSessions]
    ([Username]);
GO

-- Creating foreign key on [CAPPReports_ReportID] in table 'CAPPReportRequirement'
ALTER TABLE [dbo].[CAPPReportRequirement]
ADD CONSTRAINT [FK_CAPPReportRequirement_CAPPReport]
    FOREIGN KEY ([CAPPReports_ReportID])
    REFERENCES [dbo].[CAPPReports]
        ([ReportID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [Requirements_RequirementID] in table 'CAPPReportRequirement'
ALTER TABLE [dbo].[CAPPReportRequirement]
ADD CONSTRAINT [FK_CAPPReportRequirement_Requirement]
    FOREIGN KEY ([Requirements_RequirementID])
    REFERENCES [dbo].[Requirements]
        ([RequirementID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_CAPPReportRequirement_Requirement'
CREATE INDEX [IX_FK_CAPPReportRequirement_Requirement]
ON [dbo].[CAPPReportRequirement]
    ([Requirements_RequirementID]);
GO

-- Creating foreign key on [CourseFulfillments_CourseFulfillmentID] in table 'RequirementFulfillment'
ALTER TABLE [dbo].[RequirementFulfillment]
ADD CONSTRAINT [FK_RequirementFulfillment_CourseFulfillment]
    FOREIGN KEY ([CourseFulfillments_CourseFulfillmentID])
    REFERENCES [dbo].[CourseFulfillments]
        ([CourseFulfillmentID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [Requirements_RequirementID] in table 'RequirementFulfillment'
ALTER TABLE [dbo].[RequirementFulfillment]
ADD CONSTRAINT [FK_RequirementFulfillment_Requirement]
    FOREIGN KEY ([Requirements_RequirementID])
    REFERENCES [dbo].[Requirements]
        ([RequirementID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_RequirementFulfillment_Requirement'
CREATE INDEX [IX_FK_RequirementFulfillment_Requirement]
ON [dbo].[RequirementFulfillment]
    ([Requirements_RequirementID]);
GO

-- Creating foreign key on [CAPPReportID] in table 'RequirementSets'
ALTER TABLE [dbo].[RequirementSets]
ADD CONSTRAINT [FK_RequirementSet_CAPPRerport]
    FOREIGN KEY ([CAPPReportID])
    REFERENCES [dbo].[CAPPReports]
        ([ReportID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_RequirementSet_CAPPRerport'
CREATE INDEX [IX_FK_RequirementSet_CAPPRerport]
ON [dbo].[RequirementSets]
    ([CAPPReportID]);
GO

-- Creating foreign key on [RequirementSetID] in table 'Courses'
ALTER TABLE [dbo].[Courses]
ADD CONSTRAINT [FK_Course_RequirementSet]
    FOREIGN KEY ([RequirementSetID])
    REFERENCES [dbo].[RequirementSets]
        ([RequirementSetID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_Course_RequirementSet'
CREATE INDEX [IX_FK_Course_RequirementSet]
ON [dbo].[Courses]
    ([RequirementSetID]);
GO

-- Creating foreign key on [RequirementSets_RequirementSetID] in table 'Required'
ALTER TABLE [dbo].[Required]
ADD CONSTRAINT [FK_Required_RequirementSet]
    FOREIGN KEY ([RequirementSets_RequirementSetID])
    REFERENCES [dbo].[RequirementSets]
        ([RequirementSetID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [Requirements_RequirementID] in table 'Required'
ALTER TABLE [dbo].[Required]
ADD CONSTRAINT [FK_Required_Requirement]
    FOREIGN KEY ([Requirements_RequirementID])
    REFERENCES [dbo].[Requirements]
        ([RequirementID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_Required_Requirement'
CREATE INDEX [IX_FK_Required_Requirement]
ON [dbo].[Required]
    ([Requirements_RequirementID]);
GO

-- Creating foreign key on [RequirementSets1_RequirementSetID] in table 'RequirementSetRequirement'
ALTER TABLE [dbo].[RequirementSetRequirement]
ADD CONSTRAINT [FK_RequirementSetRequirement_RequirementSet]
    FOREIGN KEY ([RequirementSets1_RequirementSetID])
    REFERENCES [dbo].[RequirementSets]
        ([RequirementSetID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [RequirementSetRequirements_RequirementID] in table 'RequirementSetRequirement'
ALTER TABLE [dbo].[RequirementSetRequirement]
ADD CONSTRAINT [FK_RequirementSetRequirement_Requirement]
    FOREIGN KEY ([RequirementSetRequirements_RequirementID])
    REFERENCES [dbo].[Requirements]
        ([RequirementID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_RequirementSetRequirement_Requirement'
CREATE INDEX [IX_FK_RequirementSetRequirement_Requirement]
ON [dbo].[RequirementSetRequirement]
    ([RequirementSetRequirements_RequirementID]);
GO

-- Creating foreign key on [Advisors_AdvisorID] in table 'Advises'
ALTER TABLE [dbo].[Advises]
ADD CONSTRAINT [FK_Advises_Advisor]
    FOREIGN KEY ([Advisors_AdvisorID])
    REFERENCES [dbo].[Advisors]
        ([AdvisorID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [ApplicationUsers_Username] in table 'Advises'
ALTER TABLE [dbo].[Advises]
ADD CONSTRAINT [FK_Advises_ApplicationUser]
    FOREIGN KEY ([ApplicationUsers_Username])
    REFERENCES [dbo].[ApplicationUsers]
        ([Username])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_Advises_ApplicationUser'
CREATE INDEX [IX_FK_Advises_ApplicationUser]
ON [dbo].[Advises]
    ([ApplicationUsers_Username]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------