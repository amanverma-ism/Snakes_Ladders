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
        Ladder ladder;
        public MainWindow()
        {
            InitializeComponent();
            currentFace = DiceFace.Three;
        }

        public void AddLadder(Point start, Point end)
        {
            ladder = new Ladder();
            ladder.LadderWidth = Math.Sqrt(GameBoard.Boxes[0].boxTextBlock.ActualWidth * GameBoard.Boxes[0].boxTextBlock.ActualWidth + GameBoard.Boxes[0].boxTextBlock.ActualHeight * GameBoard.Boxes[0].boxTextBlock.ActualHeight);
            ladder.StepsDifference = GameBoard.ActualWidth / 50;
            ladder.DrawLadder(start, end);
            BoardCanvas.Children.Add(ladder);
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

        private void StartGameButton_Click(object sender, RoutedEventArgs e)
        {
            Random random = new Random();
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
            AddLadder(start, end);
            StartGameButton.IsEnabled = false;
        }
    }
}
