using System;
using TTMS.Common.Abstractions;
using TTMS.Common.Entities;

namespace TTMS.Web.Api.Services
{
    public interface ITravelerService : IBasicDataProvider<Guid, Traveler>
    {
    }
}
