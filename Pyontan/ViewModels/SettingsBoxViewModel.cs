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
using System.Windows.Input;

namespace Pyontan.ViewModels
{
    public class SettingsBoxViewModel : MenuItemViewModelBase
    {
        public SettingsBoxViewModel(MainWindowViewModel parent) : base(parent)
        {
            this.AssembliesBoxViewModel = new AssembliesBoxViewModel(parent);
            this.ImportsBoxViewModel = new ImportsBoxViewModel(parent);
            this.EnvironmentVariablesBoxViewModel = new EnvironmentVariablesBoxViewModel(parent);
            this.DbContextBoxViewModel = new DbContextBoxViewModel(parent);
            this.AdditionalSourceBoxViewModel = new AdditionalSourceBoxViewModel(parent);
            this.VisualBoxViewModel = new VisualBoxViewModel(parent);
        }
        public VisualBoxViewModel VisualBoxViewModel
        {
            get;
        }
        public AssembliesBoxViewModel AssembliesBoxViewModel
        {
            get;
        }
        public ImportsBoxViewModel ImportsBoxViewModel
        {
            get;
        }
        public EnvironmentVariablesBoxViewModel EnvironmentVariablesBoxViewModel
        {
            get;
        }
        public DbContextBoxViewModel DbContextBoxViewModel
        {
            get;
        }
        public AdditionalSourceBoxViewModel AdditionalSourceBoxViewModel
        {
            get;
        }


        private ViewModelCommand _LoadSettingsCommand;

        public ViewModelCommand LoadSettingsCommand
        {
            get
            {
                if (_LoadSettingsCommand == null)
                {
                    _LoadSettingsCommand = new ViewModelCommand(LoadSettings);
                }
                return _LoadSettingsCommand;
            }
        }

        public void LoadSettings()
        {
            SettingsConverter.Load(this.Parent.Settings);

        }

        private ViewModelCommand _SaveSettingsCommand;

        public ViewModelCommand SaveSettingsCommand
        {
            get
            {
                if (_SaveSettingsCommand == null)
                {
                    _SaveSettingsCommand = new ViewModelCommand(SaveSettings);
                }
                return _SaveSettingsCommand;
            }
        }

        public void SaveSettings()
        {
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                SettingsConverter.Save(this.Parent.Settings);
            }
            finally
            {
                Mouse.OverrideCursor = null;
            }
        }

    }
}
