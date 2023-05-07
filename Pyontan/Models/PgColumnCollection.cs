using Pyontan.Database;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pyontan.Models
{
    public class PgColumnCollection :IEnumerable<PgColumn>
    {
        private List<PgColumn> _columns = null;

        private void Initialize()
        {
            var sb = new System.Text.StringBuilder();
            sb.AppendLine("SELECT");
            sb.AppendLine(" column_name");
            sb.AppendLine(",ordinal_position");
            sb.AppendLine(",is_nullable");
            sb.AppendLine(",data_type");
            sb.AppendLine(",character_maximum_length");
            sb.AppendLine(",numeric_precision");
            sb.AppendLine(",numeric_scale");
            sb.AppendLine(",datetime_precision");
            sb.AppendLine(",collation_name");
            sb.AppendLine("FROM");
            sb.AppendLine(" information_schema.columns");
            sb.AppendLine("WHERE");
            sb.AppendLine("table_schema = @table_schema");
            sb.AppendLine("AND table_name = @table_name");
            sb.AppendLine("ORDER BY");
            sb.AppendLine(" ordinal_position");
            using (var q = new PgQuery())
            {
                _columns =
                    q.GetSqlResult(sb.ToString(), new Dictionary<string, object>
                    {
                        {"table_schema",this.Table.Schema.Name },
                        {"table_name",this.Table.Name }
                    }).Rows.Select(x => x.Create<PgColumn, PgTable>(this.Table)).ToList();
            }
        }
        private void InitTypes()
        {
            using (var q = new PgQuery())
            {
                var columnTypes = q.GetColumnTypes(this.Table.Schema.Name, this.Table.Name);
                foreach (var columnType in columnTypes)
                {
                    this[columnType.Key].Type = columnType.Value;
                }
            }
        }
        public PgColumnCollection(PgTable table)
        {
            this.Table = table;
            Initialize();
            InitTypes();
        }
        public PgTable Table
        {
            get;
        }
        public PgColumn this[int index]
        {
            get
            {
                return _columns[index];
            }
        }
        public PgColumn this[string name]
        {
            get
            {
                return _columns.Where(x => x.Name.Equals(name, StringComparison.OrdinalIgnoreCase)).Single();
            }
        }
        public IEnumerator<PgColumn> GetEnumerator()
        {
            return ((IEnumerable<PgColumn>)_columns).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)_columns).GetEnumerator();
        }
    }
}
