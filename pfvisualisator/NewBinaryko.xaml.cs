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

namespace pfVisualisator
{
    /// <summary>
    /// Логика взаимодействия для NewBinaryko.xaml
    /// </summary>
    public partial class NewBinaryko : Window
    {
        private mybino winbino;

//        public mybino PubloBino { get { return winbino; } }

        public NewBinaryko()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Модификация от 29 апреля 2015 года
        /// Заложен 29 апреля 2015 года
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FiloOpena_Click(object sender, RoutedEventArgs e) {
            // Create OpenFileDialog
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            // Set filter for file extension and default file extension
            dlg.InitialDirectory = @"D:\tempo";
            dlg.Filter = "MainoBukoType (*.djvu)|*.djvu|All files (*.*)|*.*";
            dlg.FilterIndex = 1;
            dlg.RestoreDirectory = true;

            try {
                // Display OpenFileDialog by calling ShowDialog method
                Nullable<bool> result = dlg.ShowDialog();
                byte[] binka;

                // Get the selected file name and display in a TextBox
                if (result == true) {
                    // Open document
                    string filename = dlg.FileName;
                    BinaryReader br = new BinaryReader(File.Open(filename, FileMode.Open, FileAccess.Read, FileShare.Read));
                    long razmeroo = br.BaseStream.Length;
                    if (razmeroo < 200000000L) {
                        int raz = (int)razmeroo;
                        binka = br.ReadBytes(raz);
                        br.Close();
                        FileInfo afi = new FileInfo(filename);
                        winbino = new mybino(binka, afi.Extension, afi.Name, "Комментировать будете?");
                        this.FiloTBl.Text = winbino.FiloNamo;
                        this.SizoTBl.Text = winbino.SizoStroke;
                        this.CommentoTbx.Text = winbino.Commento;
                    } else {
                        this.CommentoTbx.Text = string.Format(@"Размер выбранного файла -{0}- 
превышает зафиксированную квоту в 200 000 000 байт
и не будет загружен:(", razmeroo);
                        br.Close();
                        }
                    }
                }
            catch (Exception ex) {
                MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message);
                }
            }

        private void FromoBaso_Click(object sender, RoutedEventArgs e)
        {

        }

        /// <summary>
        /// Модификация от 29 апреля 2015 года
        /// Заложен 29 апреля 2015 года
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void savoBtn_Click(object sender, RoutedEventArgs e) {
            if (winbino != null) {
                myleo aa = (myleo)this.Bosso.DataContext;
                aa.AddBinaryObjecto(winbino);
                this.Close();
            } else {
                vpfGluka.BackoMess("А что Вы собрались сохранять, Уважаемый?");
                }
            }

        private void canceloBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
