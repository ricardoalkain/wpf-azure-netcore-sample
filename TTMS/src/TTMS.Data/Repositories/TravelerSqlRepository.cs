using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using TTMS.Common.Enums;
using TTMS.Data.Entities;

namespace TTMS.Data.Repositories
{
    public class TravelerSqlRepository : ITravelerRepository
    {
        private readonly string connectionString;

        public TravelerSqlRepository(string connectionstring)
        {
            this.connectionString = connectionstring;
        }

        public async Task DeleteAsync(Guid id)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                var cmd = connection.CreateCommand();
                cmd.CommandText = "dbo.spu_DeleteTraveler";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("id", SqlDbType.UniqueIdentifier).Value = id;

                await cmd.ExecuteNonQueryAsync();
            }
        }

        public async Task<Traveler> GetByIdAsync(Guid id)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                var cmd = connection.CreateCommand();
                cmd.CommandText = "dbo.spu_GetTraveler";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("id", SqlDbType.UniqueIdentifier).Value = id;

                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        return TravelerFromReader(reader);
                    }
                    else
                    {
                        return null;
                    }
                }
            }
        }

        public async Task<IEnumerable<Traveler>> GetAllAsync()
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                var cmd = connection.CreateCommand();
                cmd.CommandText = "dbo.spu_GetAllTravelers";
                cmd.CommandType = CommandType.StoredProcedure;

                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    var list = new List<Traveler>();
                    while (await reader.ReadAsync())
                    {
                        list.Add(TravelerFromReader(reader));
                    }

                    return list;
                }
            }
        }

        public async Task InsertOrReplaceAsync(Traveler traveler)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                var cmd = connection.CreateCommand();
                cmd.CommandText = "spu_AddOrUpdateTraveler";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("id", SqlDbType.UniqueIdentifier).Value = traveler.Id;
                cmd.Parameters.Add("name", SqlDbType.NVarChar).Value = traveler.Name;
                cmd.Parameters.Add("alias", SqlDbType.NVarChar).Value = traveler.Alias;
                cmd.Parameters.Add("type", SqlDbType.Int).Value = traveler.Type;
                cmd.Parameters.Add("status", SqlDbType.Int).Value = traveler.Status;
                cmd.Parameters.Add("deviceModel", SqlDbType.Int).Value = traveler.DeviceModel;
                cmd.Parameters.Add("birthDate", SqlDbType.DateTime2).Value = traveler.BirthDate;
                cmd.Parameters.Add("birthLocation", SqlDbType.NVarChar).Value = traveler.BirthLocation;
                cmd.Parameters.Add("birthTimeline", SqlDbType.Int).Value = traveler.BirthTimelineId;
                cmd.Parameters.Add("lastDateTime", SqlDbType.DateTime2).Value = traveler.LastDateTime;
                cmd.Parameters.Add("lastLocation", SqlDbType.NVarChar).Value = traveler.LastLocation;
                cmd.Parameters.Add("lastTimeline", SqlDbType.Int).Value = traveler.LastTimelineId;
                cmd.Parameters.Add("picture", SqlDbType.VarBinary).Value = traveler.Picture;
                cmd.Parameters.Add("skills", SqlDbType.NVarChar).Value = traveler.Skills;

                await cmd.ExecuteNonQueryAsync();
            }
        }

        private Traveler TravelerFromReader(SqlDataReader reader)
        {
            return new Traveler
            {
                Id = (reader[nameof(Traveler.Id)] as Guid?) ?? default,
                Name = (reader[nameof(Traveler.Name)] as string),
                Alias = (reader[nameof(Traveler.Alias)] as string),
                BirthDate = (reader[nameof(Traveler.BirthDate)] as DateTime?) ?? default,
                BirthLocation = (reader[nameof(Traveler.BirthLocation)] as string),
                BirthTimelineId = (reader[nameof(Traveler.BirthTimelineId)] as int?) ?? default,
                DeviceModel = (DeviceModel)((reader[nameof(Traveler.DeviceModel)] as int?) ?? default),
                LastDateTime = (reader[nameof(Traveler.LastDateTime)] as DateTime?) ?? default,
                LastLocation = (reader[nameof(Traveler.LastLocation)] as string),
                LastTimelineId = (reader[nameof(Traveler.LastTimelineId)] as int?) ?? default,
                Picture = (reader[nameof(Traveler.Picture)] as byte[]),
                Skills = (reader[nameof(Traveler.Skills)] as string),
                Status = (TravelerStatus)((reader[nameof(Traveler.Status)] as int?) ?? default),
                Type = (TravelerType)((reader[nameof(Traveler.Type)] as int?) ?? default)
            };
        }
    }
}
