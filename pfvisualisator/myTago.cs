using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace pfVisualisator {

    public class myTago : INotifyPropertyChanged {

        public event PropertyChangedEventHandler PropertyChanged;

        private string tagstroke;
        private List<string> tagset;
        private int tagcount;

        public myTago() {
            tagstroke = string.Empty;
            tagset = new List<string>();
            tagcount = 0;
            }

        /// <summary>
        /// Модификация от 2 февраля 2015 года
        /// Заложен от 2 февраля 2015 года
        /// </summary>
        /// <param name="pstrtag">Инициализирующая строка с пробелом в качестве разделителя</param>
        public myTago(string pstrtag) {
            tagstroke = pstrtag.Trim();
            string[] razd = {" "};
            tagset = tagstroke.Split(razd, StringSplitOptions.RemoveEmptyEntries).ToList();
            tagcount = tagset.Count;
            }

        /// <summary>
        /// Модификация от 6 марта 2015 года
        /// Заложен от 2 февраля 2015 года
        /// </summary>
        /// <param name="dobavka">Добавляемый тег</param>
        public void AddTag(string dobavka) {
            if (tagset.Contains(dobavka)) { return; }
            tagset.Add(dobavka);
            tagstroke += ((tagcount > 0) ? " " : "") + dobavka;
            tagcount++;
            }

        /// <summary>
        /// Модификация от 3 февраля 2015 года
        /// Заложен от 3 февраля 2015 года
        /// </summary>
        /// <param name="nabor">Измененный набор, полностью переписывает имевшийся до этого</param>
        public void SaveTago(List<string> nabor) {
            tagstroke = string.Empty;
            tagset.Clear();
            tagcount = 0;
            this.AddTagSet(nabor);
            OnPropertyChanged("TStroke");
            }

        private void OnPropertyChanged(string name) {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) {
                handler(this, new PropertyChangedEventArgs(name));
                }
            }

        /// <summary>
        /// Модификация от 3 февраля 2015 года
        /// Заложен от 2 февраля 2015 года
        /// </summary>
        /// <param name="nabor">Добавляемое множество тегов</param>
        private void AddTagSet(List<string> nabor) {
            foreach (string aa in nabor) {
                this.AddTag(aa);
                }
            }

        public string TStroke { get { return tagcount > 0 ? tagstroke : "          "; } }
        public List<string> TSet { get { return tagset; } }
        public int TCount { get { return tagcount; } }
    }

    public class myLTago {
        private List<string> manolist = myTagoList.Listo;

        public myLTago() { }

        /// <summary>
        /// Модификация от 2 февраля 2015 года
        /// Заложен от 2 февраля 2015 года
        /// </summary>
        /// <param name="i">Для заполнения списка должна быть единичка</param>
        public myLTago(int i) {
            if (manolist.Count == 0 && i == 1) {
                foreach (vOKPReport aa in vRepoList.Listo) {
                    myTagoList.SetoAddo(aa.Tago.TSet);
                    }
                }
            }

        public List<string> Listo { get { return manolist; } }

    }
    
    public class myTagoList : Singleton<myTagoList> {
        private static List<string> manolist = new List<string>();

        private myTagoList() { }

        /// <summary>
        /// Модификация от 2 февраля 2015 года
        /// Заложен от 2 февраля 2015 года
        /// </summary>
        /// <param name="plist"></param>
        public static void SetoAddo(List<string> plist) {
            foreach (string aa in plist) {
                if (manolist.Contains(aa)) { continue; }
                manolist.Add(aa);
                }
            }

        /// <summary>
        /// Модификация от 18 февраля 2015 года
        /// Заложен от 18 февраля 2015 года
        /// </summary>
        /// <param name="ptago"></param>
        public static void StrokeAddo(string ptago) {
            if (manolist.Contains(ptago)) { return; }
            manolist.Add(ptago);
            }

        public static List<string> Listo { get { return manolist; } }
        }
    }
