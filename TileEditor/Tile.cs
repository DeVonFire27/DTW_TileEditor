using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TileEditor
{
    public struct Tile
    {
        int x;

        public int X
        {
            get { return x; }
            set { x = value; }
        }

        int y;

        public int Y
        {
            get { return y; }
            set { y = value; }
        }

    }
}
