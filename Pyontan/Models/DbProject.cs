using Livet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pyontan.Models
{
    public class DbProject:NotificationObject
    {
        public DbProject()
        {
        }

        private string _Source;

        public string Source
        {
            get
            {
                return _Source;
            }
            set
            {
                if (_Source == value)
                {
                    return;
                }
                _Source = value;
                RaisePropertyChanged();
            }
        }
    }
}
