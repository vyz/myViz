using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace pfVisualisator {

    /// <summary>
    /// Первый объект в истории создается автоматом при создании самого объекта, и по сути пустой.
    /// Последующие создаются путем сохранения в историю с перечнем сохраняемых атрибутов в момент сохранения 
    /// (желательно автоматически - сравнением объекта в базе (эталона перед началом редактирования) и сохраняемого)
    /// </summary>
    public class myhistory {
        private DateTime eddate;
        private string edtext;
        private myleo oldsavo;

        public myhistory() { }

        /// <summary>
        /// Модификация от 2 марта 2015 года
        /// Заложен 2 марта 2015 года
        /// </summary>
        /// <param name="comto"></param>
        /// <param name="histoclone"></param>
        public myhistory(string comto, myleo histoclone) {
            eddate = DateTime.Now;
            edtext = comto;
            oldsavo = histoclone;
            }

        /// <summary>
        /// Модификация от 2 марта 2015 года
        /// Заложен 8 января 2015 года
        /// </summary>
        /// <param name="Elemo"></param>
        /// <param name="selfa"></param>
        public myhistory(XElement Elemo) {
            eddate = DateTime.Parse(Elemo.Attribute("Dato").Value);
            edtext = Elemo.Attribute("Comto").Value;
            XElement aa = Elemo.Element("Leo");
            oldsavo = new myleo(aa);
            }

        /// <summary>
        /// Модификация от 2 марта 2015 года
        /// Заложен 23 декабря 2014 года
        /// </summary>
        /// <returns></returns>
        public XElement LeoXML() {
            XElement xRet = new XElement("History", new XAttribute("Dato", eddate), new XAttribute("Comto", edtext));
            xRet.Add(oldsavo.LeoToXML());
            return xRet;
            }

        public DateTime Dato { get { return eddate; } }
        public string DatoStroke { get { return eddate.ToString("yy-MM-dd HH:mm"); } }
        public string Commento { get { return edtext; } 
                                 set { edtext = value; } }
        public myleo Hobago { get { return oldsavo; } }

        }

    public class vHistorySaver : Singleton<myleo>
    {

        private vHistorySaver() { }

        private static Dictionary<Guid, myleo> dictonar = new Dictionary<Guid, myleo>();

        /// <summary>
        /// Модификация от 2 марта 2015 года
        /// Заложен 25 февраля 2015 года
        /// </summary>
        /// <param name="nob"></param>
        public static void AddNewObject(myleo nob) {
            if (dictonar.ContainsKey(nob.LeoGuid)) {
                dictonar[nob.LeoGuid] = nob.HistoricalClone();
            } else {
                dictonar.Add(nob.LeoGuid, nob.HistoricalClone());
                }
            }

        /// <summary>
        /// Модификация от 2 марта 2015 года
        /// Заложен 26 февраля 2015 года
        /// </summary>
        /// <param name="sravn"></param>
        /// <returns></returns>
        public static bool IsADifference(myleo sravn) {
            bool reto = false;
            if (dictonar.ContainsKey(sravn.LeoGuid)) {
                reto = sravn.PoiskDiffov(dictonar[sravn.LeoGuid]);
            } else {
                //НОНСЕНС
                throw new HistoryLeoException("Попытка найти различия с несохранённым в \"историческом словаре\" объектом");
                }
            return reto;
            }

        public static myleo GetDifferenceHistor(myleo sravn)
        {
            myleo reto = null;
            reto = sravn.GetDifferenceHistor(dictonar[sravn.LeoGuid]);
            return reto;
        }
    }

    public class HistoryLeoException : Exception
    {
        public HistoryLeoException()
        {
        }
        public HistoryLeoException(string message)
            : base(message)
        {
        }
        public HistoryLeoException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }

}
