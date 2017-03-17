using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace OnlyWorko {

#region--------------------------------ПЕРВИЧНЫЙ КЛАСС--Valuing------------------------
    /// Модификация от 5 февраля 2016 года
    /// Заложен 28 декабря 2015 года
    public class Valuing {
        int rng;
        int valo;
        int timo;
        int depso;
        long nodes;
        int hlop;
        string mostha;
        DateTime mig;
        vlEngino etypo;
        Vario vara;

        /// <summary>
        /// Модификация от 29 января 2016 года
        /// Заложен 29 января 2016 года
        /// </summary>
        /// <param name="pp">Начальная позиция для оценки</param>
        /// <param name="stroke">Набор ходов в четырехсимвольном формате</param>
        /// <param name="ten">Тип двигателя</param>
        /// <param name="pvalo">Цельная оценка</param>
        /// <param name="dlito">Длительност расчёта в минутах</param>
        /// <param name="pdep">Глубина расчёта</param>
        /// <param name="pnod">Количество узликов</param>
        /// <param name="prng">Место в наборе</param>
        /// <param name="pmo">Строковая характеристика мощи</param>
        public Valuing(pozo pp, string stroke, vlEngino ten, int pvalo, int dlito, int pdep, long pnod, int prng, string pmo) {
            vara = new Vario(pp, stroke);
            etypo = ten;
            valo = pvalo;
            timo = dlito;
            depso = pdep;
            nodes = pnod;
            rng = prng;
            mostha = pmo;
            mig = DateTime.Now;
            }

        /// <summary>
        /// Модификация от 5 февраля 2016 года
        /// Заложен 5 февраля 2016 года
        /// </summary>
        /// <param name="xel"></param>
        public Valuing(XElement xel) {
            valo = int.Parse(xel.Attribute("Valo").Value);
            etypo = (vlEngino)Enum.Parse(typeof(vlEngino), xel.Attribute("Dvig").Value);
            depso = int.Parse(xel.Attribute("Glub").Value);
            timo = int.Parse(xel.Attribute("Tima").Value);
            rng = int.Parse(xel.Attribute("Rang").Value);
            nodes = long.Parse(xel.Attribute("NDS").Value);
            hlop = int.Parse(xel.Attribute("Hlop").Value);
            mig = DateTime.Parse(xel.Attribute("Mig").Value);
            mostha = xel.Element("Moisha").Value;
            vara = new Vario(xel.Element("Vario"));
            }

        /// <summary>
        /// Модификация от 3 февраля 2016 года
        /// Заложен 3 февраля 2016 года
        /// </summary>
        /// <returns></returns>
        public XElement XMLOut() {
            XElement reto = new XElement("ValuPrice");
            reto.Add(new XAttribute("Valo", valo));
            reto.Add(new XAttribute("Dvig", etypo.ToString()));
            reto.Add(new XAttribute("Glub", depso));
            reto.Add(new XAttribute("Tima", timo));
            reto.Add(new XAttribute("Rang", rng));
            reto.Add(new XAttribute("NDS", nodes));
            reto.Add(new XAttribute("Hlop", hlop));
            reto.Add(new XAttribute("Mig", mig));
            reto.Add(new XElement("Moisha", mostha));
            reto.Add(vara.XMLOut());
            return reto;
            }

#region--------------------------Свойства объекта-----------------------------------------
        public int Rango { get { return rng; } }
        public int Valo { get { return valo; } }
        public int Duma { get { return timo; } }
        public int Glub { get { return depso; } }
        public long Nody { get { return nodes; } }
        public int Hlopo { get { return hlop; } set { hlop = value; } }
        public string Mostha { get { return mostha; } }
        public DateTime Dato { get { return mig; } }
        public string DatoStroke { get { return mig.ToString("yy-MM-dd HH:mm"); } }
        public string Dvigatel { get { return etypo.ToString(); } }
        public string Texa { get { return vara.OnlyMova; } }
        public pozo Finalo { get { return vara.LastPo; } }
        public string MovoVan { get { return vara.Begino; } }
        public Vario SelfVarianto { get { return vara; } }

#endregion-----------------------Свойства объекта-----------------------------------------

        }
#endregion-----------------------------ПЕРВИЧНЫЙ КЛАСС--Valuing------------------------

#region--------------------------------ДРУГОЙ КЛАСС--ValuWorka------------------------
    /// <summary>
    /// Модификация от 29 января 2016 года
    /// Заложен 29 января 2016 года
    /// </summary>
    public class ValuWorka {
        List<Valuing> lival;
        pozo targa;

        /// <summary>
        /// Модификация от 29 января 2016 года
        /// Заложен 29 января 2016 года
        /// </summary>
        /// <param name="pp">Оцениваемая позиция</param>
        public ValuWorka(pozo pp) {
            targa = pp;
            lival = null;
            }

        /// <summary>
        /// Модификация от 29 января 2016 года
        /// Заложен 29 января 2016 года
        /// </summary>
        /// <param name="seta">Результат анализа в виде набора строк</param>
        /// <param name="ten">Тип двигателя</param>
        /// <param name="dlito">Время расчёта в минутах</param>
        /// <param name="kvo">Количество оцениваемых ходов</param>
        public void AddValuSet(List<string> seta, vlEngino ten, int dlito, int kvo) {
            int poradok = 0;
            string mosha = string.Empty;
            foreach (string aa in seta) {
                if (poradok == 2) {
                    if (aa.StartsWith("info string")) {
                        mosha += aa.Substring(11);
                        continue;
                    } else {
                        break;
                        }
                } else if (poradok == 1 && aa.StartsWith("readyok") ) {
                    poradok = 2;
                    continue;
                } else if( poradok == 0 && aa.StartsWith("info string") ) {
                    mosha = aa.Substring(12);
                    poradok = 1;
                    continue;
                    }
                }
            int kava = seta.Count;
            List<string> mano = new List<string>(kvo);
            bool vklo = false;
            for (int i = kava - 1; i > kava - 100 && i > 0; i--) {
                if (vklo) {
                    if (seta[i].StartsWith("info multipv")) {
                        mano.Add(seta[i]);
                        continue;
                    } else {
                        break;
                        }
                } else {
                    if (seta[i].StartsWith("info multipv")) {
                        mano.Add(seta[i]);
                        vklo = true;
                        continue;
                        }
                    }
                }
            if (kvo != mano.Count) {
                throw new myClasterException(string.Format("Valuing->AddValuSet, kvo != mano.Count --{0}--{1}--", kvo, mano.Count));
                }
            string pstroke = string.Empty;
            int pvalo = 0;
            int pdep = 0;
            long pnod = 0;
            int prng = 0;
            foreach (string aa in mano) {
                pstroke = AnalizeVanStroke(aa, out pvalo, out pdep, out pnod, out prng);
                Valuing vv = new Valuing(targa, pstroke, ten, pvalo, dlito, pdep, pnod, prng, mosha);
                if (lival == null) { lival = new List<Valuing>(); }
                lival.Add(vv);
                }

            //Еще нужна проверка на схлопывание вариантов
            }

        /// <summary>
        /// Модификация от 1 февраля 2016 года
        /// Заложен 1 февраля 2016 года
        /// </summary>
        /// <param name="xx">искомая строка</param>
        /// <param name="pvalo">значение оценки</param>
        /// <param name="pdep">глубина</param>
        /// <param name="pnod">очень большое число узелков</param>
        /// <param name="prng">ранг - место</param>
        /// <returns>Набор ходов для варианта</returns>
        private static string AnalizeVanStroke(string xx, out int pvalo, out int pdep, out long pnod, out int prng) {
            string reto = string.Empty;
            pvalo = pdep = prng = 0;
            pnod = 0L;
            string patera = @"info multipv (\d+)";
            Match resa = Regex.Match(xx, patera);
            string rr = resa.Groups[1].Value;
            if (!int.TryParse(rr, out prng)) {
                throw new myClasterException(string.Format("Valuing->AalizeVanStroke, не определил ранг оценки --{0}--", rr));
                }
            patera = @" depth (\d+)";
            resa = Regex.Match(xx, patera);
            rr = resa.Groups[1].Value;
            if (!int.TryParse(rr, out pdep)) {
                throw new myClasterException(string.Format("Valuing->AalizeVanStroke, не определил глубину --{0}--", rr));
                }
            patera = @"score cp (-?\d+)";
            resa = Regex.Match(xx, patera);
            rr = resa.Groups[1].Value;
            if (!int.TryParse(rr, out pvalo)) {
                throw new myClasterException(string.Format("Valuing->AalizeVanStroke, не определил цифровую оценку --{0}--", rr));
                }
            patera = @"nodes (\d+)";
            resa = Regex.Match(xx, patera);
            rr = resa.Groups[1].Value;
            if (!long.TryParse(rr, out pnod)) {
                throw new myClasterException(string.Format("Valuing->AalizeVanStroke, не определил узелки --{0}--", rr));
                }
            patera = @"hashfull \d+ pv\s(.*)\Z";
            resa = Regex.Match(xx, patera);
            rr = resa.Groups[1].Value;
            if( rr.Length > 0 ) {
                reto = rr;
                }
            return reto;
            }

#region--------------------------Свойства объекта-----------------------------------------
        public List<Valuing> LiValus { get { return lival; } }
#endregion-----------------------Свойства объекта-----------------------------------------
        }
#endregion-----------------------------ДРУГОЙ КЛАСС--ValuWorka------------------------

#region--------------------------------ENUM--vlEngino---------------------------------
    /// Модификация от 28 декабря 2015 года
    /// Заложен 28 декабря 2015 года
    public enum vlEngino
    {
        Houdini_3a_Pro_w32,
        EventDateHasAnswer,
        Unknown
        }
#endregion--------------------------------ENUM--vlEngino------------------------------

    }
