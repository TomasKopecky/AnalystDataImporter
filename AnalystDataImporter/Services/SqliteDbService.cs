using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AnalystDataImporter.Globals;
using AnalystDataImporter.Models;
using AnalystDataImporter.ViewModels;

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

        public async Task InsertElementsIntoGlobalAsync(List<string> elements)
        {
            using (var conn = new SQLiteConnection(_connectionString))
            {
                await conn.OpenAsync();
                using (var transaction = conn.BeginTransaction())
                {
                    foreach (var element in elements)
                    {
                        string insertGlobalSql = @"
                        INSERT INTO global_objects (ObjectKey)
                        SELECT @ObjectKey
                        WHERE NOT EXISTS (
                            SELECT 1 FROM global_objects WHERE ObjectKey = @ObjectKey
                        );
                        SELECT last_insert_rowid();";

                        using (var cmd = new SQLiteCommand(insertGlobalSql, conn))
                        {

                            cmd.Parameters.AddWithValue("@ObjectKey", element);
                            //cmd.Parameters.AddWithValue("@ObjectType", element.);
                            await cmd.ExecuteNonQueryAsync();

                        }
                    }
                    transaction.Commit();
                }
            }
        }

        public async Task InsertIntoUserObjectsAsync(string objectKey, ElementViewModel element, IndexAction indexAction)
        {
            using (var conn = new SQLiteConnection(_connectionString))
            {
                await conn.OpenAsync();

                // Query to get the PrimaryKey from global_objects
                string getPrimaryKeySql = "SELECT PrimaryKey FROM global_objects WHERE ObjectKey = @ObjectKey;";
                int globalObjectId;

                using (var cmd = new SQLiteCommand(getPrimaryKeySql, conn))
                {
                    cmd.Parameters.AddWithValue("@ObjectKey", objectKey);
                    var result = await cmd.ExecuteScalarAsync();
                    if (result == null)
                    {
                        throw new InvalidOperationException("ObjectKey not found in global_objects table.");
                    }
                    globalObjectId = Convert.ToInt32(result);
                }

                // Now insert into user_objects using the obtained globalObjectId
                string insertUserObjectsSql = @"
                INSERT INTO user_objects (ObjectKey, Class, Icon, ActionId, DateOfInsert)
                SELECT @GlobalObjectId, @Class, @Icon, @ActionId, @DateOfInsert
                WHERE NOT EXISTS (
                    SELECT 1 FROM user_objects WHERE ObjectKey = @GlobalObjectId AND ActionId = @ActionId
                );";

                using (var cmd = new SQLiteCommand(insertUserObjectsSql, conn))
                {
                    cmd.Parameters.AddWithValue("@GlobalObjectId", globalObjectId);
                    //cmd.Parameters.AddWithValue("@ObjectType", element.ObjectType);
                    cmd.Parameters.AddWithValue("@Class", element.Class);
                    cmd.Parameters.AddWithValue("@Icon", Constants.AnalystDataIconsSource[element.IconSourcePath]);
                    cmd.Parameters.AddWithValue("@ActionId", indexAction.ActionId);
                    cmd.Parameters.AddWithValue("@DateOfInsert", DateTime.Now.Date);
                    await cmd.ExecuteNonQueryAsync();
                }
            }
        }

        //public async Task InsertElementsGlobalAndUserAsync(List<string> elements, ElementViewModel elementViewModel, IndexAction indexAction)
        //{
        //    using (var conn = new SQLiteConnection(_connectionString))
        //    {
        //        await conn.OpenAsync();
        //        using (var transaction = conn.BeginTransaction())
        //        {
        //            foreach (var element in elements)
        //            {
        //                int globalObjectId;

        //                // First, try to get the PrimaryKey for an existing ObjectKey
        //                string getPrimaryKeySql = "SELECT PrimaryKey FROM global_objects WHERE ObjectKey = @ObjectKey;";
        //                using (var cmd = new SQLiteCommand(getPrimaryKeySql, conn))
        //                {
        //                    cmd.Parameters.AddWithValue("@ObjectKey", element);
        //                    var result = await cmd.ExecuteScalarAsync();
        //                    if (result != null)
        //                    {
        //                        globalObjectId = Convert.ToInt32(result);
        //                    }
        //                    else
        //                    {
        //                        // If not existing, insert and get the last inserted row id
        //                        string insertGlobalSql = @"
        //                    INSERT INTO global_objects (ObjectKey)
        //                    VALUES (@ObjectKey);
        //                    SELECT last_insert_rowid();";
        //                        using (var insertCmd = new SQLiteCommand(insertGlobalSql, conn))
        //                        {
        //                            insertCmd.Parameters.AddWithValue("@ObjectKey", element);
        //                            globalObjectId = Convert.ToInt32(await insertCmd.ExecuteScalarAsync());
        //                        }
        //                    }
        //                }

        //                // Check if the user_objects record exists
        //                string checkUserObjectSql = "SELECT 1 FROM user_objects WHERE ObjectKey = @GlobalObjectId AND ActionId = @ActionId;";
        //                bool recordExists;
        //                using (var cmd = new SQLiteCommand(checkUserObjectSql, conn))
        //                {
        //                    cmd.Parameters.AddWithValue("@GlobalObjectId", globalObjectId);
        //                    cmd.Parameters.AddWithValue("@ActionId", indexAction.ActionId);
        //                    recordExists = (await cmd.ExecuteScalarAsync()) != null;
        //                }

        //                if (recordExists)
        //                {
        //                    // Update the existing record
        //                    string updateUserObjectSql = @"
        //                UPDATE user_objects
        //                SET Class = @Class, Icon = @Icon, DateOfInsert = @DateOfInsert
        //                WHERE ObjectKey = @GlobalObjectId AND ActionId = @ActionId;";
        //                    using (var cmd = new SQLiteCommand(updateUserObjectSql, conn))
        //                    {
        //                        cmd.Parameters.AddWithValue("@GlobalObjectId", globalObjectId);
        //                        cmd.Parameters.AddWithValue("@Class", elementViewModel.Class);
        //                        cmd.Parameters.AddWithValue("@Icon", Constants.AnalystDataIconsSource[elementViewModel.Class]);
        //                        cmd.Parameters.AddWithValue("@ActionId", indexAction.ActionId);
        //                        cmd.Parameters.AddWithValue("@DateOfInsert", DateTime.Now.Date);
        //                        await cmd.ExecuteNonQueryAsync();
        //                    }
        //                }
        //                else
        //                {
        //                    // Insert new record
        //                    string insertUserObjectsSql = @"
        //                INSERT INTO user_objects (ObjectKey, Class, Icon, ActionId, DateOfInsert)
        //                VALUES (@GlobalObjectId, @Class, @Icon, @ActionId, @DateOfInsert);";
        //                    using (var cmd = new SQLiteCommand(insertUserObjectsSql, conn))
        //                    {
        //                        cmd.Parameters.AddWithValue("@GlobalObjectId", globalObjectId);
        //                        cmd.Parameters.AddWithValue("@Class", elementViewModel.Class);
        //                        cmd.Parameters.AddWithValue("@Icon", Constants.AnalystDataIconsSource[elementViewModel.Class]);
        //                        cmd.Parameters.AddWithValue("@ActionId", indexAction.ActionId);
        //                        cmd.Parameters.AddWithValue("@DateOfInsert", DateTime.Now.Date);
        //                        await cmd.ExecuteNonQueryAsync();
        //                    }
        //                }
        //            }

        //            transaction.Commit();
        //        }
        //    }
        //}

        public async Task<List<int>> InsertElementsGlobalAndUserAsync(List<string> elements, ElementViewModel elementViewModel, IndexAction indexAction)
        {
            List<int> newlyInsertedIds = new List<int>();

            using (var conn = new SQLiteConnection(_connectionString))
            {
                await conn.OpenAsync();
                using (var transaction = conn.BeginTransaction())
                {
                    foreach (var element in elements)
                    {
                        int globalObjectId;

                        // First, try to get the PrimaryKey for an existing ObjectKey
                        string getPrimaryKeySql = "SELECT PrimaryKey FROM global_objects WHERE ObjectKey = @ObjectKey;";
                        using (var cmd = new SQLiteCommand(getPrimaryKeySql, conn))
                        {
                            cmd.Parameters.AddWithValue("@ObjectKey", element);
                            var result = await cmd.ExecuteScalarAsync();
                            if (result != null)
                            {
                                globalObjectId = Convert.ToInt32(result);
                            }
                            else
                            {
                                // If not existing, insert and get the last inserted row id
                                string insertGlobalSql = @"
                            INSERT INTO global_objects (ObjectKey)
                            VALUES (@ObjectKey);
                            SELECT last_insert_rowid();";
                                using (var insertCmd = new SQLiteCommand(insertGlobalSql, conn))
                                {
                                    insertCmd.Parameters.AddWithValue("@ObjectKey", element);
                                    globalObjectId = Convert.ToInt32(await insertCmd.ExecuteScalarAsync());
                                }
                            }
                        }

                        // Check if the user_objects record exists
                        string checkUserObjectSql = "SELECT 1 FROM user_objects WHERE ObjectKey = @GlobalObjectId AND ActionId = @ActionId;";
                        bool recordExists;
                        using (var cmd = new SQLiteCommand(checkUserObjectSql, conn))
                        {
                            cmd.Parameters.AddWithValue("@GlobalObjectId", globalObjectId);
                            cmd.Parameters.AddWithValue("@ActionId", indexAction.ActionId);
                            recordExists = (await cmd.ExecuteScalarAsync()) != null;
                        }

                        if (!recordExists)
                        {
                            // Insert new record into user_objects
                            string insertUserObjectsSql = @"
                        INSERT INTO user_objects (ObjectKey, Class, Icon, ActionId, DateOfInsert)
                        VALUES (@GlobalObjectId, @Class, @Icon, @ActionId, @DateOfInsert);";
                            using (var cmd = new SQLiteCommand(insertUserObjectsSql, conn))
                            {
                                cmd.Parameters.AddWithValue("@GlobalObjectId", globalObjectId);
                                cmd.Parameters.AddWithValue("@Class", elementViewModel.Class);
                                cmd.Parameters.AddWithValue("@Icon", Constants.AnalystDataIconsSource[elementViewModel.Class]);
                                cmd.Parameters.AddWithValue("@ActionId", indexAction.ActionId);
                                cmd.Parameters.AddWithValue("@DateOfInsert", DateTime.Now.Date);
                                await cmd.ExecuteNonQueryAsync();

                                // Add the globalObjectId to the list of newly inserted IDs
                                newlyInsertedIds.Add(globalObjectId);
                            }
                        }
                        else
                        {
                            // Update existing record in user_objects
                            string updateUserObjectSql = @"
                        UPDATE user_objects
                        SET Class = @Class, Icon = @Icon, DateOfInsert = @DateOfInsert
                        WHERE ObjectKey = @GlobalObjectId AND ActionId = @ActionId;";
                            using (var cmd = new SQLiteCommand(updateUserObjectSql, conn))
                            {
                                cmd.Parameters.AddWithValue("@GlobalObjectId", globalObjectId);
                                cmd.Parameters.AddWithValue("@Class", elementViewModel.Class);
                                cmd.Parameters.AddWithValue("@Icon", Constants.AnalystDataIconsSource[elementViewModel.Class]);
                                cmd.Parameters.AddWithValue("@ActionId", indexAction.ActionId);
                                cmd.Parameters.AddWithValue("@DateOfInsert", DateTime.Now.Date);
                                await cmd.ExecuteNonQueryAsync();
                            }
                        }
                    }

                    transaction.Commit();
                }
            }

            return newlyInsertedIds;
        }

        public async Task<(string Key, string Class, string Title, string Style)> FetchObjectDataAsync(int objectId)
        {
            using (var conn = new SQLiteConnection(_connectionString))
            {
                await conn.OpenAsync();
                string sql = @"
            SELECT go.ObjectKey, uo.Class, go.ObjectKey AS Title, uo.Icon
            FROM user_objects uo
            JOIN global_objects go ON uo.ObjectKey = go.PrimaryKey
            WHERE go.PrimaryKey = @ObjectId;";

                using (var cmd = new SQLiteCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@ObjectId", objectId);
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            return (
                                Key: reader["ObjectKey"].ToString(),
                                Class: reader["Class"].ToString(),
                                Title: reader["Title"].ToString(),
                                Style: $"icon:object/{reader["Icon"]};"
                            );
                        }
                    }
                }
            }
            return default;
        }




        //public async Task InsertElementsGlobalAndUserAsync(List<string> elements, ElementViewModel elementViewModel, IndexAction indexAction)
        //{
        //    using (var conn = new SQLiteConnection(_connectionString))
        //    {
        //        await conn.OpenAsync();
        //        using (var transaction = conn.BeginTransaction())
        //        {
        //            foreach (var element in elements)
        //            {
        //                int globalObjectId;

        //                // First, try to get the PrimaryKey for an existing ObjectKey
        //                string getPrimaryKeySql = "SELECT PrimaryKey FROM global_objects WHERE ObjectKey = @ObjectKey;";
        //                using (var cmd = new SQLiteCommand(getPrimaryKeySql, conn))
        //                {
        //                    cmd.Parameters.AddWithValue("@ObjectKey", element);
        //                    var result = await cmd.ExecuteScalarAsync();
        //                    if (result != null)
        //                    {
        //                        globalObjectId = Convert.ToInt32(result);
        //                    }
        //                    else
        //                    {
        //                        // If not existing, insert and get the last inserted row id
        //                        string insertGlobalSql = @"
        //                    INSERT INTO global_objects (ObjectKey)
        //                    VALUES (@ObjectKey);
        //                    SELECT last_insert_rowid();";
        //                        using (var insertCmd = new SQLiteCommand(insertGlobalSql, conn))
        //                        {
        //                            insertCmd.Parameters.AddWithValue("@ObjectKey", element);
        //                            globalObjectId = Convert.ToInt32(await insertCmd.ExecuteScalarAsync());
        //                        }
        //                    }
        //                }

        //                // Now insert into user_objects using the obtained globalObjectId
        //                string insertUserObjectsSql = @"
        //        INSERT INTO user_objects (ObjectKey, Class, Icon, ActionId, DateOfInsert)
        //        SELECT @GlobalObjectId, @Class, @Icon, @ActionId, @DateOfInsert
        //        WHERE NOT EXISTS (
        //            SELECT 1 FROM user_objects WHERE ObjectKey = @GlobalObjectId AND ActionId = @ActionId
        //        );";

        //                using (var cmd = new SQLiteCommand(insertUserObjectsSql, conn))
        //                {
        //                    cmd.Parameters.AddWithValue("@GlobalObjectId", globalObjectId);
        //                    cmd.Parameters.AddWithValue("@Class", elementViewModel.Class);
        //                    cmd.Parameters.AddWithValue("@Icon", Constants.AnalystDataIconsSource[elementViewModel.Class]);
        //                    cmd.Parameters.AddWithValue("@ActionId", indexAction.ActionId);
        //                    cmd.Parameters.AddWithValue("@DateOfInsert", DateTime.Now);
        //                    await cmd.ExecuteNonQueryAsync();
        //                }
        //            }

        //            transaction.Commit();
        //        }
        //    }
        //}



    }
}
