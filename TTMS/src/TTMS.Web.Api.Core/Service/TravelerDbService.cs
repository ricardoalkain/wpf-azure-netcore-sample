using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using TTMS.Common.Abstractions;
using TTMS.Common.Enums;
using TTMS.Common.Models;

namespace TTMS.Web.Api.Core.Services
{
    public class TravelerDbService : ITravelerDbService
    {
        private readonly ILogger logger;
        private readonly ITravelerReader reader;
        private readonly ITravelerWriter writer;

        public TravelerDbService(ILogger<TravelerDbService> logger, ITravelerReader reader, ITravelerWriter writer)
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.reader = reader ?? throw new ArgumentNullException(nameof(reader));
            this.writer = writer ?? throw new ArgumentNullException(nameof(writer));
        }

        public async Task<Traveler> CreateAsync(Traveler traveler)
        {
            if (traveler == null)
            {
                var ex = new ArgumentNullException(nameof(traveler), "Enitity can't be null");
                logger.LogError(ex, ex.Message);
                throw ex;
            }

            if (traveler.Id == default)
            {
                traveler.Id = Guid.NewGuid();
                logger.LogInformation("New traveler key generated: {id}", traveler.Id);
            }

            return await writer.CreateAsync(traveler).ConfigureAwait(false);
        }

        public async Task DeleteAsync(Guid id)
        {
            await writer.DeleteAsync(id).ConfigureAwait(false);
        }

        public async Task<IEnumerable<Traveler>> GetAllAsync()
        {
            var travelers = await reader.GetAllAsync().ConfigureAwait(false);
            return travelers;
        }

        public async Task<Traveler> GetByIdAsync(Guid key)
        {
            var traveler = await reader.GetByIdAsync(key).ConfigureAwait(false);
            return traveler;
        }

        public async Task<IEnumerable<Traveler>> GetByTypeAsync(TravelerType travelerType)
        {
            var traveler = await reader.GetByTypeAsync(travelerType).ConfigureAwait(false);
            return traveler;
        }

        public async Task UpdateAsync(Traveler traveler)
        {
            await writer.UpdateAsync(traveler).ConfigureAwait(false);
        }
    }
}
