﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;


namespace OnlyWorko {
#region--------------------------------ПЕРВИЧНЫЙ КЛАСС--Gamo------------------------
    public class Gamo {
        private List<string> pgatr;
        private List<string> pgmova;
        private List<string> onlymainmoves;
        private bool flagComments;
        private bool flagTiming;
        private bool flagVario;
        private bool flagFromStart;
        private bool flagEndResulte;
        private bool flagworkabigcomment; //Комментарий на несколько строк
        private bool flagworkabigtimo;  //Таймовый коммент разбит концом строки (WorldCup 2015)
        private bool flagworkabigVaro;
        private bool flagImpossibleMove;
        private string strimpossiblemove;
        private string strResulte;
        private string bigCommento;
        private string bigVaro;
        private int stackBigVaro;
        private List<Mova> lFactMoves;
        private List<pozo> lPozos;
        private List<gTimo> lTimos;
        private List<VarQvant> lVarQva;
        private gmKorrAtr intellectoattr;
        private int curpozoindex;
        private pozo curpoza;
        private Dictionary<gmAttro, string> datro;

        public Gamo() { }

        public Gamo(List<string> attro, List<string> mova) {
            pgatr = attro;
            pgmova = mova;
            flagComments = flagEndResulte = flagFromStart = false;
            }

        /// <summary>
        /// Модификация от 14 апреля 2016 года
        /// Заложен 8 ноября 2015 года
        /// </summary>
        /// <param name="vg"></param>
        public Gamo(vGamo vg) {
            datro = new Dictionary<gmAttro, string>();
            if (vg.Fen == string.Empty) { flagFromStart = true; }
            else { SetAttro(gmAttro.Fen, vg.Fen); }
            if (vg.Result.Length > 0) {
                strResulte = vg.Result;
                SetAttro(gmAttro.Result, strResulte);
                flagEndResulte = true;
                }
            if (vg.Event.Length > 0) { SetAttro(gmAttro.Event, vg.Event); }
            if (vg.Site.Length > 0) { SetAttro(gmAttro.Site, vg.Site); }
            if (vg.Date.Length > 0) { SetAttro(gmAttro.Date, vg.Date); }
            if (vg.Round.Length > 0) { SetAttro(gmAttro.Round, vg.Round); }
            if (vg.White.Length > 0) { SetAttro(gmAttro.White, vg.White); }
            if (vg.Black.Length > 0) { SetAttro(gmAttro.Black, vg.Black); }
            if (vg.ECO.Length > 0) { SetAttro(gmAttro.ECO, vg.ECO); }
            if (vg.WElo.Length > 0) { SetAttro(gmAttro.WhiteElo, vg.WElo); }
            if (vg.BElo.Length > 0) { SetAttro(gmAttro.BlackElo, vg.BElo); }
            if (vg.PlyCount.Length > 0) { SetAttro(gmAttro.PlyCount, vg.PlyCount); }
            FillMovesAndPozos(vg.OnlyMova);
            if (vg.AddAtr.Length > 0) {
                FillAddingAttribute(vg.AddAtr);
                }
            if (vg.Timingo.Length > 0) {
                FillListTimo(vg.Timingo);
                }
            if (vg.ListoVarCom != null) {
                lVarQva = vg.ListoVarCom;
                foreach (VarQvant aa in lVarQva) {
                    if (flagVario && flagComments) {
                        break;
                    } else {
                        if (!flagVario) {
                            if (aa.Varo != null) {
                                flagVario = true;
                                }
                            }
                        if (!flagComments) {
                            if (aa.Commento.Length > 0) {
                                flagComments = true;
                                }
                            }
                        }
                    }
                }
            }

        /// <summary>
        /// Модификация от 3 января 2017 года
        /// Заложен июнь 2015 года
        /// </summary>
        /// <returns></returns>
        public bool MovaControlling() {
            bool reto = false;
            pozo tp = null;
            int kvaqva = 0;
            VarQvant tvq = null;


            string aa = GetAttro(gmAttro.Fen);
            if (aa == string.Empty) {
                tp = pozo.Starto();
                flagFromStart = true;
            } else {
                }
            FillMainMoves();
            lFactMoves = new List<Mova>();
            lPozos = new List<pozo>();
            lPozos.Add(tp);
            foreach( string minimov in onlymainmoves ) {
                if( minimov.StartsWith("(") ) { //Это вариант. Может иметь вложенности. Требует отдельной проработки.
                    flagVario = true;
                    string doba = DopFiltroForCommentAnaVariant(minimov.Substring(1), 2);
                    if (doba.Length > 0) {
                        tvq = CreateFromVarioString(lPozos[kvaqva - 1], doba, kvaqva); //Вариант следует уже после сделанного хода и tp уже ушла вперёд, поэтому надо использовать предыдущую позицию.
                        if (tvq != null) {
                            if (lVarQva == null) {
                                lVarQva = new List<VarQvant>();
                                }
                            lVarQva.Add(tvq);
                            }
                        }
                } else if( minimov.StartsWith("{") ) { //Это комментарий из текста, добавляем его непосредственно к позиции
                                                       //И в лист комментов с указанием ссылки на позу для обратного восстановления из сохраненного гамо
                    flagComments = true;
                    string doba = DopFiltroForCommentAnaVariant(minimov.Substring(1), 1);
                    if (doba.Length > 0) {
                        tvq = CreateFromCommentoString(doba, kvaqva);
                        if (tvq != null) {
                            if (lVarQva == null) {
                                lVarQva = new List<VarQvant>();
                                }
                            lVarQva.Add(tvq);
                            }
                        }
                } else if (minimov.StartsWith("$")) { //Это строка тайминга. Добавляем ее в лист таймингов с указанием позиции. 
                                                      //Так как возможен неполный, а только выборочный тайминг.
                    flagTiming = true;
                    if (lTimos == null) {
                        lTimos = new List<gTimo>();
                        }
                    string timo = minimov.Substring(1);
                    int curmove = tp.NumberMove;
                    bool curcolor = tp.IsQueryMoveWhite;
                    gTimo curTimo = new gTimo(curmove.ToString(), curcolor ? "w" : "b", timo);
                    lTimos.Add(curTimo);
                } else if (tp.ContraMov(minimov, 1)) {
                    lFactMoves.Add(tp.GetFactMoveFilled());
                    tp = tp.GetPozoAfterControlMove();
                    lPozos.Add(tp);
                    kvaqva++;
                } else { //раз дошли досюдова, то ход был, но он невозможный - фиксирукм это в гаму
                    flagImpossibleMove = true;
                    strimpossiblemove = string.Format("Невозможный ход {0} после {1} хода {2}", minimov, tp.NumberMove, !tp.IsQueryMoveWhite ? "белых" : "чёрных");
                    }
                }
            if (lFactMoves.Count > 0) { 
                reto = true;
                if (Qavo == string.Empty) {
                    SetAttro(gmAttro.PlyCount, lFactMoves.Count.ToString());
                    }
                }
            if (flagImpossibleMove) {
                LogoCM.OutString(string.Format("{0} - {1} $$ {2}", GamerWhite, GamerBlack, strimpossiblemove));
                }
            return reto;
            }

