using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Text;
using System.IO;
using OnlyWorko;
using System.Text.RegularExpressions;

namespace Dzobo
{
    public class JoWorker
    {
        string sesso;
        int hur;
        int mina;
        string workodir;
        string posodir;

        public JoWorker() {
            hur = Properties.Settings.Default.huro;
            mina = Properties.Settings.Default.mina;
            workodir = Properties.Settings.Default.diga;
            posodir = Properties.Settings.Default.dipo;
            CreateSession();
            }

        public JoWorker(int ph, int pm, string wd) {
            CreateSession();
            hur = ph;
            mina = pm;
            workodir = wd;
            }

        /// <summary>
        /// Модификация от 6 апреля 2017 года
        /// Заложен 27 марта 2017 года
        /// </summary>
        public void runo() {
            LogoCM.initologo("lDzobo.log");
            LogoCM.OutString(string.Format("{0} Старт Дюрация {1}:{2}", sesso, hur, mina));
            try {
                PoiskTaskov();
            } catch( Exception ex) {
                LogoCM.OutString("ERROR->" + ex.Message + Environment.NewLine + ex.StackTrace); 
                }
            LogoCM.OutString(sesso + " Финиш");
            LogoCM.finitalogo();
            }

        /// <summary>
        /// Модификация от 6 апреля 2017 года
        /// Заложен 6 апреля 2017 года
        /// </summary>
        private void PoiskTaskov() {
            TimeSpan bz = new TimeSpan(hur, mina, 0);
            DateTime finalo = DateTime.Now + bz;
            DirectoryInfo dd = new DirectoryInfo(workodir);
            if( dd.Exists ) {
                foreach (FileInfo aa in dd.EnumerateFiles(@"*.xml")) {
                    string shortname = aa.Name;
                    if ( Regex.IsMatch(shortname, @"[0-9a-f]{32}") ) {
                        XDocument doco = XDocument.Load(aa.FullName);
                        XElement xel = doco.Element("Gana");
                        if (xel != null) {
                            Gana wrk = new Gana(xel.Element("Leo"));
                            wrk.FullNamoFile = aa.FullName;
                            if (!wrk.Ready) {
                                bool rst = wrk.Worko(bz, posodir);
                                if (rst && finalo > DateTime.Now) {
                                    bz = finalo - DateTime.Now;
                                    continue;
                                } else {
                                    break;
                                    }
                                }
                            }
                        }
                    }
                }
            }

        /// <summary>
        /// Модификация от 27 марта 2017 года
        /// Заложен 27 марта 2017 года
        /// </summary>
        private void CreateSession() {
            DateTime dd = DateTime.Now;
            sesso = string.Format("{0:000}{1}{2:00}{3:00}",dd.DayOfYear,dd.DayOfWeek,dd.Hour,dd.Minute);
            }   
    }
}
