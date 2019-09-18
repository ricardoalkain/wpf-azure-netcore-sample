using System;
using TTMS.Common.Abstractions;
using TTMS.Common.Models;

namespace TTMS.Web.Api.Services
{
    public interface ITravelerService : IBasicDataProvider<Guid, Traveler>
    {
    }
}
