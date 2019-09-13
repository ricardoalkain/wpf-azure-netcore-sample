using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using TTMS.Data.Entities;
using Dapper;

namespace TTMS.Data.Repositories
{
    public class TravelerSqlRepository : ITravelerRepository
    {
        private readonly string connectionString;

        public TravelerSqlRepository(string connectionstring)
        {
            this.connectionString = connectionstring;
        }

        public async Task DeleteAsync(Guid key)
        {
            using (IDbConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                var parameters = new DynamicParameters();
                parameters.Add("id", key);

                await connection.ExecuteAsync(
                    sql: "dbo.spu_DeleteTraveler",
                    commandType: CommandType.StoredProcedure,
                    param: parameters);
            }
        }

        public async Task<Traveler> GetByIdAsync(Guid id)
        {
            using (IDbConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                var parameters = new DynamicParameters();
                parameters.Add("id", id);

                var traveler = (await connection.QueryAsync<Traveler>(
                    sql: "dbo.spu_GetTraveler",
                    commandType: CommandType.StoredProcedure,
                    param: parameters))?.FirstOrDefault();

                return traveler;
            }
        }

        public async Task<IEnumerable<Traveler>> GetAllAsync()
        {
            using (IDbConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                return (await connection.QueryAsync<Traveler>(
                    sql: "dbo.spu_GetAllTravelers",
                    commandType: CommandType.StoredProcedure))?.ToList();
            }
        }

        public async Task InsertOrReplaceAsync(Traveler traveler)
        {
            using (IDbConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                var queryParameters = new DynamicParameters();
                queryParameters.Add("id", traveler.Id);
                queryParameters.Add("name", traveler.Name);
                queryParameters.Add("alias", traveler.Alias);
                queryParameters.Add("type", traveler.Type);
                queryParameters.Add("status", traveler.Status);
                queryParameters.Add("deviceModel", traveler.DeviceModel);
                queryParameters.Add("birthDate", traveler.BirthDate, DbType.DateTime2);
                queryParameters.Add("birthLocation", traveler.BirthLocation);
                queryParameters.Add("birthTimeline", traveler.BirthTimelineId);
                queryParameters.Add("lastDateTime", traveler.LastDateTime, DbType.DateTime2);
                queryParameters.Add("lastLocation", traveler.LastLocation);
                queryParameters.Add("lastTimeline", traveler.LastTimelineId);
                queryParameters.Add("picture", traveler.Picture, DbType.Binary);
                queryParameters.Add("skills", traveler.Skills);

                await connection.ExecuteAsync(
                    sql: "dbo.spu_AddOrUpdateTraveler",
                    commandType: CommandType.StoredProcedure,
                    param: queryParameters);
            }
        }
    }
}
