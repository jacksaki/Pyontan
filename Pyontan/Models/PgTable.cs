using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using Pyontan.Database;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pyontan.Models
{
    public class PgTable
    {
        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }
            if(!(obj is PgTable))
            {
                return false;
            }
            var b = (PgTable)obj;
            return b.Schema.Name == this.Schema.Name && b.Name == this.Name;
        }
        public override int GetHashCode()
        {
            return $"{this.Schema.Name}.{this.Name}".GetHashCode();
        }

        private PgColumnCollection _columns = null;
        public PgColumnCollection Columns
        {
            get
            {
                if (_columns == null)
                {
                    _columns = new PgColumnCollection(this);
                }
                return _columns;
            }
        }
        private void InitColumnTypes()
        {
            using (var q = new PgQuery())
            {
                var columnTypes = q.GetColumnTypes(this.Schema.Name, this.Name);
                foreach(var columnType in columnTypes)
                {
                    this.Columns[columnType.Key].Type = columnType.Value;
                }
            }
        }

        public PgTable(PgSchema schema)
        {
            this.Schema = schema;
        }
        public PgSchema Schema
        {
            get;
        }
        [DbColumn("table_name")]
        public string Name
        {
            get;
            private set;
        }
        [DbColumn("table_type")]
        public string TableType
        {
            get;
            private set;
        }

        public Dictionary<PgConstraint,IEnumerable<string>> Constraints
        {
            get
            {
                return PgConstraintCollection.GetAll(this);
            }
        }
        public IEnumerable<string> Keys
        {
            get
            {
                if (this.Constraints.Where(x => x.Key.ConstraintType == ConstraintType.PrimaryKey).Any())
                {
                    return this.Constraints.Where(x => x.Key.ConstraintType == ConstraintType.PrimaryKey).First().Value;
                }
                else if (this.Constraints.Where(x => x.Key.ConstraintType == ConstraintType.UniqueKey).Any())
                {
                    return this.Constraints.Where(x => x.Key.ConstraintType == ConstraintType.UniqueKey).First().Value;
                }
                else
                {
                    return null;
                }
            }
        }

        public string GetPropertySource()
        {
            return $"public virtual DbSet<{this.Name.ToPascalCase()}> {this.Name.ToPascalCase()} {{ get; set; }}";
        }
        public string GetOnModelCreatingSource()
        {
            if (this.Keys == null)
            {
                return $"modelBuilder.Entity<{this.Name.ToPascalCase()}>().HasNoKey();";

            }
            else
            {
                var sb = new System.Text.StringBuilder();
                if (this.Keys.Count() == 1)
                {
                    sb.AppendLine($"modelBuilder.Entity<{this.Name.ToPascalCase()}>().HasKey(x => x.{this.Keys.First()}");
                }
                else
                {
                    var s = string.Join(",", this.Keys.Select(x => "x." + x));
                    sb.AppendLine($"modelBuilder.Entity<{this.Name.ToPascalCase()}>().HasKey(x => new {{ {s} }}");
                    sb.AppendLine($");");
                }
                sb.AppendLine("}");
                return sb.ToString();
            }
        }
        public string GetDbSetSource()
        {
            var sb = new System.Text.StringBuilder();
            sb.AppendLine($"[Table(\"{this.Name}\")]");
            sb.AppendLine($"public class {this.Name.ToPascalCase()} {{");

            foreach(var col in this.Columns)
            {
                if (col.IsKey)
                {
                    sb.AppendLine("[Key]");
                }
                else if (!col.IsNullable)
                {
                    sb.AppendLine("[Required]");

                }
                if (col.TypeNameWithLength != null)
                {
                    sb.AppendLine($"[Column(\"{col.Name}\",TypeName = \"{col.TypeNameWithLength}\")]");
                }
                else
                {
                    sb.AppendLine($"[Column(\"{col.Name}\")]");
                }

                sb.AppendLine($"public {col.Type.ToCSharpTypeName()} {col.Name.ToPascalCase()} {{ get; set; }}");
            }
            sb.AppendLine("}");
            return sb.ToString();
        }
    }
}