        /// <summary>
        /// Очистка комментариев и вариантов от специальных символов 
        /// $dd
        /// Модификация от 3 января 2017 года
        /// Заложен 3 января 2017 года
        /// </summary>
        /// <param name="obra">Обрабатываемая строка для миничистки</param>
        /// <param name="typo">1 - это комментарий; 2 - это вариант</param>
        /// <returns></returns>
        public string DopFiltroForCommentAnaVariant(string obra, int typo) {
            string reto = obra;
            string sty = (typo == 1) ? "Como" : "Varo";
            string patero1 = @"\$\d+";
            string patero2 = @"\x7F";
            if (Regex.IsMatch(reto, patero1)) {
                LogoCM.OutString(string.Format("Gamo:CAVFiltro-{0}->$\\d+ {1}", sty, reto));
                reto = Regex.Replace(reto, patero1, "");
                }
            if (Regex.IsMatch(reto, patero2)) {
                LogoCM.OutString(string.Format("Gamo:CAVFiltro-{0}->127 {1}", sty, reto));
                Regex.Replace(reto, patero2, "");
                }
            return reto.Trim();
            }

        /// <summary>
        /// Модификация от 29 сентября 2015 года
        /// Заложен 29 сентября 2015 года
        /// </summary>
        /// <param name="pcommon">Словарь с предустановленными фиксированными значениями</param>
        /// <returns>Истина - если всё чисто</returns>
        public bool FillDictoAttribute(Dictionary<gmAttro, string> pcommon) {
            bool reto = true;

            if (pgatr != null) {
                string vnutr = @"\[(.*)\s""(.*)""\]";
                intellectoattr = gmKorrAtr.AllOK;
                datro = new Dictionary<gmAttro, string>();
                foreach (string bb in pgatr) {
                    Match m1 = Regex.Match(bb, vnutr);
                    if (m1.Groups.Count > 2) {
                        string b1 = m1.Groups[1].Value;
                        string b2 = m1.Groups[2].Value;
                        gmAttro tatr = (gmAttro)Enum.Parse(typeof(gmAttro), b1);
                        if (pcommon.Keys.Contains(tatr)) {
                            b2 = pcommon[tatr];
                            }
                        //Попытка разрешить известные проблемы
                        if (b2.Contains("?")) {
                            if (tatr == gmAttro.EventDate) { //Будем брать дату по партиям их этого тура
                                intellectoattr = gmKorrAtr.EventDateHasAnswer;
                                reto = false;
                                }
                            }
                        datro.Add(tatr, b2);
                        }
                    }
                }
            return reto;
            }

        /// <summary>
        /// Модификация от 31 июля 2015 года
        /// Заложен 31 июля 2015 года
        /// </summary>
        /// <returns></returns>
        public List<string> CreateMovaRegionWithoutComments() {
            List<string> reto = null;
            const int STLE = 78;

            if (null != lFactMoves) {
                reto = new List<string>();
                StringBuilder stbu = new StringBuilder(STLE);
                bool prmove = lPozos[0].IsQueryMoveWhite;
                int mvnumber = lPozos[0].NumberMove;
                foreach (Mova aa in lFactMoves) {
                    string bb = aa.Shorto;
                    string snm = string.Empty;
                    if (prmove) {
                        snm = mvnumber.ToString().Trim() + ".";
                        if (stbu.Length + snm.Length > STLE) {
                            reto.Add(stbu.ToString());
                            stbu = new StringBuilder(STLE);
                            stbu.Append(snm);
                        } else {
                            stbu.Append((stbu.Length > 0 ? " " : "") + snm);
                            }
                    } else {
                        mvnumber++;
                        }
                    prmove = !prmove;
                    if (stbu.Length + bb.Length > STLE) {
                        reto.Add(stbu.ToString());
                        stbu = new StringBuilder(STLE);
                        stbu.Append(bb);
                    } else {
                        stbu.Append(" " + bb);
                        }
                    }
                if (stbu.Length + strResulte.Length > STLE) {
                    reto.Add(stbu.ToString());
                    stbu = new StringBuilder(STLE);
                    stbu.Append(strResulte);
                } else {
                    stbu.Append(" " + strResulte);
                    }
                if (stbu.Length > 0) {
                    reto.Add(stbu.ToString());
                    }
                }
            return reto;
            }

        /// <summary>
        /// Модификация от 7 октября 2015 года
        /// Заложен 7 октября 2015 года
        /// </summary>
        /// <returns></returns>
        public string VanStrokeMovaRegion() {
            string reto = string.Empty;
            StringBuilder sbb = new StringBuilder();
            foreach (string aa in CreateMovaRegionWithoutComments()) {
                if (sbb.Length > 0) {
                    sbb.Append(Environment.NewLine);
                    }
                sbb.Append(aa);
                }
            if (sbb.Length > 0) {
                reto = sbb.ToString();
                }
            return reto;
            }

        /// <summary>
        /// Задуман для отладочной информационной выдачи при крахе
        /// Модификация от 3 декабря 2015 года
        /// Заложен 3 декабря 2015 года
        /// </summary>
        /// <returns></returns>
        public string VanStrokePgnMovaRegion() {
            string reto = string.Empty;
            StringBuilder sbb = new StringBuilder();
            foreach (string aa in pgmova) {
                if (sbb.Length > 0) {
                    sbb.Append(Environment.NewLine);
                    }
                sbb.Append(aa);
                }
            if (sbb.Length > 0) {
                reto = sbb.ToString();
                }
            return reto;
            }

        /// <summary>
        /// Модификация от 14 октября 2015 года
        /// Заложен 14 октября 2015 года
        /// </summary>
        /// <returns></returns>
        public string VanStrokeRestAttributes() {
            string reto = string.Empty;
            StringBuilder bub = new StringBuilder();
            foreach (gmAttro aa in datro.Keys) {
                switch (aa) {
                    case gmAttro.White:
                    case gmAttro.Black:
                    case gmAttro.Result:
                    case gmAttro.Date:
                    case gmAttro.Event:
                    case gmAttro.ECO:
                    case gmAttro.PlyCount:
                    case gmAttro.WhiteElo:
                    case gmAttro.BlackElo:
                    case gmAttro.Site:
                    case gmAttro.Round:
                    case gmAttro.Fen:
                        break;
                    default:
                        string valo = datro[aa];
                        if(valo.Length > 0) {
                            if (bub.Length > 0) { bub.Append(Environment.NewLine); }
                            bub.AppendFormat("[{0}] - [{1}]", aa.ToString(), valo); 
                            }
                        break;
                    }
                }
            if (bub.Length > 0) { reto = bub.ToString(); }
            return reto;
            }

