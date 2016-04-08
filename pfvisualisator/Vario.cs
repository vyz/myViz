using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Text;
using System.Text.RegularExpressions;


namespace pfVisualisator {
    /// Модификация от 25 марта 2016 года
    /// Заложен 28 декабря 2015 года
    public class Vario {
        private pozo bego;
        private List<Mova> lima;
        private List<pozo> lipa;
        private List<VarQvant> liqva;
        private string notata;

        /// <summary>
        /// Простейший вариант варианта
        /// liqva быть не может. Они считаются нулевыми. liqva = null
        /// Модификация от 29 января 2016 года
        /// Заложен 26 января 2016 года
        /// </summary>
        /// <param name="pp">Экземпляр позы</param>
        /// <param name="mvlist">Содержит только одни хода в нотации [a-h][1-8][a-h][1-8]</param>
        public Vario(pozo pp, string mvlist) {
            bego = pp;
            liqva = null;
            Mova mva; 
            pozo tpp = bego;
            string vnutr = @"([a-h][1-8][a-h][1-8])";
            foreach (Match m1 in Regex.Matches(mvlist, vnutr)) {
                string mv = m1.Groups[1].Value;
                if(tpp.ContraMov(mv, 2)) {
                    mva = tpp.GetFactMoveFilled();
                    if (lima == null) { lima = new List<Mova>(); }
                    lima.Add(mva);
                    tpp = tpp.GetPozoAfterControlMove();
                    if (lipa == null) { lipa = new List<pozo>(); }
                    lipa.Add(tpp);
                    }
                }
            notata = VanStrokeMovaRegion();
            }

        /// <summary>
        /// Модификация от 31 марта 2016 года
        /// Заложен 31 марта 2016 года
        /// </summary>
        /// <param name="pp"></param>
        /// <param name="limv"></param>
        /// <param name="lipz"></param>
        /// <param name="livq"></param>
        public Vario(pozo pp, List<Mova> limv, List<pozo> lipz, List<VarQvant> livq ) {
            bego = pp;
            lima = limv;
            lipa = lipz;
            liqva = livq;
            notata = VanStrokeMovaRegion();
            }
        
        /// <summary>
        /// Модификация от 5 февраля 2016 года
        /// Заложен 4 февраля 2016 года
        /// </summary>
        /// <param name="varel"></param>
        public Vario(XElement varel) {
            bego = new pozo(varel.Element("Feno").Value);
            notata = varel.Element("Tshepa").Value;
            List<Mova> tmplist = new List<Mova>();
            foreach (XElement aa in varel.Elements("Mova")) {//Порядок следования элементов обещан самой функцией
                tmplist.Add(new Mova(aa));
                }
            if( tmplist.Count > 0 ) {
                lima = tmplist;
                pozo tpp = bego;
                lipa = new List<pozo>();
                foreach (Mova mv in lima) {
                    if (tpp.ContraMov(mv.Shorto, 1)) {
                        tpp = tpp.GetPozoAfterControlMove();
                        lipa.Add(tpp);
                    } else {
                        throw new VisualisatorException(string.Format("Vario-Constructor(varel), Плохая мова  --{0}--{1}--", tpp.NumberMove, mv.Shorto));
                        }
                    }
                }
            List<VarQvant> tmpqva = new List<VarQvant>();
            foreach (XElement aa in varel.Elements("VarQuant")) {//А здесь порядок не важен, всё равно ему не верят и сортируют перед выводом
                tmpqva.Add(new VarQvant(aa));
                }
            if( tmpqva.Count > 0 ) {
                liqva = tmpqva;
                }
            }

        /// <summary>
        /// Модификация от 25 марта 2016 года
        /// Заложен 27 января 2016 года
        /// </summary>
        /// <returns></returns>
        public XElement XMLOut() {
            XElement reto = new XElement("Vario");
            reto.Add(new XElement("Feno", bego.fenout()));
            reto.Add(new XElement("Tshepa", notata));
            int kavo = 0;
            int fstop = 0;
            int uqva = 0;
            List<VarQvant> sorta = null;
            if( null != lima ) {
                kavo = lima.Count;
                fstop = kavo;
                }
            if (null != liqva) {
                sorta = liqva.OrderBy(F => F.Numa).ToList();
                fstop = liqva[uqva].Numa;
                }
            for (int umo = 0; umo < kavo; umo++) {
                while (fstop == umo) {
                    reto.Add(sorta[uqva++].XMLOut());
                    if (uqva < sorta.Count) {
                        fstop = sorta[uqva].Numa;
                    } else {
                        fstop = kavo;
                        }
                    }
                reto.Add(lima[umo].XMLOut());
                }
            //Комменты и варианты могут быть и после всех ходиков варианта
            if (sorta != null) {
                for (; uqva < sorta.Count; uqva++) {
                    reto.Add(sorta[uqva].XMLOut());
                    }
                }
            return reto;
            }

