using Snakes_and_Ladders.Shapes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Snakes_and_Ladders
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DiceFace currentFace;
        Dictionary<int, int> _ladderNumbers;
        Dictionary<int, int> _snakeNumbers;
        List<Ladder> _ladders;
        List<Snake> _snakes;
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

        public MainWindow()
        {
            InitializeComponent();
            currentFace = DiceFace.Three;
            _ladders = new List<Ladder>();
            _snakes = new List<Snake>();
            _ladderNumbers = new Dictionary<int, int>();
            _snakeNumbers = new Dictionary<int, int>();
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
            snake.LineStrokeThickness = GameBoard.ActualWidth / 140;
            snake.CanvasHeight = GameBoard.ActualHeight;
            snake.CanvasWidth = GameBoard.ActualWidth;
            snake.StartPoint = start;
            snake.EndPoint = end;
            if (!checkIsIntersecting || !IsIntersecting(snake))
            {
                snake.DrawCurve();
                _snakes.Add(snake);
                BoardCanvas.Children.Add(snake);
                return true;
            }
            return false;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            GameBoard.fillNumbers();
        }
        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            double constant1 = BoardColumn.ActualWidth;
            //BoardPanel.Width = constant1;
            GameBoard.Height = constant1;
            GameBoard.Width = constant1;
            GameBoard.OnSizeChanged(constant1);
            GameDice.InitializeDice();
            GameDice.SetFace(currentFace);
            DiceCanvas.Width = DicePanel.ActualWidth/2 > 150 ? 150 : DicePanel.ActualWidth / 2;
            DiceCanvas.Height = DicePanel.ActualWidth/2 > 150 ? 150 : DicePanel.ActualWidth / 2;

            DiceCanvas.Margin = new Thickness((DicePanel.ActualWidth - DiceCanvas.Width) / 2, 10, (DicePanel.ActualWidth - DiceCanvas.Width) / 2, 0);
            ResizeLadders();
            ResizeSnakes();
        }

        private void Dice_Click(object sender, RoutedEventArgs e)
        {
            //GameDice.SetFace(currentFace, 100);
            GameDice.EndAnimation();

            GameDice.StartAnimation(400);
            UpdateLayout();
            Random random = new Random();
            currentFace = (DiceFace)random.Next(1, 7);
            GameDice.SetFace(currentFace);
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
                        snake.ResizeSnake(GameBoard.ActualWidth, GameBoard.ActualHeight);
                        snake.LineStrokeThickness = GameBoard.ActualWidth / 140;
                    }
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
            int numberofLadders = random.Next(4, 9);
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
                    BoardCanvas.Children.Remove(snake);
                }
                _snakes.Clear();
            }

            Random random = new Random();
            int numberofSnakes = random.Next(4, 9);
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

        private void StartGameButton_Click(object sender, RoutedEventArgs e)
        {
            GameBoard.Reset();
            AddLaddersOnStartGame();
            System.Threading.Thread.Sleep(200);
            AddSnakesOnStartGame();
            //StartGameButton.IsEnabled = false
        }
        
    }
}
