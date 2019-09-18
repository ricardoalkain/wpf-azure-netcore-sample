using System;
using TTMS.Common.Abstractions;
using TTMS.Common.Entities;

namespace TTMS.Common.Abstractions
{
    public interface ITravelerReader : IDataReader<Guid, Traveler>
    {
    }
}
