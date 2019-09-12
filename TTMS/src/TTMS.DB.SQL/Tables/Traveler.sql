CREATE TABLE [dbo].[Traveler] (
    [Id]            UNIQUEIDENTIFIER NOT NULL,
    [Type]          INT              NULL,
    [Name]          NVARCHAR (100)   NOT NULL,
    [Status]        INT              NULL,
    [Alias]         NVARCHAR (20)    NOT NULL,
    [Picture]       VARBINARY (MAX)  NULL,
    [BirthDate]     DATE             NULL,
    [BirthTimeline] INT              NULL,
    [BirthLocation] NVARCHAR (50)    NULL,
    [LastDateTime]  DATETIME         NULL,
    [LastTimeline]  INT              NULL,
    [LastLocation]  NVARCHAR (50)    NULL,
    [TMModel]       INT              NULL,
    [Skills]        NVARCHAR (4000)  NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

