using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pyontan.Models
{
    public class PgColumn
    {
        public PgColumn(PgTable owner)
        {
            this.Owner = owner;
        }

        public bool IsKey
        {
            get
            {
                if (this.Owner.Constraints.Where(x => x.Key.ConstraintType == ConstraintType.PrimaryKey && x.Value.Contains(this.Name)).Any())
                {
                    return true;
                }
                if (this.Owner.Constraints.Where(x => x.Key.ConstraintType == ConstraintType.PrimaryKey).Any())
                {
                    return this.Owner.Constraints.Where(x => x.Value.Contains(this.Name)).Any();
                }
                return false;
            }
        }
        public PgTable Owner
        {
            get;
        }
        public Type Type
        {
            get;
            internal set;
        }

        private static List<Type> RequireTypeNameList = new List<Type> {
            typeof(string),
            typeof(DateTime),
        };
        private bool RequireTypeName
        {
            get
            {
                return RequireTypeNameList.Where(x => x == this.Type).Any();
            }
        }
        public string TypeNameWithLength
        {
            get
            {
                if (this.RequireTypeName)
                {
                    return null;
                }
                if (this.DateTimePrecision.HasValue)
                {
                    return $"{this.DataTypeName}({this.DateTimePrecision})";
                }
                if (this.CharacterMaximumLength.HasValue)
                {
                    return $"{this.DataTypeName}({this.CharacterMaximumLength})";
                }
                return null;
            }
        }
        [DbColumn("column_name")]
        public string Name
        {
            get;
            private set;
        }
        [DbColumn("ordinal_position")]
        public int OrdinalPosition
        {
            get;
            private set;
        }
        [DbColumn("is_nullable")]
        public bool IsNullable
        {
            get;
            private set;
        }
        [DbColumn("data_type")]
        public string DataTypeName
        {
            get;
            private set;
        }
        [DbColumn("character_maximum_length")]
        public int? CharacterMaximumLength
        {
            get;
            private set;
        }
        [DbColumn("numeric_precision")]
        public int? NumericPrecision
        {
            get;
            private set;
        }
        [DbColumn("numeric_scale")]
        public int? NumericScale
        {
            get;
            private set;
        }
        [DbColumn("datetime_precision")]
        public int? DateTimePrecision
        {
            get;
            private set;
        }
        [DbColumn("collation_name")]
        public string CollationName
        {
            get;
            private set;
        }
    }
}
