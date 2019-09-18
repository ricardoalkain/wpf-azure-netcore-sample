using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TTMS.Common.Models;
using TTMS.Data.Repositories;

namespace TTMS.Web.Api.Services
{
    public class TravelerApiService : ITravelerService
    {
        private readonly ITravelerRepository travelerRepository;

        public TravelerApiService(ITravelerRepository travelerRepository)
        {
            this.travelerRepository = travelerRepository ?? throw new ArgumentNullException(nameof(travelerRepository));
        }

        public async Task<Traveler> CreateAsync(Traveler traveler)
        {
            if (traveler.Id == default)
            {
                traveler.Id = Guid.NewGuid();
            }

            await travelerRepository.InsertOrReplaceAsync(traveler).ConfigureAwait(false);
            return traveler;
        }

        public async Task DeleteAsync(Guid id)
        {
            await travelerRepository.DeleteAsync(id).ConfigureAwait(false);
        }

        public async Task<IEnumerable<Traveler>> GetAllAsync()
        {
            var travelers = await travelerRepository.GetAllAsync().ConfigureAwait(false);
            return travelers;
        }

        public async Task<Traveler> GetByIdAsync(Guid key)
        {
            var traveler = await travelerRepository.GetByIdAsync(key).ConfigureAwait(false);
            return traveler;
        }

        public async Task UpdateAsync(Traveler traveler)
        {
            await travelerRepository.InsertOrReplaceAsync(traveler).ConfigureAwait(false);
        }
    }
}
