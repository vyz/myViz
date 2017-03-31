using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Text;
using System.Reflection;

namespace OnlyWorko
{
    public class myclast
    {
        private Guid ida;
        private clasterType obty;
        protected string namo;
        protected string bignamo;
        protected List<string> ltexto;

        /// <summary>
        /// Модификация от 2 февраля 2015 года
        /// 
        /// </summary>
        /// <param name="typo"></param>
        protected myclast(clasterType typo)
        {
            ida = Guid.NewGuid();
            obty = typo;
        }

        /// <summary>
        /// Модификация от 5 марта 2015 года
        /// Заложен 8 января 2015 года
        /// </summary>
        /// <param name="Elemo"></param>
        public myclast(XElement Elemo)
        {
            ida = Guid.Parse(Elemo.Attribute("lid").Value);
            obty = TypoFromString(Elemo.Attribute("typo").Value);
            namo = Elemo.Element("Namo").Value;
            bignamo = Elemo.Element("BigNamo").Value;
            XElement aa = Elemo.Element("Histories");
            aa = Elemo.Element("Parameters");
            if (aa != null) {
                ltexto = new List<string>();
                foreach (XElement bb in aa.Elements("TParo")) {
                    ltexto.Add(bb.Value);
                    }
                }
            aa = Elemo.Element("Dopico");
            if (aa != null) {
                Dopico(aa);
                }
            }

        /// <summary>
        /// Модификация от 3 февраля 2016 года
        /// Заложен 23 декабря 2014 года
        /// </summary>
        /// <returns></returns>
        public XElement LeoToXML() {
            XElement xRet = new XElement("Leo", new XAttribute("lid", ida), new XAttribute("typo", obty));
            xRet.Add(new XElement("Namo", namo));
            xRet.Add(new XElement("BigNamo", bignamo));
            if(ltexto != null) {
                XElement aa = new XElement("Parameters");
                foreach (string bb in ltexto) {
                    aa.Add(new XElement("TParo", bb));
                    }
                xRet.Add(aa);
                }
            XElement dopi = Dopico();
            if (null != dopi) {
                xRet.Add(dopi);
                }
            return xRet;
            }

        /// <summary>
        /// Модификация от 3 февраля 2016 года
        /// Заложен 3 февраля 2016 года
        /// </summary>
        /// <returns></returns>
        public virtual XElement Dopico() {
            XElement reto = null;
            return reto;
            }

        /// <summary>
        /// Модификация от 5 февраля 2016 года
        /// Заложен 5 февраля 2016 года
        /// </summary>
        /// <param name="xel"></param>
        public virtual void Dopico(XElement xel) {
            }

        /// <summary>
        /// Модификация от 24 декабря 2015 года
        /// Заложен 12 марта 2015 года
        /// </summary>
        /// <returns></returns>
        public List<string> GetParamoNames() {
            List<string> reto = new List<string>();
            switch (obty) {
                case clasterType.OKPReport:
                    reto.Add("Коммент из базы");
                    reto.Add("Описание");
                    reto.Add("Файлы скриптов");
                    reto.Add("SQL-запрос");
                    reto.Add("GUID отчёта");
                    break;
                case clasterType.Gamo:
                    reto.Add("White");
                    reto.Add("Black");
                    reto.Add("Result");
                    reto.Add("Date");
                    reto.Add("Event");
                    reto.Add("ECO");
                    reto.Add("PlyCount");
                    reto.Add("WhiteElo");
                    reto.Add("BlackElo");
                    reto.Add("Site");
                    reto.Add("Round");
                    reto.Add("Fen");
                    reto.Add("OnlyMova");
                    reto.Add("AddAtr");
                    reto.Add("Описание");
                    reto.Add("Timingo");
                    break;
                case clasterType.Pozo:
                    reto.Add("Fena");
                    reto.Add("AnaRes");
                    reto.Add("Описание");
                    break;
                default:
                    reto = null;
                    break;
                }
            return reto;
            }
        /// <summary>
        /// Модификация от 4 февраля 2016 года
        /// Заложен от 8 января 2015 года
        /// </summary>
        /// <param name="typo"></param>
        /// <returns></returns>
        private clasterType TypoFromString(string typo)
        {
            clasterType reto = (clasterType)Enum.Parse(typeof(clasterType), typo);
            return reto;
        }

        public Guid LeoGuid { get { return ida; } }
        public clasterType LeoTypo { get { return obty; } }
        public string LeoNamo { get { return namo; } }
        public string LeoBigo { get { return bignamo; } }
        public List<string> LeoParams { get { return ltexto; } }

    }

    public enum clasterType {
        OKPReport = 0,
        Gamo = 201,
        Pozo = 202,
        AnTask = 203,
        None = 10001
        }

    public class Singleton<T> where T : class
    {
        protected Singleton() { }
        private sealed class SingletonCreator<S> where S : class
        {
            private static readonly S instance = (S)typeof(S).GetConstructor(
                BindingFlags.Instance | BindingFlags.NonPublic,
                null,
                new Type[0],
                new ParameterModifier[0]).Invoke(null);

            public static S CreatorInstance { get { return instance; } }
        }

        public static T Instance { get { return SingletonCreator<T>.CreatorInstance; } }
    }

    //Базовые объекты
    public class myClasterException : Exception
    {
        public myClasterException() { }
        public myClasterException(string message) : base(message) { }
        public myClasterException(string message, Exception inner) : base(message, inner) { }
    }

}
