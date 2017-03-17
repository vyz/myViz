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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Media.Animation;
using System.Windows.Threading;
using System.ComponentModel;
using OnlyWorko;

namespace Vifa
{
    /// <summary>
    /// Логика взаимодействия для Borda.xaml
    /// </summary>
    public partial class Borda : UserControl
    {

        #region Inner Class
        
        /// <summary>
        /// Integer Point
        /// </summary>
        public struct IntPoint {
            /// <summary>
            /// Class Ctor
            /// </summary>
            /// <param name="x"></param>
            /// <param name="y"></param>
            public  IntPoint(int x, int y) { X = x; Y = y; }
            /// <summary>X point</summary>
            public  int X;
            /// <summary>Y point</summary>
            public  int Y;
        }

        /// <summary>
        /// Show a piece moving from starting to ending point
        /// </summary>
        private class SyncFlash {
            /// <summary>Chess Board Control</summary>
            private Borda           m_chessBoardControl;
            /// <summary>Solid Color Brush to flash</summary>
            private SolidColorBrush             m_brush;
            /// <summary>First Flash Color</summary>
            private Color                       m_colorStart;
            /// <summary>Second Flash Color</summary>
            private Color                       m_colorEnd;
            //System.Windows.Threading.Dispatcher
            DispatcherFrame                     m_dispatcherFrame;


            /// <summary>
            /// Class Ctor
            /// </summary>
            /// <param name="chessBoardControl">    Chess Board Control</param>
            /// <param name="brush">                Solid Color Brush to flash</param>
            /// <param name="colorStart">           First flashing color</param>
            /// <param name="colorEnd">             Second flashing color</param>
            public SyncFlash(Borda chessBoardControl, SolidColorBrush brush, Color colorStart, Color colorEnd) {
                m_chessBoardControl = chessBoardControl;
                m_brush             = brush;
                m_colorStart        = colorStart;
                m_colorEnd          = colorEnd;
            }

            /// <summary>
            /// Flash the specified cell
            /// </summary>
            /// <param name="iCount">                   Flash count</param>
            /// <param name="dSec">                     Flash duration</param>
            /// <param name="eventHandlerTerminated">   Event handler to call when flash is finished</param>
            private void FlashCell(int iCount, double dSec, EventHandler eventHandlerTerminated) {
                ColorAnimation                  colorAnimation;
            
                colorAnimation                  = new ColorAnimation(m_colorStart, m_colorEnd, new Duration(TimeSpan.FromSeconds(dSec)));
                colorAnimation.AutoReverse      = true;
                colorAnimation.RepeatBehavior   = new RepeatBehavior(2);
                if (eventHandlerTerminated != null) {
                    colorAnimation.Completed   += new EventHandler(eventHandlerTerminated);
                }
                m_brush.BeginAnimation(SolidColorBrush.ColorProperty, colorAnimation);
            }

            /// <summary>
            /// Show the move
            /// </summary>
            public void Flash() {
                m_chessBoardControl.IsEnabled       = false;
                FlashCell(4, 0.15, new EventHandler(FirstFlash_Completed));
                m_dispatcherFrame   = new DispatcherFrame();
                Dispatcher.PushFrame(m_dispatcherFrame);
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="sender"></param>
            /// <param name="e"></param>
            private void FirstFlash_Completed(object sender, EventArgs e) {
                m_chessBoardControl.IsEnabled   = true;
                m_dispatcherFrame.Continue      = false;

            }
        } // Class SyncFlash

                /// <summary>Event argument for the MoveSelected event</summary>
                public class MoveSelectedEventArgs : System.EventArgs {
                    /// <summary>Move position</summary>
                    public Mova  Move;
                    /// <summary>
                    /// Constructor
                    /// </summary>
                    /// <param name="move">     Move position</param>
                    public                                      MoveSelectedEventArgs(Mova move) { Move = move; }
                }

                /// <summary>Event argument for the QueryPiece event</summary>
                public class QueryPieceEventArgs : System.EventArgs {
                            /// <summary>Position of the square</summary>
                            public int                  Pos;
                            /// <summary>Piece</summary>
                            public Pieco    Piece;
                            /// <summary>
                            /// Constructor
                            /// </summary>
                            /// <param name="iPos">     Position of the square</param>
                            /// <param name="ePiece">   Piece</param>
                            public                                      QueryPieceEventArgs(int iPos, Pieco ePiece) { Pos = iPos; Piece = ePiece; }
                        }
        /*Консервация 25.10.2015
                /// <summary>Event argument for the QueryPawnPromotionType event</summary>
                public class QueryPawnPromotionTypeEventArgs : System.EventArgs {
                    /// <summary>Promotion type (Queen, Rook, Bishop, Knight or Pawn)</summary>
                    public Borda.MoveTypeE                 PawnPromotionType;
                    /// <summary>Possible pawn promotions in the current context</summary>
                    public Borda.ValidPawnPromotionE       ValidPawnPromotion;
                    /// <summary>
                    /// Constructor
                    /// </summary>
                    /// <param name="eValidPawnPromotion">  Possible pawn promotions in the current context</param>
                    public                                      QueryPawnPromotionTypeEventArgs(Borda.ValidPawnPromotionE eValidPawnPromotion) { ValidPawnPromotion = eValidPawnPromotion; PawnPromotionType = ChessBoard.MoveTypeE.Normal; }
                }
         Консервация 25.10.2015*/

