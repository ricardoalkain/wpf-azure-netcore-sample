CREATE PROCEDURE dbo.spu_UpdateTraveler
    @id             UNIQUEIDENTIFIER,
    @type           INT,
    @name           NVARCHAR(100),
    @status         INT,
    @alias          NVARCHAR(20),
    @picture        VARBINARY(MAX),
    @birthDate      DATE,
    @birthTimeline  INT,
    @birthLocation  NVARCHAR(50),
    @lastDateTime   DATETIME,
    @lastTimeline   INT,
    @lastLocation   NVARCHAR(50),
    @tMModel        INT,
    @skills         NVARCHAR(4000)
AS
BEGIN
    UPDATE dbo.Traveler
    SET [Type]          = @type,
        [Name]          = @name,
        [Status]        = @status,
        [Alias]         = @alias,
        [Picture]       = @picture,
        [BirthDate]     = @birthDate,
        [BirthTimeline] = @birthTimeline,
        [BirthLocation] = @birthLocation,
        [LastDateTime]  = @lastDateTime,
        [LastTimeline]  = @lastTimeline,
        [LastLocation]  = @lastLocation,
        [TMModel]       = @tMModel,
        [Skills]        = @skills
    WHERE [Id] = @id

    RETURN @@ROWCOUNT;
END