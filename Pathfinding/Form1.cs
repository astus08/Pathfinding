using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Pathfinding
{
    public partial class Form1 : Form
    {
        Graphics drawArea;
        private int x = -1, y;
        private int width = 10, height = 10;
        private Node start;
        private Node end;
        Node[,] node = new Node[10, 10];

        public Form1()
        {
            InitializeComponent();
            drawArea = panel1.CreateGraphics();
            listBox1.SetSelected(1, true);

            //Double-buffering
            this.SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.DoubleBuffer, true);

            int i, j;
            for (i = 0; i < height; i++)
            {
                for (j = 0; j < width; j++)
                {
                    node[i, j] = new Node(i, j);
                }
            }
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            System.Drawing.Pen mypen = new System.Drawing.Pen(System.Drawing.Color.Black);
            System.Drawing.SolidBrush myBrushGray = new System.Drawing.SolidBrush(System.Drawing.Color.Gray);
            int i, j;
            for (i = 0; i < height; i++)
            {
                for (j = 0; j < width; j++)
                {
                    //draws the outlines of rectangles
                    drawArea.DrawRectangle(mypen, new Rectangle(panel1.Width / width * i, panel1.Height / height * j, panel1.Width / width, panel1.Height / height));
                    //fills the rectangles
                    if (node[i, j].getPermeability())
                        drawArea.FillRectangle(myBrushGray, new Rectangle(panel1.Width / width * j + 1, panel1.Height / height * i + 1, panel1.Width / width - 1, panel1.Height / height - 1));
                    else if (node[i, j].getStart())
                        drawArea.FillRectangle(new System.Drawing.SolidBrush(System.Drawing.Color.Orange), new Rectangle(panel1.Width / width * j + 1, panel1.Height / height * i + 1, panel1.Width / width - 1, panel1.Height / height - 1));
                    else if (node[i, j].getEnd())
                        drawArea.FillRectangle(new System.Drawing.SolidBrush(System.Drawing.Color.LimeGreen), new Rectangle(panel1.Width / width * j + 1, panel1.Height / height * i + 1, panel1.Width / width - 1, panel1.Height / height - 1));
                    else if (node[i, j].isUsed())
                        drawArea.FillRectangle(new System.Drawing.SolidBrush(System.Drawing.Color.Purple), new Rectangle(panel1.Width / width * j + 1, panel1.Height / height * i + 1, panel1.Width / width - 1, panel1.Height / height - 1));
                    else if (node[i, j].isOnPath())
                        drawArea.FillRectangle(new System.Drawing.SolidBrush(System.Drawing.Color.Blue), new Rectangle(panel1.Width / width * j + 1, panel1.Height / height * i + 1, panel1.Width / width - 1, panel1.Height / height - 1));
                }
            }
            drawArea.DrawRectangle(mypen, new Rectangle(0, 0, panel1.Height - 1, panel1.Width - 1));
            //released the tools
            mypen.Dispose();
            myBrushGray.Dispose();

        }

        private void panel1_MouseClick(object sender, MouseEventArgs e)
        {
            Point p = new Point(e.X, e.Y);
            x = p.X / (panel1.Width / width);
            y = p.Y / (panel1.Height / height);
            bool validation = true;
            switch (listBox1.SelectedIndex)
            {
                case 0:
                    node[y, x].setAsClicked();
                    break;
                case 1:
                    foreach (Node nodetab in this.node)
                    {
                        if (nodetab.getStart())
                        {
                            validation = false;
                            nodetab.reset();
                            break;
                        }
                    }
                    if (node[y, x].getStart() && !validation)
                        node[y, x].setAsClicked();
                    else
                        node[y, x].setAsStart();

                    listBox1.SetSelected(0, true);
                    break;
                case 2:
                    foreach (Node nodetab in this.node)
                    {
                        if (nodetab.getEnd())
                        {
                            validation = false;
                            nodetab.reset();
                            break;
                        }
                    }
                    if (node[y, x].getEnd() && !validation)
                        node[y, x].setAsClicked();
                    else
                        node[y, x].setAsEnd();


                    listBox1.SetSelected(0, true);
                    break;
            }

            panel1.Invalidate();
        }

        private void startButton_Click(object sender, EventArgs e) //A* algorithm
        {
            foreach (Node nodetab in this.node)
            {
                if (nodetab.getEnd())
                {
                    this.end = nodetab;
                }
                if (nodetab.getStart())
                {
                    this.start = nodetab;
                }
            }
            foreach (Node nodetab in this.node)
            {
                nodetab.calcDistance(end);
            }
        }

        private void resetButton_Click(object sender, EventArgs e)
        {
            foreach (Node nodetab in this.node)
            {
                nodetab.reset();
            }
            listBox1.SetSelected(1, true);
            panel1.Invalidate();
        }


        //Double-buffering function
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= 0x02000000;  // Turn on WS_EX_COMPOSITED
                return cp;
            }
        }

        private void newGridToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.resetButton_Click(new object(), new EventArgs());
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("This application was created by SIMON-RIMBAULT Guillaume", "About", MessageBoxButtons.OK);
        }


    }
}
