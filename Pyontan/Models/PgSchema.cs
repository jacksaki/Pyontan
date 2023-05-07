using MahApps.Metro.Controls;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Pyontan.Database;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pyontan.Models
{
    public class PgSchema
    {
        private static Dictionary<string, IEnumerable<PgTable>> _allTables = new Dictionary<string, IEnumerable<PgTable>>();

        private IEnumerable<PgTable> GetTables()
        {
            var sb = new System.Text.StringBuilder();
            sb.AppendLine("SELECT");
            sb.AppendLine(" table_name");
            sb.AppendLine(",table_type");
            sb.AppendLine("FROM");
            sb.AppendLine(" information_schema.tables");
            sb.AppendLine("WHERE");
            sb.AppendLine("table_schema = @table_schema");
            sb.AppendLine("ORDER BY");
            sb.AppendLine(" table_name");
            sb.AppendLine(",table_type");
            using (var q = new PgQuery())
            {
                foreach (var row in q.GetSqlResult(sb.ToString(), new Dictionary<string, object> { { "table_schema", this.Name } }).Rows)
                {
                    yield return row.Create<PgTable, PgSchema>(this);
                }
            }
        }

        public IEnumerable<PgTable> GetAllTables()
        {
            if(_allTables.TryGetValue(this.Name,out var ret))
            {
                return ret;
            }
            var results = GetTables();
            _allTables.Add(this.Name, results);
            return results;
        }

        public static IEnumerable<PgSchema> GetAll()
        {
            var sb = new System.Text.StringBuilder();
            sb.AppendLine("SELECT");
            sb.AppendLine(" schema_name");
            sb.AppendLine(",schema_owner");
            sb.AppendLine("FROM");
            sb.AppendLine(" information_schema.schemata");
            sb.AppendLine("ORDER BY");
            sb.AppendLine(" schema_name");
            using (var q = new PgQuery())
            {
                foreach(var row in q.GetSqlResult(sb.ToString(), null).Rows)
                {
                    yield return row.Create<PgSchema>();
                }
            }
        }
        [DbColumn("schema_name")]
        public string Name
        {
            get;
            private set;
        }
        [DbColumn("schema_owner")]
        public string SchemaOwner
        {
            get;
            private set;
        }
    }
}
