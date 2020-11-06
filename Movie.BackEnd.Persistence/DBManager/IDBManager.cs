using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Movie.BackEnd.Persistence.DBManager
{
    public interface IDBManager<T> where T : class
    {
        Task<int> ExecuteCommandAsync(string spQuery, Hashtable oHashtable, string conString);
        Task<string> ExecuteCommandStringAsync(string spQuery, Hashtable oHashtable, string conString);
        Task<List<T>> ExecuteCommandListAsync(string spQuery, Hashtable oHashtable, string conString);
        Task<T> ExecuteCommandSingleAsync(string spQuery, Hashtable oHashtable, string conString);
        Task<T> ExecuteQuerySingleAsync(string spQuery, Hashtable oHashtable, string conString);
        Task<List<T>> ExecuteQueryAsync(string spQuery, Hashtable oHashtable, string conString);
        List<T> DataReaderMapToList<Tentity>(IDataReader reader);
        Task<SqlParameter[]> CreateParameter<Tentity>(Tentity model) where Tentity : class;
        Task<int> ExecuteNonQueryTransAsync(string spQuery, string connStr, SqlParameter[] parameters);
        void NonReturnExecuteNonQueryAsync(string spQuery, string connStr);

        Task<object> ExecuteScalarAsync(string commandText, string connStr);
    }
}
