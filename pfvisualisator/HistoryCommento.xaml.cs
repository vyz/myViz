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
    /// Логика взаимодействия для HistoryCommento.xaml
    /// </summary>
    public partial class HistoryCommento : Window
    {
        public HistoryCommento() {
            InitializeComponent();
            }

        /// <summary>
        /// Модификация от 6 марта 2015 года
        /// Заложен 6 марта 2015 года
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void savoBtn_Click(object sender, RoutedEventArgs e) {
            myleo aa = (myleo)this.Bosso.DataContext;
            myleo bb = vHistorySaver.GetDifferenceHistor(aa);
            myhistory cc = new myhistory(this.tbxComto.Text, bb);
            aa.AddHistoryObjecto(cc);
            bido bz = new bido();
            bz.PutLeoRecord(aa);
            this.Close();
            }

        /// <summary>
        /// Модификация от 6 марта 2015 года
        /// Заложен 6 марта 2015 года
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void canceloBtn_Click(object sender, RoutedEventArgs e) {
            this.Close();
            }
    }
}
