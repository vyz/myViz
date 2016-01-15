using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;


namespace pfVisualisator
{
    public class pozo {
        /// <summary>
        /// Массив полей доски со значениями фигур
        /// 63 62 61 60 59 58 57 56
        /// 55 54 53 52 51 50 49 48
        /// 47 46 45 44 43 42 41 40
        /// 39 38 37 36 35 34 33 32
        /// 31 30 29 28 27 26 25 24
        /// 23 22 21 20 19 18 17 16
        /// 15 14 13 12 11 10 9  8
        /// 7  6  5  4  3  2  1  0
        /// </summary>
        private Pieco[] pBoard;
        private long bazaId;
        private bool whitomv;
        private Caslo rokko;
        private bool enpasso;
        /// <summary>
        /// Это третий, либо шестой ряд, в случае истины enpasso.
        /// Поэтому значения от 0 до 15.
        /// </summary>
        private int enfield;
        private int pustomv;
        private int numbero;
        private PozoUtils pzu;
        private Mova controllomova;
        /// <summary>
        /// Расчётная величина. Может быть и 0, но не меньше. -1 - показатель отсутствия расчёта.
        /// </summary>
        private int qava;
        private List<Mova> limov;
        private bool ForBaseSavingFlag;
        private Guid[] bsg;
        private int bstwa;
        private int setablack;
        private int setawhite;

        public pozo() { 
            pBoard = new Pieco[64];
            qava = -1;
            pzu = null;
            controllomova = null;
            ForBaseSavingFlag = false;
            }

        /// <summary>
        /// Модификация от 29 июля 2015 года
        /// Заложен 29 июля 2015 года
        /// </summary>
        /// <param name="ppb"></param>
        /// <param name="coloro"></param>
        /// <param name="prok"></param>
        /// <param name="penp"></param>
        /// <param name="penf"></param>
        /// <param name="ppmv"></param>
        /// <param name="pnum"></param>
        public pozo(Pieco[] ppb, bool coloro, Caslo prok, bool penp, int penf, int ppmv, int pnum) {
            pBoard = new Pieco[64];
            for (int i = 0; i < 64; i++) {
                pBoard[i] = ppb[i];
                }
            whitomv = coloro;
            rokko = prok;
            enpasso = penp;
            enfield = penf;
            pustomv = ppmv;
            numbero = pnum;
            qava = -1;
            pzu = null;
            controllomova = null;
            }

        /// <summary>
        /// Модификация от 10 декабря 2015 года
        /// Заложен 10 декабря 2015 года
        /// </summary>
        /// <param name="van"></param>
        /// <param name="twa"></param>
        public pozo(Guid[] van, int twa) : this() {
            BoardFromGvido(van);
            PartFenFromInto(twa);
            }

        /// <summary>
        /// Модификация от 14 декабря 2015 года
        /// Заложен 11 декабря 2015 года
        /// </summary>
        /// <param name="feno"></param>
        public pozo(string feno) : this() {
            string[] masso = feno.Split(' ');
            BoardFromFeno(masso[0]);
            PropertyFromFeno(masso);
            }

        /// <summary>
        /// Модификация от 24 июля 2015 года
        /// Заложен 24 июля 2015 года
        /// </summary>
        /// <param name="aa"></param>
        /// <returns></returns>
        public bool ContraMov(string aa) {
            //AvailableFill();
            if (null == pzu) {
                pzu = new PozoUtils(pBoard, whitomv, rokko, enpasso, enfield);
                }
            controllomova = pzu.MovaProverka(aa);
            bool reto = false;
            if( null != controllomova ) {
                reto = true;
                }
            return reto;
            }

        /// <summary>
        /// Модификация от 29 июля 2015 года
        /// Заложен 26 июля 2015 года
        /// </summary>
        /// <returns></returns>
        public Mova GetFactMoveFilled()
        {
            Mova reto = null;
            if (null != controllomova) {
                controllomova.FormShortoString(pzu);
                reto = controllomova;
                }
            return reto;
        }

