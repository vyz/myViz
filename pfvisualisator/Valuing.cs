using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace pfVisualisator {

    /// Модификация от 29 января 2016 года
    /// Заложен 28 декабря 2015 года
    public class Valuing
    {
        int rng;
        int valo;
        int timo;
        int depso;
        int nodes;
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
        public Valuing(pozo pp, string stroke, vlEngino ten, int pvalo, int dlito, int pdep, int pnod, int prng, string pmo) {
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

#region--------------------------Свойства объекта-----------------------------------------
        public int Rango { get { return rng; } }
        public int Valo { get { return valo; } }
        public int Duma { get { return timo; } }
        public int Glub { get { return depso; } }
        public int Nody { get { return nodes; } }
        public int Hlopo { get { return hlop; } set { hlop = value; } }
        public string Mostha { get { return mostha; } }
        public DateTime Dato { get { return mig; } }
        public string DatoStroke { get { return mig.ToString("yy-MM-dd HH:mm"); } }
        public string Dvigatel { get { return etypo.ToString(); } }
        public string Texa { get { return vara.OnlyMova; } }
        public pozo Finalo { get { return vara.LastPo; } }
#endregion-----------------------Свойства объекта-----------------------------------------

        }

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

            }
    }

    /// Модификация от 28 декабря 2015 года
    /// Заложен 28 декабря 2015 года
    public enum vlEngino
    {
        Houdini_3a_Pro_w32,
        EventDateHasAnswer,
        Unknown
        }
    }
