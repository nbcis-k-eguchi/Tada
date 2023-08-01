using Microsoft.Data.SqlClient;
using System.Data;

namespace Tada.Server.Database
{
    /// <summary>
    /// SQLServerデータアクセス用
    /// </summary>
    public class SqlServerConnetion 
    {
        private static string _connectionString = "";

        public SqlServerConnetion()
        {
            if (_connectionString == "")
            {
                var appSettings = new ConfigurationManager();

                var connectionString = appSettings.GetConnectionString("SqlServer");
                if (connectionString != null)
                {
                    _connectionString = connectionString;
                }
                else
                {
                    // 接続文字列が取得できない場合はエラー
                    throw new Exception("接続文字列が取得できませんでした。");
                }
            }
                
        }
        public SqlServerConnetion(string connectionString)
        {
            _connectionString = connectionString;
        }
        public SqlConnection GetConnection()
        {
            return new SqlConnection(_connectionString);
        }

        // Query
        public List<T> Query<T>(Func<SqlDataReader,T> convertReaderToMddel, string sql, object? param = null)
        {
            var result = new List<T>();
            using (var connection = GetConnection())
            {
                connection.Open();

                // SQLコマンド
                SqlCommand cmd = connection.CreateCommand();
                cmd.CommandText = sql;

                // パラメーター
                if (param != null)
                {
                    foreach (var item in param.GetType().GetProperties())
                    {
                        cmd.Parameters.Add(new SqlParameter(item.Name, item.GetValue(param)));
                    }
                }

                // SQL実行
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    result.Add(convertReaderToMddel(reader));
                }
            }

            return result;
        }

        /// <summary>
        /// SQL発行
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <param name="overZero">実行結果が0超過を実施するか　true:0超過の実施、false:0超過は行わない</param>
        /// <returns>true:成功</returns>
        public bool Execute(string sql, object? param = null, bool overZero = true )
        {
            using (var connection = GetConnection())
            {
                connection.Open();
                // SQLコマンド
                SqlCommand cmd = connection.CreateCommand();
                cmd.CommandText = sql;
                // パラメーター
                if (param != null)
                {
                    foreach (var item in param.GetType().GetProperties())
                    {
                        // DateTime?で値がNullの場合、SqlParameterをDateTimeで作成し、Null許容にして、DBNullを入れる
                        // Null値やDBNullをそのまま入れるとString型と認識してSQLServer側でエラーになる
                        if (item.PropertyType == typeof(DateTime?) && item.GetValue(param) == null)
                        {
                            var dtparam = new SqlParameter(item.Name, SqlDbType.DateTime)
                            {
                                IsNullable = true,
                                Value = DBNull.Value
                            };
                            cmd.Parameters.Add(dtparam);
                        }
                        else
                        {
                            cmd.Parameters.Add(new SqlParameter(item.Name, item.GetValue(param)));
                        }
                    }
                }
                try
                {
                    // SQL実行
                    if (overZero)
                    {
                        return cmd.ExecuteNonQuery() > 0;
                    }
                    else
                    {
                        cmd.ExecuteNonQuery();
                        return true;
                    }
                }
                catch (Exception ex)
                {
                    // TODO:エラーログ出力など適切な例外処理の実装
                    return false;
                }
            }
        }

        public bool InsertExecute(string tableName, string[] fieldNames, object? param)
        {
            string sql = $"INSERT INTO {tableName} ({string.Join(",", fieldNames)}) VALUES ({string.Join(",", fieldNames.Select(x => "@" + x))})";
            return Execute(sql, param);
        }

        public bool InsertExecute(string tableName, string[] fieldNames, object? param, string[] identityFieldNames)
        {
            // Identityフィールドは除外する
            string sql = $"INSERT INTO {tableName} ({string.Join(",", fieldNames.Where(x => !identityFieldNames.Contains(x)))}) VALUES ({string.Join(",", fieldNames.Where(x => !identityFieldNames.Contains(x)).Select(x => "@" + x))})";
            return Execute(sql, param);
        }

        public bool UpdateExecute(string tableName, string[] fieldNames, object? param, string where)
        {
            string sql = $"UPDATE {tableName} SET {string.Join(",", fieldNames.Select(x => x + "=@" + x))} WHERE {where}";
            return Execute(sql, param);
        }

        public bool DeleteExecute(string tableName, string where)
        {
            string sql = $"DELETE FROM {tableName} WHERE {where}";
            return Execute(sql);
        }

    }
}
