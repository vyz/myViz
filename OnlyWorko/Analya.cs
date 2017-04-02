using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Text;

namespace OnlyWorko {

#region--------------------------------ПЕРВИЧНЫЙ КЛАСС--Analya------------------------
    /// <summary>
    /// Модификация от 27 марта 2017 года
    /// Заложен 27 марта 2017 года
    /// </summary>
    public class Analya {
        const int dipazon = 20;
        int z1;
        int zn;
        int nodi;
        int dz1;
        int dzn;
        string basta;
        string rilo;
        int orda;
        int kavo;
        string sovokup;
        
        public Analya() { }

        /// <summary>
        /// Модификация от 27 марта 2017 года
        /// Заложен 27 марта 2017 года
        /// </summary>
        /// <param name="rda"></param>
        /// <param name="pz1old"></param>
        /// <param name="pznold"></param>
        /// <param name="pmvr"></param>
        public Analya(List<Valuing> rda, int pz1old, int pznold, Mova pmvr) {
            if (rda == null) {
                throw new myClasterException("Analya -> null Valuing");
                }
            bool colo = rda[0].SelfVarianto.BegoPo.IsQueryMoveWhite;
            kavo = rda[0].SelfVarianto.BegoPo.AvaQvo;
            basta = rda[0].MovoVan;
            int qz = rda[0].Valo;
            z1 = colo ? qz : -qz;

            }

        /// <summary>
        /// Модификация от 30 марта 2017 года
        /// Заложен 30 марта 2017 года
        /// </summary>
        /// <param name="xel"></param>
        public Analya(XElement xel)
        {
        }

        /// <summary>
        /// Модификация от 30 марта 2017 года
        /// Заложен 30 марта 2017 года
        /// </summary>
        /// <returns></returns>
        public XElement XMLOut()
        {
            XElement reto = new XElement("Analya");
            return reto;
        }

#region--------------------------Свойства объекта-----------------------------------------
        public int ZFirsto { get { return z1; } }
        public int ZCommo { get { return zn; } }
        public string Besto { get { return basta; } }
        public string Realo { get { return rilo; } }
        public int Ordo { get { return orda; } }
        public int Qavo { get { return kavo; } }
        public string Descripto { get { return sovokup; } }
#endregion-----------------------Свойства объекта-----------------------------------------

        }
#endregion-----------------------------ПЕРВИЧНЫЙ КЛАСС--Analya------------------------


#region--------------------------------ДРУГОЙ КЛАСС--Gana-----------------------------
    /// <summary>
    /// Модификация от 30 марта 2017 года
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
        /// Модификация от 31 марта 2017 года
        /// Заложен 31 марта 2017 года
        /// </summary>
        /// <param name="tamo"></param>
        /// <param name="outposodir">Каталог для складывания проанализированных позиций. Уже существует. Не проверяется на житие.</param>
        /// <returns></returns>
        public bool Worko(TimeSpan tamo, string outposodir)
        {
            bool reto = false;
            subjo.EtaloCreate();
            return reto;
        }

        /// <summary>
        /// Модификация от 1 апреля 2017 года
        /// Заложен 31 марта 2017 года
        /// </summary>
        /// <param name="outposodir">Каталог для складывания проанализированных позиций. Уже существует. Не проверяется на житие.</param>
        /// <returns></returns>
        private bool OdinRaz(string outposodir) {
            bool reto = false;
            Mova cumv = subjo.Gamma.ListoMovo[mvc-1];
            vPoza wrk = new vPoza(subjo.Gamma.ListoPozo[mvc], subjo.BigNamo);
            wrk.Descripto = string.Format("Создана {0} при автоанализе {1} для партии {2}", DateTime.Now.ToString(), LeoGuid.ToString(), subjo.LeoGuid.ToString());
            if( dstaro == null ) { dstaro = DateTime.Now; }
            wrk.rasschet(wen, timo, qvaro);
            int oldpz1, oldpzn;
            if (lenya == null || lenya.Count == 0)  { 
                oldpz1 = oldpzn = 0;
                lenya = new List<Analya>();
            } else {
                oldpz1 = lenya[lenya.Count-1].ZFirsto;
                oldpzn = lenya[lenya.Count-1].ZCommo;
                }
            Analya aly = new Analya(wrk.SetoAnalo, oldpz1, oldpzn, cumv);
            lenya.Add(aly);
            if (mvc == mvf)
            {
                ready = true;
                dready = DateTime.Now;
            }
            else
            {
                mvc++; 

            }
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
        /// Модификация от 30 марта 2017 года
        /// Заложен 30 марта 2017 года
        /// </summary>
        /// <param name="xel"></param>
        public override void Dopico(XElement xel)
        {
            foreach (XElement aa in xel.Elements("Analya"))
            {
                if (null == lenya) { lenya = new List<Analya>(); }
                lenya.Add(new Analya(aa));
            }
            subjo = new vGamo(xel.Element("Leo"));
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
        /// Модификация от 30 марта 2017 года
        /// Заложен 30 марта 2017 года
        /// </summary>
        private void PodyemDatov() {
            if(ltexto[2].Length > 0) { dcreato = DateTime.Parse(ltexto[2]); }
            if(ltexto[3].Length > 0) { dstaro = DateTime.Parse(ltexto[3]); }
            if(ltexto[4].Length > 0) { dready = DateTime.Parse(ltexto[4]); }
            }

#region--------------------------Свойства объекта-----------------------------------------
        public string Namo { get { return namo; } set { namo = value; } }
        public string BigNamo { get { return bignamo; } set { bignamo = value; } }
        public string Descripto { get { return ltexto[0]; } set { ltexto[0] = value; } }
        public string Fitchy { get { return ltexto[1]; } set { ltexto[1] = value; } }
        public string FullNamoFile { get { return fulofilename; } set { fulofilename = value; } }
        public vGamo Grusha { get { return subjo; } }
#endregion-----------------------Свойства объекта-----------------------------------------

    }
#endregion--------------------------------ДРУГОЙ КЛАСС--Gana--------------------------

}
