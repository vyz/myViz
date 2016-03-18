using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Documents;

namespace pfVisualisator {
    public class vrtVara {
        Vario varo;
        int numbero;
        int makso;

        public vrtVara() { }

        /// <summary>
        /// Модификация от 18 марта 2016 года
        /// Заложен 18 марта 2016 года
        /// </summary>
        /// <param name="pV"></param>
        public vrtVara(Vario pV) {
            varo = pV;
            numbero = 0;
            makso = varo.MovaList.Count - 1;
            }

        /// <summary>
        /// Модификация от 18 марта 2016 года
        /// Заложен 18 марта 2016 года
        /// </summary>
        /// <param name="aa"></param>
        public void ChangeCurrentNumber(int aa) {
            if (aa == 1) {
                numbero += (numbero == makso) ? 0 : 1;
            } else if(aa == -1) {
                numbero -= (numbero == 0) ? 0 : 1;
            } else if (aa == 0) {
                numbero = 0;
            } else if (aa == -2) {
                numbero = makso;
                }
            }

        public Paragraph GetColoredParagraph() {
            Paragraph reto = new Paragraph();
            Span zz = null;
            int numa = varo.Numerok;
            for (int i = 0; i <= makso; i++)
            {
                string ss = null;
                if (i == 0)
                {
                    ss = string.Format("{0}. {1}{2}", numa, (varo.MovaList[0].Koler) ? String.Empty : "... ", varo.Begino);
                }
                else
                {
                    Mova aa = varo.MovaList[i];
                    string doba = string.Empty;
                    if (aa.Koler)
                    {
                        numa++;
                        doba = string.Format("{0}. ", numa);
                    }
                    ss = doba + aa.Shorto;
                }
                if (i == numbero)
                {
                    zz = new Span(new Run(ss));
                    zz.Background = System.Windows.Media.Brushes.DeepSkyBlue;
                }
            }
            return reto;
            }

        public pozo GetCurrentPoza() {
            pozo reto = varo.PozoList[numbero];
            return reto;
            }

        }
    }
