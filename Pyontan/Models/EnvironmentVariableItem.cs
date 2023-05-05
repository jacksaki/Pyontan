using Livet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pyontan.Models
{
    public class EnvironmentVariableItem:NotificationObject
    {

        private string _Key;

        public string Key
        {
            get
            {
                return _Key;
            }
            set
            { 
                if (_Key == value)
                {
                    return;
                }
                _Key = value;
                RaisePropertyChanged();
            }
        }

        private string _Value;

        public string Value
        {
            get
            {
                return _Value;
            }
            set
            { 
                if (_Value == value)
                {
                    return;
                }
                _Value = value;
                RaisePropertyChanged();
            }
        }

    }
}
