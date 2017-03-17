using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Text;

namespace OnlyWorko {
    public class Mova {
        private int fromo;
        private int tomo;
        private bool coala;
        private Pieco figo;
        private MovoTypo typo;
        private string shorto;

        public Mova() { }

        public Mova(bool pcolo, Pieco pfigo, int pif, int pit, MovoTypo pmt) {
            coala = pcolo;
            figo = pfigo;
            fromo = pif;
            tomo = pit;
            typo = pmt;
            shorto = null;
            }

        /// <summary>
        /// Модификация от 4 февраля 2016 года
        /// Заложен 4 февраля 2016 года
        /// </summary>
        /// <param name="xel"></param>
        public Mova(XElement xel) {
            fromo = int.Parse(xel.Attribute("Fromo").Value);
            tomo = int.Parse(xel.Attribute("Tomo").Value);
            coala = xel.Attribute("Coala").Value == "W";
            figo = (Pieco)Enum.Parse(typeof(Pieco), xel.Element("Zver").Value);
            typo = (MovoTypo)Enum.Parse(typeof(MovoTypo), xel.Element("Typon").Value);
            shorto = xel.Element("Nota").Value;
            }

        /// <summary>
        /// Модификация от 26 августа 2015 года
        /// Заложен 27 июля 2015 года
        /// </summary>
        /// <param name="pp"></param>
        public void FormShortoString(PozoUtils pp) {
            StringBuilder fremo = new StringBuilder(4);
            if (typo == MovoTypo.Castle) {
                if (tomo == 1 || tomo == 57) {
                    fremo.Append("O-O");
                } else if (tomo == 5 || tomo == 61) {
                    fremo.Append("O-O-O");
                } else {
                    fremo.Append("?????");
                    throw new myClasterException("Mova, рокировка неизвестного типа");
                    }
            } else {
                switch (figo & Pieco.PieceMask) {
                    case Pieco.Pawn:
                        if ((typo & MovoTypo.PieceEaten) > 0) {
                            fremo.Append(Convert.ToChar((int)'h' - (fromo % 8)));
                            }
                        break;
                    case Pieco.Knight:
                        fremo.Append('N');
                        break;
                    case Pieco.Bishop:
                        fremo.Append('B');
                        break;
                    case Pieco.Rook:
                        fremo.Append('R');
                        break;
                    case Pieco.Queen:
                        fremo.Append('Q');
                        break;
                    case Pieco.King:
                        fremo.Append('K');
                        break;
                    default:
                        fremo.Append('?');
                        break;
                    }
                List<int> lifi = pp.ListFieldsFromAttackedThisField(figo, tomo, typo);
                if (lifi.Count > 1 && lifi.Count < 3) {
                    if( lifi[0] % 8 == lifi[1] % 8 ) { //Совпадение колонок
                        fremo.Append( Convert.ToChar((int) '1' + (fromo / 8))); 
                        }
                    else { //На совпадение строк не проверяем, главное что не совпадают колонки
                        if (!((figo & Pieco.PieceMask) == Pieco.Pawn && (typo & MovoTypo.PieceEaten) > 0)) {
                            fremo.Append(Convert.ToChar((int)'h' - (fromo % 8)));
                            }
                        }
                    }
                else if( lifi.Count >= 3 || lifi.Count < 1) 
                {
                    throw new myClasterException(string.Format("Mova, Количество стартовых полей превышает разумные пределы --{0}--", lifi.Count));
                }
                if ((typo & MovoTypo.PieceEaten) > 0)
                {
                    fremo.Append('x');
                }
                fremo.Append(Convert.ToChar((int)'h' - (tomo % 8)));
                fremo.Append(Convert.ToChar((int)'1' + (tomo / 8)));
                }
            if ((typo & MovoTypo.MoveTypeMask) == MovoTypo.PawnPromotionToQueen) { fremo.Append("=Q"); }
            else if ((typo & MovoTypo.MoveTypeMask) == MovoTypo.PawnPromotionToRook) { fremo.Append("=R"); }
            else if ((typo & MovoTypo.MoveTypeMask) == MovoTypo.PawnPromotionToKnight) { fremo.Append("=N"); }
            else if ((typo & MovoTypo.MoveTypeMask) == MovoTypo.PawnPromotionToBishop) { fremo.Append("=B"); }
            //мат и шах в хвост
            PozoUtils testo = new PozoUtils(pp.Boardo, !pp.Coloro, fromo, tomo, figo, typo, false);
            if(testo.IsCheckmated()) {
                fremo.Append('#');
            } else if (!testo.IsNoCheck()) {
                fremo.Append('+');
                }
            shorto = fremo.ToString();
            }

        /// <summary>
        /// Модификация от 4 февраля 2016 года
        /// Заложен 4 февраля 2016 года
        /// </summary>
        /// <returns></returns>
        public XElement XMLOut() {
            XElement reto = new XElement("Mova");
            reto.Add(new XAttribute("Fromo", fromo));
            reto.Add(new XAttribute("Tomo", tomo));
            reto.Add(new XAttribute("Coala", coala ? "W" : "B"));
            reto.Add(new XElement("Zver", figo));
            reto.Add(new XElement("Typon", typo));
            reto.Add(new XElement("Nota", shorto));
            return reto;
            }

        public string Shorto { get { return shorto; } }
        public int FromField { get { return fromo; } }
        public int ToField { get { return tomo; } }
        public Pieco Figura { get { return figo; } }
        public MovoTypo MvType { get { return typo; } }
        public bool Koler { get { return coala; } }
        }
    }