        #endregion

        #region Members
        /// <summary>Lite Cell Color property</summary>
        public static readonly DependencyProperty       LiteCellColorProperty;
        /// <summary>Dark Cell Color property</summary>
        public static readonly DependencyProperty       DarkCellColorProperty;
        /// <summary>White Pieces Color property</summary>
        public static readonly DependencyProperty       WhitePieceColorProperty;
        /// <summary>Black Pieces Color property</summary>
        public static readonly DependencyProperty       BlackPieceColorProperty;

        private bool DesignModeYES;
        /// <summary>Delegate for the MoveSelected event</summary>
        public delegate void                            MoveSelectedEventHandler(object sender, MoveSelectedEventArgs e);
        /// <summary>Called when a user select a valid move to be done</summary>
        public event MoveSelectedEventHandler           MoveSelected;
        /// <summary>Delegate for the QueryPiece event</summary>
        public delegate void                            QueryPieceEventHandler(object sender, QueryPieceEventArgs e);
        /// <summary>Called when chess control in design mode need to know which piece to insert in the board</summary>
        public event QueryPieceEventHandler             QueryPiece;
        /// <summary>Delegate for the QueryPawnPromotionType event</summary>
//        public delegate void                            QueryPawnPromotionTypeEventHandler(object sender, QueryPawnPromotionTypeEventArgs e);
        /// <summary>Called when chess control needs to know which type of pawn promotion must be done</summary>
//        public event QueryPawnPromotionTypeEventHandler QueryPawnPromotionType;
        /// <summary>Called to refreshed the command state (menu, toolbar etc.)</summary>
        public event System.EventHandler                UpdateCmdState;

        private pozo currentopoza;
        
        /// <summary>Piece Set to use</summary>
        private PieceSet                                m_pieceSet;

        /// <summary>Board</summary>
        private Pieco[] m_board;
        /// <summary>Array of frames containing the chess piece</summary>
        private Border[]                                m_arrBorder;
        /// <summary>Array containing the current piece</summary>
        private Pieco[] m_arrPiece;
        /// <summary>true to have white in the bottom of the screen, false to have black</summary>
        private bool                                    m_bWhiteInBottom = true;
        ///// <summary>Font use to draw coordinate on the side of the control</summary>
        //private Font                                  m_fontCoord;  TODO
        /// <summary>Currently selected cell</summary>
        private IntPoint                                m_ptSelectedCell;
        /// <summary>true to enable auto-selection</summary>
        private bool                                    m_bAutoSelection;
        /// <summary>Time the last search was started</summary>
        private DateTime                                m_dateTimeStartSearching;
        /// <summary>Elapse time of the last search</summary>
        private TimeSpan                                m_timeSpanLastSearch;
        #endregion

        #region Board creation

        /// <summary>
        /// Static Ctor
        /// </summary>
        static Borda() {
            LiteCellColorProperty   = DependencyProperty.Register("LiteCellColor",
                                                                  typeof(Color),
                                                                  typeof(Borda),
                                                               new FrameworkPropertyMetadata(Colors.Moccasin,
                                                                                             FrameworkPropertyMetadataOptions.AffectsRender,
                                                                                             ColorInfoChanged));
            DarkCellColorProperty   = DependencyProperty.Register("DarkCellColor",
                                                               typeof(Color),
                                                               typeof(Borda),
                                                               new FrameworkPropertyMetadata(Colors.SaddleBrown,
                                                                                             FrameworkPropertyMetadataOptions.AffectsRender,
                                                                                             ColorInfoChanged));
            WhitePieceColorProperty   = DependencyProperty.Register("WhitePieceColor",
                                                               typeof(Color),
                                                               typeof(Borda),
                                                               new FrameworkPropertyMetadata(Colors.White,
                                                                                             FrameworkPropertyMetadataOptions.AffectsRender,
                                                                                             ColorInfoChanged));
            BlackPieceColorProperty   = DependencyProperty.Register("BlackPieceColor",
                                                                    typeof(Color),
                                                                    typeof(Borda),
                                                                    new FrameworkPropertyMetadata(Colors.Black,
                                                                                                  FrameworkPropertyMetadataOptions.AffectsRender,
                                                                                                  ColorInfoChanged));
        }

        /// <summary>
        /// Class Ctor
        /// </summary>
        public Borda()
        {
            InitializeComponent();
            currentopoza = pozo.Starto();    ///////ALLLERT
            m_board                     = currentopoza.BoardoSet;    ///////ALLLERT
            m_ptSelectedCell            = new IntPoint(-1, -1);
            m_bAutoSelection            = true;
            DesignModeYES = false;
            InitCell();
            PieceSet = new PieceSetStandard("Leipzig", "Figures");
        }

