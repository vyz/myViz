using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading;
using System.Diagnostics;


namespace OnlyWorko
{
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
        /// Модификация от 9 февраля 2016 года
        /// Заложен 9 февраля 2016 года
        /// </summary>
        public void Analase() {
            ppm.PrAsy();
            ValuWorka target = new ValuWorka(wrka.Selfa);
            target.AddValuSet(ppm.NaboroStroke(), dvig, minqvo, varqvo);
            if (null == wrka.SetoAnalo) {
                wrka.SetoAnalo = target.LiValus;
            } else { wrka.SetoAnalo.AddRange(target.LiValus); 
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


    public class Processo {
        private StringBuilder strOutput;
        private TimeSpan interval;
        private string filomodul;
        private string[] sma;

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

        public void PrAsy()
        {

            Process Pro;
            Pro = new Process();
            Pro.StartInfo.FileName = filomodul;

            // Set UseShellExecute to false for redirection.
            Pro.StartInfo.UseShellExecute = false;
            Pro.StartInfo.RedirectStandardInput = true;
            Pro.StartInfo.RedirectStandardOutput = true;
            Pro.OutputDataReceived += new DataReceivedEventHandler(StrOutputHandler);

            Pro.Start();
            Pro.BeginOutputReadLine();

            // Use a stream writer to synchronously write the sort input.
            StreamWriter sInpo = Pro.StandardInput;
            

            foreach (string aa in sma)
            {
                sInpo.WriteLine(aa);
                if (aa.StartsWith("go"))
                {
                    Thread.Sleep(interval);
                }
            }
            sInpo.Close();
            Pro.WaitForExit();
            Pro.Close();
        }

        public void ToTextFile( string fname ) {
            StreamWriter wfilo = new StreamWriter(fname);
            wfilo.WriteLine(" Resulto");
            wfilo.WriteLine("----------");
            wfilo.WriteLine(strOutput);
            wfilo.Close();            
            }

        /// <summary>
        /// Модификация от 8 февраля 2016 года
        /// Заложен 8 февраля 2016 года
        /// </summary>
        /// <returns></returns>
        public List<string> NaboroStroke() {
            string [] bb = { Environment.NewLine, };
            string[] aa = this.strOutput.ToString().Split(bb, StringSplitOptions.RemoveEmptyEntries);
            return aa.ToList();
            }

        private void StrOutputHandler( object sendingProcess, DataReceivedEventArgs outLine ) {
            if ( !String.IsNullOrEmpty( outLine.Data ) ) {
                strOutput.Append(Environment.NewLine + outLine.Data);
                }
            }

        }
    }

