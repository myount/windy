/*
    Windy, a lightweight window manager to make life easier for users of dockable mobile PCs.
    Copyright 2015 Michael Yount.
    
    This file is part of Windy.

    Windy is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    Windy is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with Enthalpy.  If not, see <http://www.gnu.org/licenses/>.
*/

using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;

using ManagedWinapi.Windows;
using Newtonsoft.Json;

namespace Windy
{
    public static class WindySerializationHelpers
    {
        private static IEnumerable<Window> GetAllWindows()
        {
            return
                SystemWindow.AllToplevelWindows
                            .Where(win => win.Visible && win.Size != Size.Empty && win.Style.HasFlag(WindowStyleFlags.BORDER))
                            .Select(win => new Window(win));
        }

        private static DesktopState GetDesktopState()
        {
            return new DesktopState(System.Windows.Forms.Screen.AllScreens);
        }

        public static void SaveDesktopState()
        {
            DeleteStaleDesktopState();
            Utilities.WriteFile(Utilities.GenerateTempFileName("DesktopState"), JsonConvert.SerializeObject(GetDesktopState()));
        }

        public static DesktopState LoadDesktopState()
        {
            var files = Directory.EnumerateFiles(Path.GetTempPath(), "Windy_DesktopState_*.json");

            if (!files.Any())
            {
                throw new InvalidOperationException("No serialized desktop state data was found.");
            }

            string file = files.First();

            using (var sr = new StreamReader(File.Open(file, FileMode.Open, FileAccess.Read, FileShare.None)))
            {
                return JsonConvert.DeserializeObject<DesktopState>(sr.ReadToEnd());
            }
        }

        private static void DeleteStaleDesktopState()
        {
            foreach (var f in Directory.EnumerateFiles(Path.GetTempPath(), "Windy_DesktopState_*.json"))
            {
                File.Delete(f);
            }
        }

        public static void SaveWindows()
        {
            DeleteStaleWindowState();
            Utilities.WriteFile(Utilities.GenerateTempFileName("WindowState"), JsonConvert.SerializeObject(GetAllWindows()));
        }

        public static void RestoreWindows()
        {
            var files = Directory.EnumerateFiles(Path.GetTempPath(), "Windy_WindowState_*.json");

            if (!files.Any())
            {
                throw new InvalidOperationException("No serialized window data was found.");
            }
            
            string file = files.First();

            using (var sr = new StreamReader(File.Open(file, FileMode.Open, FileAccess.Read, FileShare.None)))
            {
                // because Window just wraps SystemWindow (because certain properties of SystemWindow cause
                // infinite loops when JSON.NET tries to serialize them (and I didn't want to make a custom
                // build of ManagedWinapi solely to add [JsonIgnore] attributes, or try to inject them with
                // reflection (is that even possible?))), and because the [JsonConstructor] constructor for
                // Window initializes its private SystemWindow instance using the serialized HWnd value and
                // then sets properties on it, simply deserializing the list of Window objects will restore
                // the window layout, so we don't actually need to assign the result of this.
                JsonConvert.DeserializeObject<IEnumerable<Window>>(sr.ReadToEnd());
            }
        }

        private static void DeleteStaleWindowState()
        {
            foreach (var f in Directory.EnumerateFiles(Path.GetTempPath(), "Windy_WindowState_*.json"))
            {
                File.Delete(f);
            }
        }
    }
}
