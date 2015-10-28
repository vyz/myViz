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
    /// Логика взаимодействия для GruzoGamo.xaml
    /// </summary>
    public partial class GruzoGamo : Window {
        public GruzoGamo() {
            InitializeComponent();
            }

        /// <summary>
        /// Модификация от 6 октября 2015 года
        /// Заложен 6 октября 2015 года
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GridBtn_Click(object sender, RoutedEventArgs e) {
            pgWoka paro = (pgWoka)Mano.DataContext;
            vLGamo gl1 = new vLGamo(paro.Listo);
            var winda = new GridoGamo();
            winda.ManoDP.DataContext = gl1;
            winda.GamoGrido.ItemsSource = gl1.Listo;
            winda.Show();
            }

        /// <summary>
        /// Модификация от 22 октября 2015 года
        /// Заложен 22 октября 2015 года
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SavoBtn_Click(object sender, RoutedEventArgs e) {
            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
            // Set filter for file extension and default file extension
            dlg.InitialDirectory = Properties.Settings.Default.GamoPGNStartDir;
            dlg.Filter = "Стандартный (*.txt)|*.txt|All files (*.*)|*.*";
            dlg.FilterIndex = 1;
            dlg.RestoreDirectory = true;
            try {
                // Display OpenFileDialog by calling ShowDialog method
                Nullable<bool> result = dlg.ShowDialog();
                if (result == true) {
                    string filename = dlg.FileName;
                    using (System.IO.StreamWriter sw = System.IO.File.CreateText(filename)) {
                        sw.WriteLine(Logo.Text);
                        }
                    }
            } catch (Exception ex) {
                MessageBox.Show("Error: Could not wtite file on the disk. Original error: " + ex.Message);
                }
            }
        }
    }
