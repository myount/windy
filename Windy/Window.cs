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
        public string Title { get { return _sysWin.Title; } }
        public Point Location { get { return _sysWin.Location; } set { _sysWin.Location = value; } }
        public Size Size { get { return _sysWin.Size; } set { _sysWin.Size = value; } }
        public WindowStyleFlags Style { get { return _sysWin.Style; } set { _sysWin.Style = value; } }
        public WindowExStyleFlags ExStyle { get { return _sysWin.ExtendedStyle; } set { _sysWin.ExtendedStyle = value; } }
        public FormWindowState State { get { return _sysWin.WindowState; } set { _sysWin.WindowState = value; } }
        public string ClassName { get { return _sysWin.ClassName; } }
        public int DialogId { get { return _sysWin.DialogID; }}
        public bool Resizable { get { return _sysWin.Resizable; } }
        public bool Visible { get { return _sysWin.Visible; } }
        public bool IsValid { get { return _sysWin != null && _sysWin.IsValid(); } }

        public Window(SystemWindow sysWin)
        {
            _sysWin = sysWin;
        }

        private Window() { }

        [JsonConstructor]
        public Window(IntPtr HWnd,
                      string Title,
                      Point Location,
                      Size Size,
                      WindowStyleFlags Style,
                      WindowExStyleFlags ExStyle,
                      FormWindowState State,
                      string ClassName,
                      int DialogId,
                      bool Resizable,
                      bool Visible)
        {
            _sysWin = new SystemWindow(HWnd);

            // don't mess with an invalid window
            if (!_sysWin.IsValid())
            {
                return;
            }

            // if it's valid but the ClassNames don't match, it's probably not the same window
            if (ClassName != _sysWin.ClassName)
            {
                return;
            }

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
