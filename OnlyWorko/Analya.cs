using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Text;

namespace OnlyWorko {

#region--------------------------------ПЕРВИЧНЫЙ КЛАСС--Analya------------------------
    /// <summary>
    /// Модификация от 4 апреля 2017 года
    /// Заложен 27 марта 2017 года
    /// </summary>
    public class Analya {
        const int dipazon = 20;
        int z1;
        int zn;
        int zr;
        int nodi;
        int dz1;
        int dzn;
        string basta;
        string rilo;
        int orda;
        int kavo;
        string sovokup;
        string ffposo;
        
        public Analya() { }

        /// <summary>
        /// Модификация от 5 апреля 2017 года
        /// Заложен 27 марта 2017 года
        /// </summary>
        /// <param name="rda">Набор анализов</param>
        /// <param name="pz1old">Предыдущая лучшая оценка</param>
        /// <param name="pznold">Предыдущая энная оценка</param>
        /// <param name="pmvr">Ход в партии</param>
        /// <param name="pffp">Полный путь к файлу с проанализированной позицией</param>
        public Analya(List<Valuing> rda, int pz1old, int pznold, Mova pmvr, string pffp)
        {
            if (rda == null) {
                throw new myClasterException("Analya -> null Valuing");
                }
            bool colo = rda[0].SelfVarianto.BegoPo.IsQueryMoveWhite;
            int numbero = rda[0].SelfVarianto.BegoPo.NumberMove;
            kavo = rda[0].SelfVarianto.BegoPo.AvaQvo;
            basta = rda[0].MovoVan;
            ffposo = pffp;
            int qz = rda[0].Valo;
            z1 = colo ? qz : -qz;
            rilo = pmvr.Shorto;
            orda = 0;
            if (rilo == basta) { orda = 1; }
            int i = 1;
            int smz = z1;
            nodi = 0;
            for (; i < rda.Count; i++) {
                Valuing aa = rda[i];
                if (aa.SelfVarianto.Begino == rilo) { orda = i + 1; }
                if ((qz - aa.Valo) < dipazon) {
                    smz += colo ? aa.Valo : -aa.Valo;
                } else if( orda > 0 ) {
                    break;
                    }
                }
            if( orda == 0 ) {
                orda = 99; 
                zr = -888;
            } else { 
                zr = colo ? rda[orda-1].Valo : -rda[orda-1].Valo; 
                }
            nodi = i;
            zn = (int)((double)smz) / i;
            dz1 = z1 - pz1old;
            dzn = zn - pznold;
            sovokup = string.Format(@"{0}{1}:Z({2})={3} R({4} - {5})={6} N({7})={8} Dl {9} n {10} из {11}", numbero, colo ? "w" : "b", basta, z1, rilo, orda, zr, nodi, zn, dz1, dzn, kavo);
            }

        /// <summary>
        /// Модификация от 5 апреля 2017 года
        /// Заложен 30 марта 2017 года
        /// </summary>
        /// <param name="xel"></param>
        public Analya(XElement xel) {
            foreach( XNode bb in xel.Nodes() ) {
                if (bb.NodeType == System.Xml.XmlNodeType.Text) {
                    ffposo = bb.ToString();
                    }
                }
            
            XElement aa = xel.Element("Zety");
            z1 = int.Parse(aa.Attribute("Z1").Value);
            zn = int.Parse(aa.Attribute("ZN").Value);
            zr = int.Parse(aa.Attribute("ZR").Value);
            aa = xel.Element("Delty");
            dz1 = int.Parse(aa.Attribute("DZ1").Value);
            dzn = int.Parse(aa.Attribute("DZN").Value);
            nodi = int.Parse(aa.Attribute("Nody").Value);
            aa = xel.Element("Muvy");
            basta = aa.Attribute("Besto").Value;
            rilo = aa.Attribute("Rilo").Value;
            orda = int.Parse(aa.Attribute("Orda").Value);
            aa = xel.Element("Sovok");
            sovokup = aa.Value;
            kavo = int.Parse(aa.Attribute("Kavo").Value);
            }

