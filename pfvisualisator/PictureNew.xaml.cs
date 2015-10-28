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
using System.IO;

namespace pfVisualisator {
    /// <summary>
    /// Логика взаимодействия для PictureNew.xaml
    /// </summary>
    public partial class PictureNew : Window {
        private mypict winpicture;

        public PictureNew() {
            winpicture = new mypict();
            InitializeComponent();
            }

        /// <summary>
        /// Модификация от 15 января 2015 года
        /// Заложен от 15 января 2015 года
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FiloOpena_Click(object sender, RoutedEventArgs e) {
            // Create OpenFileDialog
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            // Set filter for file extension and default file extension
            dlg.InitialDirectory = @"D:\tempo";
            dlg.Filter = "MainoImageType (*.png)|*.png|All files (*.*)|*.*";
            dlg.FilterIndex = 1;
            dlg.RestoreDirectory = true;

            try {
                // Display OpenFileDialog by calling ShowDialog method
                Nullable<bool> result = dlg.ShowDialog();
                byte[] kartinka;

                // Get the selected file name and display in a TextBox
                if (result == true) {
                    // Open document
                    string filename = dlg.FileName;
                    BinaryReader br = new BinaryReader(File.Open(filename, FileMode.Open, FileAccess.Read, FileShare.Read));
                    int raz = (int)br.BaseStream.Length;
                    kartinka = br.ReadBytes(raz);
                    br.Close();
                    winpicture = new mypict(kartinka);
                    Bosso.DataContext = winpicture;
                    }
                }
            catch (Exception ex) {
                MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message);
                }
            }

        /// <summary>
        /// Модификация от 21 января 2015 года
        /// Заложен 21 января 2015 года
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FromoBuffo_Click(object sender, RoutedEventArgs e) {
            if (Clipboard.ContainsImage()) {
                BitmapSource cliba = Clipboard.GetImage();
                if (cliba != null) {
                    winpicture = new mypict(cliba);
                    Bosso.DataContext = winpicture;
                    }
                }
            }

        /// <summary>
        /// Модификация от 3 апреля 2015 года
        /// Заложен от 15 января 2015 года
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void savoBtn_Click(object sender, RoutedEventArgs e) {
            vRepoList.AddNewPicture(winpicture);
            this.Close();
            }

        private void canceloBtn_Click(object sender, RoutedEventArgs e) {
            this.Close();
            }

        }
    }
