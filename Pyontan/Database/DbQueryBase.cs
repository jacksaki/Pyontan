using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pyontan
{
    public abstract class DbQueryBase : IDbQuery
    {

        protected bool disposedValue;
        public IDbConnection Connection
        {
            get;
        }
        public DbQueryBase(string connectionString)
        {
            this.Connection = GenerateConnection(connectionString);
            this.Connection.Open();
        }
        protected abstract IDbConnection GenerateConnection(string connectionString);

        public IDbTransaction BeginTransaction()
        {
            return this.Connection.BeginTransaction();
        }

        public IDictionary<string,Type>GetColumnTypes(string schema, string tableName)
        {
            var sb = new System.Text.StringBuilder();
            sb.AppendLine("SELECT");
            sb.AppendLine(" *");
            sb.AppendLine("FROM");
            sb.AppendLine($" \"{schema}\".\"{tableName}\"");
            sb.AppendLine("WHERE");
            sb.AppendLine("1 = 2");
            using(var cmd = GenerateCommand(sb.ToString(), null))
            using(var dr = cmd.ExecuteReader(CommandBehavior.SchemaOnly))
            {
                var columnCount = dr.FieldCount;
                var result = new Dictionary<string, Type>();
                for(var i = 0; i < columnCount; i++)
                {
                    result.Add(dr.GetName(i), dr.GetFieldType(i));
                }
                return result;
            }
        }
        protected virtual IDbCommand GenerateCommand(string sql, IDictionary<string, object> param)
        {
            var cmd = this.Connection.CreateCommand();
            cmd.CommandText = sql;
            cmd.CommandType = CommandType.Text;
            if (param != null)
            {
                foreach (var p in param)
                {
                    cmd.Parameters.Add(CreateParameter(p.Key, p.Value));
                }
            }
            return cmd;
        }
        protected abstract IDbDataParameter CreateParameter(string name, object value);

        public SqlResultRow GetFirstRow(string sql, IDictionary<string, object> param)
        {
            return GetSqlResult(sql, param, 1).Rows.FirstOrDefault();
        }
        public int ExecuteNonQuery(string sql, IDictionary<string, object> param)
        {
            return GenerateCommand(sql, param).ExecuteNonQuery();
        }

        public object ExecuteScalar(string sql, IDictionary<string, object> param)
        {
            return GenerateCommand(sql, param).ExecuteScalar();
        }
        public virtual DataTable GetDataTable(string sql, IDictionary<string, object> param, int? fetchSize)
        {
            using (var cmd = GenerateCommand(sql, param))
            using (var dr = cmd.ExecuteReader())
            {
                var dt = new System.Data.DataTable();
                dt.Load(dr);
                return dt;
            }
        }
        public DataTable GetDataTable(string sql, IDictionary<string, object> param)
        {
            return GetDataTable(sql, param, null);
        }
        public virtual SqlResult GetSqlResult(string sql, IDictionary<string, object> param, int? fetchSize)
        {
            using (var cmd = GenerateCommand(sql, param))
            using (var dr = cmd.ExecuteReader())
            {
                return new SqlResult(dr);
            }
        }
        public SqlResult GetSqlResult(string sql, IDictionary<string, object> param)
        {
            return GetSqlResult(sql, param, null);
        }


        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: マネージド状態を破棄します (マネージド オブジェクト)
                }
                try
                {
                    this.Connection?.Close();
                    this.Connection?.Dispose();
                }
                catch
                {
                }
                // TODO: アンマネージド リソース (アンマネージド オブジェクト) を解放し、ファイナライザーをオーバーライドします
                // TODO: 大きなフィールドを null に設定します
                disposedValue = true;
            }
        }

        // // TODO: 'Dispose(bool disposing)' にアンマネージド リソースを解放するコードが含まれる場合にのみ、ファイナライザーをオーバーライドします
        // ~OracleQuery()
        // {
        //     // このコードを変更しないでください。クリーンアップ コードを 'Dispose(bool disposing)' メソッドに記述します
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            // このコードを変更しないでください。クリーンアップ コードを 'Dispose(bool disposing)' メソッドに記述します
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}