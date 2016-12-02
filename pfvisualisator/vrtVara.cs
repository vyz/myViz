using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Documents;
using System.Windows.Input;

namespace pfVisualisator {
#region--------------------------------ПЕРВЫЙ КЛАСС--vrtVara------------------------
    public class vrtVara {
        Vario varo;
        int numbero;
        int makso;
        PozoWinda refa;

        public vrtVara() { }

        /// <summary>
        /// Модификация от 22 марта 2016 года
        /// Заложен 18 марта 2016 года
        /// </summary>
        /// <param name="pV"></param>
        /// <param name="pW">Окошко</param>
        public vrtVara(Vario pV, PozoWinda pW)
        {
            varo = pV;
            numbero = 0;
            makso = varo.MovaList.Count - 1;
            refa = pW;
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

        /// <summary>
        /// Модификация от 22 марта 2016 года
        /// Заложен 22 марта 2016 года
        /// </summary>
        /// <param name="pn"></param>
        public void SetCurrentNumber(int pn) {
            if (0 <= pn && pn <= makso) {
                numbero = pn;
                }
            }

        /// <summary>
        /// Модификация от 22 марта 2016 года
        /// Заложен 18 марта 2016 года
        /// </summary>
        /// <returns></returns>
        public Paragraph GetColoredParagraph() {
            Paragraph reto = new Paragraph();
            Span zz = null;
            int numa = varo.Numerok;
            for (int i = 0; i <= makso; i++) {
                string ss = null;
                if (i == 0) {
                    ss = string.Format("{0}. {1}{2}", numa, (varo.MovaList[0].Koler) ? String.Empty : "... ", varo.Begino);
                } else {
                    Mova aa = varo.MovaList[i];
                    string doba = string.Empty;
                    if (aa.Koler) {
                        numa++;
                        doba = string.Format("{0}. ", numa);
                        }
                    ss = doba + aa.Shorto;
                    }
                if (i == numbero) {
                    if (i != 0) {
                        zz = new Span(new Run(" "));
                        zz.Name = "Probel";
                        reto.Inlines.Add(zz);
                        }
                    zz = new Span(new Run(ss));
                    zz.Background = System.Windows.Media.Brushes.DeepSkyBlue;
                    zz.Name = "spi" + i.ToString();
                    zz.MouseLeftButtonDown += new System.Windows.Input.MouseButtonEventHandler(refa.Spanio_MouseLeftButtonDown);
                    reto.Inlines.Add(zz);
                } else {
                    zz = new Span(new Run(" " + ss));
                    zz.Name = "spi" + i.ToString();
                    zz.MouseLeftButtonDown += new System.Windows.Input.MouseButtonEventHandler(refa.Spanio_MouseLeftButtonDown);
                    reto.Inlines.Add(zz);
                    }
                }
            return reto;
            }

        /// <summary>
        /// Модификация от 18 марта 2016 года
        /// Заложен 18 марта 2016 года
        /// </summary>
        /// <returns></returns>
        public pozo GetCurrentPoza() {
            pozo reto = varo.PozoList[numbero];
            return reto;
            }

        }
#endregion-----------------------------ПЕРВЫЙ КЛАСС--vrtVara------------------------

#region--------------------------------ДРУГОЙ КЛАСС--vrtGamo------------------------
    /// <summary>
    /// Модификация от 25 апреля 2016 года
    /// Заложен 25 апреля 2016 года
    /// 6 кнопок: также надо посадить на клавиши
    /// В начало; В конец; Вперед; Назад; В ближайший вариант вперед; Выход назад из варианта на один уровень.
    /// Home; End; Right; Left; Down; Up
    /// </summary>
    public class vrtGamo {
        List<vgElem> setoElem;
        Dictionary <string, List<vgElem>> setoVaroElem;
        int numbero;
        int makso;
        EnaBo cureb;
        Gamo gz;
        GamoWinda refa;
        System.Windows.Media.Brush Stado = null;
        System.Windows.Media.SolidColorBrush Videlo = System.Windows.Media.Brushes.DeepSkyBlue;
        System.Windows.Media.SolidColorBrush VaroColorNotActive = System.Windows.Media.Brushes.Chocolate;