        /// <summary>
        /// Модификация от 4 апреля 2017 года
        /// Заложен 30 марта 2017 года
        /// </summary>
        /// <returns></returns>
        public XElement XMLOut() {
            XElement reto = new XElement("Analya", ffposo);
            reto.Add(new XElement("Zety", new XAttribute("Z1", z1), new XAttribute("ZN", zn), new XAttribute("ZR", zr)));
            reto.Add(new XElement("Delty", new XAttribute("DZ1", dz1), new XAttribute("DZN", dzn), new XAttribute("Nody", nodi)));
            reto.Add(new XElement("Muvy", new XAttribute("Besto", basta), new XAttribute("Rilo", rilo), new XAttribute("Orda", orda)));
            reto.Add(new XElement("Sovok", new XAttribute("Kavo", kavo), sovokup));
            return reto;
            }

#region--------------------------Свойства объекта-----------------------------------------
        public int ZFirsto { get { return z1; } }
        public int ZCommo { get { return zn; } }
        public string Besto { get { return basta; } }
        public string Realo { get { return rilo; } }
        public int Ordo { get { return orda; } }
        public int Qavo { get { return kavo; } }
        public string FFPoso { get { return ffposo; } }
        public string Descripto { get { return sovokup; } }
#endregion-----------------------Свойства объекта-----------------------------------------

        }
#endregion-----------------------------ПЕРВИЧНЫЙ КЛАСС--Analya------------------------


#region--------------------------------ДРУГОЙ КЛАСС--Gana-----------------------------
    /// <summary>
    /// Модификация от 4 апреля 2017 года
    /// Заложен 27 марта 2017 года
    /// </summary>
    public class Gana : myclast
    {
        string filonamo;
        string fulofilename;
        bool ready;
        DateTime dcreato;
        DateTime dstaro;
        DateTime dready;
        vGamo subjo;
        int mvs;
        int mvf;
        int mvc;
        vlEngino wen;
        int timo;
        int qvaro;
        List<Analya> lenya;

        /// <summary>
        /// Модификация от 30 марта 2017 года
        /// Заложен 30 марта 2017 года
        /// </summary>
        public Gana() : base(clasterType.AnTask) {
            string[] akva = new string[5];
            ltexto = akva.ToList();
            }

        /// <summary>
        /// Модификация от 27 марта 2017 года
        /// Заложен 27 марта 2017 года
        /// </summary>
        /// <param name="pwg"></param>
        /// <param name="ps"></param>
        /// <param name="pf"></param>
        public Gana(vGamo pwg, int ps, int pf) : this(pwg, ps, pf, vlEngino.Houdini_3a_Pro_w32, 3, 5) { }

        /// <summary>
        /// Модификация от 31 марта 2017 года
        /// Заложен 27 марта 2017 года
        /// </summary>
        /// <param name="pwg"></param>
        /// <param name="ps"></param>
        /// <param name="pf"></param>
        /// <param name="pen"></param>
        /// <param name="pminuto"></param>
        /// <param name="pqvo"></param>
        public Gana(vGamo pwg, int ps, int pf, vlEngino pen, int pminuto, int pqvo)
            : base(clasterType.AnTask) {
            string[] akva = new string[5];
            ltexto = akva.ToList();
            subjo = pwg;
            subjo.EtaloCreate();
            if (ps <= 0) { ps = 1; }
            mvs = mvc = ps;
            mvf = pf;
            if (mvf > subjo.Gamma.ListoMovo.Count) { mvf = subjo.Gamma.ListoMovo.Count; }
            wen = pen;
            timo = pminuto;
            qvaro = pqvo;
            dcreato = DateTime.Now;
            ready = false;
            namo = string.Format("Для {0} с {1} по {2}", subjo.BigNamo, mvs, mvf);
            bignamo = string.Format("Задан {0}", dcreato);
            Descripto = "Анализ ещё не проводился";
            Fitchy = string.Format("{0}-{1}-{2}-{3}-{4}-{5}", mvs, mvf, mvc, timo, qvaro, wen);
            filonamo = LeoGuid.ToString("N");
            ltexto[2] = dcreato.ToString();
            lenya = new List<Analya>();
            }

