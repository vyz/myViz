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
    /// Логика взаимодействия для GridoRepo.xaml
    /// </summary>
    public partial class GridoRepo : Window
    {
        public GridoRepo() {
            InitializeComponent();
            }

        /// <summary>
        /// Модификация от 29 апреля 2015 года
        /// Добавлено присвоение отдельного контента стековой панели с тегами
        /// Заложен в 2014 году
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RepoGrido_MouseDoubleClick(object sender, MouseButtonEventArgs e) {
            vOKPReport aa = (vOKPReport) this.RepoGrido.CurrentItem;
            if (!(aa == null)) {
                var winda = new OKPRepo();
                winda.Grido.DataContext = aa;
                winda.stackPanelTago.DataContext = aa.Tago;
                vRepoList.SetCurrentRepo(aa);
                vHistorySaver.AddNewObject(aa);
                if (aa.LeoHistoList != null) {
                    winda.HistoGrido.ItemsSource = aa.LeoHistoList;
                    }
                if (aa.LeoBinoList != null) {
                    winda.BinoGrido.ItemsSource = aa.LeoBinoList;
                    }
                winda.Show();
                }
            }

        private void AddNewReport_Click(object sender, RoutedEventArgs e)
        {
            var winda = new RepoNew();
            bool? rewa = winda.ShowDialog();
        }

        /// <summary>
        /// Модификация от 17 декабря 2014 года
        /// Заложен 17 декабря 2014 года
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Closed(object sender, EventArgs e)
        {
            vLRepo rl3 = new vLRepo(0);
            rl3.Edina = true;
        }

        private void RepoGrido_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = e.Row.GetIndex() + 1;
        }
      
    }
}
