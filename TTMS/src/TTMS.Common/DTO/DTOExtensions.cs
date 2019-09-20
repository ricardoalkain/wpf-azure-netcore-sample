using System;
using System.Collections.Generic;
using System.Linq;
using TTMS.Common.Models;

namespace TTMS.Common.DTO
{
    public static class DTOExtensions
    {
        public static TravelerRequest CreateRequest(this Traveler traveler)
        {
            return new TravelerRequest
            {
                Id = traveler.Id == default ? (Guid?)null : traveler.Id,
                Name = traveler.Name,
                Alias = traveler.Alias,
                BirthDate = traveler.BirthDate,
                BirthTimelineId = traveler.BirthTimelineId,
                BirthLocation = traveler.BirthLocation,
                LastDateTime = traveler.LastDateTime,
                LastTimelineId = traveler.LastTimelineId,
                LastLocation = traveler.LastLocation,
                Picture = traveler.Picture,
                Skills = traveler.Skills,
                Status = traveler.Status,
                DeviceModel = traveler.DeviceModel,
                Type = traveler.Type
            };
        }

        public static TravelerResponse CreateResponse(this Traveler traveler)
        {
            return new TravelerResponse
            {
                Id = traveler.Id,
                Name = traveler.Name,
                Alias = traveler.Alias,
                BirthDate = traveler.BirthDate,
                BirthTimelineId = traveler.BirthTimelineId,
                BirthLocation = traveler.BirthLocation,
                LastDateTime = traveler.LastDateTime,
                LastTimelineId = traveler.LastTimelineId,
                LastLocation = traveler.LastLocation,
                Picture = traveler.Picture,
                Skills = traveler.Skills,
                Status = traveler.Status,
                DeviceModel = traveler.DeviceModel,
                Type = traveler.Type
            };
        }

        public static IEnumerable<Traveler> CreateResponse(this IEnumerable<Traveler> travelers)
        {
            return travelers.Select(t => t.CreateResponse());
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
            return response;
        }
    }
}