        /// <summary>
        /// Модификация от 30 марта 2017 года
        /// Заложен 30 марта 2017 года
        /// </summary>
        /// <param name="Elemo"></param>
        public Gana(XElement Elemo)
            : base(Elemo) {
            FitchyToInty();
            PodyemDatov();
            filonamo = LeoGuid.ToString("N");
            }

        /// <summary>
        /// Многократный расчёт по заданным условиям после каждого полухода в заданных рамках
        /// Модификация от 5 апреля 2017 года
        /// Заложен 31 марта 2017 года
        /// </summary>
        /// <param name="tamo">Отведенный на выполнение работ с данным заданием временной промежуток</param>
        /// <param name="outposodir">Каталог для складывания проанализированных позиций. Уже существует. Не проверяется на житие.</param>
        /// <returns>Истина в случае нормального окончания выполнения задания, ложь при выходе по "tamo", либо по антициклу в 500 разов</returns>
        public bool Worko(TimeSpan tamo, string outposodir) {
            bool reto = false;
            subjo.EtaloCreate();
            DateTime prov = DateTime.Now.Add(tamo);
            for (int i = 500; i > 0; i--) {
                if (OdinRaz(outposodir)) {
                    if (DateTime.Now > prov) {
                        break;
                        }
                    if (ready) {
                        reto = true;
                        break;
                        }
                    }
                }
            return reto;
            }

        /// <summary>
        /// Модификация от 5 апреля 2017 года
        /// Заложен 31 марта 2017 года
        /// </summary>
        /// <param name="outposodir">Каталог для складывания проанализированных позиций. Уже существует. Не проверяется на житие.</param>
        /// <returns></returns>
        private bool OdinRaz(string outposodir) {
            bool reto = false;
            Mova cumv = subjo.Gamma.ListoMovo[mvc-1];
            vPoza wrk = new vPoza(subjo.Gamma.ListoPozo[mvc-1], subjo.BigNamo);
            wrk.Descripto = string.Format("Создана {0} при автоанализе {1} для партии {2}", DateTime.Now.ToString(), LeoGuid.ToString(), subjo.LeoGuid.ToString());
            if (dstaro == DateTime.MinValue) { 
                dstaro = DateTime.Now;
                ltexto[3] = dstaro.ToString();
                bignamo = string.Format("Задан {0} Первый старт {1}", dcreato, dstaro);
                }
            wrk.rasschet(wen, timo, qvaro);
            int oldpz1, oldpzn;
            if (lenya == null || lenya.Count == 0)  { 
                oldpz1 = oldpzn = 0;
                lenya = new List<Analya>();
            } else {
                oldpz1 = lenya[lenya.Count-1].ZFirsto;
                oldpzn = lenya[lenya.Count-1].ZCommo;
                }
            string fipo = outposodir + "\\" + filonamo + "_" + mvc.ToString("000") + ".rdy";
            Analya aly = new Analya(wrk.SetoAnalo, oldpz1, oldpzn, cumv, fipo);
            lenya.Add(aly);
            wrk.SavoInXmlFilo(fipo);
            Descripto = string.Format("Последний расчёт {0}", DateTime.Now);
            if (mvc == mvf) {
                ready = true;
                dready = DateTime.Now;
                ltexto[4] = dready.ToString();
                Descripto = string.Format("Расчёты завершены {0}", dready);
            } else {
                mvc++;
                Fitchy = string.Format("{0}-{1}-{2}-{3}-{4}-{5}", mvs, mvf, mvc, timo, qvaro, wen);
                }
            SavoInXmlFilo();
            reto = true;
            return reto;
            }

        /// <summary>
        /// Модификация от 30 марта 2017 года
        /// Заложен 30 марта 2017 года
        /// </summary>
        /// <returns></returns>
        public override XElement Dopico()
        {
            XElement reto = new XElement("Dopico");
            if(lenya.Count > 0) {
                foreach (Analya aa in lenya) {
                    reto.Add(aa.XMLOut());
                    }
                }
            reto.Add(subjo.LeoToXML());
            return reto;
        }

