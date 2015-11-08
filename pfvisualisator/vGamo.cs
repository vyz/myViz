using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Xml.Linq;

namespace pfVisualisator
{
    /// <summary>
    /// Модификация от 21 октября 2015 года
    /// Заложен 4 октября 2015 года
    /// </summary>
    public class vGamo : myleo, INotifyPropertyChanged {
        public event PropertyChangedEventHandler PropertyChanged;

        private int movacount;

        /// <summary>
        /// Модификация от 4 октября 2015 года
        /// Заложен 4 октября 2015 года
        /// </summary>
        public vGamo()
            : base(leoType.Gamo) {

            string[] akva = new string[15];
            ltexto = akva.ToList();
            }

        /// <summary>
        /// Модификация от 6 октября 2015 года
        /// Заложен 4 октября 2015 года
        /// </summary>
        /// <param name="gm"></param>
        public vGamo(Gamo gm)
            : this() {
                namo = gm.GamerWhite + " - " + gm.GamerBlack;
                bignamo = namo + " " + gm.EventoDate + " " + gm.Resulto + " " + gm.ECO;
                White = gm.GamerWhite;
                Black = gm.GamerBlack;
                Result = gm.Resulto;
                Date = gm.Date;
                Event = gm.Evento;
                ECO = gm.ECO;
                PlyCount = gm.Qavo;
                WElo = gm.EloWhite;
                BElo = gm.EloBlack;
                Site = gm.Sito;
                Round = gm.Roundo;
                Fen = gm.Fen;
                if (gm.Qavo.Length > 0) {
                    movacount = int.Parse(gm.Qavo) / 2;
                } else { 
                    movacount = 0; 
                    }
                OnlyMova = gm.VanStrokeMovaRegion();
                AddAtr = gm.VanStrokeRestAttributes();
                Descripto = string.Empty;
                }

        /// <summary>
        /// Модификация от 22 октября 2015 года
        /// Заложен 11 октября 2015 года
        /// </summary>
        /// <param name="Elemo"></param>
        public vGamo(XElement Elemo)
            : base(Elemo) {
                if (PlyCount.Length > 0) {
                    movacount = int.Parse(PlyCount) / 2;
                } else {
                    movacount = 0;
                    }
            }

        /// <summary>
        /// Модификация от 8 ноября 2015 года
        /// Заложен 8 ноября 2015 года
        /// </summary>
        /// <param name="offseto"></param>
        /// <returns></returns>
        public pozo GetPozoOnOffset(int offseto) {
            pozo reto = null;
            return reto;
            }

#region--------------------------Свойства объекта-----------------------------------------
        public string Namo { get { return namo; } set { namo = value; } }
        public string BigNamo { get { return bignamo; } set { bignamo = value; } }
        public string White { get { return ltexto[0]; } set { ltexto[0] = value; } }
        public string Black { get { return ltexto[1]; } set { ltexto[1] = value; } }
        public string Result { get { return ltexto[2]; } set { ltexto[2] = value; } }
        public string Date { get { return ltexto[3]; } set { ltexto[3] = value; } }
        public string Event { get { return ltexto[4]; } set { ltexto[4] = value; } }
        public string ECO { get { return ltexto[5]; } set { ltexto[5] = value; } }
        public string PlyCount { get { return ltexto[6]; } set { ltexto[6] = value; } }
        public string WElo { get { return ltexto[7]; } set { ltexto[7] = value; } }
        public string BElo { get { return ltexto[8]; } set { ltexto[8] = value; } }
        public string Site { get { return ltexto[9]; } set { ltexto[9] = value; } }
        public string Round { get { return ltexto[10]; } set { ltexto[10] = value; } }
        public string Fen { get { return ltexto[11]; } set { ltexto[11] = value; } }
        public string OnlyMova { get { return ltexto[12]; } set { ltexto[12] = value; } }
        public string AddAtr { get { return ltexto[13]; } set { ltexto[13] = value; } }
        public string Descripto { get { return ltexto[14]; } set { ltexto[14] = value; } }
        public int movoQvo { get { return movacount; } }
        public myTago Tago { get { return tago; } }
        public string TagoTextStroke { get { return tago.TStroke; } }
#endregion-----------------------Свойства объекта-----------------------------------------

        private void OnPropertyChanged(string name) {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) {
                handler(this, new PropertyChangedEventArgs(name));
                }
            }
        }

    /// <summary>
    /// Модификация от 6 октября 2015 года
    /// Заложен 6 октября 2015 года
    /// </summary>
    public class vLGamo {
        private List<vGamo> manolist = vGamoList.Listo;
        private bool edina = vGamoList.Edina;

        public vLGamo() { }

        /// <summary>
        /// Модификация от 6 октября 2015 года
        /// Заложен 6 октября 2015 года
        /// </summary>
        /// <param name="lga"></param>
        public vLGamo(List<Gamo> lga) {
            vGamoList.FillListFromListGamo(lga);
            }

        /// <summary>
        /// Модификация от 11 октября 2015 года
        /// Заложен 11 октября 2015 года
        /// </summary>
        /// <param name="XMLFiloName"></param>
        public vLGamo(string XMLFiloName) {
            XDocument doca = XDocument.Load(XMLFiloName);
            vGamoList.FillListFromXML(doca.Element("Gamosyky").Elements("Leo").ToList());
            }

        /// <summary>
        /// Модификация от 10 октября 2015 года
        /// Заложен 10 октября 2015 года
        /// </summary>
        /// <param name="filo">Имя файла для сохранения XML-содержимого</param>
        public void SavoInXmlFilo(string filo) {
            XComment xcom = new XComment(string.Format("Набор партеечек от {0} ", DateTime.Now.ToLongDateString()));
            XDocument doca = new XDocument(new XDeclaration("1.0", "utf-8", "yes"), xcom);
            XElement ruta = new XElement("Gamosyky");
            foreach (vGamo aa in manolist) {
                ruta.Add(aa.LeoToXML());
                }
            doca.Add(ruta);
            doca.Save(filo);
            }

        public bool Edina { set { edina = value; } get { return edina; } }
        public List<vGamo> Listo { get { return manolist; } }
        }

    /// <summary>
    /// Модификация от 6 октября 2015 года
    /// Заложен 6 октября 2015 года
    /// </summary>
    public class vGamoList : Singleton<vGamoList> {

        private vGamoList() { }

        private static List<vGamo> manolist = new List<vGamo>();
        private static bool edina = false;
        private static vGamo curgamo = null;

        /// <summary>
        /// Модификация от 6 октября 2015 года
        /// Заложен 6 октября 2015 года
        /// </summary>
        /// <param name="lga"></param>
        public static void FillListFromListGamo(List<Gamo> lga) {
            vGamo tema;
            foreach (Gamo aa in lga) {
                tema = new vGamo(aa);
                manolist.Add(tema);
                }
            }

        /// <summary>
        /// Модификация от 11 октября 2015 года
        /// Заложен 11 октября 2015 года
        /// </summary>
        /// <param name="lxe"></param>
        public static void FillListFromXML(List<XElement> lxe) {
            vGamo tema;
            foreach (XElement aa in lxe) {
                tema = new vGamo(aa);
                manolist.Add(tema);
                }
            }

        public static bool Edina { set { edina = value; } get { return edina; } }
        public static List<vGamo> Listo { get { return manolist; } }

        }
}
