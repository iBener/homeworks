using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LOCCounterNet
{
    /// <summary>
    /// Bu interface'i kullanarak farklı dillerdeki kaynak kodlarının sayımı da sağlanabilir.
    /// </summary>
    interface IKaynakKod
    {
        /// <summary>
        /// Sayılacak kaynak kodu uzantısı. "|" karakteri ile birden fazla dosya uzantısı tanımlanabilir.
        /// </summary>
        string DosyaFiltresi { get; }

        /// <summary>
        /// Belirlenen dosya uzantısına uyan dosyanın sayılması için bu metod çağırılır.
        /// </summary>
        Sonuc SatirSay(string[] satirlar);
    }
}
