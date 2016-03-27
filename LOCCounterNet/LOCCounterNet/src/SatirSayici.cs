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
                    foreach (var ks in KodSayicilar)
                    {
                        if (UygunKaynakKodu(item, ks))
                        {
                            var lines = IO.File.ReadAllLines(item);
                            var sonuc = ks.SatirSay(lines);
                            sonuclar.Add(sonuc);
                        }
                    }
                }
            }

            Console.WriteLine("{0}\t{1}\t{2}\t{3}\t{4}", "#Program", "Part Name", "#Items", "Size", "Total Size");
            foreach (var sonuc in sonuclar)
            {
                Console.WriteLine("{0}\t{1}\t{2}\t{3}\t{4}", "", sonuc.PartName, sonuc.ItemCount, sonuc.Size, "");
            }
        }

        private bool UygunKaynakKodu(string item, IKaynakKod ks)
        {
            var dosyaUzantisi = IO.Path.GetExtension(item);
            foreach (var uzanti in ks.DosyaFiltresi.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries))
            {
                if (dosyaUzantisi.Equals(uzanti))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
