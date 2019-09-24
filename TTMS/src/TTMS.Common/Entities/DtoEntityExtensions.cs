using System;
using System.Collections.Generic;
using System.Linq;
using TTMS.Common.Entities;
using TTMS.Common.Enums;

namespace TTMS.Common.DTO.Extensions
{
    public static class DtoEntityExtensions
    {
        public static TravelerRequest CreateRequest(this Traveler entity)
        {
            return new TravelerRequest
            {
                Id = string.IsNullOrEmpty(entity.RowKey) ? default : Guid.Parse(entity.RowKey),
                Type = (TravelerType)Enum.Parse(typeof(TravelerType), entity.PartitionKey),

                Name = entity.Name,
                Alias = entity.Alias,
                BirthDate = entity.BirthDate,
                BirthTimelineId = entity.BirthTimelineId,
                BirthLocation = entity.BirthLocation,
                LastDateTime = entity.LastDateTime,
                LastTimelineId = entity.LastTimelineId,
                LastLocation = entity.LastLocation,
                Picture = entity.Picture,
                Skills = entity.Skills,
                Status = (TravelerStatus)entity.Status,
                DeviceModel = (DeviceModel)entity.DeviceModel,
            };
        }

        public static TravelerResponse CreateResponse(this Traveler entity)
        {
            return new TravelerResponse
            {
                Id = string.IsNullOrEmpty(entity.RowKey) ? default : Guid.Parse(entity.RowKey),
                Type = (TravelerType)Enum.Parse(typeof(TravelerType), entity.PartitionKey),

                Name = entity.Name,
                Alias = entity.Alias,
                BirthDate = entity.BirthDate,
                BirthTimelineId = entity.BirthTimelineId,
                BirthLocation = entity.BirthLocation,
                LastDateTime = entity.LastDateTime,
                LastTimelineId = entity.LastTimelineId,
                LastLocation = entity.LastLocation,
                Picture = entity.Picture,
                Skills = entity.Skills,
                Status = (TravelerStatus)entity.Status,
                DeviceModel = (DeviceModel)entity.DeviceModel,
            };
        }

        public static IEnumerable<TravelerResponse> CreateResponse(this IEnumerable<Traveler> travelers)
        {
            return travelers.Select(t => t.CreateResponse());
        }

        public static Traveler ToEntity(this TravelerRequest request)
        {
            return new Traveler
            {
                RowKey = request.Id.ToString(),
                PartitionKey = request.Type.ToString(),

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
                Status = (int)request.Status,
                DeviceModel = (int)request.DeviceModel,
            };
        }

        public static Traveler ToEntity(this TravelerResponse response)
        {
            return new Traveler
            {
                RowKey = response.Id.ToString(),
                PartitionKey = response.Type.ToString(),

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
                Status = (int)response.Status,
                DeviceModel = (int)response.DeviceModel,
            };
        }
    }
}
