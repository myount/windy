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

using Windy.Properties;

namespace Windy
{
    public partial class AboutForm : Form
    {
        public AboutForm()
        {
            InitializeComponent();
            pictureBox1.Image = new Icon(Resources.windy, 256, 256).ToBitmap();
            labelVersion.Text = string.Format("v{0}", System.Reflection.Assembly.GetExecutingAssembly().GetName().Version);
            labelBlurb.Text = Resources.AboutBox_Blurb;
        }

        private void AboutForm_Load(object sender, EventArgs e)
        {
            this.Font = SystemFonts.MessageBoxFont;
            labelName.Font = new Font(SystemFonts.MessageBoxFont.FontFamily,
                                      48,
                                      (SystemFonts.MessageBoxFont.FontFamily.Name != "Tahoma" ? FontStyle.Italic : FontStyle.Bold));
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://mwinapi.sf.net/");
        }

        private void labelName_Click(object sender, EventArgs e)
        {

        }

        private void labelVersion_Click(object sender, EventArgs e)
        {

        }

        private void labelBlurb_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
