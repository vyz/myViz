using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Reflection;


namespace pfVisualisator {
    public class myleo {
        private Guid ida;
        private leoType obty;
        protected string namo;
        protected string bignamo;
        private List<myhistory> lhistory;
        protected List<mypict> lpicto;
        private List<mybino> lbino;
        protected List<string> ltexto;
        protected myTago tago;

        /// <summary>
        /// Модификация от 2 февраля 2015 года
        /// 
        /// </summary>
        /// <param name="typo"></param>
        protected myleo(leoType typo)
        {
            ida = Guid.NewGuid();
            obty = typo;
            tago = new myTago();
        }

        /// <summary>
        /// Модификация от 29 апреля 2015 года
        /// Заложен 25 февраля 2015 года
        /// </summary>
        /// <param name="pida"></param>
        /// <param name="pobty"></param>
        /// <param name="pnamo"></param>
        /// <param name="pbig"></param>
        /// <param name="plpict"></param>
        /// <param name="plbin"></param>
        /// <param name="pltex"></param>
        /// <param name="ptag"></param>
        private myleo(Guid pida, leoType pobty, string pnamo, string pbig, List<mypict> plpict, List<mybino> plbin, List<string> pltex, myTago ptag) {
            ida = pida;
            obty = pobty;
            namo = pnamo;
            bignamo = pbig;
            if (plpict != null) {
                lpicto = new List<mypict>(plpict.Count);
                foreach (mypict aa in plpict) {
                    if (aa != null) {
                        lpicto.Add(aa.Clone());
                        }
                    }
                }
            else lpicto = null;
            if (plbin != null) {
                lbino = new List<mybino>(plbin.Count);
                foreach (mybino aa in plbin) {
                    if (aa != null) {
                        lbino.Add(aa);
                        }
                    }
                }
            else lbino = null;
            if (pltex != null) {
                ltexto = new List<string>(pltex.Count);
                foreach (string aa in pltex) {
                    ltexto.Add(aa);
                    }
                }
            else ltexto = null;
            if (ptag != null) {
                tago = new myTago(ptag.TStroke);
            } else {
                tago = new myTago();
                }
            }

        /// <summary>
        /// Модификация от 5 марта 2015 года
        /// Заложен 8 января 2015 года
        /// </summary>
        /// <param name="Elemo"></param>
        public myleo(XElement Elemo) {
            ida = Guid.Parse(Elemo.Attribute("lid").Value);
            obty = TypoFromString(Elemo.Attribute("typo").Value);
            namo = Elemo.Element("Namo").Value;
            bignamo = Elemo.Element("BigNamo").Value;
            XElement aa = Elemo.Element("Histories");
            if(aa != null) {
                lhistory = new List<myhistory>();
                foreach( XElement bb in aa.Elements("History") )  {
                    lhistory.Add( new myhistory(bb) );
                    }
                }
            aa = Elemo.Element("Picturyki");
            if (aa != null) {
                lpicto = new List<mypict>();
                foreach (XElement bb in aa.Elements("Picta")) {
                    lpicto.Add(new mypict(bb));
                    }
                }
            aa = Elemo.Element("Binaryki");
            if (aa != null) {
                lbino = new List<mybino>();
                foreach (XElement bb in aa.Elements("BinaryDato")) {
                    lbino.Add(new mybino(bb));
                    }
                }
            aa = Elemo.Element("Parameters");
            if (aa != null) {
                ltexto = new List<string>();
                foreach (XElement bb in aa.Elements("TParo")) {
                    ltexto.Add(bb.Value);
                    }
                }
            aa = Elemo.Element("Tags");
            tago = new myTago();
            if (aa != null) {
                foreach (XElement bb in aa.Elements("Tago")) {
                    tago.AddTag(bb.Value);
                    }
                }
            }

