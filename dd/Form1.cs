using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace dd
{
    public partial class Form1 : Form
    {
        public struct PathDots
        {
            private int path_no;
            private int[] x;
            private int[] y;
            private int is_active;
            public int[] X
            {
                get { return x; }
                set { x = value; }
            }
            public int[] Y
            {
                get { return y; }
                set { y = value; }
            }
            public int PATH_NO
            {
                get { return path_no; }
                set { path_no = value; }
            }
            public int IS_ACTIVE
            {
                get { return is_active; }
                set { is_active = value; }
            }
        }

        List<int> coordsX;
        List<int> coordsY;
        Hesaplamalar hesaplamalar;
        int path_counter;
        PathDots pathDots_s;
        public List<PathDots> ListOfPaths;
        public int alan;
        Graphics graphics;
        Dictionary<colors, Color> colors_d;
        Array col_enum_val;
        int offset;
        int coordType;

        public enum colors
        {
            Blue,
            Black,
            Purple,
            Orange,
            Pink,
            DarkSlateGray,
            SaddleBrown
        }

        public Form1()
        {
            InitializeComponent(); 
            coordsX = new List<int>();
            coordsY = new List<int>();
            path_counter = 0;
            alan = 25000;
            offset = 0;
            coordType = 2;
            label1.Text = "Alan: " + alan.ToString();
            label7.Text = "Offset Değeri: " + offset.ToString();
            hesaplamalar = new Hesaplamalar(this);
            pathDots_s = new PathDots();
            ListOfPaths = new List<PathDots>();
            graphics = panel1.CreateGraphics();
            ListView1Settings();
            ColorsEnumSettings();
        }

        void ListView1Settings()
        {
            listView1.View = View.Details;
            listView1.Columns.Add("Path No");
            listView1.Columns.Add("X");
            listView1.Columns.Add("Y");
            listView1.GridLines = true;
        }

        void ColorsEnumSettings()
        {
            colors_d = new Dictionary<colors, Color>();
            colors_d.Add(colors.Blue, Color.Blue);
            colors_d.Add(colors.Black, Color.Black);
            colors_d.Add(colors.Purple, Color.Purple);
            colors_d.Add(colors.Orange, Color.Orange);
            colors_d.Add(colors.Pink, Color.Pink); 
            colors_d.Add(colors.DarkSlateGray, Color.DarkSlateGray); 
            colors_d.Add(colors.SaddleBrown, Color.SaddleBrown);
            col_enum_val = Enum.GetValues(typeof(colors));
        }

        Color GetColor(int path_no)
        {
            int get_val = path_no % col_enum_val.Length;
            colors col_val = (colors)col_enum_val.GetValue(get_val);
            Color color = colors_d[col_val];
            return color;
        }

        void DrawExampleRectangle()
        {
            Graphics graphics_example_rect;
            graphics_example_rect = panel3.CreateGraphics();
            Pen pen = new Pen(Color.Red, 1);
            graphics_example_rect.DrawRectangle(pen, 10, 10, 400, 250);
            graphics_example_rect.Dispose();
        }

        void FillListView(int path_no, Color color)
        {
            for (int i = 0; i < ListOfPaths[path_no].X.Length; i++)
            {
                listView1.Items.Add(new ListViewItem(new string[] { ListOfPaths[path_no].PATH_NO.ToString(), ListOfPaths[path_no].X[i].ToString(),
                        ListOfPaths[path_no].Y[i].ToString()}));
            }
            ListViewItem color_lv = new ListViewItem();
            color_lv.ForeColor = color;
            color_lv.Text = "PATH NO: " + path_no.ToString();
            listView2.Items.Add(color_lv);
        }

        void FillPathDotsStruct(int path_no, List<int> cordX, List<int> cordY, int is_active)
        {
            pathDots_s.X = new int[cordX.Count];
            pathDots_s.Y = new int[cordY.Count];
            for (int i = 0; i < coordsX.Count; i++)
            {
                pathDots_s.X[i] = cordX[i];
                pathDots_s.Y[i] = cordY[i];
            }
            pathDots_s.PATH_NO = path_no;
            pathDots_s.IS_ACTIVE = is_active;
            ListOfPaths.Add(pathDots_s);
        }

        void DrawLine(Graphics graphics, Color color, int thick)
        {
            Pen pen = new Pen(color, thick);
            Point[] Coords = new Point[coordsX.Count];
            for (int i = 0; i < coordsX.Count; i++)
            {
                Coords[i] = new Point(coordsX[i], coordsY[i]);
            }
            graphics.DrawLines(pen, Coords);
            path_counter++;
            Update();
        }

        void HighlightLine(Graphics graphics, int path_no, int thick)
        {
            Pen pen = new Pen(Color.Red, thick);
            Point[] Coords = new Point[ListOfPaths[path_no].X.Length];
            for (int i = 0; i < Coords.Length; i++)
            {
                Coords[i] = new Point(ListOfPaths[path_no].X[i], ListOfPaths[path_no].Y[i]);
            }
            Label label = new Label();
            label.Text = "PATH: " + path_no.ToString();
            label.Location = Coords[0];
            label.BackColor = Color.Transparent;
            panel1.Controls.Add(label);
            graphics.DrawLines(pen, Coords);
            Update();
        }

        void DrawRectangle(Graphics graphics, Hesaplamalar.RectangleSpecs rectangleSpecs_s, int thick)
        {
            Pen pen = new Pen(Color.Green, thick);
            if (rectangleSpecs_s.IS_IT_OK == true) graphics.DrawRectangle(pen, rectangleSpecs_s.COORDS_X[0], rectangleSpecs_s.COORDS_Y[0],
                    rectangleSpecs_s.W, rectangleSpecs_s.H);
            else 
            {
                richTextBox1.AppendText("ERR: PATH " + rectangleSpecs_s.PATH_NO + " ALANA SIĞMIYOR" + Environment.NewLine + 
                    "Girilen Alan: " + alan.ToString() + "  Hesaplanan Alan: " + rectangleSpecs_s.AREA + Environment.NewLine +
                    "-------------------------------------------------------" + Environment.NewLine);
                //HighlightLine(graphics, rectangleSpecs_s.PATH_NO, 2);
            }
            textBox3.AppendText("###### PATH NO : " + rectangleSpecs_s.PATH_NO.ToString() + " ###########" + Environment.NewLine);
            textBox3.AppendText("Rect x:       " + rectangleSpecs_s.COORDS_X[0] + "     Rect y: " + rectangleSpecs_s.COORDS_Y[0] + Environment.NewLine);
            textBox3.AppendText("Rect w:       " + rectangleSpecs_s.W + "     Rect h: " + rectangleSpecs_s.H + Environment.NewLine);
            textBox3.AppendText("Rect x1:      " + rectangleSpecs_s.COORDS_X[0] + "     Rect y1: " + rectangleSpecs_s.COORDS_Y[0] + Environment.NewLine);
            textBox3.AppendText("Rect x2:      " + rectangleSpecs_s.COORDS_X[1] + "     Rect y2: " + rectangleSpecs_s.COORDS_Y[1] + Environment.NewLine);
            textBox3.AppendText("Rect x3:      " + rectangleSpecs_s.COORDS_X[2] + "     Rect y3: " + rectangleSpecs_s.COORDS_Y[2] + Environment.NewLine);
            textBox3.AppendText("Rect x4:      " + rectangleSpecs_s.COORDS_X[3] + "     Rect y4: " + rectangleSpecs_s.COORDS_Y[3] + Environment.NewLine);
            textBox3.AppendText("Durumu:      " + rectangleSpecs_s.IS_IT_OK + Environment.NewLine);
            textBox3.AppendText("Girilen Alan: " + alan.ToString() + "       Path Alanı: " + rectangleSpecs_s.AREA + Environment.NewLine);
            textBox3.AppendText("######################################################################" + Environment.NewLine);
            Update();
        }

        void LeftMouseClicked(int x, int y)
        {
            coordsX.Add(x);
            coordsY.Add(y);
            graphics.FillRectangle(new SolidBrush(GetColor(path_counter)), (x - 2), (y - 2), 4, 4);
            textBox3.AppendText("Left Mouse clicked    " + "X:  " + x + "   Y:  " + y + Environment.NewLine +
                "Point Counter: " + coordsX.Count() + Environment.NewLine);
        }
        void RightMouseClicked()
        {
            textBox3.AppendText("Right Mouse clicked" + Environment.NewLine + 
                "########################" + Environment.NewLine);
            if (coordsX.Count > 1)
            {
                FillPathDotsStruct(path_counter, coordsX, coordsY, 1);
                Color color = GetColor(path_counter);
                FillListView(path_counter, color);
                if(ListOfPaths[path_counter].IS_ACTIVE==1)DrawLine(graphics, color, 2);
            }
            coordsX.Clear();
            coordsY.Clear();
            for (int i = 0; i < ListOfPaths.Count; i++)
            {
                for (int a = 0; a < ListOfPaths[i].X.Length; a++)
                {
                    textBox3.AppendText("Path No: " + ListOfPaths[i].PATH_NO + " X" + a + " " + ListOfPaths[i].X[a]
                        + " Y" + a + " " + ListOfPaths[i].Y[a] + Environment.NewLine);
                }
                textBox3.AppendText("########################" + Environment.NewLine);
            }
        }

        private void panel1_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                LeftMouseClicked(e.X, e.Y);
            }

            if (e.Button == MouseButtons.Right)
            {
                RightMouseClicked();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
            listView2.Items.Clear();
            richTextBox1.Clear();
            coordsX.Clear();
            coordsY.Clear();
            graphics.Clear(Color.White);
            ListOfPaths.Clear();
            panel1.Controls.Clear();
            path_counter = 0;
            textBox3.AppendText(Environment.NewLine + "######### CLEAR #########" + Environment.NewLine + Environment.NewLine);
            panel1.BackgroundImage = Properties.Resources.map;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            textBox3.AppendText("Alan: " + textBox1.Text + Environment.NewLine);
            label1.Text = "Alan: " + textBox1.Text;
            if (String.IsNullOrEmpty(textBox1.Text))
            {
                richTextBox1.AppendText("ERR: ALAN DEĞERİ GİRİLMEMİŞ" + Environment.NewLine);
            }
            else 
            {
                alan = int.Parse(textBox1.Text);
                if (alan == 0)
                {
                    richTextBox1.AppendText("ERR: ALAN DEĞERİ 0" + Environment.NewLine);
                }
                else
                {
                    richTextBox1.Clear();
                }
            }
            textBox1.Clear();
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            richTextBox1.Clear();
            if (alan > 0)
            {
                textBox3.AppendText("########## MAKE RECT ##########" + Environment.NewLine);
                for (int i = 0; i < ListOfPaths.Count; i++) //path sayısı
                {
                    if (ListOfPaths[i].IS_ACTIVE == 1) 
                    {
                        DrawRectangle(graphics, hesaplamalar.calculate(alan, i, ListOfPaths[i].X, ListOfPaths[i].Y, offset, coordType), 2);
                    } 
                }
            }
            else
            {
                richTextBox1.AppendText("ERR: ALAN DEĞERİ GEÇERSİZ" + Environment.NewLine);
            }
        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {
            DrawExampleRectangle();
        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            textBox3.AppendText("Offset Değeri: " + textBox2.Text + Environment.NewLine);
            label7.Text = "Offset Değeri: " + textBox2.Text;
            if (String.IsNullOrEmpty(textBox2.Text))
            {
                richTextBox1.AppendText("ERR: OFFSET DEĞERİ GİRİLMEMİŞ" + Environment.NewLine);
            }
            else
            {
                offset = int.Parse(textBox2.Text);
            }
            textBox2.Clear();
        }
    }
}