        /// <summary>
        /// Модификация от 20 ноября 2015 года
        /// Заложен 19 ноября 2015 года
        /// </summary>
        /// <returns></returns>
        public string VanStrokeTimingo() {
            string reto = string.Empty;
            if (flagTiming) {
                StringBuilder bub = new StringBuilder();
                const int STRTIMELENGTH = 80;
                int i = 0;
                foreach (gTimo aa in lTimos) {
                    if (bub.Length > 0) {
                        if (i > STRTIMELENGTH) {
                            i = 0;
                            bub.Append(Environment.NewLine);
                        } else {
                            bub.Append(" ");
                            i++;
                            }
                        }
                     bub.Append(aa.VanStroke);
                     i += aa.VanStroke.Length;
                     }
                if (bub.Length > 0) { reto = bub.ToString(); }
                }
            return reto;
            }

        /// <summary>
        /// Модификация от 20 ноября 2015 года
        /// Заложен 20 ноября 2015 года
        /// </summary>
        /// <returns></returns>
        public string VanStrokeTimoForView()
        {
            string reto = string.Empty;
            if (flagTiming)
            {
                StringBuilder bub = new StringBuilder();
                const int STRTIMELENGTH = 80;
                int i = 0;
                int k1 = 0;
                string t1 = string.Empty;
                string t2big = string.Empty;
                bool beginoVan = true;  
                foreach (gTimo aa in lTimos) {
                    if( beginoVan ) {
                        if( aa.ColorTimoIsWhite ) {
                            k1 = aa.NumberForMove;
                            t1 = aa.TimoValue;
                            beginoVan = false;
                            continue;
                        } else {
                            //Одиночное таймирование хода чёрных ??? Не знаю, возможно ли?
                            beginoVan = false;
                            k1 = aa.NumberForMove;
                            }
                    }
                    if( ! beginoVan ) {
                        if( k1 != aa.NumberForMove ) {
                            bub.AppendFormat("НОНСЕНС VanStrokeTimoForView k1 - {0}, k2 - {1}",k1, aa.NumberForMove);
                            break;
                            }
                        t2big = string.Format("{0}-{1}-{2}", k1, t1, aa.TimoValue);
                        beginoVan = true;
                        }
                    if (bub.Length > 0)
                    {
                        if (i > STRTIMELENGTH)
                        {
                            i = 0;
                            bub.Append(Environment.NewLine);
                        }
                        else
                        {
                            bub.Append(" ");
                            i++;
                        }
                    }
                    bub.Append(t2big);
                    i += t2big.Length;
                }
                if (bub.Length > 0) { reto = bub.ToString(); }
            }
            return reto;
        }

        /// <summary>
        /// Модификация от 22 ноября 2015 года
        /// Заложен 16 ноября 2015 года
        /// </summary>
        /// <param name="nm"></param>
        /// <param name="coloro"></param>
        /// <returns></returns>
        public pozo GetPozoAfterMove(int nm, string coloro) {
            pozo reto = null;
            bool wmv = true;
            if (coloro == "b") { nm++; }
            else { wmv = false; }
            reto = lPozos.SingleOrDefault(F => F.NumberMove == nm && F.IsQueryMoveWhite == wmv);
            if (reto != null) {
                curpoza = reto;
                }
            return reto;
            }

        /// <summary>
        /// Модификация от 16 ноября 2015 года
        /// Заложен 16 ноября 2015 года
        /// </summary>
        /// <returns></returns>
        public pozo GetFirstPozo() {
            curpoza = lPozos[0];
            curpozoindex = 0;
            return curpoza;
            }

        /// <summary>
        /// Модификация от 22 ноября 2015 года
        /// Заложен 22 ноября 2015 года
        /// </summary>
        /// <param name="aa"></param>
        /// <returns></returns>
        public pozo GetNextPozo(pozo aa) {
            pozo reto = null;
            int iind = 0;

            if (curpozoindex > 0) {
                iind = curpozoindex;
            } else {
                iind = lPozos.FindIndex(F => F == aa);
                }
            iind++;
            if (iind < lPozos.Count) {
                curpozoindex = iind;
                curpoza = lPozos[curpozoindex];
                reto = curpoza;
                }
            return reto;
            }

        /// <summary>
        /// Модификация от 23 ноября 2015 года
        /// Заложен 23 ноября 2015 года
        /// </summary>
        /// <param name="aa"></param>
        /// <returns></returns>
        public pozo GetPrevPozo(pozo aa) {
            pozo reto = null;
            int iind = 0;

            if (curpozoindex > 0) {
                iind = curpozoindex;
            } else {
                iind = lPozos.FindIndex(F => F == aa);
                }
            iind--;
            if( iind >= 0 ) {
                curpozoindex = iind;
                curpoza = lPozos[curpozoindex];
                reto = curpoza;
                }
            return reto;
            }

        /// <summary>
        /// Модификация от 22 июля 2015 года
        /// Заложен 22 июля 2015 года
        /// </summary>
        private void FillMainMoves() {
            onlymainmoves = new List<string>();
            flagworkabigcomment = false;
            flagworkabigtimo = false;
            foreach (string vanstring in pgmova) {
                List<string> aa = GetMovesFromString(vanstring.TrimStart());
                if( aa.Count > 0 ) {
                    onlymainmoves.AddRange( aa );
                    }
                }
            }

        /// <summary>
        /// Модификация от 12 ноября 2015 года
        /// Заложен 12 ноября 2015 года
        /// </summary>
        /// <param name="bigstrika"></param>
        private void FillMainMovesTrusted(string bigstrika) {
            onlymainmoves = new List<string>();
            string Patterno = @"\s+";
            string[] arkus = Regex.Split(bigstrika, Patterno);
            string patnumbermove = @"\d+\.";
            string patmovesymbol = @"[KQRBNa-hO][a-h1-8\-x=\+#QRBN]+";
            string patresulte = @"1-0|0-1|1/2-1/2";
            string patprobelnost = @"\s+";
            foreach (string aa in arkus) {
                if (Regex.IsMatch(aa, patnumbermove)) {
                    continue;
                } else if (Regex.IsMatch(aa, patmovesymbol)) {
                    onlymainmoves.Add(aa);
                } else if (Regex.IsMatch(aa, patresulte)) {
                    continue;
                } else if (Regex.IsMatch(aa, patprobelnost) || aa.Length == 0) {
                    continue; //Пустоту просто пропускаем
                } else {
                    onlymainmoves.Add("ЩУЧАЩУЩУ");
                    }
                }
            }

        /// <summary>
        /// Модификация от 12 ноября 2015 года
        /// Заложен 12 ноября 2015 года
        /// </summary>
        /// <param name="addingo"></param>
        private void FillAddingAttribute(string addingo) {
            string vnutr = @"\[(.+)\] - \[(.+)\]";
            foreach (Match m1 in Regex.Matches(addingo, vnutr)) {
                if (m1.Groups.Count > 2) {
                    string b1 = m1.Groups[1].Value;
                    string b2 = m1.Groups[2].Value;
                    gmAttro tatr = (gmAttro)Enum.Parse(typeof(gmAttro), b1);
                    datro.Add(tatr, b2);
                    }
                }
            }

