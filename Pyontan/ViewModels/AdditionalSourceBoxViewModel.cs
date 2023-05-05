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
    public class AdditionalSourceBoxViewModel : MenuItemViewModelBase
    {
        public AdditionalSourceBoxViewModel(MainWindowViewModel parent) : base(parent)
        {
            this.AdditionalSourceDocument = new ICSharpCode.AvalonEdit.Document.TextDocument();
            this.AdditionalSourceDocument.TextChanged += (sender, e) =>
            {
                this.ProjectSettings.AdditionalSource = this.AdditionalSourceDocument.Text;
                RaisePropertyChanged(nameof(AdditionalSourceDocument));
            };
            this.AdditionalSourceDocument.Text = this.ProjectSettings.AdditionalSource ?? "";
        }
        public ProjectSettings ProjectSettings
        {
            get
            {
                return this.Parent.Settings.ProjectSettings;
            }
        }
        public ICSharpCode.AvalonEdit.Document.TextDocument AdditionalSourceDocument
        {
            get;
        }
    }
}
