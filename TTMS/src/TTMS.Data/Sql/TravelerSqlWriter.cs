using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.Logging;
using TTMS.Common.Abstractions;
using TTMS.Common.Models;

namespace TTMS.Data.Sql
{
    public class TravelerSqlWriter : ITravelerWriter
    {
        private readonly ILogger logger;
        private readonly string connectionString;

        public TravelerSqlWriter(ILogger logger, string connectionString)
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));

            if (string.IsNullOrEmpty(connectionString))
            {
                throw new ArgumentNullException(nameof(connectionString));
            }

            this.connectionString = connectionString;
        }

        public async Task DeleteAsync(Guid id)
        {
            logger.LogDebug("{Method} => {id}", nameof(DeleteAsync), id);

            using (IDbConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                var parameters = new DynamicParameters();
                parameters.Add("id", id);

                await connection.ExecuteAsync(
                    sql: "dbo.spu_DeleteTraveler",
                    commandType: CommandType.StoredProcedure,
                    param: parameters);
            }
        }

        public async Task<Traveler> CreateAsync(Traveler traveler)
        {
            logger.LogDebug("{Method} => {@Traveler}", nameof(CreateAsync), traveler);

            using (IDbConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                var queryParameters = GenerateSqlParameters(traveler);
                return (await connection.QueryAsync<Traveler>(
                    sql: "dbo.spu_InsertTraveler",
                    commandType: CommandType.StoredProcedure,
                    param: queryParameters))?.FirstOrDefault();
            }
        }

        public async Task UpdateAsync(Traveler traveler)
        {
            logger.LogDebug("{Method} => {@Traveler}", nameof(UpdateAsync), traveler);

            using (IDbConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                var queryParameters = GenerateSqlParameters(traveler);
                await connection.ExecuteAsync(
                    sql: "dbo.spu_UpdateTraveler",
                    commandType: CommandType.StoredProcedure,
                    param: queryParameters);
            }
        }

        private DynamicParameters GenerateSqlParameters(Traveler traveler)
        {
            var parameters = new DynamicParameters();
            parameters.Add("id", traveler.Id);
            parameters.Add("name", traveler.Name);
            parameters.Add("alias", traveler.Alias);
            parameters.Add("type", traveler.Type);
            parameters.Add("status", traveler.Status);
            parameters.Add("deviceModel", traveler.DeviceModel);
            parameters.Add("birthDate", traveler.BirthDate, DbType.DateTime2);
            parameters.Add("birthLocation", traveler.BirthLocation);
            parameters.Add("birthTimeline", traveler.BirthTimelineId);
            parameters.Add("lastDateTime", traveler.LastDateTime, DbType.DateTime2);
            parameters.Add("lastLocation", traveler.LastLocation);
            parameters.Add("lastTimeline", traveler.LastTimelineId);
            parameters.Add("picture", traveler.Picture, DbType.Binary);
            parameters.Add("skills", traveler.Skills);

            return parameters;
        }
    }
}