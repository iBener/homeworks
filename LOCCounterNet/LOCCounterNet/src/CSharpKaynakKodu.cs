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

        private bool inBlockComment = false;

        public IEnumerable<Sonuc> SatirSay(string[] satirlar)
        {
            var c = new MyClass();

            var sonuc = new List<Sonuc>();
            var son = new Sonuc();
            var partNameLine = false;
            foreach (var satir in satirlar)
            {
                if (BosVeyaComment(satir))
                {
                    continue;
                }

                son.Size += 1;

                if (satir.Contains("class") && satir.Contains("interface") && satir.Contains("enum") && partNameLine)
                {
                    var partNameStartIndex = 0;
                    if (satir.Contains("class"))
                    {
                        partNameStartIndex = satir.IndexOf("class");
                    }
                    else if (satir.Contains("interface"))
                    {
                        partNameStartIndex = satir.IndexOf("class");
                    }
                    else if (satir.Contains("enum"))
                    {
                        partNameStartIndex = satir.IndexOf("class");
                    }
                    var partName = satir.Substring(partNameStartIndex).Trim();
                    partNameLine = true;
                }
                if (partNameLine)
                {


                }
            }


            return sonuc;
        }

        bool BosVeyaComment(string satir)
        {
            if (String.IsNullOrWhiteSpace(satir))
            {
                return true; // boş satır
            }
            if (satir.Contains("/*"))
            {
                inBlockComment = true;
            }
            if (satir.Contains("*/"))
            {
                inBlockComment = false;
                return false;
            }
            if (inBlockComment)
            {
                return true;
            }
            if (satir.TrimStart().StartsWith("//"))
            {
                return true; // line comment
            }
            return false;
        }
    }
}
