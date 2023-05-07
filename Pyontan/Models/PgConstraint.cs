using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pyontan.Models
{
    public enum ConstraintType
    {
        [EnumText("p")]
        PrimaryKey,
        [EnumText("u")]
        UniqueKey,
    }
    public class PgConstraint
    {
        [DbColumn("constraint_name")]
        public string Name
        {
            get;
            private set;
        }
        [DbColumn("constraint_type")]
#pragma warning disable CS0649 // フィールド 'PgConstraint._constraintType' は割り当てられません。常に既定値 null を使用します
        private string _constraintType;
#pragma warning restore CS0649 // フィールド 'PgConstraint._constraintType' は割り当てられません。常に既定値 null を使用します
        public ConstraintType ConstraintType
        {
            get
            {
                if (_constraintType == "p")
                {
                    return ConstraintType.PrimaryKey;
                }
                else if (_constraintType == "u")
                {
                    return ConstraintType.UniqueKey;
                }
                else
                {
                    throw new NotImplementedException($"Constraint type {_constraintType} is not implemented.");
                }
            }
        }
        [DbColumn("table_name")]
        public string TableName
        {
            get;
            private set;
        }
        [DbColumn("schema_name")]
        public string SchemaName
        {
            get;
            private set;
        }
    }
}
