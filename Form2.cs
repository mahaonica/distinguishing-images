using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace Constellation
{
    public partial class Form2 : Form
    {
        private List<DataGridView> grids1, grids2;
        private List<Label> labels1, labels2;
        private StarPicture picture1, picture2;
        private List<Chart> charts;

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

        private void Form2_Resize(object sender, EventArgs e)
        {
            int tmpWidth = this.Width / 2 - 50;
            foreach (var dgv in grids1)
            {
                ResizeColumns(dgv, tmpWidth);
            }
            foreach (var dgv in grids2)
            {
                dgv.Location = new System.Drawing.Point(this.Width / 2 + 10, dgv.Location.Y);
                ResizeColumns(dgv, tmpWidth);
            }
            foreach (var lbl in labels2)
            {
                lbl.Width = tmpWidth;
                lbl.Location = new System.Drawing.Point(this.Width / 2 + 10, lbl.Location.Y);
            }
        }

        private double Ro(Point a, Point b)
        {
            return Math.Sqrt((a.X - b.X) * (a.X - b.X) + (a.Y - b.Y) * (a.Y - b.Y));
        }

        private double CharactersSumDiff(int[] charc1, int[] charc2)
        {
            double ans = 0;
            for (int i = 0; i < charc1.Length; i++)
                ans += Math.Abs(charc1[i] - charc2[i]);
            return Math.Sqrt(ans);
        }

        public Form2(StarPicture picture1, StarPicture picture2)
        {
            InitializeComponent();
            if (picture1 == null || picture2 == null || picture1.detectedPoints == null || picture2.detectedPoints == null)
                return;
            this.picture1 = picture1;
            this.picture2 = picture2;
            int top = 11;

            labels1 = new List<Label>();
            labels1.Add(CreateLeftLabel("headerLabelA", "Характеристика A", ref top, false));
            labels2 = new List<Label>();
            labels2.Add(CreateLeftLabel("headerLabelB", "Характеристика B", ref top));

            top += 2;

            grids1 = new List<DataGridView>();
            var tmpViev = CreateLeftViev("headerVievA", ref top, false);
            grids1.Add(tmpViev);
            FillViev(tmpViev, picture1.GetCharacters());

            grids2 = new List<DataGridView>();
            tmpViev = CreateLeftViev("headerVievB", ref top);
            grids2.Add(tmpViev);
            FillViev(tmpViev, picture2.GetCharacters());

            top += 5;

            charts = new List<Chart>();
            charts.Add(CreateLeftChart("chartAB", "Сравнение A и B без учета K0", "A", "B", ref top, picture1.GetCharacters(), picture2.GetCharacters()));

            top += 5;

            double maxCharactersSumDiff = 0;
            Point[] maxDiffPoints = new Point[2];

            for (int i = 0; i < picture1.detectedPoints.Count; i++)
            {
                string pointLiter1 = String.Format("({0}; {1})", picture1.detectedPoints[i].X, picture1.detectedPoints[i].Y);
                labels1.Add(CreateLeftLabel("LabelA" + pointLiter1, "Точка A " + pointLiter1, ref top, false));
                string pointLiter2 = String.Format("({0}; {1})", picture2.detectedPoints[i].X, picture2.detectedPoints[i].Y);
                labels2.Add(CreateLeftLabel("LabelA" + pointLiter2, "Точка B " + pointLiter2, ref top));

                top += 2;

                tmpViev = CreateLeftViev("VievA" + pointLiter1, ref top, false);
                grids1.Add(tmpViev);
                FillViev(tmpViev, picture1.pointsCharacters[i]);
                tmpViev = CreateLeftViev("VievB" + pointLiter2, ref top);
                grids2.Add(tmpViev);
                FillViev(tmpViev, picture2.pointsCharacters[i]);

                top += 5;

                double charactersSumDiff = CharactersSumDiff(picture1.pointsCharacters[i], picture2.pointsCharacters[i]);
                if(charactersSumDiff > maxCharactersSumDiff)
                {
                    maxCharactersSumDiff = charactersSumDiff;
                    maxDiffPoints[0] = picture1.detectedPoints[i];
                    maxDiffPoints[1] = picture2.detectedPoints[i];
                }

                //labels1.Add(CreateLeftLabel("ro" + pointLiter1 + pointLiter2, "ρ(" + pointLiter1 + ", " + pointLiter2 + ") = " + string.Format("{0:0.0000}", charactersSumDiff), ref top, false));
                labels1.Add(CreateLeftLabel("diff" + pointLiter1 + pointLiter2, "Расстояние между A" + pointLiter1 + " и B" + pointLiter2 + " = " + string.Format("{0:0.00000}", charactersSumDiff), ref top));

                top += 2;

                charts.Add(CreateLeftChart("chartA" + pointLiter1 + "B" + pointLiter2, "Сравнение A" + pointLiter1 + " и B" + pointLiter2 + " без учета K0", "A", "B", ref top, picture1.pointsCharacters[i], picture2.pointsCharacters[i]));

                top += 5;
            }

            if (maxCharactersSumDiff != 0)
            {
                string pointLiter1 = String.Format("({0}; {1})", maxDiffPoints[0].X, maxDiffPoints[0].Y);
                string pointLiter2 = String.Format("({0}; {1})", maxDiffPoints[1].X, maxDiffPoints[1].Y);
                labels1.Add(CreateLeftLabel("maxdiff", "Максимальное расстояние между A" + pointLiter1 + " и B" + pointLiter2 + " = " + string.Format("{0:0.00000}", maxCharactersSumDiff), ref top));
            }
            else
                labels1.Add(CreateLeftLabel("maxdiff", "Характеристики всех точек совпадают", ref top));
            top += 50;
            labels1.Add(CreateLeftLabel("empty", "", ref top));
            Form2_Resize(null, null);
        }

        private void FillViev(DataGridView grinViev, int[] data)
        {
            grinViev.Columns.Clear();
            for (int i = 0; i < data.Length; i++)
            {
                grinViev.Columns.Add("k" + i.ToString(), "K" + i.ToString());
                grinViev.Rows[0].Cells[i].Value = data[i];
            }   
        }

        private DataGridView CreateLeftViev(string ident, ref int top, bool changeTop = true)
        {
            DataGridView newGrid = new DataGridView();
            newGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)));
            newGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            newGrid.Location = new System.Drawing.Point(11, top);
            newGrid.Name = ident;
            newGrid.ReadOnly = true;
            newGrid.RowHeadersVisible = false;
            newGrid.RowHeadersWidth = 10;
            newGrid.ShowCellErrors = false;
            newGrid.ShowCellToolTips = false;
            newGrid.ShowEditingIcon = false;
            newGrid.ShowRowErrors = false;
            newGrid.Size = new System.Drawing.Size(this.Width / 2 - 40, 44);
            newGrid.TabIndex = 3;
            this.Controls.Add(newGrid);

            if(changeTop)
                top += 44;

            return newGrid;
        }

        private Label CreateLeftLabel(string ident, string text, ref int top, bool changeTop = true)
        {
            Label newLabel = new Label();

            newLabel.AutoSize = true;
            newLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            newLabel.Location = new System.Drawing.Point(11, top);
            newLabel.Name = ident;
            newLabel.Height = 15;
            newLabel.TabIndex = 7;
            newLabel.Text = text;
            this.Controls.Add(newLabel);

            if(changeTop)
                top += 15;

            return newLabel;
        }

        private Chart CreateLeftChart(string ident, string title, string series1t, string series2t, ref int top, int[] data1, int[] data2)
        {
            ChartArea chartArea1 = new ChartArea();
            Legend legend1 = new Legend();
            Series series1 = new Series();
            Series series2 = new Series();
            Title title1 = new Title();

            Chart chart1 = new Chart();
            chart1.Name = ident;
            chart1.Palette = ChartColorPalette.Pastel;
            chart1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));

            chartArea1.Name = "ChartArea1";
            chartArea1.AxisX.IntervalAutoMode = IntervalAutoMode.VariableCount;
            chart1.ChartAreas.Add(chartArea1);
            legend1.Name = "Legend1";
            chart1.Legends.Add(legend1);
            chart1.Location = new System.Drawing.Point(10, top);
            chart1.Name = "chart1";

            series1.ChartArea = "ChartArea1";
            series1.Legend = "Legend1";
            series1.MarkerStep = 1;
            series1.Name = series1t;

            series2.ChartArea = "ChartArea1";
            series2.Legend = "Legend1";
            series2.MarkerStep = 1;
            series2.Name = series2t;

            bool equal = data1[0] == data2[0];
            for (int i = 1; i < data2.Length; i++)
            {
                series1.Points.AddXY("K" + i.ToString(), data1[i]);
                series2.Points.AddXY("K" + i.ToString(), data2[i]);
                if (data1[i] != data2[i])
                    equal = false;
            }

            chart1.Series.Add(series1);
            chart1.Series.Add(series2);
            chart1.Height = 200;
            chart1.Width = this.Width - 20;
            chart1.TabIndex = 0;
            chart1.Text = "chart1";
            title1.Name = "Title1";
            title1.Text = title + (equal ? "(Характеристики совпадают)" : "(Характеристики не совпадают)");

            chart1.Titles.Add(title1);
            this.Controls.Add(chart1);

            top += 200;

            return chart1;
        } 
    }
}
