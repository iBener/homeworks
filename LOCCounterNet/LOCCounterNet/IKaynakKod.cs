using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LOCCounterNet
{
    /// <summary>
    /// Block comment
    /// </summary>
    interface IKaynakKod
    {
        string DosyaFiltresi { get; }

        //line comment
        IEnumerable<Sonuc> SatirSay(string[] satirlar);
    }



    public class MyClass
    {
        public int MyProperty { get; set; }

        void foo()
        {

        }

        string bar()
        {

            return String.Empty;
        }
    }
}
