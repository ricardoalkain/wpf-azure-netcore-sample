using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using TTMS.Common.Models;
using Dapper;
using TTMS.Common.Abstractions;
using Microsoft.Extensions.Logging;

namespace TTMS.Data.Sql
{
    public class TravelerSqlReader : ITravelerReader
    {
        private readonly ILogger logger;
        private readonly string connectionString;

        public TravelerSqlReader(ILogger logger, string connectionString)
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));

            if (string.IsNullOrEmpty(connectionString))
            {
                throw new ArgumentNullException(nameof(connectionString));
            }
            this.connectionString = connectionString;
        }

        public async Task<Traveler> GetByIdAsync(Guid id)
        {
            logger.LogDebug("{Method} => {id}", nameof(GetByIdAsync), id);

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
            logger.LogDebug("{Method}", nameof(GetAllAsync));

            using (IDbConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                return (await connection.QueryAsync<Traveler>(
                    sql: "dbo.spu_GetAllTravelers",
                    commandType: CommandType.StoredProcedure))?.ToList();
            }
        }
    }
}