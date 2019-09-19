using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using TTMS.Common.Models;
using TTMS.Data.Repositories;

namespace TTMS.Web.Api.Services
{
    public class TravelerApiService : ITravelerService
    {
        private readonly ILogger logger;
        private readonly ITravelerRepository travelerRepository;

        public TravelerApiService(ILogger logger, ITravelerRepository travelerRepository)
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.travelerRepository = travelerRepository ?? throw new ArgumentNullException(nameof(travelerRepository));
        }

        public async Task<Traveler> CreateAsync(Traveler traveler)
        {
            if (traveler.Id == default)
            {
                traveler.Id = Guid.NewGuid();
                logger.LogDebug("Traveler ID auto-generated: {id}", traveler.Id);
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