        /// <summary>
        /// Модификация от 4 апреля 2017 года
        /// Заложен 30 марта 2017 года
        /// </summary>
        /// <param name="xel"></param>
        public override void Dopico(XElement xel) {
            foreach (XElement aa in xel.Elements("Analya")) {
                if (null == lenya) { lenya = new List<Analya>(); }
                lenya.Add(new Analya(aa));
                }
            subjo = new vGamo(xel.Element("Leo"));
            //subjo.EtaloCreate();
            }

        /// <summary>
        /// Модификация от 30 марта 2017 года
        /// Заложен 30 марта 2017 года
        /// <param name="diro">Каталог для сохранения файла</param>
        /// </summary>
        public void SavoInXmlFilo(string diro) {
            fulofilename = diro + "\\" + filonamo + ".xml";
            this.SavoInXmlFilo();
            }

        /// <summary>
        /// Модификация от 30 марта 2017 года
        /// Заложен 30 марта 2017 года
        /// </summary>
        private void SavoInXmlFilo() {
            XComment xcom = new XComment(string.Format("Набор для анализа модификация от {0} ", DateTime.Now.ToString()));
            XDocument doca = new XDocument(new XDeclaration("1.0", "utf-8", "yes"), xcom);
            XElement ruta = new XElement("Gana");
            ruta.Add(LeoToXML());
            doca.Add(ruta);
            doca.Save(fulofilename);
            }

        /// <summary>
        /// Модификация от 30 марта 2017 года
        /// Заложен 30 марта 2017 года
        /// </summary>
        /// <param name="filo"></param>
        /// <returns></returns>
        public static Gana CreateExemplaroFromXmlFile(string filo) {
            XDocument doca = XDocument.Load(filo);
            Gana reto = new Gana(doca.Element("Gana").Element("Leo"));
            reto.FullNamoFile = filo;
            return reto;
            }

        /// <summary>
        /// Модификация от 30 марта 2017 года
        /// Заложен 30 марта 2017 года
        /// </summary>
        private void FitchyToInty() {
            string[] ss = Fitchy.Split('-');
            try {
                mvs = int.Parse(ss[0]);
                mvf = int.Parse(ss[1]);
                mvc = int.Parse(ss[2]);
                timo = int.Parse(ss[3]);
                qvaro = int.Parse(ss[4]);
                wen = (vlEngino)Enum.Parse(typeof(vlEngino), ss[5]);
            } catch (Exception ex) {
                LogoCM.OutString("ERR Analya-->FitchyToInty :: " + ex.Message);
                }
            }

        /// <summary>
        /// Модификация от 5 апреля 2017 года
        /// Заложен 30 марта 2017 года
        /// </summary>
        private void PodyemDatov() {
            if(ltexto[2].Length > 0) { dcreato = DateTime.Parse(ltexto[2]); }
            if (ltexto[3].Length > 0) { dstaro = DateTime.Parse(ltexto[3]); } else { dstaro = DateTime.MinValue; }
            if (ltexto[4].Length > 0) { dready = DateTime.Parse(ltexto[4]); } else { dready = DateTime.MinValue; }
            }

#region--------------------------Свойства объекта-----------------------------------------
        public string Namo { get { return namo; } set { namo = value; } }
        public string BigNamo { get { return bignamo; } set { bignamo = value; } }
        public string Descripto { get { return ltexto[0]; } set { ltexto[0] = value; } }
        public string Fitchy { get { return ltexto[1]; } set { ltexto[1] = value; } }
        public string FullNamoFile { get { return fulofilename; } set { fulofilename = value; } }
        public vGamo Grusha { get { return subjo; } }
        public bool Ready { get { return ready; } }
#endregion-----------------------Свойства объекта-----------------------------------------

    }
#endregion--------------------------------ДРУГОЙ КЛАСС--Gana--------------------------

}