        /// <summary>
        /// Refresh the board color
        /// </summary>
        private void RefreshBoardColor() {
            int     iPos;
            Border  border;
            Brush   brushDark;
            Brush   brushLite;

            iPos        = 63;
            brushDark   = new SolidColorBrush(DarkCellColor); // m_colorInfo.m_colDarkCase);
            brushLite   = new SolidColorBrush(LiteCellColor); // m_colorInfo.m_colLiteCase);
            for (int y = 0; y < 8; y++) {
                for (int x = 0; x < 8; x++) {
                    border              = m_arrBorder[iPos];
                    border.Background   = (((x + y) & 1) == 0) ? brushLite : brushDark;
                    iPos--;
                }
            }
        }

        /// <summary>
        /// Initialize the cell
        /// </summary>
        private void InitCell() {
            int     iPos;
            Border  border;
            Brush   brushDark;
            Brush   brushLite;

            m_arrBorder = new Border[64];
            m_arrPiece  = new Pieco[64];
            iPos        = 63;
            brushDark   = new SolidColorBrush(DarkCellColor);   // m_colorInfo.m_colDarkCase);
            brushLite   = new SolidColorBrush(LiteCellColor);   // m_colorInfo.m_colLiteCase);
            for (int y = 0; y < 8; y++) {
                for (int x = 0; x < 8; x++) {
                    border                  = new Border();
                    border.Name             = "Cell" + (iPos.ToString());
                    border.BorderThickness  = new Thickness(0);
                    border.Background       = (((x + y) & 1) == 0) ? brushLite : brushDark;
                    border.BorderBrush      = border.Background;
                    border.SetValue(Grid.ColumnProperty, x);
                    border.SetValue(Grid.RowProperty, y);
                    m_arrBorder[iPos]       = border;
                    m_arrPiece[iPos]    = Pieco.None;
                    CellContainer.Children.Add(border);
                    iPos--;
                }
            }
        }

        /// <summary>
        /// Set the chess piece control
        /// </summary>
        /// <param name="iBoardPos">    Board position</param>
        /// <param name="ePiece">       Piece</param>
        private void SetPieceControl(int iBoardPos, Pieco ePiece) {
            Border      border;
            UserControl userControlPiece;

            border              = m_arrBorder[iBoardPos];
            userControlPiece    = m_pieceSet[ePiece];
            if (userControlPiece != null) {
                userControlPiece.Margin  = (border.BorderThickness.Top == 0) ? new Thickness(3) : new Thickness(1);
            }
            m_arrPiece[iBoardPos]   = ePiece;
            border.Child            = userControlPiece;
        }

        /// <summary>
        /// Refresh the specified cell
        /// </summary>
        /// <param name="iBoardPos">    Board position</param>
        /// <param name="bFullRefresh"> true to refresh even if its the same piece</param>
        private void RefreshCell(int iBoardPos, bool bFullRefresh) {
           Pieco   ePiece;

            if (m_board != null && m_pieceSet != null) {
                ePiece = m_board[iBoardPos];
                if (ePiece != m_arrPiece[iBoardPos] || bFullRefresh) {
                    SetPieceControl(iBoardPos, ePiece);
                }
            }
        }

        /// <summary>
        /// Refresh the specified cell
        /// </summary>
        /// <param name="iBoardPos">    Board position</param>
        private void RefreshCell(int iBoardPos) {
            RefreshCell(iBoardPos, false);  // bFullRefresh
        }

        /// <summary>
        /// Refresh the board
        /// </summary>
        /// <param name="bFullRefresh"> Refresh even if its the same piece</param>
        private void Refresh(bool bFullRefresh) {
            if (m_board != null && m_pieceSet != null) {
                for (int iBoardPos = 0; iBoardPos < 64; iBoardPos++) {
                    RefreshCell(iBoardPos, bFullRefresh);
                }
            }
        }

        /// <summary>
        /// Refresh the board
        /// </summary>
        public void Refresh() {
            Refresh(false); // bFullRefresh
        }

        /// <summary>
        /// Reset the board to the initial condition
        /// </summary>
        public void ResetBoard() {
//            m_board.ResetBoard();               ////ALLLERT
            SelectedCell    = new IntPoint(-1, -1);
            OnUpdateCmdState(System.EventArgs.Empty);
            Refresh(false); // bForceRefresh
        }
        #endregion

        #region Properties
        /// <summary>
        /// Called when Image property changed
        /// </summary>
        private static void ColorInfoChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e) {
            Borda   me;

            me = obj as Borda;
            if (me != null && e.OldValue != e.NewValue) {
                me.RefreshBoardColor();
            }
        }

        /// <summary>
        /// Image displayed to the button
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [Browsable(true)]
        [Bindable(true)]
        [Category("Brushes")]
        [Description("Lite Cell Color")]
        public Color LiteCellColor {
            get {
                return ((Color)GetValue(LiteCellColorProperty));
            }
            set {
                SetValue(LiteCellColorProperty, value);
            }
        }

