using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pyontan.Models
{
    public class OutputRedirector : IDisposable
    {
        private MemoryStreamEx _ms = new MemoryStreamEx();
        private MemoryStreamEx _me = new MemoryStreamEx();
        private StreamWriterEx _ws;
        private StreamWriterEx _we;
        public event EventHandler<StreamWriterExEventArgs> StandardOutputWritten = delegate { };
        public event EventHandler<StreamWriterExEventArgs> StandardErrorWritten = delegate { };
        public OutputRedirector()
        {
            _ws = new StreamWriterEx(_ms);
            _we = new StreamWriterEx(_me);
            _ws.Written += (sender, e) =>
            {
                StandardOutputWritten.Invoke(this, e);
            };
            _we.Written += (sender, e) =>
            {
                StandardErrorWritten.Invoke(this, e);
            };
            Console.SetOut(_ws);
            Console.SetError(_we);
        }

        private bool disposedValue;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: マネージド状態を破棄します (マネージド オブジェクト)
                }
                _ws.Close();
                _we.Close();
                _ms.Close();
                _me.Close();
                // TODO: アンマネージド リソース (アンマネージド オブジェクト) を解放し、ファイナライザーをオーバーライドします
                // TODO: 大きなフィールドを null に設定します
                disposedValue = true;
            }
        }

        // // TODO: 'Dispose(bool disposing)' にアンマネージド リソースを解放するコードが含まれる場合にのみ、ファイナライザーをオーバーライドします
        // ~OutputRedirector()
        // {
        //     // このコードを変更しないでください。クリーンアップ コードを 'Dispose(bool disposing)' メソッドに記述します
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            // このコードを変更しないでください。クリーンアップ コードを 'Dispose(bool disposing)' メソッドに記述します
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
