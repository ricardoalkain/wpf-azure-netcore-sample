using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TTMS.Common.Enums;
using TTMS.Common.Models;

namespace TTMS.Web.Api.Services
{
    public interface ITravelerDbService
    {
        Task<Traveler> CreateAsync(Traveler traveler);

        Task DeleteAsync(Guid id);

        Task<IEnumerable<Traveler>> GetAllAsync();

        Task<Traveler> GetByIdAsync(Guid key);

        Task<IEnumerable<Traveler>> GetByTypeAsync(TravelerType travelerType);

        Task UpdateAsync(Traveler traveler);
    }
}