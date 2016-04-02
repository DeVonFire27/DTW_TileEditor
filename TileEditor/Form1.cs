using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace TileEditor
{
    public partial class Form1 : Form
    {
        Customize tool = null;
        Size tileSetSize = new Size(4, 2);
        Size tileSize = new Size(64, 64);

        Size mapSize = new Size(5, 5);
        Tile[,] map = new Tile[5, 5];

        Tile selectedTile;
        Bitmap BitTile = Properties.Resources._default;

        public Form1()
        {
            InitializeComponent();

            Graphics g = graphicsPanel1.CreateGraphics();
            BitTile.SetResolution(g.DpiX, g.DpiY);
            g.Dispose();

            graphicsPanel1.AutoScrollMinSize = BitTile.Size;
            graphicsPanel2.AutoScrollMinSize = new Size((tileSize.Width*mapSize.Width), (tileSize.Height*mapSize.Height));
        }

        private void graphicsPanel1_Paint(object sender, PaintEventArgs e)
        {
            Point offset = new Point(0, 0);
            offset.X += graphicsPanel1.AutoScrollPosition.X;
            offset.Y += graphicsPanel1.AutoScrollPosition.Y;

            e.Graphics.DrawImage(BitTile, offset);
            for (int x = 0; x < tileSetSize.Width; x++)
            {
                for (int y = 0; y < tileSetSize.Height; y++)
                {
                    offset = new Point(0, 0);
                    offset.X += graphicsPanel1.AutoScrollPosition.X;
                    offset.Y += graphicsPanel1.AutoScrollPosition.Y;

                    Rectangle destRect = Rectangle.Empty;
                    destRect.X = (x * tileSize.Width) + offset.X;
                    destRect.Y = (y * tileSize.Height) + offset.Y;
                    destRect.Size = tileSize;

                    e.Graphics.DrawRectangle(Pens.Black, destRect);
                }
            }
        }

        private void graphicsPanel2_Paint(object sender, PaintEventArgs e)
        {
            for (int x = 0; x < mapSize.Width; x++)
            {
                for (int y = 0; y < mapSize.Height; y++)
                {
                    Point offset = new Point(0, 0);
                    offset.X += graphicsPanel2.AutoScrollPosition.X;
                    offset.Y += graphicsPanel2.AutoScrollPosition.Y;

                    Rectangle destRect = Rectangle.Empty;
                    destRect.X = (x * tileSize.Width) + offset.X;
                    destRect.Y = (y * tileSize.Height) + offset.Y;
                    destRect.Size = tileSize;

                    Rectangle srcRect = Rectangle.Empty;
                    srcRect.X = map[x, y].X * tileSize.Width;
                    srcRect.Y = map[x, y].Y * tileSize.Height;
                    srcRect.Size = tileSize;

                    e.Graphics.DrawImage(BitTile, destRect, srcRect, GraphicsUnit.Pixel);

                    e.Graphics.DrawRectangle(Pens.Black, destRect);
                }
            }
        }

        private void graphicsPanel1_MouseClick(object sender, MouseEventArgs e)
        {
            Point offset = e.Location;
            offset.X -= graphicsPanel1.AutoScrollPosition.X;
            offset.Y -= graphicsPanel1.AutoScrollPosition.Y;
            int x = offset.X / tileSize.Width;
            int y = offset.Y / tileSize.Height;
            if (x < tileSetSize.Width && y < tileSetSize.Height)
            {
                selectedTile.X = x;
                selectedTile.Y = y;
            }
            graphicsPanel1.Invalidate();
        }

        private void graphicsPanel2_MouseClick(object sender, MouseEventArgs e)
        {
            Point offset = e.Location;
            offset.X -= graphicsPanel2.AutoScrollPosition.X;
            offset.Y -= graphicsPanel2.AutoScrollPosition.Y;
            int x = offset.X / tileSize.Width;
            int y = offset.Y / tileSize.Height;
            if (x < mapSize.Width && y < mapSize.Height)
                map[x, y] = selectedTile;

            graphicsPanel2.Invalidate();
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            selectedTile.X = 0;
            selectedTile.Y = 0;
            mapSize = new Size(5, 5);
            map = new Tile[5, 5];
            for (int x = 0; x < mapSize.Width; x++)
                for (int y = 0; y < mapSize.Height; y++)
                    map[x, y] = selectedTile;
            graphicsPanel1.Invalidate();
            graphicsPanel2.Invalidate();
        }

        private void customizeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (tool == null)
            {
                tool = new Customize();
                tool.SetDefault(mapSize, tileSize, tileSetSize);
                tool.FormClosed += tool_FormClosed;
                tool.Owner = this;
                tool.Show(this);
            }
        }

        void tool_FormClosed(object sender, FormClosedEventArgs e)
        {
            tool = null;
        }

        public void update_custom(Size SizeMap, Size TilePixels, Size SetSize)
        {
            Tile[,] mapTemp = new Tile[mapSize.Width, mapSize.Height];
            Size tempSize = new Size(mapSize.Width, mapSize.Height);
            mapTemp = map;
            mapSize = new Size(SizeMap.Width, SizeMap.Height);
            map = new Tile[SizeMap.Width, SizeMap.Height];
            selectedTile.X = 0;
            selectedTile.Y = 0;

            tileSetSize = SetSize;
            tileSize = TilePixels;

            graphicsPanel1.AutoScrollMinSize = BitTile.Size;
            graphicsPanel2.AutoScrollMinSize = new Size((tileSize.Width * mapSize.Width), (tileSize.Height * mapSize.Height));

            for (int x = 0; x < mapSize.Width; x++)
                for (int y = 0; y < mapSize.Height; y++)
                {
                    if (x < tempSize.Width && y < tempSize.Height)
                        map[x, y] = mapTemp[x, y];
                    else
                        map[x, y] = selectedTile;
                }

            graphicsPanel1.Invalidate();
            graphicsPanel2.Invalidate();
        }

        private void importTileSetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();

            if (DialogResult.OK == dlg.ShowDialog())
            {
                BitTile = new Bitmap(dlg.FileName);

                // Adjust DPI of Bitmap
                Graphics g = graphicsPanel1.CreateGraphics();
                BitTile.SetResolution(g.DpiX, g.DpiY);
                g.Dispose();

                graphicsPanel1.AutoScrollMinSize = BitTile.Size;

                selectedTile.X = 0;
                selectedTile.Y = 0;
                for (int x = 0; x < mapSize.Width; x++)
                    for (int y = 0; y < mapSize.Height; y++)
                        map[x, y] = selectedTile;
                graphicsPanel1.Invalidate();
                graphicsPanel2.Invalidate();
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Stream myStream;
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();

            saveFileDialog1.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            saveFileDialog1.AddExtension = true;
            saveFileDialog1.DefaultExt = ".txt";

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                if ((myStream = saveFileDialog1.OpenFile()) != null)
                {
                    // Code to write the stream goes here.
                    StreamWriter write = new StreamWriter(myStream);
                    string line = tileSize.Width.ToString() + ',' + tileSize.Height.ToString() + ',' + tileSetSize.Width.ToString() + ','
                        + tileSetSize.Height.ToString() + ',' + mapSize.Width.ToString() + ',' + mapSize.Height.ToString();
                    write.WriteLine(line);
                    for (int y = 0; y < mapSize.Height; y++)
                    {
                        line = "";
                        for (int x = 0; x < mapSize.Width; x++)
                        {
                            int index = (map[x, y].Y * tileSetSize.Width) + map[x, y].X;
                            line += index.ToString();
                            if (x < mapSize.Width - 1)
                                line += ',';
                        }
                        write.WriteLine(line);
                    }
                    write.Close();
                    myStream.Close();
                }
            }
        }

        private void loadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            openFileDialog1.InitialDirectory = "c:\\";
            openFileDialog1.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                StreamReader read = new StreamReader(openFileDialog1.FileName);
                string line = read.ReadLine();
                string[] result;
                char[] comma = new char[] { ',' };

                result = line.Split(comma);
                tileSize = new Size(int.Parse(result[0]), int.Parse(result[1]));
                tileSetSize = new Size(int.Parse(result[2]), int.Parse(result[3]));
                mapSize = new Size(int.Parse(result[4]), int.Parse(result[5]));
                map = new Tile[mapSize.Width, mapSize.Height];
                int x = 0, y = 0;
                while (!read.EndOfStream)
                {
                    line = read.ReadLine();
                    result = line.Split(comma);
                    foreach (string i in result)
                    {
                        int tileX = int.Parse(i) % tileSetSize.Width;
                        int tileY = int.Parse(i) / tileSetSize.Width;
                        map[x, y].X = tileX; map[x, y].Y = tileY;
                        x++;
                    }
                    x = 0; y++;
                }

            }
            graphicsPanel1.Invalidate();
            graphicsPanel2.Invalidate();
        }
    }
}
