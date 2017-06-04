using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading;
using System.Diagnostics;


namespace OnlyWorko {
#region--------------------------------ПЕРВИЧНЫЙ КЛАСС--EngiPro------------------------
    /// <summary>
    /// Модификация от 8 февраля 2016 года
    /// Заложен 8 февраля 2016 года
    /// </summary>
    public class EngiPro {
        private vPoza wrka;
        vlEngino dvig;
        int minqvo;
        int varqvo;
        Processo ppm;

        /// <summary>
        /// Модификация от 8 февраля 2016 года
        /// Заложен 8 февраля 2016 года
        /// </summary>
        /// <param name="vpp"></param>
        /// <param name="ptypo"></param>
        /// <param name="minutka"></param>
        /// <param name="pvarqvo"></param>
        public EngiPro( vPoza vpp, vlEngino ptypo, int minutka, int pvarqvo ) {
            wrka = vpp;
            dvig = ptypo;
            minqvo = minutka;
            varqvo = pvarqvo;
            ppm = new Processo(KonkretizationModulo(dvig), NaborCommandov(varqvo, wrka.Fena), minqvo);
            }

        /// <summary>
        /// Модификация от 24 мая 2017 года
        /// Заложен 9 февраля 2016 года
        /// </summary>
        public void Analase() {
            int antycykl = 10;
            ValuWorka target = new ValuWorka(wrka.Selfa);
            for (; antycykl > 0; antycykl--) {
                ppm.PrAsy();
                if (target.AddValuSet(ppm.NaborStrok, dvig, minqvo, varqvo)) { break; }
                }
            if (antycykl > 0) {
                if (null == wrka.SetoAnalo) {
                    wrka.SetoAnalo = target.LiValus;
                } else {
                    wrka.SetoAnalo.AddRange(target.LiValus);
                    }
            } else {
                throw new myClasterException(string.Format("EngiPro->Analase, antycykl --{0}--", antycykl));
                }
            }

        /// <summary>
        /// Модификация от 8 февраля 2016 года
        /// Заложен 8 февраля 2016 года
        /// </summary>
        /// <param name="aa"></param>
        /// <returns></returns>
        private static string KonkretizationModulo( vlEngino aa ) {
            string reto = string.Empty;
            string swita = Properties.Settings.Default.Zapuskatel;
            switch( aa ) {
                case vlEngino.Houdini_3a_Pro_w32:
                    reto = (swita == "Homa") ? Properties.Settings.Default.Houdini : @"d:\Worka\Chaso\engino\Houdini3Prow32.exe";
                    break;
                case vlEngino.Komodo_TCECr_64_bit:
                    reto = (swita == "Homa") ? Properties.Settings.Default.Komodo : @"d:\Worka\Chaso\engino\komodo-tcecr-64bit.exe";
                    break;
                case vlEngino.Stockfish_2_3_1_JA_64bit:
                    reto = (swita == "Homa") ? Properties.Settings.Default.Houdini : @"d:\Worka\Chaso\engino\stockfish-231-64-ja.exe";
                    break;
                }
            return reto;
            }

        /// <summary>
        /// Модификация от 8 февраля 2016 года
        /// Заложен 8 февраля 2016 года
        /// </summary>
        /// <param name="ppvar"></param>
        /// <param name="feno"></param>
        /// <returns></returns>
        private static string[] NaborCommandov( int ppvar, string feno ) {
            List<string> rret = new List<string>();
            rret.Add("uci");
            rret.Add("isready");
            rret.Add("setoption name Hash value 512");
            rret.Add("setoption name Threads value 3");
            rret.Add(string.Format(@"setoption name MultiPV value {0}", ppvar));
            rret.Add("ucinewgame");
            rret.Add(string.Format(@"position fen {0}", feno));
            rret.Add("go infinite");
            rret.Add("stop");
            rret.Add("quit");
            return rret.ToArray();
            }
        }
#endregion-----------------------------ПЕРВИЧНЫЙ КЛАСС--EngiPro------------------------

#region--------------------------------ДРУГОЙ КЛАСС--Processo--------------------------
    public class Processo {
        private StringBuilder strOutput;
        private TimeSpan interval;
        private string filomodul;
        private string[] sma;
        private string[] vykhod;
        bool nevpervoy;

