using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace pfVisualisator
{
    public class vpfGluka
    {
        static public string explanation = "Начальное глюка";

        /// <summary>
        /// 
        /// Модификация от 17 декабря 2014 года
        /// Заложен 17 декабря 2014 года
        /// </summary>
        /// <param name="paroexplanation"></param>
        public static void BackoMess(string paroexplanation) {
            explanation = paroexplanation;
            var winda = new Gluko();
            bool? rewa = winda.ShowDialog();
            }

        static public string Explanation { get { return explanation; } }
    }
}
