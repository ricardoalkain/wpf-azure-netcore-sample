using System.Collections.Generic;
using System.Linq;
using TTMS.Common.Models;

namespace TTMS.Common.DTO
{
    public static class DTOExtensions
    {
        public static TravelerRequest CreateRequest(this Traveler model)
        {
            return new TravelerRequest
            {
                Id = model.Id,
                Name = model.Name,
                Alias = model.Alias,
                BirthDate = model.BirthDate,
                BirthTimelineId = model.BirthTimelineId,
                BirthLocation = model.BirthLocation,
                LastDateTime = model.LastDateTime,
                LastTimelineId = model.LastTimelineId,
                LastLocation = model.LastLocation,
                Picture = model.Picture,
                Skills = model.Skills,
                Status = model.Status,
                DeviceModel = model.DeviceModel,
                Type = model.Type
            };
        }

        public static TravelerResponse CreateResponse(this Traveler model)
        {
            return new TravelerResponse
            {
                Id = model.Id,
                Name = model.Name,
                Alias = model.Alias,
                BirthDate = model.BirthDate,
                BirthTimelineId = model.BirthTimelineId,
                BirthLocation = model.BirthLocation,
                LastDateTime = model.LastDateTime,
                LastTimelineId = model.LastTimelineId,
                LastLocation = model.LastLocation,
                Picture = model.Picture,
                Skills = model.Skills,
                Status = model.Status,
                DeviceModel = model.DeviceModel,
                Type = model.Type
            };
        }

        public static IEnumerable<Traveler> CreateResponse(this IEnumerable<Traveler> models)
        {
            return models.Select(t => t.CreateResponse());
        }

        public static void SyncWith(this TravelerResponse response, Traveler model)
        {
            response.Id = model.Id;
            response.Name = model.Name;
            response.Alias = model.Alias;
            response.BirthDate = model.BirthDate;
            response.BirthTimelineId = model.BirthTimelineId;
            response.BirthLocation = model.BirthLocation;
            response.LastDateTime = model.LastDateTime;
            response.LastTimelineId = model.LastTimelineId;
            response.LastLocation = model.LastLocation;
            response.Picture = model.Picture;
            response.Skills = model.Skills;
            response.Status = model.Status;
            response.DeviceModel = model.DeviceModel;
            response.Type = model.Type;
        }


        public static Traveler ToModel(this TravelerRequest request)
        {
            return new Traveler
            {
                Id = request.Id ?? default,
                Name = request.Name,
                Alias = request.Alias,
                BirthDate = request.BirthDate,
                BirthTimelineId = request.BirthTimelineId,
                BirthLocation = request.BirthLocation,
                LastDateTime = request.LastDateTime,
                LastTimelineId = request.LastTimelineId,
                LastLocation = request.LastLocation,
                Picture = request.Picture,
                Skills = request.Skills,
                Status = request.Status,
                DeviceModel = request.DeviceModel,
                Type = request.Type
            };
        }

        public static Traveler ToModel(this TravelerResponse response)
        {
            return new Traveler
            {
                Id = response.Id,
                Name = response.Name,
                Alias = response.Alias,
                BirthDate = response.BirthDate,
                BirthTimelineId = response.BirthTimelineId,
                BirthLocation = response.BirthLocation,
                LastDateTime = response.LastDateTime,
                LastTimelineId = response.LastTimelineId,
                LastLocation = response.LastLocation,
                Picture = response.Picture,
                Skills = response.Skills,
                Status = response.Status,
                DeviceModel = response.DeviceModel,
                Type = response.Type
            };
        }
    }
}
