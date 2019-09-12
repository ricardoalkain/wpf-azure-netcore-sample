CREATE PROCEDURE dbo.spu_DeleteTraveler
    @id UNIQUEIDENTIFIER
AS
BEGIN
    DELETE dbo.Traveler
    WHERE [Id] = @id
END