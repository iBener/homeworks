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
            SatirYaz("#Program", "Part Name", "#Items", "Size", "Total Size");
            var number = 0;
            var son = 0;
            foreach (var path in args)
            {
                number++;
                var totalSize = 0;
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
                        totalSize += sonuc.Size;
                        SatirYaz((number == son ? "" : number.ToString()), sonuc);
                        son = number;
                    }
                }
                SatirYaz("", "", "", "", totalSize.ToString());
            }
        }

        private void SatirYaz(string number, Sonuc sonuc)
        {
            SatirYaz(number, sonuc.PartName, sonuc.ItemCount.ToString(), sonuc.Size.ToString(), "");
        }

        private void SatirYaz(string number, string partName, string itemCount, string size, string totalSize)
        {
            Console.WriteLine("{0}{1}{2}{3}{4}",
                number.PadRight(10),
                partName.PadRight(20),
                itemCount.PadRight(8),
                size.PadRight(8),
                totalSize);
        }

        /// <summary>
        /// Verilen dosyanın uzantısına göre kaynak kod sayıcı nesnesini verir.
        /// </summary>
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
