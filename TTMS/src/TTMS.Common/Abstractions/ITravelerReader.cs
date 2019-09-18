using System;
using TTMS.Common.Abstractions;
using TTMS.Common.Models;

namespace TTMS.Common.Abstractions
{
    public interface ITravelerReader : IDataReader<Guid, Traveler>
    {
    }
}
