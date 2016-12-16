using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace CodeMvvm.View
{
    public class Tile
    {
        public string Name;
        public string Path;
        public Image Image;
        public int PositionX;
        public int PositionY;
        public double RotationAngle;
        public bool TopLeft;
        public bool TopMiddle;
        public bool TopRight;
        public bool RightMiddle;
        public bool BottomRight;
        public bool BottomMiddle;
        public bool BottomLeft;
        public bool LeftMiddle;


        public Tile(string name, int positionX, int positionY) : this(name, "", new Image())
        {
            this.PositionX = positionX;
            this.PositionY = positionY;
        }

        public Tile(string name, string path, Image img)
        {
            this.Name = name;
            this.Path = path;
            this.Image = img;
            PositionX = PositionY = 0;
            RotationAngle = 0;
            TopLeft = TopMiddle = TopRight = RightMiddle = BottomRight = BottomMiddle = BottomLeft = LeftMiddle = false;
        }
    }
}
