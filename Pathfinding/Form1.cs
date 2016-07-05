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
            for (i = 0; i < height; i++)
            {
                for (j = 0; j < width; j++)
                {
                    int u = j - 1;
                    int d = j + 1;
                    int l = i - 1;
                    int r = i + 1;
                    Node up = null;
                    Node down = null;
                    Node left = null;
                    Node right = null;
                    if (u >= 0 && u < height)
                    {
                        up = this.node[i, u];
                    }
                    if (d >= 0 && d < height)
                    {
                        down = this.node[i, d];
                    }
                    if (l >= 0 && l < width)
                    {
                        left = this.node[l, j];
                    }
                    if (r >= 0 && r < width)
                    {
                        right = this.node[r, j];
                    }
                    this.node[i, j].setDirections(left, right, up, down);
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
                    else if (node[i, j].isOnPath())
                        drawArea.FillRectangle(new System.Drawing.SolidBrush(System.Drawing.Color.Blue), new Rectangle(panel1.Width / width * j + 1, panel1.Height / height * i + 1, panel1.Width / width - 1, panel1.Height / height - 1));
                    else if (node[i, j].isUsed())
                        drawArea.FillRectangle(new System.Drawing.SolidBrush(System.Drawing.Color.Purple), new Rectangle(panel1.Width / width * j + 1, panel1.Height / height * i + 1, panel1.Width / width - 1, panel1.Height / height - 1));
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

        //This is the A* algorithm
        private void startButton_Click(object sender, EventArgs e)
        {
            Boolean calculated = false;
            Boolean calculCalculated = false;
            foreach (Node nodetab in this.node)     //Save starting and ending nodes
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
            List<Node> closedList = new List<Node>();
            List<Node> openList = new List<Node>();
            openList.Add(this.start);
            while (!calculated)
            {
                calculCalculated = false;
                if (openList.Count == 0)
                {
                    break;
                }
                List<Node> oldOpen = new List<Node>();
                int i=0;
                foreach (Node node in openList)
                {
                    oldOpen.Add(node);
                }
                while (i < oldOpen.Count)
                {
                    System.Threading.Thread.Sleep(20);
                    if (calculated)
                    {
                        calculCalculated = true;
                        break;
                    }
                    Node s = (Node)oldOpen[i];
                    Node up = s.getUp();
                    Node down = s.getDown();
                    Node left = s.getLeft();
                    Node right = s.getRight();
                    if (up != null && up.getEnd()) 
                        calculated = true;
                    else if (down != null && down.getEnd()) 
                        calculated = true;
                    else if (left != null && left.getEnd()) 
                        calculated = true;
                    else if (right != null && right.getEnd()) 
                        calculated = true;
                    else
                    {
                        if (!(up == null || closedList.Contains(up) || up.getPermeability() || up.getParent() != null && up.getDistance() + s.getFactor() >= up.getFactor()))
                        {
                            up.setFactor(s.getFactor() + up.getDistance());
                            up.setParent(s);
                            openList.Add(up);
                            up.use();
                        }
                        if (!(down == null || closedList.Contains(down) || down.getPermeability() || down.getParent() != null && down.getDistance() + s.getFactor() >= down.getFactor()))
                        {
                            down.setFactor(s.getFactor() + up.getDistance());
                            down.setParent(s);
                            openList.Add(down);
                            down.use();
                        }
                        if (!(right == null || closedList.Contains(right) || right.getPermeability() || right.getParent() != null && right.getDistance() + s.getFactor() >= right.getFactor()))
                        {
                            right.setFactor(s.getFactor() + right.getDistance());
                            right.setParent(s);
                            openList.Add(right);
                            right.use();
                        }
                        if (!(left == null || closedList.Contains(left) || left.getPermeability() || left.getParent() != null && left.getDistance() + s.getFactor() >= left.getFactor()))
                        {
                            left.setFactor(s.getFactor() + left.getDistance());
                            left.setParent(s);
                            openList.Add(left);
                            left.use();
                        }
                        closedList.Add(s);
                        openList.Remove(s);
                    }
                    if (calculCalculated)
                    {
                        continue;
                    }
                    if (calculated)
                    {
                        Node n = s.traceBack();
                        while (n != null)
                        {
                            System.Threading.Thread.Sleep(20);
                            n = n.traceBack();
                        }
                    }
                    ++i;
                }
            }
            panel1.Invalidate();
        }

        // This function reset all nodes
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
