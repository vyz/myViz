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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace pfVisualisator {

    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool edina = true;

        public MainWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Модификация от 17 декабря 2014 года
        /// Заложен 17 декабря 2014 года
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btWan_Click(object sender, RoutedEventArgs e) {
            vLRepo rl1 = new vLRepo(1);
            if (edina) {
                edina = false;
                rl1.Edina = false;
                var winda = new GridoRepo();
                winda.RepoGrido.ItemsSource = vRepoList.Listo;
                winda.Show();
            } else {
                edina = rl1.Edina;
                if (edina) {
                    edina = false;
                    rl1.Edina = false;
                    var winda = new GridoRepo();
                    winda.RepoGrido.ItemsSource = vRepoList.Listo;
                    winda.Show();
                } else {
                    vpfGluka.BackoMess(@"На будущее, пожалуйста, запомните: 
- Для работы с отчётами можно запускать только одну гридовую форму!");
                    }
                }
            }

        /// <summary>
        /// Модификация от 30 сентября 2015 года
        /// Заложен 30 сентября 2015 года
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GamoLoadFromFile_Click(object sender, RoutedEventArgs e) {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            // Set filter for file extension and default file extension
            dlg.InitialDirectory = Properties.Settings.Default.GamoPGNStartDir;
            dlg.Filter = "Стандартный (*.pgn)|*.pgn|All files (*.*)|*.*";
            dlg.FilterIndex = 1;
            dlg.RestoreDirectory = true;
            try {
                // Display OpenFileDialog by calling ShowDialog method
                Nullable<bool> result = dlg.ShowDialog();
                if (result == true) {
                    string filename = dlg.FileName;
                    pgWoka wrk = new pgWoka(filename);
                    wrk.GruzoWorko();
                    var winda = new GruzoGamo();
                    winda.Mano.DataContext = wrk;
                    winda.Show();
                    }
                }
            catch (Exception ex) {
                MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message);
                }
            }

        /// <summary>
        /// Модификация от 11 октября 2015 года
        /// Заложен 11 октября 2015 года
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GamoLoadFromXMLFile_Click(object sender, RoutedEventArgs e) {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            // Set filter for file extension and default file extension
            dlg.InitialDirectory = Properties.Settings.Default.GamoPGNStartDir;
            dlg.Filter = "Стандартный (*.xml)|*.xml|All files (*.*)|*.*";
            dlg.FilterIndex = 1;
            dlg.RestoreDirectory = true;
            try {
                // Display OpenFileDialog by calling ShowDialog method
                Nullable<bool> result = dlg.ShowDialog();
                if (result == true) {
                    string filename = dlg.FileName;
                    vLGamo wrk = new vLGamo(filename);
                    var winda = new GridoGamo();
                    winda.ManoDP.DataContext = wrk;
                    winda.GamoGrido.ItemsSource = wrk.Listo;
                    winda.Show();
                    }
            } catch (Exception ex) {
                MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message);
                }
            }

        private void vPozaTesto_Click(object sender, RoutedEventArgs e) {
            vPoza wrk = vPoza.CreateExemplarusForLife();
            var winda = new PozoWinda();
            winda.Grido.DataContext = wrk;
            winda.ValueGrido.ItemsSource = wrk.SetoAnalo;
            winda.pfBoard.CurrentoPoza = wrk.Selfa;
            winda.Show();
            }

        /// <summary>
        /// Модификация от 5 февраля 2016 года
        /// Заложен 5 февраля 2016 года
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PozoLoadFromXMLFile_Click(object sender, RoutedEventArgs e) {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            // Set filter for file extension and default file extension
            dlg.InitialDirectory = Properties.Settings.Default.GamoPGNStartDir;
            dlg.Filter = "Стандартный (*.xml)|*.xml|All files (*.*)|*.*";
            dlg.FilterIndex = 1;
            dlg.RestoreDirectory = true;
            try {
                // Display OpenFileDialog by calling ShowDialog method
                Nullable<bool> result = dlg.ShowDialog();
                if (result == true) {
                    string filename = dlg.FileName;
                    vPoza wrk = vPoza.CreateExemparFromXmlFile(filename);
                    var winda = new PozoWinda();
                    winda.Grido.DataContext = wrk;
                    winda.ValueGrido.ItemsSource = wrk.SetoAnalo;
                    winda.pfBoard.CurrentoPoza = wrk.Selfa;
                    winda.Show();
                    }
            } catch (Exception ex) {
                MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message);
                }
            }

        private void Window_Closed(object sender, EventArgs e)
        {
            LogoCM.finitalogo();
        }

        /// <summary>
        /// Модификация от 20 октября 2016 года
        /// Заложен 20 октября 2016 года
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void vGamoWithVaraTesto_Click(object sender, RoutedEventArgs e) {
            var winda = new GamoWinda();
            vGamo aa = vGamo.CreateExempWithVariantoAndComments(); 
            aa.EtaloCreate();
            winda.vvStartOtrisovka(aa);
            winda.Show();
            }

        }
}
