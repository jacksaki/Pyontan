using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pyontan.Models
{
    public class AdditionalAssembly : AssemblyBase
    {
        public AdditionalAssembly(string path) : base(path)
        {
            this.Path = path;
        }

        public string Path
        {
            get;
        }
        public override bool IsGlobal
        {
            get
            {
                return false;
            }
        }
    }
}