        /// <summary>
        /// vjlj
        /// </summary>
        /// <param name="pmod"></param>
        /// <param name="pcmd"></param>
        /// <param name="qvomin"></param>
        public Processo(string pmod, string[]pcmd, int qvomin) {
            filomodul = pmod;
            interval = new TimeSpan(0, qvomin, 0);
            sma = pcmd;
            strOutput = new StringBuilder();
            nevpervoy = false;
            }

        public void PrSyn() {

            Process Pro;
            Pro = new Process();
            Pro.StartInfo.FileName = filomodul;

            // Set UseShellExecute to false for redirection.
            Pro.StartInfo.UseShellExecute = false;
            Pro.StartInfo.RedirectStandardInput = true;
            Pro.StartInfo.RedirectStandardOutput = true;

            Pro.Start();

            // Use a stream writer to synchronously write the sort input.
            StreamWriter sInpo = Pro.StandardInput;
            StreamReader sOut = Pro.StandardOutput;

            foreach (string aa in sma) {
                sInpo.WriteLine(aa);
                if (aa.StartsWith("go")) {
                    Thread.Sleep(interval);
                    }
                }
            sInpo.Close();
            strOutput.Append(sOut.ReadToEnd());
            Pro.WaitForExit();
            Pro.Close();
            }

        /// <summary>
        /// Модификация от 11 мая 2017 года
        /// Заложен в феврале 2016 года
        /// </summary>
        public void PrAsy() {

            Process Pro;
            Pro = new Process();
            Pro.StartInfo.FileName = filomodul;

            // Set UseShellExecute to false for redirection.
            Pro.StartInfo.UseShellExecute = false;
            Pro.StartInfo.CreateNoWindow = true;
            Pro.StartInfo.RedirectStandardInput = true;
            Pro.StartInfo.RedirectStandardOutput = true;
            Pro.OutputDataReceived += new DataReceivedEventHandler(StrOutputHandler);
            if (nevpervoy) {
                LogoCM.OutString(string.Format("EngiPro->PrAsy:Невпервой Pro.Id:{0} Pro.Handle:{1} HandleCount{2}", Pro.Id, Pro.Handle, Pro.HandleCount));
                }
            try {
                Pro.Start();
                Pro.BeginOutputReadLine();

                // Use a stream writer to synchronously write the sort input.
                StreamWriter sInpo = Pro.StandardInput;


                foreach (string aa in sma) {
                    sInpo.WriteLine(aa);
                    if (aa.StartsWith("go")) {
                        LogoCM.OutString(string.Format("EngiPro->PrAsy:После go Pro.Id:{0} Pro.Handle:{1}", Pro.Id, Pro.Handle));
                        Thread.Sleep(interval);
                        }
                    }
                sInpo.Close();
                int itimo = 3000;
                for (int i = 10; i > 0; i--) {
                    bool zeta = Pro.WaitForExit(itimo);
                    if (!zeta) {
                        LogoCM.OutString(string.Format("EngiPro->PrAsy->Не кончили сходу i = {0} Pro.Id:{1} Pro.Handle:{2}", i, Pro.Id, Pro.Handle));
                    } else {
                        break;
                        }
                    }
                Pro.Close();
                NaboroStroke();
            } catch (Exception ex) {
                LogoCM.OutString(string.Format(@"EngiPro->Processo->PrAsy :: модуль {0} -- проблема {1}", filomodul, ex.Message));
                throw new myClasterException(@"EngiPro->Processo->PrAsy :: модуль " + filomodul, ex);
                }
            nevpervoy = true;
            }

        public void ToTextFile( string fname ) {
            StreamWriter wfilo = new StreamWriter(fname);
            wfilo.WriteLine(" Resulto");
            wfilo.WriteLine("----------");
            wfilo.WriteLine(strOutput);
            wfilo.Close();            
            }

        /// <summary>
        /// Модификация от 24 мая 2017 года
        /// Заложен 8 февраля 2016 года
        /// </summary>
        private void NaboroStroke() {
            string [] bb = { Environment.NewLine, };
            vykhod = this.strOutput.ToString().Split(bb, StringSplitOptions.RemoveEmptyEntries);
            }

        private void StrOutputHandler( object sendingProcess, DataReceivedEventArgs outLine ) {
            if ( !String.IsNullOrEmpty( outLine.Data ) ) {
                strOutput.Append(Environment.NewLine + outLine.Data);
                }
            }

        public List<string> NaborStrok { get { return vykhod.ToList(); } }
        }
#endregion-----------------------------ДРУГОЙ КЛАСС--Processo--------------------------
}

