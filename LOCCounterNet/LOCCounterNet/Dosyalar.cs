using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IO = System.IO;

namespace LOCCounterNet
{
    class Dosyalar
    {
        private string _anaPath;

        public Dosyalar(string path)
        {
            _anaPath = path;
        }

        public IEnumerable<string> DosyaListesi()
        {
            if (IO.File.Exists(_anaPath))
            {
                return null;
            }

            return null;
        }
    }
}
