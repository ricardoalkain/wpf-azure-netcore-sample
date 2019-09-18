using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using TTMS.Common.Abstractions;
using TTMS.Common.Entities;
using TTMS.Data.Abstractions;

namespace TTMS.Data.JsonFile
{
    public class TravelerFileRepository : ITravelerReader, ITravelerWriter
    {
        private readonly string fileName;

        public TravelerFileRepository(string dataSource)
        {
            if (string.IsNullOrWhiteSpace(dataSource))
            {
                throw new ArgumentException("No data file location provided", nameof(dataSource));
            }

            fileName = dataSource;
        }

        public async Task<IEnumerable<Traveler>> GetAllAsync()
        {
            return await LoadFromFileAsync().ConfigureAwait(false);
        }

        public async Task<IEnumerable<Traveler>> GetAsync(Func<Traveler, bool> filter)
        {
            var list = await LoadFromFileAsync().ConfigureAwait(false);
            return list?.Where(filter);
        }

        public async Task<Traveler> GetByIdAsync(Guid id)
        {
            var travelers = await GetAllAsync();
            return travelers.FirstOrDefault(traveler => traveler.Id.Equals(id));
        }

        public async Task<Traveler> CreateAsync(Traveler traveler)
        {
            if (traveler == null)
            {
                throw new ArgumentNullException(nameof(traveler));
            }

            var travelers = (await GetAllAsync()).ToList();
            travelers.Add(traveler);

            SaveToFile(travelers);

            return traveler;
        }

        public async Task UpdateAsync(Traveler traveler)
        {
            if (traveler == null)
            {
                throw new ArgumentNullException(nameof(traveler));
            }

            var travelers = (await GetAllAsync()).ToList();
            var index = travelers.FindIndex(t => t.Id.Equals(traveler.Id));

            if (index < 0)
            {
                throw new KeyNotFoundException("No traveler with this ID was found to be updated.");

            }

            travelers[index] = traveler;

            SaveToFile(travelers);
        }

        public async Task DeleteAsync(Guid id)
        {
            var travelers = (await GetAllAsync()).ToList();
            var travelerToUpdate = travelers.FirstOrDefault(t => t.Id.Equals(id));
            if (travelerToUpdate != null)
            {
                travelers.Remove(travelerToUpdate);
                SaveToFile(travelers);
            }
        }

        private void SaveToFile(List<Traveler> travelers)
        {
            File.WriteAllText(fileName, JsonConvert.SerializeObject(travelers));
        }

        private async Task<List<Traveler>> LoadFromFileAsync()
        {
            if (!File.Exists(fileName))
            {
                throw new FileNotFoundException($"File '{fileName}' not found or moved since last access.", fileName);
            }

            using (var file = File.OpenText(fileName))
            {
                var fileContent = await file.ReadToEndAsync().ConfigureAwait(false);
                return JsonConvert.DeserializeObject<List<Traveler>>(fileContent);
            }
        }

        public async Task<bool> Exists(Guid id)
        {
            return (await GetByIdAsync(id).ConfigureAwait(false)) != null;
        }
    }
}
