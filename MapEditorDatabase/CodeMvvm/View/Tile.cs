using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace CodeMvvm.View
{
    class Tile
    {
        public string name;
        public string path;
        public Image img;

        public Tile(string name, string path, Image img)
        {
            this.name = name;
            this.path = path;
            this.img = img;
        }
    }
}