        /// <summary>
        /// Модификация от 19 ноября 2015 года
        /// Заложен 19 ноября 2015 года
        /// </summary>
        /// <param name="timango"></param>
        private void FillListTimo(string timango) {
            if (lTimos == null) {
                lTimos = new List<gTimo>();
                string vnutr = @"(\d+)-([bw])-([:0-9]+)";
                foreach (Match m1 in Regex.Matches(timango, vnutr)) {
                    if (m1.Groups.Count > 3) {
                        string b1 = m1.Groups[1].Value;
                        string b2 = m1.Groups[2].Value;
                        string b3 = m1.Groups[3].Value;
                        lTimos.Add(new gTimo(b1, b2, b3));
                        }
                    }
                if (lTimos.Count > 0) {
                    flagTiming = true;
                    }
                }
            }

        /// <summary>
        /// Модификация от 11 января 2017 года
        /// Заложен 22 июля 2015 года
        /// </summary>
        /// <param name="vv"></param>
        /// <returns></returns>
        private List<string> GetMovesFromString( string vv ) {
            List<string> reto = new List<string>();
            List<string> kommy = null;
            List<string> varry = null;
            int anticikl = 100;
            while(flagworkabigVaro && anticikl-- > 0) { //Может быть очень и очень длинная и с вложенными скобками :(
                                      //В этом месте мы не строим иерархию. Задача вытащить целиком в одну строку самый внешний вариант.  
                string patvaroendo = @"\A([^(^)]*)\)\s*";
                Match regVaro = Regex.Match(vv, patvaroendo);
                if (regVaro.Success) { 
                    string hvost = regVaro.Groups[1].Value;
                    if (stackBigVaro > 1) {
                        bigVaro += ((bigVaro.Length > 0) ? " " : "") + hvost + ")";
                        stackBigVaro--;
                    } else {
                        bigVaro += ((bigVaro.Length > 0) ? " " : "") + hvost;
                        stackBigVaro = 0;
                        flagworkabigVaro = false;
                        reto.Add("(" + bigVaro);
                        }
                    vv = Regex.Replace(vv, patvaroendo, "");
                } else {
                    //А тута нужно проанализировать хвост на количество открывающихся скобок, но при этом не имеющих парных закрытий
                    //Такой пример не поймался предыдущим ЗАХВАТЧИКОМ (....)...)
                    bool probr = true;
                    if (vv.Contains("(")) {
                        int kop = vv.Count(S => S == '(');
                        if (vv.Contains(")")) {
                            //Т.е. это уже достаточно редкий гость и мы должны посчитать разницу между открывающимися и закрывающимися скобками.
                            int kzak = vv.Count(S => S == ')');
                            if (kop >= kzak) {
                                stackBigVaro += kop - kzak;
                            } else {
                                stackBigVaro -= kzak - kop;
                                if (stackBigVaro < 0) {
                                    throw (new myClasterException(string.Format("Нонсенс. Gamo.GetMovesFromString. Отрицательный stackBigVaro - {0}", stackBigVaro)));
                                    }
                                else if (stackBigVaro == 0) {
                                    //Тогда мы нашли окончание и надо используя жадный поиск схватить кусок и выплюнуть остаток по аналогии с верхним "хвостом"
                                    string patvnvaroendo = @"\A(.*)\)\s*";
                                    Match regvn = Regex.Match(vv, patvnvaroendo);
                                    if (regvn.Success) {
                                        string hhh = regVaro.Groups[1].Value;
                                        bigVaro += ((bigVaro.Length > 0) ? " " : "") + hhh;
                                        flagworkabigVaro = false;
                                        reto.Add("(" + bigVaro);
                                        vv = Regex.Replace(vv, patvaroendo, "");
                                        probr = false;
                                    } else {
                                        throw (new myClasterException("МиниНонсенс. Должно было быть, а не нашлось Gamo.GetMovesFromString."));
                                        }
                                    }
                                else {
                                    //Всё нормально. Брейкуем как указано ниже.
                                    }
                                }

                        } else {
                            stackBigVaro += kop;
                            }
                        }
                    if( probr ) {
                        bigVaro += ((bigVaro.Length > 0) ? " " : "") + vv;
                        vv = string.Empty;
                        }
                    break;
                    }
                }
            if( anticikl == 0 ) {
                throw (new myClasterException("Сработал антицикл."));
                }
            if (vv.Length > 0) {
                if (flagworkabigcomment) {
                    //Возможно это и не комментарий вовсе :(, а начало тайминга
                    string pataddtimo = @"\A\s*\[%clk ([:0-9.]+)]\s*}";
                    if (Regex.IsMatch(vv, pataddtimo)) {
                        flagworkabigcomment = false;
                        vv = Regex.Replace(vv, pataddtimo, "$$$1");
                    } else {
                        //Если на этой строке есть окончание комментария, то убираем этот хвостик
                        //иначе пропускаем целиком данную строку
                        string Patterno = @"\A([^}]*)}\s*";
                        Match regHvost = Regex.Match(vv, Patterno);
                        if (regHvost.Success) {
                            string hvost = regHvost.Groups[1].Value;
                            reto.Add("{" + bigCommento + " " + hvost);
                            vv = Regex.Replace(vv, Patterno, "");
                            flagworkabigcomment = false;
                        } else {
                            bigCommento += ((bigCommento.Length > 0) ? " " : "") + vv;   //Бывает на предыдущей строке только одна фигурная скобка :(
                            vv = string.Empty;
                            }
                        }
                    }
                else if (flagworkabigtimo) {
                    string Patterno = @"\A\s*([:0-9.]+)]\s*}";
                    if (Regex.IsMatch(vv, Patterno)) {
                        flagworkabigtimo = false;
                        vv = Regex.Replace(vv, Patterno, "$$$1");
                        }
                    }
                if (vv.Length > 0) {
                    string pattimo = @"{\s*\[%clk ([:0-9.]+)]\s*}";
                    vv = Regex.Replace(vv, pattimo, "$$$1");
                    string pattimoRazryv = @"\s*{\s*\[%clk\s*\z";
                    if (Regex.IsMatch(vv, pattimoRazryv)) {
                        flagworkabigtimo = true;
                        vv = Regex.Replace(vv, pattimoRazryv, "");
                        }
                    string pattimoRazryvDrugoy = @"{\s*\[%clk ([:0-9.]+)]\s*\Z";
                    if (Regex.IsMatch(vv, pattimoRazryvDrugoy)) {
                        flagworkabigcomment = true;
                        vv = Regex.Replace(vv, pattimoRazryvDrugoy, "$$$1");
                        }
                    if (vv.Contains("(")) {
                        string Patterno = @"\((.*?)\)\s*";
                        MatchCollection mcc = Regex.Matches(vv, Patterno);
                        if (mcc.Count > 0) {
                            varry = new List<string>();
                            foreach (Match aa in mcc) {
                                varry.Add(aa.Groups[1].Value);
                                }
                            vv = Regex.Replace(vv, Patterno, "& ");
                            }
                        string patvariostartonthisstring = @"\s*\((.*)\z";
                        Match regVario = Regex.Match(vv, patvariostartonthisstring);
                        if (regVario.Success) {
                            string ddst = regVario.Groups[1].Value;
                            bigVaro = ((flagworkabigVaro) ? bigVaro + "(" : "") + ddst;
                            flagworkabigVaro = true;
                            stackBigVaro++;
                            //Добавленный кусок мог и внутри себя содержать открывающиеся варианты, а мы пока про них ничего не знаем.
                            //Закрывающую скобку варианта мы бы съели как парную предыдущим патерном.
                            //Поэтому надо увеличивать стек на количество открывающихся скобок в добавленной строке.
                            if( ddst.Contains("(") ) {
                                int ddkvo = ddst.Count(S => S == '(');
                                stackBigVaro += ddkvo;
                                }
                            vv = Regex.Replace(vv, patvariostartonthisstring, "");
                            }
                        }
                    if (vv.Length > 0) {
                        if (vv.Contains("{")) {
                            string Patterno = @"{(.*?)}\s*";
                            MatchCollection mcc = Regex.Matches(vv, Patterno);
                            if (mcc.Count > 0) {
                                kommy = new List<string>();
                                foreach (Match aa in mcc) {
                                    kommy.Add(aa.Groups[1].Value);
                                    }
                                vv = Regex.Replace(vv, Patterno, "@ ");
                                }
                            string patcommentstartonthisstring = @"\s*{(.*)\z";
                            Match regCommo = Regex.Match(vv, patcommentstartonthisstring);
                            if (regCommo.Success) {
                                flagworkabigcomment = true;
                                bigCommento = regCommo.Groups[1].Value;
                                vv = Regex.Replace(vv, patcommentstartonthisstring, "");
                                }
                            }
                        if (vv.Length > 0) {
                            string[] arkus = vv.Split(' ');
                            string patnumbermove = @"\d+\.";
                            string patmovesymbol = @"[KQRBNa-hO][a-h1-8\-x=\+#QRBN]+";
                            string patresulte = @"1-0|0-1|1/2-1/2";
                            string patprobelnost = @"\s+";
                            int kk = 0;
                            int kv = 0;
                            foreach (string aa in arkus) {
                                if (Regex.IsMatch(aa, patnumbermove)) { //Бывает что после точки номера хода не стоит пробел (не по стандарту, но), а сразу следует ход белых (Кириши2014)
                                                                        //Поэтому нужно анализировать на наличие такого казуса
                                    if( aa.EndsWith(".") ) {
                                        continue;
                                    } else { //Турки использовали точку как разделитель у секунд и она находится в середине :(
                                        //Поэтому проверим сначала на тайминг
                                        if( aa.StartsWith("$") ) {
                                            reto.Add(aa);
                                            continue;
                                        } else {
                                            string[] bbkus = aa.Split('.');
                                            if( Regex.IsMatch(bbkus[1], patmovesymbol) ) {
                                                reto.Add(bbkus[1]);
                                                }
                                            }
                                        }
                                    } 
                                else if (Regex.IsMatch(aa, patmovesymbol)) {
                                    reto.Add(aa);
                                    }
                                else if (Regex.IsMatch(aa, patresulte)) {
                                    flagEndResulte = true;
                                    strResulte = aa;
                                    continue;
                                    }
                                else if (aa.StartsWith("$")) {
                                    reto.Add(aa);
                                    }
                                else if (aa.StartsWith("@")) {
                                    string skma = kommy[kk++];
                                    anticikl = 10;
                                    while( skma.Contains("&") && anticikl-- > 0) {
                                        int ia = skma.IndexOf("&");
                                        string svar = varry[kv++];
                                        string svar2 = (svar.StartsWith("(") ? "" : "(") + svar + ")";
                                        skma = skma.Substring(0, ia) + svar2 + skma.Substring(ia + 1);
                                        }
                                    reto.Add("{" + skma);                                    
                                    }
                                else if (aa.StartsWith("&")) {
                                    reto.Add("(" + varry[kv++]);
                                    }
                                else if (Regex.IsMatch(aa, patprobelnost) || aa.Length == 0) {
                                    continue; //Пустоту просто пропускаем
                                    }
                                else {
                                    reto.Add("ЖУЖАЖУЖУ");
                                    }
                                }
                            }
                        }
                    }
                }
            return reto;
            }
        
