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
        System.Windows.Threading.DispatcherTimer sptimer = null;

        public GamoWinda() {

            ExecutedRoutedEventHandler onExecutedCmd;
            CanExecuteRoutedEventHandler onCanExecuteCmd;

            InitializeComponent();

            onExecutedCmd = new ExecutedRoutedEventHandler(OnExecutedCmd);
            onCanExecuteCmd = new CanExecuteRoutedEventHandler(OnCanExecuteCmd);

            CommandBindings.Add(new CommandBinding(NextMoveCommand, onExecutedCmd, onCanExecuteCmd));
            CommandBindings.Add(new CommandBinding(PrevMoveCommand, onExecutedCmd, onCanExecuteCmd));
            CommandBindings.Add(new CommandBinding(BegoPosCommand, onExecutedCmd, onCanExecuteCmd));
            CommandBindings.Add(new CommandBinding(EndoPosCommand, onExecutedCmd, onCanExecuteCmd));
            CommandBindings.Add(new CommandBinding(IntoVaraCommand, onExecutedCmd, onCanExecuteCmd));
            CommandBindings.Add(new CommandBinding(ExitVaraCommand, onExecutedCmd, onCanExecuteCmd));
        }

        /// <summary>
        /// Модификация от 28 октября 2016 года
        /// Заложен 22 ноября 2015 года
        /// </summary>
        static GamoWinda() {
            NextMoveCommand.InputGestures.Add(new KeyGesture(Key.Right));
            PrevMoveCommand.InputGestures.Add(new KeyGesture(Key.Left));
            BegoPosCommand.InputGestures.Add(new KeyGesture(Key.Home));
            EndoPosCommand.InputGestures.Add(new KeyGesture(Key.End));
            IntoVaraCommand.InputGestures.Add(new KeyGesture(Key.Down));
            ExitVaraCommand.InputGestures.Add(new KeyGesture(Key.Up));
            }

        public static readonly RoutedUICommand NextMoveCommand = new RoutedUICommand("NextMove", "NextPosition", typeof(GamoWinda));
        public static readonly RoutedUICommand PrevMoveCommand = new RoutedUICommand("PrevMove", "PrevPosition", typeof(GamoWinda));
        public static readonly RoutedUICommand BegoPosCommand  = new RoutedUICommand("BegoPos",  "BegoPosition", typeof(GamoWinda));
        public static readonly RoutedUICommand EndoPosCommand  = new RoutedUICommand("EndoPos",  "EndoPosition", typeof(GamoWinda));
        public static readonly RoutedUICommand IntoVaraCommand = new RoutedUICommand("IntoVara", "IntoVariant", typeof(GamoWinda));
        public static readonly RoutedUICommand ExitVaraCommand = new RoutedUICommand("ExitVara", "ExitVariant", typeof(GamoWinda));

#region -------------------------------------Command Handling--------------------------------------------
        /// <summary>
        /// Модификация от 7 октября 2016 года
        /// Заложен в ноябре 2015 года
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public virtual void OnExecutedCmd(object sender, ExecutedRoutedEventArgs e) {
            if (e.Command == NextMoveCommand) {
                cmdNextPosition();
            } else if (e.Command == PrevMoveCommand) {
                cmdPrevPosition();
            } else if (e.Command == BegoPosCommand) {
                cmdBegoPosition();
            } else if (e.Command == EndoPosCommand) {
                cmdEndoPosition();
            } else if (e.Command == IntoVaraCommand) {
                cmdIntoVariant();
            } else if (e.Command == ExitVaraCommand) {
                cmdExitFromVariant();
                }
            }

        /// <summary>
        /// Модификация от 7 октября 2016 года
        /// Заложен в ноябре 2015 года
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public virtual void OnCanExecuteCmd(object sender, CanExecuteRoutedEventArgs e) {
            EnaBo feba = (pokazukha == null) ? EnaBo.nona : pokazukha.Knopki;

            if (e.Command == NextMoveCommand) {
                //Нужна проверка на последность позиции
                e.CanExecute = (feba & EnaBo.next) > 0;
            } else if (e.Command == PrevMoveCommand) {
                //Нужна проверка на начальность позиции
                e.CanExecute = (feba & EnaBo.prev) > 0;
            } else if (e.Command == BegoPosCommand) {
                e.CanExecute = (feba & EnaBo.bego) > 0;
            } else if (e.Command == EndoPosCommand) {
                e.CanExecute = (feba & EnaBo.endo) > 0;
            } else if (e.Command == IntoVaraCommand) {
                e.CanExecute = (feba & EnaBo.vara) > 0;
            } else if (e.Command == ExitVaraCommand) {
                e.CanExecute = (feba & EnaBo.exit) > 0;
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

        /// <summary>
        /// Модификация от 6 октября 2016 года
        /// Заложен 6 октября 2016 года
        /// </summary>
        private void cmdBegoPosition() {
            }

        /// <summary>
        /// Модификация от 6 октября 2016 года
        /// Заложен 6 октября 2016 года
        /// </summary>
        private void cmdEndoPosition() {
            }

        /// <summary>
        /// Модификация от 6 октября 2016 года
        /// Заложен 6 октября 2016 года
        /// </summary>
        private void cmdIntoVariant() {
            }

        /// <summary>
        /// Модификация от 6 октября 2016 года
        /// Заложен 6 октября 2016 года
        /// </summary>
        private void cmdExitFromVariant() {
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

        /// <summary>
        /// Модификация от 23 ноября 2016 года
        /// Модификация от 23 ноября 2016 года
        /// </summary>
        /// <param name="timovar"></param>
        private void ZapuskKino(int timovar) {
            if (sptimer == null) {
                sptimer = new System.Windows.Threading.DispatcherTimer();
                }
            //Предположительно нужно и имзменение действующего таймера, т.е. ЭЛЬЗЕ
            sptimer.Tick += new EventHandler(sptimerTick);
            sptimer.Interval = new TimeSpan(0, 0, timovar);
            sptimer.Start();
            }

        /// <summary>
        /// Модификация от 23 ноября 2016 года
        /// Модификация от 23 ноября 2016 года
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void sptimerTick(object sender, EventArgs e)
        {
            cmdNextPosition();
        }

        private void PrevBtn_Click(object sender, RoutedEventArgs e) {
            cmdPrevPosition();
            }

        private void NextBtn_Click(object sender, RoutedEventArgs e) {
            cmdNextPosition();
            }

        private void BegoBtn_Click(object sender, RoutedEventArgs e) {
            cmdBegoPosition();
            }

        private void ExitBtn_Click(object sender, RoutedEventArgs e) {
            cmdExitFromVariant();
            }

        private void EndoBtn_Click(object sender, RoutedEventArgs e) {
            cmdEndoPosition();
            }

        private void VaraBtn_Click(object sender, RoutedEventArgs e) {
            cmdIntoVariant();
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

        /// <summary>
        /// Модификация от 23 ноября 2016 года
        /// Модификация от 23 ноября 2016 года
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Kino1_Click(object sender, RoutedEventArgs e) {
            ZapuskKino(1);
            }

        /// <summary>
        /// Модификация от 23 ноября 2016 года
        /// Модификация от 23 ноября 2016 года
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Kino2_Click(object sender, RoutedEventArgs e) {
            ZapuskKino(5);
            }

        /// <summary>
        /// Модификация от 23 ноября 2016 года
        /// Модификация от 23 ноября 2016 года
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Kino3_Click(object sender, RoutedEventArgs e) {
            ZapuskKino(10);
            }


        }
    }
