using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TTMS.Common.Abstractions;
using TTMS.Common.Entities;

namespace TTMS.UI.Services
{
    public class TravelerService : ITravelerService
    {
        private readonly ITravelerReader reader;
        private readonly ITravelerWriter writer;

        public TravelerService(ITravelerReader travelerReader, ITravelerWriter travelerWriter)
        {
            reader = travelerReader ?? throw new ArgumentNullException(nameof(travelerReader));
            writer = travelerWriter ?? throw new ArgumentNullException(nameof(travelerWriter));
        }

        public async Task<IEnumerable<Traveler>> GetAllAsync()
        {
            return await reader.GetAllAsync().ConfigureAwait(false);
        }

        public async Task<Traveler> GetByIdAsync(Guid id)
        {
            return await reader.GetByIdAsync(id).ConfigureAwait(false);
        }

        public async Task<Traveler> CreateAsync(Traveler traveler)
        {
            return await writer.CreateAsync(traveler);
        }

        public async Task DeleteAsync(Guid id)
        {
            await writer.DeleteAsync(id);
        }

        public async Task UpdateAsync(Traveler traveler)
        {
            await writer.UpdateAsync(traveler);
        }
    }
}
