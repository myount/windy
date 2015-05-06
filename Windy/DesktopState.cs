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

using System.Collections.Generic;
using System.Windows.Forms;

using Newtonsoft.Json;

namespace Windy
{
    public class DesktopState
    {
        public List<Display> Displays { get; set; }

        public DesktopState(Screen[] screens)
        {
            Displays = new List<Display>();
            foreach (var screen in screens)
            {
                Displays.Add(new Display(screen));
            }
        }

        [JsonConstructor]
        public DesktopState(IEnumerable<Display> displays)
        {
            Displays = new List<Display>();
            Displays.AddRange(displays);
        }
    }
}
