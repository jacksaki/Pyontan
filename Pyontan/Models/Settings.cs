using Livet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.VisualStyles;

namespace Pyontan.Models
{
    public class Settings:NotificationObject
    {
        public event EventHandler Loaded = delegate { };
        public Settings()
        {
            this.AppSettings = new AppSettings(this);
            this.VisualSettings = new VisualSettings(this);
            this.ProjectSettings = new ProjectSettings(this);
        }
        public void OnLoaded()
        {
            this.Loaded(this, EventArgs.Empty);
            this.VisualSettings.OnLoaded();
            this.ProjectSettings.OnLoaded();
            this.AppSettings.OnLoaded();
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
        } 
        public ProjectSettings ProjectSettings
        {
            get;
        } 
        public VisualSettings VisualSettings
        {
            get;
        } 
    }
}