        /// <summary>
        /// Модификация от 10 июня 2015 года
        /// Заложен от 10 июня 2015 года
        /// </summary>
        /// <param name="pp"></param>
        /// <returns></returns>
        private string GetAttro( gmAttro pp ) {
            string reto = string.Empty;
            if (null != datro) {
                if (datro.ContainsKey(pp)) {
                    reto = datro[pp];
                    }
                }
            return reto;                
            }

        /// <summary>
        /// Модификация от 29 сентября 2015 года
        /// Заложен 29 сентября 2015 года
        /// </summary>
        /// <param name="pp"></param>
        /// <param name="valo"></param>
        private void SetAttro(gmAttro pp, string valo) {
            if (null != datro) {
                if (datro.ContainsKey(pp)) {
                    datro[pp] = valo;
                } else {
                    datro.Add(pp, valo);
                    }
                }
            }

        /// <summary>
        /// Модификация от 12 ноября 2015 года
        /// Заложен 12 ноября 2015 года
        /// </summary>
        /// <param name="movazone"></param>
        private void FillMovesAndPozos(string movazone) {
            pozo tp = null;
            FillMainMovesTrusted(movazone);
            if (flagFromStart) {
                tp = pozo.Starto();
                }
            lFactMoves = new List<Mova>();
            lPozos = new List<pozo>();

            lPozos.Add(tp);
            foreach (string minimov in onlymainmoves) {
                if (tp.ContraMov(minimov, 1)) {
                    lFactMoves.Add(tp.GetFactMoveFilled());
                    tp = tp.GetPozoAfterControlMove();
                    lPozos.Add(tp);
                } else { //раз дошли досюдова, то трастовый набор ходов оказался подмоченным - фиксируем это в гаму
                    flagImpossibleMove = true;
                    strimpossiblemove = string.Format("ТРАСТО-Невозможный ход {0} после {1} хода {2}", minimov, tp.NumberMove, !tp.IsQueryMoveWhite ? "белых" : "чёрных");
                    }
                }
            }

        /// <summary>
        /// Модификация от 30 марта 2016 года
        /// Заложен 30 марта 2016 года
        /// </summary>
        /// <param name="pcom"></param>
        /// <param name="npoluhod"></param>
        /// <returns></returns>
        private VarQvant CreateFromCommentoString(string pcom, int npoluhod) {
            VarQvant reto = new VarQvant(npoluhod, pcom, null);
            return reto;
            }

