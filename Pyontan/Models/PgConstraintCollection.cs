using Pyontan.Database;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pyontan.Models
{
    public class PgConstraintCollection
    {
        private static Dictionary<PgConstraint,IEnumerable<string>> _dict = null;
        public static Dictionary<PgConstraint, IEnumerable<string>> GetAll(PgTable table)
        {
            if (_dict == null)
            {
                Initialize();
            }
            if (!_dict.Where(x => x.Key.TableName == table.Name && x.Key.SchemaName == table.Schema.Name).Any())
            {
                return null;
            }
            if (!_dict.Where(x => x.Key.TableName == table.Name && x.Key.SchemaName == table.Schema.Name && x.Value==null).Any())
            {
                Initialize(table);
            }
            return _dict.Where(x => x.Key.TableName == table.Name && x.Key.SchemaName == table.Schema.Name).ToDictionary(x => x.Key, y => y.Value);
        }

        private static void Initialize(PgTable table)
        {
            foreach(var constraint in _dict.Where(x => x.Key.TableName == table.Name && x.Key.SchemaName == table.Schema.Name).Select(x=>x.Key))
            {
                _dict[constraint] = GetColumns(constraint);
            }
        }
        private static IEnumerable<string> GetColumns(PgConstraint constraint)
        {
            var sb = new System.Text.StringBuilder();
            sb.AppendLine("SELECT");
            sb.AppendLine(" column_name");
            sb.AppendLine("FROM");
            sb.AppendLine(" information_schema.key_column_usage");
            sb.AppendLine("WHERE");
            sb.AppendLine("constraint_name = @constraint_name");
            sb.AppendLine("AND table_schema = @table_schema");
            sb.AppendLine("AND table_name = @table_name");
            sb.AppendLine("ORDER BY");
            sb.AppendLine(" ordinal_position");
            using(var q= new PgQuery())
            {
                return q.GetSqlResult(sb.ToString(), new Dictionary<string, object>
                {
                    {"constraint_name",constraint.Name },
                    {"table_schema",constraint.SchemaName },
                    {"table_name",constraint.TableName }
                }).Rows.Select(x => x["column_name"].ToString());
            }
        }
        private static void Initialize()
        {
            _dict = new Dictionary<PgConstraint, IEnumerable<string>>();
            var sb = new System.Text.StringBuilder();

            sb.AppendLine("SELECT");
            sb.AppendLine(" c.contype AS constraint_type");
            sb.AppendLine(",c.conname AS constraint_name");
            sb.AppendLine(",c.connamespace::regnamespace::text AS schema_name");
            sb.AppendLine(",c.conrelid::regclass::text AS table_name");
            sb.AppendLine("FROM");
            sb.AppendLine(" pg_constraint c");
            sb.AppendLine("LEFT OUTER JOIN pg_namespace n ON (n.oid = c.connamespace)");
            sb.AppendLine("WHERE");
            sb.AppendLine("c.contype IN ('p', 'u')");
            sb.AppendLine("ORDER BY");
            sb.AppendLine(" table_name");
            sb.AppendLine(",constraint_name");

            using (var q = new PgQuery())
            {
                _dict = q.GetSqlResult(sb.ToString(), null).Rows.Select(x => x.Create<PgConstraint>()).ToDictionary(x => x, y => (IEnumerable<string>)null);
            }
        }
    }
}
