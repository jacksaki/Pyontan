using Livet;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Pyontan.Models
{
    public class VisualSettings: NotificationObject
    {
        public event EventHandler Loaded = delegate { };
        private bool _IsDarkMode;
        public void OnLoaded()
        {
            this.Loaded(this, EventArgs.Empty);
        }
        public VisualSettings(Settings parent)
        {
            this.Parent = parent;
        }
        public Settings Parent
        {
            get;
        }
        public bool IsDarkMode
        {
            get
            {
                return _IsDarkMode;
            }
            set
            {
                if (_IsDarkMode == value)
                {
                    return;
                }
                _IsDarkMode = value;
                RaisePropertyChanged();
            }
        }


        private Color _AccentColor;

        public Color AccentColor
        {
            get
            {
                return _AccentColor;
            }
            set
            {
                if (_AccentColor == value)
                {
                    return;
                }
                _AccentColor = value;
                RaisePropertyChanged();
            }
        }


        private Color _PrimaryColor;

        public Color PrimaryColor
        {
            get
            {
                return _PrimaryColor;
            }
            set
            {
                if (_PrimaryColor == value)
                {
                    return;
                }
                _PrimaryColor = value;
                RaisePropertyChanged();
            }
        }


        private bool _IsColorAdjusted;

        public bool IsColorAdjusted
        {
            get
            {
                return _IsColorAdjusted;
            }
            set
            {
                if (_IsColorAdjusted == value)
                {
                    return;
                }
                _IsColorAdjusted = value;
                RaisePropertyChanged();
            }
        }


        private float _DesiredContrastRatio;

        public float DesiredContrastRatio
        {
            get
            {
                return _DesiredContrastRatio;
            }
            set
            {
                if (_DesiredContrastRatio == value)
                {
                    return;
                }
                _DesiredContrastRatio = value;
                RaisePropertyChanged();
            }
        }


        private Contrast _ContrastValue;

        public Contrast ContrastValue
        {
            get
            {
                return _ContrastValue;
            }
            set
            {
                if (_ContrastValue == value)
                {
                    return;
                }
                _ContrastValue = value;
                RaisePropertyChanged();
            }
        }


        private ColorSelection _ColorSelectionValue;

        public ColorSelection ColorSelectionValue
        {
            get
            {
                return _ColorSelectionValue;
            }
            set
            {
                if (_ColorSelectionValue == value)
                {
                    return;
                }
                _ColorSelectionValue = value;
                RaisePropertyChanged();
            }
        }
    }
}