        /// <summary>
        /// Модификация от 30 июля 2015 года
        /// Заложен 29 июля 2015 года
        /// </summary>
        /// <returns></returns>
        public pozo GetPozoAfterControlMove() {
            pozo reto = null;
            PozoUtils tmp = new PozoUtils(pBoard, !whitomv, controllomova.FromField, controllomova.ToField, controllomova.Figura, controllomova.MvType, false);
            Caslo cafmv = tmp.RokkoChangeAfterMove(rokko, controllomova);
            int zenpass = tmp.CanEnpasso(controllomova);
            bool benpass = (zenpass >= 0);
            if (benpass) { //Превращение из нормального целого поля в специальное короткое для позиции
                zenpass -= (zenpass < 24) ? 16 : 40;
            } else {
                zenpass = 0;
                }
            int ppustomv = pustomv + 1;
            if( (controllomova.Figura & Pieco.PieceMask) == Pieco.Pawn || (controllomova.MvType & MovoTypo.PieceEaten) > 0 ) { ppustomv = 0; }
            reto = new pozo(tmp.Boardo, tmp.Coloro, cafmv, benpass, zenpass, ppustomv, numbero + (tmp.Coloro ? 1 : 0));
            return reto;
            }

        /// <summary>
        /// Модификация от 27 мая 2015 года
        /// Заложен 27 мая 2015 года
        /// </summary>
        private void startoinito() {
            for(int i = 47; i >= 16; i--) {
                pBoard[i] = Pieco.None;
                }
            for (int i = 55; i >= 48; i--) {
                pBoard[i] = Pieco.Pawn | Pieco.Black;
                }
            for (int i = 15; i >= 8; i--) {
                pBoard[i] = Pieco.Pawn | Pieco.White;
                }
            pBoard[63] = pBoard[56] = Pieco.Rook | Pieco.Black;
            pBoard[62] = pBoard[57] = Pieco.Knight | Pieco.Black;
            pBoard[61] = pBoard[58] = Pieco.Bishop | Pieco.Black;
            pBoard[60] = Pieco.Queen | Pieco.Black;
            pBoard[59] = Pieco.King | Pieco.Black;
            pBoard[7] = pBoard[0] = Pieco.Rook | Pieco.White;
            pBoard[6] = pBoard[1] = Pieco.Knight | Pieco.White;
            pBoard[5] = pBoard[2] = Pieco.Bishop | Pieco.White;
            pBoard[4] = Pieco.Queen | Pieco.White;
            pBoard[3] = Pieco.King | Pieco.White;
            whitomv = true;
            rokko = Caslo.KingWhite | Caslo.QueenWhite | Caslo.KingBlack | Caslo.QueenBlack;
            enpasso = false;
            enfield = 0;
            pustomv = 0;
            numbero = 1;
            }

        /// <summary>
        /// Модификация от 28 мая 2015 года
        /// Заложен 28 мая 2015 года
        /// </summary>
        /// <param name="k"></param>
        /// <returns></returns>
        private char ForPrintoFigure(int k) {
            char reto = '-';
            bool bf = (pBoard[k] & Pieco.Black) > 0;
            if (pBoard[k] > 0) {
                switch (pBoard[k] & Pieco.PieceMask) {
                    case Pieco.Pawn:
                        reto = bf ? 'p' : 'P';
                        break;
                    case Pieco.Knight:
                        reto = bf ? 'n' : 'N';
                        break;
                    case Pieco.Bishop:
                        reto = bf ? 'b' : 'B';
                        break;
                    case Pieco.Rook:
                        reto = bf ? 'r' : 'R';
                        break;
                    case Pieco.Queen:
                        reto = bf ? 'q' : 'Q';
                        break;
                    case Pieco.King:
                        reto = bf ? 'k' : 'K';
                        break;
                    default:
                        reto = '?';
                        break;
                    }
                }
            return reto;
            }

        /// <summary>
        /// Модификация от 11 декабря 2015 года
        /// Заложен 11 декабря 2015 года
        /// </summary>
        /// <param name="aa"></param>
        /// <returns></returns>
        private Pieco PiecoFromSymbol(char aa) {
            Pieco reto = Pieco.None;
            switch (aa) {
                    case 'P':
                        reto = Pieco.Pawn;
                        break;
                    case 'p':
                        reto = Pieco.Pawn | Pieco.Black;
                        break;
                    case 'N':
                        reto = Pieco.Knight;
                        break;
                    case 'n':
                        reto = Pieco.Knight | Pieco.Black;
                        break;
                    case 'B':
                        reto = Pieco.Bishop;
                        break;
                    case 'b':
                        reto = Pieco.Bishop | Pieco.Black;
                        break;
                    case 'R':
                        reto = Pieco.Rook;
                        break;
                    case 'r':
                        reto = Pieco.Rook | Pieco.Black;
                        break;
                    case 'Q':
                        reto = Pieco.Queen;
                        break;
                    case 'q':
                        reto = Pieco.Queen | Pieco.Black;
                        break;
                    case 'K':
                        reto = Pieco.King;
                        break;
                    case 'k':
                        reto = Pieco.King | Pieco.Black;
                        break;
                    default:
                        reto = Pieco.None;
                        break;
                    }
            return reto;
            }

