using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pyontan.Database
{
    public class PgQuery : DbQueryBase
    {
        private static string _connectionString;
        public PgQuery(string connectionString) : base(connectionString)
        {
        }
        public PgQuery() : base(_connectionString)
        {
        }

        public static void SetConnectionString(string value)
        {
            using (var conn = new NpgsqlConnection(value))
            {
                conn.Open();
                _connectionString = value;
            }
        }

        protected override IDbConnection GenerateConnection(string connectionString)
        {
            return new NpgsqlConnection(connectionString);
        }

        protected override IDbDataParameter CreateParameter(string name, object value)
        {
            return new NpgsqlParameter(name, value);
        }
    }
}
