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
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

using ManagedWinapi;

using Microsoft.Win32;

using Windy.Properties;

namespace Windy
{
    public class WindyApplicationContext : ApplicationContext
    {
        private NotifyIcon _trayIcon;

        private readonly Hotkey _saveHotkey = new Hotkey();

        private readonly Hotkey _restoreHotkey = new Hotkey();

        public WindyApplicationContext()
        {
            Initialize();

            this.ThreadExit += Application_ApplicationExit;
            Application.ApplicationExit += Application_ApplicationExit;
            SystemEvents.DisplaySettingsChanging += SystemEvents_DisplaySettingsChanging;

            _trayIcon.Visible = true;
            _trayIcon.MouseClick += (sender, args) =>
                {
                    // show only on a deliberate left click and not when they mash the mouse buttons or something
                    if (args.Button == MouseButtons.Left)
                    {
                        ShowWindyIsRunningTip();
                    }
                };

            WindySerializationHelpers.SaveDesktopState();
            ShowWindyIsRunningTip();
        }

        private void ShowWindyIsRunningTip()
        {
            _trayIcon.ShowBalloonTip(10000,
                         GetString("TipTitle_WindyIsRunning"),
                         GetString("TipText_WindyInstructions"),
                         ToolTipIcon.Info);
        }

        private void SystemEvents_DisplaySettingsChanging(object sender, EventArgs e)
        {
            var ds = WindySerializationHelpers.LoadDesktopState();
            if (CurrentDesktopLayoutMatches(ds))
            {
                try
                {
                    WindySerializationHelpers.RestoreWindows();
                    _trayIcon.ShowBalloonTip(5000,
                                             GetString("TipTitle_WindowsAutomaticallyRestored"),
                                             GetString("TipText_WindowsAutomaticallyRestored"),
                                             ToolTipIcon.Info);
                }
                catch (Exception ex)
                {
                    _trayIcon.ShowBalloonTip(10000,
                                             GetString("TipTitle_CouldntRestoreWindows"),
                                             string.Format(GetString("TipText_WindowsNotAutomaticallyRestored"),
                                                 ex.Message,
                                                 ex.GetType()),
                                             ToolTipIcon.Error);
                }
            }
        }

        private void Application_ApplicationExit(object sender, EventArgs e)
        {
            SystemEvents.DisplaySettingsChanging -= SystemEvents_DisplaySettingsChanging;
            _trayIcon.Visible = false;
        }

        private void Initialize()
        {
            // set up the hotkey handlers first, so we can fail fast if they're already taken.
            try
            {
                _saveHotkey.Ctrl = true;
                _saveHotkey.WindowsKey = true;
                _saveHotkey.KeyCode = Keys.S;
                _saveHotkey.Enabled = true;
                _saveHotkey.HotkeyPressed += SaveHotkeyOnHotkeyPressed;

                _restoreHotkey.Ctrl = true;
                _restoreHotkey.WindowsKey = true;
                _restoreHotkey.KeyCode = Keys.R;
                _restoreHotkey.Enabled = true;
                _restoreHotkey.HotkeyPressed += RestoreHotkeyOnHotkeyPressed;
            }
            catch (HotkeyAlreadyInUseException)
            {
                MessageBox.Show(GetString("MessageBoxText_WindyFailedToStart_ShortcutsInUse"),
                                GetString("MessageBoxText_WindyFailedToStart"),
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                Environment.Exit(1);
            }

            _trayIcon = new NotifyIcon();
            _trayIcon.Text = "Windy";
            _trayIcon.Icon = Resources.windy;

            _trayIcon.ContextMenuStrip = new ContextMenuStrip();
            _trayIcon.ContextMenuStrip.Items.AddRange(
                new ToolStripItem[]
                {
                    new ToolStripMenuItem(GetString("MenuItem_SaveWindowLayout"), null, (sender, args) => SaveHotkeyOnHotkeyPressed(null, null))
                    { ShortcutKeyDisplayString = "Ctrl+Win+S" },
                    new ToolStripMenuItem(GetString("MenuItem_RestoreWindowLayout"), null, (sender, args) => RestoreHotkeyOnHotkeyPressed(null, null))
                    { ShortcutKeyDisplayString = "Ctrl+Win+R" },
                    new ToolStripSeparator(),
                    new ToolStripMenuItem(GetString("MenuItem_AboutWindy"), null, (sender, args) => (new AboutForm()).Show()),
                    new ToolStripMenuItem(GetString("MenuItem_Exit"), null, (sender, args) => Application.Exit()),
                });
        }

        private void SaveHotkeyOnHotkeyPressed(object sender, EventArgs eventArgs)
        {
            try
            {
                WindySerializationHelpers.SaveDesktopState();
                WindySerializationHelpers.SaveWindows();
                _trayIcon.ShowBalloonTip(5000, GetString("TipTitle_WindowLayoutSaved"), GetString("TipText_WindowLayoutSaved"), ToolTipIcon.Info);
            }
            catch (Exception ex)
            {
                _trayIcon.ShowBalloonTip(10000,
                                         GetString("TipTitle_CouldntSaveWindows"),
                                         string.Format(GetString("TipText_CouldntSaveWindows"), ex.Message, ex.GetType()),
                                         ToolTipIcon.Error);
            }
        }

        private void RestoreHotkeyOnHotkeyPressed(object sender, EventArgs eventArgs)
        {
            var loaded = WindySerializationHelpers.LoadDesktopState();
            if (!CurrentDesktopLayoutMatches(loaded))
            {
                var res =
                    MessageBox.Show(
                        GetString("MessageBoxText_RestoringToNonMatchingScreenLayout"),
                        GetString("MessageBoxTitle_RestoringToNonMatchingScreenLayout"),
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Warning);

                if (res == DialogResult.No)
                {
                    return;
                }
            }

            try
            {
                WindySerializationHelpers.RestoreWindows();
                _trayIcon.ShowBalloonTip(5000, GetString("TipTitle_WindowLayoutRestored"), GetString("TipText_WindowLayoutRestored"), ToolTipIcon.Info);
            }
            catch (Exception ex)
            {
                _trayIcon.ShowBalloonTip(10000,
                                         GetString("TipTitle_CouldntRestoreWindows"),
                                         string.Format(GetString("TipText_CouldntRestoreWindows"), ex.Message, ex.GetType()),
                                         ToolTipIcon.Error);
            }
        }

        private static bool CurrentDesktopLayoutMatches(DesktopState loaded)
        {
            // the Screen class doesn't seem to invalidate its own internal display state array correctly, so let's just
            // invalidate it using reflection whenever we need to ask about the current display configuration. yes, this
            // is terrible, but the alternative is reimplementing what the Screen class does myself, which would require
            // grinding out a bunch of P/Invoke wrappers, so I'll gladly do the terrible thing instead. I'll at least be
            // a tiny bit reasonable and hedge against the possibility of that implementation detail changing in the
            // future.
            var screenScreens = typeof(Screen).GetField("screens", BindingFlags.Static | BindingFlags.NonPublic);
            if (screenScreens != null)
            {
                screenScreens.SetValue(null, null);
            }

            return (loaded.Displays.Count == Screen.AllScreens.Length)
                   && !loaded.Displays.Where((t, i) => t.Bounds != Screen.AllScreens[i].Bounds).Any();
        }

        private static string GetString(string name)
        {
            return Resources.ResourceManager.GetString(name);
        }
    }
}
