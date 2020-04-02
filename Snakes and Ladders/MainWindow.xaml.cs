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
        }

        public bool AddLadder(Point start, Point end)
        {
            Ladder ladder = new Ladder();
            ladder.LadderWidth = Math.Sqrt(GameBoard.Boxes[1].BoxTextBlock.ActualWidth * GameBoard.Boxes[1].BoxTextBlock.ActualWidth + GameBoard.Boxes[1].BoxTextBlock.ActualHeight * GameBoard.Boxes[1].BoxTextBlock.ActualHeight);
            ladder.StepsDifference = GameBoard.ActualWidth / 50;
            ladder.LineThickness = GameBoard.ActualWidth / 145;
            ladder.DrawLadder(start, end);
            //SnLUtility.Path path = SnLUtility.GenerateRandomPath((int)start.X, (int)start.Y, (int)end.X, (int)end.Y, 0.3);

            if (!IsIntersecting(ladder))
            {
                BoardCanvas.Children.Add(ladder);
                _ladders.Add(ladder);
                Snake snake = new Snake();
                snake.DrawCurve(start, end);
                _snakes.Add(snake);
                BoardCanvas.Children.Add(snake.SnakePath);

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
            if (_ladders.Count > 0)
            {
                foreach (Ladder ladder in _ladders)
                {
                    BoardCanvas.Children.Remove(ladder);
                }
                _ladders.Clear();
            }

            Dispatcher.BeginInvoke(new Action(() =>
            {
                foreach (KeyValuePair<int, int> nums_it in _ladderNumbers)
                {
                    Point start = GameBoard.GetCenterCoordinates(nums_it.Key);
                    Point end = GameBoard.GetCenterCoordinates(nums_it.Value);
                    AddLadder(start, end);
                }
            }));
            
        }

        private void StartGameButton_Click(object sender, RoutedEventArgs e)
        {
            GameBoard.Reset();
            _ladderNumbers.Clear();
            if (_ladders.Count > 0)
            {
                foreach(Ladder ladder in _ladders)
                {
                    BoardCanvas.Children.Remove(ladder);
                }
                _ladders.Clear();
            }

            if (_snakes.Count > 0)
            {
                foreach (Snake snake in _snakes)
                {
                    BoardCanvas.Children.Remove(snake.SnakePath);
                }
                _snakes.Clear();
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
            //StartGameButton.IsEnabled = false
        }
    }
}
