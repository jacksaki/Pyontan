using MaterialDesignThemes.Wpf;
using MessagePack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Pyontan.Models
{
    [MessagePackObject]
    public class SettingsConverter
    {
        public static void Save(Settings settings)
        {
            Save(settings, Settings.SavedPath);
        }
        public static void Save(Settings settings, string path)
        {
            var obj = new SettingsConverter();
            obj.Source = settings.ProjectSettings.Source;
            obj.SelectedGlobalAssemblies = settings.ProjectSettings.GlobalAssemblies.Where(x => x.IsSelected).Select(x => x.Name).ToList();
            obj.AdditionalAssemblies = settings.ProjectSettings.AdditionalAssemblies.Select(x => x.Name).ToList();
            obj.EnvironmentVariables = settings.ProjectSettings.EnvironmentVariables.ToDictionary(x => x.Key, y => y.Value);
            obj.Imports = settings.ProjectSettings.ImportList.ToList();

            obj.DbContextSource = settings.ProjectSettings.DbContextSource;
            obj.AdditionalSource = settings.ProjectSettings.AdditionalSource;
            obj.ConnectionString = settings.AppSettings.ConnectionString;
            obj.AccentColor = settings.VisualSettings.AccentColor.ToString();
            obj.PrimaryColor = settings.VisualSettings.PrimaryColor.ToString();
            obj.IsColorAdjusted = settings.VisualSettings.IsColorAdjusted;
            obj.IsDarkTheme = settings.VisualSettings.IsDarkMode;
            obj.ColorSelectionValue = (byte)settings.VisualSettings.ColorSelectionValue;
            obj.ContrastValue = Enum.GetName<Contrast>(settings.VisualSettings.ContrastValue);
            obj.DesiredContrastRatio = settings.VisualSettings.DesiredContrastRatio;
            var piyo = MessagePackSerializer.Serialize<SettingsConverter>(obj);
            System.IO.File.WriteAllText(path, MessagePackSerializer.ConvertToJson(piyo));
        }
        public static void Load(Settings settings)
        {
            Load(settings, Settings.SavedPath);
        }
        public static void Load(Settings settings, string path)
        {
            var piyo = MessagePackSerializer.ConvertFromJson(System.IO.File.ReadAllText(path));
            var obj = MessagePackSerializer.Deserialize<SettingsConverter>(piyo);
            settings.ProjectSettings.Source = obj.Source;
            foreach (var asm in settings.ProjectSettings.GlobalAssemblies)
            {
                asm.IsSelected = obj.SelectedGlobalAssemblies.Where(x => x.Equals(asm.Name)).Any();
            }
            settings.ProjectSettings.AdditionalAssemblies.Clear();
            foreach (var asm in obj.AdditionalAssemblies)
            {
                settings.ProjectSettings.AdditionalAssemblies.Add(new AdditionalAssembly(asm));
            }
            settings.ProjectSettings.EnvironmentVariables.Clear();
            foreach (var env in obj.EnvironmentVariables)
            {
                settings.ProjectSettings.EnvironmentVariables.Add(new EnvironmentVariableItem() { Key = env.Key, Value = env.Value });
            }
            settings.ProjectSettings.Imports = string.Join("\r\n,", obj.Imports);
            settings.ProjectSettings.DbContextSource = obj.DbContextSource;
            settings.ProjectSettings.AdditionalSource = obj.AdditionalSource;
            settings.AppSettings.ConnectionString = obj.ConnectionString;
            settings.VisualSettings.AccentColor = (Color)System.Windows.Media.ColorConverter.ConvertFromString(obj.AccentColor);
            settings.VisualSettings.PrimaryColor = (Color)System.Windows.Media.ColorConverter.ConvertFromString(obj.PrimaryColor);
            settings.VisualSettings.IsColorAdjusted = obj.IsColorAdjusted;
            settings.VisualSettings.IsDarkMode = obj.IsDarkTheme;
            settings.VisualSettings.ColorSelectionValue = (ColorSelection)obj.ColorSelectionValue;
            settings.VisualSettings.ContrastValue = obj.ContrastValue.ToContrast();
            settings.VisualSettings.DesiredContrastRatio = obj.DesiredContrastRatio;
        }
        [Key(0)]
        public string Source
        {
            get;
            set;
        }
        [Key(1)]
        public List<string> SelectedGlobalAssemblies
        {
            get;
            set;
        }
        [Key(2)]
        public List<string> AdditionalAssemblies
        {
            get;
            set;
        }
        [Key(3)]
        public Dictionary<string, string> EnvironmentVariables
        {
            get;
            set;
        }
        [Key(4)]
        public string DbContextSource
        {
            get;
            set;
        }
        [Key(5)]
        public string AdditionalSource
        {
            get;
            set;
        }
        [Key(6)]
        public List<string> Imports
        {
            get;
            set;
        }
        [Key(7)]
        public string ConnectionString
        {
            get;
            set;
        }
        [Key(8)]
        public bool IsDarkTheme
        {
            get;
            set;
        }
        [Key(9)]
        public string PrimaryColor
        {
            get;
            set;
        }
        [Key(10)]
        public string AccentColor
        {
            get;
            set;
        }
        [Key(11)]
        public bool IsColorAdjusted
        {
            get;
            set;
        }
        [Key(12)]
        public float DesiredContrastRatio
        {
            get;
            set;
        }
        [Key(13)]
        public string ContrastValue
        {
            get;
            set;
        }
        [Key(14)]
        public byte ColorSelectionValue
        {
            get;
            set;
        }
    }
}