        /// <summary>
        /// Модификация от 2 февраля 2015 года
        /// Заложен 23 декабря 2014 года
        /// </summary>
        /// <returns></returns>
        public XElement LeoToXML() {
            XElement xRet = new XElement("Leo", new XAttribute("lid", ida), new XAttribute("typo", obty));
            xRet.Add(new XElement("Namo", namo));
            xRet.Add(new XElement("BigNamo", bignamo));
            if(lhistory != null) {
                XElement aa = new XElement("Histories");
                foreach (myhistory bb in lhistory) {
                    aa.Add(bb.LeoXML());
                    }
                xRet.Add(aa);
                }
            if(lpicto != null) {
                XElement aa = new XElement("Picturyki");
                foreach (mypict bb in lpicto) {
                    aa.Add(bb.LeoXML());
                    }
                xRet.Add(aa);
                }
            if(lbino != null) {
                XElement aa = new XElement("Binaryki");
                foreach (mybino bb in lbino) {
                    aa.Add(bb.LeoXML());
                    }
                xRet.Add(aa);
                }
            if(ltexto != null) {
                XElement aa = new XElement("Parameters");
                foreach (string bb in ltexto) {
                    aa.Add(new XElement("TParo", bb));
                    }
                xRet.Add(aa);
                }
            if (tago.TCount > 0) {
                XElement aa = new XElement("Tags");
                foreach (string bb in tago.TSet) {
                    aa.Add(new XElement("Tago", bb));
                    }
                xRet.Add(aa);
                }
            return xRet;
            }

        /// <summary>
        /// Модификация от 25 февраля 2015 года
        /// Заложен 25 февраля 2015 года
        /// </summary>
        /// <returns></returns>
        public myleo HistoricalClone() {
            myleo reto = new myleo(ida, obty, namo, bignamo, lpicto, lbino, ltexto, tago);
            return reto;
            }

