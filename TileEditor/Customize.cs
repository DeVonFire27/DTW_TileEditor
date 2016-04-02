using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TileEditor
{
    public partial class Customize : Form
    {
        public Customize()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Size map = new Size((int)numericUpDown1.Value, (int)numericUpDown2.Value);
            Size tiles = new Size();
            Size set = new Size((int)numericUpDown3.Value, (int)numericUpDown4.Value);
            if (radioButton1.Checked)
                tiles = new Size(16, 16);
            else if (radioButton2.Checked)
                tiles = new Size(32, 32);
            else
                tiles = new Size(64, 64);

            Form1 form = (Form1)this.Owner;
            form.update_custom(map, tiles, set);
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        public void SetDefault(Size map, Size pixel, Size set)
        {
            numericUpDown1.Value = map.Width;
            numericUpDown2.Value = map.Height;

            if (pixel.Width == 16)
                radioButton1.Checked = true;
            else if (pixel.Width == 32)
                radioButton2.Checked = true;
            else
                radioButton3.Checked = true;

            numericUpDown3.Value = set.Width;
            numericUpDown4.Value = set.Height;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Size map = new Size((int)numericUpDown1.Value, (int)numericUpDown2.Value);
            Size tiles = new Size();
            Size set = new Size((int)numericUpDown3.Value, (int)numericUpDown4.Value);
            if (radioButton1.Checked)
                tiles = new Size(16, 16);
            else if (radioButton2.Checked)
                tiles = new Size(32, 32);
            else
                tiles = new Size(64, 64);

            Form1 form = (Form1)this.Owner;
            form.update_custom(map, tiles, set);
        }
    }
}
