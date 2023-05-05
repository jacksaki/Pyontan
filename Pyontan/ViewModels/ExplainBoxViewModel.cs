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
            this.SourceDocument.Text = this.Project.Source ?? "";
        }
        public DbProject Project
        {
            get
            {
                return this.Parent.Project;
            }
        }
        public TextDocument SourceDocument
        {
            get;
        } = new TextDocument();
    }
}
