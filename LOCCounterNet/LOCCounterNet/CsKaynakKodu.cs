using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LOCCounterNet
{
    class CsKaynakKodu : IKaynakKod
    {

        public string DosyaFiltresi { get { return "cs|csharp"; } }

        public void SatirSay(string dosyaYolu)
        {
            throw new NotImplementedException();
        }
    }
}