        /// <summary>
        /// Модификация от 1 апреля 2016 года
        /// Заложен 30 марта 2016 года
        /// </summary>
        /// <param name="ap"></param>
        /// <param name="pvar"></param>
        /// <param name="npoluhod"></param>
        /// <returns></returns>
        private VarQvant CreateFromVarioString(pozo ap, string pvar, int npoluhod) {
            VarQvant reto = null;
            pozo tp = ap;
            int kvaqva = 0;
            VarQvant tvq = null;
            List<string> movastringlist = null;
            List<Mova> lFMova = new List<Mova>();
            List<pozo> lNPozo = new List<pozo>();
            List<VarQvant> lVQva = null;
            string exepostringo = string.Empty;

            bool vlogha = false;

            vlogha = FillVarioMoves(pvar, out movastringlist);
            if (vlogha) {
                lVQva = new List<VarQvant>();
                }
            try {
                foreach (string minimov in movastringlist) {
                    if (minimov.StartsWith("(")) { //Это вариант. Может иметь вложенности. Требует отдельной проработки.
                        string doba = minimov.Substring(1);
                        if (doba.Length > 0) {
                            //Предполагается, что это вариант, но встречается и просто комментарий без ходов вообще, но в нужных скобках. Обычно это происходит в теле варокоммента.
                            //Поэтому следует проверить на наличие ходов. А при их отсутствии отнести к комментарию
                            string patmovesymbol = @"[KQRBNa-hO][a-h1-8\-x=\+#QRBN]+";
                            if (Regex.IsMatch(doba, patmovesymbol)) {
                                tvq = CreateFromVarioString(lNPozo[kvaqva - 2], doba, kvaqva);   //Вариант следует уже после сделанного хода и tp уже ушла вперёд, поэтому надо использовать предыдущую позицию.
                                //Но в подварианте позиции кладутся в лист уже после хода. Поэтому сдвигать надо на 2.   
                            } else {
                                tvq = CreateFromCommentoString(doba, kvaqva);
                                }
                            if (tvq != null) {
                                lVQva.Add(tvq);
                                }
                            }
                        }
                    else if (minimov.StartsWith("{")) { //Это комментарий из текста, добавляем его непосредственно к позиции
                                                        //И в лист комментов с указанием ссылки на позу для обратного восстановления из сохраненного гамо
                        string doba = minimov.Substring(1);
                        tvq = CreateFromCommentoString(doba, kvaqva);
                        if (tvq != null) {
                            lVQva.Add(tvq);
                            }
                        }
                    else if (tp.ContraMov(minimov, 1)) {
                        lFMova.Add(tp.GetFactMoveFilled());
                        tp = tp.GetPozoAfterControlMove();
                        lNPozo.Add(tp);
                        kvaqva++;
                        }
                    else { //раз дошли досюдова, то ход был, но он невозможный - фиксирукм это в гаму
                        flagImpossibleMove = true;
                        strimpossiblemove = string.Format("Невозможный ход в варианте {0} после {1} хода {2}", minimov, tp.NumberMove, !tp.IsQueryMoveWhite ? "белых" : "чёрных");
                        }
                    }
            } catch (Exception exepa) {
                exepostringo = "КРАХ@@->Gamo][CreateFromVarioString " + exepa.Message;
                LogoCM.OutString(exepostringo);
                lFMova = new List<Mova>();
                }
            if (lFMova.Count > 0) {
                Vario aa = new Vario(ap, lFMova, lNPozo, lVQva);
                reto = new VarQvant(npoluhod, null, aa);
            } else {
                Vario aa = new Vario(ap, false, exepostringo);
                reto = new VarQvant(npoluhod, "Была строка " + pvar, aa);
                }
            return reto;
            }

        /// <summary>
        /// Модификация от 31 марта 2016 года
        /// Заложен 31 марта 2016 года
        /// </summary>
        /// <param name="vv"></param>
        /// <param name="naboro"></param>
        /// <returns></returns>
        private bool FillVarioMoves(string vv, out List<string> naboro) {
            bool reto = false;
            naboro = new List<string>();
            List<string> kommy = null;
            List<string> varry = null;
            int anticikl = 10;

            while( vv.Contains("(") && anticikl-- > 0 ) {
                //if (vv[0] == '(') { vv = " " + vv; }
                string Patterno = "(" +  
                                          "(" +
                                              @"(?<Open>\s*\()" +   //Пробел после опен смысловой!!!
                                              /*@"(?<Open> \()" +   //Пробел после опен смысловой!!!*/
                                              "[^)(]*" +
                                          ")+" +
                                          @"(?<Close-Open>\)\s*)+" +
                                   ")+" +
                                  @"(?(Open)(?!))";
                MatchCollection mcc = Regex.Matches(vv, Patterno);
                if (mcc.Count > 0) {
                    if (varry == null) { varry = new List<string>(); }
                    foreach (Match aa in mcc) {
                        string bb = aa.Value.TrimEnd();
                        vv = vv.Replace(bb, " &");
                        varry.Add(bb.Substring(1, bb.Length - 2).TrimStart());
                        }
                    }
                }
            if (vv.Contains("{")) {
                string Patterno = @"{(.*?)}\s*";
                MatchCollection mcc = Regex.Matches(vv, Patterno);
                if (mcc.Count > 0) {
                    kommy = new List<string>();
                    foreach (Match aa in mcc) {
                        kommy.Add(aa.Groups[1].Value);
                        }
                    vv = Regex.Replace(vv, Patterno, "@ ");
                    }
                }
            string[] arkus = vv.Split(' ');
            string patnumbermove = @"\d+\.";
            string patmovesymbol = @"[KQRBNa-hO][a-h1-8\-x=\+#QRBN]+";
            string patresulte = @"1-0|0-1|1/2-1/2";
            string patprobelnost = @"\s+";
            int kk = 0;
            int kv = 0;
            foreach (string aa in arkus) {
                if( Regex.IsMatch(aa, patnumbermove) ) { //Бывает что после точки номера хода не стоит пробел (не по стандарту, но), а сразу следует ход белых (Кириши2014)
                                                         //Поэтому нужно анализировать на наличие такого казуса
                    if( aa.EndsWith(".") ) {
                        continue;
                    } else {
                        string[] bbkus = aa.Split('.');
                        if( Regex.IsMatch(bbkus[1], patmovesymbol) ) {
                            naboro.Add(bbkus[1]);
                            }
                        }
                    }
                else if( Regex.IsMatch(aa, patmovesymbol) ) {
                    naboro.Add(aa);
                    }
                else if( Regex.IsMatch(aa, patresulte) ) {
                    naboro.Add("{" + aa);
                    reto = true;
                    }
                else if( aa.StartsWith("@") ) {
                    string sko = kommy[kk++];
                    anticikl = 10;
                    while( sko.Contains("&") && anticikl-- > 0) {
                        int ia = sko.IndexOf("&");
                        string svar = varry[kv++];
                        string svar2 = (svar.StartsWith("(") ? "" : "(") + svar + ")";
                        sko = sko.Substring(0, ia) + svar2 + sko.Substring(ia + 1);
                        }
                    naboro.Add("{" + sko);
                    reto = true;
                    }
                else if( aa.StartsWith("&") ) {
                    string svar = varry[kv++];
                    string svar2 = (svar.StartsWith("(") ? "" : "(") + svar;
                    naboro.Add(svar2);
                    reto = true;
                    }
                else if( Regex.IsMatch(aa, patprobelnost) || aa.Length == 0 ) {
                    continue; //Пустоту просто пропускаем
                    }
                else {
                    naboro.Add("ЖУЖАЖУЖУ");
                    }
                }
            return reto;
            }

