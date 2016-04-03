using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LOCCounterNet
{
    class CSharpKaynakKodu : IKaynakKod
    {

        public string DosyaFiltresi { get { return ".cs|.csharp"; } }

        private bool blockCommentFlag;
        private bool partNameLineFlag;
        List<string> partNames;
        Stack<int> partBaslangicParantez;
        private int parantezler;
        private int itemCount;

        public Sonuc SatirSay(string[] satirlar)
        {
            blockCommentFlag = false;
            partNameLineFlag = false;
            partBaslangicParantez = new Stack<int>();
            parantezler = 0;
            partNames = new List<string>();
            itemCount = 0;

            int i = -1;

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
                    partNameLineFlag = false;
                    partBaslangicParantez.Push(parantezler + (satir.Contains("{") ? 0 : 1));
                    partNames.Add(partName);
                    return partName;
                }
                partNameLineFlag = true;
            }
            return String.Empty;
        }

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
