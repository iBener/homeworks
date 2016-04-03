using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LOCCounterNet
{
    /// <summary>
    /// C# kaynak kodu satır sayıcı nesnesi.
    /// </summary>
    class CSharpKaynakKodu : IKaynakKod
    {
        /// <summary>
        /// C# kaynak kodu dosya uzantısı. İkinci uzantı sadece deneme için verildi.
        /// </summary>
        public string DosyaFiltresi { get { return ".cs|.csharp"; } }

        private bool blockCommentFlag;
        private bool partNameLineFlag;
        List<string> partNames;
        Stack<int> partBaslangicParantez;
        private int parantezler;
        private int itemCount;

        /// <summary>
        /// C# satır sayma metodu
        /// </summary>
        public Sonuc SatirSay(string[] satirlar)
        {
            //değişkenleri hazırla
            blockCommentFlag = false;
            partNameLineFlag = false;
            partBaslangicParantez = new Stack<int>();
            parantezler = 0;
            partNames = new List<string>();
            itemCount = 0;

            //debug larken izleme için kullanılan değişken
            int i = -1;

            //sayma döngüsü
            var sonuc = new Sonuc();
            foreach (var satir in satirlar)
            {
                i++;
                var s = CommentVeStringCikar(satir);
                if (String.IsNullOrWhiteSpace(s))
                {
                    continue;
                }

                sonuc.Size += 1;

                ParantezSayYukari(s);

                PartNameSay(s);

                ItemSay(s);

                ParantezSayAsagi(s);
            }

            //sonuçlar
            sonuc.PartName = String.Join(", ", partNames.ToArray());
            sonuc.ItemCount = itemCount;

            return sonuc;
        }

        private void ItemSay(string s)
        {
            if (partBaslangicParantez.Count > 0)
            {
                var sonPartBaslangic = partBaslangicParantez.Peek();
                if (s.Contains("{") && parantezler - 1 == sonPartBaslangic)
                {
                    itemCount++;
                }
            }
        }

        private void ParantezSayYukari(string s)
        {
            if (s.Contains("{"))
            {
                parantezler++;
            }
        }

        private void ParantezSayAsagi(string s)
        {
            if (s.Contains("}"))
            {
                if (partBaslangicParantez.Count > 0)
                {
                    var sonPartBaslangic = partBaslangicParantez.Peek();
                    if (parantezler == sonPartBaslangic)
                    {
                        partBaslangicParantez.Pop();
                    }
                }
                parantezler--;
            }
        }

        /// <summary>
        /// Part'ların (class, interface, enum) sayılması
        /// </summary>
        private string PartNameSay(string satir)
        {
            if (satir.Contains("class") || satir.Contains("interface") || satir.Contains("enum") || partNameLineFlag)
            {
                var partNameStartIndex = 0;
                if (satir.Contains("class"))
                {
                    partNameStartIndex = satir.IndexOf("class") + "class".Length;
                }
                else if (satir.Contains("interface"))
                {
                    partNameStartIndex = satir.IndexOf("interface") + "interface".Length;
                }
                else if (satir.Contains("enum"))
                {
                    partNameStartIndex = satir.IndexOf("enum") + "enum".Length;
                }
                var partName = satir.Substring(partNameStartIndex).Trim();
                if (partName.Contains(" "))
                {
                    partName = partName.Substring(0, partName.IndexOf(" "));
                }
                if (partName.Contains(":"))
                {
                    partName = partName.Substring(0, partName.IndexOf(":"));
                }
                if (!String.IsNullOrWhiteSpace(partName))
                {
                    //part ismini buldum
                    partNameLineFlag = false;
                    partBaslangicParantez.Push(parantezler + (satir.Contains("{") ? 0 : 1));
                    partNames.Add(partName);
                    return partName;
                }
                //part ismi sonraki satırda
                partNameLineFlag = true;
            }
            //part yok
            return String.Empty;
        }

        /// <summary>
        /// Verilen satırdaki "comment" ve "string literal"lerin çıkarılmasını sağlar.
        /// </summary>
        string CommentVeStringCikar(string satir)
        {
            var s = StringCikar(satir);
            if (blockCommentFlag)
            {
                if (s.Contains("*/"))
                {
                    var blokBitis = s.IndexOf("*/");
                    s = s.Substring(blokBitis + 2);
                    blockCommentFlag = false;
                }
                else
                {
                    return String.Empty;
                }
            }
            if (s.Contains("/*"))
            {
                var blokBaslangic = s.IndexOf("/*");
                var blokBitis = s.IndexOf("*/", blokBaslangic + 1);
                if (blokBitis >= blokBaslangic)
                {
                    s = s.Remove(blokBaslangic, blokBitis - blokBaslangic + 2);
                }
                else
                {
                    s = s.Remove(blokBaslangic);
                    blockCommentFlag = true;
                }
            }
            if (s.Contains("//"))
            {
                var commentBaslangic = s.IndexOf("//");
                s = s.Substring(0, commentBaslangic);
            }
            return s;
        }

        /// <summary>
        /// Verilen satırdaki "string literal"lerin çıkarılmasını sağlar.
        /// </summary>
        private string StringCikar(string satir)
        {
            var s = satir.Replace("\\\"", "");
            var i = s.IndexOf("\"");
            while (i >= 0)
            {
                var j = s.IndexOf("\"", i + 1);
                s = s.Remove(i, j - i + 1);
                i = s.IndexOf("\"");
            }
            return s;
        }
    }
}
