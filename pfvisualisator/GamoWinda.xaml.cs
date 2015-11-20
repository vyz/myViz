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
    /// Логика взаимодействия для GamoWinda.xaml
    /// </summary>
    public partial class GamoWinda : Window
    {
        public GamoWinda()
        {
            InitializeComponent();
        }

        private void Parto_MouseDoubleClick(object sender, MouseButtonEventArgs e) {
            TextBox curra = (TextBox)sender;
            int ipos = curra.CaretIndex;
            vGamo aa = (vGamo)Grido.DataContext;
            pozo newposo = aa.GetPozoOnOffset(ipos);
            if (newposo != null) {
                pfBoard.CurrentoPoza = newposo;
                }
            }
    }
}
