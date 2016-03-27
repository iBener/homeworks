using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LOCCounterNet
{
    class Program
    {

        public static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("Lütfen kaynak kod dosyasını veya proje klasörünü belirtiniz.");
                return;
            }

            var ss = new SatirSayici();
            if (ss.Olustur())
            {
                ss.Say(args);
            }
            
            Console.ReadKey();
        }
    }
}
