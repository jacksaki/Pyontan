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
using ICSharpCode.AvalonEdit.Document;
using Pyontan.Database;

namespace Pyontan.ViewModels
{
    public class DbSetBoxViewModel : MenuItemViewModelBase
    {
        public DbSetBoxViewModel(MainWindowViewModel parent) : base(parent)
        {
            this.PropertyDocument.TextChanged += (sender, e) => { RaisePropertyChanged(nameof(PropertyDocument)); };
            this.SourceDocument.TextChanged += (sender, e) => { RaisePropertyChanged(nameof(SourceDocument)); };
            this.OnModelCreatingDocument.TextChanged += (sender, e) => { RaisePropertyChanged(nameof(OnModelCreatingDocument)); };
            try
            {
                PgQuery.SetConnectionString(this.Parent.Settings.AppSettings.ConnectionString);
                InitializeSchema();
            }
            catch (Exception ex)
            {
                OnErrorOccurred(new ErrorOccurredEventArgs("接続文字列を設定してください", ex));
            }
        }

        private ViewModelCommand _RefreshCommand;

        public ViewModelCommand RefreshCommand
        {
            get
            {
                if (_RefreshCommand == null)
                {
                    _RefreshCommand = new ViewModelCommand(Refresh);
                }
                return _RefreshCommand;
            }
        }

        public void Refresh()
        {
            PgQuery.SetConnectionString(this.Parent.Settings.AppSettings.ConnectionString);
            InitializeSchema();
        }

        private void InitializeSchema()
        {
            foreach (var schema in PgSchema.GetAll().OrderBy(x => x.Name))
            {
                this.AllSchema.Add(schema);
            }
        }
        private void InitTables(PgSchema schema)
        {
            this.Tables.Clear();
            foreach(var table in schema.GetAllTables().OrderBy(x => x.Name))
            {
                this.Tables.Add(table);
            }
        }
        public DispatcherCollection<PgSchema> AllSchema
        {
            get;
        } = new DispatcherCollection<PgSchema>(DispatcherHelper.UIDispatcher);

        private PgSchema _SelectedSchema;

        public PgSchema SelectedSchema
        {
            get
            {
                return _SelectedSchema;
            }
            set
            { 
                if (_SelectedSchema == value)
                {
                    return;
                }
                _SelectedSchema = value;
                InitTables(value);
                RaisePropertyChanged();
            }
        }
        public TextDocument PropertyDocument
        {
            get;
        } = new TextDocument();
        public TextDocument OnModelCreatingDocument
        {
            get;
        } = new TextDocument();
        public TextDocument SourceDocument
        {
            get;
        } = new TextDocument();
        public DispatcherCollection<PgTable> Tables
        {
            get;
        } = new DispatcherCollection<PgTable>(DispatcherHelper.UIDispatcher);


        private PgTable _SelectedTable;

        public PgTable SelectedTable
        {
            get
            {
                return _SelectedTable;
            }
            set
            { 
                if (_SelectedTable == value)
                {
                    return;
                }
                _SelectedTable = value;
                ShowSource(value);
                RaisePropertyChanged();
            }
        }
        private void ShowSource(PgTable table)
        {
            try
            {
                PgQuery.SetConnectionString(this.Parent.Settings.AppSettings.ConnectionString);
                this.PropertyDocument.Text = table.GetPropertySource();
                this.OnModelCreatingDocument.Text = table.GetOnModelCreatingSource();
                this.SourceDocument.Text = table.GetDbSetSource();
            }
            catch (Exception ex)
            {
                OnErrorOccurred(new ErrorOccurredEventArgs(ex.Message, ex));
            }
        }
        public AppSettings AppSettings
        {
            get
            {
                return this.Parent.Settings.AppSettings;
            }
        }
    }
}
