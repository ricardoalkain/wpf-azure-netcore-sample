CREATE PROCEDURE dbo.spu_InserTraveler
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
    SET @id = ISNULL(@id, NEWID());

    IF EXISTS(SELECT 1 FROM dbo.Traveler WHERE Id = @id)
    BEGIN
        RAISERROR('There is already a traveler with this ID.', 16, 1)
    END
    ELSE
    BEGIN
        INSERT INTO dbo.Traveler(
            [Id],
            [Type],
            [Name],
            [Status],
            [Alias],
            [Picture],
            [BirthDate],
            [BirthTimelineId],
            [BirthLocation],
            [LastDateTime],
            [LastTimelineId],
            [LastLocation],
            [DeviceModel],
            [Skills])
         VALUES(
            @id,
            @type,
            @name,
            @status,
            @alias,
            @picture,
            @birthDate,
            @birthTimeline,
            @birthLocation,
            @lastDateTime,
            @lastTimeline,
            @lastLocation,
            @deviceModel,
            @skills)
    END

    EXEC spu_GetTraveler @id;
END
