using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pyontan.Models
{
    public enum LogType
    {
        [EnumText("info")]
        Info,
        [EnumText("error")]
        Error,
    }
    public class LogItem
    {
        public LogItem(string text, LogType t)
        {
            this.Text = text;
            this.Type = t;
        }
        public string Text
        {
            get;
        }
        public LogType Type
        {
            get;
        }
    }
}
