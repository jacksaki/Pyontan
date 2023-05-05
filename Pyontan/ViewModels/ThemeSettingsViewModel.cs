using Livet;
using Livet.Commands;
using Livet.EventListeners;
using Livet.Messaging;
using Livet.Messaging.IO;
using Livet.Messaging.Windows;
using MaterialDesignThemes.Wpf;
using Pyontan.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Pyontan.ViewModels
{
    public class ThemeSettingsViewModel : MenuItemViewModelBase
    {
        public ThemeSettingsViewModel(MainWindowViewModel parent):base(parent)
        {
            var paletteHelper = new PaletteHelper();
            var theme = paletteHelper.GetTheme();
            ApplyIsDarkTheme(this.VisualSettings.IsDarkMode);

            this.VisualSettings.PropertyChanged += (sender, e) =>
            {
                if (e.PropertyName.Equals(nameof(this.VisualSettings.IsColorAdjusted)))
                {
                    ApplyIsColorAdjusted(this.VisualSettings.IsColorAdjusted);
                }
                if (e.PropertyName.Equals(nameof(this.VisualSettings.IsDarkMode)))
                {
                    ApplyIsDarkTheme(this.VisualSettings.IsDarkMode);
                }
                if (e.PropertyName.Equals(nameof(this.VisualSettings.ColorSelectionValue)))
                {
                    ApplyColorSelectionValue(this.VisualSettings.ColorSelectionValue);
                }
                if (e.PropertyName.Equals(nameof(this.VisualSettings.ContrastValue)))
                {
                    ApplyContrastValue(this.VisualSettings.ContrastValue);
                }
                if (e.PropertyName.Equals(nameof(this.VisualSettings.DesiredContrastRatio)))
                {
                    ApplyDesiredContrastRatio(this.VisualSettings.DesiredContrastRatio);
                }
            };

            this.IsColorAdjusted = this.VisualSettings.IsColorAdjusted;

            this.DesiredContrastRatio = this.VisualSettings.DesiredContrastRatio;
            this.ContrastValue = this.VisualSettings.ContrastValue;
            this.ColorSelectionValue = this.VisualSettings.ColorSelectionValue;

            if (paletteHelper.GetThemeManager() is { } themeManager)
            {
                themeManager.ThemeChanged += (_, e) =>
                {
                    IsDarkTheme = e.NewTheme?.GetBaseTheme() == BaseTheme.Dark;
                };
            }
        }

        public VisualSettings VisualSettings
        {
            get
            {
                return this.Parent.Settings.VisualSettings;
            }
        }

        private void ApplyIsDarkTheme(bool value)
        {
            ModifyTheme(theme => theme.SetBaseTheme(value ? Theme.Dark : Theme.Light));
        }
        public bool IsDarkTheme
        {
            get
            {
                return this.VisualSettings.IsDarkMode;
            }
            set
            { 
                if (this.VisualSettings.IsDarkMode == value)
                {
                    return;
                }
                this.VisualSettings.IsDarkMode = value;
                ApplyIsDarkTheme(value);
                RaisePropertyChanged();
            }
        }

        private void ApplyIsColorAdjusted(bool value)
        {
            ModifyTheme(theme =>
            {
                if (theme is Theme internalTheme)
                {
                    internalTheme.ColorAdjustment = value
                        ? new ColorAdjustment
                        {
                            DesiredContrastRatio = DesiredContrastRatio,
                            Contrast = ContrastValue,
                            Colors = ColorSelectionValue
                        }
                        : null;
                }
            });
        }
        public bool IsColorAdjusted
        {
            get
            {
                return this.VisualSettings.IsColorAdjusted;
            }
            set
            { 
                if (this.VisualSettings.IsColorAdjusted == value)
                {
                    return;
                }
                this.VisualSettings.IsColorAdjusted = value;
                ApplyIsColorAdjusted(value);
                RaisePropertyChanged();
            }
        }

        private void ApplyDesiredContrastRatio(float value)
        {
            ModifyTheme(theme =>
            {
                if (theme is Theme internalTheme && internalTheme.ColorAdjustment != null)
                    internalTheme.ColorAdjustment.DesiredContrastRatio = value;
            });
        }

        public float DesiredContrastRatio
        {
            get
            {
                return this.VisualSettings.DesiredContrastRatio;
            }
            set
            { 
                if (this.VisualSettings.DesiredContrastRatio == value)
                {
                    return;
                }
                this.VisualSettings.DesiredContrastRatio = value;
                ApplyDesiredContrastRatio(value);
                RaisePropertyChanged();
            }
        }

        public IEnumerable<Contrast> ContrastValues => Enum.GetValues(typeof(Contrast)).Cast<Contrast>();

        private void ApplyContrastValue(Contrast value)
        {
            ModifyTheme(theme =>
            {
                if (theme is Theme internalTheme && internalTheme.ColorAdjustment != null)
                    internalTheme.ColorAdjustment.Contrast = value;
            });
        }
        public Contrast ContrastValue
        {
            get
            {
                return this.VisualSettings.ContrastValue;
            }
            set
            { 
                if (this.VisualSettings.ContrastValue == value)
                {
                    return;
                }
                this.VisualSettings.ContrastValue = value;
                ApplyContrastValue(value);
                RaisePropertyChanged();
            }
        }

        public IEnumerable<ColorSelection> ColorSelectionValues => Enum.GetValues<ColorSelection>().Cast<ColorSelection>();

        private void ApplyColorSelectionValue(ColorSelection value)
        {
            ModifyTheme(theme =>
            {
                if (theme is Theme internalTheme && internalTheme.ColorAdjustment != null)
                    internalTheme.ColorAdjustment.Colors = value;
            });
        }
        public ColorSelection ColorSelectionValue
        {
            get
            {
                return this.VisualSettings.ColorSelectionValue;
            }
            set
            { 
                if (this.VisualSettings.ColorSelectionValue == value)
                {
                    return;
                }
                this.VisualSettings.ColorSelectionValue = value;
                ApplyColorSelectionValue(value);
                RaisePropertyChanged();
            }
        }

        private static void ModifyTheme(Action<ITheme> modificationAction)
        {
            var paletteHelper = new PaletteHelper();
            ITheme theme = paletteHelper.GetTheme();

            modificationAction?.Invoke(theme);

            paletteHelper.SetTheme(theme);
        }
    }
}
