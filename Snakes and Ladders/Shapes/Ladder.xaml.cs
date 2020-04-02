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
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Snakes_and_Ladders.Shapes
{
    /// <summary>
    /// Interaction logic for Ladder.xaml
    /// </summary>
    public partial class Ladder : UserControl, INotifyPropertyChanged
    {
        private Collection<PropertyChangedEventHandler> _Handlers = new Collection<PropertyChangedEventHandler>();
        private double _ladderWidth;
        private double _stepsDifference;
        private Line firstStep, lastStep;
        private double _LineThickness = 4;
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

        public double LineThickness
        {
            get
            {
                return _LineThickness;
            }
            set
            {
                _LineThickness = value;
                OnPropertyChanged("LineThickness");
            }
        }

        public Ladder()
        {
            InitializeComponent();
            DataContext = this;
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

        public bool IsIntersecting(Ladder iLadder)
        {
            Vector L1Start = new Vector(Line1.X1, Line1.Y1);
            Vector L1End = new Vector(Line1.X2, Line1.Y2);

            Vector L2Start = new Vector(Line2.X1, Line2.Y1);
            Vector L2End = new Vector(Line2.X2, Line2.Y2);

            Vector L3Start = new Vector(firstStep.X1, firstStep.Y1);
            Vector L3End = new Vector(firstStep.X2, firstStep.Y2);

            Vector L4Start = new Vector(lastStep.X1, lastStep.Y1);
            Vector L4End = new Vector(lastStep.X2, lastStep.Y2);

            Vector iL1Start = new Vector(iLadder.Line1.X1, iLadder.Line1.Y1);
            Vector iL1End = new Vector(iLadder.Line1.X2, iLadder.Line1.Y2);

            Vector iL2Start = new Vector(iLadder.Line2.X1, iLadder.Line2.Y1);
            Vector iL2End = new Vector(iLadder.Line2.X2, iLadder.Line2.Y2);
            Vector intersectionPoint = new Vector();

            return (SnLUtility.LineSegementsIntersect(iL1Start, iL1End, L1Start, L1End, out intersectionPoint, true) ||
                SnLUtility.LineSegementsIntersect(iL1Start, iL1End, L2Start, L2End, out intersectionPoint, true) ||
                SnLUtility.LineSegementsIntersect(iL2Start, iL2End, L1Start, L1End, out intersectionPoint, true) ||
                SnLUtility.LineSegementsIntersect(iL2Start, iL2End, L2Start, L2End, out intersectionPoint, true) ||
                SnLUtility.LineSegementsIntersect(iL2Start, iL2End, L3Start, L3End, out intersectionPoint, true) ||
                SnLUtility.LineSegementsIntersect(iL2Start, iL2End, L4Start, L4End, out intersectionPoint, true)
                );
        }

        public void DrawLadder(Point start, Point end, double opacity = 1)
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
                line.Stroke = Brushes.Black;
                line.SetBinding(Line.StrokeThicknessProperty, "LineThickness");

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
                if (i == 1)
                    firstStep = line;
                else if (i == numberofSteps - 1)
                    lastStep = line;
            }
        }
    }
}