        /// <summary>
        /// Модификация от 24 декабря 2015 года
        /// Заложен 12 марта 2015 года
        /// </summary>
        /// <returns></returns>
        public List<string> GetParamoNames() {
            List<string> reto = new List<string>();
            switch (obty) {
                case leoType.OKPReport:
                    reto.Add("Коммент из базы");
                    reto.Add("Описание");
                    reto.Add("Файлы скриптов");
                    reto.Add("SQL-запрос");
                    reto.Add("GUID отчёта");
                    break;
                case leoType.Gamo:
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
                case leoType.Pozo:
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
        /// Проверка на расхождение при сохранении с историей
        /// Модификация от 27 февраля 2015 года
        /// Заложен 26 февраля 2015 года
        /// </summary>
        /// <param name="hiob">Отложенный для исторического сохранения объект (снимок перед открытием в режиме редактирования)</param>
        /// <returns>Истина при нахождении существенных для истории отличий</returns>
        public bool PoiskDiffov(myleo hiob) {
            bool reto = false;
            if (this.ida == hiob.LeoGuid && this.obty == hiob.LeoTypo) {
                //По иному как-бы и не должно, но на всякий случай
                string svnamo = hiob.eqnamo(this.namo);
                if (svnamo != null) { return true; }
                string svbigo = hiob.eqbigo(this.bignamo);
                if (svbigo != null) { return true; }
                myTago svtago = hiob.eqtago(this.tago);
                if (svtago != null) { return true; }
                List<string> svtexo = hiob.eqtexto(this.ltexto);
                if (svtexo != null) { return true; }
                List<mypict> svpixo = hiob.eqpicto(this.lpicto);
                if (svpixo != null) { return true; }
                List<mybino> svbino = hiob.eqbino(this.lbino);
                if (svbino != null) { return true; }
                }
            return reto;
            }

        /// <summary>
        /// Модификация от 2 марта 2015 года
        /// Заложен 2 марта 2015 года
        /// </summary>
        /// <param name="hiob"></param>
        /// <returns></returns>
        public myleo GetDifferenceHistor(myleo hiob) {
            myleo reto = null;
            if (this.ida == hiob.LeoGuid && this.obty == hiob.LeoTypo) {
                //По иному как-бы и не должно, но на всякий случай
                string svnamo = hiob.eqnamo(this.namo);
                string svbigo = hiob.eqbigo(this.bignamo);
                myTago svtago = hiob.eqtago(this.tago);
                List<string> svtexo = hiob.eqtexto(this.ltexto);
                List<mypict> svpixo = hiob.eqpicto(this.lpicto);
                List<mybino> svbino = hiob.eqbino(this.lbino);
                reto = new myleo(this.ida, this.obty, svnamo, svbigo, svpixo, svbino, svtexo, svtago);
                }
            return reto;
            }

        /// <summary>
        /// Модификация от 6 марта 2015 года
        /// Заложен 6 марта 2015 года
        /// </summary>
        /// <param name="aa"></param>
        public void AddHistoryObjecto(myhistory aa) {
            if (lhistory == null) {
                lhistory = new List<myhistory>();
                }
            lhistory.Add(aa);
            }

        /// <summary>
        /// Модификация от 29 апреля 2015 года
        /// Заложен 29 апреля 2015 года
        /// </summary>
        /// <param name="aa"></param>
        public void AddBinaryObjecto(mybino aa) {
            if (lbino == null) {
                lbino = new List<mybino>();
                }
            lbino.Add(aa);
            }

        /// <summary>
        /// Сравнение объектной переменной namo с переданной.
        /// null при равенстве, иначе возвращается объектная для сохранения в истории
        /// переданная принадлежит новому объекту и предполагается, что она его затрёт
        /// Модификация от 26 февраля 2015 года
        /// Заложен 26 февраля 2015 года
        /// </summary>
        /// <param name="nwnamo">принадлежит новому объекту</param>
        /// <returns>null или старое значение</returns>
        private string eqnamo(string nwnamo) {
            string reto = null;
            if (nwnamo != this.namo) { reto = this.namo; }
            return reto;
            }

        /// <summary>
        /// Сравнение объектной переменной bignamo с переданной.
        /// null при равенстве, иначе возвращается объектная для сохранения в истории
        /// переданная принадлежит новому объекту и предполагается, что она его затрёт
        /// Модификация от 26 февраля 2015 года
        /// Заложен 26 февраля 2015 года
        /// </summary>
        /// <param name="nwbigo">принадлежит новому объекту</param>
        /// <returns>null или старое значение</returns>
        private string eqbigo(string nwbigo) {
            string reto = null;
            if (nwbigo != this.bignamo) { reto = this.bignamo; }
            return reto;
            }

        /// <summary>
        /// Сравнение объектной переменной tago с переданной.
        /// null при равенстве, иначе возвращается объектная для сохранения в истории
        /// переданная принадлежит новому объекту и предполагается, что она его затрёт
        /// Модификация от 26 февраля 2015 года
        /// Заложен 26 февраля 2015 года
        /// </summary>
        /// <param name="nwtago">принадлежит новому объекту</param>
        /// <returns>null или старое значение</returns>
        private myTago eqtago(myTago nwtago) {
            myTago reto = null;
            if (nwtago.TStroke != this.tago.TStroke) { reto = new myTago(this.tago.TStroke); }
            return reto;
            }

        /// <summary>
        /// Сравнение объектного списка параметров с переданным.
        /// null при равенстве, иначе возвращается список с отличными объектными элементами (строки) для сохранения в истории
        /// сохраняются только старые при их удалении - добавление нового элемента не фиксируется
        /// В списке сохраняются только измененные элементы, остальные null.
        /// Модификация от 27 февраля 2015 года
        /// Заложен 26 февраля 2015 года
        /// </summary>
        /// <param name="nwtexo">принадлежит новому объекту</param>
        /// <returns>null или список с null-ами и старыми строками параметров</returns>
        private List<string> eqtexto(List<string> nwtexo) {
            bool bfla = false;
            List<string> reto = new List<string>(this.ltexto.Count);
            for(int i = 0; i < this.ltexto.Count; i++) {
                string aa = null;
                if (!(i < nwtexo.Count && this.ltexto[i] == nwtexo[i])) {
                    aa = this.ltexto[i];
                    bfla = true;
                    }
                reto.Add(aa);
                }
            if(!bfla) {
                reto = null; 
                }
            return reto;
            }

        /// <summary>
        /// Сравнение объектного списка картинок с переданным.
        /// null при равенстве, иначе возвращается список с отличными объектными элементами (картинки) для сохранения в истории
        /// сохраняются только старые при их удалении - добавление нового элемента не фиксируется
        /// В списке сохраняются только измененные элементы, остальные null.
        /// Модификация от 27 февраля 2015 года
        /// Заложен 27 февраля 2015 года
        /// </summary>
        /// <param name="nwpixo">принадлежит новому объекту</param>
        /// <returns>null или список с null-ами и старыми картинками</returns>
        private List<mypict> eqpicto(List<mypict> nwpixo) {
            bool bfla = false;
            if(this.lpicto == null || this.lpicto.Count == 0) { return null; }
            if(nwpixo == null || nwpixo.Count == 0) { return this.lpicto; }
            List<mypict> reto = new List<mypict>(this.lpicto.Count);
            List<Guid> teguid = nwpixo.Select(P => P.Gvido).ToList();
            foreach (mypict aa in this.lpicto) {
                if( teguid.Contains(aa.Gvido) ) {
                    reto.Add(null); 
                } else {
                    bfla = true;
                    reto.Add(aa);
                    }
                }
            if (bfla) { return reto; }
            return null;
            }

        private List<mybino> eqbino(List<mybino> nwbino) {
            //Пока только так
            return null;
            }

        /// <summary>
        /// Модификация от 19 ноября 2015 года
        /// Заложен от 8 января 2015 года
        /// </summary>
        /// <param name="typo"></param>
        /// <returns></returns>
        private leoType TypoFromString(string typo) {
            leoType reto = leoType.None;
            switch (typo) {
                case "OKPReport":
                    reto = leoType.OKPReport;
                    break;
                case "Gamo":
                    reto = leoType.Gamo;
                    break;
                case "Pozo":
                    reto = leoType.Pozo;
                    break;
            }
            return reto;
            }

        public Guid LeoGuid { get { return ida; } }
        public leoType LeoTypo { get { return obty; } }
        public List<myhistory> LeoHistoList { get { return lhistory; } }
        public List<mybino> LeoBinoList { get { return lbino; } }
        public string LeoNamo { get { return namo; } }
        public string LeoBigo { get { return bignamo; } }
        public myTago LeoTago { get { return tago; } }
        public string LeoTagoStroke { get { return tago.TStroke; } }
        public List<mypict> LeoPictures { get { return lpicto; } }
        public List<string> LeoParams { get { return ltexto; } }
    }

    public class Singleton<T> where T : class {
        protected Singleton() { }
        private sealed class SingletonCreator<S> where S : class {
            private static readonly S instance = (S)typeof(S).GetConstructor(
                BindingFlags.Instance | BindingFlags.NonPublic,
                null,
                new Type[0],
                new ParameterModifier[0]).Invoke(null);

            public static S CreatorInstance { get { return instance; } }
            }

        public static T Instance { get { return SingletonCreator<T>.CreatorInstance; } }
        }

    public enum leoType {
        OKPReport = 0,
        Gamo = 201,
        Pozo = 202,
        None = 10001
        }

    //Базовые объекты
    public class VisualisatorException : Exception {
        public VisualisatorException() { }
        public VisualisatorException(string message) : base(message) { }
        public VisualisatorException(string message, Exception inner) : base(message, inner) { }
        }


    }

