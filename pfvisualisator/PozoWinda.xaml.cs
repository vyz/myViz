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
    /// Логика взаимодействия для PozoWinda.xaml
    /// </summary>
    public partial class PozoWinda : Window
    {
        public PozoWinda() {
            InitializeComponent();
        }

        /// <summary>
        /// Модификация от 9 февраля 2016 года
        /// Заложен 9 февраля 2016 года
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AnaloBtn_Click(object sender, RoutedEventArgs e) {
            vPoza vp = (vPoza)Grido.DataContext;
            vp.rasschet(vlEngino.Houdini_3a_Pro_w32, 5, 5);
            }

        /// <summary>
        /// Модификация от 3 февраля 2016 года
        /// Заложен 3 февраля 2016 года
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void xSavoBtn_Click(object sender, RoutedEventArgs e) {
            vPoza vp = (vPoza)Grido.DataContext;
            vp.SavoInViborXmlFilo();
            }
    }
}
