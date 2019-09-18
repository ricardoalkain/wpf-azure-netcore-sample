using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TTMS.Common.Enums;
using TTMS.Common.Models;

namespace TTMS.Common.Abstractions
{
    public interface ITravelerReader : IDataReader<Guid, Traveler>
    {
        Task<IEnumerable<Traveler>> GetByTypeAsync(TravelerType travelerType);
    }
}
