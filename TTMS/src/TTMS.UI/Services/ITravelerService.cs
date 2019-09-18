﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TTMS.Common.Entities;

namespace TTMS.UI.Services
{
    public interface ITravelerService
    {
        Task<Traveler> CreateAsync(Traveler traveler);
        Task DeleteAsync(Guid key);
        Task<IEnumerable<Traveler>> GetAllAsync();
        Task<Traveler> GetByIdAsync(Guid id);
        Task UpdateAsync(Traveler traveler);
    }
}