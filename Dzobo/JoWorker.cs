using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OnlyWorko;

namespace Dzobo
{
    public class JoWorker
    {
        string sesso;

        public JoWorker() {
            CreateSession();
            }

        /// <summary>
        /// Модификация от 27 марта 2017 года
        /// Заложен 27 марта 2017 года
        /// </summary>
        public void runo() {
            LogoCM.initologo("lDzobo.log");
            LogoCM.OutString(sesso + " Старт");
            PoiskTaskov();
            LogoCM.OutString(sesso + " Финиш");
            LogoCM.finitalogo();
            }

        private void PoiskTaskov() {

            }

        /// <summary>
        /// Модификация от 27 марта 2017 года
        /// Заложен 27 марта 2017 года
        /// </summary>
        private void CreateSession() {
            DateTime dd = DateTime.Now;
            sesso = string.Format("{0:000}{1}{2:00}{3:00}",dd.DayOfYear,dd.DayOfWeek,dd.Hour,dd.Minute);
            }   
    }
}
