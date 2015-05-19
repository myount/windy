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
using System.Diagnostics;
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

        public static bool RestoreWindows()
        {
            var files = Directory.EnumerateFiles(Path.GetTempPath(), "Windy_WindowState_*.json");

            if (!files.Any())
            {
                throw new InvalidOperationException("No serialized window data was found.");
            }
            
            string file = files.First();

            using (var sr = new StreamReader(File.Open(file, FileMode.Open, FileAccess.Read, FileShare.None)))
            {
                try
                {
                    // because the [JsonConstructor] constructor for Window initializes its private SystemWindow
                    // instance using the serialized HWnd value, makes sure the window handle is valid, and then
                    // sets properties if it is, the mere act of deserializing the window state restores all the
                    // positions and properties, so in fact, we don't need to assign this to anything...
                    var wins = JsonConvert.DeserializeObject<IEnumerable<Window>>(sr.ReadToEnd());

                    // ... but we should delete window state if all the windows are invalid.
                    if (wins.All(win => !win.IsValid))
                    {
                        DeleteStaleWindowState();
                        return false;
                    }
                }
                catch (Exception)
                {
                    return false;
                }
            }

            return true;
        }

        private static void DeleteStaleWindowState()
        {
            foreach (var f in Directory.EnumerateFiles(Path.GetTempPath(), "Windy_WindowState_*.json"))
            {
                File.Delete(f);
            }
        }

        public static void CleanUpStaleState()
        {
            var bootTime = DateTime.Now - TimeSpan.FromSeconds((double)Stopwatch.GetTimestamp() / Stopwatch.Frequency);
            foreach (var file in Directory.EnumerateFiles(Path.GetTempPath(), "Windy_*.json")
                                          .Select(file => new { file, fileInfo = new FileInfo(file) })
                                          .Where(f => f.fileInfo.CreationTime < bootTime)
                                          .Select(f => f.file))
            {
                File.Delete(file);
            }
        }
    }
}
