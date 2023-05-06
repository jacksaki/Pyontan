using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pyontan.Models
{
    public class MemoryStreamEx:MemoryStream
    {
        public event EventHandler<MemoryStreamExEventArgs> Written = delegate { };

        public override void Write(byte[] buffer, int offset, int count)
        {
            base.Write(buffer, offset, count);
            Written.Invoke(this, new MemoryStreamExEventArgs(buffer, offset, count));
        }
    }
}
