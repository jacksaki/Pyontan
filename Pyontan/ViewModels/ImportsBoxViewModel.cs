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
    public class ImportsBoxViewModel : MenuItemViewModelBase
    {
        public ImportsBoxViewModel(MainWindowViewModel parent) : base(parent)
        {
            this.ImportsDocument = new ICSharpCode.AvalonEdit.Document.TextDocument();
            this.ImportsDocument.TextChanged += (sender, e) =>
            {
                this.ProjectSettings.Imports = ImportsDocument.Text;
                RaisePropertyChanged(nameof(ImportsDocument));
            };
            this.ImportsDocument.Text = this.ProjectSettings.Imports ?? "";
            this.ProjectSettings.Loaded += (sender, e) =>
            {
                this.ImportsDocument.Text = this.ProjectSettings.Imports;
            };
        }
        public ProjectSettings ProjectSettings
        {
            get
            {
                return this.Parent.Settings.ProjectSettings;
            }
        }
        public ICSharpCode.AvalonEdit.Document.TextDocument ImportsDocument
        {
            get;
        }
    }
}
