CREATE PROCEDURE dbo.spu_AddOrUpdateTraveler
    @id             UNIQUEIDENTIFIER = NULL,
    @type           INT,
    @name           NVARCHAR(100),
    @status         INT,
    @alias          NVARCHAR(20),
    @picture        VARBINARY(MAX)  = NULL,
    @birthDate      DATETIME2       = NULL,
    @birthTimeline  INT,
    @birthLocation  NVARCHAR(50)    = NULL,
    @lastDateTime   DATETIME2       = NULL,
    @lastTimeline   INT,
    @lastLocation   NVARCHAR(50)    = NULL,
    @deviceModel    INT,
    @skills         NVARCHAR(4000)  = NULL
AS
BEGIN
    IF EXISTS(SELECT 1 FROM dbo.Traveler WHERE Id = @id)
    BEGIN
        UPDATE dbo.Traveler
        SET [Type]            = @type,
            [Name]            = @name,
            [Status]          = @status,
            [Alias]           = @alias,
            [Picture]         = @picture,
            [BirthDate]       = @birthDate,
            [BirthTimelineId] = @birthTimeline,
            [BirthLocation]   = @birthLocation,
            [LastDateTime]    = @lastDateTime,
            [LastTimelineId]  = @lastTimeline,
            [LastLocation]    = @lastLocation,
            [DeviceModel]     = @deviceModel,
            [Skills]          = @skills
        WHERE [Id] = @id
    END
    ELSE
    BEGIN
        RAISERROR('There is no traveler with this ID to be updated.', 16, 1);
    END
END
