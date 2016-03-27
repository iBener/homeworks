using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IO = System.IO;

namespace LOCCounterNet
{
    class SatirSayici
    {
        List<IKaynakKod> KodSayicilar { get; set; }

        public bool Olustur()
        {
            try
            {
                KodSayicilar = new List<IKaynakKod>();

                var type = typeof(IKaynakKod);
                var types = AppDomain.CurrentDomain.GetAssemblies()
                    .SelectMany(s => s.GetTypes())
                    .Where(p => type.IsAssignableFrom(p));

                foreach (var sayici in types)
                {
                    if (!sayici.IsInterface)
                    {
                        var ks = Activator.CreateInstance(sayici) as IKaynakKod;
                        if (ks != null)
                        {
                            KodSayicilar.Add(ks);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }
            return true;
        }

        public void Say(string[] args)
        {
            var sonuclar = new List<Sonuc>();
            foreach (var path in args)
            {
                var p = new Proje(path);
                foreach (var item in p.Dosyalar)
                {
                    var ks = KaynakKodSayiciBul(item);
                    if (ks != null)
                    {
                        var lines = IO.File.ReadAllLines(item);
                        var sonuc = ks.SatirSay(lines);
                        if (String.IsNullOrWhiteSpace(sonuc.PartName))
                        {
                            sonuc.PartName = IO.Path.GetFileName(item);
                        }
                        sonuclar.Add(sonuc);
                    }
                }
            }

            Console.WriteLine("{0}{1}{2}{3}{4}",
                "#Program".PadRight(10),
                "Part Name".PadRight(20),
                "#Items".PadRight(8),
                "Size".PadRight(8),
                "Total Size");
            var number = "1";
            var totalSize = 0;
            foreach (var sonuc in sonuclar)
            {
                if (sonuc.Size != 0)
                {
                    Console.WriteLine("{0}{1}{2}{3}{4}",
                        number.PadRight(10),
                        sonuc.PartName.PadRight(20),
                        sonuc.ItemCount.ToString().PadRight(8),
                        sonuc.Size.ToString().PadRight(8), "");
                    number = "";
                    totalSize += sonuc.Size;
                }
            }
            Console.WriteLine("{0}{1}{2}{3}{4}",
                "".PadRight(10),
                "".PadRight(20),
                "".PadRight(8),
                "".PadRight(8),
                totalSize.ToString());
        }

        private IKaynakKod KaynakKodSayiciBul(string item)
        {
            var dosyaUzantisi = IO.Path.GetExtension(item);
            foreach (var ks in KodSayicilar)
            {
                foreach (var uzanti in ks.DosyaFiltresi.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    if (dosyaUzantisi.Equals(uzanti))
                    {
                        return ks;
                    }
                }
            }
            return null;
        }
    }
}
