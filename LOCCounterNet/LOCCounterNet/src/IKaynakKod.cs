using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LOCCounterNet
{
    /// <summary>
    /// 
    /// </summary>
    interface IKaynakKod
    {
        string DosyaFiltresi { get; }

        Sonuc SatirSay(string[] satirlar);
    }
}
