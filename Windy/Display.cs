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

using System.Drawing;
using System.Windows.Forms;

using Newtonsoft.Json;

namespace Windy
{
    /// <summary>
    /// A class to encapsulate the properties of <see cref="System.Windows.Forms.Screen"/> that we actually care about,
    /// for the purposes of serialization.
    /// </summary>
    /// <remarks>
    /// <para>
    ///     JSON.NET can get real cranky when you ask it to serialize arbitrary classes.
    /// </para>
    /// </remarks>
    public class Display
    {
        public string DeviceName { get; set; }
        public Rectangle Bounds { get; set; }

        public Display(Screen screen)
        {
            DeviceName = screen.DeviceName;
            Bounds = screen.Bounds;
        }

        [JsonConstructor]
        public Display(string DeviceName, Rectangle Bounds)
        {
            this.DeviceName = DeviceName;
            this.Bounds = Bounds;
        }
    }
}
