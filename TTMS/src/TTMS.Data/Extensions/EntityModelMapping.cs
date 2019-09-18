using System;
using System.Collections.Generic;
using System.Linq;
using TTMS.Common.Enums;
using Models = TTMS.Common.Models;

namespace TTMS.Data.Extensions
{
    public static class EntityModelMappings
    {
        #region Model Extensions

        public static Entities.Traveler ToEntity(this Models.Traveler model)
        {
            return new Entities.Traveler
            {
                RowKey = model.Id.ToString(),
                PartitionKey = model.Type.ToString(),

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
            };
        }

        public static IEnumerable<Entities.Traveler> ToEntity(this IEnumerable<Models.Traveler> travelers)
        {
            return travelers.Select(t => t.ToEntity());
        }

        #endregion

        #region Entity Extensions

        public static Models.Traveler ToModel(this Entities.Traveler entity)
        {
            return new Models.Traveler
            {
                Id = Guid.Parse(entity.RowKey),
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
                Status = entity.Status,
                DeviceModel = entity.DeviceModel,
            };
        }

        public static IEnumerable<Models.Traveler> ToModel(this IEnumerable<Entities.Traveler> travelers)
        {
            return travelers.Select(t => t.ToModel()).ToList();
        }

        #endregion
    }
}