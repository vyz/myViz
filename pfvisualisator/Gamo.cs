using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;


namespace pfVisualisator {
    public class Gamo {
        private List<string> pgatr;
        private List<string> pgmova;
        private List<string> onlymainmoves;
        private bool flagComments;
        private bool flagTiming;
        private bool flagFromStart;
        private bool flagEndResulte;
        private bool flagworkabigcomment; //Комментарий на несколько строк
        private bool flagworkabigtimo;  //Таймовый коммент разбит концом строки (WorldCup 2015)
        private bool flagImpossibleMove;
        private string strimpossiblemove;
        private string strResulte;
        private List<Mova> lFactMoves;
        private List<pozo> lPozos;
        private List<gTimo> lTimos;
        private gmKorrAtr intellectoattr;

        private Dictionary<gmAttro, string> datro;

        public Gamo() { }

        public Gamo(List<string> attro, List<string> mova) {
            pgatr = attro;
            pgmova = mova;
            flagComments = flagEndResulte = flagFromStart = false;
            }

        /// <summary>
        /// Модификация от 12 ноября 2015 года
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
            }

        /// <summary>
        /// Модификация от 22 октября 2015 года
        /// Заложен июнь 2015 года
        /// </summary>
        /// <returns></returns>
        public bool MovaControlling() {
            bool reto = false;
            pozo tp = null; 
            List<string> variantosy;
            string commento;

            string aa = GetAttro(gmAttro.Fen);
            if (aa == string.Empty)
            {
                tp = pozo.Starto();
                flagFromStart = true;
            }
            else
            {
            }
            FillMainMoves();
            lFactMoves = new List<Mova>();
            lPozos = new List<pozo>();

            lPozos.Add(tp);
            foreach( string minimov in onlymainmoves ) {
                if (minimov.StartsWith("{")) { //Это комментарий из текста, добавляем его непосредственно к позиции
                                               //И в лист комментов с указанием ссылки на позу для обратного восстановления из сохраненного гамо
                    flagComments = true;
                    string doba = minimov.Substring(1);
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
                } else if (tp.ContraMov(minimov)) {
                    lFactMoves.Add(tp.GetFactMoveFilled());
                    tp = tp.GetPozoAfterControlMove();
                    lPozos.Add(tp);
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
            return reto;
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
        /// Модификация от 21 октября 2015 года
        /// Заложен 22 июля 2015 года
        /// </summary>
        /// <param name="vv"></param>
        /// <returns></returns>
        private List<string> GetMovesFromString( string vv ) {
            List<string> reto = new List<string>();

            if (flagworkabigcomment) {
                //Если на этой строке есть окончание комментария, то убираем этот хвостик
                //иначе пропускаем целиком данную строку
                string Patterno = @"\A.*}\s*";
                if (Regex.IsMatch(vv, Patterno)) {
                    vv = Regex.Replace(vv, Patterno, "");
                    flagworkabigcomment = false;
                } else {
                    vv = string.Empty;
                    }
            } else if(flagworkabigtimo) {
                string Patterno = @"\A\s*(\d:\d+:\d+)]}";
                if(Regex.IsMatch(vv, Patterno)) {
                    flagworkabigtimo = false;
                    vv = Regex.Replace(vv, Patterno, "$$$1");
                    }
                }
            string pattimo = @"{\[%clk (\d:\d+:\d+)]}";
            vv = Regex.Replace(vv, pattimo, "$$$1");
            string pattimoRazryv = @"\s*{\[%clk\s*\z";
            if( Regex.IsMatch(vv, pattimoRazryv)) {
                flagworkabigtimo = true;
                vv = Regex.Replace(vv, pattimoRazryv, ""); 
                }
            if (vv.Contains("{")) {
                string Patterno = @"{.*}\s*";
                vv = Regex.Replace(vv, Patterno, "");

                string patcommentstartonthisstring = @"\s{.*\z";
                if (Regex.IsMatch(vv, patcommentstartonthisstring)) {
                    flagworkabigcomment = true;
                    vv = Regex.Replace(vv, patcommentstartonthisstring, "");
                    }
                }
            string[] arkus = vv.Split(' ');
            string patnumbermove = @"\d+\.";
            string patmovesymbol = @"[KQRBNa-hO][a-h1-8\-x=\+#QRBN]+";
            string patresulte = @"1-0|0-1|1/2-1/2";
            string patprobelnost = @"\s+";
            foreach (string aa in arkus) {
                if (Regex.IsMatch(aa, patnumbermove))
                { //Бывает что после точки номера хода не стоит пробел (не по стандарту, но), а сразу следует ход белых (Кириши2014)
                  //Поэтому нужно анализировать на наличие такого казуса
                    if (aa.EndsWith(".")) {
                        continue;
                    } else {
                        string[] bbkus = aa.Split('.');
                        if( Regex.IsMatch(bbkus[1], patmovesymbol) ) {
                            reto.Add(bbkus[1]);
                            }
                        }
                } else if (Regex.IsMatch(aa, patmovesymbol)) {
                    reto.Add(aa);
                } else if (Regex.IsMatch(aa, patresulte)) {
                    flagEndResulte = true;
                    strResulte = aa;
                    continue;
                } else if (aa.StartsWith("$")) {
                    reto.Add(aa);
                } else if (Regex.IsMatch(aa, patprobelnost) || aa.Length == 0) {
                    continue; //Пустоту просто пропускаем
                } else {
                    reto.Add("ЖУЖАЖУЖУ");
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
            if (flagFromStart)
            {
                tp = pozo.Starto();
            }
            lFactMoves = new List<Mova>();
            lPozos = new List<pozo>();

            lPozos.Add(tp);
            foreach (string minimov in onlymainmoves) {
                if (tp.ContraMov(minimov)) {
                    lFactMoves.Add(tp.GetFactMoveFilled());
                    tp = tp.GetPozoAfterControlMove();
                    lPozos.Add(tp);
                } else { //раз дошли досюдова, то трастовый набор ходов оказался подмоченным - фиксируем это в гаму
                    flagImpossibleMove = true;
                    strimpossiblemove = string.Format("ТРАСТО-Невозможный ход {0} после {1} хода {2}", minimov, tp.NumberMove, !tp.IsQueryMoveWhite ? "белых" : "чёрных");
                    }
                }
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
        public List<gTimo> ListoTimo { get { return lTimos; } }
        public gmKorrAtr KorrFlago { get { return intellectoattr; } }
#endregion-----------------------Свойства объекта-----------------------------------------
    }

#region--------------------------------ДРУГОЙ КЛАСС--pgWorka------------------------
    public class pgWoka
    {
        private List<Gamo> lga;
        private string failonamo;
        private List<string> logomesso;
        private Dictionary<string,string> rondoeventdatelist;

        public pgWoka() { }
        public pgWoka(string fnamo) {

            failonamo = fnamo;
            TextReader fafo = new StreamReader(File.Open(fnamo, FileMode.Open));
            lga = new List<Gamo>();
            string strLine;
          
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
                    strLine = GetNextNonEmptyLine(fafo);
                    while (strLine != null && strLine[0] != '[') {
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
        /// Модификация от 22 октября 2015 года
        /// Заложен 31 августа 2015 года
        /// </summary>
        /// <param name="attrolist">Набор предустановленных атрибутов</param>
        /// <param name="intellipredolist">Набор предустановленных заготовок для интеллектуального обновления значений атрибутов
        /// Готовится на основании анализа лога</param>
        /// <returns></returns>
        public bool FullListControl(List<string> attrolist, List<string> intellipredolist) {
            List<Gamo> problemolist = null;
            bool reto = false;

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
                aa.MovaControlling();
                progo = aa.CreateMovaRegionWithoutComments();
                for (int i = 0; i < progo.Count; i++) {
                    string vanProgo = progo[i];
                    string twoFailo = aa.pgnMoveRegione[i];
                    if (vanProgo != twoFailo) { //Пишем сообщение в лог
                        string bblogo = string.Format("{0}-{1} Отличие:файл-прого${2}${3}$", aa.GamerWhite, aa.GamerBlack, twoFailo, vanProgo);
                        logomesso.Add(bblogo);
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
            return reto;
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
        Remark
        }

    public enum gmKorrAtr {
        AllOK,
        EventDateHasAnswer,
        Unknown
        }
#endregion-----------------------------ENUM------------------------

    }
