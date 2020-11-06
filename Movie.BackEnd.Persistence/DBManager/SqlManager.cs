using Movie.BackEnd.Common.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Movie.BackEnd.Persistence.DBManager
{
    public class SqlManager<T> : IDBManager<T> where T : class, new()
    {
        
        public bool AddReturnParamIfMissing { get; set; }
        private async void PrepareTransaction(SqlConnection connection, DbTransaction dbTransaction, Boolean reqTrans)
        {
            if (!reqTrans)
            {
                await connection.OpenAsync();
                dbTransaction = await connection.BeginTransactionAsync(IsolationLevel.ReadCommitted);
            }
            else
            {
                if (dbTransaction.IsNull()) throw new Exception("Transaction is not initialized.");
            }
        }
        public async Task<DbCommand> CreateCommand(string commandText, CommandType commandType, DbConnection connection)
        {
            Task<DbCommand> task = new Task<DbCommand>(() =>
            {
                return new SqlCommand { CommandText = commandText, Connection = (SqlConnection)connection, CommandType = commandType };

            });
            task.Start();
            return await task;

        }
        public async void CommitTransaction(SqlConnection connection, DbTransaction dbTransaction)
        {
            if (dbTransaction.IsNotNull())
            {
                await dbTransaction.CommitAsync();
                await ConnectionClose(connection);
            }
            else
                throw new Exception("Transaction Object not Initialized");
        }
        private static async Task ConnectionClose(SqlConnection connection)
        {
            await connection.CloseAsync();
            await connection.DisposeAsync();
        }
        public async void RollBack(SqlConnection connection, DbTransaction dbTransaction)
        {
            if (dbTransaction.IsNotNull())
            {
                await dbTransaction.RollbackAsync();
                await ConnectionClose(connection);
            }
            else
                throw new Exception("Transaction Object not Initialized");
        }
        public async Task<SqlParameter[]> CreateParameter<Tentity>(Tentity model) where Tentity : class
        {
            Task<SqlParameter[]> task = new Task<SqlParameter[]>(() =>
            {
                PropertyInfo[] properties = model.GetType().GetProperties().ToArray();

                List<SqlParameter> parameters = new List<SqlParameter>();

                foreach (PropertyInfo property in properties)
                {
                    parameters.Add(new SqlParameter(string.Format("@{0}", property.Name), property.GetValue(model)));
                }

                return ((parameters.Count > 0) ? parameters.ToArray() : null);

            });
            task.Start();
            return await task;
            
        }
        public async Task<int> ExecuteCommandAsync(string spQuery, Hashtable ht, string conString)
        {
            int result = 0;
            try
            {
                using (SqlConnection con = new SqlConnection(conString))
                {
                    await con.OpenAsync().ConfigureAwait(false);

                    DbCommand command = con.CreateCommand();
                    command.CommandText = spQuery;
                    command.CommandType = CommandType.StoredProcedure;
                    command.Connection = con;

                    foreach (object obj in ht.Keys)
                    {
                        string str = Convert.ToString(obj);
                        SqlParameter parameter = new SqlParameter("@" + str, ht[obj]);
                        command.Parameters.Add(parameter);
                    }

                    using (DbDataReader reader = await command.ExecuteReaderAsync().ConfigureAwait(false))
                    {
                        while (await reader.ReadAsync().ConfigureAwait(false))
                        {
                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                result = reader.GetInt32(i);
                            }
                        }
                    }
                    command.Parameters.Clear();
                }
            }
            catch (Exception Ex)
            {

                throw Ex;
            }

            return result;
        }
        public async Task<string> ExecuteCommandStringAsync(string spQuery, Hashtable ht, string conString)
        {
            string result = string.Empty;
            try
            {
                using (SqlConnection con = new SqlConnection(conString))
                {
                    await con.OpenAsync().ConfigureAwait(false);

                    DbCommand command = con.CreateCommand();
                    command.CommandText = spQuery;
                    command.CommandType = CommandType.StoredProcedure;
                    command.Connection = con;

                    foreach (object obj in ht.Keys)
                    {
                        string str = Convert.ToString(obj);
                        SqlParameter parameter = new SqlParameter("@" + str, ht[obj]);
                        command.Parameters.Add(parameter);
                    }

                    IDataReader dr = await command.ExecuteReaderAsync().ConfigureAwait(false);
                    if (dr.Read())
                    {
                        result = Convert.ToString(dr.GetString(0));
                    }
                    command.Parameters.Clear();
                }
            }
            catch (Exception Ex)
            {

                throw Ex;
            }

            return result;
        }
        public async Task<List<T>> ExecuteCommandListAsync(string spQuery, Hashtable ht, string conString)
        {
            List<T> Results = null;
            try
            {
                using (SqlConnection con = new SqlConnection(conString))
                {
                    await con.OpenAsync().ConfigureAwait(false);
                    DbCommand command = con.CreateCommand();
                    command.CommandText = spQuery;
                    command.CommandType = CommandType.StoredProcedure;
                    command.Connection = con;

                    foreach (object obj in ht.Keys)
                    {
                        string str = Convert.ToString(obj);
                        SqlParameter parameter = new SqlParameter("@" + str, ht[obj]);
                        command.Parameters.Add(parameter);
                    }

                    Results = DataReaderMapToList<T>(await command.ExecuteReaderAsync().ConfigureAwait(false));
                    command.Parameters.Clear();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Results;

        }
        public async Task<T> ExecuteCommandSingleAsync(string spQuery, Hashtable ht, string conString)
        {
            T Results = null;
            try
            {
                using (SqlConnection con = new SqlConnection(conString))
                {
                    await con.OpenAsync().ConfigureAwait(false);
                    //SqlCommand cmd = new SqlCommand();
                    DbCommand command = con.CreateCommand();
                    command.CommandText = spQuery;
                    command.CommandType = CommandType.StoredProcedure;
                    command.Connection = con;

                    foreach (object obj in ht.Keys)
                    {
                        string str = Convert.ToString(obj);
                        SqlParameter parameter = new SqlParameter("@" + str, ht[obj]);
                        command.Parameters.Add(parameter);
                    }

                    Results = DataReaderMapToList<T>(await command.ExecuteReaderAsync().ConfigureAwait(false)).FirstOrDefault();
                    command.Parameters.Clear();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return Results;
        }
        public async Task<T> ExecuteQuerySingleAsync(string spQuery, Hashtable ht, string conString)
        {
            T Results = null;

            try
            {
                using (SqlConnection con = new SqlConnection(conString))
                {
                    await con.OpenAsync().ConfigureAwait(false);
                    //SqlCommand cmd = new SqlCommand();
                    DbCommand command = con.CreateCommand();
                    command.CommandText = spQuery;
                    command.CommandType = CommandType.StoredProcedure;
                    command.Connection = con;

                    foreach (object obj in ht.Keys)
                    {
                        string str = Convert.ToString(obj);
                        SqlParameter parameter = new SqlParameter("@" + str, ht[obj]);
                        command.Parameters.Add(parameter);
                    }

                    Results = DataReaderMapToList<T>(await command.ExecuteReaderAsync().ConfigureAwait(false)).FirstOrDefault();
                    command.Parameters.Clear();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return Results;
        }
        public async Task<List<T>> ExecuteQueryAsync(string spQuery, Hashtable ht, string conString)
        {
            List<T> Results = null;
            try
            {
                using (SqlConnection con = new SqlConnection(conString))
                {
                    await con.OpenAsync().ConfigureAwait(false);
                    //SqlCommand cmd = new SqlCommand();
                    DbCommand command = con.CreateCommand();
                    command.CommandText = spQuery;
                    command.CommandType = CommandType.StoredProcedure;
                    command.Connection = con;

                    foreach (object obj in ht.Keys)
                    {
                        string str = Convert.ToString(obj);
                        SqlParameter parameter = new SqlParameter("@" + str, ht[obj]);
                        command.Parameters.Add(parameter);
                    }

                    using (IDataReader reader = await command.ExecuteReaderAsync().ConfigureAwait(false))
                    {
                        //Results =  DataReaderMapToList<T>(reader).ToList();
                        Results = DataReaderMapToList<T>(reader).ToList();

                    }

                    command.Parameters.Clear();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return Results;
        }
        public List<T> DataReaderMapToList<Tentity>(IDataReader reader)
        {
            var results = new List<T>();

            var columnCount = reader.FieldCount;
            while (reader.Read())
            {
                var item = Activator.CreateInstance<T>();
                try
                {
                    var rdrProperties = Enumerable.Range(0, columnCount).Select(i => reader.GetName(i)).ToArray();
                    foreach (var property in typeof(T).GetProperties())
                    {
                        if ((typeof(T).GetProperty(property.Name).GetGetMethod().IsVirtual) || (!rdrProperties.Contains(property.Name)))
                        {
                            continue;
                        }
                        else
                        {
                            if (!reader.IsDBNull(reader.GetOrdinal(property.Name)))
                            {
                                Type convertTo = Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType;
                                property.SetValue(item, Convert.ChangeType(reader[property.Name], convertTo), null);
                            }
                        }
                    }
                    results.Add(item);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return results;
        }
        public async Task<int> ExecuteNonQueryTransAsync(string spQuery, string connStr, SqlParameter[] parameters)
        {
            int result = -1;
            AddReturnParamIfMissing = true;
            DbTransaction dbTransaction = null;
            SqlParameter rowsAffected = null;
            try
            {
                using (SqlConnection con = new SqlConnection(connStr))
                {
                    PrepareTransaction(con, dbTransaction, false);
                    using (var command = await CreateCommand(spQuery, CommandType.StoredProcedure, con))
                    {
                        command.Transaction = dbTransaction;

                        if (parameters != null)
                        {
                            command.Parameters.AddRange(parameters);
                            // if this is a stored proc and we want to add a return param

                            // see if we already have a return parameter
                            rowsAffected = parameters.FirstOrDefault(x => x.Direction == ParameterDirection.ReturnValue);
                            // if we don't, add one.
                            if (rowsAffected == null)
                            {
                                rowsAffected = new SqlParameter("@rowsAffected", SqlDbType.Int);
                                rowsAffected.Direction = ParameterDirection.ReturnValue;
                                command.Parameters.Add(rowsAffected);
                                
                            }
                        }
                        try
                        {
                            result = await command.ExecuteNonQueryAsync();
                            result = (rowsAffected != null) ? (int)rowsAffected.Value : result;
                            CommitTransaction(con, dbTransaction);
                        }
                        catch (Exception Ex)
                        {
                            RollBack(con, dbTransaction);
                            throw Ex;
                        }
                        finally
                        {
                            await ConnectionClose(con);
                        }
                    }
                }
                return result;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
        public async void NonReturnExecuteNonQueryAsync(string spQuery, string connStr)
        {
            try
            {
                using (var conn = new SqlConnection(connStr))
                {
                    await conn.OpenAsync();
                    var cmd = new SqlCommand(spQuery, conn, conn.BeginTransaction());
                    try
                    {
                        await cmd.ExecuteNonQueryAsync();
                        await cmd.Transaction.CommitAsync();
                    }
                    catch (Exception ex)
                    {

                        try
                        {
                            await cmd.Transaction.RollbackAsync();
                        }
                        catch (Exception Ex)
                        {
                            throw Ex;
                        }
                        throw ex;
                    }

                    conn.Close();
                }
            }
            catch (Exception)
            {

                throw;
            }
            
        }

        public async Task<object> ExecuteScalarAsync(string commandText, string conString)
        {
            object result = null;
            try
            {
                using (SqlConnection con = new SqlConnection(conString))
                {

                    await con.OpenAsync();
                    DbCommand command = con.CreateCommand();
                    command.CommandText = commandText;
                    command.CommandType = CommandType.Text;
                    command.Connection = con;
                    try
                    {
                        result = await command.ExecuteScalarAsync();
                    }
                    catch (Exception InEx)
                    {

                        throw InEx;
                    }

                    con.Close();

                }
            }
            catch (Exception FinalEx)
            {

                throw FinalEx;
            }
            return result;
        }

    }
}
