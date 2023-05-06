using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pyontan.Models
{
    public class MemoryStreamExEventArgs : EventArgs
    {
        public byte[] Buffer { get; }
        public int Offset { get; }
        public int Count { get; }

        public MemoryStreamExEventArgs(byte[] buffer, int offset, int count)
        {
            Buffer = buffer;
            Offset = offset;
            Count = count;
        }
    }
}
