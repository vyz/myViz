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
using System.Diagnostics;

namespace pfVisualisator {
    /// <summary>
    /// Логика взаимодействия для OKPRepo.xaml
    /// </summary>
    public partial class OKPRepo : Window {

        public OKPRepo() {
            InitializeComponent();
            //PrintLogicalTree(0, this); //По книге Адама Натана
            }

        protected override void OnContentRendered(EventArgs e)
        {
            base.OnContentRendered(e);
            //PrintVisualTree(0, this);  //По книге Адама Натана
        }

        private void IncreasePicture_Click(object sender, RoutedEventArgs e) {
            var winda = new BigPicture();
            winda.dockoPa.DataContext = ((vOKPReport) this.Grido.DataContext).SamoPicture;
            winda.Show();
            }

        private void AddBtn_Click(object sender, RoutedEventArgs e) {
            var winda = new PictureNew();
            winda.Show();
            }

        private void PrevBtn_Click(object sender, RoutedEventArgs e) {
            vOKPReport aa = (vOKPReport)this.Grido.DataContext;
            aa.PicturePositionChange(-1);
            this.Grido.DataContext = aa;
            }

        private void NextBtn_Click(object sender, RoutedEventArgs e) {
            vOKPReport aa = (vOKPReport)this.Grido.DataContext;
            aa.PicturePositionChange(1);
            this.Grido.DataContext = aa;
            }

        private void DelBtn_Click(object sender, RoutedEventArgs e) {
            vOKPReport aa = (vOKPReport)this.Grido.DataContext;
            aa.PictureDelete();
            this.Grido.DataContext = aa;
            }

        private void XMLFileSave_Click(object sender, RoutedEventArgs e) {
            vLRepo rl2 = new vLRepo(0);
            rl2.SavoList("xml");
            }

        private void MSSQLAllSave_Click(object sender, RoutedEventArgs e) {
            vLRepo rl2 = new vLRepo(0);
            rl2.SavoList("baso");
            }

        private void MSSQLCurSave_Click(object sender, RoutedEventArgs e) {
            vOKPReport aa = (vOKPReport)this.Grido.DataContext;
            bido bz = new bido();
            bz.PutLeoRecord(aa);
            }

        /// <summary>
        /// Модификация от 6 марта 2015 года
        /// Заложен 6 марта 2015 года
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MSSQLCurSaveWithHistory_Click(object sender, RoutedEventArgs e) {
            myleo aa = (vOKPReport)this.Grido.DataContext;
            if (vHistorySaver.IsADifference(aa)) {
                var winda = new HistoryCommento();
                winda.Bosso.DataContext = this.Grido.DataContext;
                bool? rewa = winda.ShowDialog();
            } else {
                vpfGluka.BackoMess("Историческое сохранение не проведено! Отсутствуют различия.");
                }
            }

        private void TagoAddo_Click(object sender, RoutedEventArgs e) {
            var winda = new NewTagoElement();
            myTago aa = ((vOKPReport)this.Grido.DataContext).Tago;
            winda.spNewTago.DataContext = aa;
            winda.TagoChecko(aa);
            winda.Show();
            }

        /// <summary>
        /// Модификация от 11 марта 2015 года
        /// Заложен 11 марта 2015 года
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HistoGrido_MouseDoubleClick(object sender, MouseButtonEventArgs e) {
            myhistory aa = (myhistory)this.HistoGrido.CurrentItem;
            if (!(aa == null)) {
                var winda = new HistoryElementView(aa);
                winda.Show();
                }
            }

        private void BinoGrido_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

        }

        private void BinoAddo_Click(object sender, RoutedEventArgs e) {
            myleo aa = (vOKPReport)this.Grido.DataContext;
            var winda = new NewBinaryko();
            winda.Bosso.DataContext = this.Grido.DataContext;
            bool? rewa = winda.ShowDialog();
            }

        void PrintLogicalTree(int depth, object obj)
        {
            // Print the object with preceding spaces that represent its depth
            Debug.WriteLine(new string(' ', depth) + obj);
            // Sometimes leaf nodes aren’t DependencyObjects (e.g. strings)
            if (!(obj is DependencyObject)) return;
            // Recursive call for each logical child
            foreach (object child in LogicalTreeHelper.GetChildren(
            obj as DependencyObject))
                PrintLogicalTree(depth + 1, child);
        }

        void PrintVisualTree(int depth, DependencyObject obj)
        {
            // Print the object with preceding spaces that represent its depth
            Debug.WriteLine(new string(' ', depth) + obj);
            // Recursive call for each visual child
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
                PrintVisualTree(depth + 1, VisualTreeHelper.GetChild(obj, i));
        }

        /// <summary>
        /// Модификация от 14 мая 2015 года
        /// Заложен 13 мая 2015 года
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BinoRepoAddo_Click(object sender, RoutedEventArgs e) {
            vOKPReport aa = (vOKPReport)this.Grido.DataContext;
            bido bz = new bido();
            Guid ogu = Guid.Parse(aa.SUid);
            byte[] zna = bz.GetRepoBinoData(ogu);
            if( zna == null ) {
                vpfGluka.BackoMess("Не обнаружено бинарных данных в базе для данного отчёта.");
            } else if( aa.HasBinoRepoData ) {
                vpfGluka.BackoMess("Нельзя повторно загружать бинарные данные об отчёте, можно только обновлять.");
            } else {
                mybino dd = new mybino(zna, "№", "ОКПBaseReport", "Без");
                aa.AddBinaryObjecto(dd);
                bz.PutLeoRecord(aa);
                }
            }

        }
    }
