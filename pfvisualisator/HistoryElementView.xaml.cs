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
    /// Логика взаимодействия для HistoryElementView.xaml
    /// </summary>
    public partial class HistoryElementView : Window
    {
        private PictoSeto pPictoList;

        public HistoryElementView() {
            InitializeComponent();
            }

        /// <summary>
        /// Модификация от 12 марта 2015 года
        /// Заложен 12 марта 2015 года
        /// </summary>
        /// <param name="aa"></param>
        public HistoryElementView(myhistory aa) : this() {
            Maina.DataContext = aa;
            TopaObago.DataContext = aa.Hobago;
            Title = string.Format("Просмотр истории для {0} от {1}", aa.Hobago.LeoTypo, aa.DatoStroke);
            if (aa.Hobago.LeoParams != null) {
                List<string> lsp = aa.Hobago.LeoParams;
                List<string> lsn = aa.Hobago.GetParamoNames();
                for (int i = 0; i < lsp.Count; i++) {
                    StackPanel esp = new StackPanel();
                    esp.Name = string.Format("esp{0}", i);
                    esp.Orientation = Orientation.Horizontal;
                    Label elb = new Label();
                    elb.Name = string.Format("elb{0}", i);
                    elb.Content = lsn[i] + ": ";
                    TextBlock etbl = new TextBlock();
                    etbl.Name = string.Format("etbl{0}", i);
                    etbl.VerticalAlignment = System.Windows.VerticalAlignment.Top;
                    etbl.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                    if( lsp[i] != null ) { 
                        etbl.Text = lsp[i];
                        etbl.Focusable = true;
                        MenuItem emi = new MenuItem();
                        emi.Header = "Подробнее об элементе .....";
                        emi.Click += Podrobnee_Click;
                        emi.DataContext = lsp[i];
                        ContextMenu ecm = new ContextMenu();
                        ecm.Name = string.Format("ecm{0}", i);
                        ecm.Items.Add(emi);
                        etbl.ContextMenu = ecm;
                        }
                    esp.Children.Add(elb);
                    esp.Children.Add(etbl);
                    stPLeft.Children.Add(esp);
                    }
                }
            if (aa.Hobago.LeoPictures != null)
            {
                //Вот такой хардкор
                pPictoList = new PictoSeto(aa.Hobago.LeoPictures);
                StackPanel esp = new StackPanel();
                esp.Name = "stPpicture";
                esp.Orientation = Orientation.Horizontal;
                esp.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
                esp.VerticalAlignment = System.Windows.VerticalAlignment.Top;
                Binding ebiComto = new Binding("CurPictDescription");
                ebiComto.Source = pPictoList;
                TextBlock etblComto = new TextBlock();
                etblComto.Name = "PictureCommento";
                etblComto.SetBinding(TextBlock.TextProperty, ebiComto);
                Button ebtPrev = new Button();
                ebtPrev.Name = "PrevBtn";
                ebtPrev.Content = "Prev";
                ebtPrev.Width = 75;
                ebtPrev.Click += PrevBtn_Click;
                Binding ebiPiIz = new Binding("CurPictPositionIz");
                ebiPiIz.Source = pPictoList;
                TextBlock etblCur = new TextBlock();
                etblCur.Name = "CurPos";
                etblCur.SetBinding(TextBlock.TextProperty, ebiPiIz);
                Button ebtNext = new Button();
                ebtNext.Name = "NextBtn";
                ebtNext.Content = "Next";
                ebtNext.Width = 75;
                ebtNext.Click += NextBtn_Click;
                esp.Children.Add(etblComto);
                esp.Children.Add(ebtPrev);
                esp.Children.Add(etblCur);
                esp.Children.Add(ebtNext);
                stPRight.Children.Add(esp);
                MenuItem emi = new MenuItem();
                emi.Header = "В отдельном окне .....";
                emi.Click += IncreasePicture_Click;
                ContextMenu ecm = new ContextMenu();
                ecm.Name = "ecmPicture";
                ecm.Items.Add(emi);
                Binding ebin = new Binding("CurPicture");
                ebin.Source = pPictoList;
                ebin.Converter = new BinaryImageConverter();
                Image eimo = new Image();
                eimo.Name = "CodoPictura";
                eimo.SetBinding(Image.SourceProperty, ebin);
                eimo.Stretch = Stretch.Uniform;
                eimo.StretchDirection = StretchDirection.Both;
                eimo.MaxHeight = 400;
                eimo.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
                eimo.ContextMenu = ecm;
                stPRight.Children.Add(eimo);
                }
            }

        private void Podrobnee_Click(object sender, RoutedEventArgs e) {
            var winda = new ElViewer();
            winda.tbEl.Text = (string)((MenuItem)sender).DataContext;
            bool? rewa = winda.ShowDialog();
            }

        private void PrevBtn_Click(object sender, RoutedEventArgs e) {
            pPictoList.PositionChange(-1);
            }

        private void NextBtn_Click(object sender, RoutedEventArgs e) {
            pPictoList.PositionChange(1);
            }

        private void IncreasePicture_Click(object sender, RoutedEventArgs e)
        {
            var winda = new BigPicture();
            winda.dockoPa.DataContext = pPictoList.SamoPicture;
            winda.Show();
        }
        
    }
}
