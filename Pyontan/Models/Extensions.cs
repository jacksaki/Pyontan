using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pyontan.Models
{
    public static class Extensions
    {
        private static Dictionary<string, Contrast> _contrasts = null;
        private static Dictionary<string, Contrast> Contrasts
        {
            get
            {
                if (_contrasts == null)
                {
                    InitContrasts();
                }
                return _contrasts;
            }
        }
        private static void InitContrasts()
        {
            _contrasts = new Dictionary<string, Contrast>();
            foreach(var c in Enum.GetValues<Contrast>())
            {
                _contrasts.Add(Enum.GetName(c), c);
            }
        }
        public static Contrast ToContrast(this string value)
        {
            return Contrasts.TryGetValue(value, out var contrast) ? contrast : Contrast.None;
        }
    }
}
