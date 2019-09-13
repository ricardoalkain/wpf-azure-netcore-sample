using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TTMS.Common.Abstractions;
using TTMS.Common.Enums;
using TTMS.Common.Models;
using System.Linq;
using TTMS.Data.Repositories;

namespace TTMS.Web.Api.Services
{
    public class TravelerApiService : ITravelerApiService
    {
        private readonly ITravelerService service;

        public TravelerApiService(ITravelerService service)
        {
            this.service = service ?? throw new ArgumentNullException(nameof(service));
        }

        public async Task<Traveler> CreateAsync(Traveler traveler)
        {
            return await service.CreateAsync(traveler).ConfigureAwait(false);
        }

        public async Task DeleteAsync(Guid id)
        {
            await service.DeleteAsync(id).ConfigureAwait(false);
        }

        public async Task<IEnumerable<Traveler>> GetAllAsync()
        {
            return await service.GetAllAsync().ConfigureAwait(false);
        }

        public async Task<IEnumerable<Traveler>> GetAllAsync(TravelerStatus? filterByStatus, bool loadPictures)
        {
            var list = await this.GetAllAsync().ConfigureAwait(false);

            if (filterByStatus.HasValue)
            {
                list = list.Where(item => item.Status == filterByStatus);
            }

            if (!loadPictures)
            {
                list = list.Select(item =>
                    {
                        item.Picture = null;
                        return item;
                    });
            }

            return list;
        }

        public async Task<Traveler> GetByIdAsync(Guid id, bool loadPicture)
        {
            var traveler = await this.GetByIdAsync(id).ConfigureAwait(false);

            if (!loadPicture)
            {
                traveler.Picture = null;
            }

            return traveler;
        }

        public async Task<Traveler> GetByIdAsync(Guid id)
        {
            return await service.GetByIdAsync(id).ConfigureAwait(false);
        }

        public async Task UpdateAsync(Traveler traveler)
        {
            await service.UpdateAsync(traveler).ConfigureAwait(false);
        }
    }
}
