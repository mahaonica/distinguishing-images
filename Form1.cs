using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;

namespace Constellation
{
    public partial class Form1 : Form
    {
        int height, width, areaSize;
        StarPicture picture1, picture2;
        List<StarPicture> history = new List<StarPicture>();

        public Form1()
        {
            FormConfig formConfig = new FormConfig();
            formConfig.ShowDialog();

            InitializeComponent();
            height = formConfig.width;
            width = formConfig.height;
            areaSize = formConfig.areaSize;

            label4.Text = String.Format("Высота: {0}, ширина: {1}, радиус окресности: {2}", height, width, areaSize);

            picture1 = new StarPicture(height, width, areaSize);
            picture2 = new StarPicture(height, width, areaSize);

            (new Thread(() => { Thread.Sleep(200); RefreshPictures(); })).Start();//отрисовка областей
        }

        private void panel1_MouseClick(object sender, MouseEventArgs e)
        {
            picture1.DrawPoint((double)e.X / panel1.Width, (double)e.Y / panel1.Height);
            RefreshPictures();
        }

        private void panel2_MouseClick(object sender, MouseEventArgs e)
        {
            picture2.DrawPoint((double)e.X / panel2.Width, (double)e.Y / panel2.Height);
            RefreshPictures();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            picture1.Clear();
            picture2.Clear();
            RefreshPictures();
            dataGridView1.Columns.Clear();
            dataGridView2.Columns.Clear();
            label3.Text = "";
        }
        
        private void RefreshPictures()
        {
            picture1.PrintGraphics(panel1.CreateGraphics(), panel1.Width, panel1.Height);
            picture2.PrintGraphics(panel2.CreateGraphics(), panel2.Width, panel2.Height);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            picture1.ClearLines();
            picture2.ClearLines();
            RefreshPictures();
            dataGridView1.Columns.Clear();
            dataGridView2.Columns.Clear();
            label3.Text = "";
        }

        private void button4_Click(object sender, EventArgs e)
        {
            picture2 = new StarPicture(picture1);
            RefreshPictures();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            picture1 = new StarPicture(picture2);
            RefreshPictures();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            var newForm = new Form2(picture1, picture2);
            newForm.ShowDialog();
        }

        private void ResizeColumns(DataGridView dgv, int width)
        {
            dgv.Width = width;
            for (int i = 0; i < dgv.Columns.Count - 1; i++)
            {
                dgv.Columns[i].Width = dgv.Width / dgv.Columns.Count;
            }
            if (dgv.Columns.Count - 1 >= 0)
                dgv.Columns[dgv.Columns.Count - 1].Width = dgv.Width - ((dgv.Width / dgv.Columns.Count) * (dgv.Columns.Count - 1)) - 4;
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            //масштаб элементов
            panel1.Width = Math.Min(this.Width / 2 - 80, panel1.Height);
            panel2.Width = Math.Min(this.Width / 2 - 80, panel2.Height);
            panel2.Location = new System.Drawing.Point(this.Width / 2 + 35, panel2.Location.Y);
            label2.Location = new System.Drawing.Point(this.Width / 2 + 5, label2.Location.Y);

            dataGridView1.Width = this.Width / 2 - 40;
            dataGridView2.Width = this.Width / 2 - 40;
            dataGridView2.Location = new System.Drawing.Point(this.Width / 2 + 10, dataGridView2.Location.Y);

            ResizeColumns(dataGridView1, dataGridView1.Width);
            ResizeColumns(dataGridView2, dataGridView2.Width);

            RefreshPictures();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (picture1.Count != picture2.Count)
            {
                MessageBox.Show("Неодинаковое количество точек");
                return;
            }
            if (!picture1.IsCorrectIsolatedPointsSet || !picture2.IsCorrectIsolatedPointsSet)
            {
                MessageBox.Show("Не все точки изолированы");
                return;
            }

            picture1.DrawLines();
            picture2.DrawLines();

            history.Add(new StarPicture(picture1));
            history.Add(new StarPicture(picture2));

            RefreshPictures();
            ShowCharacteristics();
        }

        private void ShowCharacteristics()
        {
            int[] tmpCrt = picture1.GetCharacters();

            dataGridView1.Columns.Clear();
            for (int i = 0; i < tmpCrt.Length; i++)
            {
                dataGridView1.Columns.Add("k" + i.ToString(), "K" + i.ToString());
            }
            for (int i = 0; i < tmpCrt.Length; i++)
                dataGridView1.Rows[0].Cells[i].Value = tmpCrt[i];
            int[] tmpCrt2 = picture2.GetCharacters();
            dataGridView2.Columns.Clear();
            for (int i = 0; i < tmpCrt2.Length; i++)
            {
                dataGridView2.Columns.Add("k" + i.ToString(), "K" + i.ToString());
            }
            for (int i = 0; i < tmpCrt2.Length; i++)
                dataGridView2.Rows[0].Cells[i].Value = tmpCrt2[i];

            bool equal = true;
            for (int i = 0; i < tmpCrt.Length; i++)
                if (tmpCrt[i] != tmpCrt2[i])
                    equal = false;
            if (equal)
                label3.Text = "Характеристики A и B совпадают";
            else
                label3.Text = "Характеристики A и B не совпадают";

            Form1_Resize(null, null);
        }
    }
}