        /// <summary>
        /// Модификация от 28 апреля 2016 года
        /// Заложен 28 апреля 2016 года
        /// </summary>
        /// <param name="plm"></param>
        /// <param name="pgw"></param>
        public vrtGamo(Gamo pgm, GamoWinda pgw) {
            gz = pgm;
            numbero = -1;
            makso = pgm.ListoMovo.Count;
            refa = pgw;
            }

        /// <summary>
        /// Модификация от 29 апреля 2016 года
        /// Заложен 28 апреля 2016 года
        /// </summary>
        /// <returns></returns>
        public Paragraph GetoParagraph() {
            Paragraph reto = new Paragraph();

            Span zz = null;
            Run zh = null;
            vgElem elema;
            pozo pzcu = null;

            numbero = 0;
            setoElem = new List<vgElem>(makso + 1);
            int numa = 0;

            if (gz.VariantoFlag) {
                //Тогда другой путь
                int mvi = 0;
                int mvmaks = gz.ListVaroCom.Count;
                int spvi = 0;
                setoVaroElem = new Dictionary<string, List<vgElem>>();
                cureb = EnaBo.next | EnaBo.prev;
                System.Windows.FontWeight fonbold = System.Windows.FontWeights.Bold;
                for (int i = 0; i <= makso; i++) {
                    while(mvi < mvmaks && gz.ListVaroCom[mvi].Numa == i) {
                        VarQvant curvar = gz.ListVaroCom[mvi];
                        if(curvar.Commento.Length > 0) {
                            zh = new Run( (i > 0 ? " " : "") + curvar.Commento);
                            if (Stado == null) { Stado = zh.Background; }
                            reto.Inlines.Add(zh);
                            }
                        if(curvar.Varo != null) {
                            spvi++;
                            if(spvi >= 100) { //Кричим об ужасе. Паникуем 
                                throw new GamaException(string.Format("<<vrtVara:GetoParagraph>> spvi превысил 99 --> {0}", spvi.ToString()));
                                }
                            reto.Inlines.Add(VaraInToBigSpan(curvar.Varo, spvi));
                            }
                        mvi++;
                        }
                    pzcu = gz.ListoPozo[i];
                    if (i == 0) {
                        elema = new vgElem(pzcu, null);
                        setoElem.Add(elema);
                        continue;
                        }
                    Mova aa = gz.ListoMovo[i - 1];
                    string doba = string.Empty;
                    if (aa.Koler) {
                        numa = pzcu.NumberMove;
                        doba = string.Format("{0}. ", numa);
                        }
                    zh = new Run((i == 0 ? "" : " ") + doba);
                    if (Stado == null) { Stado = zh.Background; }
                    zh.FontWeight = fonbold;
                    reto.Inlines.Add(zh);
                    zh = new Run(aa.Shorto);
                    zh.FontWeight = fonbold;
                    zz = new Span(zh);
                    zz.Name = "spi" + i.ToString();
                    zz.MouseLeftButtonDown += new System.Windows.Input.MouseButtonEventHandler(refa.Spanio_MouseLeftButtonDown);
                    reto.Inlines.Add(zz);
                    elema = new vgElem(pzcu, zz);
                    setoElem.Add(elema);
                    }
            } else {
                //Упрощенный параграф без вариантов. Отдельные комментарии не считаем. Комментарии без вариантов не рассматриваем.
                cureb = EnaBo.next | EnaBo.endo;

            for (int i = 0; i <= makso; i++) {
                pzcu = gz.ListoPozo[i];
                if( i == 0 ) {
                    elema = new vgElem(pzcu, null);
                    setoElem.Add(elema);
                    continue;
                    }
                Mova aa = gz.ListoMovo[i - 1];
                string doba = string.Empty;
                if (aa.Koler) {
                    numa = pzcu.NumberMove;
                    doba = string.Format("{0}. ", numa);
                    }
                zh = new Run((i == 0 ? "" : " ") + doba);
                if (Stado == null) { Stado = zh.Background; }
                reto.Inlines.Add(zh);
                zz = new Span(new Run(aa.Shorto));
                zz.Name = "spi" + i.ToString();
                zz.MouseLeftButtonDown += new System.Windows.Input.MouseButtonEventHandler(refa.Spanio_MouseLeftButtonDown);
                reto.Inlines.Add(zz);
                elema = new vgElem(pzcu, zz);
                setoElem.Add(elema);
                }
            }
            return reto;
            }
        
