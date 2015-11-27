using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Xml.Linq;
using System.Text.RegularExpressions;

namespace pfVisualisator
{
    /// <summary>
    /// Модификация от 21 октября 2015 года
    /// Заложен 4 октября 2015 года
    /// </summary>
    public class vGamo : myleo, INotifyPropertyChanged {
        public event PropertyChangedEventHandler PropertyChanged;

        private int movacount;
        private Gamo etalo;
        private pozo currento;

        /// <summary>
        /// Модификация от 4 октября 2015 года
        /// Заложен 4 октября 2015 года
        /// </summary>
        public vGamo()
            : base(leoType.Gamo) {

            string[] akva = new string[16];
            ltexto = akva.ToList();
            }

        /// <summary>
        /// Модификация от 19 ноября 2015 года
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
                Timingo = gm.VanStrokeTimingo();
                }

        /// <summary>
        /// Модификация от 19 ноября 2015 года
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
                if (ltexto.Count < 16)
                {
                    ltexto.Add(string.Empty);
                }
            }

        /// <summary>
        /// Модификация от 16 ноября 2015 года
        /// Заложен 8 ноября 2015 года
        /// </summary>
        /// <param name="offseto"></param>
        /// <returns></returns>
        public pozo GetPozoOnOffset(int offseto) {
            pozo reto = null;
            if (etalo == null) {
                etalo = new Gamo(this);
                }
            if( offseto <= 0 ) { //Это стартовая позиция - самое начало. Необязательно нулевая позиция
                reto = etalo.GetFirstPozo();                
            } else {
                string issledo = this.OnlyMova.Substring(0, offseto);
                if (issledo.Length == 0) {
                    throw new VisualisatorException("vGamo-GetPozoOnOffset !!! Не существует набора ходов");
                } else {
                    string patnumbermove = @"\d+\.";
                    string patmovesymbol = @"[KQRBNa-hO][a-h1-8\-x=\+#QRBN]+";
                    Match aa = Regex.Match(issledo, patnumbermove, RegexOptions.RightToLeft);
                    if (aa.Index > 0) {
                        string zz = aa.Value;
                        string zp = zz.Substring(0, zz.Length - 1);
                        int nmove = int.Parse(zp);
                        zz = issledo.Substring(aa.Index + zz.Length).TrimStart();
                        MatchCollection bbc = Regex.Matches(zz, patmovesymbol);
                        zp = (bbc.Count == 0) ? "w" : "b";
                        reto = etalo.GetPozoAfterMove(nmove, zp);
                    } else {
                        reto = etalo.GetFirstPozo();
                        }
                    }
                }
            if (reto != null) {
                currento = reto;
                }
            return reto;
            }

        /// <summary>
        /// Модификация от 23 ноября 2015 года
        /// Заложен 22 ноября 2015 года
        /// </summary>
        /// <returns></returns>
        public pozo GetNextPozo() {
            pozo reto = null;
            if (etalo == null) {
                etalo = new Gamo(this);
                }
            if (currento == null) {
                currento = etalo.GetFirstPozo();
                }
            reto = etalo.GetNextPozo(currento);
            if (reto != null) {
                currento = reto;
                }
            return reto;
            }

        /// <summary>
        /// Модификация от 23 ноября 2015 года
        /// Заложен 23 ноября 2015 года
        /// </summary>
        /// <returns></returns>
        public pozo GetPrevPozo() {
            pozo reto = null;
            if (etalo == null) {
                etalo = new Gamo(this);
                }
            if (currento == null) {
                currento = etalo.GetFirstPozo();
                reto = currento;
            } else {
                reto = etalo.GetPrevPozo(currento);
                if (reto != null) {
                    currento = reto;
                    }
                }
            return reto;
            }

        /// <summary>
        /// Модификация от 24 ноября 2015 года
        /// Заложен 18 ноября 2015 года
        /// </summary>
        /// <returns></returns>
        public bool SaveInDB() {
            bool reto = false;
            if (etalo == null) {
                etalo = new Gamo(this);
                }
            bido dbisa = new bido(leoType.Gamo);
                //bido dbisa = new bido(); Проверка для заброса Leo-объекта в базу. Прошла на ура.
            dbisa.PutLeoRecord(this);

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
        public string Timingo { get { return ltexto[15]; } set { ltexto[15] = value; } }
        public int movoQvo { get { return movacount; } set { movacount = value; } }
        public myTago Tago { get { return tago; } }
        public string TagoTextStroke { get { return tago.TStroke; } }
        public string TimoView { get { 
            if (etalo == null) {
                etalo = new Gamo(this);
                }
            return etalo.VanStrokeTimoForView(); 
            } }
#endregion-----------------------Свойства объекта-----------------------------------------

        private void OnPropertyChanged(string name) {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) {
                handler(this, new PropertyChangedEventArgs(name));
                }
            }

        /// <summary>
        /// Модификация от 11 ноября 2015 года
        /// Заложен 11 ноября 2015 года
        /// </summary>
        /// <returns></returns>
        public static vGamo CreateExemplarusForLife() {
            vGamo reto = new vGamo();
            reto.Namo = "Nikitenko, Mihail,  - Vorontsov, Pavlo, ";
            reto.BigNamo = "Nikitenko, Mihail,  - Vorontsov, Pavlo,   1-0 C10";
            reto.White = "Nikitenko, Mihail, ";
            reto.Black = "Vorontsov, Pavlo, ";
            reto.Result = "1-0";
            reto.Date = "2014.05.18";
            reto.Event = "Vanya Somov Memorial 2014";
            reto.ECO = "C10";
            reto.PlyCount = "75";
            reto.WElo = "2199";
            reto.BElo = "2413";
            reto.Site = "Kirishi (Russia)";
            reto.Round = "5.5";
            reto.Fen = "";
            reto.movoQvo = int.Parse(reto.PlyCount) / 2;
            reto.OnlyMova = @"1. e4 e6 2. d4 d5 3. Nd2 dxe4 4. Nxe4 Bd7 5. Nf3 Bc6 6. Bd3 Nd7 7. c3 Ngf6 8.
Qc2 Bxe4 9. Bxe4 Nxe4 10. Qxe4 c6 11. Qe2 Nf6 12. O-O Be7 13. c4 O-O 14. b3 Qa5
15. Qe5 Qxe5 16. dxe5 Nd7 17. Rd1 Rfd8 18. Be3 a5 19. Kf1 Kf8 20. Ke2 f6 21.
exf6 gxf6 22. Rd4 e5 23. Rh4 Kg8 24. Rg4+ Kf7 25. Rh4 Nf8 26. Rd1 Rd7 27. g4
Kg8 28. Rh5 Ne6 29. Rg1 e4 30. Nh4 a4 31. b4 Bxb4 32. Rh6 Nd4+ 33. Bxd4 Rxd4
34. g5 fxg5 35. Rxg5+ Kh8 36. Ng6+ Kg7 37. Ne5+ Kf8 38. Rxh7 1-0";
            reto.AddAtr = @"[Board] - [1]
[WhiteTitle] - [GM]
[WhiteCountry] - [BUL]
[WhiteFideId] - [2900084]
[WhiteEloChange] - [1]
[BlackTitle] - [IM]
[BlackCountry] - [NGR]
[BlackFideId] - [8500258]
[BlackEloChange] - [-2]";
            reto.Descripto = "Искусственный член";

            return reto;
            }

        }
#region--------------------------------ДРУГОЙ КЛАСС--vlWGamo------------------------------
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
#endregion-----------------------------ДРУГОЙ КЛАСС--vlWGamo------------------------------
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
