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
        private vrtVara pokazukha;
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

        /// <summary>
        /// Модификация от 18 марта 2016 года
        /// Заложен 17 марта 2016 года
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pzNextBtn_Click(object sender, RoutedEventArgs e) {
            vvDNF(1);
            }

        /// <summary>
        /// Модификация от 17 марта 2016 года
        /// Заложен 17 марта 2016 года
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pzPrevBtn_Click(object sender, RoutedEventArgs e) {
            }

        /// <summary>
        /// Модификация от 18 марта 2016 года
        /// Заложен 17 марта 2016 года
        /// </summary>
        /// <param name="dtl"></param>
        private void vvDNF(int dtl) {
            if (pokazukha == null) {
                Vario aa = ((Valuing)ValueGrido.CurrentItem).SelfVarianto;
                pokazukha = new vrtVara(aa);
                if (dtl == 1 || dtl == -2) {
                    pokazukha.ChangeCurrentNumber(dtl);
                    vvOtrisovka(pokazukha.GetColoredParagraph(), pokazukha.GetCurrentPoza());
                    }
            } else {
                pokazukha.ChangeCurrentNumber(dtl);
                vvOtrisovka(pokazukha.GetColoredParagraph(), pokazukha.GetCurrentPoza());
                }
            }

        /// <summary>
        /// Модификация от 18 марта 2016 года
        /// Заложен 18 марта 2016 года
        /// </summary>
        /// <param name="aa"></param>
        /// <param name="bb"></param>
        private void vvOtrisovka(Paragraph aa, pozo bb) {
            FlowDocument wk = this.VibroVara.Document;
            wk.Blocks.Clear();
            wk.Blocks.Add(aa);
            pfBoard.CurrentoPoza = bb;
            }

        /// <summary>
        /// Модификация от 18 марта 2016 года
        /// Заложен 18 марта 2016 года
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ValueGrido_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            string aa = ((Valuing)((DataGrid)sender).CurrentItem).Texa;
            pozo bb = ((vPoza)Grido.DataContext).Selfa;
            vvOtrisovka(new Paragraph(new Run(aa)), bb);
            }

    }
}