        /// <summary>
        /// Image displayed to the button
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [Browsable(true)]
        [Bindable(true)]
        [Category("Brushes")]
        [Description("Dark Cell Color")]
        public Color DarkCellColor {
            get {
                return ((Color)GetValue(DarkCellColorProperty));
            }
            set {
                SetValue(DarkCellColorProperty, value);
            }
        }

        /// <summary>
        /// Image displayed to the button
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [Browsable(true)]
        [Bindable(true)]
        [Category("Brushes")]
        [Description("White Pieces Color")]
        public Color WhitePieceColor {
            get {
                return ((Color)GetValue(WhitePieceColorProperty));
            }
            set {
                SetValue(WhitePieceColorProperty, value);
            }
        }

        /// <summary>
        /// Image displayed to the button
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [Browsable(true)]
        [Bindable(true)]
        [Category("Brushes")]
        [Description("Black Pieces Color")]
        public Color BlackPieceColor {
            get {
                return ((Color)GetValue(BlackPieceColorProperty));
            }
            set {
                SetValue(BlackPieceColorProperty, value);
            }
        }

        /// <summary>
        /// Current piece set
        /// </summary>
        public PieceSet PieceSet {
            get {
                return(m_pieceSet);
            }
            set {
                if (m_pieceSet != value) {
                    m_pieceSet = value;
                    Refresh(true);  // bForceRefresh
                }
            }
        }

        /// <summary>
        /// Current chess board
        /// </summary>
        public Pieco[] Board {
            get {
                return(m_board);
            }
            set {
                if (m_board != value) {
                    m_board = value;
                    Refresh(false); // bForceRefresh
                }
            }
        }

        /// <summary>
        /// По аналогии с Board и на нее (Board) перевесили перерисовку через внутренний вызов при изменении
        /// Модификация от 16 ноября 2015 года
        /// Заложен 16 ноября 2015 года
        /// </summary>
        public pozo CurrentoPoza {
            get { return currentopoza; }
            set {
                if (currentopoza != value) {
                    currentopoza = value;
                    Board = currentopoza.BoardoSet;
                    }
                }
            }


        /// <summary>
        /// Determine if the White are in the top or bottom of the draw board
        /// </summary>
        public bool WhiteInBottom {
            get {
                return(m_bWhiteInBottom);
            }
            set {
                if (value != m_bWhiteInBottom) {
                    m_bWhiteInBottom = value;
                    Refresh(false);  // bForceRefresh
                }
            }
        }

        /// <summary>
        /// Enable or disable the auto selection mode
        /// </summary>
        public bool AutoSelection {
            get {
                return(m_bAutoSelection);
            }
            set {
                m_bAutoSelection = value;
            }
        }

        /// <summary>
        /// Определяет режим просмотра или создания
        /// Модификация от 27 октября 2015 года
        /// Вёрнуто 27 октября 2015 года
        /// </summary>
        public bool BoardDesignMode {
            get { return (DesignModeYES); }
            set {
                MessageBoxResult eRes;
                bool whitomove = true;

                if (DesignModeYES != value) {
                    if (value) { //Перешли в составление позиции - пока об этом не думаем.
                        OnUpdateCmdState(System.EventArgs.Empty);
                    } else {
                        eRes = MessageBox.Show("А будет ли первый ход за беленькими?", "Дизайнёр", MessageBoxButton.YesNo);
                        whitomove = (eRes == MessageBoxResult.Yes);
                        //Здесь бы организовать какуб-то работу по созданию позиции и вообще для чего всё это?
                        }
                    }
                }
            }

        /// <summary>
        /// Gets the number of move which can be undone
        /// </summary>
        public int UndoCount {
            get {
//                return(m_board.MovePosStack.PositionInList + 1);
                return 1;
            }
        }

        /// <summary>
        /// Gets the number of move which can be redone
        /// </summary>
        public int RedoCount {
            get {
                int iCurPos;
                int iCount;
                
//                iCurPos = m_board.MovePosStack.PositionInList;
//                iCount  = m_board.MovePosStack.Count;
                iCurPos = 5;
                iCount = 4;
                return(iCount - iCurPos - 1);
            }
        }

        /// <summary>
        /// Current color to play
        /// </summary>
  /*      public ChessBoard.PlayerColorE NextMoveColor {
            get {
                return(m_board.NextMoveColor);
            }
        }
   */ 

        /// <summary>
        /// List of played moves
        /// </summary>
/*
        private ChessBoard.MovePosS[] MoveList {
            get {
                ChessBoard.MovePosS[]   arrMoveList;
                int                     iMoveCount;
                
                iMoveCount  = m_board.MovePosStack.PositionInList + 1;
                arrMoveList = new ChessBoard.MovePosS[iMoveCount];
                if (iMoveCount != 0) {
                    m_board.MovePosStack.List.CopyTo(0, arrMoveList, 0, iMoveCount);
                }
                return(arrMoveList);
            }
        }
 */

