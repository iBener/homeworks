using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IO = System.IO;

namespace LOCCounterNet
{
    class Program
    {
        List<IKaynakKod> KodSayicilar { get; set; }

        public static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("Lütfen kaynak kod dosyasını veya proje klasörünü belirtiniz.");
                return;
            }
            var path = args[0];
            if (IO.File.Exists(path))
            {

            }
            Console.WriteLine(path);
            if (args.Length > 1)
            {
                Console.WriteLine(args[1]);
            }
        }

        protected void KodSayicilariIseAl()
        {

        }

        public void KaynakKodSatirlariSay(string dosyaYolu)
        {
            

        }
    }
}
