using Snakes_and_Ladders.Shapes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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
        private double _StartGameFontSize;
        private enGameToken _currentToken;
        private bool bIsDiceClicked = false;
        private WinText _winText;
        private int _numOfPlayersLeft = 0;
        private Random _LadderSnakeRandom;
        private string _GameRulesText;
        private bool bIsTokenMoved = false;
        private bool bIsRulesVisible = true;
        private double _dbLadderLineThicknessFactor = 120.0;
        private double _dbStepDifferenceFactor = 50.0;
        private double _dbDiceAnimationTime = 400;
        private double _dbSnakeThicknessFactor = 40.0;
        private bool _bCanSnakesAndLaddersIntersect = true;
        private int _intMaxLadderLength = 60;
        private int _intMaxSnakeLength = 80;

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

            CurrentToken = enGameToken.Green;
            DataContext = this;
            GameDice.DiceAnimation.Completed += DiceAnimation_Completed;
            _LadderSnakeRandom = new Random();
            RestartBoardButton.IsEnabled = false;
            StopGameButton.IsEnabled = false;

            _GameRulesText = "Rules of the game:\n1. The colored tokens will start moving once you score first six.\n2. The token which reaches 100 first wins the game.\n3. If you reach to the number where snake head is, you will have to go to the snake tail position.\n4. If you reach to the number where ladder starts, you will reach to the higher end of the ladder.";
        }

        ~MainWindow()
        {

        }

        #endregion

        #region Properties
        public double StartGameFontSize
        {
            get
            {
                return _StartGameFontSize;
            }
            set
            {
                _StartGameFontSize = value;
                OnPropertyChanged("StartGameFontSize");
                OnPropertyChanged("GameRulesFontSize");

            }
        }

        public double GameRulesFontSize
        {
            get
            {
                return _StartGameFontSize / 2.0;
            }
        }

        public Visibility IsRulesVisible
        {
            get
            {
                return bIsRulesVisible == true? Visibility.Visible : Visibility.Collapsed;
            }
            set
            {
                bIsRulesVisible = value == Visibility.Visible ? true : false;
                OnPropertyChanged("IsRulesVisible");
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
                if(GameType != enGameType.TwoPlayer)
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
                if(GameType != enGameType.ThreePlayer)
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
                if(GameType != enGameType.FourPlayer)
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
                return BoardColumn.ActualWidth;
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
            double constant1 = BoardColumn.ActualWidth;
            OnPropertyChanged("GameBoardLength");

            GameBoard.OnSizeChanged(constant1);
            GameDice.InitializeDice();
            GameDice.SetFace(currentFace);
            DiceCanvas.Width = DicePanel.ActualWidth / 2 > 150 ? 150 : DicePanel.ActualWidth / 2;
            DiceCanvas.Height = DicePanel.ActualWidth / 2 > 150 ? 150 : DicePanel.ActualWidth / 2;
            StartGameFontSize = StartGameButton.ActualWidth / 6;
            ResizeLadders();
            ResizeSnakes();
            ResizeTokens();
            RefreshView();

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
        /// This method gets called when start game button is clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void StartGameButton_Click(object sender, RoutedEventArgs e)
        {
            (new System.Media.SoundPlayer(Properties.Resources.Click)).Play();
            GameBoard.Reset();
            AddLaddersOnStartGame();
            AddSnakesOnStartGame();
            AddTokens();
            CurrentToken = enGameToken.Green;
            DiceCanvas.IsEnabled = true;
            StartGameButton.IsEnabled = false;
            StopGameButton.IsEnabled = true;
            PlayerSelectionPanel.IsEnabled = false;
            _numOfPlayersLeft = (int)_enGameType;
        }

        /// <summary>
        /// This method gets called when Restart Game button is clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RestartBoardButton_Click(object sender, RoutedEventArgs e)
        {
            (new System.Media.SoundPlayer(Properties.Resources.Click)).Play();
            CurrentToken = enGameToken.Green;
            _numOfPlayersLeft = (int)_enGameType;
            foreach (Token token in _tokens)
            {
                token.Reset();
            }
            DiceCanvas.IsEnabled = true;
            StartGameButton.IsEnabled = false;
            PlayerSelectionPanel.IsEnabled = false;
            RestartBoardButton.IsEnabled = false;
            StopGameButton.IsEnabled = true;
        }

        /// <summary>
        /// This method gets called when Stop Game button is clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void StopGameButton_Click(object sender, RoutedEventArgs e)
        {
            RestartBoardButton_Click(sender, e);
            StartGameButton.IsEnabled = true;
            StopGameButton.IsEnabled = false;
            RestartBoardButton.IsEnabled = true;
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

            if (RestartBoardButton.IsEnabled == false)
                RestartBoardButton.IsEnabled = true;
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
                StartGameButton.IsEnabled = true;
                RestartBoardButton.IsEnabled = true;
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
            Dispatcher.BeginInvoke(new Action(() =>
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
            }));
        }

        /// <summary>
        /// This method resizes the snakes according to the new width and height of the board.
        /// </summary>
        private void ResizeSnakes()
        {
            Dispatcher.BeginInvoke(new Action(() =>
            {
                if (_snakes.Count > 0)
                {
                    foreach (Snake snake in _snakes)
                    {
                        snake.LineStrokeThickness = GameBoard.ActualWidth / _dbSnakeThicknessFactor;
                        snake.ResizeSnake(GameBoard.ActualWidth, GameBoard.ActualHeight);
                    }
                }
            }));
        }

        /// <summary>
        /// This method resizes the tokens according to the new width and height of the board.
        /// </summary>
        private void ResizeTokens()
        {
            Dispatcher.BeginInvoke(new Action(() =>
            {
                foreach (Token token in _tokens)
                {
                    token.ResizeToken(BoardCanvas.ActualWidth, BoardCanvas.ActualHeight);
                }
            }));
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
            int numberofLadders = _LadderSnakeRandom.Next(4, 7);
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
            GameBoard.SetBoxColor(_ladderNumbers.Keys, Brushes.LawnGreen);
            GameBoard.SetBoxColor(_ladderNumbers.Values, Brushes.DarkGreen);
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
            int numberofSnakes = _LadderSnakeRandom.Next(4, 7);
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
            GameBoard.SetBoxColor(_snakeNumbers.Keys, Brushes.DarkRed);
            GameBoard.SetBoxColor(_snakeNumbers.Values, Brushes.OrangeRed);
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
            OnPropertyChanged("PlayerRBWidth");
            OnPropertyChanged("PlayerRBHeight");
        }
        #endregion
    }
}
