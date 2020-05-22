using Snakes_and_Ladders.Shapes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Snakes_and_Ladders
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml. This class is the main window that is lanched when the exe is executed. This will handle all the UIElements to show on the window.
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        #region Variables
        private Collection<PropertyChangedEventHandler> _Handlers = new Collection<PropertyChangedEventHandler>();
        private DiceFace currentFace;
        private Dictionary<int, int> _ladderNumbers;
        private Dictionary<int, int> _snakeNumbers;
        private List<Ladder> _ladders;
        private List<Snake> _snakes;
        private List<Token> _tokens;
        private enGameType _enGameType;
        private enGameToken _currentToken;
        private bool bIsDiceClicked = false;
        private WinText _winText;
        private int _numOfPlayersLeft = 0;
        private Random _LadderSnakeRandom;
        private string _GameRulesText;
        private bool bIsTokenMoved = false;
        private double _dbLadderLineThicknessFactor = 120.0;
        private double _dbStepDifferenceFactor = 50.0;
        private double _dbDiceAnimationTime = 400;
        private double _dbSnakeThicknessFactor;
        private bool _bCanSnakesAndLaddersIntersect;
        private int _intMaxLadderLength;
        private int _intMaxSnakeLength;
        private Brush _LadderStartBoxColor;
        private Brush _LadderEndBoxColor;
        private Brush _SnakeTailBoxColor;
        private Brush _SnakeHeadBoxColor;
        private Brush _SnakeColor;
        private Brush _LadderColor;
        private int _intNumberOfSnakes;
        private int _intNumberOfLadders;

        public event PropertyChangedEventHandler PropertyChanged
        {
            add
            {
                _Handlers.Add(value);
            }
            remove
            {
                _Handlers.Remove(value);
            }
        }
        #endregion

        #region Constructor and Destructor

        /// <summary>
        /// This is the constructor for the mainwindow. Here we will initialize all the variables that we are going to use throughout the application.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            //Initial face
            currentFace = DiceFace.Three;
            _ladders = new List<Ladder>();
            _snakes = new List<Snake>();
            _tokens = new List<Token>();

            //This is the data that will be saved to reproduce the gameboard.
            _ladderNumbers = new Dictionary<int, int>();
            _snakeNumbers = new Dictionary<int, int>();

            _SnakeTailBoxColor = Brushes.PaleVioletRed;
            _SnakeHeadBoxColor = Brushes.DarkRed;
            _LadderEndBoxColor = Brushes.DarkGreen;
            _LadderStartBoxColor = Brushes.LawnGreen;
            _SnakeColor = Brushes.Red;
            _LadderColor = Brushes.Black;

            _intMaxLadderLength = 60;
            _intMaxSnakeLength = 80;
            _dbSnakeThicknessFactor = 40.0;
            _intNumberOfSnakes = 5;
            _intNumberOfLadders = 5;
            _bCanSnakesAndLaddersIntersect = true;
            LoadSettings();

            CurrentToken = enGameToken.Green;
            DataContext = this;
            GameDice.DiceAnimation.Completed += DiceAnimation_Completed;
            _LadderSnakeRandom = new Random();

            DiceCanvas.IsEnabled = false;
            CreateBoardButton.IsEnabled = true;
            StartGameButton.IsEnabled = false;
            PlayerSelectionPanel.IsEnabled = true;
            StopGameButton.IsEnabled = false;

            _GameRulesText = "Rules of the game:\n1. The colored tokens will start moving once you score first six.\n2. The token which reaches 100 first wins the game.\n3. If you reach to the number where snake head is, you will have to go to the snake tail position.\n4. If you reach to the number where ladder starts, you will reach to the higher end of the ladder.";
        }

        ~MainWindow()
        {

        }

        #endregion

        #region Properties
        public double BoardColumnWidth
        {
            get { return this.ActualWidth * 0.833; }
        }

        public double DiceColumnWidth
        {
            get { return this.ActualWidth * 0.167; }
        }

        public double GridRowHeight
        {
            get { return this.ActualHeight - 20; }
        }

        public double ButtonTextFontSize
        {
            get
            {
                return this.ActualHeight * 0.0214;
            }
        }

        public enGameToken CurrentToken
        {
            get { return _currentToken; }
            set
            {
                _currentToken = value;
                OnPropertyChanged("CurrentUserColor");
            }
        }

        public Brush CurrentUserColor
        {
            get
            {
                if (_currentToken == enGameToken.Green)
                    return Brushes.ForestGreen;
                else if (_currentToken == enGameToken.Blue)
                    return Brushes.DarkBlue;
                else if (_currentToken == enGameToken.Red)
                    return Brushes.DarkRed;
                else
                    return Brushes.Goldenrod;
            }
        }

        public string GameRulesText
        {
            get { return _GameRulesText; }
        }

        public Dictionary<int, int> SnakeNumbers
        {
            get { return _snakeNumbers; }
        }

        public Dictionary<int, int> LadderNumbers
        {
            get { return _ladderNumbers; }
        }


        public List<Token> Tokens
        {
            get
            {
                return _tokens;
            }
        }
        public List<Ladder> Ladders
        {
            get
            {
                return _ladders;
            }
        }

        public List<Snake> Snakes
        {
            get
            {
                return _snakes;
            }
        }

        public enGameType GameType
        {
            get
            {
                return _enGameType;
            }
            set
            {
                _enGameType = value;
                _numOfPlayersLeft = (int)value;
                AddTokens();
                OnPropertyChanged("TwoPlayerRB_IsChecked");
                OnPropertyChanged("ThreePlayerRB_IsChecked");
                OnPropertyChanged("FourPlayerRB_IsChecked");
            }
        }

        public bool TwoPlayerRB_IsChecked
        {
            get
            {
                if (_enGameType == enGameType.TwoPlayer)
                    return true;
                else
                    return false;
            }
            set
            {
                if (GameType != enGameType.TwoPlayer)
                    GameType = enGameType.TwoPlayer;
            }
        }

        public bool ThreePlayerRB_IsChecked
        {
            get
            {
                if (_enGameType == enGameType.ThreePlayer)
                    return true;
                else
                    return false;
            }
            set
            {
                if (GameType != enGameType.ThreePlayer)
                    GameType = enGameType.ThreePlayer;
            }
        }

        public bool FourPlayerRB_IsChecked
        {
            get
            {
                if (_enGameType == enGameType.FourPlayer)
                    return true;
                else
                    return false;
            }
            set
            {
                if (GameType != enGameType.FourPlayer)
                    GameType = enGameType.FourPlayer;
            }
        }

        public double PlayerRBWidth
        {
            get
            {
                return PlayerSelectionPanel.ActualWidth / 3;
            }
        }

        public double PlayerRBHeight
        {
            get
            {
                return PlayerSelectionPanel.ActualWidth / 3;
            }
        }

        public double GameBoardLength
        {
            get
            {
                return Math.Min(BoardColumnWidth, GridRowHeight) - 60.0;
            }
        }

        public double DiceCanvasLength
        {
            get
            {
                return DiceColumnWidth - 60;
            }
        }

        #endregion

        #region Properties for GameBoardSettings

        public int NumberOfSnakes
        {
            get
            {
                return _intNumberOfSnakes;
            }
            set
            {
                _intNumberOfSnakes = value;
            }
        }

        public int NumberOfLadders
        {
            get
            {
                return _intNumberOfLadders;
            }
            set
            {
                _intNumberOfLadders = value;
            }
        }

        public Brush SnakeTailBoxColor
        {
            get
            { return _SnakeTailBoxColor; }
            set
            {
                _SnakeTailBoxColor = value;
                GameBoard.SetBoxColor(_snakeNumbers.Values, _SnakeTailBoxColor);
            }
        }

        public Brush SnakeHeadBoxColor
        {
            get
            { return _SnakeHeadBoxColor; }
            set
            {
                _SnakeHeadBoxColor = value;
                GameBoard.SetBoxColor(_snakeNumbers.Keys, _SnakeHeadBoxColor);
            }
        }

        public Brush LadderStartBoxColor
        {
            get
            { return _LadderStartBoxColor; }
            set
            {
                _LadderStartBoxColor = value;
                GameBoard.SetBoxColor(_ladderNumbers.Keys, _LadderStartBoxColor);
            }
        }

        public Brush LadderEndBoxColor
        {
            get
            { return _LadderEndBoxColor; }
            set
            {
                _LadderEndBoxColor = value;
                GameBoard.SetBoxColor(_ladderNumbers.Values, _LadderEndBoxColor);
            }
        }

        public Brush SnakeColor
        {
            get
            { return _SnakeColor; }
            set
            {
                _SnakeColor = value;
                if (_snakes.Count > 0)
                {
                    foreach (Snake snake in _snakes)
                    {
                        snake.SnakeColor = value;
                    }
                }
            }
        }

        public Brush LadderColor
        {
            get
            { return _LadderColor; }
            set
            {
                _LadderColor = value;
                if (_ladders.Count > 0)
                {
                    foreach (Ladder ladder in _ladders)
                    {
                        ladder.LadderColor = value;
                    }
                }
            }
        }

        public bool CanSnakeLadderIntersect
        {
            get
            {
                return _bCanSnakesAndLaddersIntersect;
            }
            set
            {
                _bCanSnakesAndLaddersIntersect = value;
            }
        }

        public int MaxLadderLength
        {
            get
            {
                return _intMaxLadderLength;
            }
            set
            {
                _intMaxLadderLength = value;
            }
        }

        public int MaxSnakeLength
        {
            get
            {
                return _intMaxSnakeLength;
            }
            set
            {
                _intMaxSnakeLength = value;
            }
        }
        public double SnakeThicknessFactor
        {
            get { return _dbSnakeThicknessFactor; }
            set
            {
                _dbSnakeThicknessFactor = value;
                ResizeSnakes(true);
            }
        }
        #endregion

        #region Window Event Subscriber Methods

        /// <summary>
        /// This method is called when the window is loaded successfully.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            RefreshView();
            //Create the boxes from 1 to 100.
            GameBoard.fillNumbers();
            GameType = enGameType.TwoPlayer;        //Initially the gametype selected as two player.
            DiceCanvas.IsEnabled = false;
        }

        /// <summary>
        /// This method is called when the size of mainwindow changes.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            RefreshView();
            Dispatcher.BeginInvoke(new Action(() =>
            {
                OnPropertyChanged("PlayerRBWidth");
                OnPropertyChanged("PlayerRBHeight");
                double constant1 = GameBoard.ActualWidth;
                GameBoard.OnSizeChanged(constant1);
                GameDice.InitializeDice();
                GameDice.SetFace(currentFace);
                //StartGameFontSize = StartGameButton.ActualWidth / 6;
                ResizeLadders();
                ResizeSnakes();
                ResizeTokens();
            }));
        }

        /// <summary>
        /// This method is called when someone closes the window. We are using this method to clear all the resources.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Closing(object sender, CancelEventArgs e)
        {
            if (_Handlers != null)
                _Handlers = new Collection<PropertyChangedEventHandler>();

            if (_ladderNumbers != null)
                _ladderNumbers.Clear();

            if (_snakeNumbers != null)
                _snakeNumbers.Clear();

            if (_ladders != null)
                _ladders.Clear();

            if (_snakes != null)
                _snakes.Clear();

            if (_tokens != null)
                _tokens.Clear();

            if (GameDice != null && GameDice.DiceAnimation != null)
                GameDice.DiceAnimation.Completed -= DiceAnimation_Completed;

            _winText = null;
            _LadderSnakeRandom = null;

            Properties.Settings.Default.Save();
        }

        #endregion

        #region Dice Event Subcriber Methods

        /// <summary>
        /// This method is called when we click on the dice canvas.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Dice_Click(object sender, RoutedEventArgs e)
        {
            bIsDiceClicked = true;
            DiceCanvas.IsEnabled = false;
            //GameDice.SetFace(currentFace, 100);
            GameDice.EndAnimation();
            //We roll the dice for some time.
            GameDice.StartAnimation(_dbDiceAnimationTime);
        }

        /// <summary>
        /// This method is called when the rolling of dice is completed. So, we can set a new random face to the dice.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DiceAnimation_Completed(object sender, EventArgs e)
        {
            if (bIsDiceClicked == true)
            {
                bIsDiceClicked = false;
                
                Random diceRandom = new Random();

                currentFace = (DiceFace)diceRandom.Next(1, 7);
                GameDice.SetFace(currentFace);

                int nextPos = Tokens[(int)_currentToken].CurrentPosition;
                if ((Tokens[(int)_currentToken].CanMove == false && (int)currentFace == 6))
                {
                    Tokens[(int)_currentToken].CanMove = true;
                    bIsTokenMoved = true;
                }
                else if (Tokens[(int)_currentToken].CanMove == true)
                {
                    if (nextPos + (int)currentFace > 100)
                    {
                        bIsTokenMoved = false;
                    }
                    else
                    {
                        nextPos = nextPos + (int)currentFace;
                        bIsTokenMoved = true;
                    }
                }

                Point dest;
                if (nextPos == 0)
                    dest = Tokens[(int)_currentToken].StartPoint;
                else
                    dest = GameBoard.GetCenterCoordinates(nextPos);

                Tokens[(int)_currentToken].CurrentPosition = nextPos;
                Tokens[(int)_currentToken].MoveTo(dest.X, dest.Y);
            }
        }

        #endregion

        #region Button Event Subscribers

        /// <summary>
        /// This method gets called when Create Board button is clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CreateBoardButton_Click(object sender, RoutedEventArgs e)
        {
            (new System.Media.SoundPlayer(Properties.Resources.Click)).Play();
            GameBoard.Reset();
            AddLaddersOnStartGame();
            AddSnakesOnStartGame();
            AddTokens();
            CurrentToken = enGameToken.Green;
            StartGameButton.IsEnabled = true;
            PlayerSelectionPanel.IsEnabled = true;
        }

        /// <summary>
        /// This method gets called when start game button is clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void StartGameButton_Click(object sender, RoutedEventArgs e)
        {
            (new System.Media.SoundPlayer(Properties.Resources.Click)).Play();
            CurrentToken = enGameToken.Green;
            _numOfPlayersLeft = (int)_enGameType;

            CreateBoardButton.IsEnabled = false;
            DiceCanvas.IsEnabled = true;
            StartGameButton.IsEnabled = false;
            StopGameButton.IsEnabled = true;
            PlayerSelectionPanel.IsEnabled = false;
        }

        /// <summary>
        /// This method gets called when Stop Game button is clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void StopGameButton_Click(object sender, RoutedEventArgs e)
        {
            (new System.Media.SoundPlayer(Properties.Resources.Click)).Play();
            CurrentToken = enGameToken.Green;
            _numOfPlayersLeft = (int)_enGameType;
            foreach (Token token in _tokens)
            {
                token.Reset();
            }

            CreateBoardButton.IsEnabled = true;
            StartGameButton.IsEnabled = true;
            StopGameButton.IsEnabled = false;
            PlayerSelectionPanel.IsEnabled = true;
            DiceCanvas.IsEnabled = false;

        }

        /// <summary>
        /// This method gets called when Game Rules button is clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RulesButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(this, _GameRulesText, "Rules of Snakes and Ladders", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void BoardSettingButton_Click(object sender, RoutedEventArgs e)
        {
            GameBoardSettings gameBoardSettings = new GameBoardSettings(this);
            gameBoardSettings.ShowDialog();
        }

        #endregion

        #region Methods

        /// <summary>
        /// PropertyChanged handler to send call to all the subscribers.
        /// </summary>
        /// <param name="_strProperty">PropertyName to be included in PropertyChangedEventArgs</param>
        public void OnPropertyChanged(string _strProperty)
        {
            if (_Handlers != null && _Handlers.Count != 0)
            {
                for (int i = 0; i < _Handlers.Count; i++)
                {
                    _Handlers[i].Invoke(this, new PropertyChangedEventArgs(_strProperty));
                }
            }
        }

        public void LoadSettings()
        {
            _SnakeTailBoxColor = Properties.Settings.Default.SnakeTailBoxColor.ConvertToWindowsBrush();
            _SnakeHeadBoxColor = Properties.Settings.Default.SnakeHeadBoxColor.ConvertToWindowsBrush();
            _LadderEndBoxColor = Properties.Settings.Default.LadderEndBoxColor.ConvertToWindowsBrush();
            _LadderStartBoxColor = Properties.Settings.Default.LadderStartBoxColor.ConvertToWindowsBrush();
            _SnakeColor = Properties.Settings.Default.SnakeColor.ConvertToWindowsBrush();
            _LadderColor = Properties.Settings.Default.LadderColor.ConvertToWindowsBrush();

            _intMaxLadderLength = Properties.Settings.Default.MaxLadderLength;
            _intMaxSnakeLength = Properties.Settings.Default.MaxSnakeLength;
            _dbSnakeThicknessFactor = Properties.Settings.Default.SnakeThicknessFactor;
            _intNumberOfSnakes = Properties.Settings.Default.NumberOfSnakes;
            _intNumberOfLadders = Properties.Settings.Default.NumberOfLadders;
            _bCanSnakesAndLaddersIntersect = Properties.Settings.Default.CanSnakesAndLaddersIntersect;

        }

        /// <summary>
        /// This method is called when the movement of token is completed. So that we can set the final values. Or decide if the token needs another movement.
        /// </summary>
        public void OnTokenAnimationComplete()
        {
            //If the user wins the game. Start the win animation.
            if (Tokens[(int)_currentToken].CurrentPosition == 100)
            {
                (new System.Media.SoundPlayer(Properties.Resources.WinAudio)).Play();
                DoWinAmination();
                _numOfPlayersLeft--;
                return;
            }

            //If the user reaches the snake head. Then we will move the user to the tail.
            if (_snakeNumbers.ContainsKey(Tokens[(int)_currentToken].CurrentPosition))
            {
                Point newDest = GameBoard.GetCenterCoordinates(_snakeNumbers[Tokens[(int)_currentToken].CurrentPosition]);
                Tokens[(int)_currentToken].MoveToAlongPath(newDest, ref _snakes[_snakeNumbers.GetIndexOfKey(Tokens[(int)_currentToken].CurrentPosition)].SnakePath);
                Tokens[(int)_currentToken].CurrentPosition = _snakeNumbers[Tokens[(int)_currentToken].CurrentPosition];
                return;
            }
            //If the user reaches the lower end of ladder, then we will move the user to the upper end of the ladder.
            else if (_ladderNumbers.ContainsKey(Tokens[(int)_currentToken].CurrentPosition))
            {
                Point newDest = GameBoard.GetCenterCoordinates(_ladderNumbers[Tokens[(int)_currentToken].CurrentPosition]);
                Tokens[(int)_currentToken].MoveTo(newDest.X, newDest.Y, true);
                Tokens[(int)_currentToken].CurrentPosition = _ladderNumbers[Tokens[(int)_currentToken].CurrentPosition];
                return;
            }

            //Conditions to check if the chance should be transferred to the next user.
            if (currentFace != DiceFace.Six || (bIsTokenMoved == false && currentFace == DiceFace.Six && Tokens[(int)_currentToken].CurrentPosition + (int)currentFace > 100))
            {
                do
                {
                    CurrentToken = CurrentToken.Next(_enGameType);
                }
                while (Tokens[(int)_currentToken].CurrentPosition == 100);
            }
            DiceCanvas.IsEnabled = true;

            //if (RestartGameButton.IsEnabled == false)
            //    RestartGameButton.IsEnabled = true;
        }

        /// <summary>
        /// This method is used to Start the win game animation.
        /// </summary>
        void DoWinAmination()
        {
            (new System.Media.SoundPlayer(Properties.Resources.WinAudio)).Play();

            GameBoard.Opacity = 0.4;
            foreach (Ladder ladder in _ladders)
            {
                ladder.Opacity = 0.4;
            }
            foreach (Snake snake in _snakes)
            {
                snake.Opacity = 0.4;
            }

            (new System.Media.SoundPlayer(Properties.Resources.Applause)).Play();
            _winText = new WinText(_currentToken);
            _winText.CanvasWidth = GameBoard.ActualWidth;
            if (_numOfPlayersLeft == (int)_enGameType)
                _winText.Text = _currentToken.ToString() + " won the game!!!";
            else if (_numOfPlayersLeft == (int)_enGameType - 1 && _numOfPlayersLeft > 1)
                _winText.Text = "Second winner is " + _currentToken.ToString() + "!!!";
            else if (_numOfPlayersLeft == (int)_enGameType - 2 && _numOfPlayersLeft > 1)
                _winText.Text = "Third winner is " + _currentToken.ToString() + "!!!";

            _winText.WinAnimation.Completed += WinAnimation_Completed;
            Canvas.SetLeft(_winText, (GameBoard.ActualWidth / 2) - (GameBoard.ActualWidth / 5));
            Canvas.SetTop(_winText, (GameBoard.ActualHeight / 2) - (GameBoard.ActualHeight / 15));
            BoardCanvas.Children.Add(_winText);

            _winText.StartAnimation();
        }

        /// <summary>
        /// This method is called when the wingame animation is completed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WinAnimation_Completed(object sender, EventArgs e)
        {
            BoardCanvas.Children.Remove(_winText);

            //Resotre the opacity of the board.
            GameBoard.Opacity = 1;
            foreach (Ladder ladder in _ladders)
            {
                ladder.Opacity = 1;
            }
            foreach (Snake snake in _snakes)
            {
                snake.Opacity = 1;
            }

            //If the number of players left is more than 1, then the game continues.
            if (_numOfPlayersLeft > 1)
            {
                do
                {
                    CurrentToken = CurrentToken.Next(_enGameType);
                }
                while (Tokens[(int)_currentToken].CurrentPosition == 100);
                DiceCanvas.IsEnabled = true;
            }
            //Else we will enable the start game button and we dont enable the dice canvas.
            else
            {
                CreateBoardButton.IsEnabled = true;
                StartGameButton.IsEnabled = true;
                StopGameButton.IsEnabled = false;
                PlayerSelectionPanel.IsEnabled = true;
            }
        }

        /// <summary>
        /// This method is used to add the ladder on the board. This method doesn't check the start and end point rules.
        /// It will assume that the start and end points are already checked for possible errors and rules.
        /// This method will check only the intersection of new ladder with other already drawn ladders.
        /// </summary>
        /// <param name="start">Start point from which the ladder will start.</param>
        /// <param name="end">End point upto which the ladder will go.</param>
        /// <param name="checkIsIntersecting">If this argument is true, then before adding the ladder, it will be checked for intersection with other ladders. 
        /// Else, it will be directly added to the board.</param>
        /// <returns>True if the ladder is added to the board, else false.</returns>
        public bool AddLadder(Point start, Point end, bool checkIsIntersecting = true)
        {
            Ladder ladder = new Ladder();
            ladder.LadderWidth = Math.Sqrt(GameBoard.Boxes[1].BoxTextBlock.ActualWidth * GameBoard.Boxes[1].BoxTextBlock.ActualWidth + GameBoard.Boxes[1].BoxTextBlock.ActualHeight * GameBoard.Boxes[1].BoxTextBlock.ActualHeight);
            ladder.StepsDifference = GameBoard.ActualWidth / _dbStepDifferenceFactor;
            ladder.LineThickness = GameBoard.ActualWidth / _dbLadderLineThicknessFactor;
            ladder.CanvasHeight = GameBoard.ActualHeight;
            ladder.CanvasWidth = GameBoard.ActualWidth;
            ladder.DrawLadder(start, end);

            if (!checkIsIntersecting || !IsIntersecting(ladder))
            {
                BoardCanvas.Children.Add(ladder);
                _ladders.Add(ladder);
                return true;
            }
            return false;
        }

        /// <summary>
        /// This method is used to add the snake on the board. This method doesn't check the start and end point rules.
        /// It will assume that the start and end points are already checked for possible errors and rules.
        /// This method will check only the intersection of new snake with other already drawn snakes.
        /// </summary>
        /// <param name="start">Start point from which the snake will start.</param>
        /// <param name="end">End point upto which the snake will go.</param>
        /// <param name="checkIsIntersecting">If this argument is true, then before adding the snake, it will be checked for intersection with other snakes. 
        /// Else, it will be directly added to the board.</param>
        /// <returns>True if the snake is added to the board, else false.</returns>
        public bool AddSnake(Point start, Point end, bool checkIsIntersecting = true)
        {
            Snake snake = new Snake();
            snake.SnakeWidth = Math.Sqrt(GameBoard.Boxes[1].BoxTextBlock.ActualWidth * GameBoard.Boxes[1].BoxTextBlock.ActualWidth + GameBoard.Boxes[1].BoxTextBlock.ActualHeight * GameBoard.Boxes[1].BoxTextBlock.ActualHeight);
            snake.LineStrokeThickness = GameBoard.ActualWidth / _dbSnakeThicknessFactor;
            snake.CanvasHeight = GameBoard.ActualHeight;
            snake.CanvasWidth = GameBoard.ActualWidth;
            snake.StartPoint = start;
            snake.EndPoint = end;

            if (!checkIsIntersecting || !IsIntersecting(snake))
            {
                snake.DrawCurve();
                _snakes.Add(snake);
                BoardCanvas.AddSnake(snake);
                return true;
            }
            return false;
        }

        /// <summary>
        /// This method is used to check the intersection of the snake with already drawn snakes.
        /// If _bCanSnakesAndLaddersIntersect is false, then it checks the intersection with already drawn ladders too.
        /// </summary>
        /// <param name="iSnake">The snake whose intersection is to be checked.</param>
        /// <returns>true if its intersecting with other snakes and ladders else false.</returns>
        private bool IsIntersecting(Snake iSnake)
        {
            bool bIsIntersecting = false;
            foreach (Snake snake in _snakes)
            {
                bIsIntersecting = snake.IsIntersecting(iSnake);
                if (bIsIntersecting == true)
                    break;

            }

            if (bIsIntersecting == false && _bCanSnakesAndLaddersIntersect == false)
            {
                foreach (Ladder ladder in _ladders)
                {
                    bIsIntersecting = ladder.IsIntersecting(iSnake);
                    if (bIsIntersecting == true)
                        break;
                }
            }

            return bIsIntersecting;
        }

        /// <summary>
        /// This method is used to check the intersection of the ladder with already drawn ladders.
        /// If _bCanSnakesAndLaddersIntersect is false, then it checks the intersection with already drawn snakes too.
        /// </summary>
        /// <param name="iladder">The ladder whose intersection is to be checked.</param>
        /// <returns>true if its intersecting with other snakes and ladders else false.</returns>
        private bool IsIntersecting(Ladder iladder)
        {
            bool bIsIntersecting = false;
            foreach (Ladder ladder in _ladders)
            {
                bIsIntersecting = ladder.IsIntersecting(iladder);
                if (bIsIntersecting == true)
                    break;
            }

            if (bIsIntersecting == false && _bCanSnakesAndLaddersIntersect == false)
            {
                foreach (Snake snake in _snakes)
                {
                    bIsIntersecting = snake.IsIntersecting(iladder);
                    if (bIsIntersecting == true)
                        break;
                }
            }

            return bIsIntersecting;
        }

        /// <summary>
        /// This method resizes the ladders according to the new width and height of the board.
        /// </summary>
        private void ResizeLadders()
        {
            if (_ladders.Count > 0)
            {
                foreach (Ladder ladder in _ladders)
                {
                    ladder.StepsDifference = GameBoard.ActualWidth / _dbStepDifferenceFactor;
                    ladder.ResizeLadder(GameBoard.ActualWidth, GameBoard.ActualHeight);
                    ladder.LineThickness = GameBoard.ActualWidth / _dbLadderLineThicknessFactor;
                }
            }
        }

        /// <summary>
        /// This method resizes the snakes according to the new width and height of the board.
        /// </summary>
        private void ResizeSnakes(bool RedrawTail = false)
        {
            if (_snakes.Count > 0)
            {
                foreach (Snake snake in _snakes)
                {
                    snake.LineStrokeThickness = GameBoard.ActualWidth / _dbSnakeThicknessFactor;
                    snake.ResizeSnake(GameBoard.ActualWidth, GameBoard.ActualHeight, RedrawTail);
                }
            }
        }

        /// <summary>
        /// This method resizes the tokens according to the new width and height of the board.
        /// </summary>
        private void ResizeTokens()
        {
            foreach (Token token in _tokens)
            {
                token.ResizeToken(BoardCanvas.ActualWidth, BoardCanvas.ActualHeight);
            }
        }

        /// <summary>
        /// Adds all the ladders on the board. This method should be called when creating a new board.
        /// This method clears any previously drawn ladders from the canvas.
        /// </summary>
        private void AddLaddersOnStartGame()
        {
            _ladderNumbers.Clear();
            if (_ladders.Count > 0)
            {
                foreach (Ladder ladder in _ladders)
                {
                    BoardCanvas.Children.Remove(ladder);
                }
                _ladders.Clear();
            }

            Random _LadderSnakeRandom = new Random();
            int numberofLadders = _intNumberOfLadders;
            for (int i = 0; i < numberofLadders; i++)
            {
                int startNumber, endNumber;
                startNumber = _LadderSnakeRandom.Next(1, 100);
                endNumber = _LadderSnakeRandom.Next(1, 100);
                if (endNumber < startNumber)
                {
                    int x = startNumber;
                    startNumber = endNumber;
                    endNumber = x;
                }

                Point start = GameBoard.GetCenterCoordinates(startNumber);
                Point end = GameBoard.GetCenterCoordinates(endNumber);

                if (_ladderNumbers.ContainsKey(startNumber) ||
                    _ladderNumbers.Values.Contains(startNumber) ||
                    _ladderNumbers.ContainsKey(endNumber) ||
                    _ladderNumbers.Values.Contains(endNumber) ||
                    endNumber - startNumber > _intMaxLadderLength ||
                    start.X - end.X == 0 ||
                    start.Y - end.Y == 0)
                {
                    i--;
                    continue;
                }

                if (!AddLadder(start, end))
                {
                    i--;
                    continue;
                }
                _ladderNumbers.Add(startNumber, endNumber);
            }
            GameBoard.SetBoxColor(_ladderNumbers.Keys, _LadderStartBoxColor);
            GameBoard.SetBoxColor(_ladderNumbers.Values, _LadderEndBoxColor);
        }

        /// <summary>
        /// Adds all the snakes on the board. This method should be called when creating a new board.
        /// This method clears any previously drawn snakes from the canvas.
        /// </summary>
        private void AddSnakesOnStartGame()
        {
            _snakeNumbers.Clear();
            if (_snakes.Count > 0)
            {
                foreach (Snake snake in _snakes)
                {
                    BoardCanvas.RemoveSnake(snake);
                }
                _snakes.Clear();
            }

            Random _LadderSnakeRandom = new Random();
            int numberofSnakes = _intNumberOfSnakes;
            for (int i = 0; i < numberofSnakes; i++)
            {
                int startNumber, endNumber;
                startNumber = _LadderSnakeRandom.Next(1, 100);
                endNumber = _LadderSnakeRandom.Next(1, 100);
                if (endNumber > startNumber)
                {
                    int x = startNumber;
                    startNumber = endNumber;
                    endNumber = x;
                }

                Point start = GameBoard.GetCenterCoordinates(startNumber);
                Point end = GameBoard.GetCenterCoordinates(endNumber);

                if (_ladderNumbers.ContainsKey(startNumber) ||
                    _ladderNumbers.Values.Contains(startNumber) ||
                    _ladderNumbers.ContainsKey(endNumber) ||
                    _ladderNumbers.Values.Contains(endNumber) ||
                    _snakeNumbers.ContainsKey(startNumber) ||
                    _snakeNumbers.Values.Contains(startNumber) ||
                    _snakeNumbers.ContainsKey(endNumber) ||
                    _snakeNumbers.Values.Contains(endNumber) ||
                    startNumber - endNumber > _intMaxSnakeLength ||
                    start.X - end.X == 0 ||
                    start.Y - end.Y == 0)
                {
                    i--;
                    continue;
                }

                if (!AddSnake(start, end))
                {
                    i--;
                    continue;
                }
                _snakeNumbers.Add(startNumber, endNumber);
            }
            GameBoard.SetBoxColor(_snakeNumbers.Keys, _SnakeHeadBoxColor);
            GameBoard.SetBoxColor(_snakeNumbers.Values, _SnakeTailBoxColor);
        }

        /// <summary>
        /// Adds all the tokens on the board. 
        /// This method clears any previously drawn tokens from the canvas.
        /// </summary>
        private void AddTokens()
        {
            foreach (Token token in _tokens)
            {
                BoardCanvas.RemoveToken(token);
            }
            _tokens.Clear();

            Point p1 = new Point(0, BoardCanvas.ActualHeight);
            Point p2 = new Point(0, BoardCanvas.ActualHeight - 15);
            Point p3 = new Point(0, BoardCanvas.ActualHeight - 30);
            Point p4 = new Point(0, BoardCanvas.ActualHeight - 45);
            Point[] pts = new Point[] { p1, p2, p3, p4 };

            enGameToken gametoken = enGameToken.Green;
            for (int i = 0; i < (int)_enGameType; i++)
            {
                Token token = new Token(this, gametoken, pts[i], BoardCanvas.ActualWidth, BoardCanvas.ActualHeight);
                gametoken = gametoken.Next(_enGameType);
                _tokens.Add(token);
                BoardCanvas.AddToken(token);
            }
        }

        /// <summary>
        /// Refreshes the view.
        /// </summary>
        private void RefreshView()
        {
            OnPropertyChanged("GridRowHeight");
            OnPropertyChanged("BoardColumnWidth");
            OnPropertyChanged("DiceColumnWidth");
            OnPropertyChanged("PlayerRBWidth");
            OnPropertyChanged("PlayerRBHeight");
            OnPropertyChanged("GameBoardLength");
            OnPropertyChanged("DiceCanvasLength");
            OnPropertyChanged("ButtonTextFontSize");
        }
        #endregion
    }
}
