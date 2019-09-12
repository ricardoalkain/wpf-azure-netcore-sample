
CREATE PROCEDURE dbo.spu_GetTraveler
    @id UNIQUEIDENTIFIER
AS
BEGIN
    SELECT [Id], [Type], [Name], [Status], [Alias], [Picture], [BirthDate], [BirthTimelineId],
           [BirthLocation], [LastDateTime], [LastTimelineId], [LastLocation], [DeviceModel], [Skills]
    FROM dbo.Traveler
    WHERE [Id] = @id
END