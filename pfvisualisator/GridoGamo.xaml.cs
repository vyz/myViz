﻿using System;
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
    /// Логика взаимодействия для GridoGamo.xaml
    /// </summary>
    public partial class GridoGamo : Window {
        public GridoGamo()
        {
            InitializeComponent();
        }

        private void GamoGrido_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            vGamo aa = (vGamo)this.GamoGrido.CurrentItem;
            if (!(aa == null))
            {
                var winda = new GamoWinda();
                winda.Grido.DataContext = aa;
                winda.Show();
            }

        }

        private void GamoGrido_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = e.Row.GetIndex() + 1;
        }

        private void SavoXMLListo_Click(object sender, RoutedEventArgs e) {
            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
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
                    vLGamo aa = (vLGamo)ManoDP.DataContext;
                    aa.SavoInXmlFilo(filename);
                    }
            } catch (Exception ex) {
                MessageBox.Show("Error: Could not wtite file on the disk. Original error: " + ex.Message);
                }
            }
        }
    }