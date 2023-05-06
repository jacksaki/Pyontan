using Livet;
using Livet.Commands;
using Livet.EventListeners;
using Livet.Messaging;
using Livet.Messaging.IO;
using Livet.Messaging.Windows;
using Pyontan.Database;
using Pyontan.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Pyontan.ViewModels
{
    public class EtcSettingsBoxViewModel : MenuItemViewModelBase
    {
        public EtcSettingsBoxViewModel(MainWindowViewModel parent) : base(parent)
        {
            this.Settings.PropertyChanged += (sender, e) =>
            {
                TestCommand.RaiseCanExecuteChanged();
            };
        }
        public AppSettings Settings
        {
            get
            {
                return this.Parent.Settings.AppSettings;
            }
        }

        private ViewModelCommand _TestCommand;

        public ViewModelCommand TestCommand
        {
            get
            {
                if (_TestCommand == null)
                {
                    _TestCommand = new ViewModelCommand(Test, CanTest);
                }
                return _TestCommand;
            }
        }

        public bool CanTest()
        {
            return !string.IsNullOrWhiteSpace(this.Settings.ConnectionString);
        }

        public void Test()
        {
            try
            {
                using(var q = new PgQuery(this.Settings.ConnectionString))
                {
                    foreach(var row in q.GetSqlResult("SELECT * FROM products", null).Rows)
                    {
                        Console.WriteLine($"{row}");
                    }
                }
                OnMessage(new MessageEventArgs("成功", "接続成功しました"));
            }
            catch (Exception ex)
            {
                OnErrorOccurred(new ErrorOccurredEventArgs(ex.Message, ex));
            }
        }

    }
}
