using Livet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pyontan.Models
{
    public class Settings:NotificationObject
    {
        public Settings()
        {

        }
        public static string SavedPath
        {
            get
            {
                return System.IO.Path.ChangeExtension(System.Reflection.Assembly.GetExecutingAssembly().Location, ".conf");
            }
        }
        public static Settings Create()
        {
            var settings = new Settings();
            if (System.IO.File.Exists(SavedPath))
            {
                SettingsConverter.Load(settings, SavedPath);
            }
            return settings;
        }

        public AppSettings AppSettings
        {
            get;
        } = new AppSettings();
        public ProjectSettings ProjectSettings
        {
            get;
        } = new ProjectSettings();
        public VisualSettings VisualSettings
        {
            get;
        } = new VisualSettings();
    }
}
