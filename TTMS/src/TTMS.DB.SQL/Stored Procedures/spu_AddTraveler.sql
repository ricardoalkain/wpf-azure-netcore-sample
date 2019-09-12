
CREATE PROCEDURE dbo.spu_AddTraveler
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
    SET @id = ISNULL(@id, NEWID());

    INSERT INTO dbo.Traveler(
        [Id],
        [Type],
        [Name],
        [Status],
        [Alias],
        [Picture],
        [BirthDate],
        [BirthTimeline],
        [BirthLocation],
        [LastDateTime],
        [LastTimeline],
        [LastLocation],
        [TMModel],
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
        @tMModel,
        @skills)

    EXEC dbo.spu_GetTraveler @id;
END