        /// <summary>
        /// Time use to find the last best move
        /// </summary>
        public TimeSpan LastFindBestMoveTimeSpan {
            get {
                return(m_timeSpanLastSearch);
            }
        }

        /// <summary>
        /// Set the cell selection  appearance
        /// </summary>
        /// <param name="ptCell"></param>
        /// <param name="bSelected"></param>
        private void SetCellSelectionState(IntPoint ptCell, bool bSelected) {
            Border  border;
            Control ctl;
            int     iPos;

            if (ptCell.X != -1 && ptCell.Y != -1) {
                iPos                    = ptCell.X + ptCell.Y * 8;
                border                  = m_arrBorder[iPos];
                border.BorderBrush      = (bSelected) ? Brushes.Black : border.Background;
                border.BorderThickness  = (bSelected) ? new Thickness(1) : new Thickness(0);
                ctl                     = border.Child as Control;
                if (ctl != null) {
                    ctl.Margin  = (bSelected) ? new Thickness(1) : new Thickness(3);
                }
            }
        }

        /// <summary>
        /// Currently selected case
        /// </summary>
        public IntPoint SelectedCell {
            get {
                return(m_ptSelectedCell);
            }
            set {
                SetCellSelectionState(m_ptSelectedCell, false);
                m_ptSelectedCell    = value;
                SetCellSelectionState(m_ptSelectedCell, true);
            }
        }

        /// <summary>
        /// true if a cell is selected
        /// </summary>
        public bool IsCellSelected {
            get {
                return(SelectedCell.X != -1 || SelectedCell.Y != -1);
            }
        }

        #endregion

        #region Events
        /// <summary>
        /// Trigger the UpdateCmdState event. Called when command state need to be reevaluated.
        /// </summary>
        /// <param name="e">    Event argument</param>
        protected void OnUpdateCmdState(System.EventArgs e) {
            if (UpdateCmdState != null) {
                UpdateCmdState(this, e);
            }
        }

        /// <summary>
        /// Trigger the MoveSelected event
        /// </summary>
        /// <param name="e">    Event arguments</param>
        protected virtual void OnMoveSelected(MoveSelectedEventArgs e) {
            if (MoveSelected != null) {
                MoveSelected(this, e);
            }
        }

        /// <summary>
        /// OnQueryPiece:       Trigger the QueryPiece event
        /// </summary>
        /// <param name="e">    Event arguments</param>
        protected virtual void OnQueryPiece(QueryPieceEventArgs e) {
            if (QueryPiece != null) {
                QueryPiece(this, e);
            }
        }

/*
        /// <summary>
        /// OnQweryPawnPromotionType:   Trigger the QueryPawnPromotionType event
        /// </summary>
        /// <param name="e">            Event arguments</param>
        protected virtual void OnQueryPawnPromotionType(QueryPawnPromotionTypeEventArgs e) {
            if (QueryPawnPromotionType != null) {
                QueryPawnPromotionType(this, e);
            }
        }
 */ 
        #endregion

        #region Methods

/* 
        /// <summary>
        /// Create a new game using the specified list of moves
        /// </summary>
        /// <param name="chessBoardStarting">   Starting board or null if standard board</param>
        /// <param name="listMove">             List of moves</param>
        /// <param name="eNextMoveColor">       Color starting to play</param>
        /// <param name="strWhitePlayerName">   Name of the player playing white pieces</param>
        /// <param name="strBlackPlayerName">   Name of the player playing black pieces</param>
        /// <param name="eWhitePlayerType">     Type of player playing white pieces</param>
        /// <param name="eBlackPlayerType">     Type of player playing black pieces</param>
        /// <param name="spanPlayerWhite">      Timer for white</param>
        /// <param name="spanPlayerBlack">      Timer for black</param>
        public virtual void CreateGameFromMove(ChessBoard                   chessBoardStarting,
                                               List<ChessBoard.MovePosS>    listMove,
                                               ChessBoard.PlayerColorE      eNextMoveColor,
                                               string                       strWhitePlayerName,
                                               string                       strBlackPlayerName,
                                               PgnParser.PlayerTypeE        eWhitePlayerType,
                                               PgnParser.PlayerTypeE        eBlackPlayerType,
                                               TimeSpan                     spanPlayerWhite,
                                               TimeSpan                     spanPlayerBlack) {
            m_board.CreateGameFromMove(chessBoardStarting,
                                       listMove,
                                       eNextMoveColor);
            if (m_moveListUI != null) {
                m_moveListUI.Reset(m_board);
            }
            WhitePlayerName = strWhitePlayerName;
            BlackPlayerName = strBlackPlayerName;
            WhitePlayerType = eWhitePlayerType;
            BlackPlayerType = eBlackPlayerType;
            OnUpdateCmdState(System.EventArgs.Empty);
            m_gameTimer.ResetTo(m_board.NextMoveColor,
                                spanPlayerWhite.Ticks,
                                spanPlayerBlack.Ticks);
            m_gameTimer.Enabled = true;
            Refresh(false); // bForceRefresh
        }
*/