        /// <summary>
        /// Модификация от 11 ноября 2016 года
        /// Заложен 11 ноября 2016 года
        /// </summary>
        /// <param name="pv">Это сам Варио, который надо превратить в кусочек параграфа(спан)</param>
        /// <param name="ima">Порядковый номер варианта на верхнем уровне</param>
        /// <returns>Тот самый кусочек параграфа</returns>
        private Span VaraInToBigSpan(Vario pv, int ima) {
            Span reto = new Span();
            Span zz;
            Run zh = null;
            pozo pzcu = null;
            vgElem elema;
            List<vgElem> lelya = new List<vgElem>();
            string sfo = "Vasp" + ima.ToString("00") + '_';
            List<Mova> lvrmo = pv.MovaList;
            List<VarQvant> lnest = pv.VaroCommoList;
            int imx = lvrmo.Count;
            int mvi = 0;
            int mvmaks = lnest.Count;
            int spvi = 0;
            int numa = 0;
            for (int i = 0; i < imx; i++) {
                while (mvi < mvmaks && lnest[mvi].Numa == i) {
                    VarQvant curvar = lnest[mvi];
                    if (curvar.Commento.Length > 0) {
                        zh = new Run((i > 0 ? " " : "") + curvar.Commento);
                        if (Stado == null) { Stado = zh.Background; }
                        zh.Foreground = VaroColorNotActive;
                        reto.Inlines.Add(zh);
                        }
                    if (curvar.Varo != null) {
                        spvi++;
                        if (spvi >= 100) { //Кричим об ужасе. Паникуем 
                            throw new GamaException(string.Format("<<vrtVara:VaraInToBigSpan>> spvi превысил 99 --> {0}", spvi.ToString()));
                            }
                        reto.Inlines.Add(VaraInToBigSpan(curvar.Varo, spvi));
                        }
                    mvi++;
                    }
                if (i == 0) {
                    elema = new vgElem(pv.BegoPo, null);
                    lelya.Add(elema);
                    continue;
                    }

                pzcu = pv.PozoList[i];
                Mova aa = lvrmo[i];
                string doba = string.Empty;
                if (aa.Koler)
                {
                    numa = pzcu.NumberMove;
                    doba = string.Format("{0}. ", numa);
                }
                zh = new Run((i == 0 ? "" : " ") + doba);
                zh.Foreground = VaroColorNotActive;
                reto.Inlines.Add(zh);
                zh = new Run(aa.Shorto);
                zz = new Span(zh);
                zz.Name =  sfo + i.ToString();
                zz.Foreground = VaroColorNotActive;
                zz.MouseLeftButtonDown += new System.Windows.Input.MouseButtonEventHandler(refa.Spanio_MouseLeftButtonDown);
                reto.Inlines.Add(zz);
                elema = new vgElem(pzcu, zz);
                lelya.Add(elema);
            }
            setoVaroElem.Add(sfo,lelya);
            return reto;
            }

        public void ChangeCurrentNumber(int delto) {
            int nova = (delto > 1000) ? delto - 1000 : numbero + delto;
            if (nova >= 0 && nova <= makso) {
                if (setoElem[numbero].Spano != null) {
                    setoElem[numbero].Spano.Background = Stado;
                    }
                numbero = nova;
                if (setoElem[numbero].Spano != null) {
                    setoElem[numbero].Spano.Background = Videlo;
                    }
                }
            }

        public pozo GetCurrentPoza() {
            pozo reto = setoElem[numbero].Pizza;
            return reto;
            }

        public EnaBo Knopki { get { return cureb; } }

        private class vgElem {
            pozo afterpos;
            Span spano;


            public vgElem() { }

            public vgElem(pozo pp, Span psp) {
                afterpos = pp;
                spano = psp;
                }

            public Span Spano { get { return spano; } }
            public pozo Pizza { get { return afterpos; } }
            }

        }
#endregion-----------------------------ДРУГОЙ КЛАСС--vrtGamo------------------------

    public enum EnaBo : byte {
        nona = 0,
        bego = 1,
        endo = 2,
        prev = 4,
        next = 8,
        vara = 16,
        exit = 32,
        }

    }
