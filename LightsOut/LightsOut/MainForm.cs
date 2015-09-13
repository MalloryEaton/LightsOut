using System;
using System.Drawing;
using System.Windows.Forms;

namespace LightsOut
{
    public partial class MainForm : Form
    {
        private static int gridSize = 3;

        private const int GRID_OFFSET = 25; // Distance from upper-left side of window
        private static int gridLength = 200; // Size in pixels of grid
        //private const int NUM_CELLS = 3; // Number of cells in grid
        private static int cellLength = gridLength / gridSize;
        private bool[,] grid; // Stores on/off state of cells in grid
        private Random rand; // Used to generate random numbers
        
        public MainForm()
        {
            InitializeComponent();

            rand = new Random(); // Initializes random number generator
            grid = new bool[gridSize, gridSize];
            // Turn entire grid on
            for (int r = 0; r < gridSize; r++)
            {
                for (int c = 0; c < gridSize; c++)
                {
                    grid[r, c] = true;
                }
            }
        }

        private void MainForm_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            for (int r = 0; r < gridSize; r++)
            {
                for (int c = 0; c < gridSize; c++)
                {
                    // Get proper pen and brush for on/off
                    // grid section
                    Brush brush;
                    Pen pen;
                    if (grid[r, c])
                    {
                        pen = Pens.Black;
                        brush = Brushes.White; // On
                    }
                    else
                    {
                        pen = Pens.White;
                        brush = Brushes.Black; // Off
                    }
                    // Determine (x,y) coord of row and col to draw rectangle
                    int x = c * cellLength + GRID_OFFSET;
                    int y = r * cellLength + GRID_OFFSET;
                    // Draw outline and inner rectangle
                    g.DrawRectangle(pen, x, y, cellLength, cellLength);
                    g.FillRectangle(brush, x + 1, y + 1, cellLength - 1, cellLength - 1);
                }
            }
        }

        private void MainForm_MouseDown(object sender, MouseEventArgs e)
        {
            // Make sure click was inside the grid
            if (e.X < GRID_OFFSET || e.X > cellLength * gridSize + GRID_OFFSET ||
            e.Y < GRID_OFFSET || e.Y > cellLength * gridSize + GRID_OFFSET)
                return;
            // Find row, col of mouse press
            int r = (e.Y - GRID_OFFSET) / cellLength;
            int c = (e.X - GRID_OFFSET) / cellLength;
            // Invert selected box and all surrounding boxes
            for (int i = r - 1; i <= r + 1; i++)
            {
                for (int j = c - 1; j <= c + 1; j++)
                {
                    if (i >= 0 && i < gridSize && j >= 0 && j < gridSize)
                    {
                        grid[i, j] = !grid[i, j];
                    }
                }
            }

            // Redraw grid
            this.Invalidate();
            // Check to see if puzzle has been solved
            if (PlayerWon())
            {
                // Display winner dialog box
                MessageBox.Show(this, "Congratulations! You've won!", "Lights Out!",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private bool PlayerWon()
        {
            for(int i = 0; i < gridSize; i++)
            {
                for(int j = 0; j < gridSize; j++)
                {
                    if(grid[i, j])
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        private void newGameButton_Click(object sender, EventArgs e)
        {
            // Fill grid with either white or black
            for (int r = 0; r < gridSize; r++)
                for (int c = 0; c < gridSize; c++)
                    grid[r, c] = rand.Next(2) == 1;
            // Redraw grid
            this.Invalidate();
        }

        private void exitButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            newGameButton_Click(sender, e);
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutForm aboutBox = new AboutForm();
            aboutBox.ShowDialog(this);
        }

        private void x3ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            changeSize(3);
        }

        private void x4ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            changeSize(4);
        }

        private void x5ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            changeSize(5);
        }

        private void changeSize(int size)
        {
            gridSize = size;
            x3ToolStripMenuItem.Checked = false;
            x4ToolStripMenuItem.Checked = false;
            x5ToolStripMenuItem.Checked = false;
            switch (size)
            {
                case 3:
                    x3ToolStripMenuItem.Checked = true;
                    gridLength = 200;
                    cellLength = gridLength / gridSize;
                    break;
                case 4:
                    x4ToolStripMenuItem.Checked = true;
                    gridLength = 190;
                    cellLength = gridLength / gridSize;
                    break;
                case 5:
                    x5ToolStripMenuItem.Checked = true;
                    gridLength = 180;
                    cellLength = gridLength / gridSize;
                    break;
            }

            grid = new bool[gridSize, gridSize];
            // Turn entire grid on
            for (int r = 0; r < gridSize; r++)
            {
                for (int c = 0; c < gridSize; c++)
                {
                    grid[r, c] = true;
                }
            }
            this.Invalidate();
        }
    }
}