        /// <summary>
        /// Модификация от 29 января 2016 года
        /// Заложен 29 января 2016 года
        /// </summary>
        /// <returns></returns>
        private List<string> CreateMovaRegionWithoutComments() {
            List<string> reto = null;
            const int STLE = 78;

            if (null != lima) {
                reto = new List<string>();
                StringBuilder stbu = new StringBuilder(STLE);
                bool prmove = bego.IsQueryMoveWhite;
                int mvnumber = bego.NumberMove;
                if (!prmove) { //Для первого черного хода специфика
                    stbu.AppendFormat("{0}. ...", mvnumber.ToString().Trim());
                    }
                foreach (Mova aa in lima) {
                    string bb = aa.Shorto;
                    string snm = string.Empty;
                    if (prmove) {
                        snm = mvnumber.ToString().Trim() + ".";
                        if (stbu.Length + snm.Length > STLE) {
                            reto.Add(stbu.ToString());
                            stbu = new StringBuilder(STLE);
                            stbu.Append(snm);
                        } else {
                            stbu.Append((stbu.Length > 0 ? " " : "") + snm);
                            }
                    } else {
                        mvnumber++;
                        }
                    prmove = !prmove;
                    if (stbu.Length + bb.Length > STLE) {
                        reto.Add(stbu.ToString());
                        stbu = new StringBuilder(STLE);
                        stbu.Append(bb);
                    } else {
                        stbu.Append(" " + bb);
                        }
                    }
                if (stbu.Length > 0) {
                    reto.Add(stbu.ToString());
                    }
                }
            return reto;
            }

        /// <summary>
        /// Модификация от 29 января 2016 года
        /// Заложен 29 января 2016 года
        /// </summary>
        /// <returns></returns>
        private string VanStrokeMovaRegion() {
            string reto = string.Empty;
            StringBuilder sbb = new StringBuilder();
            foreach( string aa in CreateMovaRegionWithoutComments() ) {
                if (sbb.Length > 0) {
                    sbb.Append(Environment.NewLine);
                    }
                sbb.Append(aa);
                }
            if (sbb.Length > 0) {
                reto = sbb.ToString();
                }
            return reto;
            }

#region--------------------------Свойства объекта-----------------------------------------
        public string OnlyMova { get { return notata; } }
        public string Begino { get { return null == lima ? "Пусто" : lima[0].Shorto; } }
        public pozo LastPo { get { return lipa[lipa.Count - 1]; } }
        public List<Mova> MovaList { get { return lima; } }
        public List<pozo> PozoList { get { return lipa; } }
        public int Numerok { get { return bego.NumberMove; } }
#endregion-----------------------Свойства объекта-----------------------------------------

    }

    /// <summary>
    /// Модификация от 24 марта 2016 года
    /// Заложен 26 января 2016 года
    /// </summary>
    public class VarQvant {
        private int numa;  //Это порядковый номер мовы, перед которой находится данная позиция или вариант.
                           //Может и равняться самому количестыу мов. Т.е. быть после последней мовы.
        private string como;
        private Vario vrvnu;

        public VarQvant()
        {
            numa = 0;
            como = string.Empty;
            vrvnu = null;
        }

        public VarQvant(int pi, string ps, Vario pv) {
            numa = pi;
            como = (ps == null) ? string.Empty : ps;
            vrvnu = pv;
            }

        /// <summary>
        /// Модификация от 4 февраля 2016 года
        /// Заложен 4 февраля 2016 года
        /// </summary>
        /// <param name="xel"></param>
        public VarQvant(XElement xel) {
            numa = int.Parse(xel.Attribute("VarNumo").Value);
            XElement aa = xel.Element("Commento");
            como = (null == aa) ? string.Empty : aa.Value;
            aa = xel.Element("Vario");
            if (null != aa) {
                vrvnu = new Vario(aa);
                }
            }

        /// <summary>
        /// Модификация от 4 февраля 2016 года
        /// Заложен 4 февраля 2016 года
        /// </summary>
        /// <returns></returns>
        public XElement XMLOut()
        {
            XElement reto = new XElement("VarQuant");
            reto.Add(new XAttribute("VarNumo", numa));
            if (como.Length > 0) { reto.Add(new XElement("Commento", como)); }
            if (null != vrvnu) { reto.Add(vrvnu.XMLOut()); }
            return reto;
        }

        public int Numa { get { return numa; } }
        }
    
    }
