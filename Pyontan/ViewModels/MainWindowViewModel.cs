using Livet;
using Livet.Commands;
using Livet.EventListeners;
using Livet.Messaging;
using Livet.Messaging.IO;
using Livet.Messaging.Windows;
using MaterialDesignThemes.Wpf;
using Pyontan.Models;
using Pyontan.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Pyontan.ViewModels
{
    public class MainWindowViewModel : ViewModel
    {
        public MainWindowViewModel() : base()
        {
            this.DialogCoordinator = MahApps.Metro.Controls.Dialogs.DialogCoordinator.Instance;
            this.Settings = Settings.Create();
            this.Project = new DbProject();
            this.ExplainBoxViewModel = new ExplainBoxViewModel(this);
            this.ExplainBoxViewModel.PropertyChanged += (sender, e) => { RaisePropertyChanged(nameof(ExplainBoxViewModel)); };
            this.SettingsBoxViewModel = new SettingsBoxViewModel(this);
            this.SettingsBoxViewModel.PropertyChanged += (sender, e) => { RaisePropertyChanged(nameof(SettingsBoxViewModel)); };
            var fv = System.Diagnostics.FileVersionInfo.GetVersionInfo(System.Reflection.Assembly.GetExecutingAssembly().Location);
            this.AppTitle = $"{fv.ProductName} Ver {fv.ProductVersion}";
        }
        public MahApps.Metro.Controls.Dialogs.IDialogCoordinator DialogCoordinator
        {
            get;
            set;
        }
        public string AppTitle
        {
            get;
        }
        public Settings Settings
        {
            get;
        }
        public void Initialize()
        {
        }
        public DbProject Project
        {
            get;
            private set;
        }
        public ExplainBoxViewModel ExplainBoxViewModel
        {
            get;
        }
        public SettingsBoxViewModel SettingsBoxViewModel
        {
            get;
        }

        private void Menu_Message(object sender, MessageEventArgs e)
        {
            DialogCoordinator.ShowMessageAsync(this, e.Title, e.Message, MahApps.Metro.Controls.Dialogs.MessageDialogStyle.Affirmative);
        }

        private void Menu_ErrorOccurred(object sender, ErrorOccurredEventArgs e)
        {
            DialogCoordinator.ShowMessageAsync(this, "エラー", e.Message, MahApps.Metro.Controls.Dialogs.MessageDialogStyle.Affirmative);
        }
    }
}