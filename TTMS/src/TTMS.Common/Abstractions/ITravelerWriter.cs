using System;
using System.Collections.Generic;
using System.Text;
using TTMS.Common.Abstractions;
using TTMS.Common.Entities;

namespace TTMS.Common.Abstractions
{
    public interface ITravelerWriter : IDataWriter<Guid, Traveler>
    {
    }
}
