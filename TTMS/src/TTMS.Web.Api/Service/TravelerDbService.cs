using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TTMS.Common.Abstractions;
using TTMS.Common.Entities;

namespace TTMS.Web.Api.Services
{
    public class TravelerDbService : ITravelerDbService
    {
        private readonly ITravelerReader reader;
        private readonly ITravelerWriter writer;

        public TravelerDbService(ITravelerReader reader, ITravelerWriter writer)
        {
            this.reader = reader ?? throw new ArgumentNullException(nameof(reader));
            this.writer = writer ?? throw new ArgumentNullException(nameof(writer));
        }

        public async Task<Traveler> CreateAsync(Traveler traveler)
        {
            if (traveler.Id == default)
            {
                traveler.Id = Guid.NewGuid();
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

        public async Task UpdateAsync(Traveler traveler)
        {
            await writer.UpdateAsync(traveler).ConfigureAwait(false);
        }
    }
}
