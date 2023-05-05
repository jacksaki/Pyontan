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
    public class VisualBoxViewModel : MenuItemViewModelBase
    {
        public VisualBoxViewModel(MainWindowViewModel parent) : base(parent)
        {
            this.PaletteSelectorViewModel = new PaletteSelectorViewModel(parent);
        }
        public AppSettings AppSettings
        {
            get
            {
                return this.Parent.Settings.AppSettings;
            }
        }
        public PaletteSelectorViewModel PaletteSelectorViewModel
        {
            get;
        }

    }
}
