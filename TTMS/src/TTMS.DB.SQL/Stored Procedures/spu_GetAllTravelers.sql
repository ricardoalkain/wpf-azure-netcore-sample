CREATE PROCEDURE dbo.spu_GetAllTravelers
AS
BEGIN
    SELECT [Id], [Type], [Name], [Status], [Alias], [Picture], [BirthDate], [BirthTimelineId],
           [BirthLocation], [LastDateTime], [LastTimelineId], [LastLocation], [DeviceModel], [Skills]
    FROM dbo.Traveler
END
