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
using Microsoft.WindowsAPICodePack.Dialogs;

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


        private AdditionalAssembly _SelectedAssembly;

        public AdditionalAssembly SelectedAssembly
        {
            get
            {
                return _SelectedAssembly;
            }
            set
            { 
                if (_SelectedAssembly == value)
                {
                    return;
                }
                _SelectedAssembly = value;
                RemoveSelectedAssemblyCommand.RaiseCanExecuteChanged();
                RaisePropertyChanged();
            }
        }

        private ViewModelCommand _RemoveSelectedAssemblyCommand;

        public ViewModelCommand RemoveSelectedAssemblyCommand
        {
            get
            {
                if (_RemoveSelectedAssemblyCommand == null)
                {
                    _RemoveSelectedAssemblyCommand = new ViewModelCommand(RemoveSelectedAssembly, CanRemoveSelectedAssembly);
                }
                return _RemoveSelectedAssemblyCommand;
            }
        }

        public bool CanRemoveSelectedAssembly()
        {
            return this.SelectedAssembly != null;
        }

        public void RemoveSelectedAssembly()
        {
            this.ProjectSettings.AdditionalAssemblies.Remove(this.SelectedAssembly);
        }

        private ViewModelCommand _AddAssemblyCommand;

        public ViewModelCommand AddAssemblyCommand
        {
            get
            {
                if (_AddAssemblyCommand == null)
                {
                    _AddAssemblyCommand = new ViewModelCommand(AddAssembly);
                }
                return _AddAssemblyCommand;
            }
        }

        public void AddAssembly()
        {
            using(var dlg = new CommonOpenFileDialog())
            {
                dlg.Filters.Add(new CommonFileDialogFilter("dllファイル (*.dll)", "*.dll"));
                dlg.Multiselect = true;
                if(dlg.ShowDialog() == CommonFileDialogResult.Ok)
                {
                    this.ProjectSettings.AdditionalAssemblies.Add(new AdditionalAssembly(dlg.FileName));
                }
            }
        }

    }
}
