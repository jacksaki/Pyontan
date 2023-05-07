using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pyontan.Models
{
    public class PrimaryKeyCollection
    {
        public PrimaryKeyCollection(PgTable table)
        {
            this.Table = table;
            Initialize();
        }
        public PgTable Table
        {
            get;
        }
        private void Initialize()
        {

        }
    }
}
