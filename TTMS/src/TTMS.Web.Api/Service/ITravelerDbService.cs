using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TTMS.Common.Entities;

namespace TTMS.Web.Api.Services
{
    public interface ITravelerDbService
    {
        Task<Traveler> CreateAsync(Traveler traveler);

        Task DeleteAsync(Guid id);

        Task<IEnumerable<Traveler>> GetAllAsync();

        Task<Traveler> GetByIdAsync(Guid key);

        Task UpdateAsync(Traveler traveler);
    }
}