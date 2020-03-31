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
        private double _ladderWidth;
        private double _stepsDifference;
        public double LadderWidth
        {
            get { return _ladderWidth; }
            set { _ladderWidth = value; }
        }

        public double StepsDifference
        {
            get { return _stepsDifference; }
            set { _stepsDifference = value; }
        }
        public Ladder()
        {
            InitializeComponent();
        }

        public void DrawLadder(Point start, Point end, double opacity = 0.5)
        { 
            double dx = end.X - start.X;
            double dy = end.Y - start.Y;
            double dist = Math.Sqrt(dx * dx + dy * dy);
            dx /= dist;
            dy /= dist;

            Point line1Start = new Point(), line1End = new Point(), line2Start = new Point(), Line2End = new Point();
            line1Start.X = start.X - (_ladderWidth / 2) * dy;
            line1Start.Y = start.Y + (_ladderWidth / 2) * dx;

            line1End.X = end.X - (_ladderWidth / 2) * dy;
            line1End.Y = end.Y + (_ladderWidth / 2) * dx;

            line2Start.X = start.X + (_ladderWidth / 2) * dy;
            line2Start.Y = start.Y - (_ladderWidth / 2) * dx;

            Line2End.X = end.X + (_ladderWidth / 2) * dy;
            Line2End.Y = end.Y - (_ladderWidth / 2) * dx;

            Line1.X1 = line1Start.X;
            Line1.Y1 = line1Start.Y;
            Line1.X2 = line1End.X;
            Line1.Y2 = line1End.Y;

            Line1.Opacity = opacity;
            Line1.Visibility = Visibility.Visible;

            Line2.X1 = line2Start.X;
            Line2.Y1 = line2Start.Y;
            Line2.X2 = Line2End.X;
            Line2.Y2 = Line2End.Y;

            Line2.Opacity = opacity;
            Line2.Visibility = Visibility.Visible;

            AddSteps(opacity);
        }

        public void AddSteps(double opacity)
        {
            int numberofSteps = (int)(Math.Sqrt(Math.Pow((Line1.X1 - Line1.X2), 2) + Math.Pow((Line1.Y1 - Line1.Y2), 2))/_stepsDifference);

            for (int i = 1; i<numberofSteps; i++)
            {
                Line line = new Line();
                line.StrokeThickness = 4;
                line.Stroke = Brushes.Black;

                Vector A1 = new Vector(Line1.X1, Line1.Y1);
                Vector B1 = new Vector(Line1.X2, Line1.Y2);
                Vector C1 = B1 - A1;
                C1.Normalize();
                Vector P1 = ((_stepsDifference * i)*C1) + A1;

                line.X1 = P1.X;
                line.Y1 = P1.Y;

                Vector A2 = new Vector(Line2.X1, Line2.Y1);
                Vector B2 = new Vector(Line2.X2, Line2.Y2);
                Vector C2 = B2 - A2;
                C2.Normalize();
                Vector P2 = ((_stepsDifference * i) * C2) + A2;

                line.X1 = P1.X;
                line.Y1 = P1.Y;

                line.X2 = P2.X;
                line.Y2 = P2.Y;
                line.Opacity = opacity;
                line.Visibility = Visibility.Visible;
                MainGrid.Children.Add(line);
            }
        }
    }
}
