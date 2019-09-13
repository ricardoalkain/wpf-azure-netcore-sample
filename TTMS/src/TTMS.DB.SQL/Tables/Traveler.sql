CREATE TABLE [dbo].[Traveler] (
    [Id]              UNIQUEIDENTIFIER NOT NULL,
    [Type]            INT              NULL,
    [Name]            NVARCHAR (100)   NOT NULL,
    [Status]          INT              NULL,
    [Alias]           NVARCHAR (20)    NOT NULL,
    [Picture]         VARBINARY (MAX)  NULL,
    [BirthDate]       DATETIME2 (0)    NULL,
    [BirthTimelineId] INT              NULL,
    [BirthLocation]   NVARCHAR (50)    NULL,
    [LastDateTime]    DATETIME2 (7)    NULL,
    [LastTimelineId]  INT              NULL,
    [LastLocation]    NVARCHAR (50)    NULL,
    [DeviceModel]     INT              NULL,
    [Skills]          NVARCHAR (4000)  NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

