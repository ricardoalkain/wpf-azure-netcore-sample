using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using TTMS.Data.Entities;

namespace TTMS.Data.Repositories
{
    public class TravelerFileRepository : ITravelerRepository
    {
        private readonly IFileSystem fileSystem;
        private readonly ILogger logger;
        private readonly string fileName;

        public TravelerFileRepository(
            ILogger logger,
            IFileSystem fileSystem,
            string dataSource)
        {
            if (string.IsNullOrWhiteSpace(dataSource))
            {
                throw new ArgumentException("No data file location provided", nameof(dataSource));
            }

            this.fileSystem = fileSystem ?? throw new ArgumentNullException(nameof(fileSystem));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            fileName = dataSource;
        }

        public async Task<IEnumerable<Traveler>> GetAllAsync()
        {
            logger.LogDebug("{Method}", nameof(GetAllAsync));
            return await LoadFromFileAsync().ConfigureAwait(false);
        }

        public async Task<Traveler> GetByIdAsync(Guid id)
        {
            logger.LogDebug("{Method} => {id}", nameof(InsertOrReplaceAsync), id);

            var travelers = await GetAllAsync();
            return travelers.FirstOrDefault(traveler => traveler.Id.Equals(id));
        }

        public async Task InsertOrReplaceAsync(Traveler traveler)
        {
            logger.LogDebug("{Method} => {@Traveler}", nameof(InsertOrReplaceAsync), traveler);

            if (traveler == null)
            {
                throw new ArgumentNullException(nameof(traveler));
            }

            var travelers = (await GetAllAsync()).ToList();
            var index = travelers.FindIndex(t => t.Id.Equals(traveler.Id));

            if (index < 0)
            {
                travelers.Add(traveler);
                logger.LogInformation("INSERT: Traveler {Id}", traveler.Id);
            }
            else
            {
                travelers[index] = traveler;
                logger.LogInformation("UPDATE: Traveler {Id}", traveler.Id);
            }

            SaveToFile(travelers);
        }

        public async Task DeleteAsync(Guid id)
        {
            logger.LogDebug("{Method} => {id}", nameof(InsertOrReplaceAsync), id);

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
            logger.LogInformation("Saving file: {File}", fileName);
            fileSystem.File.WriteAllText(fileName, JsonConvert.SerializeObject(travelers));
            logger.LogInformation("File saved");
        }

        private async Task<List<Traveler>> LoadFromFileAsync()
        {
            logger.LogInformation("Loading file: {File}", fileName);

            if (!fileSystem.File.Exists(fileName))
            {
                throw new FileNotFoundException($"File '{fileName}' not found or moved since last access.", fileName);
            }

            using (var file = fileSystem.File.OpenText(fileName))
            {
                var fileContent = await file.ReadToEndAsync().ConfigureAwait(false);
                var list = JsonConvert.DeserializeObject<List<Traveler>>(fileContent);
                logger.LogInformation("File loaded");
                return list;
            }
        }
    }
}
