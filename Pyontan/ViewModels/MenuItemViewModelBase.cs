using Livet;
using Livet.Commands;
using Livet.EventListeners;
using Livet.Messaging;
using Livet.Messaging.IO;
using Livet.Messaging.Windows;
using Pyontan.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Pyontan.ViewModels
{
    public class MenuItemViewModelBase : ViewModel
    {
        public delegate void ErrorOccurredEventHandler(object sender, ErrorOccurredEventArgs e);
        public event ErrorOccurredEventHandler ErrorOccurred = delegate { };
        public delegate void MessageEventHandler(object sender, MessageEventArgs e);
        public event MessageEventHandler Message = delegate { };
        protected void OnErrorOccurred(ErrorOccurredEventArgs e)
        {
            ErrorOccurred(this, e);
        }
        protected void OnMessage(MessageEventArgs e)
        {
            Message(this, e);
        }
        public MenuItemViewModelBase(MainWindowViewModel parent) : base()
        {
            this.Parent = parent;
        }
        public MainWindowViewModel Parent
        {
            get;
        }
    }
}
