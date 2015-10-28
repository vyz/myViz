using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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

        public pozo() { 
            pBoard = new Pieco[64];
            qava = -1;
            pzu = null;
            controllomova = null;
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
        /// Модификация от 22 июля 2015 года
        /// Заложен май 2015 года
        /// </summary>
        private void AvailableFill() {
            pzu = new PozoUtils(pBoard, whitomv, rokko, enpasso, enfield);
            limov = pzu.MovaFill();
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
        public static pozo Starto()
        {
            pozo reto = new pozo();
            reto.startoinito();
            return reto;
        }

        public Pieco[] BoardoSet { get { return pBoard; } }
        public bool IsQueryMoveWhite { get { return whitomv; } }
        public int NumberMove { get { return numbero; } }
        }

    }
    