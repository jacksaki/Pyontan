using Livet;
using Livet.Commands;
using Livet.EventListeners;
using Livet.Messaging;
using Livet.Messaging.IO;
using Livet.Messaging.Windows;
using MaterialDesignColors;
using MaterialDesignThemes.Wpf;
using Pyontan.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Input;
using System.Windows.Media;

namespace Pyontan.ViewModels
{
    public class PaletteSelectorViewModel : MenuItemViewModelBase
    {
        public PaletteSelectorViewModel(MainWindowViewModel parent):base(parent)
        {
            Swatches = new SwatchesProvider().Swatches;
            this.ThemeSettingsViewModel = new ThemeSettingsViewModel(parent);

            ApplyPrimaryColor(this.VisualSettings.PrimaryColor);
            ApplyAccentColor(this.VisualSettings.AccentColor);

            this.VisualSettings.PropertyChanged += (sender, e) =>
            {
                if (e.PropertyName.Equals(nameof(VisualSettings.PrimaryColor)))
                {
                    ApplyPrimaryColor(this.VisualSettings.PrimaryColor);
                }
                if (e.PropertyName.Equals(nameof(VisualSettings.AccentColor)))
                {
                    ApplyAccentColor(this.VisualSettings.AccentColor);
                }
            };
        }
        public ThemeSettingsViewModel ThemeSettingsViewModel
        {
            get;
        }

        
        public IEnumerable<Swatch> Swatches { get; }


        private ListenerCommand<Swatch> _ApplyPrimaryCommand;

        public ListenerCommand<Swatch> ApplyPrimaryCommand
        {
            get
            {
                if (_ApplyPrimaryCommand == null)
                {
                    _ApplyPrimaryCommand = new ListenerCommand<Swatch>(ApplyPrimary, CanApplyPrimary);
                }
                return _ApplyPrimaryCommand;
            }
        }

        public bool CanApplyPrimary()
        {
            return true;
        }

        public void ApplyPrimary(Swatch swatch)
        {
            ApplyPrimaryColor(swatch.ExemplarHue.Color);
            this.VisualSettings.PrimaryColor = swatch.ExemplarHue.Color;
        }
        private void ApplyPrimaryColor(Color color)
        {
            ModifyTheme(theme => theme.SetPrimaryColor(color));
        }
        public VisualSettings VisualSettings
        {
            get
            {
                return this.Parent.Settings.VisualSettings;
            }
        }

        private ListenerCommand<Swatch> _ApplyAccentCommand;

        public ListenerCommand<Swatch> ApplyAccentCommand
        {
            get
            {
                if (_ApplyAccentCommand == null)
                {
                    _ApplyAccentCommand = new ListenerCommand<Swatch>(ApplyAccent);
                }
                return _ApplyAccentCommand;
            }
        }

        private void ApplyAccent(Swatch swatch)
        {
            if (swatch is { AccentExemplarHue: not null })
            {
                ApplyAccentColor(swatch.AccentExemplarHue.Color);
                this.VisualSettings.AccentColor = swatch.AccentExemplarHue.Color;
            }
        }
        private void ApplyAccentColor(Color color)
        {
            try
            {
                ModifyTheme(theme => theme.SetSecondaryColor(color));
            }
            catch 
            {
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