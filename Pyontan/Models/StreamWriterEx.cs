using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pyontan.Models
{
    public class StreamWriterEx : StreamWriter
    {
        public event EventHandler<StreamWriterExEventArgs> Written = delegate { };

        public StreamWriterEx(Stream stream) : base(stream) { }
        public StreamWriterEx(Stream stream, Encoding encoding) : base(stream, encoding) { }
        public StreamWriterEx(Stream stream, Encoding encoding, int bufferSize) : base(stream, encoding, bufferSize) { }
        public StreamWriterEx(Stream stream, Encoding encoding, int bufferSize, bool leaveOpen) : base(stream, encoding, bufferSize, leaveOpen) { }

        public override void Write(char value)
        {
            base.Write(value);
            Written?.Invoke(this, new StreamWriterExEventArgs(value.ToString()));
        }

        public override void Write(string value)
        {
            base.Write(value);
            Written?.Invoke(this, new StreamWriterExEventArgs(value));
        }

        public override void Write(char[] buffer, int index, int count)
        {
            base.Write(buffer, index, count);
            Written?.Invoke(this, new StreamWriterExEventArgs(new string(buffer, index, count)));
        }
    }
}
