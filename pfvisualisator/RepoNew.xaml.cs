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
    /// Логика взаимодействия для RepoNew.xaml
    /// </summary>
    public partial class RepoNew : Window
    {
        public RepoNew()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 
        /// Модификация от 15 мая 2015 года
        /// Заложен 17 декабря 2014 года
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gvido3Btn_Click(object sender, RoutedEventArgs e) {
            bido baza = new bido();
            string sgvido = gvidoTB.Text;
            Guid repogvid;
            if (Guid.TryParse(sgvido, out repogvid)) {
                List<string> smas = baza.GetRepoStartSet(sgvido);
                if (smas.Count == 6) {
                    this.namoTB.Text = smas[0];
                    this.bignamoTB.Text = smas[1];
                    this.OrBaseCommentTB.Text = smas[2];
                    this.DescriptoTB.Text = smas[3];
                    this.ScriptFileNameTB.Text = smas[4];
                    this.SQueryTB.Text = smas[5];
                } else {
                    vpfGluka.BackoMess(string.Format("Возникли проблемы с чтением из базы данных информации по указанному GUID: {0}", sgvido.ToString()));
                    }
            } else {
                vpfGluka.BackoMess("Заданное значение GUID, увы, не является допустимым вариантом для GUID");
                }
            }

        /// <summary>
        /// Модификация от 15 мая 2015 года
        /// Заложен 15 мая 2015 года
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gvido4Btn_Click(object sender, RoutedEventArgs e) {
            bido baza = new bido();
            string sgvido = gvidoTB.Text;
            Guid repogvid;
            if (Guid.TryParse(sgvido, out repogvid)) {
                List<string> smas = baza.GetRepoStartSetFrom4(sgvido);
                if (smas.Count == 6) {
                    this.namoTB.Text = smas[0];
                    this.bignamoTB.Text = smas[1];
                    this.OrBaseCommentTB.Text = smas[2];
                    this.DescriptoTB.Text = smas[3];
                    this.ScriptFileNameTB.Text = smas[4];
                    this.SQueryTB.Text = smas[5];
                } else {
                    vpfGluka.BackoMess(string.Format("Возникли проблемы с чтением из четверошной базы данных информации по указанному GUID: {0}", sgvido.ToString()));
                    }
            } else {
                vpfGluka.BackoMess("Заданное значение GUID, увы, не является допустимым вариантом для GUID");
                }
            }

        /// <summary>
        /// Модификация от 17 декабря 2014 года
        /// Заложен 17 декабря 2014 года
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void canceloBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Модификация от 26 января 2015 года
        /// Заложен 17 декабря 2014 года
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void savoBtn_Click(object sender, RoutedEventArgs e) {
            Guid repogvid;
            if (Guid.TryParse(this.gvidoTB.Text, out repogvid)) {
                string[] smas = new string[6];
                smas[0] = this.namoTB.Text;
                smas[1] = this.bignamoTB.Text;
                smas[2] = this.OrBaseCommentTB.Text;
                smas[3] = this.DescriptoTB.Text;
                smas[4] = this.ScriptFileNameTB.Text;
                smas[5] = this.SQueryTB.Text;
                vLRepo rl2 = new vLRepo(0);
                string reso = rl2.AddNewObject( repogvid, smas );
                if (reso == string.Empty) {
                    rl2.SavoElement(repogvid);
                    this.Close();
                } else {
                    vpfGluka.BackoMess(reso);
                    }
            } else {
                vpfGluka.BackoMess("Для сохранения информации о репорте минимально необходимо наличие синтаксически правильного значения GUID." + Environment.NewLine +
                    "Заданное Вами не является допустимым вариантом для GUID.");
                }
            }
        }
    }
