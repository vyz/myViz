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
        private vrtGamo pokazukha;
        private vGamo vPartia;

        public GamoWinda() {

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
            vvDNF(1);
            }

        /// <summary>
        /// Модификация от 23 ноября 2015 года
        /// Заложен 23 ноября 2015 года
        /// </summary>
        private void cmdPrevPosition() {
            vvDNF(-1);
            }

#endregion -------------------------------------Command Handling--------------------------------------------

        /// <summary>
        /// Модификация от 29 апреля 2016 года
        /// Заложен 29 апреля 2016 года
        /// </summary>
        /// <param name="gg">Показываемая партия в виде vGamo</param>
        public void vvStartOtrisovka(vGamo gg) {
            vPartia = gg;
            this.Grido.DataContext = gg;
            pokazukha = new vrtGamo(vPartia.Gamma, this);
            Parto.Document.Blocks.Add(pokazukha.GetoParagraph());
            }

        private void vvOtrisovka(pozo bb)
        {
            pfBoard.CurrentoPoza = bb;
        }

        private void vvDNF(int dtl)
        {
            pokazukha.ChangeCurrentNumber(dtl);
            vvOtrisovka(pokazukha.GetCurrentPoza());
        }


        /// <summary>
        /// Модификация от 29 апреля 2016 года
        /// Заложен 29 апреля 2016 года
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void Spanio_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (sender is Span)
                {
                    Span aa = (Span)sender;
                    if (aa.Name.StartsWith("spi"))
                    {
                        int k = int.Parse(aa.Name.Substring(3));
                        vvDNF(k+1000);
                    } 
                }
            }
            catch (Exception ex)
            {
                LogoCM.OutString(ex.Message);
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

        //public vGamo Gamma { get { return vPartia; } set { vPartia = value; } }
        }
    }
