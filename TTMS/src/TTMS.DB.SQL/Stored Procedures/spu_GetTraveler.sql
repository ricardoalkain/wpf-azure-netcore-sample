
CREATE PROCEDURE dbo.spu_GetTraveler
    @id UNIQUEIDENTIFIER
AS
BEGIN
    SELECT [Id], [Type], [Name], [Status], [Alias], [Picture], [BirthDate], [BirthTimeline],
           [BirthLocation], [LastDateTime], [LastTimeline], [LastLocation], [TMModel], [Skills]
    FROM dbo.Traveler
    WHERE [Id] = @id
END