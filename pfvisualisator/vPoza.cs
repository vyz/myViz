using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace pfVisualisator {

    /// <summary>
    /// Модификация от 24 декабря 2015 года
    /// Заложен 24 декабря 2015 года
    /// </summary>
    public class vPoza : myleo, INotifyPropertyChanged {
        public event PropertyChangedEventHandler PropertyChanged;

        private pozo poza;

        /// <summary>
        /// Модификация от 24 декабря 2015 года
        /// Заложен 24 декабря 2015 года
        /// </summary>
        public vPoza()
            : base(leoType.Pozo) {

            string[] akva = new string[3];
            ltexto = akva.ToList();
            }

        /// <summary>
        /// Модификация от 24 декабря 2015 года
        /// Заложен 24 декабря 2015 года
        /// </summary>
        /// <param name="pz"></param>
        public vPoza(pozo pz)
            : this() {
            pz.AvailableFill();
            namo = pz.NumberMove.ToString() + " ход " + (pz.IsQueryMoveWhite ? "белых" : "чёрных");
            bignamo = namo + ". Доступно " + pz.AvaQvo.ToString();
            Fena = pz.fenout();
            poza = pz;
            }   
 

        #region--------------------------Свойства объекта-----------------------------------------
        public string Namo { get { return namo; } set { namo = value; } }
        public string BigNamo { get { return bignamo; } set { bignamo = value; } }
        public string Fena { get { return ltexto[0]; } set { ltexto[0] = value; } }
        public string AnaRes { get { return ltexto[1]; } set { ltexto[1] = value; } }
        public string Descripto { get { return ltexto[2]; } set { ltexto[2] = value; } }
        public myTago Tago { get { return tago; } }
        public string TagoTextStroke { get { return tago.TStroke; } }
        public pozo Selfa { get { return poza; } }
        #endregion-----------------------Свойства объекта-----------------------------------------

        private void OnPropertyChanged(string name) {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) {
                handler(this, new PropertyChangedEventArgs(name));
                }
            }

        /// <summary>
        /// Модификация от 24 декабря 2015 года
        /// Заложен 24 декабря 2015 года
        /// </summary>
        /// <returns></returns>
        public static vPoza CreateExemplarusForLife() {
            pozo zg = pozo.SluchaynoPozo();
            vPoza reto = new vPoza(zg);
            reto.AnaRes = "Статико_Искусство";
            reto.Descripto = "Искусственный член";
            return reto;
            }

        }
    }
