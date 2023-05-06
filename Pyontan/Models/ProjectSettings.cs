using Livet;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pyontan.Models
{
    public class ProjectSettings : NotificationObject
    {
        public event EventHandler Loaded = delegate { };
        public Settings Parent
        {
            get;
        }
        public void OnLoaded()
        {
            this.Loaded(this, EventArgs.Empty);
        }

        public ProjectSettings(Settings parent)
        {
            this.Parent = parent;

            this.GlobalAssemblies.CollectionChanged += (sender, e) =>
            {
                if (e.OldItems != null)
                {
                    foreach (GlobalAssembly asm in e.OldItems)
                    {
                        asm.PropertyChanged -= Asm_PropertyChanged;
                    }

                }
                if (e.NewItems != null)
                {
                    foreach (GlobalAssembly asm in e.NewItems)
                    {
                        asm.PropertyChanged += Asm_PropertyChanged;
                    }
                }
            };
            this.AdditionalAssemblies.CollectionChanged += (sender, e) =>
            {
                if (e.OldItems != null)
                {
                    foreach (AdditionalAssembly asm in e.OldItems)
                    {
                        asm.PropertyChanged -= Asm_PropertyChanged1;
                    }

                }
                if (e.NewItems != null)
                {
                    foreach (AdditionalAssembly asm in e.NewItems)
                    {
                        asm.PropertyChanged += Asm_PropertyChanged1;
                    }
                }
                RaisePropertyChanged(nameof(AdditionalAssemblies));
            };
            this.EnvironmentVariables.CollectionChanged += (sender, e) =>
            {
                if (e.OldItems != null)
                {
                    foreach (EnvironmentVariableItem item in e.OldItems)
                    {
                        item.PropertyChanged -= Item_PropertyChanged;
                    }
                }
                if (e.NewItems != null)
                {
                    foreach (EnvironmentVariableItem item in e.NewItems)
                    {
                        item.PropertyChanged += Item_PropertyChanged;
                    }
                }
            };
        }


        private string _Source;

        public string Source
        {
            get
            {
                return _Source;
            }
            set
            { 
                if (_Source == value)
                {
                    return;
                }
                _Source = value;
                RaisePropertyChanged();
            }
        }

        private void Item_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            RaisePropertyChanged(nameof(EnvironmentVariables));
        }

        public DispatcherCollection<EnvironmentVariableItem> EnvironmentVariables
        {
            get;
        } = new DispatcherCollection<EnvironmentVariableItem>(DispatcherHelper.UIDispatcher);

        private string _DbContextSource;

        public string DbContextSource
        {
            get
            {
                return _DbContextSource;
            }
            set
            { 
                if (_DbContextSource == value)
                {
                    return;
                }
                _DbContextSource = value;
                RaisePropertyChanged();
            }
        }


        private string _AdditionalSource;

        public string AdditionalSource
        {
            get
            {
                return _AdditionalSource;
            }
            set
            { 
                if (_AdditionalSource == value)
                {
                    return;
                }
                _AdditionalSource = value;
                RaisePropertyChanged();
            }
        }

        private void Asm_PropertyChanged1(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            RaisePropertyChanged(nameof(AdditionalAssemblies));
        }

        private void Asm_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            RaisePropertyChanged(nameof(GlobalAssemblies));
        }
        public IEnumerable<GlobalAssembly> SelectedGlobalAssemblies
        {
            get
            {
                return this.GlobalAssemblies.Where(x => x.IsSelected);
            }
        }
        private static DispatcherCollection<GlobalAssembly> _globalAssemblies = null;
        private static void InitGlobalAssemblies()
        {
            _globalAssemblies = new DispatcherCollection<GlobalAssembly>(DispatcherHelper.UIDispatcher);
            foreach(var asm in GlobalAssembly.GetAll().OrderBy(x=>x.Name))
            {
                _globalAssemblies.Add(asm);
            }
        }

        internal void Validate()
        {

        }

        public DispatcherCollection<GlobalAssembly> GlobalAssemblies
        {
            get
            {
                if (_globalAssemblies == null)
                {
                    InitGlobalAssemblies();
                }
                return _globalAssemblies;
            }
        } 
        public DispatcherCollection<AdditionalAssembly> AdditionalAssemblies
        {
            get;
        } = new DispatcherCollection<AdditionalAssembly>(DispatcherHelper.UIDispatcher);

        private string _Imports;

        public string Imports
        {
            get
            {
                return _Imports;
            }
            set
            { 
                if (_Imports == value)
                {
                    return;
                }
                _Imports = value;
                RaisePropertyChanged();
            }
        }
        public IEnumerable<string> ImportList
        {
            get
            {
                return (this.Imports ?? "").Split(new string[] { "\r\n", "\r", "\n" }, StringSplitOptions.None).Where(x=>!string.IsNullOrEmpty(x));
            }
        }
    }
}
