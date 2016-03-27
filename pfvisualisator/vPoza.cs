using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.ComponentModel;
using System.Xml.Linq;

namespace pfVisualisator {

    /// <summary>
    /// Модификация от 24 декабря 2015 года
    /// Заложен 24 декабря 2015 года
    /// </summary>
    public class vPoza : myleo, INotifyPropertyChanged {
        public event PropertyChangedEventHandler PropertyChanged;

        private pozo poza;
        private List<Valuing> setoanalizo;

        /// <summary>
        /// Модификация от 24 декабря 2015 года
        /// Заложен 24 декабря 2015 года
        /// </summary>
        public vPoza()
            : base(leoType.Pozo) {

            string[] akva = new string[3];
            ltexto = akva.ToList();
            setoanalizo = null;
            }

        /// <summary>
        /// Модификация от 24 декабря 2015 года
        /// Заложен 24 декабря 2015 года
        /// </summary>
        /// <param name="pz"></param>
        public vPoza(pozo pz)
            : this() {
            pz.AvailableFill();
            namo = pz.NumberMove.ToString() + " ход " + (pz.IsQueryMoveWhite ? "белых" : "чёрных");
            bignamo = namo + ". Доступно " + pz.AvaQvo.ToString();
            Fena = pz.fenout();
            poza = pz;
            }

        /// <summary>
        /// Модификация от 27 марта 2016 года
        /// Заложен 27 марта 2016 года
        /// </summary>
        /// <param name="pz"></param>
        /// <param name="bigonamo"></param>
        public vPoza(pozo pz, string bigonamo)
            : this(pz) {
            bignamo = namo + " " + bigonamo;
            }


        /// <summary>
        /// Модификация от 5 февраля 2016 года
        /// Заложен 5 февраля 2016 года
        /// </summary>
        /// <param name="Elemo"></param>
        public vPoza(XElement Elemo)
            : base(Elemo) {
            poza = new pozo(Fena);
            poza.AvailableFill();
            }

        /// <summary>
        /// Модификация от 3 февраля 2016 года
        /// Заложен 3 февраля 2016 года
        /// </summary>
        /// <returns></returns>
        public override XElement Dopico() {
            XElement reto = new XElement("Dopico");
            if (null != setoanalizo) {
                foreach (Valuing aa in setoanalizo.OrderBy(F => F.Rango)) {
                    reto.Add(aa.XMLOut());
                    }
                }
            return reto;
            }

        /// <summary>
        /// Модификация от 5 февраля 2016 года
        /// Заложен 5 февраля 2016 года
        /// </summary>
        /// <param name="xel"></param>
        public override void Dopico(XElement xel) {
            foreach (XElement aa in xel.Elements("ValuPrice")) {
                if (null == setoanalizo) { setoanalizo = new List<Valuing>(); }
                setoanalizo.Add(new Valuing(aa));
                }
            }

        /// <summary>
        /// Модификация от 22 марта 2016 года
        /// Заложен 9 февраля 2016 года
        /// </summary>
        /// <param name="ptypo"></param>
        /// <param name="minutka"></param>
        /// <param name="pvarqvo"></param>
        public void rasschet(vlEngino ptypo, int minutka, int pvarqvo) {
            EngiPro aa = new EngiPro(this, ptypo, minutka, pvarqvo);
            aa.Analase();
            if (setoanalizo.Count > 0) {
                AnaRes = setoanalizo.OrderBy(F => F.Rango).ToArray()[0].Texa;
                OnPropertyChanged("Anares");
                OnPropertyChanged("SetoAnalo");
                }
            }

