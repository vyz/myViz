using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Security.Cryptography;

namespace pfVisualisator {

    public class mybino {
        private Guid ida;
        private string md5;
        private string sha;
        private string exto;
        private string shortfilename;
        private string comto;
        private byte[] valo;
        private int sizo;

        public mybino() { }

        /// <summary>
        /// Модификация от 13 мая 2015 года
        /// Заложен 27 марта 2015 года
        /// </summary>
        /// <param name="pbival"></param>
        /// <param name="pex"></param>
        /// <param name="pfname">Определяет кроме нормального имени, фиктивный только для расчёта и проверок бинарный элемент: ОКПBaseReport</param>
        /// <param name="pcommo"></param>
        public mybino( byte[] pbival, string pex, string pfname, string pcommo ) {
            ida = Guid.NewGuid();
            if (pfname.StartsWith("ОКПBaseReport")) {
                valo = new byte[1] { 0x66 };
            } else {
                valo = pbival;
                }
            comto = pcommo;
            shortfilename = pfname;
            exto = pex;
            md5 = tomd5(valo);
            sha = tosha(valo);
            sizo = pbival.Length;
            }

        /// <summary>
        /// Модификация от 14 апреля 2015 года
        /// Заложен от 9 января 2015 года
        /// </summary>
        /// <param name="Elemo"></param>
        public mybino(XElement Elemo)
        {
            this.ida = Guid.Parse(Elemo.Attribute("Bid").Value);
            this.exto = Elemo.Attribute("Extensiona").Value;
            this.sizo = int.Parse(Elemo.Attribute("Sizo").Value);
            this.shortfilename = Elemo.Attribute("FiloNamo").Value;
            this.md5 = Elemo.Attribute("Md5").Value;
            this.sha = Elemo.Attribute("Sha").Value;
            this.comto = Elemo.Attribute("Commento").Value;
            this.valo = Convert.FromBase64String(Elemo.Value);
        }

        /// <summary>
        /// Возвращает клон текущего объекта.
        /// Пока только глушак.
        /// Сама бинарная тема пока отложена
        /// Модификация от 25 февраля 2015 года
        /// Заложен 25 февраля 2015 года
        /// </summary>
        /// <returns></returns>
        public mybino Clone() {
            mybino reto = new mybino();
            return reto;
            }

        /// <summary>
        /// Модификация от 14 апреля 2015 года
        /// Заложен 23 декабря 2014 года
        /// </summary>
        /// <returns></returns>
        public XElement LeoXML() {
            XElement xRet = new XElement("BinaryDato", new XAttribute("Bid", ida), new XAttribute("Commento", comto), new XAttribute("Sizo", sizo), 
                                          new XAttribute("Extensiona", exto),
                                          new XAttribute("FiloNamo", shortfilename),
                                          new XAttribute("Md5", md5), new XAttribute("Sha", sha), Convert.ToBase64String(valo));
            return xRet;
            }

        /// <summary>
        /// Модификация от 27 марта 2015 года
        /// Заложен 27 марта 2015 года
        /// </summary>
        /// <param name="pfrom"></param>
        /// <returns></returns>
        private string tomd5(byte[] pfrom) {
            MD5CryptoServiceProvider provo = new MD5CryptoServiceProvider();
            byte[] data = provo.ComputeHash(pfrom);
            StringBuilder sBuilder = new StringBuilder();

            for (int i = 0; i < data.Length; i++) {
                sBuilder.Append(data[i].ToString("x2"));
                }
            return sBuilder.ToString();
            }

        /// <summary>
        /// Модификация от 27 марта 2015 года
        /// Заложен 27 марта 2015 года
        /// </summary>
        /// <param name="pfrom"></param>
        /// <returns></returns>
        private string tosha(byte[] pfrom) {
            SHA256CryptoServiceProvider provo = new SHA256CryptoServiceProvider(); 
            byte[] data = provo.ComputeHash(pfrom);
            StringBuilder sBuilder = new StringBuilder();

            for (int i = 0; i < data.Length; i++) {
                sBuilder.Append(data[i].ToString("x2"));
                }
            return sBuilder.ToString();
            }

        public string FiloNamo { get { return shortfilename; } }
        public string SizoStroke { get { return sizo.ToString("# ### ###"); } }
        public string Commento { get { return comto; } set { comto = value; } }
        public byte[] SelfBinaryObj { get { return valo; } }
        public string HaMD5 { get { return md5; } }
        public string HaSha256 { get { return sha; } }
        }
    }
