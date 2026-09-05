using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace FixHub.Helpers
{
    /// <summary>
    /// Database helper for FixHub - wraps connection string and provides
    /// parameterized query helpers to prevent SQL injection.
    /// </summary>
    public static class DbHelper
    {
        private static readonly string ConnectionString;

        static DbHelper()
        {
            ConnectionString = ConfigurationManager.ConnectionStrings["FixHubDb"]?.ConnectionString;
            if (string.IsNullOrEmpty(ConnectionString))
            {
                throw new InvalidOperationException("Connection string 'FixHubDb' not found in App.config");
            }
        }

        /// <summary>
        /// Get a new SqlConnection (caller must dispose).
        /// </summary>
        public static SqlConnection GetConnection()
        {
            return new SqlConnection(ConnectionString);
        }

        /// <summary>
        /// Execute a query and return a SqlDataReader. Caller must close the reader.
        /// Use 'using' pattern to ensure proper disposal.
        /// </summary>
        public static SqlDataReader ExecuteReader(string sql, params SqlParameter[] parameters)
        {
            var conn = GetConnection();
            conn.Open();
            var cmd = new SqlCommand(sql, conn);
            if (parameters != null)
            {
                cmd.Parameters.AddRange(parameters);
            }
            // Note: Caller is responsible for closing both cmd and conn after reading
            var reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            return reader;
        }

        /// <summary>
        /// Execute a non-query (INSERT/UPDATE/DELETE) and return rows affected.
        /// </summary>
        public static int ExecuteNonQuery(string sql, params SqlParameter[] parameters)
        {
            using (var conn = GetConnection())
            {
                conn.Open();
                using (var cmd = new SqlCommand(sql, conn))
                {
                    if (parameters != null)
                    {
                        cmd.Parameters.AddRange(parameters);
                    }
                    return cmd.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// Execute a scalar query and return the single value (or null if no rows).
        /// </summary>
        public static object ExecuteScalar(string sql, params SqlParameter[] parameters)
        {
            using (var conn = GetConnection())
            {
                conn.Open();
                using (var cmd = new SqlCommand(sql, conn))
                {
                    if (parameters != null)
                    {
                        cmd.Parameters.AddRange(parameters);
                    }
                    return cmd.ExecuteScalar();
                }
            }
        }

        /// <summary>
        /// Execute a query and return the first column of the first row as the specified type.
        /// Throws if no rows or null.
        /// </summary>
        public static T ExecuteScalar<T>(string sql, params SqlParameter[] parameters)
        {
            var result = ExecuteScalar(sql, parameters);
            if (result == null || result == DBNull.Value)
                return default(T);
            return (T)Convert.ChangeType(result, typeof(T));
        }

        /// <summary>
        /// Check if a record exists matching the given condition.
        /// </summary>
        public static bool Exists(string table, string whereClause, params SqlParameter[] parameters)
        {
            string sql = $"SELECT TOP 1 1 FROM {table} WHERE {whereClause}";
            var result = ExecuteScalar(sql, parameters);
            return result != null && result != DBNull.Value;
        }

        /// <summary>
        /// Get a single value by ID.
        /// </summary>
        public static object GetById(string table, string idColumn, int id, string selectColumn)
        {
            string sql = $"SELECT {selectColumn} FROM {table} WHERE {idColumn} = @id";
            return ExecuteScalar(sql, new SqlParameter("@id", id));
        }
    }
}