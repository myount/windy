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
using System.Drawing;
using System.Windows.Forms;

using ManagedWinapi.Windows;
using Newtonsoft.Json;

namespace Windy
{
    /// <summary>
    /// A class to encapsulate only the properties of <see cref="SystemWindow"/> that we actually care about.
    /// </summary>
    /// <remarks>
    /// <para>
    ///     This exists entirely for serialization's sake, as JSON.NET trying to serialize things like the AllChildWindows
    ///     property causes problems.
    /// </para>
    /// <para>
    ///     Also, we don't actually care about some of these properties right now, but we might someday.
    /// </para>
    /// </remarks>
    public class Window
    {
        private readonly SystemWindow _sysWin;

        public IntPtr HWnd { get { return _sysWin.HWnd; } }
        [JsonIgnore] public string Title { get { return _sysWin.Title; } }
        public Point Location { get; private set; }
        public Size Size { get; private set; }
        public WindowStyleFlags Style { get; private set; }
        public WindowExStyleFlags ExStyle { get; private set; }
        public FormWindowState State { get; private set; }
        public string ClassName { get; private set; }
        [JsonIgnore] public bool IsValid { get; private set; }

        public Window(SystemWindow sysWin)
        {
            if (sysWin == null)
            {
                throw new ArgumentException("Constructor requires a non-null SystemWindow instance.", "sysWin");
            }

            _sysWin = sysWin;
            this.Location = _sysWin.Location;
            this.Size = _sysWin.Size;
            this.Style = _sysWin.Style;
            this.ExStyle = _sysWin.ExtendedStyle;
            this.State = _sysWin.WindowState;
            this.ClassName = _sysWin.ClassName;
        }

        private Window() { }

        [JsonConstructor]
        public Window(IntPtr HWnd,
                      Point Location,
                      Size Size,
                      WindowStyleFlags Style,
                      WindowExStyleFlags ExStyle,
                      FormWindowState State,
                      string ClassName)
        {
            _sysWin = new SystemWindow(HWnd);
            IsValid = false; // until proven otherwise

            // SystemWindow.IsValid only checks that the HWnd != IntPtr.Zero, so we perform our own checks
            try
            {
                // if the ClassNames don't match, it's probably not the same window
                IsValid = (ClassName == _sysWin.ClassName);
                if (!IsValid) return;
            }
            catch (System.ComponentModel.Win32Exception ex)
            {
                // this might not really be an error...
                if (ex.HResult == 0)
                {
                    IsValid = true;
                }
                // ... but usually it is.  attempting to access an invalid window throws a Win32Exception whose
                // NativeErrorCode is 0x0 ("operation completed successfully"), but whose HResult is not S_OK =
                // 0x0 (it's usually E_FAIL, "Unspecified failure", 0x80004005).
            }

            this.Location = Location;
            this.Size = Size;
            this.Style = Style;
            this.ExStyle = ExStyle;
            this.State = State;
        }

        public void Restore()
        {
            // minimized and maximized windows won't be moved and resized, so normal-ify them first
            _sysWin.WindowState = FormWindowState.Normal;

            _sysWin.Location = Location;
            _sysWin.Size = Size;
            _sysWin.WindowState = State;
            _sysWin.Style = Style;
            _sysWin.ExtendedStyle = ExStyle;
        }
    }
}
