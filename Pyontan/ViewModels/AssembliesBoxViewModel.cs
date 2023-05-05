using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

using Livet;
using Livet.Commands;
using Livet.Messaging;
using Livet.Messaging.IO;
using Livet.EventListeners;
using Livet.Messaging.Windows;
using Pyontan.Models;

namespace Pyontan.ViewModels
{
    public class AssembliesBoxViewModel : MenuItemViewModelBase
    {
        public AssembliesBoxViewModel(MainWindowViewModel parent) : base(parent)
        {
        }
        
        public ProjectSettings ProjectSettings
        {
            get
            {
                return this.Parent.Settings.ProjectSettings;
            }
        }
    }
}
