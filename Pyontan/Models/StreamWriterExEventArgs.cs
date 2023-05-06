using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pyontan.Models
{
    public class StreamWriterExEventArgs
    {
        public string Text { get; }
        public StreamWriterExEventArgs(string text)
        {
            Text = text;
        }
    }
}
