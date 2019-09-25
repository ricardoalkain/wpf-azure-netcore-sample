using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TTMS.Common.Enums;
using TTMS.Common.Models;

namespace TTMS.UI.Services
{
    public interface ITravelerService
    {
        Task<Traveler> CreateAsync(Traveler traveler);
        Task DeleteAsync(Guid key);
        Task<IEnumerable<Traveler>> GetAllAsync();
        Task<IEnumerable<Traveler>> GetByTypeAsync(TravelerType type);
        Task<Traveler> GetByIdAsync(Guid id);
        Task UpdateAsync(Traveler traveler);
    }
}