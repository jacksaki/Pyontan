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
    public class DbContextBoxViewModel : MenuItemViewModelBase
    {
        public DbContextBoxViewModel(MainWindowViewModel parent) : base(parent)
        {
            this.DbContextSourceDocument = new ICSharpCode.AvalonEdit.Document.TextDocument();
            this.DbContextSourceDocument.TextChanged += (sender, e) =>
            {
                this.ProjectSettings.DbContextSource = this.DbContextSourceDocument.Text;
                RaisePropertyChanged(nameof(DbContextSourceDocument));
            };
            this.DbContextSourceDocument.Text = this.ProjectSettings.DbContextSource ?? "";
        }
        public ProjectSettings ProjectSettings
        {
            get
            {
                return this.Parent.Settings.ProjectSettings;
            }
        }
        public ICSharpCode.AvalonEdit.Document.TextDocument DbContextSourceDocument
        {
            get;
        }
    }
}
