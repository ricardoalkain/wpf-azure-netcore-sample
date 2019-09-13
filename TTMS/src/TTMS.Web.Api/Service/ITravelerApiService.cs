using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TTMS.Common.Abstractions;
using TTMS.Common.Enums;
using TTMS.Common.Models;

namespace TTMS.Web.Api.Services
{
    public interface ITravelerApiService : ITravelerService
    {
        Task<IEnumerable<Traveler>> GetAllAsync(TravelerStatus? filterByStatus, bool loadPictures);

        Task<Traveler> GetByIdAsync(Guid id, bool loadPicture);
    }
}