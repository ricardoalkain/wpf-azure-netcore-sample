using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using TTMS.Common.Abstractions;
using TTMS.Common.Entities;

namespace TTMS.Data.Sql
{
    public class TravelerSqlWriter : ITravelerWriter
    {
        private readonly string connectionString;

        public TravelerSqlWriter(string connectionstring)
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

        public async Task<Traveler> CreateAsync(Traveler traveler)
        {
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