using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using TTMS.Common.Models;
using Dapper;
using TTMS.Common.Abstractions;

namespace TTMS.Data.Sql
{
    public class TravelerSqlReader : ITravelerReader
    {
        private readonly string connectionString;

        public TravelerSqlReader(string connectionstring)
        {
            this.connectionString = connectionstring;
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

        public async Task<IEnumerable<Traveler>> GetAsync(Func<Traveler, bool> filter)
        {
            using (IDbConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                var list = (await connection.QueryAsync<Traveler>(
                    sql: "dbo.spu_GetAllTravelers",
                    commandType: CommandType.StoredProcedure))?.Where(filter);

                return list?.ToList();
            }
        }

        public async Task<bool> Exists(Guid id)
        {
            using (IDbConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                return await connection.ExecuteScalarAsync<bool>(
                    "SELECT COUNT(1) FROM dbo.Traveler WHERE Id = @id", new { id })
                    .ConfigureAwait(false);
            }
        }
    }
}