        static public Dictionary<gmAttro, string> CreateDictoAttribute(List<string> aa) {
            Dictionary<gmAttro, string> reto = null;
            
            if (aa != null) {
                string vnutr = @"\[(.*)\s""(.*)""\]";
                reto = new Dictionary<gmAttro, string>();
                foreach( string bb in aa ) {
                    Match m1 = Regex.Match(bb, vnutr);
                    if (m1.Groups.Count > 2) {
                        string b1 = m1.Groups[1].Value;
                        string b2 = m1.Groups[2].Value;
                        gmAttro tatr = (gmAttro)Enum.Parse(typeof(gmAttro), b1);
                        reto.Add(tatr, b2);
                        }
                    }
                }
            return reto;
            }


#region--------------------------Свойства объекта-----------------------------------------
        public List<string> pgnMoveRegione { get { return pgmova; } }
        public string GamerWhite { get { return GetAttro(gmAttro.White); } }
        public string GamerBlack { get { return GetAttro(gmAttro.Black); } }
        public string Date { get { return GetAttro(gmAttro.Date); } set { SetAttro(gmAttro.Date, value); } }
        public string EventoDate { get { return GetAttro(gmAttro.EventDate); } set { SetAttro(gmAttro.EventDate, value); } }
        public string Roundo { get { return GetAttro(gmAttro.Round); } }
        public string Resulto { get { return GetAttro(gmAttro.Result); } }
        public string ECO { get { return GetAttro(gmAttro.ECO); } }
        public string Evento { get { return GetAttro(gmAttro.Event); } }
        public string Qavo { get { return GetAttro(gmAttro.PlyCount); } }
        public string EloWhite { get { return GetAttro(gmAttro.WhiteElo); } }
        public string EloBlack { get { return GetAttro(gmAttro.BlackElo); } }
        public string Sito { get { return GetAttro(gmAttro.Site); } }
        public string Fen { get { return GetAttro(gmAttro.Fen); } }
        public bool ImpossibleMoveFlag { get { return flagImpossibleMove; } }
        public string ImpossibleMoveString { get { return strimpossiblemove; } }
        public bool TimingFlag { get { return flagTiming; } }
        public bool StartoFlag { get { return flagFromStart; } }
        public bool CommtoFlag { get { return flagComments; } }
        public bool VariantoFlag { get { return flagVario; } }
        public List<gTimo> ListoTimo { get { return lTimos; } }
        public List<pozo> ListoPozo { get { return lPozos; } }
        public List<Mova> ListoMovo { get { return lFactMoves; } }
        public List<VarQvant> ListVaroCom { get { return lVarQva; } }
        public gmKorrAtr KorrFlago { get { return intellectoattr; } }
#endregion-----------------------Свойства объекта-----------------------------------------
    }
#endregion--------------------------------ПЕРВИЧНЫЙ КЛАСС--Gamo------------------------

#region--------------------------------ДРУГОЙ КЛАСС--pgWorka------------------------
    public class pgWoka
    {
        private List<Gamo> lga;
        private string failonamo;
        private List<string> logomesso;
        private Dictionary<string,string> rondoeventdatelist;
        private bool pvpustoline;


        public pgWoka() { }
        public pgWoka(string fnamo) {

            failonamo = fnamo;
            TextReader fafo = new StreamReader(File.Open(fnamo, FileMode.Open));
            lga = new List<Gamo>();
            string strLine;
            pvpustoline = false;
          
            strLine = GetNextNonEmptyLine(fafo);
            while (strLine != null) {
                List<string> maatr = new List<string>();
                List<string> mamov = new List<string>();
                while (strLine != null && strLine[0] == '[') {
                    maatr.Add(strLine);
                    strLine = GetNextNonEmptyLine(fafo);
                    }
                if (strLine != null) {
                    mamov.Add(strLine);
                    pvpustoline = true;
                    strLine = GetNextNonEmptyLine(fafo);
                    while (strLine != null && pvpustoline) {
                        if (strLine[0] == '[') {
                            string vnutr = @"\A\[(.*)\s""(.*)""\]\Z";
                            if (Regex.IsMatch(strLine, vnutr)) {
                                break;
                                }
                            }
                        mamov.Add(strLine);
                        strLine = GetNextNonEmptyLine(fafo);
                        }
                    if (maatr.Count != 0 || mamov.Count != 0) {
                        lga.Add(new Gamo(maatr, mamov));
                        }
                    }
                }
            fafo.Close();
            }

        /// <summary>
        /// Модификация от 16 марта 2017 года
        /// Заложен 31 августа 2015 года
        /// </summary>
        /// <param name="attrolist">Набор предустановленных атрибутов</param>
        /// <param name="intellipredolist">Набор предустановленных заготовок для интеллектуального обновления значений атрибутов
        /// Готовится на основании анализа лога</param>
        public void FullListControl(List<string> attrolist, List<string> intellipredolist) {
            List<Gamo> problemolist = null;

            Dictionary<gmAttro, string> predo = Gamo.CreateDictoAttribute(attrolist);

            logomesso = new List<string>();
            List<string> progo = null;
            foreach (Gamo aa in lga) {
                if (!aa.FillDictoAttribute(predo)) {
                    if (problemolist == null) {
                        problemolist = new List<Gamo>();
                        }
                    problemolist.Add(aa);
                    }
                try {
                    aa.MovaControlling();
                } catch (Exception ex) {
                    throw new myClasterException(string.Format("Дополнительная инфо по партии: {0}-{1} {2} {3} {4}", aa.GamerWhite, aa.GamerBlack, 
                                                    aa.VanStrokePgnMovaRegion(), Environment.NewLine, ex.Message ));
                    }
                if (!aa.TimingFlag) {
                    progo = aa.CreateMovaRegionWithoutComments();
                    for (int i = 0; i < progo.Count; i++) {
                        string vanProgo = progo[i];
                        string twoFailo = (i < aa.pgnMoveRegione.Count) ? aa.pgnMoveRegione[i] : string.Empty;
                        if (vanProgo != twoFailo) { //Пишем сообщение в лог
                            string bblogo = string.Format("{0}-{1} Отличие:файл-прого${2}${3}$", aa.GamerWhite, aa.GamerBlack, twoFailo, vanProgo);
                            logomesso.Add(bblogo);
                            }
                        }
                    }
                if (aa.ImpossibleMoveFlag) {
                    logomesso.Add(string.Format("{0}-{1} !!! {2}", aa.GamerWhite, aa.GamerBlack, aa.ImpossibleMoveString));
                    }
                }
            if (problemolist != null) {
                IntelPredustanovka(intellipredolist);
                foreach (Gamo aa in problemolist) {
                    string ichres = string.Empty;
                    if (aa.KorrFlago == gmKorrAtr.EventDateHasAnswer) {
                        ichres = IntelChangeEventDate(aa);
                        }
                    string bblogo = string.Format("{0}-{1} ИнтеллиЧенж:{2}", aa.GamerWhite, aa.GamerBlack, ichres);
                    logomesso.Add(bblogo);
                    }
                }
            }

