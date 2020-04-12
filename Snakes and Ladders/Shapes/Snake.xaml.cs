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
    /// Interaction logic for Snake.xaml
    /// </summary>
    public partial class Snake : UserControl, INotifyPropertyChanged
    {
        private Collection<PropertyChangedEventHandler> _Handlers = new Collection<PropertyChangedEventHandler>();
        private double _snakeWidth;
        private Point _startPoint;
        private Point _endPoint;
        private double _canvasWidth;
        private double _canvasHeight;
        private double _eye1Left;
        private double _eye1Top;
        private double _eye2Left;
        private double _eye2Top;
        private double _strokeThickness = 5;
        private Ellipse _eye1;
        private Ellipse _eye2;


        public Snake()
        {
            DataContext = this;
            InitializeComponent();
            _eye1 = new Ellipse();
            _eye1.DataContext = this;
            _eye2 = new Ellipse();
            _eye2.DataContext = this;
        }

        #region Properties
        public double SnakeWidth
        {
            get { return _snakeWidth; }
            set { _snakeWidth = value; }
        }

        public Path UpperPath
        {
            get { return InnerSnakePath; }
        }

        public Path UpperPath2
        {
            get { return CenterSnakePath; }
        }

        public Triangle Tail
        {
            get { return _triangle; }
        }

        public Ellipse Eye1
        {
            get { return _eye1; }
        }

        public Ellipse Eye2
        {
            get { return _eye2; }
        }

        public Point StartPoint
        {
            get { return _startPoint; }
            set { _startPoint = value; }
        }

        public double CanvasWidth
        {
            get { return _canvasWidth; }
            set { _canvasWidth = value; }
        }

        public double CanvasHeight
        {
            get { return _canvasHeight; }
            set { _canvasHeight = value; }
        }

        public Point EndPoint
        {
            get { return _endPoint; }
            set { _endPoint = value; }
        }

        public double LineStrokeThickness
        {
            get
            {
                return _strokeThickness;
            }
            set
            {
                _strokeThickness = value;
                OnPropertyChanged("LineStrokeThickness");
                OnPropertyChanged("UpperLineStrokeThickness");
                OnPropertyChanged("UpperLine2StrokeThickness");
                OnPropertyChanged("TongueThickness");
                OnPropertyChanged("EyeStrokeThickness");
                OnPropertyChanged("EyeSize");
            }
        }

        public double UpperLineStrokeThickness
        {
            get
            {
                return _strokeThickness / 2.0;
            }
        }

        public double UpperLine2StrokeThickness
        {
            get
            {
                return _strokeThickness / 6.0;
            }
        }

        public double TongueThickness
        {
            get
            {
                return _strokeThickness / 6.0;
            }
        }

        public double EyeStrokeThickness
        {
            get
            {
                return _strokeThickness / 6.0;
            }
        }

        public double EyeSize
        {
            get
            {
                return _strokeThickness / 2.0;
            }
        }

        public double Eye1Top
        {
            get
            {
                return _eye1Top;
            }
            set
            {
                _eye1Top = value;
                OnPropertyChanged("Eye1Top");
            }
        }

        public double Eye1Left
        {
            get
            {
                return _eye1Left;
            }
            set
            {
                _eye1Left = value;
                OnPropertyChanged("Eye1Left");
            }
        }

        public double Eye2Top
        {
            get
            {
                return _eye2Top;
            }
            set
            {
                _eye2Top = value;
                OnPropertyChanged("Eye2Top");
            }
        }

        public double Eye2Left
        {
            get
            {
                return _eye2Left;
            }
            set
            {
                _eye2Left = value;
                OnPropertyChanged("Eye2Left");
            }
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

        #endregion

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

        public void ResizeSnake(double width, double height)
        {
            _startPoint.X = (_startPoint.X / _canvasWidth) * width;
            _startPoint.Y = (_startPoint.Y / _canvasHeight) * height;

            _endPoint.X = (_endPoint.X / _canvasWidth) * width;
            _endPoint.Y = (_endPoint.Y / _canvasHeight) * height;

            ResizePath(SnakePath, width, height);
            ResizePath(Tongue, width, height);
            ResizePath(Tongue2, width, height);
            ResizePath(InnerSnakePath, width, height);
            ResizePath(CenterSnakePath, width, height);

            ResizeEyes();
            _triangle.ResizeTriangle(width, height);
            _triangle2.ResizeTriangle(width, height);
            _triangle3.ResizeTriangle(width, height);
            _canvasWidth = width;
            _canvasHeight = height;
        }

        // Make the curve.
        public void DrawCurve()
        {
            // Remove any previous curves.
            // Make a path.
            if (_startPoint != null && _endPoint != null)
            {
                List<Point> ptArr = CreatePathPoints(_startPoint, _endPoint);
                MakeCurve(ptArr, 50);
                SnakePath.Stroke = Brushes.Red;
                System.Windows.Media.Effects.BlurEffect blurEffect2 = new System.Windows.Media.Effects.BlurEffect();
                blurEffect2.Radius = 4;
                blurEffect2.KernelType = System.Windows.Media.Effects.KernelType.Gaussian;
                SnakePath.Effect = blurEffect2;

                SnakePath.Clone(ref InnerSnakePath);
                InnerSnakePath.Stroke = Brushes.DarkRed;
                System.Windows.Media.Effects.BlurEffect blurEffect0 = new System.Windows.Media.Effects.BlurEffect();
                blurEffect0.Radius = 2;
                blurEffect0.KernelType = System.Windows.Media.Effects.KernelType.Gaussian;
                InnerSnakePath.Effect = blurEffect0;

                SnakePath.Clone(ref CenterSnakePath);
                CenterSnakePath.Stroke = Brushes.DarkOliveGreen;
                System.Windows.Media.Effects.BlurEffect blurEffect1 = new System.Windows.Media.Effects.BlurEffect();
                blurEffect1.Radius = 1;
                blurEffect1.KernelType = System.Windows.Media.Effects.KernelType.Gaussian;
                CenterSnakePath.Effect = blurEffect1;

                DrawTongue();
                DrawEyes();
                DrawTail();
                UpdateLayout();
            }
        }

        public void ResizePath(Path path, double width, double height)
        {
            (path.Data as PathGeometry).Figures[0].StartPoint = new Point(((path.Data as PathGeometry).Figures[0].StartPoint.X / _canvasWidth) * width, ((path.Data as PathGeometry).Figures[0].StartPoint.Y / _canvasHeight) * height);

            PointCollection pathPoints = ((path.Data as PathGeometry).Figures[0].Segments[0] as PolyBezierSegment).Points;

            for (int i = 0; i < pathPoints.Count; i++)
            {
                pathPoints[i] = new Point((pathPoints[i].X / _canvasWidth) * width, (pathPoints[i].Y / _canvasHeight) * height);
            }
        }

        private void DrawTongue()
        {
            Vector A1 = new Vector(_startPoint.X, _startPoint.Y);
            Vector B1 = new Vector(_endPoint.X, _endPoint.Y);
            Vector C1 = A1 - B1;
            C1.Normalize();
            Vector P1 = ((_strokeThickness) * C1) + A1;

            List<Point> pts = new List<Point>();
            List<Point> pts2 = new List<Point>();

            pts.Add(_startPoint);
            pts2.Add(_startPoint);
            pts.Add(new Point(P1.X, P1.Y));
            pts2.Add(new Point(P1.X, P1.Y));

            P1 = ((_strokeThickness*2) * C1) + A1;
            pts.Add(new Point(P1.X, P1.Y));
            pts2.Add(new Point(P1.X, P1.Y));

            double dx = pts[1].X - pts[0].X;
            double dy = pts[1].Y - pts[0].Y;
            double dist = Math.Sqrt(dx * dx + dy * dy);
            dx /= dist;
            dy /= dist;

            Point controlPt1 = new Point();
            Point controlPt2 = new Point();
            controlPt1.X = ((pts[1].X + pts[0].X)/2) - (_strokeThickness) * dy;
            controlPt1.Y = ((pts[1].Y + pts[0].Y) / 2) + (_strokeThickness/2 ) * dx;

            controlPt2.X = ((pts[1].X + pts[0].X) / 2) + (_strokeThickness) * dy;
            controlPt2.Y = ((pts[1].Y + pts[0].Y) / 2) - (_strokeThickness/2) * dx;

            dx = pts[2].X - pts[1].X;
            dy = pts[2].Y - pts[1].Y;
            dist = Math.Sqrt(dx * dx + dy * dy);
            dx /= dist;
            dy /= dist;

            Point ControlPt3 = new Point();
            ControlPt3.X = ((pts[2].X + pts[1].X) / 2) + (_strokeThickness/2) * dy;
            ControlPt3.Y = ((pts[2].Y + pts[1].Y) / 2) - (_strokeThickness) * dx;

            Point ControlPt4 = new Point();
            ControlPt4.X = ((pts[2].X + pts[1].X) / 2) - (_strokeThickness/2) * dy;
            ControlPt4.Y = ((pts[2].Y + pts[1].Y) / 2) + (_strokeThickness) * dx;


            pts.Insert(1, controlPt1);
            pts.Insert(2, controlPt2);
            pts.Insert(4, ControlPt4);
            pts.Insert(5, ControlPt3);

            pts2.Insert(1, controlPt2);
            pts2.Insert(2, controlPt1);
            //pts2.Insert(4, ControlPt4);
            pts2.Insert(4, ControlPt3);

            Point[] points = pts.ToArray();
            //points = MakeCurvePoints(points, 1);
            MakeBezierPath(ref Tongue, pts);
            Tongue.Stroke = Brushes.GreenYellow;

            MakeBezierPath(ref Tongue2, pts2);
            Tongue2.Stroke = Brushes.Yellow;
        }

        private void DrawEyes()
        {

            Vector A1 = new Vector(_startPoint.X, _startPoint.Y);
            Vector B1 = new Vector(_endPoint.X, _endPoint.Y);
            Vector C1 = B1 - A1;
            C1.Normalize();
            Vector P1 = ((_strokeThickness/3) * C1) + A1;

            double dx = _endPoint.X - _startPoint.X;
            double dy = _endPoint.Y - _startPoint.Y;
            double dist = Math.Sqrt(dx * dx + dy * dy);
            dx /= dist;
            dy /= dist;

            Point line1Start = new Point(), line2Start = new Point();
            line1Start.X = P1.X - (_strokeThickness / 2) * dy;
            line1Start.Y = P1.Y + (_strokeThickness / 2) * dx;

            line2Start.X = P1.X + (_strokeThickness / 2) * dy;
            line2Start.Y = P1.Y - (_strokeThickness / 2) * dx;

            
            Eye1.SetBinding(Canvas.LeftProperty, "Eye1Left");
            Eye1.SetBinding(Canvas.TopProperty, "Eye1Top");
            Eye1.SetBinding(Ellipse.HeightProperty, "EyeSize");
            Eye1.SetBinding(Ellipse.WidthProperty, "EyeSize");
            Eye1.SetBinding(Ellipse.StrokeThicknessProperty, "EyeStrokeThickness");
            Eye1.Stroke = Brushes.Yellow;
            Eye1.Fill = Brushes.Black;
            Eye1.Visibility = Visibility.Visible;
            Eye1Left = line1Start.X;
            Eye1Top = line1Start.Y;

            Eye2.SetBinding(Canvas.LeftProperty, "Eye2Left");
            Eye2.SetBinding(Canvas.TopProperty, "Eye2Top");
            Eye2.SetBinding(Ellipse.HeightProperty, "EyeSize");
            Eye2.SetBinding(Ellipse.WidthProperty, "EyeSize");
            Eye2.SetBinding(Ellipse.StrokeThicknessProperty, "EyeStrokeThickness");
            Eye2.Stroke = Brushes.Yellow;
            Eye2.Fill = Brushes.Black;
            Eye2.Visibility = Visibility.Visible;
            Eye2Left = line2Start.X;
            Eye2Top = line2Start.Y;

        }

        private void DrawTail()
        {
            Vector A1 = new Vector(_startPoint.X, _startPoint.Y);
            Vector B1 = new Vector(_endPoint.X, _endPoint.Y);
            Vector C1 = B1 - A1;
            C1.Normalize();
            Vector P1 = ((_strokeThickness * 2) * C1) + B1;

            double dx = _endPoint.X - _startPoint.X;
            double dy = _endPoint.Y - _startPoint.Y;
            double dist = Math.Sqrt(dx * dx + dy * dy);
            dx /= dist;
            dy /= dist;
            Point Point1 = new Point(), Point2 = new Point();

            {
                Point1.X = _endPoint.X - (_strokeThickness / 2.0) * dy;
                Point1.Y = _endPoint.Y + (_strokeThickness / 2.0) * dx;

                Point2.X = _endPoint.X + (_strokeThickness / 2.0) * dy;
                Point2.Y = _endPoint.Y - (_strokeThickness / 2.0) * dx;

                Point Point3 = new Point(P1.X, P1.Y);

                //_triangle = new Triangle();
                _triangle.CanvasHeight = _canvasHeight;
                _triangle.CanvasWidth = _canvasWidth;
                _triangle.DrawTriangle(Point1, Point2, Point3);
                System.Windows.Media.Effects.BlurEffect blurEffect = new System.Windows.Media.Effects.BlurEffect();
                blurEffect.Radius = 4;
                blurEffect.KernelType = System.Windows.Media.Effects.KernelType.Gaussian;
                _triangle.Effect = blurEffect;
                _triangle.FillColor = Brushes.Red;
            }
            {
                Point1.X = _endPoint.X - (_strokeThickness / 4.0) * dy;
                Point1.Y = _endPoint.Y + (_strokeThickness / 4.0) * dx;

                Point2.X = _endPoint.X + (_strokeThickness / 4.0) * dy;
                Point2.Y = _endPoint.Y - (_strokeThickness / 4.0) * dx;

                Point Point3 = new Point(P1.X, P1.Y);

                //_triangle = new Triangle();
                _triangle2.CanvasHeight = _canvasHeight;
                _triangle2.CanvasWidth = _canvasWidth;
                _triangle2.DrawTriangle(Point1, Point2, Point3);
                System.Windows.Media.Effects.BlurEffect blurEffect = new System.Windows.Media.Effects.BlurEffect();
                blurEffect.Radius = 2;
                blurEffect.KernelType = System.Windows.Media.Effects.KernelType.Gaussian;
                _triangle2.Effect = blurEffect;
                _triangle2.FillColor = Brushes.DarkRed;

            }
            {
                Point1.X = _endPoint.X - (_strokeThickness / 12.0) * dy;
                Point1.Y = _endPoint.Y + (_strokeThickness / 12.0) * dx;

                Point2.X = _endPoint.X + (_strokeThickness / 12.0) * dy;
                Point2.Y = _endPoint.Y - (_strokeThickness / 12.0) * dx;

                Point Point3 = new Point(P1.X, P1.Y);

                //_triangle = new Triangle();
                _triangle3.CanvasHeight = _canvasHeight;
                _triangle3.CanvasWidth = _canvasWidth;
                _triangle3.DrawTriangle(Point1, Point2, Point3);
                System.Windows.Media.Effects.BlurEffect blurEffect = new System.Windows.Media.Effects.BlurEffect();
                blurEffect.Radius = 1;
                blurEffect.KernelType = System.Windows.Media.Effects.KernelType.Gaussian;
                _triangle3.Effect = blurEffect;
                _triangle3.FillColor = Brushes.DarkOliveGreen;

            }
        }

        private void ResizeEyes()
        {
            Vector A1 = new Vector(_startPoint.X, _startPoint.Y);
            Vector B1 = new Vector(_endPoint.X, _endPoint.Y);
            Vector C1 = B1 - A1;
            C1.Normalize();
            Vector P1 = ((_strokeThickness / 3) * C1) + A1;

            double dx = _endPoint.X - _startPoint.X;
            double dy = _endPoint.Y - _startPoint.Y;
            double dist = Math.Sqrt(dx * dx + dy * dy);
            dx /= dist;
            dy /= dist;

            Point line1Start = new Point(), line2Start = new Point();
            line1Start.X = P1.X - (_strokeThickness / 2) * dy;
            line1Start.Y = P1.Y + (_strokeThickness / 2) * dx;

            line2Start.X = P1.X + (_strokeThickness / 2) * dy;
            line2Start.Y = P1.Y - (_strokeThickness / 2) * dx;

            Eye1Left = line1Start.X;
            Eye1Top = line1Start.Y;

            Eye2Left = line2Start.X;
            Eye2Top = line2Start.Y;
            OnPropertyChanged("EyeSize");

        }
        // Make a Bezier curve connecting these points.

        private void MakeCurve(List<Point> points, double tension)
        {
            if (points.Count < 3) return;
            List<Point> result_points = MakeCurvePoints(points, tension);
            // Use the points to create the path.
            MakeBezierPath(ref SnakePath,result_points);
        }

        private List<Point> MakeCurvePoints(List<Point> iPoints, double tension)
        {
            Random random = new Random();
            List<Point> oPoints = new List<Point>();
            


            double dx = _endPoint.X - _startPoint.X;
            double dy = _endPoint.Y - _startPoint.Y;
            double dist = Math.Sqrt(dx * dx + dy * dy);
            dx /= dist;
            dy /= dist;

            //Add the two control points at the line only for start point and the next point so that at last the head will be straight.
            oPoints.Add(iPoints[0]);

            Point startMidPoint = new Point((iPoints[1].X + iPoints[0].X) / 2, (iPoints[1].Y + iPoints[0].Y) / 2);
            Point firstcontrolPt1 = new Point();
            Point firstcontrolPt2 = new Point();


            firstcontrolPt1.X = ((startMidPoint.X + iPoints[0].X) / 2) - (0.3) * dy;
            firstcontrolPt1.Y = ((startMidPoint.Y + iPoints[0].Y) / 2) + (0.3) * dx;

            firstcontrolPt2.X = ((iPoints[1].X + startMidPoint.X) / 2) + (0.3) * dy;
            firstcontrolPt2.Y = ((iPoints[1].Y + startMidPoint.Y) / 2) - (0.3) * dx;

            oPoints.Add(firstcontrolPt1);
            oPoints.Add(firstcontrolPt2);
            oPoints.Add(iPoints[1]);

            //Now add the control points for other points except for last two.

            bool changeDir = false;
            for (int i = 1; i< iPoints.Count - 2; i++)
            {
                Point midPoint = new Point((iPoints[i + 1].X + iPoints[i].X) / 2, (iPoints[i + 1].Y + iPoints[i].Y) / 2);
                Point controlPt1 = new Point();
                Point controlPt2 = new Point();
                int bswitch = random.Next(0, 2);

                if (changeDir == true)
                {
                    double tensionTemp = tension * random.NextDouble();
                    tensionTemp = tensionTemp > 0.6 * tension ? tensionTemp : tension * 0.6;
                    if (i == 1 || i == iPoints.Count - 3)
                        tensionTemp = tensionTemp / 2;
                    controlPt1.X = ((midPoint.X + iPoints[i].X) / 2) + (tensionTemp) * dy;
                    controlPt1.Y = ((midPoint.Y + iPoints[i].Y) / 2) - (tensionTemp) * dx;
                }
                else
                {
                    double tensionTemp = tension * random.NextDouble();
                    tensionTemp = tensionTemp > 0.6 * tension ? tensionTemp : tension * 0.6;
                    if (i == 1 || i == iPoints.Count - 3)
                        tensionTemp = tensionTemp / 2;
                    controlPt1.X = ((midPoint.X + iPoints[i].X) / 2) - (tensionTemp) * dy;
                    controlPt1.Y = ((midPoint.Y + iPoints[i].Y) / 2) + (tensionTemp) * dx;
                }

                if (bswitch == 1)
                {
                    double tensionTemp = tension * random.NextDouble();
                    tensionTemp = tensionTemp > 0.6 * tension ? tensionTemp : tension * 0.6;
                    if (i == 1 || i == iPoints.Count - 3)
                        tensionTemp = tensionTemp / 2;
                    controlPt2.X = ((iPoints[i + 1].X + midPoint.X) / 2) + (tensionTemp) * dy;
                    controlPt2.Y = ((iPoints[i + 1].Y + midPoint.Y) / 2) - (tensionTemp) * dx;
                    changeDir = false;
                }
                else
                {
                    double tensionTemp = tension * random.NextDouble();
                    tensionTemp = tensionTemp > 0.6 * tension ? tensionTemp : tension * 0.6;
                    if (i == 1 || i == iPoints.Count - 3)
                        tensionTemp = tensionTemp / 2;
                    controlPt2.X = ((iPoints[i + 1].X + midPoint.X) / 2) - (tensionTemp) * dy;
                    controlPt2.Y = ((iPoints[i + 1].Y + midPoint.Y) / 2) + (tensionTemp) * dx;
                    changeDir = true;
                }
                
                oPoints.Add(controlPt1);
                oPoints.Add(controlPt2);
                oPoints.Add(iPoints[i + 1]);
            }

            //Add the two control points at the line only for second last point and the end point so that at last the tail will be straight.

            Point endMidPoint = new Point((iPoints[iPoints.Count - 1].X + iPoints[iPoints.Count - 2].X) / 2, (iPoints[iPoints.Count - 1].Y + iPoints[iPoints.Count - 2].Y) / 2);
            Point lastcontrolPt1 = new Point();
            Point lastcontrolPt2 = new Point();

            if (changeDir == true)
            {
                lastcontrolPt1.X = ((endMidPoint.X + iPoints[iPoints.Count - 2].X) / 2) + (0.2) * dy;
                lastcontrolPt1.Y = ((endMidPoint.Y + iPoints[iPoints.Count - 2].Y) / 2) - (0.2) * dx;

                lastcontrolPt2.X = ((iPoints[iPoints.Count - 1].X + endMidPoint.X) / 2) - (0.2) * dy;
                lastcontrolPt2.Y = ((iPoints[iPoints.Count - 1].Y + endMidPoint.Y) / 2) + (0.2) * dx;
            }
            else
            {
                lastcontrolPt1.X = ((endMidPoint.X + iPoints[iPoints.Count - 2].X) / 2) - (0.2) * dy;
                lastcontrolPt1.Y = ((endMidPoint.Y + iPoints[iPoints.Count - 2].Y) / 2) + (0.2) * dx;

                lastcontrolPt2.X = ((iPoints[iPoints.Count - 1].X + endMidPoint.X) / 2) + (0.2) * dy;
                lastcontrolPt2.Y = ((iPoints[iPoints.Count - 1].Y + endMidPoint.Y) / 2) - (0.2) * dx;
            }
            oPoints.Add(lastcontrolPt1);
            oPoints.Add(lastcontrolPt2);
            oPoints.Add(iPoints[iPoints.Count - 1]);
            return oPoints;
        }

        // Make a Path holding a series of Bezier curves.
        // The points parameter includes the points to visit
        // and the control points.
        private void MakeBezierPath(ref Path path, List<Point> points)
        {
            // Create a Path to hold the geometry.

            // Add a PathGeometry.
            PathGeometry path_geometry = new PathGeometry();
            path.Data = path_geometry;
            
            // Create a PathFigure.
            PathFigure path_figure = new PathFigure();
            path_geometry.Figures.Add(path_figure);

            // Start at the first point.
            path_figure.StartPoint = points[0];

            // Create a PathSegmentCollection.
            PathSegmentCollection path_segment_collection =
                new PathSegmentCollection();
            path_figure.Segments = path_segment_collection;

            // Add the rest of the points to a PointCollection.
            PointCollection point_collection =
                new PointCollection(points.Count - 1);
            for (int i = 1; i < points.Count; i++)
                point_collection.Add(points[i]);

            // Make a PolyBezierSegment from the points.
            PolyBezierSegment bezier_segment = new PolyBezierSegment();
            bezier_segment.Points = point_collection;

            // Add the PolyBezierSegment to othe segment collection.
            path_segment_collection.Add(bezier_segment);
        }

        public Line[] GetLines()
        {
            double dx = _endPoint.X - _startPoint.X;
            double dy = _endPoint.Y - _startPoint.Y;
            double dist = Math.Sqrt(dx * dx + dy * dy);
            dx /= dist;
            dy /= dist;

            Point line1Start = new Point(), line1End = new Point(), line2Start = new Point(), Line2End = new Point();
            line1Start.X = _startPoint.X - (_snakeWidth / 2) * dy;
            line1Start.Y = _startPoint.Y + (_snakeWidth / 2) * dx;

            line1End.X = _endPoint.X - (_snakeWidth / 2) * dy;
            line1End.Y = _endPoint.Y + (_snakeWidth / 2) * dx;

            line2Start.X = _startPoint.X + (_snakeWidth / 2) * dy;
            line2Start.Y = _startPoint.Y - (_snakeWidth / 2) * dx;

            Line2End.X = _endPoint.X + (_snakeWidth / 2) * dy;
            Line2End.Y = _endPoint.Y - (_snakeWidth / 2) * dx;

            Line Line1 = new Line();
            Line1.X1 = line1Start.X;
            Line1.Y1 = line1Start.Y;
            Line1.X2 = line1End.X;
            Line1.Y2 = line1End.Y;

            Line Line2 = new Line();
            Line2.X1 = line2Start.X;
            Line2.Y1 = line2Start.Y;
            Line2.X2 = Line2End.X;
            Line2.Y2 = Line2End.Y;

            Line Line3 = new Line();
            Line3.X1 = line1Start.X;
            Line3.Y1 = line1Start.Y;
            Line3.X2 = line2Start.X;
            Line3.Y2 = line2Start.Y;

            Line Line4 = new Line();
            Line4.X1 = line1End.X;
            Line4.Y1 = line1End.Y;
            Line4.X2 = Line2End.X;
            Line4.Y2 = Line2End.Y;
            return new Line[] { Line1, Line2, Line3, Line4 };
        }
        public bool IsIntersecting(Snake iSnake)
        {
            Line[] snakeLines = GetLines();
            Line[] iSnakeLines = iSnake.GetLines();
            Vector L1Start = new Vector(snakeLines[0].X1, snakeLines[0].Y1);
            Vector L1End = new Vector(snakeLines[0].X2, snakeLines[0].Y2);

            Vector L2Start = new Vector(snakeLines[1].X1, snakeLines[1].Y1);
            Vector L2End = new Vector(snakeLines[1].X2, snakeLines[1].Y2);

            Vector L3Start = new Vector(snakeLines[2].X1, snakeLines[2].Y1);
            Vector L3End = new Vector(snakeLines[2].X2, snakeLines[2].Y2);

            Vector L4Start = new Vector(snakeLines[3].X1, snakeLines[3].Y1);
            Vector L4End = new Vector(snakeLines[3].X2, snakeLines[3].Y2);

            Vector iL1Start = new Vector(iSnakeLines[0].X1, iSnakeLines[0].Y1);
            Vector iL1End = new Vector(iSnakeLines[0].X2, iSnakeLines[0].Y2);

            Vector iL2Start = new Vector(iSnakeLines[1].X1, iSnakeLines[1].Y1);
            Vector iL2End = new Vector(iSnakeLines[1].X2, iSnakeLines[1].Y2);
            Vector intersectionPoint = new Vector();

            return (SnLUtility.LineSegementsIntersect(iL1Start, iL1End, L1Start, L1End, out intersectionPoint, true) ||
                SnLUtility.LineSegementsIntersect(iL1Start, iL1End, L2Start, L2End, out intersectionPoint, true) ||
                SnLUtility.LineSegementsIntersect(iL2Start, iL2End, L1Start, L1End, out intersectionPoint, true) ||
                SnLUtility.LineSegementsIntersect(iL2Start, iL2End, L2Start, L2End, out intersectionPoint, true) ||
                SnLUtility.LineSegementsIntersect(iL2Start, iL2End, L3Start, L3End, out intersectionPoint, true) ||
                SnLUtility.LineSegementsIntersect(iL2Start, iL2End, L4Start, L4End, out intersectionPoint, true)
                );
        }

        public List<Point> CreatePathPoints(Point start, Point end)
        {
            Random random = new Random();
            
            List<Point> points = new List<Point>();

            Vector A1 = new Vector(start.X, start.Y);
            Vector B1 = new Vector(end.X, end.Y);
            Vector C1 = B1 - A1;
            C1.Normalize();
            Vector PStart = ((_strokeThickness) * C1) + A1;
            Vector PEnd = B1 - ((_strokeThickness) * C1);

            double dy = (PEnd.Y - PStart.Y);
            double dx = (PEnd.X - PStart.X);
            double dist = Math.Sqrt(dx * dx + dy * dy);
            int numberofPoints = (int)dist/(int)(7*_strokeThickness);
            numberofPoints = numberofPoints > 3 ? numberofPoints : 3;


            if (numberofPoints % 2 == 0)
                numberofPoints--;

            dy = (end.Y - start.Y) / numberofPoints;
            dx = (end.X - start.X) / numberofPoints;
            dist = Math.Sqrt(dx * dx + dy * dy);

            points.Add(start);
            points.Add(new Point(PStart.X, PStart.Y));
            Vector prevPoint = PStart;

            bool bswitch = false;
            for (int i = 1; i < numberofPoints; i++)
            {
                Vector nextPt;
                
                nextPt = ((dist) * C1) + prevPoint;

                points.Add(new Point(nextPt.X, nextPt.Y));
                prevPoint = nextPt;
                bswitch = !bswitch;
            }

            points.Add(new Point(PEnd.X, PEnd.Y));
            points.Add(end);

            return points;
        }
    }
}
