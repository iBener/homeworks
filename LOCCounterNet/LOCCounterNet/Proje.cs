using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IO = System.IO;

namespace LOCCounterNet
{
    class Proje
    {
        private string _anaPath;
        private List<string> _dosyalar;

        public Proje(string path)
        {
            _anaPath = path;
            _dosyalar = new List<string>();
            DosyalariOku(_anaPath);
        }

        private void DosyalariOku(string path)
        {
            if (IO.File.Exists(path))
            {
                _dosyalar.Add(path);
                return;
            }
            if (IO.Directory.Exists(path))
            {
                foreach (var item in IO.Directory.GetFiles(path))
                {
                    _dosyalar.Add(item);
                }
                foreach (var item in IO.Directory.GetDirectories(path))
                {
                    DosyalariOku(item);
                }
            }
        }

        public IEnumerable<string> Dosyalar
        {
            get
            {
                foreach (var item in _dosyalar)
                {
                    yield return item;
                }
            }
        }

    }
}
