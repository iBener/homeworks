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

        private bool blockCommentFlag = false;
        private bool partNameLineFlag = false;
        private int partParantezBaslangic = 0;
        private int parantezler = 0;

        public Sonuc SatirSay(string[] satirlar)
        {
            blockCommentFlag = false;
            partNameLineFlag = false;
            partParantezBaslangic = 0;
            parantezler = 0;

            var sonuc = new Sonuc();
            var partNames = new List<string>();
            foreach (var satir in satirlar)
            {
                var s = CommentVeStringCikar(satir);
                if (String.IsNullOrWhiteSpace(s))
                {
                    continue;
                }

                sonuc.Size += 1;
                ParantezSayYukari(s);

                var partName = GetPartName(s);
                if (!String.IsNullOrEmpty(partName))
                {
                    partNames.Add(partName);
                }

                if (partParantezBaslangic > 0 && partParantezBaslangic + 1 == parantezler)
                {
                    sonuc.ItemCount += 1;
                }

                ParantezSayAsagi(s);
            }

            sonuc.PartName = String.Join(", ", partNames.ToArray());

            return sonuc;
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
                parantezler--;
            }
        }

        private string GetPartName(string satir)
        {
            if (satir.Contains("class") || satir.Contains("interface") || satir.Contains("enum") || partNameLineFlag)
            {
                partParantezBaslangic = parantezler;
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
                if (!String.IsNullOrWhiteSpace(partName))
                {
                    partNameLineFlag = false;
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
