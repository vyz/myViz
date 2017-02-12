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
    /// Модификация от 12 января 2017 года
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
        int manmakso;
        EnaBo cureb;
        bool nalvara;                //Cигнализирует о безвариантности -> false.
        Gamo gz;
        GamoWinda refa;
        string kupol = string.Empty; //Зверская строка. Отвечает за текущее месторасположение.
                                     //Одновременно, будучи пустой, сигнализирует о безвариантности.
                                     //Отказался от идеи многофункциональности. Ввёл переменную nalvara. 2017-01-17.  
        Dictionary<string, List<string>> DiEntry;
        Dictionary<string, int> DiMakso;
        Dictionary<string, string> DiParento;


        System.Windows.Media.Brush Stado = null;
        System.Windows.Media.SolidColorBrush Videlo = System.Windows.Media.Brushes.DeepSkyBlue;
        System.Windows.Media.SolidColorBrush VaroColorNotActive = System.Windows.Media.Brushes.Chocolate;

        /// <summary>
        /// Модификация от 25 января 2017 года
        /// Заложен 28 апреля 2016 года
        /// </summary>
        /// <param name="plm"></param>
        /// <param name="pgw"></param>
        public vrtGamo(Gamo pgm, GamoWinda pgw) {
            gz = pgm;
            numbero = -1;
            manmakso = makso = pgm.ListoMovo.Count;
            refa = pgw;
            }

        /// <summary>
        /// Модификация от 25 января 2017 года
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
                nalvara = true;
                this.kupol = "manna";
                DiEntry = new Dictionary<string, List<string>>();
                DiMakso = new Dictionary<string, int>();
                DiParento = new Dictionary<string, string>();
                int mvi = 0;
                int mvmaks = gz.ListVaroCom.Count;
                int spvi = 0;
                bool netnachalniycomment = true;
                setoVaroElem = new Dictionary<string, List<vgElem>>();
                System.Windows.FontWeight fonbold = System.Windows.FontWeights.Bold;
                for (int i = 0; i <= makso; i++) {
                    while(mvi < mvmaks && gz.ListVaroCom[mvi].Numa == i) {
                        VarQvant curvar = gz.ListVaroCom[mvi];
                        if(curvar.Commento.Length > 0) {
                            zh = new Run( (i > 0 ? " " : "") + curvar.Commento);
                            if (Stado == null) { Stado = zh.Background; }
                            reto.Inlines.Add(zh);
                            if (netnachalniycomment) { netnachalniycomment = false; }
                            }
                        if(curvar.Varo != null) {
                            spvi++;
                            if(spvi >= 100) { //Кричим об ужасе. Паникуем 
                                throw new GamaException(string.Format("<<vrtVara:GetoParagraph>> spvi превысил 99 --> {0}", spvi.ToString()));
                                }
                            reto.Inlines.Add(VaraInToBigSpan(curvar.Varo, spvi, i, "manna"));
                            this.kupol = "manna";
                            }
                        mvi++;
                        }
                    if (i == 0) {
                        pzcu = gz.ListoPozo[i];
                        elema = new vgElem(pzcu, null);
                        setoElem.Add(elema);
                        }
                    if (i < makso) {
                        pzcu = gz.ListoPozo[i+1];
                        Mova aa = gz.ListoMovo[i];
                        string doba = string.Empty;
                        if (aa.Koler) {
                            numa = pzcu.NumberMove;
                            doba = string.Format("{0}. ", numa);
                            }
                        zh = new Run(((i == 0 && netnachalniycomment) ? "" : " ") + doba);
                        if (Stado == null) { Stado = zh.Background; }
                        zh.FontWeight = fonbold;
                        reto.Inlines.Add(zh);
                        zh = new Run(aa.Shorto);
                        zh.FontWeight = fonbold;
                        zz = new Span(zh);
                        zz.Name = "spi" + (i+1).ToString();
                        zz.MouseLeftButtonDown += new System.Windows.Input.MouseButtonEventHandler(refa.Spanio_MouseLeftButtonDown);
                        reto.Inlines.Add(zz);
                        elema = new vgElem(pzcu, zz);
                        setoElem.Add(elema);
                        }
                    }
                cureb = EnaBo.next | EnaBo.endo;
                if (DiEntry.Count > 0) {
                    cureb |= EnaBo.vara;
                    }
            } else {
                //Упрощенный параграф без вариантов. Отдельные комментарии не считаем. Комментарии без вариантов не рассматриваем.
                cureb = EnaBo.next | EnaBo.endo;
                nalvara = false;

            for (int i = 0; i < makso; i++) {
                if( i == 0 ) {
                    pzcu = gz.ListoPozo[i];
                    elema = new vgElem(pzcu, null);
                    setoElem.Add(elema);
                    }
                pzcu = gz.ListoPozo[i+1];
                Mova aa = gz.ListoMovo[i];
                string doba = string.Empty;
                if (aa.Koler) {
                    numa = pzcu.NumberMove;
                    doba = string.Format("{0}. ", numa);
                    }
                zh = new Run((i == 0 ? "" : " ") + doba);
                if (Stado == null) { Stado = zh.Background; }
                reto.Inlines.Add(zh);
                zz = new Span(new Run(aa.Shorto));
                zz.Name = "spi" + (i+1).ToString();
                zz.MouseLeftButtonDown += new System.Windows.Input.MouseButtonEventHandler(refa.Spanio_MouseLeftButtonDown);
                reto.Inlines.Add(zz);
                elema = new vgElem(pzcu, zz);
                setoElem.Add(elema);
                }
            }
            return reto;
            }
        
        /// <summary>
        /// Модификация от 25 января 2017 года
        /// Заложен 11 ноября 2016 года
        /// </summary>
        /// <param name="pv">Это сам Варио, который надо превратить в кусочек параграфа(спан)</param>
        /// <param name="ima">Порядковый номер варианта на верхнем уровне</param>
        /// <param name="ipo">Переменная цикла по ходопозам. Нужна для заполнения массива навигации для прихода в данный вариант по кнопке входа</param>
        /// <param name="papa">Тег вызвавшего родителя, для будущего возвращения</param>
        /// <returns>Тот самый кусочек параграфа</returns>
        private Span VaraInToBigSpan(Vario pv, int ima, int ipo, string papa) {
            Span reto = new Span();
            Span zz;
            Run zh = null;
            pozo pzcu = null;
            vgElem elema;
            List<vgElem> lelya = new List<vgElem>();
            int iima = 0;
            string simo = string.Empty;
            if(ima > 100 * 10 * 10 * 10 * 10 * 10 * 10 * 10) {//Кричим об ужасе. Паникуем 
                throw new GamaException(string.Format("<<vrtVara:VaraInToBigSpan>> ima превысил 100 * 10 * 10 * 10 *10 * 10 * 10 * 10 --> {0}", ima.ToString()));
            } else if (ima > 100 * 10 * 10 * 10 * 10 * 10 * 10) {
                iima = ima * 10;
                simo = "000000000"; 
            } else if (ima > 100 * 10 * 10 * 10 * 10 * 10) {
                iima = ima * 10;
                simo = "00000000";
            } else if (ima > 100 * 10 * 10 * 10 * 10) {
                iima = ima * 10;
                simo = "0000000";
            } else if (ima > 100 * 10 * 10 * 10) {
                iima = ima * 10;
                simo = "000000";
            } else if (ima > 100 * 10 * 10) {
                iima = ima * 10;
                simo = "00000";
            } else if (ima > 100 * 10) {
                iima = ima * 10;
                simo = "0000";
            } else if (ima > 100) {
                iima = ima * 10;
                simo = "000";
            } else {
                iima = ima * 100;
                simo = "00";
                }
            string sfo = "Vasp" + ima.ToString(simo);
            int idie = 0;
            if( DiEntry.Keys.Contains(this.kupol) ) {
                idie = DiEntry[this.kupol].Count;
            } else {
                DiEntry[this.kupol] = new List<string>();
                }
            for (; idie < ipo; idie++) {
                DiEntry[this.kupol].Add(sfo);
                }
            this.kupol = sfo;
            List<Mova> lvrmo = pv.MovaList;
            List<VarQvant> lnest = pv.VaroCommoList;
            int imx = lvrmo.Count;
            DiMakso.Add(sfo, imx);
            DiParento.Add(sfo, papa);
            int mvi = 0;
            int mvmaks = (lnest == null) ? 0 : lnest.Count;
            int spvi = 0;
            int numa = 0;
            bool netnachalniycomment = true;
            zh = new Run(" (");
            zh.Foreground = VaroColorNotActive;
            reto.Inlines.Add(zh);
            for (int i = 0; i < imx; i++) {
                while (mvi < mvmaks && lnest[mvi].Numa == i) {
                    VarQvant curvar = lnest[mvi];
                    if (curvar.Commento.Length > 0) {
                        zh = new Run((i > 0 ? " " : "") + curvar.Commento);
                        zh.Foreground = VaroColorNotActive;
                        reto.Inlines.Add(zh);
                        if( netnachalniycomment ) { netnachalniycomment = false; }
                        }
                    if (curvar.Varo != null) {
                        spvi++;
                        if (spvi >= 10) { //Кричим об ужасе. Паникуем 
                            throw new GamaException(string.Format("<<vrtVara:VaraInToBigSpan>> внутренний spvi превысил 9 --> {0}", spvi.ToString()));
                            }
                        reto.Inlines.Add(VaraInToBigSpan(curvar.Varo, iima + spvi, i, sfo));
                        this.kupol = sfo;
                        }
                    mvi++;
                    }
                if (i == 0) {
                    elema = new vgElem(pv.BegoPo, null);
                    lelya.Add(elema);
                    }
                pzcu = pv.PozoList[i];
                Mova aa = lvrmo[i];
                string doba = string.Empty;
                if (aa.Koler) {
                    numa = pzcu.NumberMove;
                    doba = string.Format("{0}. ", numa);
                    }
                zh = new Run(((i == 0 && netnachalniycomment) ? "" : " ") + doba);
                zh.Foreground = VaroColorNotActive;
                reto.Inlines.Add(zh);
                zh = new Run(aa.Shorto);
                zz = new Span(zh);
                zz.Name =  sfo + "_" + i.ToString();
                zz.Foreground = VaroColorNotActive;
                zz.MouseLeftButtonDown += new System.Windows.Input.MouseButtonEventHandler(refa.Spanio_MouseLeftButtonDown);
                reto.Inlines.Add(zz);
                elema = new vgElem(pzcu, zz);
                lelya.Add(elema);
                }
            setoVaroElem.Add(sfo,lelya);
            zh = new Run(")");
            zh.Foreground = VaroColorNotActive;
            reto.Inlines.Add(zh);
            return reto;
            }

        /// <summary>
        /// Модификация от 9 февраля 2017 года
        /// Заложен в апреле 2016 года
        /// </summary>
        /// <param name="delto"></param>
        public void ChangeCurrentNumber(int delto) {
            if( delto < -1 ) {
                if( delto == -25 ) { //Выход из варианта
                    ChangeExitFromVariant();
                    return;
                    }
                if (delto == -48) { //Вход вовнутрь варианта
                    ChangeEntranceToVariant();
                    return;
                    }
                if (delto == -52) { //Переход в начало то ли партии, то ли варианта
                    ChangeCurrentNumber(0, this.kupol);
                    return;
                    }
                if (delto == -57) { //Переход в хвост то ли партии, то ли варианта
                    ChangeCurrentNumber(delto, this.kupol);
                    return;
                    }
                }
            int nova = (delto > 1000) ? delto - 1000 : numbero + delto;
            if (kupol.StartsWith("Vasp")) {
                ChangeCurrentNumber(nova, kupol);
                return;
                }
            if (nova >= 0 && nova <= makso) {
                if (setoElem[numbero].Spano != null) {
                    setoElem[numbero].Spano.Background = Stado;
                    }
                numbero = nova;
                if (setoElem[numbero].Spano != null) {
                    setoElem[numbero].Spano.Background = Videlo;
                    }
                }
            cureb = EnaBo.nona;
            if(numbero > 0) {
                cureb |= (EnaBo.prev | EnaBo.bego);
                }
            if(numbero < makso) {
                cureb |= (EnaBo.next | EnaBo.endo);
                }
            if( nalvara ) {
                if (DiEntry.Keys.Contains(kupol)) {
                    if (numbero < DiEntry[kupol].Count) {
                        cureb |= EnaBo.vara;
                        }
                    }
                }
            if (kupol.StartsWith("Vasp")) {
                cureb |= EnaBo.exit;
                }
            }

        /// <summary>
        /// Модификация от 9 февраля 2017 года
        /// Заложен 13 января 2017 года
        /// </summary>
        /// <param name="delto"></param>
        /// <param name="kuvar"></param>
        public void ChangeCurrentNumber(int delto, string kuvar) {
            makso = kuvar == "manna" ? manmakso : DiMakso[kuvar];
            int nova = delto != -57 ? delto : makso;
            if (nova >= 0 && nova <= makso) {
                if (this.kupol == "manna") {
                    if (setoElem[numbero].Spano != null) {
                        setoElem[numbero].Spano.Background = Stado;
                        }
                } else {
                    if (setoVaroElem[this.kupol][numbero].Spano != null) {
                        setoVaroElem[this.kupol][numbero].Spano.Foreground = VaroColorNotActive;
                        setoVaroElem[this.kupol][numbero].Spano.Background = Stado;
                        }
                    }
                numbero = nova;
                this.kupol = kuvar;
                if (this.kupol == "manna") {
                    if (setoElem[numbero].Spano != null) {
                        setoElem[numbero].Spano.Background = Videlo;
                        }
                } else {
                    if (setoVaroElem[this.kupol][numbero].Spano != null) {
                        setoVaroElem[this.kupol][numbero].Spano.Background = Videlo;
                        }
                    }
                }
            cureb = EnaBo.nona;
            if (numbero > 0) {
                cureb |= (EnaBo.prev | EnaBo.bego);
                }
            if (numbero < makso) {
                cureb |= (EnaBo.next | EnaBo.endo);
                }
            if (DiEntry.Keys.Contains(kupol)) {
                if(numbero < DiEntry[kupol].Count) {
                    cureb |= EnaBo.vara;
                    }
                }
            if (kupol.StartsWith("Vasp")) {
                cureb |= EnaBo.exit;
                }
            }

        /// <summary>
        /// Модификация от 9 февраля 2017 года
        /// Заложен 25 января 2017 года
        /// </summary>
        private void ChangeExitFromVariant() {
            if (DiParento.Keys.Contains(this.kupol)) {
                string papa = DiParento[this.kupol];
                List<string> lisa = DiEntry[papa];
                int i = lisa.Count - 1;
                for(; i > 0; i--) {
                    if( lisa[i] == this.kupol ) {
                        break;
                        }
                    }
                ChangeCurrentNumber(i, papa);
                }
            }

        /// <summary>
        /// Модификация от 9 февраля 2017 года
        /// Заложен 1 февраля 2017 года
        /// </summary>
        private void ChangeEntranceToVariant() {
            string whera = DiEntry[kupol][numbero];
            ChangeCurrentNumber(1, whera);
            }

        /// <summary>
        /// Модификация от 12 февраля 2017 года
        /// Заложен в 2016 году
        /// </summary>
        /// <returns></returns>
        public pozo GetCurrentPoza() {
            pozo reto = null;
            if (this.VaraExist) {
                reto = (this.kupol == "manna") ? setoElem[numbero].Pizza : setoVaroElem[this.kupol][numbero].Pizza;
            } else {
                reto = setoElem[numbero].Pizza;
                }
            return reto;
            }

        public EnaBo Knopki { get { return cureb; } }
        public bool VaraExist { get { return nalvara; } }

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
