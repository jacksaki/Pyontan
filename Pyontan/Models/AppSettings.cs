using Livet;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Pyontan.Models
{
    public class AppSettings:NotificationObject
    {
        public event EventHandler Loaded = delegate { };
        public AppSettings(Settings parent)
        {
            this.Parent = parent;
        }
        public Settings Parent
        {
            get;
        }
        public void OnLoaded()
        {
            this.Loaded(this, EventArgs.Empty);
        }

        private string _ConnectionString;

        public string ConnectionString
        {
            get
            {
                return _ConnectionString;
            }
            set
            { 
                if (_ConnectionString == value)
                {
                    return;
                }
                _ConnectionString = value;
                RaisePropertyChanged();
            }
        }

    }
}
