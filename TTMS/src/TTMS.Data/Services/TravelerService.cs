using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TTMS.Data.Extensions;
using TTMS.Data.Models;
using TTMS.Data.Repositories;

namespace TTMS.Data.Services
{
    public class TravelerService : ITravelerService
    {
        private readonly ITravelerRepository travelerRepository;

        public TravelerService(ITravelerRepository travelerRepository)
        {
            this.travelerRepository = travelerRepository ?? throw new ArgumentNullException(nameof(travelerRepository));
        }

        public async Task<Traveler> CreateAsync(Traveler traveler)
        {
            var entity = traveler.ToEntity();
            await travelerRepository.InsertOrReplaceAsync(entity);
            return entity.ToModel();
        }

        public async Task DeleteAsync(Guid id)
        {
            await travelerRepository.DeleteAsync(id);
        }

        public async Task<IEnumerable<Traveler>> GetAllAsync()
        {
            var travelers = await travelerRepository.GetAllAsync().ConfigureAwait(false);
            return travelers?.ToModel();
        }

        public async Task<Traveler> GetByIdAsync(Guid key)
        {
            var traveler = await travelerRepository.GetByIdAsync(key);
            return traveler?.ToModel();
        }

        public async Task UpdateAsync(Traveler traveler)
        {
            await travelerRepository.InsertOrReplaceAsync(traveler.ToEntity());
        }
    }
}
