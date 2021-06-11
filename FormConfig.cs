using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Constellation
{
    public partial class FormConfig : Form
    {
        public int height { get; private set; }
        public int width { get; private set; }
        public int areaSize { get; private set; }
        public FormConfig()
        {
            InitializeComponent();
            textBox1_TextChanged(null, null);
            textBox2_TextChanged(null, null);
            textBox3_TextChanged(null, null);
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            int tmp;
            if (int.TryParse(textBox1.Text, out tmp) && tmp >= 10 && tmp <= 100)
                height = tmp;
            else
                textBox1.Text = height.ToString();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            int tmp;
            if (int.TryParse(textBox2.Text, out tmp) && tmp >= 10 && tmp <= 100)
                width = tmp;
            else
                textBox2.Text = width.ToString();
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            int tmp;
            if (int.TryParse(textBox3.Text, out tmp) && tmp >= 1 && tmp <= 10)
                areaSize = tmp;
            else
                textBox3.Text = areaSize.ToString();
        }
    }
}