        /// <summary>
        /// Модификация от 30 сентября 2015 года
        /// Заложен 30 сентября 2015 года
        /// </summary>
        public void GruzoWorko() {
            string conffile = Properties.Settings.Default.GamoConfigoAtr;
            TextReader fafo = new StreamReader(File.Open(conffile, FileMode.Open));

            string strLine;
            List<string> maatr = new List<string>();
            List<string> mapredo = new List<string>();

            strLine = GetNextNonEmptyLine(fafo);
            while (strLine != null) {
                while (strLine != null && strLine[0] == '[') {
                    maatr.Add(strLine);
                    strLine = GetNextNonEmptyLine(fafo);
                    }
                if (strLine != null && strLine.Contains('$') ) {
                    mapredo.Add(strLine);
                    strLine = GetNextNonEmptyLine(fafo);
                    while (strLine != null && strLine.Contains('$')) {
                        mapredo.Add(strLine);
                        strLine = GetNextNonEmptyLine(fafo);
                        }
                    }
                }
            fafo.Close();
            FullListControl(maatr, mapredo);
            }

        private string GetNextNonEmptyLine(TextReader reader) {
            string strRetVal;

            strRetVal = reader.ReadLine();
            while (strRetVal != null && strRetVal == String.Empty) {
                pvpustoline = false;
                strRetVal = reader.ReadLine();
                }
            return (strRetVal);
            }

        /// <summary>
        /// Модификация от 29 сентября 2015 года
        /// Заложен 29 сентября 2015 года
        /// </summary>
        /// <param name="pgm">Элемент, в котором и будем менять значение Даты битвы</param>
        /// <returns></returns>
        private string IntelChangeEventDate(Gamo pgm) {
            string reto = string.Format("Не удалось скорректировать значение EventDate ${0}$", pgm.EventoDate);
            string valonew = string.Empty;
            if( rondoeventdatelist == null ) {
                rondoeventdatelist = new Dictionary<string,string>();
                }
            string rondo = pgm.Roundo;
            if (rondo.Contains(".")) { //По другому вроде бы и не должно быть на дату создания метода
                string[] aa = rondo.Split('.');
                string isko = aa[0];
                if (rondoeventdatelist.ContainsKey(isko)) {
                    valonew = rondoeventdatelist[isko];
                } else {
                    List<Gamo> lrondo = lga.Where(G => G.KorrFlago == gmKorrAtr.AllOK && G.Roundo.StartsWith(isko + ".")).ToList();
                    if (lrondo.Count > 0) {
                        foreach (Gamo bb in lrondo) {
                            string bevento = bb.EventoDate;
                            if (valonew == string.Empty) {
                                valonew = bevento;
                            } else {
                                if (valonew != bevento) {
                                    reto += string.Format(" --- Две разных даты у {0} раунда ${1}${2}$", isko, valonew, bevento);
                                    valonew = string.Empty;
                                    break;
                                    }
                                }
                            }
                        if (valonew != string.Empty) {
                            rondoeventdatelist.Add(isko, valonew);
                            }
                        }
                    }
                if (valonew != string.Empty) {
                    string oldo = pgm.EventoDate;
                    pgm.EventoDate = valonew;
                    reto = string.Format("Произведена замена с {0} на {1}", oldo, valonew);
                    }
                }
            return reto;
            }

        /// <summary>
        /// Модификация от 2 октября 2015 года
        /// Заложен 2 октября 2015 года
        /// </summary>
        /// <returns></returns>
        private string bigomesso() {
            string reto = string.Empty;
            if (logomesso != null) {
                StringBuilder dd = new StringBuilder();
                foreach (string aa in logomesso) {
                    dd.AppendLine(aa);
                    }
                reto = dd.ToString();
                }
            return reto;
            }

        /// <summary>
        /// Модификация от 30 сентября 2015 года
        /// Заложен 30 сентября 2015 года
        /// </summary>
        /// <param name="predpara">Набор предустановленных заготовок для интеллектуального обновления значений атрибутов
        /// Готовится на основании анализа лога</param>
        private void IntelPredustanovka(List<string> predpara) {
            foreach (string aa in predpara) {
                string[] mm = aa.Split('$');
                string typo = mm[0];
                if (typo == "EventDateHasAnswer") {
                    if (rondoeventdatelist == null) {
                        rondoeventdatelist = new Dictionary<string, string>();
                        }
                    string isko = mm[1];
                    string valo = mm[2];
                    if (!rondoeventdatelist.ContainsKey(isko)) {
                        rondoeventdatelist.Add(isko, valo);
                        }
                    }
                }
            }

        public List<Gamo> Listo { get { return lga; } }
        public string LogoMesso { get { return bigomesso(); } }
    }
#endregion-----------------------------ДРУГОЙ КЛАСС--pgWorka------------------------

#region--------------------------------ДРУГОЙ КЛАСС--gTimo------------------------
    public class gTimo {
        private string movenumber;
        private string coloro;
        private string timostring;

        public gTimo() { }

        public gTimo(string pmn, string pcol, string ptimo) {
            movenumber = pmn;
            coloro = pcol;
            timostring = ptimo;
            }

        public string VanStroke { get { return string.Format("{0}-{1}-{2}",movenumber,coloro,timostring); } }
        public int NumberForMove {
            get {
                int nm = int.Parse(movenumber);
                if (coloro == "w") { nm--; }
                return nm;
                }
            }
        public bool ColorTimoIsWhite { get { return (coloro == "b"); } }
        public string TimoValue { get { return timostring; } }

        }
#endregion-----------------------------ДРУГОЙ КЛАСС--gTimo------------------------

    #region--------------------------------ENUM------------------------
    public enum gmAttro {
        Event,
        Site,
        Date,
        Round,
        White,
        Black,
        Result,
        ECO,
        WhiteElo,
        BlackElo,
        PlyCount,
        EventDate,
        SourceDate,
        Fen,
        LiveChessVersion,
        Board,
        WhiteTitle,
        WhiteCountry,
        WhiteFideId,
        WhiteEloChange,
        BlackTitle,
        BlackCountry,
        BlackFideId,
        BlackEloChange,
        TimeControl,
        WhiteTeam,
        BlackTeam,
        EventType, 
        EventRounds,
        EventCountry,
        Source,
        Annotator,
        Remark,
        Input,
        Owner,
        WhiteClock,
        BlackClock,
        Clock,
        GameDuration,
        Opening,
        Termination,
        TerminationDetails
        }

    public enum gmKorrAtr {
        AllOK,
        EventDateHasAnswer,
        Unknown
        }
#endregion-----------------------------ENUM------------------------

    public class GamaException : Exception
    {
        public GamaException()
        {
        }
        public GamaException(string message)
            : base(message)
        {
        }
        public GamaException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }

    }