        /// <summary>
        /// Модификация от 28 мая 2015 года
        /// Заложен 28 мая 2015 года
        /// </summary>
        /// <returns></returns>
        private string ForPrintoCaslo() {
            StringBuilder sbret = new StringBuilder(4);
            if (rokko == Caslo.None) {
                sbret.Append('-');
            } else {
                if ((rokko & Caslo.KingWhite) == Caslo.KingWhite) { sbret.Append('K'); }
                if ((rokko & Caslo.QueenWhite) == Caslo.QueenWhite) { sbret.Append('Q'); }
                if ((rokko & Caslo.KingBlack) == Caslo.KingBlack) { sbret.Append('k'); }
                if ((rokko & Caslo.QueenBlack) == Caslo.QueenBlack) { sbret.Append('q'); }
                }
            return sbret.ToString();
            }

        /// <summary>
        /// Модификация от 21 июля 2015 года
        /// Заложен 28 мая 2015 года
        /// </summary>
        /// <returns></returns>
        private string ForPrintoEnPaso() {
            StringBuilder sbret = new StringBuilder(2);
            if ( enpasso ) {
                int l = enfield % 8;
                char aa = Convert.ToChar('h' - l);
                sbret.Append(aa);
                aa = (enfield >= 8) ? '6' : '3';
                sbret.Append(aa);
            } else {
                sbret.Append('-');
                }
            return sbret.ToString();
            }

        /// <summary>
        /// Модификация от 16 декабря 2015 года
        /// Заложен май 2015 года
        /// </summary>
        public void AvailableFill() {
            pzu = new PozoUtils(pBoard, whitomv, rokko, enpasso, enfield);
            limov = pzu.MovaFill();
            foreach (Mova aa in limov) {
                aa.FormShortoString(pzu);
                }
            qava = limov.Count;
            CalculateSeta();
            }

        /// <summary>
        /// Модификация от 10 декабря 2015 года
        /// Заложен 10 декабря 2015 года
        /// </summary>
        /// <returns>Массив из двух Гвидов</returns>
        private Guid[] BoardToGvid() {
            Guid[] reto = new Guid[2];
            StringBuilder a1 = new StringBuilder(36);
            StringBuilder a2 = new StringBuilder(36);

            for (int i = 0; i < 32; i++) {
                if( i == 8 || i == 12 || i == 16 || i == 20 ) {
                    a1.Append('-');
                    a2.Append('-');
                    }
                byte aa = (byte)pBoard[i * 2];
                byte bb = (byte)pBoard[i * 2 + 1];
                a1.AppendFormat("{0:X1}",aa);
                a2.AppendFormat("{0:X1}",bb);
                }
            reto[0] = Guid.Parse(a1.ToString());
            reto[1] = Guid.Parse(a2.ToString());
            return reto;
            }

        /// <summary>
        /// Модификация от 16 декабря 2015 года
        /// Заложен 10 декабря 2015 года
        /// </summary>
        /// <param name="aa"></param>
        private void BoardFromGvido(Guid[] aa ) {
            string Patterno = "-";
            string a1 = aa[0].ToString();
            string a2 = aa[1].ToString();
            a1 = Regex.Replace(a1, Patterno, "");
            a2 = Regex.Replace(a2, Patterno, "");
            char[] b1 = a1.ToCharArray();
            char[] b2 = a2.ToCharArray();
            for (int i = 0; i < 32; i++) {
                string z = b1[i].ToString();
                byte k = byte.Parse(z, System.Globalization.NumberStyles.AllowHexSpecifier);
                Pieco pp = (Pieco)k;
                pBoard[i * 2] = pp;
                z = b2[i].ToString();
                k = byte.Parse(z, System.Globalization.NumberStyles.AllowHexSpecifier);
                pp = (Pieco)k;
                pBoard[i * 2 + 1] = pp;
                }
            }

        /// <summary>
        /// Модификация от 11 декабря 2015 года
        /// Заложен 11 декабря 2015 года
        /// </summary>
        /// <param name="aa"></param>
        private void BoardFromFeno(string aa) {
            char[] masso = aa.ToCharArray();
            int k = 63;

            foreach (char bb in masso) {
                if( bb == '/') continue;
                if (bb >= '0' && bb <= '9') {
                    int kp = k - (int)(bb - '0');
                    for (; k > kp; k--) {
                        pBoard[k] = Pieco.None;
                        }
                    continue;
                    }
                pBoard[k--] = PiecoFromSymbol(bb);
                }
            }

