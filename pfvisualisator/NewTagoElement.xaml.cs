using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace pfVisualisator
{
    /// <summary>
    /// Логика взаимодействия для NewTagoElement.xaml
    /// </summary>
    public partial class NewTagoElement : Window
    {

        /// <summary>
        /// Модификация от 17 февраля 2015 года
        /// Заложен в 2015 году
        /// </summary>
        public NewTagoElement() {
            InitializeComponent();
            }

        /// <summary>
        /// Модификация от 17 февраля 2015 года
        /// Заложен 17 февраля 2015 года
        /// </summary>
        /// <param name="tt"></param>
        public void TagoChecko(myTago tt) {
            myLTago tal = new myLTago(1);
            List<string> aal = tal.Listo;
            aal.Sort();
            foreach (string aa in aal) {
                CheckBox bb = new CheckBox();
                bb.Content = aa;
                if (tt.TSet.Contains(aa)) {
                    bb.IsChecked = true;
                    }
                this.LBVan.Items.Add(bb);
                }
            }

        /// <summary>
        /// Модификация от 18 февраля 2015 года
        /// Заложен 18 февраля 2015 года
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddTago_Click(object sender, RoutedEventArgs e) {
            string aa = this.addostrokeTBx.Text;
            if( myTagoList.Listo.Contains( aa ) ) { return; }
            CheckBox bb = new CheckBox();
            bb.Content = aa;
            bb.IsChecked = true;
            this.LBVan.Items.Add(bb);
            }

        /// <summary>
        /// Модификация от 3 февраля 2015 года
        /// Заложен от 3 февраля 2015 года
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void savoBtn_Click(object sender, RoutedEventArgs e) {
            myTago aa = (myTago)this.spNewTago.DataContext;
            List<string> nl = new List<string>();
            foreach (CheckBox bb in LBVan.Items) {
                if (bb.IsChecked == true) {
                    nl.Add(bb.Content.ToString());
                    }
                }
            bool bf = true;
            if (nl.Count == aa.TCount) {
                bf = false;
                foreach (string bb in aa.TSet) {
                    if (nl.Contains(bb)) { continue; }
                    else {
                        bf = true;
                        break;
                        }
                    }
                }
            if (bf) {
                aa.SaveTago(nl);
                }
            this.Close();
            }

        private void canceloBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
