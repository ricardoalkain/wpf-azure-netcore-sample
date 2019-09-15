using System;
using TTMS.Common.Abstractions;
using TTMS.Common.Entities;

namespace TTMS.Common.Abstractions
{
    public interface ITravelerService : IBasicDataProvider<Guid, Traveler>
    {
    }
}
