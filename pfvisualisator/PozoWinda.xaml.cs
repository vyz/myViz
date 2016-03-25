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

namespace pfVisualisator {
    /// <summary>
    /// Логика взаимодействия для PozoWinda.xaml
    /// </summary>
    public partial class PozoWinda : Window {
        private vrtVara pokazukha;
        private Valuing currovalu;

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
            vvDNF(-1);
            }

        /// <summary>
        /// Модификация от 21 марта 2016 года
        /// Заложен 17 марта 2016 года
        /// </summary>
        /// <param name="dtl"></param>
        private void vvDNF(int dtl) {
            if (pokazukha == null && currovalu != null) {
                Vario aa = currovalu.SelfVarianto;
                pokazukha = new vrtVara(aa, this);
                if (dtl == -2) {
                    pokazukha.ChangeCurrentNumber(dtl);
                    }
                vvOtrisovka(pokazukha.GetColoredParagraph(), pokazukha.GetCurrentPoza());
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
        public void vvOtrisovka(Paragraph aa, pozo bb) {
            FlowDocument wk = this.VibroVara.Document;
            wk.Blocks.Clear();
            wk.Blocks.Add(aa);
            pfBoard.CurrentoPoza = bb;
            }

        /// <summary>
        /// Модификация от 21 марта 2016 года
        /// Заложен 18 марта 2016 года
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ValueGrido_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            if (currovalu != (Valuing)((DataGrid)sender).CurrentItem) {
                currovalu = (Valuing)((DataGrid)sender).CurrentItem;
                pokazukha = null;
                }
            string aa = currovalu.Texa;
            pozo bb = ((vPoza)Grido.DataContext).Selfa;
            vvOtrisovka(new Paragraph(new Run(aa)), bb);
            }

        /// <summary>
        /// Модификация от 22 марта 2016 года
        /// Заложен 22 марта 2016 года
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void Spanio_MouseLeftButtonDown(object sender, MouseButtonEventArgs e) {
            try { if (sender is Span) {
                    Span aa = (Span)sender;
                    if (aa.Name.StartsWith("spi")) {
                        int k = int.Parse(aa.Name.Substring(3));
                        pokazukha.SetCurrentNumber(k);
                        vvOtrisovka(pokazukha.GetColoredParagraph(), pokazukha.GetCurrentPoza());
                        }
                    }
            } catch (Exception ex) {
                LogoCM.OutString(ex.Message);
                }
            }

        private void AnaRes_TextChanged(object sender, TextChangedEventArgs e)
        {
            vPoza wrk = (vPoza) Grido.DataContext;
            ValueGrido.ItemsSource = wrk.SetoAnalo;
        }

        private void PosoWindo_Click(object sender, RoutedEventArgs e)
        {
            try {
                vPoza wrk = new vPoza(pfBoard.CurrentoPoza);
                wrk.Descripto = "Вариантная позиция - ";
                var winda = new PozoWinda();
                winda.Grido.DataContext = wrk;
                winda.pfBoard.CurrentoPoza = wrk.Selfa;
                winda.Show();
            } catch (Exception ex) {
                MessageBox.Show("Проблема с окном второй позиции " + ex.Message);
                }
            }

        }
    }
