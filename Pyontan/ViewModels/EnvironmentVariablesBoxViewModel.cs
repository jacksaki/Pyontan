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
    public class EnvironmentVariablesBoxViewModel : MenuItemViewModelBase
    {
        public EnvironmentVariablesBoxViewModel(MainWindowViewModel parent) : base(parent)
        {
            this.ProjectSettings.PropertyChanged += (sender, e) =>
            {
                RaisePropertyChanged();
            };
        }
        public ProjectSettings ProjectSettings
        {
            get
            {
                return this.Parent.Settings.ProjectSettings;
            }
        }

        private ViewModelCommand _AddVariableCommand;

        public ViewModelCommand AddVariableCommand
        {
            get
            {
                if (_AddVariableCommand == null)
                {
                    _AddVariableCommand = new ViewModelCommand(AddVariable);
                }
                return _AddVariableCommand;
            }
        }

        public void AddVariable()
        {
            this.ProjectSettings.EnvironmentVariables.Add(new EnvironmentVariableItem() { Key = "NewItem", Value = "" });
        }

        private EnvironmentVariableItem _SelectedEnvironmentVariable;

        public EnvironmentVariableItem SelectedEnvironmentVariable
        {
            get
            {
                return _SelectedEnvironmentVariable;
            }
            set
            { 
                if (_SelectedEnvironmentVariable == value)
                {
                    return;
                }
                _SelectedEnvironmentVariable = value;
                RemoveSelectedVariableCommand.RaiseCanExecuteChanged();
                RaisePropertyChanged();
            }
        }

        private ViewModelCommand _RemoveSelectedVariableCommand;

        public ViewModelCommand RemoveSelectedVariableCommand
        {
            get
            {
                if (_RemoveSelectedVariableCommand == null)
                {
                    _RemoveSelectedVariableCommand = new ViewModelCommand(RemoveSelectedVariable, CanRemoveSelectedVariable);
                }
                return _RemoveSelectedVariableCommand;
            }
        }

        public bool CanRemoveSelectedVariable()
        {
            return this.SelectedEnvironmentVariable != null;
        }

        public void RemoveSelectedVariable()
        {
            this.ProjectSettings.EnvironmentVariables.Remove(this.SelectedEnvironmentVariable);
        }

    }
}
