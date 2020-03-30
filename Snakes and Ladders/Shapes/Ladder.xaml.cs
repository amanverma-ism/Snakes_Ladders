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

namespace Snakes_and_Ladders.Shapes
{
    /// <summary>
    /// Interaction logic for Ladder.xaml
    /// </summary>
    public partial class Ladder : UserControl
    {
        public Ladder(Point start, Point end)
        {
            InitializeComponent();
            Line1.X1 = start.X - 10;
            Line1.Y1 = start.Y;
            Line1.X2 = end.X - 10;
            Line1.Y2 = end.Y;

            Line1.Visibility = Visibility.Visible;

            Line2.X1 = start.X + 10;
            Line2.Y1 = start.Y;
            Line2.X2 = end.X + 10;
            Line2.Y2 = end.Y;

            Line2.Visibility = Visibility.Visible;

            AddSteps();
        }

        public void AddSteps()
        {
            int numberofSteps = (int)(Math.Sqrt(Math.Pow((Line1.X1 - Line1.X2), 2) + Math.Pow((Line1.Y1 - Line1.Y2), 2))/10);

            double slope1 = ((Line1.Y2 - Line1.Y1) / (Line1.X2 - Line1.X1));
            double const1 = Line1.Y1 - (slope1 * Line1.X1);
            double slope2 = ((Line2.Y2 - Line2.Y1) / (Line2.X2 - Line2.X1));
            double const2 = Line2.Y1 - (slope1 * Line2.X1);
            for (int i = 1; i<=numberofSteps; i++)
            {
                Line line = new Line();
                line.StrokeThickness = 4;
                line.Stroke = Brushes.Black;
                line.X1 = (Line1.X1 + 10*i);
                line.Y1 = ((Line1.X1 + 10*i)*slope1) + const1;
                line.X2 = (Line2.X1 + 10*i);
                line.Y2 = ((Line2.X1 + 10*i)*slope2) + const2;
                line.Visibility = Visibility.Visible;
                MainGrid.Children.Add(line);
            }
        }
    }
}
