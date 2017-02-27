using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace pfVisualisator
{
    public class PozoUtils
    {
        private Pieco[] pBoard;
        private bool whitomv;
        private Caslo rokko;
        private bool enpasso;
        /// <summary>
        /// Это третий, либо шестой ряд, в случае истины enpasso.
        /// Поэтому значения от 0 до 15.
        /// </summary>
        private int enfield;
        /// <summary>
        /// Поле, которое занимает король той стороны, чей сейчас ход - whitomv
        /// </summary>
        private int kingo;

#region------------------------------Статические массивы с возможными наборами полей-----------------------------------------
        /// <summary>Набор возможных диагональных и линейных ходов для каждой клетки</summary>
        static private int[][][] s_pppiCaseMoveDiagLine;
        /// <summary>Набор возможных диагональных ходов для каждой клетки</summary>
        static private int[][][] s_pppiCaseMoveDiagonal;
        /// <summary>Набор возможных линейных ходов для каждой клетки</summary>
        static private int[][][] s_pppiCaseMoveLine;
        /// <summary>Набор возможных ходов конём для каждой клетки</summary>
        static private int[][] s_ppiCaseMoveKnight;
        /// <summary>Набор возможных ходов королём для каждой клетки</summary>
        static private int[][] s_ppiCaseMoveKing;
        /// <summary>Набор возможных взятий чёрной пешкой для каждой клетки</summary>
        static private int[][] s_ppiCaseBlackPawnCanAttackFrom;
        /// <summary>Набор возможных взятий белой пешкой для каждой клетки</summary>
        static private int[][] s_ppiCaseWhitePawnCanAttackFrom;
#endregion---------------------------Статические массивы с возможными наборами полей-----------------------------------------
        static PozoUtils() {
            List<int[]> arrMove;
            arrMove = new List<int[]>(4);
            s_pppiCaseMoveDiagLine = new int[64][][];
            s_pppiCaseMoveDiagonal = new int[64][][];
            s_pppiCaseMoveLine = new int[64][][];
            s_ppiCaseMoveKnight = new int[64][];
            s_ppiCaseMoveKing = new int[64][];
            s_ppiCaseWhitePawnCanAttackFrom = new int[64][];
            s_ppiCaseBlackPawnCanAttackFrom = new int[64][];
            for (int iPos = 0; iPos < 64; iPos++) {
                FillMoves(iPos, arrMove, new int[] { -1, -1, -1, 0, -1, 1, 0, -1, 0, 1, 1, -1, 1, 0, 1, 1 }, true);
                s_pppiCaseMoveDiagLine[iPos] = arrMove.ToArray();
                FillMoves(iPos, arrMove, new int[] { -1, -1, -1, 1, 1, -1, 1, 1 }, true);
                s_pppiCaseMoveDiagonal[iPos] = arrMove.ToArray();
                FillMoves(iPos, arrMove, new int[] { -1, 0, 1, 0, 0, -1, 0, 1 }, true);
                s_pppiCaseMoveLine[iPos] = arrMove.ToArray();
                FillMoves(iPos, arrMove, new int[] { 1, 2, 1, -2, 2, -1, 2, 1, -1, 2, -1, -2, -2, -1, -2, 1 }, false);
                s_ppiCaseMoveKnight[iPos] = arrMove[0];
                FillMoves(iPos, arrMove, new int[] { -1, -1, -1, 0, -1, 1, 0, -1, 0, 1, 1, -1, 1, 0, 1, 1 }, false);
                s_ppiCaseMoveKing[iPos] = arrMove[0];
                FillMoves(iPos, arrMove, new int[] { -1, -1, 1, -1 }, false);
                s_ppiCaseWhitePawnCanAttackFrom[iPos] = arrMove[0];
                FillMoves(iPos, arrMove, new int[] { -1, 1, 1, 1 }, false);
                s_ppiCaseBlackPawnCanAttackFrom[iPos] = arrMove[0];
                }
            }

        /// <summary>
        /// Функция для заполнения массивов статическим конструктором.
        /// Fill the possible move array using the specified delta
        /// </summary>
        /// <param name="iStartPos">    Start position</param>
        /// <param name="arrMove">      Array of move to fill</param>
        /// <param name="arrDelta">     List of delta</param>
        /// <param name="bRepeat">      true to repeat, false to do it once</param>
        static private void FillMoves(int iStartPos, List<int[]> arrMove, int[] arrDelta, bool bRepeat)
        {
            int iColPos;
            int iRowPos;
            int iColIndex;
            int iRowIndex;
            int iColOfs;
            int iRowOfs;
            int iPosOfs;
            int iNewPos;
            List<int> arrMoveOnLine;

            arrMove.Clear();
            arrMoveOnLine = new List<int>(8);
            iColPos = iStartPos & 7;
            iRowPos = iStartPos >> 3;
            for (int iIndex = 0; iIndex < arrDelta.Length; iIndex += 2)
            {
                iColOfs = arrDelta[iIndex];
                iRowOfs = arrDelta[iIndex + 1];
                iPosOfs = iRowOfs * 8 + iColOfs;
                iColIndex = iColPos + iColOfs;
                iRowIndex = iRowPos + iRowOfs;
                iNewPos = iStartPos + iPosOfs;
                if (bRepeat)
                {
                    arrMoveOnLine.Clear();
                    while (iColIndex >= 0 && iColIndex < 8 && iRowIndex >= 0 && iRowIndex < 8)
                    {
                        arrMoveOnLine.Add(iNewPos);
                        if (bRepeat)
                        {
                            iColIndex += iColOfs;
                            iRowIndex += iRowOfs;
                            iNewPos += iPosOfs;
                        }
                        else
                        {
                            iColIndex = -1;
                        }
                    }
                    if (arrMoveOnLine.Count != 0)
                    {
                        arrMove.Add(arrMoveOnLine.ToArray());
                    }
                }
                else if (iColIndex >= 0 && iColIndex < 8 && iRowIndex >= 0 && iRowIndex < 8)
                {
                    arrMoveOnLine.Add(iNewPos);
                }
            }
            if (!bRepeat)
            {
                arrMove.Add(arrMoveOnLine.ToArray());
            }
        }

        public PozoUtils() { }

        public PozoUtils(Pieco[] pp, bool pcolor, Caslo pr, bool v, int penp) {
            pBoard = pp;
            whitomv = pcolor;
            rokko = pr;
            enpasso = v;
            enfield = penp;
            }

        /// <summary>
        /// Модификация от 26 августа 2015 года
        /// Заложен июнь 2015 года
        /// Функция перегружена из-за неоднозначности передаваемого цвета
        /// </summary>
        /// <param name="pp"></param>
        /// <param name="pcolor">Новый цвет - создаваемой уже после хода позиции
        /// Но это не всегда. При вызове из функции IsAvailableMove цвет старый и вся эта позиция нужна только
        /// для проверки на неполучение шаха после сделанного хода</param>
        /// <param name="pif"></param>
        /// <param name="pit"></param>
        /// <param name="fipa"></param>
        /// <param name="pmt"></param>
        /// <param name="SvoyChuzhoy">Цвет свой (true) или чужой для хода</param>
        public PozoUtils(Pieco[] pp, bool pcolor, int pif, int pit, Pieco fipa, MovoTypo pmt, bool SvoyChuzhoy) {
            pBoard = new Pieco[64];
            Pieco pkinga = pcolor ? Pieco.King : (Pieco.King | Pieco.Black);
            for (int i = 63; i >= 0; i--) {
                if (pp[i] == pkinga) { kingo = i; }
                pBoard[i] = pp[i];
                }
            if (fipa == pkinga) { kingo = pit; } //Предположительно неразумная вставка.  Условие никогда не может быть выполнено. Ха ха. Цвета разные.
            //так как короли разные у фигуры еще прошлый король, а у pkinga уже новый.
            pBoard[pif] = Pieco.None;
            pBoard[pit] = fipa;
            if ((pmt & MovoTypo.EnPassant) > 0) {
                int oldpawn = 0;
                if (SvoyChuzhoy)
                {
                    oldpawn = pit + (pcolor ? -8 : 8);
                }
                else
                {
                    oldpawn = pit + (pcolor ? 8 : -8);
                }
                pBoard[oldpawn] = Pieco.None;
                }
            if (pmt == MovoTypo.Castle) { //Страшная хардкорщина. Но на самом деле нормально.
                switch (pit) {
                    case 1:
                        pBoard[2] = pBoard[0];
                        pBoard[0] = Pieco.None;
                        break;
                    case 5:
                        pBoard[4] = pBoard[7];
                        pBoard[7] = Pieco.None;
                        break;
                    case 57:
                        pBoard[58] = pBoard[56];
                        pBoard[56] = Pieco.None;
                        break;
                    case 61:
                        pBoard[60] = pBoard[63];
                        pBoard[63] = Pieco.None;
                        break;
                    }
                }
            bool prevrashenye = (SvoyChuzhoy) ? !pcolor : pcolor;
            if ((pmt & MovoTypo.MoveTypeMask) == MovoTypo.PawnPromotionToQueen) { pBoard[pit] = Pieco.Queen | (prevrashenye ? Pieco.Black : Pieco.White); }
            else if ((pmt & MovoTypo.MoveTypeMask) == MovoTypo.PawnPromotionToRook) { pBoard[pit] = Pieco.Rook | (prevrashenye ? Pieco.Black : Pieco.White); }
            else if ((pmt & MovoTypo.MoveTypeMask) == MovoTypo.PawnPromotionToKnight) { pBoard[pit] = Pieco.Knight | (prevrashenye ? Pieco.Black : Pieco.White); }
            else if ((pmt & MovoTypo.MoveTypeMask) == MovoTypo.PawnPromotionToBishop) { pBoard[pit] = Pieco.Bishop | (prevrashenye ? Pieco.Black : Pieco.White); }

            if (fipa == (Pieco.Pawn | Pieco.White) && ((pif - pit) == 16)) {
                if ((pit % 8 > 0 && pBoard[pit - 1] == (Pieco.Pawn | Pieco.Black)) ||
                    (pit % 8 < 7 && pBoard[pit + 1] == (Pieco.Pawn | Pieco.Black))) {
                    enpasso = true;
                    enfield = pit - 24;
                    }
            } else if (fipa == (Pieco.Pawn | Pieco.Black) && (pif - pit == 16)) {
                if ((pit % 8 > 0 && pBoard[pit - 1] == (Pieco.Pawn | Pieco.White)) ||
                    (pit % 8 < 7 && pBoard[pit + 1] == (Pieco.Pawn | Pieco.White))) {
                    enpasso = true;
                    enfield = pit - 24;
                    }
                }
            whitomv = pcolor;
            }

        /// <summary>
        /// Получить список возможных ходов
        /// Модификация от 21 июля 2015 года
        /// Заложен 21 июля 2015 года
        /// </summary>
        /// <returns>NULL - если пустой список (пат или мат).</returns>
        public List<Mova> MovaFill()
        {
            List<Mova> reto = new List<Mova>();
            Pieco figura;
            for (int iIndex = 0; iIndex < 64; iIndex++)
            {
                List<Mova> culimo = null;
                figura = pBoard[iIndex];
                if (figura != Pieco.None && ((figura & Pieco.Black) == 0) == whitomv)
                {
                    switch (figura & Pieco.PieceMask)
                    {
                        case Pieco.Pawn:
                            culimo = MovaPawn(iIndex);
                            break;
                        case Pieco.Knight:
                            culimo = MovaFromVanArray(iIndex, s_ppiCaseMoveKnight[iIndex]);
                            break;
                        case Pieco.Bishop:
                            culimo = MovaFromTwoArray(iIndex, s_pppiCaseMoveDiagonal[iIndex]);
                            break;
                        case Pieco.Rook:
                            culimo = MovaFromTwoArray(iIndex, s_pppiCaseMoveLine[iIndex]);
                            break;
                        case Pieco.Queen:
                            culimo = MovaFromTwoArray(iIndex, s_pppiCaseMoveDiagLine[iIndex]);
                            break;
                        case Pieco.King:
                            culimo = MovaFromVanArray(iIndex, s_ppiCaseMoveKing[iIndex]);
                            break;
                        default:
                            break;
                    }
                    if (culimo != null)
                    {
                        reto.AddRange(culimo);
                    }
                }
            }
            List<Mova> splimo = CastleMoveList();
            if (splimo != null)
            {
                reto.AddRange(splimo);
            }
            splimo = EnPassantMoveList();
            if (splimo != null)
            {
                reto.AddRange(splimo);
            }
            if (reto.Count == 0) { reto = null; }
            return reto;
        }

        /// <summary>
        /// Модификация от 28 января 2016 года
        /// Заложен 24 июля 2015 года
        /// </summary>
        /// <param name="pst"></param>
        /// <returns></returns>
        public Mova MovaProverka(string pst, int typostring) {
            Mova reto = typostring == 1 ? Decode(pst) : DecodeFromTo(pst);
            return reto;
            }

        /// <summary>
        /// Модификация от 22 октября 2015 года
        /// Заложен 24 июля 2015 года
        /// </summary>
        /// <param name="pst"></param>
        /// <returns></returns>
        private Mova Decode(string parst) {
            Mova reto = null;

            string pst = parst;
            bool Zapret = false;
            bool Neodno = false;
            string Neodnostr = string.Empty;
            MovoTypo eMoveType = MovoTypo.Normal;
            Pieco Figura = Pieco.None;
            int pf = 0, pt = 0;
            pst = pst.Replace("#", "").Replace("+", "");
            if (pst == "O-O") {
                eMoveType = MovoTypo.Castle;
                if (whitomv) {
                    if ((rokko & Caslo.KingWhite) > 0) {
                        pf = 3; pt = 1;
                    } else {
                        Zapret = true;
                        }
                } else {
                    if ((rokko & Caslo.KingBlack) > 0) {
                        pf = 59; pt = 57;
                    } else {
                        Zapret = true;
                        }
                    }
            } else if (pst == "O-O-O") {
                eMoveType = MovoTypo.Castle;
                if (whitomv) {
                    if ((rokko & Caslo.QueenWhite) > 0) {
                        pf = 3; pt = 5;
                    } else {
                        Zapret = true;
                        }
                } else {
                    if ((rokko & Caslo.QueenBlack) > 0) {
                        pf = 59; pt = 61;
                    } else {
                        Zapret = true;
                        }
                    }
            } else {
                bool Prevra = false;
                string sPrerva = "Q";
                int iIndex = pst.IndexOf('=');
                if (iIndex != -1) { //Это стандартный метод
                    Prevra = true;
                    sPrerva = pst.Substring(iIndex + 1, 1);
                    pst = pst.Substring(0, pst.Length - 2);
                } else {
                    string patOther = @"([18])([QRBN])";
                    if (Regex.IsMatch(pst, patOther)) {
                        Match m1 = Regex.Match(pst, patOther);
                        sPrerva = m1.Groups[2].Value;
                        Prevra = true;
                        pst = Regex.Replace(pst, patOther, "$1");
                        }
                    }
                if (Prevra) {
                    switch (sPrerva) {
                            case "Q":
                                eMoveType = MovoTypo.PawnPromotionToQueen;
                                break;
                            case "R":
                                eMoveType = MovoTypo.PawnPromotionToRook;
                                break;
                            case "B":
                                eMoveType = MovoTypo.PawnPromotionToBishop;
                                break;
                            case "N":
                                eMoveType = MovoTypo.PawnPromotionToKnight;
                                break;
                            default:
                                Zapret = true;
                                break;
                        }
                        
                    }
                
                int ip = 1;
                switch (pst[0]) {
                    case 'K':
                        Figura = Pieco.King;
                        break;
                    case 'N':
                        Figura = Pieco.Knight;
                        break;
                    case 'B':
                        Figura = Pieco.Bishop;
                        break;
                    case 'R':
                        Figura = Pieco.Rook;
                        break;
                    case 'Q':
                        Figura = Pieco.Queen;
                        break;
                    default:
                        Figura = Pieco.Pawn;
                        ip = 0;
                        break;
                    }
                pst = pst.Substring(ip);
                if (pst.Contains("x")) {
                    eMoveType |= MovoTypo.PieceEaten;
                    pst = pst.Replace("x", "");
                    }
                if (pst.Length > 2) {
                    Neodno = true;
                    Neodnostr = pst.Substring(0, 1);
                    pst = pst.Substring(1);
                    }
                if (pst.Length > 2) {
                    throw new VisualisatorException(string.Format("PozoUtils-Decode, pst.Length > 2 --{0}--", parst));
                    }
                pt = ConvertStrFieldtoInt(pst);
                //Из pgn-файла взятие на проходе просто так не читается.
                //Поэтому выполним простую проверку.
                if ((eMoveType & MovoTypo.PieceEaten) > 0 && (Figura & Pieco.PieceMask) == Pieco.Pawn && pBoard[pt] == Pieco.None)
                {
                    //Набор этих трех условий должен однозначно определять взятие на проходе.
                    eMoveType |= MovoTypo.EnPassant;
                }
                List<int> lifi = ListFieldsFromAttackedThisField(Figura, pt, eMoveType);
                if (lifi.Count == 0) { //Невозможное случается - Конкретно для автодосок Король на d5 и под шах :(
                    return null;
                } else if (lifi.Count > 1) {
                    if (Neodno) {
                        if (Neodnostr[0] >= 'a' && Neodnostr[0] <= 'h') {
                            int icol = (int)('h' - Neodnostr[0]);
                            Zapret = true;
                            foreach (int ii in lifi) {
                                if (ii % 8 == icol) {
                                    pf = ii;
                                    Zapret = false;
                                    break;
                                    }
                                }
                        } else if (Neodnostr[0] >= '1' && Neodnostr[0] <= '8') {
                            int irow = (int)(Neodnostr[0] - '1');
                            Zapret = true;
                            foreach (int ii in lifi) {
                                if (ii / 8 == irow) {
                                    pf = ii;
                                    Zapret = false;
                                    break;
                                    }
                                }
                        } else {
                            throw new VisualisatorException("PozoUtils-Decode, неправильный формат указания нального поля хода");
                            }
                    } else {
                        throw new VisualisatorException(string.Format("PozoUtils-Decode, неопределенность начального поля может не одна, но указаний нет --{0}--", parst));
                        }
                } else {
                    pf = lifi[0];
                    }
                }
            if( Zapret ) {
                throw new VisualisatorException("PozoUtils, нарисовался ЗАПРЕТ");
                }
            reto = AddMoveIsNotCheck(pf, pt, eMoveType);
            return reto;
            }

        /// <summary>
        /// Модификация от 26 февраля 2017 года
        /// Заложен 27 января 2016 года
        /// </summary>
        /// <param name="fromto"></param>
        /// <returns></returns>
        private Mova DecodeFromTo(string fromto) {
            Mova reto = null;
            MovoTypo eMoveType = MovoTypo.Normal;
            Pieco Figura = Pieco.None;
            int froma = ConvertStrFieldtoInt(fromto);
            int toma = ConvertStrFieldtoInt(fromto.Substring(2,2));
            Figura = pBoard[froma];
            bool Zapret = false;
            string ZapretDescription = string.Empty;
            if( Figura == Pieco.King && froma == 3 ) {
                if (toma == 1) {
                    if ((rokko & Caslo.KingWhite) > 0) {
                        eMoveType = MovoTypo.Castle;
                    } else { 
                        Zapret = true; 
                        }
                } else if (toma == 5) {
                    if ((rokko & Caslo.QueenWhite) > 0) {
                        eMoveType = MovoTypo.Castle;
                    } else { 
                        Zapret = true; 
                        }      
                    }
            } else if (Figura == (Pieco.King | Pieco.Black) && froma == 59) {
                if (toma == 57) {
                    if ((rokko & Caslo.KingBlack) > 0) {
                        eMoveType = MovoTypo.Castle;
                    } else { 
                        Zapret = true; 
                        }
                } else if (toma == 61) {
                    if ((rokko & Caslo.QueenBlack) > 0) {
                        eMoveType = MovoTypo.Castle;
                    } else { Zapret = true; 
                        }
                    }
                }
            if( (Figura & Pieco.PieceMask) == Pieco.Pawn) { 
                if(toma > 56 || toma < 8) { 
                    //Предполагавшаяся дыра с превращениями. Фигура превращения не влазит в четырехсимвольный формат. Требует будущей проработки 
                    //В реале фигура превращения идёт 5 символом
                    if( fromto.Length > 4 ) {
                        switch(fromto[4]) {
                            case 'q':
                                eMoveType = MovoTypo.PawnPromotionToQueen;
                                break;
                            case 'r':
                                eMoveType = MovoTypo.PawnPromotionToRook;
                                break;
                            case 'b':
                                eMoveType = MovoTypo.PawnPromotionToBishop;
                                break;
                            case 'n':
                                eMoveType = MovoTypo.PawnPromotionToKnight;
                                break;
                            }
                    } else {
                        Zapret = true;
                        ZapretDescription = "Отсутствует фигура превращения! Длина не больше 4";
                        }
                    }
                if( enpasso ) {
                    int popo = (whitomv ? 32 : 16) + enfield;
                    if( toma == popo ) {
                        eMoveType = MovoTypo.EnPassant | MovoTypo.PieceEaten;
                        }
                    }
                }
            if (pBoard[toma] != Pieco.None) {
                eMoveType |= MovoTypo.PieceEaten;
                }
            if (Zapret) {
                string erre = string.Format("PozoUtils->DecodeFromTo({0}), нарисовался ЗАПРЕТ {1}", fromto, ZapretDescription);
                LogoCM.OutString(erre);
                throw new VisualisatorException(erre);
                }
            reto = AddMoveIsNotCheck(froma, toma, eMoveType);
            reto.FormShortoString(this);
            return reto;
            }

        /// <summary>
        /// Модификация от 29 января 2016 года
        /// Заложен июль 2015 года
        /// </summary>
        /// <param name="pi"></param>
        /// <returns></returns>
        private List<Mova> MovaPawn(int pi) {
            List<Mova> reto = new List<Mova>();
            int iDir;
            int iNewPos;
            int iNewColPos;
            int iRowPos;
            bool bCanMove2Case;

            iRowPos = (pi >> 3);
            bCanMove2Case = (whitomv) ? (iRowPos == 1) : (iRowPos == 6);
            iDir = (whitomv) ? 8 : -8;
            iNewPos = pi + iDir;
            if (pBoard[iNewPos] == Pieco.None) {
                iRowPos = (iNewPos >> 3);
                if( iRowPos == 0 || iRowPos == 7 ) {
                    List<Mova> curli = AddPawnPromotion(pi, iNewPos, MovoTypo.Normal);
                    if (curli != null) {
                        reto.AddRange(curli);
                        }
                } else {
                    Mova aa = AddMoveIsNotCheck(pi, iNewPos, MovoTypo.Normal);
                    if (null != aa) {
                        reto.Add(aa);
                        }
                    }
                if( bCanMove2Case && pBoard[iNewPos + iDir] == Pieco.None ) {
                    Mova aa = AddMoveIsNotCheck(pi, iNewPos + iDir, MovoTypo.Normal);
                    if( null != aa ) {
                        reto.Add(aa);
                        }
                    }
                }
            iNewColPos = iNewPos & 7;
            iRowPos = (iNewPos >> 3);
            if (iNewColPos != 0 && pBoard[iNewPos - 1] != Pieco.None) { //Проверка на взятие вправо
                if (((pBoard[iNewPos - 1] & Pieco.Black) > 0) == whitomv) {
                    if (iRowPos == 0 || iRowPos == 7) { //Взятие и превращение
                        List<Mova> curli = AddPawnPromotion(pi, iNewPos-1, MovoTypo.PieceEaten);
                        if (curli != null) {
                            reto.AddRange(curli);
                            }
                    } else {
                        Mova aa = AddMoveIsNotCheck(pi, iNewPos-1, MovoTypo.PieceEaten);
                        if (null != aa) {
                            reto.Add(aa);
                            }
                        }
                    }
                }
            if (iNewColPos != 7 && pBoard[iNewPos + 1] != Pieco.None) { //Проверка на взятие влево
                if (((pBoard[iNewPos + 1] & Pieco.Black) > 0) == whitomv) {
                    if (iRowPos == 0 || iRowPos == 7) { //Взятие и превращение
                        List<Mova> curli = AddPawnPromotion(pi, iNewPos+1, MovoTypo.PieceEaten);
                        if (curli != null) {
                            reto.AddRange(curli);
                            }
                    } else {
                        Mova aa = AddMoveIsNotCheck(pi, iNewPos+1, MovoTypo.PieceEaten);
                        if (null != aa) {
                            reto.Add(aa);
                            }
                        }
                    }
                }
            if (reto.Count == 0) {
                reto = null;
                }
            return reto;
            }

        /// <summary>
        /// Модификация от 30 января 2016 года
        /// Заложен 20 июля 2015 года
        /// </summary>
        /// <param name="pi"></param>
        /// <param name="piMoveList"></param>
        /// <returns></returns>
        private List<Mova> MovaFromVanArray(int pi, int[] piMoveList) {
            List<Mova> reto = new List<Mova>();
            foreach( int iNewPos in piMoveList ) {
                Mova aa = AddMoveIfEnemyOrEmptyAndNoCheck(pi, iNewPos);
                if (null != aa) {
                    reto.Add(aa);
                    }
                }
            if (reto.Count == 0) {
                reto = null;
                }
            return reto;
            }

        /// <summary>
        /// Модификация от 10 февраля 2017 года
        /// Заложен 20 июля 2015 года
        /// </summary>
        /// <param name="pi"></param>
        /// <param name="ppiMoveList"></param>
        /// <returns></returns>
        private List<Mova> MovaFromTwoArray(int pi, int[][] ppiMoveList) {
            List<Mova> reto = new List<Mova>();
            foreach (int[] piMoveList in ppiMoveList) {
                foreach (int iNewPos in piMoveList) {
                    Mova aa = AddMoveIfEnemyOrEmptyAndNoCheck(pi, iNewPos);
                    if (null != aa) {
                        reto.Add(aa);
                        }
                    if( pBoard[iNewPos] != Pieco.None ) {
                        break;
                        }
                    }
                }
            if (reto.Count == 0) {
                reto = null;
                }
            return reto;
            }

        /// <summary>
        /// Модификация от 20 июля 2015 года
        /// Заложен 20 июля 2015 года
        /// </summary>
        /// <param name="iStartPos"></param>
        /// <param name="iEndPos"></param>
        /// <returns></returns>
        private Mova AddMoveIfEnemyOrEmptyAndNoCheck(int iStartPos, int iEndPos)
        {
            Mova reto = null;

            bool bPusto = (pBoard[iEndPos] == Pieco.None);
            Pieco eOldPiece = pBoard[iEndPos];
            MovoTypo mvt = (bPusto) ? MovoTypo.Normal : MovoTypo.PieceEaten;
            if (bPusto || ((eOldPiece & Pieco.Black) > 0) == whitomv)
            {
                reto = AddMoveIsNotCheck(iStartPos, iEndPos, mvt);
            }
            return reto;
        }

        private Mova AddMoveIsNotCheck(int pif, int pit, MovoTypo pmt)
        {
            Mova reto = null;
            if (IsAvailableMove(pif, pit, pBoard[pif], pmt))
            {
                reto = new Mova(whitomv, pBoard[pif], pif, pit, pmt);
            }
            return reto;
        }

        /// <summary>
        /// Модификация от 21 июля 2015 года
        /// Заложен 2015 год
        /// </summary>
        /// <param name="pif"></param>
        /// <param name="pit"></param>
        /// <param name="addo"></param>
        /// <returns></returns>
        private List<Mova> AddPawnPromotion(int pif, int pit, MovoTypo addo)
        {
            List<Mova> reto = new List<Mova>();
            MovoTypo movtyp = MovoTypo.PawnPromotionToQueen | addo;
            Pieco fipa = whitomv ? Pieco.Queen : (Pieco.Queen | Pieco.Black);
            if (IsAvailableMove(pif, pit, fipa, movtyp))
            {
                Mova aa = new Mova(whitomv, pBoard[pif], pif, pit, movtyp);
                reto.Add(aa);
            }
            movtyp = MovoTypo.PawnPromotionToRook | addo;
            fipa = whitomv ? Pieco.Rook : (Pieco.Rook | Pieco.Black);
            if (IsAvailableMove(pif, pit, fipa, movtyp))
            {
                Mova aa = new Mova(whitomv, pBoard[pif], pif, pit, movtyp);
                reto.Add(aa);
            }
            movtyp = MovoTypo.PawnPromotionToBishop | addo;
            fipa = whitomv ? Pieco.Bishop : (Pieco.Bishop | Pieco.Black);
            if (IsAvailableMove(pif, pit, fipa, movtyp))
            {
                Mova aa = new Mova(whitomv, pBoard[pif], pif, pit, movtyp);
                reto.Add(aa);
            }
            movtyp = MovoTypo.PawnPromotionToKnight | addo;
            fipa = whitomv ? Pieco.Knight : (Pieco.Knight | Pieco.Black);
            if (IsAvailableMove(pif, pit, fipa, movtyp))
            {
                Mova aa = new Mova(whitomv, pBoard[pif], pif, pit, movtyp);
                reto.Add(aa);
            }
            return reto;
        }

        /// <summary>
        /// Список возможных ходов рокировочных
        /// Модификация от 21 июля 2015 года
        /// Заложен 21 июля 2015 года
        /// </summary>
        /// <returns>NULL или список. Пустой список исключен.</returns>
        private List<Mova> CastleMoveList()
        {
            List<Mova> reto = new List<Mova>();
            Mova boba;

            if (whitomv)
            {
                if ((rokko & Caslo.KingWhite) > 0)
                {
                    if (pBoard[1] == Pieco.None && pBoard[2] == Pieco.None)
                    {
                        if (!(IsFieldAttacked(false, 2) || IsFieldAttacked(false, 3)))
                        {
                            boba = AddMoveIsNotCheck(3, 1, MovoTypo.Castle);
                            if (null != boba)
                            {
                                reto.Add(boba);
                            }
                        }
                    }
                }
                if ((rokko & Caslo.QueenWhite) > 0)
                {
                    if (pBoard[4] == Pieco.None && pBoard[5] == Pieco.None && pBoard[6] == Pieco.None)
                    {
                        if (!(IsFieldAttacked(false, 3) || IsFieldAttacked(false, 4)))
                        {
                            boba = AddMoveIsNotCheck(3, 5, MovoTypo.Castle);
                            if (null != boba)
                            {
                                reto.Add(boba);
                            }
                        }
                    }
                }
            }
            else
            {
                if ((rokko & Caslo.KingBlack) > 0)
                {
                    if (pBoard[57] == Pieco.None && pBoard[58] == Pieco.None)
                    {
                        if (!(IsFieldAttacked(true, 58) || IsFieldAttacked(true, 59)))
                        {
                            boba = AddMoveIsNotCheck(59, 57, MovoTypo.Castle);
                            if (null != boba)
                            {
                                reto.Add(boba);
                            }
                        }
                    }
                }
                if ((rokko & Caslo.QueenBlack) > 0)
                {
                    if (pBoard[60] == Pieco.None && pBoard[61] == Pieco.None && pBoard[62] == Pieco.None)
                    {
                        if (!(IsFieldAttacked(true, 59) || IsFieldAttacked(true, 60)))
                        {
                            boba = AddMoveIsNotCheck(59, 61, MovoTypo.Castle);
                            if (null != boba)
                            {
                                reto.Add(boba);
                            }
                        }
                    }
                }
            }
            if (reto.Count == 0)
            {
                reto = null;
            }
            return reto;
        }

        /// <summary>
        /// Список возможных (именно так, бывает и два) взятий на проходе
        /// Модификация от 22 июля 2015 года
        /// Заложен 21 июля 2015 года
        /// </summary>
        /// <returns>NULL или список. Пустой список исключен.</returns>
        private List<Mova> EnPassantMoveList()
        {
            List<Mova> reto = new List<Mova>();
            Mova boba;

            if (enpasso)
            {
                int fildo = (whitomv ? 32 : 16) + enfield;
                int enemypawn = 24 + enfield;
                int iColPos = fildo % 8;
                Pieco friend = whitomv ? Pieco.Pawn : (Pieco.Pawn | Pieco.Black);
                if (iColPos > 0 && pBoard[enemypawn - 1] == friend)
                {
                    boba = AddMoveIsNotCheck(enemypawn - 1, fildo, MovoTypo.EnPassant | MovoTypo.PieceEaten);
                    if (null != boba)
                    {
                        reto.Add(boba);
                    }
                }
                if (iColPos < 7 && pBoard[enemypawn + 1] == friend)
                {
                    boba = AddMoveIsNotCheck(enemypawn + 1, fildo, MovoTypo.EnPassant | MovoTypo.PieceEaten);
                    if (null != boba)
                    {
                        reto.Add(boba);
                    }
                }
            }
            if (reto.Count == 0)
            {
                reto = null;
            }
            return reto;
        }

        /// <summary>
        /// Модификация от 28 января 2016 года
        /// Заложен 24 июля 2015 года
        /// </summary>
        /// <param name="fieldo"></param>
        /// <returns></returns>
        static private int ConvertStrFieldtoInt(string fieldo)
        {
            int reto;
            int van = (int)('h' - fieldo[0]);
            int two = (int)(fieldo[1] - '1');
            reto = two * 8 + van;
            return reto;
        }

        private bool IsAvailableMove(int pif, int pit, Pieco fipa, MovoTypo pmt)
        {
            bool reto = false;

            PozoUtils nnp = new PozoUtils(pBoard, whitomv, pif, pit, fipa, pmt, true);
            reto = nnp.IsNoCheck();
            return reto;
        }

        /// <summary>
        /// Проверка поля на атакованность
        /// Модификация от 21 июля 2015 года
        /// Заложен 21 июля 2015 года
        /// </summary>
        /// <param name="whoom">Какой цвет будет атаковать</param>
        /// <param name="fildo">Проверяемое на битость поле</param>
        /// <returns>Истина - когда есть желающие побить</returns>
        private bool IsFieldAttacked(bool whoom, int fildo)
        {
            bool reto = false;
            Pieco cola = whoom ? Pieco.White : Pieco.Black;

            Pieco enemyvan = Pieco.Queen | cola;
            Pieco enemytwo = Pieco.Bishop | cola;
            if (CheckFromTwoPeace(s_pppiCaseMoveDiagonal[fildo], enemyvan, enemytwo)) { reto = true; return reto; }
            enemytwo = Pieco.Rook | cola;
            if (CheckFromTwoPeace(s_pppiCaseMoveLine[fildo], enemyvan, enemytwo)) { reto = true; return reto; }
            enemyvan = Pieco.King | cola;
            if (CheckFromOnePeace(s_ppiCaseMoveKing[fildo], enemyvan)) { reto = true; return reto; }
            enemyvan = Pieco.Knight | cola;
            if (CheckFromOnePeace(s_ppiCaseMoveKnight[fildo], enemyvan)) { reto = true; return reto; }
            enemyvan = Pieco.Pawn | cola;
            if (CheckFromOnePeace(whoom ? s_ppiCaseWhitePawnCanAttackFrom[fildo] : s_ppiCaseBlackPawnCanAttackFrom[fildo], enemyvan)) { reto = true; return reto; }
            return reto;
        }

        public bool IsNoCheck()
        {
            bool reto = true;
            Pieco enemyvan;
            Pieco enemytwo;

            enemyvan = whitomv ? (Pieco.Queen | Pieco.Black) : Pieco.Queen;
            enemytwo = whitomv ? (Pieco.Bishop | Pieco.Black) : Pieco.Bishop;
            if (CheckFromTwoPeace(s_pppiCaseMoveDiagonal[kingo], enemyvan, enemytwo)) { reto = false; return reto; }
            enemytwo = whitomv ? (Pieco.Rook | Pieco.Black) : Pieco.Rook;
            if (CheckFromTwoPeace(s_pppiCaseMoveLine[kingo], enemyvan, enemytwo)) { reto = false; return reto; }
            enemyvan = whitomv ? (Pieco.King | Pieco.Black) : Pieco.King;
            if (CheckFromOnePeace(s_ppiCaseMoveKing[kingo], enemyvan)) { reto = false; return reto; }
            enemyvan = whitomv ? (Pieco.Knight | Pieco.Black) : Pieco.Knight;
            if (CheckFromOnePeace(s_ppiCaseMoveKnight[kingo], enemyvan)) { reto = false; return reto; }
            enemyvan = whitomv ? (Pieco.Pawn | Pieco.Black) : Pieco.Pawn;
            if (CheckFromOnePeace(whitomv ? s_ppiCaseBlackPawnCanAttackFrom[kingo] : s_ppiCaseWhitePawnCanAttackFrom[kingo], enemyvan)) { reto = false; return reto; }
            return reto;
        }

        /// <summary>
        /// Мы уже получили мат?
        /// Модификация от 22 августа 2015 года
        /// Заложен 29 июля 2015 года
        /// </summary>
        /// <returns>Истина в случае реального мата</returns>
        public bool IsCheckmated() {
            bool reto = false;
            if (!IsNoCheck()) {
                reto = true;
                //Из под шаха можно уйти
                foreach (int proba in s_ppiCaseMoveKing[kingo]) {
                    MovoTypo pmt = MovoTypo.Normal;
                    bool testomove = false;
                    if (pBoard[proba] == Pieco.None) { testomove = true; }
                    if (!testomove) {
                        bool probafigocolor = (pBoard[proba] & Pieco.Black) > 0; //Черная - истина 
                        if (probafigocolor == whitomv) {
                            pmt = MovoTypo.PieceEaten;
                            testomove = true;
                            }
                        }
                    if (testomove) {
                        if (IsAvailableMove(kingo, proba, Pieco.King | (whitomv ? Pieco.White : Pieco.Black), pmt)) {
                            reto = false;
                            break;
                            }
                        }
                    }
                if (reto) { //Можно сбить шахующую фигуру, если она одна
                    List<int> aa = ListFeildsFromChecked();
                    if (aa.Count == 1) {
                        reto = !CanEatEnemyOnTheField(aa[0]);
                        if (reto) { //А можно и перекрыться, и тоже только, если она одна
                            reto = !CanPerekrytie(aa[0], kingo);
                            }
                        }
                    }
                }
            return reto;
            }

        /// <summary>
        /// Перечень полей, откуда враги шахуют нас
        /// Модификация от 23 августа 2015 года
        /// Заложен 3 августа 2015 года
        /// </summary>
        /// <returns>Перечень полей</returns>
        private List<int> ListFeildsFromChecked() {
            List<int> reto = new List<int>(1);
            int aa = 0;
            Pieco enemyvan;
            Pieco enemytwo;

            enemyvan = whitomv ? (Pieco.Queen | Pieco.Black) : Pieco.Queen;
            enemytwo = whitomv ? (Pieco.Bishop | Pieco.Black) : Pieco.Bishop;
            aa = FieldFromCheckTwoPeace(s_pppiCaseMoveDiagonal[kingo], enemyvan, enemytwo);
            if (aa >= 0) { reto.Add(aa); }
            enemytwo = whitomv ? (Pieco.Rook | Pieco.Black) : Pieco.Rook;
            aa = FieldFromCheckTwoPeace(s_pppiCaseMoveLine[kingo], enemyvan, enemytwo);
            if (aa >= 0) { reto.Add(aa); }
            enemyvan = whitomv ? (Pieco.Knight | Pieco.Black) : Pieco.Knight;
            aa = FieldFromCheckOnePeace(s_ppiCaseMoveKnight[kingo], enemyvan);
            if (aa >= 0) { reto.Add(aa); }
            enemyvan = whitomv ? (Pieco.Pawn | Pieco.Black) : Pieco.Pawn;
            aa = FieldFromCheckOnePeace(whitomv ? s_ppiCaseBlackPawnCanAttackFrom[kingo] : s_ppiCaseWhitePawnCanAttackFrom[kingo], enemyvan);
            return reto;
            }

        /// <summary>
        /// Перечень полей, с которых атаковано данное поле
        /// Модификация от 24 августа 2015 года
        /// Заложен 24 августа 2015 года
        /// </summary>
        /// <param name="coloro">Цвет атакующих данное поле фигур</param>
        /// <param name="fildo">Проверяемое на атакованность поле</param>
        /// <returns>Искомый перечень</returns>
        private List<int> ListFeildsFromAttackedThisField(bool coloro, int fildo) {
            List<int> reto = new List<int>(1);
            int aa = 0;
            Pieco attackvan;
            Pieco attacktwo;

            attackvan = coloro ? Pieco.Queen : (Pieco.Queen | Pieco.Black);
            attacktwo = coloro ? Pieco.Bishop : (Pieco.Bishop | Pieco.Black);
            aa = FieldFromCheckTwoPeace(s_pppiCaseMoveDiagonal[fildo], attackvan, attacktwo);
            if (aa >= 0) { reto.Add(aa); }
            attacktwo = coloro ? Pieco.Rook : (Pieco.Rook | Pieco.Black);
            aa = FieldFromCheckTwoPeace(s_pppiCaseMoveLine[fildo], attackvan, attacktwo);
            if (aa >= 0) { reto.Add(aa); }
            attackvan = coloro ? Pieco.King : (Pieco.King | Pieco.Black);
            aa = FieldFromCheckOnePeace(s_ppiCaseMoveKing[fildo], attackvan);
            if (aa >= 0) { reto.Add(aa); }
            attackvan = coloro ? Pieco.Knight : (Pieco.Knight | Pieco.Black);
            aa = FieldFromCheckOnePeace(s_ppiCaseMoveKnight[fildo], attackvan);
            if (aa >= 0) { reto.Add(aa); }
            attackvan = coloro ? Pieco.Pawn : (Pieco.Pawn | Pieco.Black);
            aa = FieldFromCheckOnePeace(coloro ? s_ppiCaseWhitePawnCanAttackFrom[fildo] : s_ppiCaseBlackPawnCanAttackFrom[fildo], attackvan);
            return reto;
            }

        /// <summary>
        /// Перечень полей, с которых можно попасть на данное пустое поле
        /// За исключением короля
        /// Модификация от 26 августа 2015 года
        /// Заложен 26 августа 2015 года
        /// </summary>
        /// <param name="coloro">Цвет искомой попадающей фигуры</param>
        /// <param name="fildo">Поле, куда хотим попасть</param>
        /// <returns>Перечень, возможно пустой, но не null</returns>
        private List<int> ListFeildsFromMayGoTo(bool coloro, int fildo) {
            List<int> reto = new List<int>(1);
            int aa = 0;
            Pieco attackvan;
            Pieco attacktwo;

            attackvan = coloro ? Pieco.Queen : (Pieco.Queen | Pieco.Black);
            attacktwo = coloro ? Pieco.Bishop : (Pieco.Bishop | Pieco.Black);
            aa = FieldFromCheckTwoPeace(s_pppiCaseMoveDiagonal[fildo], attackvan, attacktwo);
            if (aa >= 0) { reto.Add(aa); }
            attacktwo = coloro ? Pieco.Rook : (Pieco.Rook | Pieco.Black);
            aa = FieldFromCheckTwoPeace(s_pppiCaseMoveLine[fildo], attackvan, attacktwo);
            if (aa >= 0) { reto.Add(aa); }
            attackvan = coloro ? Pieco.Knight : (Pieco.Knight | Pieco.Black);
            aa = FieldFromCheckOnePeace(s_ppiCaseMoveKnight[fildo], attackvan);
            if (aa >= 0) { reto.Add(aa); }
            //Короля исключили из проверки, но еще нужна проверка пешек и не на взятие, а только на ход
            if (coloro) {
                if (fildo / 8 >= 2) {
                    if (pBoard[fildo - 8] == Pieco.Pawn) {
                        reto.Add(fildo - 8);
                    } else if (fildo / 8 == 3 && pBoard[fildo - 8] == Pieco.None && pBoard[fildo - 16] == Pieco.Pawn) {
                        reto.Add(fildo - 16);
                        }
                    }
            } else {
                if (fildo / 8 <= 5) {
                    if (pBoard[fildo + 8] == (Pieco.Pawn | Pieco.Black)) {
                        reto.Add(fildo + 8);
                    } else if (fildo / 8 == 4 && pBoard[fildo + 8] == Pieco.None && pBoard[fildo + 16] == (Pieco.Pawn | Pieco.Black)) {
                        reto.Add(fildo + 16);
                        }
                    }
                }
            return reto;
            }

        /// <summary>
        /// Можно ли съесть эту чужую фигуру для делающего этот ход
        /// Важно, что именно съесть - для пешек, особенно с учетом взятия на проходе
        /// Модификация от 24 августа 2015 года
        /// Заложен 22 августа 2015 года
        /// </summary>
        /// <param name="fildo"></param>
        /// <returns></returns>
        private bool CanEatEnemyOnTheField(int fildo) {
            bool reto = false;
            bool coloro = whitomv;
            //Просто проверить на ударяемость мало.
            //Надо, чтобы ударяемость была возможна
            //Поэтому нужен ударяющий список, который надо тестировать на корректность.
            List<int> laa = ListFeildsFromAttackedThisField(coloro, fildo);
            foreach (int fromo in laa) {
                if (IsAvailableMove(fromo, fildo, pBoard[fromo], MovoTypo.PieceEaten)) {
                    reto = true;
                    break;
                    }
                }
            if (!reto) {//Нужно проверить ещё и на взятие на проходе, если шахует пешка
                if (enpasso && ((pBoard[fildo] & Pieco.PieceMask) == Pieco.Pawn) && fildo == enfield + 24) {
                    if( fildo % 8 > 0 && pBoard[fildo - 1] == (coloro ? Pieco.Pawn : (Pieco.Pawn | Pieco.Black)) ) {
                        int pifo = fildo - 1;
                        int pito = fildo + (coloro ? 8 : -8);
                        if( IsAvailableMove(pifo, pito, pBoard[pifo], MovoTypo.EnPassant | MovoTypo.PieceEaten ) ) {
                            reto = true;
                            }
                        }
                    if( fildo % 8 < 7 && pBoard[fildo + 1] == (coloro ? Pieco.Pawn : (Pieco.Pawn | Pieco.Black)) ) {
                        int pifo = fildo + 1;
                        int pito = fildo + (coloro ? 8 : -8);
                        if( IsAvailableMove(pifo, pito, pBoard[pifo], MovoTypo.EnPassant | MovoTypo.PieceEaten ) ) {
                            reto = true;
                            }
                        }
                    }

                }
            return reto;
            }

        /// <summary>
        /// Возможно ли текущим ходом занять указанное поле (оно пустое всегда)
        /// Модификация от 26 августа 2015 года
        /// Заложен 22 августа 2015 года
        /// </summary>
        /// <param name="fildo">Искомое пустое поле</param>
        /// <returns>Истина, если такой ход найдется</returns>
        private bool CanMoveOnThisField(int fildo) {
            bool reto = false;
            bool coloro = whitomv;
            //Просто проверить на доступность поля мало.
            //Надо, чтобы покидаемое поле не открывало дорогу к королю
            //Поэтому нужен список, который надо тестировать на корректность.
            List<int> laa = ListFeildsFromMayGoTo(coloro, fildo);
            foreach (int fromo in laa) {
                if (IsAvailableMove(fromo, fildo, pBoard[fromo], MovoTypo.Normal)) {
                    reto = true;
                    break;
                    }
                }
            return reto;
            }

        /// <summary>
        /// Возможно ли закрыться от угрозы защищаемому полю
        /// Модификация от 24 августа 2015 года
        /// Заложен 22 августа 2015 года
        /// </summary>
        /// <param name="ugroza">Откуда исходит угроза</param>
        /// <param name="zashita">Кого спасаем</param>
        /// <returns>Есть переурытие - истина</returns>
        private bool CanPerekrytie(int ugroza, int zashita) {
            bool reto = false;
            int[][] prove = null;
            //Перекрыться можно только при угрозе, исходящей от трех типов фигур
            switch(pBoard[ugroza] & Pieco.PieceMask) {
                case Pieco.Queen:
                    prove = s_pppiCaseMoveDiagLine[ugroza];
                    break;
                case Pieco.Rook:
                    prove = s_pppiCaseMoveLine[ugroza];
                    break;
                case Pieco.Bishop:
                    prove = s_pppiCaseMoveDiagonal[ugroza];
                    break;
                default:
                    break;
                }
            if (prove != null) {
                bool bestypf = false;
                List<int>perefildy = null;
                foreach (int[] aa in prove) {
                    for (int i = 0; i < aa.Length; i++) {
                        if(aa[i] == zashita && i >= 1) {
                            bestypf = true;
                            perefildy = new List<int>();
                            for (int j = 0; j < i; j++) {
                                perefildy.Add(aa[j]);
                                }
                            break;
                            }
                        if (pBoard[aa[i]] != Pieco.None) {
                            break;
                            }
                        }
                    }
                if (bestypf) {
                    foreach (int zf in perefildy) {
                        if (CanMoveOnThisField(zf)) {
                            reto = true;
                            break;
                            }
                        }
                    }                    
                }
            return reto;
            }

        private bool CheckFromTwoPeace(int[][] ppiCaseMoveList, Pieco p1, Pieco p2) {
            bool reto = false;
            Pieco curro;

            foreach (int[] piMoveList in ppiCaseMoveList) {
                foreach (int iNewPos in piMoveList) {
                    curro = pBoard[iNewPos];
                    if (curro != Pieco.None) {
                        if (curro == p1 || curro == p2) {
                            reto = true;
                            }
                        break;
                        }
                    }
                }
            return reto;
            }

        private bool CheckFromOnePeace(int[] piCaseMoveList, Pieco pp) {
            bool reto = false;
            foreach (int iNewPos in piCaseMoveList) {
                if (pBoard[iNewPos] == pp) {
                    reto = true;
                    break;
                    }
                }
            return reto;
            }

        /// <summary>
        /// Целочисленная проверка на шах с целью зафиксировать откуда шахуют
        /// Модификация от 3 августа 2015 года
        /// Заложен 3 августа 2015 года
        /// </summary>
        /// <param name="ppiCaseMoveList"></param>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <returns></returns>
        private int FieldFromCheckTwoPeace(int[][] ppiCaseMoveList, Pieco p1, Pieco p2) {
            int reto = -1;
            Pieco curro;

            foreach (int[] piMoveList in ppiCaseMoveList) {
                foreach (int iNewPos in piMoveList) {
                    curro = pBoard[iNewPos];
                    if (curro != Pieco.None) {
                        if (curro == p1 || curro == p2) {
                            reto = iNewPos;
                            }
                        break;
                        }
                    }
                }
            return reto;
            }

        /// <summary>
        /// Целочисленная проверка на шах с целью зафиксировать откуда шахуют
        /// Модификация от 3 августа 2015 года
        /// Заложен 3 августа 2015 года
        /// </summary>
        /// <param name="piCaseMoveList"></param>
        /// <param name="pp"></param>
        /// <returns></returns>
        private int FieldFromCheckOnePeace(int[] piCaseMoveList, Pieco pp) {
            int reto = -1;
            foreach (int iNewPos in piCaseMoveList) {
                if (pBoard[iNewPos] == pp) {
                    reto = iNewPos;
                    break;
                    }
                }
            return reto;
            }

        /// <summary>
        /// Модификация от 26 августа 2015 года
        /// Заложен 24 июля 2015 года
        /// </summary>
        /// <param name="fg"></param>
        /// <param name="pf"></param>
        /// <param name="pt"></param>
        /// <returns></returns>
        private bool IsCanMove(Pieco fg, int pf, int pt, bool vzyatie) {
            bool reto = false;
            switch(fg & Pieco.PieceMask) {
                case Pieco.Queen:
                    foreach( int[] aa in s_pppiCaseMoveDiagLine[pf]) {
                        if( IsAvailableFieldOnLineOrDiagonal(aa, pt) ) {
                            reto = true;
                            break;
                            }
                        }
                    break;
                case Pieco.Rook:
                    foreach (int[] aa in s_pppiCaseMoveLine[pf]) {
                        if (IsAvailableFieldOnLineOrDiagonal(aa, pt)) {
                            reto = true;
                            break;
                            }
                        }
                    break;
                case Pieco.Bishop:
                    foreach (int[] aa in s_pppiCaseMoveDiagonal[pf]) {
                        if (IsAvailableFieldOnLineOrDiagonal(aa, pt)) {
                            reto = true;
                            break;
                            }
                        }
                    break;
                case Pieco.Knight:
                    if( s_ppiCaseMoveKnight[pf].Contains(pt) ) {
                        reto = true;
                        }
                    break;
                case Pieco.King:
                    if (s_ppiCaseMoveKing[pf].Contains(pt)) {
                        reto = true;
                        }
                    break;
                case Pieco.Pawn:
                    if (vzyatie) {
                        int[] bb = whitomv ? s_ppiCaseWhitePawnCanAttackFrom[pt] : s_ppiCaseBlackPawnCanAttackFrom[pt];
                        if (bb.Contains(pf)) { reto = true; }
                    } else {
                        if (whitomv) {
                            if (pt / 8 == 3) {
                                if ((pf == pt - 16) && pBoard[pt - 8] == Pieco.None) { reto = true; }
                                }
                            if (pf == pt - 8) { reto = true; }
                        } else {
                            if (pt / 8 == 4) {
                                if ((pf == pt + 16) && pBoard[pt + 8] == Pieco.None) { reto = true; }
                                }
                            if (pf == pt + 8) { reto = true; }
                            }
                        }
                    break;
                default:
                    break;
                    }
            return reto;
            }

        /// <summary>
        /// Модификация от 31 июля 2015 года
        /// Заложен 31 июля 2015 года
        /// </summary>
        /// <param name="pset"></param>
        /// <param name="fieldo"></param>
        /// <returns></returns>
        private bool IsAvailableFieldOnLineOrDiagonal(int[] pset, int fieldo) {
            bool reto = false;

            foreach (int iNewPos in pset) {
                if (iNewPos == fieldo) {
                    reto = true;
                    break;
                    }
                if (pBoard[iNewPos] == Pieco.None) {
                    continue;
                } else {
                    break;
                    }
                }
            return reto;
            }


        /// <summary>
        /// Перечень полей, откуда данная фигура может прийти (возможно и со взятием - смотри параметр pmt) на указанное поле
        /// Модификация от 26 августа 2015 года
        /// Заложен 24 июля 2015 года
        /// </summary>
        /// <param name="who">Фигура, для которой составляется список</param>
        /// <param name="fildo">Поле, куда стремится указанная фигура</param>
        /// <param name="pmt">Тип - просто ход или даже взятие</param>
        /// <returns>Список, предполагающий максимум две начальные точки, хотя теоретически никто не отменял три и более. Вопрос реальности этого в жизни!!!</returns>
        public List<int> ListFieldsFromAttackedThisField(Pieco who, int fildo, MovoTypo pmt) {
            List<int> reto = new List<int>(2); //Не больше 2
            if (!whitomv) { who |= Pieco.Black; }
            for (int i = 0; i < 64; i++) {
                if (pBoard[i] == who) {
                    if( IsCanMove(who, i, fildo, (pmt & (~MovoTypo.MoveTypeMask)) == MovoTypo.PieceEaten) ) {
                        if (IsAvailableMove(i, fildo, who, pmt)) {
                            reto.Add(i);
                            }
                        }
                    }
                }
            return reto;
            }

        /// <summary>
        /// Модификация от 3 августа 2015 года
        /// Заложен 29 июля 2015 года
        /// </summary>
        /// <param name="old"></param>
        /// <param name="pmv"></param>
        /// <returns></returns>
        public Caslo RokkoChangeAfterMove(Caslo old, Mova pmv) {
            Caslo reto = old;
            if (pmv.MvType == MovoTypo.Castle || (pmv.Figura & Pieco.PieceMask) == Pieco.King) {
                if (whitomv) { //Это уже инверсия. Сейчас ход противника, а в pmv ещё предыдущий ход
                    if ((reto & Caslo.KingBlack) > 0) { reto ^= Caslo.KingBlack; }
                    if ((reto & Caslo.QueenBlack ) > 0) { reto ^= Caslo.QueenBlack; } 
                } else {
                    if ((reto & Caslo.KingWhite) > 0) { reto ^= Caslo.KingWhite; }
                    if ((reto & Caslo.QueenWhite) > 0) { reto ^= Caslo.QueenWhite; }
                    }
            } else if ((pmv.Figura & Pieco.PieceMask) == Pieco.Rook) {
                if (whitomv) {
                    if (pmv.FromField == 56) {
                        if ((reto & Caslo.KingBlack) > 0) { reto ^= Caslo.KingBlack; }
                        }
                    if (pmv.FromField == 63) {
                        if ((reto & Caslo.QueenBlack) > 0) { reto ^= Caslo.QueenBlack; }
                        }
                } else {
                    if (pmv.FromField == 0) {
                        if ((reto & Caslo.KingWhite) > 0) { reto ^= Caslo.KingWhite; }
                        }
                    if (pmv.FromField == 7) {
                        if ((reto & Caslo.QueenWhite) > 0) { reto ^= Caslo.QueenWhite; }
                        }
                    }
                }
            return reto;
            }

        /// <summary>
        /// Модификация от 30 июля 2015 года
        /// Заложен 29 июля 2015 года
        /// </summary>
        /// <param name="pmv"></param>
        /// <returns></returns>
        public int CanEnpasso(Mova pmv) {
            int reto = -1;
            if (pmv.Figura == (Pieco.Pawn | Pieco.White) && ((pmv.ToField - pmv.FromField) == 16)) {
                if ((pmv.ToField % 8 > 0 && pBoard[pmv.ToField - 1] == (Pieco.Pawn | Pieco.Black)) ||
                    (pmv.ToField % 8 < 7 && pBoard[pmv.ToField + 1] == (Pieco.Pawn | Pieco.Black))) {
                    reto = pmv.ToField - 8;
                    }
            } else if (pmv.Figura == (Pieco.Pawn | Pieco.Black) && (pmv.FromField - pmv.ToField == 16)) {
                if ((pmv.ToField % 8 > 0 && pBoard[pmv.ToField - 1] == (Pieco.Pawn | Pieco.White)) ||
                    (pmv.ToField % 8 < 7 && pBoard[pmv.ToField + 1] == (Pieco.Pawn | Pieco.White))) {
                    reto = pmv.ToField + 8;
                    }

                }
            return reto;
            }


#region------------------------------Публичные Свойства-----------------------------------------
        public Pieco[] Boardo { get { return pBoard; } }
        public bool Coloro { get { return whitomv; } }
#endregion---------------------------Публичные Свойства-----------------------------------------
        }

    [Flags]
    public enum Pieco : byte
    {
        /// <summary>No piece</summary>
        None = 0,
        /// <summary>Pawn</summary>
        Pawn = 1,
        /// <summary>Knight</summary>
        Knight = 2,
        /// <summary>Bishop</summary>
        Bishop = 3,
        /// <summary>Rook</summary>
        Rook = 4,
        /// <summary>Queen</summary>
        Queen = 5,
        /// <summary>King</summary>
        King = 6,
        /// <summary>Mask to find the piece</summary>
        PieceMask = 7,
        /// <summary>Piece is black</summary>
        Black = 8,
        /// <summary>White piece</summary>
        White = 0,
    }

    [Flags]
    public enum Caslo : byte
    {
        /// <summary>Нельзя никуда</summary>
        None = 0,
        /// <summary>Короткая у белых</summary>
        KingWhite = 1,
        /// <summary>Длинная у белых</summary>
        QueenWhite = 2,
        /// <summary>Короткая у чёрных</summary>
        KingBlack = 4,
        /// <summary>Длинная у чёрных</summary>
        QueenBlack = 8,
    }

    public enum MovoTypo : byte {
        /// <summary>Normal move</summary>
        Normal = 0,
        /// <summary>Pawn which is promoted to a queen</summary>
        PawnPromotionToQueen = 1,
        /// <summary>Castling</summary>
        Castle = 2,
        /// <summary>Prise en passant</summary>
        EnPassant = 8,
        /// <summary>Pawn which is promoted to a rook</summary>
        PawnPromotionToRook = 5,
        /// <summary>Pawn which is promoted to a bishop</summary>
        PawnPromotionToBishop = 6,
        /// <summary>Pawn which is promoted to a knight</summary>
        PawnPromotionToKnight = 7,
        /// <summary>Piece type mask</summary>
        MoveTypeMask = 15,
        /// <summary>The move eat a piece</summary>
        PieceEaten = 16,
        }

    }
