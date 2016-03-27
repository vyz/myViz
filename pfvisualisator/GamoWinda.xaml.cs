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
            ExecutedRoutedEventHandler onExecutedCmd;
            CanExecuteRoutedEventHandler onCanExecuteCmd;

            InitializeComponent();

            onExecutedCmd = new ExecutedRoutedEventHandler(OnExecutedCmd);
            onCanExecuteCmd = new CanExecuteRoutedEventHandler(OnCanExecuteCmd);

            CommandBindings.Add(new CommandBinding(NextMoveCommand, onExecutedCmd, onCanExecuteCmd));
            CommandBindings.Add(new CommandBinding(PrevMoveCommand, onExecutedCmd, onCanExecuteCmd));
        }

        /// <summary>
        /// Модификация от 22 ноября 2015 года
        /// Заложен 22 ноября 2015 года
        /// </summary>
        static GamoWinda() {
            NextMoveCommand.InputGestures.Add(new KeyGesture(Key.Right));
            PrevMoveCommand.InputGestures.Add(new KeyGesture(Key.Left));
            }

        public static readonly RoutedUICommand NextMoveCommand = new RoutedUICommand("NextMove", "NextPosition", typeof(GamoWinda));
        public static readonly RoutedUICommand PrevMoveCommand = new RoutedUICommand("PrevMove", "PrevPosition", typeof(GamoWinda));

#region -------------------------------------Command Handling--------------------------------------------
        public virtual void OnExecutedCmd(object sender, ExecutedRoutedEventArgs e) {
            if (e.Command == NextMoveCommand) {
                cmdNextPosition();
            } else if (e.Command == PrevMoveCommand) {
                cmdPrevPosition();
                }
            }
        public virtual void OnCanExecuteCmd(object sender, CanExecuteRoutedEventArgs e)
        {
            if (e.Command == NextMoveCommand)
            {
                //Нужна проверка на последность позиции
                e.CanExecute = true;
            }
            else if (e.Command == PrevMoveCommand)
            {
                //Нужна проверка на начальность позиции
                e.CanExecute = true;
            }
        }

        /// <summary>
        /// Модификация от 22 ноября 2015 года
        /// Заложен 22 ноября 2015 года
        /// </summary>
        private void cmdNextPosition() {
            vGamo aa = (vGamo)Grido.DataContext;
            pozo newposo = aa.GetNextPozo();
            if (newposo != null) {
                pfBoard.CurrentoPoza = newposo;
                }
            }

        /// <summary>
        /// Модификация от 23 ноября 2015 года
        /// Заложен 23 ноября 2015 года
        /// </summary>
        private void cmdPrevPosition() {
            vGamo aa = (vGamo)Grido.DataContext;
            pozo newposo = aa.GetPrevPozo();
            if (newposo != null) {
                pfBoard.CurrentoPoza = newposo;
                }
            }

#endregion -------------------------------------Command Handling--------------------------------------------

        private void Parto_MouseDoubleClick(object sender, MouseButtonEventArgs e) {
            TextBox curra = (TextBox)sender;
            int ipos = curra.CaretIndex;
            vGamo aa = (vGamo)Grido.DataContext;
            pozo newposo = aa.GetPozoOnOffset(ipos);
            if (newposo != null) {
                pfBoard.CurrentoPoza = newposo;
                }
            }

        private void PrevBtn_Click(object sender, RoutedEventArgs e)
        {
            cmdPrevPosition();
        }

        private void NextBtn_Click(object sender, RoutedEventArgs e)
        {
            cmdNextPosition();
        }

        /// <summary>
        /// Модификация от 26 января 2016 года
        /// Заложен 25 января 2016 года
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PosoWindo_Click(object sender, RoutedEventArgs e) {
            try {
                vPoza wrk = new vPoza(pfBoard.CurrentoPoza, this.Heado.Text);
                vGamo aa = (vGamo)Grido.DataContext;
                wrk.Descripto = string.Format("Создано {0} из интерфейса для партии {1}", DateTime.Now.ToLongDateString(), aa.LeoGuid.ToString());
                var winda = new PozoWinda();
                winda.Grido.DataContext = wrk;
                winda.pfBoard.CurrentoPoza = wrk.Selfa;
                winda.Show();
            } catch (Exception ex) {
                MessageBox.Show("Проблема с окном второй позиции " + ex.Message);
                }
            }
        }
    }
