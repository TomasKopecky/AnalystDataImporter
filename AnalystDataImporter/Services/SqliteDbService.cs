using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AnalystDataImporter.Models;

namespace AnalystDataImporter.Services
{
    public class SqliteDbService
    {
        private string _connectionString = @"Data Source=analyst_data_importer.db;Version=3;";

        public async Task<List<IndexAction>> GetAllActionsAsync()
        {
            var actions = new List<IndexAction>();

            using (var conn = new SQLiteConnection(_connectionString))
            {
                await conn.OpenAsync();
                string sql = "SELECT ActionId, Name, Description, Created FROM actions;";

                using (var cmd = new SQLiteCommand(sql, conn))
                {
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            actions.Add(new IndexAction
                            {
                                ActionId = Convert.ToInt32(reader["ActionId"]),
                                Name = reader["Name"].ToString(),
                                Description = reader["Description"].ToString(),
                                Created = Convert.ToDateTime(reader["Created"])
                            });
                        }
                    }
                }
            }

            return actions;
        }

        public async Task<IndexAction> GetActionAsync(int actionId)
        {
            using (var conn = new SQLiteConnection(_connectionString))
            {
                await conn.OpenAsync();
                string sql = "SELECT ActionId, Name, Description, Created FROM actions WHERE ActionId = @ActionId;";

                using (var cmd = new SQLiteCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@ActionId", actionId);
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            return new IndexAction
                            {
                                ActionId = Convert.ToInt32(reader["ActionId"]),
                                Name = reader["Name"].ToString(),
                                Description = reader["Description"].ToString(),
                                Created = Convert.ToDateTime(reader["Created"])
                            };
                        }
                    }
                }
            }

            return null;
        }

        public async Task<int> CreateActionAsync(IndexAction action)
        {
            using (var conn = new SQLiteConnection(_connectionString))
            {
                await conn.OpenAsync();
                string sql = @"
            INSERT INTO actions (Name, Description, Created)
            VALUES (@Name, @Description, @Created);
            SELECT last_insert_rowid();";

                using (var cmd = new SQLiteCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@Name", action.Name);
                    cmd.Parameters.AddWithValue("@Description", action.Description);
                    cmd.Parameters.AddWithValue("@Created", action.Created);
                    var result = await cmd.ExecuteScalarAsync();
                    return Convert.ToInt32(result);
                }
            }
        }

        public async Task UpdateActionAsync(IndexAction action)
        {
            using (var conn = new SQLiteConnection(_connectionString))
            {
                await conn.OpenAsync();
                string sql = @"
            UPDATE actions 
            SET Name = @Name, Description = @Description, Created = @Created
            WHERE ActionId = @ActionId;";

                using (var cmd = new SQLiteCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@ActionId", action.ActionId);
                    cmd.Parameters.AddWithValue("@Name", action.Name);
                    cmd.Parameters.AddWithValue("@Description", action.Description);
                    cmd.Parameters.AddWithValue("@Created", action.Created);
                    await cmd.ExecuteNonQueryAsync();
                }
            }
        }


        public async Task DeleteActionAsync(int actionId)
        {
            using (var conn = new SQLiteConnection(_connectionString))
            {
                await conn.OpenAsync();
                string sql = "DELETE FROM actions WHERE ActionId = @ActionId;";

                using (var cmd = new SQLiteCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@ActionId", actionId);
                    await cmd.ExecuteNonQueryAsync();
                }
            }
        }


    }
}
