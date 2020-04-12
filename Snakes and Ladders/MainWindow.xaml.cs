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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private Collection<PropertyChangedEventHandler> _Handlers = new Collection<PropertyChangedEventHandler>();
        DiceFace currentFace;
        Dictionary<int, int> _ladderNumbers;
        Dictionary<int, int> _snakeNumbers;
        List<Ladder> _ladders;
        List<Snake> _snakes;
        List<Token> _tokens;
        private enGameType _enGameType;
        private double _StartGameFontSize;
        private enGameToken _currentToken;
        private bool bIsDiceClicked = false;

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
        public MainWindow()
        {
            InitializeComponent();
            currentFace = DiceFace.Three;
            _ladders = new List<Ladder>();
            _snakes = new List<Snake>();
            _tokens = new List<Token>();
            _ladderNumbers = new Dictionary<int, int>();
            _snakeNumbers = new Dictionary<int, int>();
            CurrentToken = enGameToken.Green;
            DataContext = this;
            GameDice.DiceAnimation.Completed += DiceAnimation_Completed;
        }
        
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

        public void OnTokenAnimationComplete()
        {
            if (currentFace != DiceFace.Six)
            {
                if(_snakeNumbers.ContainsKey(Tokens[(int)_currentToken].CurrentPosition))
                {
                    Point newDest = GameBoard.GetCenterCoordinates(_snakeNumbers[Tokens[(int)_currentToken].CurrentPosition]);
                    Tokens[(int)_currentToken].MoveToAlongPath(newDest, ref _snakes[_snakeNumbers.GetIndexOfKey(Tokens[(int)_currentToken].CurrentPosition)].SnakePath);
                    Tokens[(int)_currentToken].CurrentPosition = _snakeNumbers[Tokens[(int)_currentToken].CurrentPosition];
                    return;
                }
                else if(_ladderNumbers.ContainsKey(Tokens[(int)_currentToken].CurrentPosition))
                {
                    Point newDest = GameBoard.GetCenterCoordinates(_ladderNumbers[Tokens[(int)_currentToken].CurrentPosition]);
                    Tokens[(int)_currentToken].MoveTo(newDest.X, newDest.Y);
                    Tokens[(int)_currentToken].CurrentPosition = _ladderNumbers[Tokens[(int)_currentToken].CurrentPosition];
                    return;
                }
                CurrentToken = CurrentToken.Next(_enGameType);
            }
            DiceCanvas.IsEnabled = true;
        }

        public bool AddLadder(Point start, Point end, bool checkIsIntersecting = true)
        {
            Ladder ladder = new Ladder();
            ladder.LadderWidth = Math.Sqrt(GameBoard.Boxes[1].BoxTextBlock.ActualWidth * GameBoard.Boxes[1].BoxTextBlock.ActualWidth + GameBoard.Boxes[1].BoxTextBlock.ActualHeight * GameBoard.Boxes[1].BoxTextBlock.ActualHeight);
            ladder.StepsDifference = GameBoard.ActualWidth / 50;
            ladder.LineThickness = GameBoard.ActualWidth / 145;
            ladder.CanvasHeight = GameBoard.ActualHeight;
            ladder.CanvasWidth = GameBoard.ActualWidth;
            ladder.DrawLadder(start, end);
            //SnLUtility.Path path = SnLUtility.GenerateRandomPath((int)start.X, (int)start.Y, (int)end.X, (int)end.Y, 0.3);
            if (!checkIsIntersecting || !IsIntersecting(ladder))
            {
                BoardCanvas.Children.Add(ladder);
                _ladders.Add(ladder);
                return true;
            }
            return false;
        }

        public bool AddSnake(Point start, Point end, bool checkIsIntersecting = true)
        {
            Snake snake = new Snake();
            snake.SnakeWidth = Math.Sqrt(GameBoard.Boxes[1].BoxTextBlock.ActualWidth * GameBoard.Boxes[1].BoxTextBlock.ActualWidth + GameBoard.Boxes[1].BoxTextBlock.ActualHeight * GameBoard.Boxes[1].BoxTextBlock.ActualHeight);
            snake.LineStrokeThickness = GameBoard.ActualWidth / 60;
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

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            GameBoard.fillNumbers();
            GameType = enGameType.TwoPlayer;
            DiceCanvas.IsEnabled = false;
        }
        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            double constant1 = BoardColumn.ActualWidth;
            OnPropertyChanged("GameBoardLength");
            //BoardPanel.Width = constant1;
            //BoardCanvas.Height = constant1;
            //BoardCanvas.Width = constant1;
            //GameBoard.Height = constant1;
            //GameBoard.Width = constant1;
            GameBoard.OnSizeChanged(constant1);
            GameDice.InitializeDice();
            GameDice.SetFace(currentFace);
            DiceCanvas.Width = DicePanel.ActualWidth/2 > 150 ? 150 : DicePanel.ActualWidth / 2;
            DiceCanvas.Height = DicePanel.ActualWidth/2 > 150 ? 150 : DicePanel.ActualWidth / 2;
            StartGameFontSize = StartGameButton.ActualWidth / 6;
            //DiceCanvas.Margin = new Thickness((DicePanel.ActualWidth - DiceCanvas.Width) / 2, 10, (DicePanel.ActualWidth - DiceCanvas.Width) / 2, 0);

            ResizeLadders();
            ResizeSnakes();
            ResizeTokens();
            RefreshView();
        }

        private void Dice_Click(object sender, RoutedEventArgs e)
        {
            bIsDiceClicked = true;
            DiceCanvas.IsEnabled = false;
            //GameDice.SetFace(currentFace, 100);
            GameDice.EndAnimation();

            GameDice.StartAnimation(400);
        }

        private void DiceAnimation_Completed(object sender, EventArgs e)
        {
            if (bIsDiceClicked == true)
            {
                bIsDiceClicked = false;
                Random random = new Random();
                currentFace = (DiceFace)random.Next(1, 7);
                GameDice.SetFace(currentFace);

                int nextPos = Tokens[(int)_currentToken].CurrentPosition;
                nextPos = nextPos + (int)currentFace > 100 ? nextPos : nextPos + (int)currentFace;

                Point dest = GameBoard.GetCenterCoordinates(nextPos);

                Tokens[(int)_currentToken].CurrentPosition = nextPos;
                Tokens[(int)_currentToken].MoveTo(dest.X, dest.Y);
            }
        }

        private bool IsIntersecting(Snake iSnake)
        {
            bool bIsIntersecting = false;
            foreach (Snake snake in _snakes)
            {
                bIsIntersecting = snake.IsIntersecting(iSnake);
                if (bIsIntersecting == true)
                    break;
            }


            return bIsIntersecting;
        }

        private bool IsIntersecting(Ladder iladder)
        {
            bool bIsIntersecting = false;
            foreach(Ladder ladder in _ladders)
            {
                bIsIntersecting = ladder.IsIntersecting(iladder);
                if (bIsIntersecting == true)
                    break;
            }


            return bIsIntersecting;
        }

        private void ResizeLadders()
        {
            Dispatcher.BeginInvoke(new Action(() =>
            {
                if (_ladders.Count > 0)
                {
                    foreach (Ladder ladder in _ladders)
                    {
                        ladder.StepsDifference = GameBoard.ActualWidth / 50;
                        ladder.ResizeLadder(GameBoard.ActualWidth, GameBoard.ActualHeight);
                        ladder.LineThickness = GameBoard.ActualWidth / 145;
                    }
                }
            }));
        }

        private void ResizeSnakes()
        {
            Dispatcher.BeginInvoke(new Action(() =>
            {
                if (_snakes.Count > 0)
                {
                    foreach (Snake snake in _snakes)
                    {
                        snake.LineStrokeThickness = GameBoard.ActualWidth / 60;
                        snake.ResizeSnake(GameBoard.ActualWidth, GameBoard.ActualHeight);
                    }
                }
            }));
        }

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

            Random random = new Random();
            int numberofLadders = random.Next(4, 7);
            for (int i = 0; i < numberofLadders; i++)
            {
                int startNumber, endNumber;
                startNumber = random.Next(1, 100);
                endNumber = random.Next(1, 100);
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
                    endNumber - startNumber > 50 ||
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
            GameBoard.SetBoxColor(_ladderNumbers.Keys.Concat(_ladderNumbers.Values), Brushes.DarkGreen);
        }

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

            Random random = new Random();
            int numberofSnakes = random.Next(4, 7);
            for (int i = 0; i < numberofSnakes; i++)
            {
                int startNumber, endNumber;
                startNumber = random.Next(1, 100);
                endNumber = random.Next(1, 100);
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
                    startNumber - endNumber > 50 ||
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
            GameBoard.SetBoxColor(_snakeNumbers.Keys.Concat(_snakeNumbers.Values), Brushes.DarkRed);
        }

        private void AddTokens()
        {
            foreach(Token token in _tokens)
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
            for(int i = 0; i<(int)_enGameType; i++)
            {
                Token token = new Token(this, gametoken, pts[i], BoardCanvas.ActualWidth, BoardCanvas.ActualHeight);
                gametoken = gametoken.Next(_enGameType);
                _tokens.Add(token);
                BoardCanvas.AddToken(token);
            }
        }

        private void StartGameButton_Click(object sender, RoutedEventArgs e)
        {
            GameBoard.Reset();
            AddLaddersOnStartGame();
            System.Threading.Thread.Sleep(200);
            AddSnakesOnStartGame();
            AddTokens();
            DiceCanvas.IsEnabled = true;
            StartGameButton.IsEnabled = false;
            PlayerSelectionPanel.IsEnabled = false;
        }

        private void RefreshView()
        {
            OnPropertyChanged("PlayerRBWidth");
            OnPropertyChanged("PlayerRBHeight");
        }
        
    }
}
