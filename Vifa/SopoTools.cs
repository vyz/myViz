using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vifa
{
    public class SopoTools {

        public static string GetFiloForSave() {
            string reto = string.Empty;
            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
            dlg.Filter = "Стандартный (*.xml)|*.xml|All files (*.*)|*.*";
            dlg.FilterIndex = 1;
            dlg.RestoreDirectory = true;
            try {
                Nullable<bool> result = dlg.ShowDialog();
                if (result == true) {
                    reto = dlg.FileName;
                    }
            } catch (Exception ex) {
                System.Windows.MessageBox.Show("Проблемы с выбором файла для сохранения. Original error: " + ex.Message);
                }
            return reto;
            }

        public static string GetFiloForRead() {
            string reto = string.Empty;
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.Filter = "Стандартный (*.xml)|*.xml|All files (*.*)|*.*";
            dlg.FilterIndex = 1;
            dlg.RestoreDirectory = true;
            try {
                Nullable<bool> result = dlg.ShowDialog();
                if (result == true) {
                    reto = dlg.FileName;
                    }
            } catch (Exception ex) {
                System.Windows.MessageBox.Show("Проблемы с выбором файла для чтения. Original error: " + ex.Message);
                }
            return reto;
            }
        }
    }
