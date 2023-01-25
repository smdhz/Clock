using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Clock
{
    internal static class SystemTheme
    {
        [DllImport("UXTheme.dll", SetLastError = true, EntryPoint = "#138")]
        private static extern bool ShouldSystemUseDarkMode();

        public static bool InDarkMode(this Application _) => ShouldSystemUseDarkMode();
    }
}