        /// <summary>
        /// Set the piece in a case. Can only be used in design mode.
        /// </summary>
        public void SetCaseValue(int iPos, Pieco ePiece) {

            if (BoardDesignMode) {
                m_board[iPos] = ePiece;
                RefreshCell(iPos);
            }
        }


        /// <summary>
        /// Gets the position express in a human form
        /// </summary>
        /// <param name="ptStart">      Starting Position</param>
        /// <param name="ptEnd">        Ending position</param>
        /// <returns>
        /// Human form position
        /// </returns>
        static public string GetHumanPos(IntPoint ptStart, IntPoint ptEnd) {
            return(pGetHumanPos(ptStart.X + (ptStart.Y << 3)) + "-" + pGetHumanPos(ptEnd.X + (ptEnd.Y << 3)));
        }

        /// <summary>
        /// Утилиточная не объектная функция
        /// Поместил сюда временно так как непонятно где ее место.
        /// Модификация от 27 октября 2015 года
        /// Заложен 27 октября 2015 года
        /// </summary>
        /// <param name="iPos"></param>
        /// <returns></returns>
        static private string pGetHumanPos(int iPos) {
            string  strRetVal;
            int     iColPos;
            int     iRowPos;
            
            iColPos     = 7 - (iPos & 7);
            iRowPos     = iPos >> 3;
            strRetVal   = ((Char)(iColPos + 'A')).ToString() + ((Char)(iRowPos + '1')).ToString();
            return(strRetVal);
            }


        /// <summary>
        /// Gets the cell position from a mouse event
        /// </summary>
        /// <param name="e">        Mouse event argument</param>
        /// <param name="ptCell">   Resulting cell</param>
        /// <returns>
        /// true if succeed, false if mouse don't point to a cell
        /// </returns>
        public bool GetCellFromPoint(MouseEventArgs e, out IntPoint ptCell) {
            bool        bRetVal;
            Point       pt;
            int         iCol;
            int         iRow;
            double      dActualWidth;
            double      dActualHeight;

            pt  = e.GetPosition(CellContainer);
            dActualHeight   = CellContainer.ActualHeight;
            dActualWidth    = CellContainer.ActualWidth;
            iCol            = (int)(pt.X * 8 / dActualWidth);
            iRow            = (int)(pt.Y * 8 / dActualHeight);
            if (iCol >= 0 && iCol < 8 && iRow >= 0 && iRow < 8) {
                ptCell  = new IntPoint(7 - iCol, 7 - iRow);
                bRetVal = true;
            } else {
                ptCell  = new IntPoint(-1, -1);
                bRetVal = false;
            }
            return(bRetVal);
        }

        /// <summary>
        /// Flash the specified cell
        /// </summary>
        /// <param name="ptCell">   Cell to flash</param>
        public void FlashCell(IntPoint ptCell) {
            int             iCellPos;
            Border          border;
            Brush           brush;
            Color           colorStart;
            Color           colorEnd;
            SyncFlash       syncFlash;
            
            iCellPos = ptCell.X + ptCell.Y * 8;
            if (((ptCell.X + ptCell.Y) & 1) != 0) {
                colorStart  = DarkCellColor;    // m_colorInfo.m_colDarkCase;
                colorEnd    = LiteCellColor;    // m_colorInfo.m_colLiteCase;
            } else {
                colorStart  = LiteCellColor;    // m_colorInfo.m_colLiteCase;
                colorEnd    = DarkCellColor;    // m_colorInfo.m_colDarkCase;
            }
            border                          = m_arrBorder[iCellPos];
            brush                           = border.Background.Clone();
            border.Background               = brush;
            syncFlash                       = new SyncFlash(this, brush as SolidColorBrush, colorStart, colorEnd);
            syncFlash.Flash();
        }

