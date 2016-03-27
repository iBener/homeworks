using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LOCCounterNet
{
    class CSharpKaynakKodu : IKaynakKod
    {

        public string DosyaFiltresi { get { return ".cs|.csharp"; } }

        public IEnumerable<Sonuc> SatirSay(string[] satirlar)
        {
            var sonuc = new List<Sonuc>();

            foreach (var satir in satirlar)
            {

            }


            return sonuc;
        }
    }
}
