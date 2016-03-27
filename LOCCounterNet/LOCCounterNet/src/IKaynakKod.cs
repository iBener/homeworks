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

        IEnumerable<Sonuc> SatirSay(string[] satirlar);
    }



    public class 
        MyClass
    {
        int a = 0; /*
        block comment
        aaa */ public int b = 1;
        public int MyProperty { get; set; }

        //line comment
        void foo()
        {

        }

        //line comment
        string bar()
        {

            return String.Empty;
        }
    }
}