        /// <summary>
        /// Flash the specified cell
        /// </summary>
        /// <param name="iStartPos">    Cell position</param>
        private void FlashCell(int iStartPos) {
            IntPoint    pt;

            pt  = new IntPoint(iStartPos & 7, iStartPos / 8);
            FlashCell(pt);
        }
/*
        /// <summary>
        /// Get additional position to update when doing or undoing a special move
        /// </summary>
        /// <param name="movePos">      Position of the move</param>
        /// <returns>
        /// Array of position to undo
        /// </returns>
        private int[] GetPosToUpdate(Mova movePos) {
            List<int>       arrRetVal = new List<int>(2);

            if ((movePos.Type & ChessBoard.MoveTypeE.MoveTypeMask) == ChessBoard.MoveTypeE.Castle) {
                switch(movePos.EndPos) {
                case 1:
                    arrRetVal.Add(0);
                    arrRetVal.Add(2);
                    break;
                case 5:
                    arrRetVal.Add(7);
                    arrRetVal.Add(4);
                    break;
                case 57:
                    arrRetVal.Add(56);
                    arrRetVal.Add(58);
                    break;
                case 61:
                    arrRetVal.Add(63);
                    arrRetVal.Add(60);
                    break;
                default:
                    MessageBox.Show("Oops!");
                    break;
                }
            } else if ((movePos.Type & ChessBoard.MoveTypeE.MoveTypeMask) == ChessBoard.MoveTypeE.EnPassant) {
                arrRetVal.Add((movePos.StartPos & 56) + (movePos.EndPos & 7));
            }
            return(arrRetVal.ToArray());
        }
*/
/*
        /// <summary>
        /// Show before move is done
        /// </summary>
        /// <param name="movePos">      Position of the move</param>
        /// <param name="bFlash">       true to flash the from and destination pieces</param>
        private void ShowBeforeMove(Mova movePos, bool bFlash) {
            if (bFlash) {
                FlashCell(movePos.StartPos);
            }
        }
 */ 
/*
        /// <summary>
        /// Show after move is done
        /// </summary>
        /// <param name="movePos">      Position of the move</param>
        /// <param name="bFlash">       true to flash the from and destination pieces</param>
        private void ShowAfterMove(Mova movePos, bool bFlash) {
            int[]       arrPosToUpdate;

            RefreshCell(movePos.StartPos);
            RefreshCell(movePos.EndPos);
            if (bFlash) {
                FlashCell(movePos.EndPos);
            }
            arrPosToUpdate = GetPosToUpdate(movePos);
            foreach (int iPos in arrPosToUpdate) {
                if (bFlash) {
                    FlashCell(iPos);
                }
                RefreshCell(iPos);
            }
        }
 */ 
/*
        /// <summary>
        /// Play the specified move
        /// </summary>
        /// <param name="movePos">      Position of the move</param>
        /// <param name="bFlash">       true to flash the from and destination pieces</param>
        /// <param name="iPermCount">   Permutation count</param>
        /// <param name="iDepth">       Maximum depth use to find the move</param>
        /// <param name="iCacheHit">    Number of permutation found in cache</param>
        /// <returns>
        /// NoRepeat, FiftyRuleRepeat, ThreeFoldRepeat, Tie, Check, Mate
        /// </returns>
        public ChessBoard.MoveResultE DoMove(ChessBoard.MovePosS movePos, bool bFlash, int iPermCount, int iDepth, int iCacheHit) {
            ChessBoard.MoveResultE eRetVal;

            
            ShowBeforeMove(movePos, bFlash);
            eRetVal = m_board.DoMove(movePos);
            ShowAfterMove(movePos, bFlash);
            OnUpdateCmdState(System.EventArgs.Empty);
            return(eRetVal);
        }
 */ 
/*
        /// <summary>
        /// Undo the last move
        /// </summary>
        /// <param name="bFlash">   true to flash the from and destination pieces</param>
        public void UndoMove(bool bFlash) {
            Mova movePos;
            int[]               arrPosToUpdate;

            movePos = m_board.MovePosStack.CurrentMove;
            if (bFlash) {
                FlashCell(movePos.EndPos);
            }
            m_board.UndoMove();
            RefreshCell(movePos.EndPos);
            RefreshCell(movePos.StartPos);
            if (bFlash) {
                FlashCell(movePos.StartPos);
            }
            arrPosToUpdate = GetPosToUpdate(movePos);
            Array.Reverse(arrPosToUpdate);
            foreach (int iPos in arrPosToUpdate) {
                if (bFlash) {
                    FlashCell(iPos);
                }
                RefreshCell(iPos);
            }
            if (m_moveListUI != null) {
                m_moveListUI.RedoPosChanged();
            }
            OnUpdateCmdState(System.EventArgs.Empty);
            m_gameTimer.PlayerColor = m_board.NextMoveColor;
            m_gameTimer.Enabled     = true;
        }
 */ 
/*
        /// <summary>
        /// Redo the most recently undone move
        /// </summary>
        /// <param name="bFlash">   true to flash</param>
        /// <returns>
        /// NoRepeat, FiftyRuleRepeat, ThreeFoldRepeat, Check, Mate
        /// </returns>
        public ChessBoard.MoveResultE RedoMove(bool bFlash) {
            ChessBoard.MoveResultE  eRetVal;
            ChessBoard.MovePosS     movePos;
            
            movePos = m_board.MovePosStack.NextMove;
            ShowBeforeMove(movePos, bFlash);
            eRetVal = m_board.RedoMove();
            ShowAfterMove(movePos, bFlash);
            if (m_moveListUI != null) {
                m_moveListUI.RedoPosChanged();
            }
            OnUpdateCmdState(System.EventArgs.Empty);
            m_gameTimer.PlayerColor = m_board.NextMoveColor;
            m_gameTimer.Enabled = (eRetVal == ChessBoard.MoveResultE.NoRepeat || eRetVal == ChessBoard.MoveResultE.Check);
            return(eRetVal);
        }
 */ 
/*
        /// <summary>
        /// Select a move by index using undo/redo buffer to move
        /// </summary>
        /// <param name="iIndex">   Index of the move. Can be -1</param>
        /// <param name="bSucceed"> true if index in range</param>
        /// <returns>
        /// Repeat result
        /// </returns>
        public ChessBoard.MoveResultE SelectMove(int iIndex, out bool bSucceed) {
            ChessBoard.MoveResultE  eRetVal = ChessBoard.MoveResultE.NoRepeat;
            int                         iCurPos;
            int                         iCount;
            
            iCurPos = m_board.MovePosStack.PositionInList;
            iCount  = m_board.MovePosStack.Count;
            if (iIndex >= -1 && iIndex < iCount) {
                bSucceed = true;
                if (iCurPos < iIndex) {
                    while (iCurPos != iIndex) {
                        eRetVal = RedoMove(false);
                        iCurPos++;
                    }
                } else if (iCurPos > iIndex) {
                    while (iCurPos != iIndex) {
                        UndoMove(false);
                        iCurPos--;
                    }
                }
            } else {
                bSucceed = false;
            }
            return(eRetVal);
        }
*/
/*
        /// <summary>
        /// ShowHintMove:                   Show a hint on the next move to do
        /// </summary>
        /// <param name="movePos">          Move position</param>
        public void ShowHintMove(ChessBoard.MovePosS movePos) {
            ShowBeforeMove(movePos, true);
            m_board.DoMoveNoLog(movePos);
            ShowAfterMove(movePos, true);
            m_board.UndoMoveNoLog(movePos);
            ShowAfterMove(movePos, false);
        }
*/
/*
        /// <summary>
        /// ShowHint:                       Find and show a hint on the next move to do
        /// </summary>
        /// <param name="searchMode">       Search mode</param>
        /// <param name="movePos">          Move position found</param>
        /// <param name="iPermCount">       Permutation count</param>
        /// <param name="iCacheHit">        Cache hit</param>
        /// <returns>
        /// true if a hint has been shown
        /// </returns>
        public bool ShowHint(SearchEngine.SearchMode searchMode, out ChessBoard.MovePosS movePos, out int iPermCount, out int iCacheHit) {
            bool    bRetVal;
            int     iMaxDepth;
            
            if (FindBestMove(searchMode, null, out movePos, out iPermCount, out iCacheHit, out iMaxDepth)) {
                ShowHintMove(movePos);
                bRetVal = true;
            } else {
                bRetVal = false;
            }
            return(bRetVal);
        }
*/
/*
        /// <summary>
        /// Intercept Mouse click
        /// </summary>
        /// <param name="e">    Event Parameter</param>
        protected override void OnMouseDown(MouseButtonEventArgs e) {
            IntPoint                        pt;
            ChessBoard.MovePosS             tMove;
            ChessBoard.ValidPawnPromotionE  eValidPawnPromotion;
            QueryPieceEventArgs             eQueryPieceEventArgs;
            int                             iPos;
            QueryPawnPromotionTypeEventArgs eventArg;
            
            base.OnMouseDown(e);
            if (BoardDesignMode) {
                if (GetCellFromPoint(e, out pt)) {
                    iPos                 = pt.X + (pt.Y << 3);
                    eQueryPieceEventArgs = new QueryPieceEventArgs(iPos, ChessBoard[iPos]);
                    OnQueryPiece(eQueryPieceEventArgs);
                    ChessBoard[iPos] = eQueryPieceEventArgs.Piece;
                    RefreshCell(iPos);
                }
            } else if (AutoSelection) {
                if (GetCellFromPoint(e, out pt)) {
                    if (SelectedCell.X == -1 || SelectedCell.Y == -1) {
                        SelectedCell = pt;
                    } else {
                        if (SelectedCell.X == pt.X  && SelectedCell.Y == pt.Y) {
                            SelectedCell = new IntPoint(-1, -1);
                        } else {
                            tMove = ChessBoard.FindIfValid(m_board.NextMoveColor,
                                                            SelectedCell.X + (SelectedCell.Y << 3),
                                                            pt.X + (pt.Y << 3));
                            if (tMove.StartPos != 255) {                                                           
                                eValidPawnPromotion = ChessBoard.FindValidPawnPromotion(m_board.NextMoveColor, 
                                                                                        SelectedCell.X + (SelectedCell.Y << 3),
                                                                                        pt.X + (pt.Y << 3));
                                if (eValidPawnPromotion != ChessBoard.ValidPawnPromotionE.None) {
                                    eventArg = new QueryPawnPromotionTypeEventArgs(eValidPawnPromotion);
                                    OnQueryPawnPromotionType(eventArg);
                                    if (eventArg.PawnPromotionType == ChessBoard.MoveTypeE.Normal) {
                                        tMove.StartPos = 255;
                                    } else {
                                        tMove.Type &= ~ChessBoard.MoveTypeE.MoveTypeMask;
                                        tMove.Type |= eventArg.PawnPromotionType;
                                    }
                                }
                            }
                            SelectedCell = new IntPoint(-1, -1);
                            if (tMove.StartPos == 255) {
                                System.Console.Beep();
                            } else {
                                OnMoveSelected(new MoveSelectedEventArgs(tMove));
                            }
                        }
                    }
                }
            }
        }
        */
        #endregion



    }
}
