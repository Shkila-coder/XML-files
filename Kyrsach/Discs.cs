using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Kyrsach
{
    public class Discs
    {
        public string Name { get; set; }
        public int FullVolume { get; set; }
        public int Volume { get; set; }

        public string Hidden = "";

        public Discs(string name, int volume, int fullvolume)
        {
            this.Name = name;
            this.FullVolume = fullvolume;
            this.Volume = volume;
        }
        public Discs()
        { }
    }
}
