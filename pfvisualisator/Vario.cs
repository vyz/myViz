using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Text;
using System.Text.RegularExpressions;


namespace pfVisualisator {
    /// Модификация от 26 января 2016 года
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

        public Vario(pozo pp, List<XElement> lixmv) { 
            //
        }
        public Vario(XElement varel) { }

        /// <summary>
        /// Модификация от 27 января 2016 года
        /// Заложен 27 января 2016 года
        /// </summary>
        /// <returns></returns>
        public XElement XMLOut()
        {
            XElement reto = new XElement("Vario");
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

        /// <summary>
        /// Модификация от 26 января 2016 года
        /// Заложен 26 января 2016 года
        /// </summary>
        private class VarQvant {
            private int numa;
            private string como;
            private Vario vrvnu;

            public VarQvant() {
                numa = 0;
                como = string.Empty;
                vrvnu = null;
                }

            public VarQvant(int pi, string ps, Vario pv) {
                numa = pi;
                como = ps;
                vrvnu = pv;
                }
            
            }

#region--------------------------Свойства объекта-----------------------------------------
        public string OnlyMova { get { return notata; } }
        public pozo LastPo { get { return lipa[lipa.Count - 1]; } }
#endregion-----------------------Свойства объекта-----------------------------------------

    }


    }
