CREATE PROCEDURE dbo.spu_GetAllTravelers
AS
BEGIN
    SELECT [Id], [Type], [Name], [Status], [Alias], [Picture], [BirthDate], [BirthTimeline],
           [BirthLocation], [LastDateTime], [LastTimeline], [LastLocation], [TMModel], [Skills]
    FROM dbo.Traveler
END
