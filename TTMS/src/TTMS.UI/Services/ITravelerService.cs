using System;
using TTMS.Common.Abstractions;
using TTMS.Common.Models;

namespace TTMS.UI.Services
{
    public interface ITravelerService : IDataService<Guid, Traveler>
    {
    }
}