        /// <summary>
        /// Модификация от 11 декабря 2015 года
        /// Заложен 11 декабря 2015 года
        /// </summary>
        /// <param name="mss"></param>
        private void PropertyFromFeno(string[] mss) {
            string wrs = mss[1];
            switch (wrs[0]) {
                case 'w':
                    whitomv = true;
                    break;
                case 'b':
                    whitomv = false;
                    break;
                default:
                    throw new VisualisatorException(string.Format("Pozo-PropertyFromFeno, Очередь цвета --{0}--", wrs));
                }
            wrs = mss[2];
            if (wrs == "-") {
                rokko = Caslo.None;
            } else {
                char[] masso = wrs.ToCharArray();
                Caslo zrokko = Caslo.None;
                foreach (char bb in masso) {
                    switch (bb) {
                        case 'K':
                            zrokko |= Caslo.KingWhite;
                            break;
                        case 'k':
                            zrokko |= Caslo.KingBlack;
                            break;
                        case 'Q':
                            zrokko |= Caslo.QueenWhite;
                            break;
                        case 'q':
                            zrokko |= Caslo.QueenBlack;
                            break;
                        default:
                            throw new VisualisatorException(string.Format("Pozo-PropertyFromFeno, Возможность рокировки --{0}--", wrs));
                        }
                    }
                }
            wrs = mss[3];
            if (wrs == "-") {
                enpasso = false;
            } else {
                enpasso = true;
                char[] masso = wrs.ToCharArray();
                int im = (int)('h' - masso[0]);
                if (masso[1] == '6') {
                    im += 8;
                    }
                enfield = im;
                }
            wrs = mss[4];
            pustomv = int.Parse(wrs);
            wrs = mss[5];
            numbero = int.Parse(wrs);
            }

        /// <summary>
        /// Модификация от 10 декабря 2015 года
        /// Заложен 10 декабря 2015 года
        /// </summary>
        /// <returns></returns>
        private int SvdbFwoInto()
        {
            int reto = 0;
            int a1 = numbero << (9 + 4 + 1 + 4 + 1);
            int a2 = pustomv << (4 + 1 + 4 + 1);
            int a3 = (int)rokko << (1 + 4 + 1);
            int a4 = enpasso ? 1 << (4 + 1) : 0;
            int a5 = enfield << 1;
            int a6 = whitomv ? 1 : 0;
            reto = a1 | a2 | a3 | a4 | a5 | a6;
            return reto;
        }

        /// <summary>
        /// Модификация от 10 декабря 2015 года
        /// Заложен 10 декабря 2015 года
        /// </summary>
        /// <param name="aa"></param>
        private void PartFenFromInto( int aa ) {
            int a1 = aa >> (9 + 4 + 1 + 4 + 1);
            int a2 = (aa >> (4 + 1 + 4 + 1)) & 0x1FF;
            int a3 = (aa >> (1 + 4 + 1)) & 0xF;
            int a4 = (aa >> (4 + 1)) & 0x1;
            int a5 = (aa >> 1) & 0xF;
            int a6 = aa & 0x1;
            numbero = a1;
            pustomv = a2;
            rokko = (Caslo)a3;
            enpasso = a4 == 1;
            enfield = a5;
            whitomv = a6 == 1;
            }

        /// <summary>
        /// Модификация от 23 декабря 2015 года
        /// Заложен 23 декабря 2015 года
        /// </summary>
        private void BaseSavingPrepare() {
            bsg = BoardToGvid();
            bstwa = SvdbFwoInto();
            ForBaseSavingFlag = true;
            }

        /// <summary>
        /// Модификация от 13 января 2016 года
        /// Заложен 13 января 2016 года
        /// </summary>
        private void CalculateSeta() {
            this.setawhite = CalculateVanSeto(true);
            this.setablack = CalculateVanSeto(false);
            }

        /// <summary>
        /// Модификация от 13 января 2016 года
        /// Заложен 13 января 2016 года
        /// </summary>
        /// <param name="ForWhite"></param>
        /// <returns></returns>
        private int CalculateVanSeto(bool ForWhite) {
            int reto = 0;
            Pieco aa = ForWhite ? Pieco.White : Pieco.Black;
            int kv = pBoard.Count(F => F == (Pieco.Queen | aa));
            reto = kv;
            kv = pBoard.Count(F => F == (Pieco.Rook | aa));
            reto = reto * 10 + kv;
            kv = pBoard.Count(F => F == (Pieco.Bishop | aa));
            reto = reto * 10 + kv;
            kv = pBoard.Count(F => F == (Pieco.Knight | aa));
            reto = reto * 10 + kv;
            kv = pBoard.Count(F => F == (Pieco.Pawn | aa));
            reto = reto * 10 + kv;
            return reto;
            }

