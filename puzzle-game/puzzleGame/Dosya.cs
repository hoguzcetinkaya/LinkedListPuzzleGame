using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace puzzleGame
{
    internal class Dosya
    {
        public string DosyaYolu;
        public Dosya()
        {
            string projeKlasoru = Directory.GetParent(Application.StartupPath).FullName;
            DosyaYolu = Path.Combine(projeKlasoru, "enyuksekskor.txt");
        }
    }
}