        /// <summary>
        /// Модификация от 3 февраля 2016 года
        /// Заложен 3 февраля 2016 года
        /// </summary>
        public void SavoInViborXmlFilo() {
            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
            // Set filter for file extension and default file extension
            dlg.InitialDirectory = Properties.Settings.Default.GamoPGNStartDir;
            dlg.Filter = "Стандартный (*.xml)|*.xml|All files (*.*)|*.*";
            dlg.FilterIndex = 1;
            dlg.RestoreDirectory = true;
            try {
                // Display OpenFileDialog by calling ShowDialog method
                Nullable<bool> result = dlg.ShowDialog();
                if (result == true) {
                    string filename = dlg.FileName;
                    SavoInXmlFilo(filename);
                    }
            } catch (Exception ex) {
                System.Windows.MessageBox.Show("Error: Could not wtite file on the disk. Original error: " + ex.Message);
                }
            }

        /// <summary>
        /// Модификация от 3 февраля 2016 года
        /// Заложен 3 февраля 2016 года
        /// </summary>
        /// <param name="filo"></param>
        private void SavoInXmlFilo(string filo) {
            XComment xcom = new XComment(string.Format("Одинокая поза от {0} ", DateTime.Now.ToLongDateString()));
            XDocument doca = new XDocument(new XDeclaration("1.0", "utf-8", "yes"), xcom);
            XElement ruta = new XElement("vPoza");
            ruta.Add(LeoToXML());
            doca.Add(ruta);
            doca.Save(filo);
            }

        #region--------------------------Свойства объекта-----------------------------------------
        public string Namo { get { return namo; } set { namo = value; } }
        public string BigNamo { get { return bignamo; } set { bignamo = value; } }
        public string Fena { get { return ltexto[0]; } set { ltexto[0] = value; } }
        public string AnaRes { get { return ltexto[1]; } set { ltexto[1] = value; } }
        public string Descripto { get { return ltexto[2]; } set { ltexto[2] = value; } }
        public myTago Tago { get { return tago; } }
        public string TagoTextStroke { get { return tago.TStroke; } }
        public pozo Selfa { get { return poza; } }
        public List<Valuing> SetoAnalo { get { return setoanalizo; } set { setoanalizo = value; } }
        public string Material { get { return (poza == null) ? "Поза не зафиксена" : (poza.SetaWhite == poza.SetaBlack) ? "Материальное равенство" : string.Format("{0} : {1}", poza.SetaWhite, poza.SetaBlack); } }
        #endregion-----------------------Свойства объекта-----------------------------------------

        private void OnPropertyChanged(string name) {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) {
                handler(this, new PropertyChangedEventArgs(name));
                }
            }

        /// <summary>
        /// Модификация от 1 февраля 2016 года
        /// Заложен 24 декабря 2015 года
        /// </summary>
        /// <returns></returns>
        public static vPoza CreateExemplarusForLife() {
            pozo zg = new pozo("3rk2r/2q1bpp1/p2p1n2/2pP1n1p/PpN2B2/3P2P1/1PP1Q1BP/4RRK1 b k - 0 22");
            vPoza reto = new vPoza(zg);
            string filfi = @"..\..\..\reso\a01_90min.txt";
            TextReader fafo = new StreamReader(File.Open(filfi, FileMode.Open));
            string aa = fafo.ReadLine();
            List<string> naboro = new List<string>();
            while (aa != null) {
                naboro.Add(aa);
                aa = fafo.ReadLine();
                }
            fafo.Close();
            ValuWorka target = new ValuWorka(zg);
            target.AddValuSet(naboro, vlEngino.Houdini_3a_Pro_w32, 90, 5);
            reto.SetoAnalo = target.LiValus;
            reto.AnaRes = reto.SetoAnalo[0].Texa;
            reto.Descripto = "Искусственный член";
            return reto;
            }

        /// <summary>
        /// Модификация от 5 февраля 2016 года
        /// Заложен 5 февраля 2016 года
        /// </summary>
        /// <param name="filo"></param>
        /// <returns></returns>
        public static vPoza CreateExemparFromXmlFile(string filo) {
            XDocument doca = XDocument.Load(filo);
            return new vPoza(doca.Element("vPoza").Element("Leo"));
            }
        }
    }