        /// <summary>
        /// Модификация от 28 мая 2015 года
        /// Заложен 25 мая 2015 года
        /// Нотация Форсайта—Эдвардса (FEN)
        ///Поля записи:
        ///Положение фигур со стороны белых. 
        ///   Позиция описывается цифрами и буквами по горизонталям сверху вниз начиная с восьмой горизонтали и заканчивая первой. 
        ///   Расположение фигур на горизонтали записывается слева направо, данные каждой горизонтали разделяются косой чертой /. 
        ///   Белые фигуры обозначаются заглавными буквами. K, Q, R, B, N, P — соответственно белые король, ферзь, ладья, слон, конь, пешка. 
        ///   k, q, r, b, n, p — соответственно чёрные король, ферзь, ладья, слон, конь, пешка. 
        ///   Обозначения фигур взяты из англоязычного варианта алгебраической нотации. 
        ///   Цифра задаёт количество пустых полей на горизонтали, счёт ведётся либо от левого края доски, либо после фигуры (8 означает пустую горизонталь).
        ///Активная сторона: w — следующий ход принадлежит белым, b — следующий ход чёрных.
        ///Возможность рокировки. k — в сторону королевского фланга (короткая), q — в сторону ферзевого фланга (длинная). 
        ///   Заглавными указываются белые. Невозможность рокировки обозначается «-».
        ///Возможность взятия пешки на проходе. Указывается проходимое поле, иначе «-».
        ///Счётчик полуходов. Число полуходов, прошедших с последнего хода пешки или взятия. Используется для определения применения правила 50 ходов.
        ///Номер хода. Любой позиции может быть присвоено любое неотрицательное значение (по умолчанию 1), счётчик увеличивается на 1 после каждого хода чёрных.
        /// </summary>
        /// <returns></returns>
        public string fenout() {
            string reto = null;
            StringBuilder sret = new StringBuilder();
            int k = 63;
            for (int i = 0; i < 8; i++) {
                int z = 0;
                for (int j = 0; j < 8; j++) {
                    if (pBoard[k] == Pieco.None) {
                        z++;
                    } else {
                        if (z > 0) {
                            char aa = Convert.ToChar(z + '0');
                            sret.Append(aa);
                            z = 0;
                            }
                        sret.Append(ForPrintoFigure(k));
                        }
                    k--;
                    }
                if (z > 0) {
                    char bb = Convert.ToChar(z + '0');
                    sret.Append(bb);
                    }
                if (i < 7) {
                    sret.Append('/');
                    }
                }
            reto = string.Format( "{0} {1} {2} {3} {4} {5}", sret.ToString(), (whitomv) ? "w" : "b", ForPrintoCaslo(), ForPrintoEnPaso(), pustomv.ToString().Trim(), numbero.ToString().Trim() );
            return reto;
            }

        /// <summary>
        /// Модификация от 27 мая 2015 года
        /// Заложен 27 мая 2015 года
        /// </summary>
        /// <returns></returns>
        public static pozo Starto() {
            pozo reto = new pozo();
            reto.startoinito();
            return reto;
            }

        /// <summary>
        /// Модификация от 11 декабря 2015 года
        /// Заложен 11 декабря 2015 года
        /// </summary>
        /// <returns></returns>
        public static pozo SluchaynoPozo() {
            string aa = "3rk2r/2q1bpp1/p2p1n2/2pP1n1p/PpN2B2/3P2P1/1PP1Q1BP/4RRK1 b k - 0 22";
            pozo reto = new pozo(aa);
            return reto;
            }

        public Pieco[] BoardoSet { get { return pBoard; } }
        public bool IsQueryMoveWhite { get { return whitomv; } }
        public int NumberMove { get { return numbero; } }
        public Guid[] VanBoardo { get {
            if (!ForBaseSavingFlag) { BaseSavingPrepare(); }
            return bsg;
            } }
        public int TwaFeno { get {
            if (!ForBaseSavingFlag) { BaseSavingPrepare(); }
            return bstwa;
            } }
        public List<Mova> AvaList { get { return limov; } }
        public int AvaQvo { get { return qava; } }
        public int SetaWhite { get { return setawhite; } }
        public int SetaBlack { get { return setablack; } }
        }

    }
   