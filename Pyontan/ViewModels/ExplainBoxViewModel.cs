using Livet;
using Livet.Commands;
using Livet.EventListeners;
using Livet.Messaging;
using Livet.Messaging.IO;
using Livet.Messaging.Windows;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using ICSharpCode.AvalonEdit.Document;
using Pyontan.Models;
using System.Windows.Markup;
using System.Windows.Forms.VisualStyles;

namespace Pyontan.ViewModels
{
    public class ExplainBoxViewModel : MenuItemViewModelBase
    {
        public ExplainBoxViewModel(MainWindowViewModel parent) : base(parent)
        {
            this.SourceDocument.TextChanged += (sender, e) => {
                this.Project.Source = this.SourceDocument.Text;
                RaisePropertyChanged(nameof(SourceDocument)); 
            };
            this.Project.PropertyChanged += (sender, e) =>
            {
                ExecuteCommand.RaiseCanExecuteChanged();
            };
            
            this.ProjectSettings.PropertyChanged += (sender, e) =>
            {
                ExecuteCommand.RaiseCanExecuteChanged();
            };
            this.Logs.CollectionChanged += (sender, e) =>
            {
                RaisePropertyChanged(nameof(Logs));
            };
            this.SourceDocument.Text = this.Project.Source ?? "";
            this.Project.Loaded += (sender, e) =>
            {
                this.SourceDocument.Text = this.Project.Source ?? "";
            };
        }
        public DbProject Project
        {
            get
            {
                return this.Parent.Project;
            }
        }
        public ProjectSettings ProjectSettings
        {
            get
            {
                return this.Parent.Settings.ProjectSettings;
            }
        }
        public TextDocument SourceDocument
        {
            get;
        } = new TextDocument();


        private ViewModelCommand _ExecuteCommand;

        public ViewModelCommand ExecuteCommand
        {
            get
            {
                if (_ExecuteCommand == null)
                {
                    _ExecuteCommand = new ViewModelCommand(Execute, CanExecute);
                }
                return _ExecuteCommand;
            }
        }

        public bool CanExecute()
        {
            try
            {
                this.Project.Validate();
                this.ProjectSettings.Validate();
                return true;
            }
            catch 
            {
                return false;
            }
        }

        public DispatcherCollection<LogItem> Logs
        {
            get;
        } = new DispatcherCollection<LogItem>(DispatcherHelper.UIDispatcher);

        public async void Execute()
        {
            try
            {
                this.Logs.Clear();
                var cp = new CSharpExecutor();
                await cp.ExecuteAsync(this.Project, this.ProjectSettings);
                foreach(var logs in cp.Logs)
                {
                    this.Logs.Add(logs);
                }
            }
            catch (Exception ex)
            {
                OnErrorOccurred(new ErrorOccurredEventArgs(ex.Message, ex));
           }
        }

    }
}
