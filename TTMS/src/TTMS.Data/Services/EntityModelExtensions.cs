using System.Collections.Generic;
using System.Linq;
using Models = TTMS.Common.Models;

namespace TTMS.Data.Services
{
    public static class EntityModelExtensions
    {
        #region Model Extensions

        public static Entities.Traveler ToEntity(this Models.Traveler model)
        {
            return new Data.Entities.Traveler
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

        public static IEnumerable<Data.Entities.Traveler> ToEntity(this IEnumerable<Models.Traveler> travelers)
        {
            return travelers.Select(t => t.ToEntity());
        }

        /// <summary>
        /// Copy the properties of an <see cref="Entities.Traveler"/> entity to the model without
        /// creating a new instance (thus keeping the original reference).
        /// </summary>
        /// <param name="model"></param>
        /// <param name="entity"></param>
        public static void ReadFrom(this Models.Traveler model, Entities.Traveler entity)
        {
            model.Id = entity.Id;
            model.Name = entity.Name;
            model.Alias = entity.Alias;
            model.BirthDate = entity.BirthDate;
            model.BirthTimelineId = entity.BirthTimelineId;
            model.BirthLocation = entity.BirthLocation;
            model.LastDateTime = entity.LastDateTime;
            model.LastTimelineId = entity.LastTimelineId;
            model.LastLocation = entity.LastLocation;
            model.Picture = entity.Picture;
            model.Skills = entity.Skills;
            model.Status = entity.Status;
            model.DeviceModel = entity.DeviceModel;
            model.Type = entity.Type;
        }

        #endregion

        #region Entity Extensions

        public static Models.Traveler ToModel(this Data.Entities.Traveler entity)
        {
            return new Models.Traveler
            {
                Id = entity.Id,
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
                Type = entity.Type
            };
        }

        public static IEnumerable<Models.Traveler> ToModel(this IEnumerable<Data.Entities.Traveler> travelers)
        {
            return travelers.Select(t => t.ToModel()).ToList();
        }

        #endregion
    }
}
