﻿using System;
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
        private Point[] ptArr;
        private double _strokeThickness = 5;
        public double SnakeWidth
        {
            get { return _snakeWidth; }
            set { _snakeWidth = value; }
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

        public Snake()
        {
            DataContext = this;
            InitializeComponent();
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

        public void ResizeSnake(double width, double height)
        {
            for(int i = 0; i< ptArr.Count(); i++)
            {
                ptArr[i].X = (ptArr[i].X / _canvasWidth) * width;
                ptArr[i].Y = (ptArr[i].Y / _canvasHeight) * height;
            }
            SnakePath.Data = null;
            _canvasWidth = width;
            _canvasHeight = height;
            DrawCurve();
        }

        // Make the curve.
        public void DrawCurve()
        {
            // Remove any previous curves.
            // Make a path.
            if (_startPoint != null && _endPoint != null)
            {
                if(ptArr == null)
                    ptArr = CreatePathPoints(_startPoint, _endPoint).ToArray();
                MakeCurve(ptArr, 1);
                SnakePath.Stroke = Brushes.Red;
            }
        }

        // Make a Bezier curve connecting these points.
        private void MakeCurve(Point[] points, double tension)
        {
            if (points.Length < 2) return;
            Point[] result_points = MakeCurvePoints(points, tension);

            // Use the points to create the path.
            MakeBezierPath(result_points.ToArray());
        }

        private Point[] MakeCurvePoints(Point[] points, double tension)
        {
            if (points.Length < 2) return null;
            double control_scale = tension / 0.5 * 0.175;

            // Make a list containing the points and
            // appropriate control points.
            List<Point> result_points = new List<Point>();
            result_points.Add(points[0]);

            for (int i = 0; i <points.Length - 1; i++)
            {
                // Get the point and its neighbors.
                Point pt_before = points[Math.Max(i - 1, 0)];
                Point pt = points[i];
                Point pt_after = points[i + 1];
                Point pt_after2 = points[Math.Min(i + 2, points.Length - 1)];

                double dx1 = pt_after.X - pt_before.X;
                double dy1 = pt_after.Y - pt_before.Y;

                Point p1 = points[i];
                Point p4 = pt_after;

                double dx = pt_after.X - pt_before.X;
                double dy = pt_after.Y - pt_before.Y;
                Point p2 = new Point(
                    pt.X + control_scale * dx,
                    pt.Y + control_scale * dy);

                dx = pt_after2.X - pt.X;
                dy = pt_after2.Y - pt.Y;
                Point p3 = new Point(
                    pt_after.X - control_scale * dx,
                    pt_after.Y - control_scale * dy);

                // Save points p2, p3, and p4.
                result_points.Add(p2);
                result_points.Add(p3);
                result_points.Add(p4);
            }

            // Return the points.
            return result_points.ToArray();
        }

        // Make a Path holding a series of Bezier curves.
        // The points parameter includes the points to visit
        // and the control points.
        private void MakeBezierPath(Point[] points)
        {
            // Create a Path to hold the geometry.

            // Add a PathGeometry.
            PathGeometry path_geometry = new PathGeometry();
            SnakePath.Data = path_geometry;

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
                new PointCollection(points.Length - 1);
            for (int i = 1; i < points.Length; i++)
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

        public Image DrawCurve1(Point start, Point end)
        {
            Rect rect = new Rect(start, end);
            System.Drawing.Bitmap bitmap = new System.Drawing.Bitmap((int)rect.Width * 2, (int)rect.Height * 2);
            System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(bitmap);
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

            System.Drawing.Pen bl = new System.Drawing.Pen(System.Drawing.Brushes.Black);
            System.Drawing.Pen red = new System.Drawing.Pen(System.Drawing.Brushes.Red);
            List<Point> points = CreatePathPoints(start, end);
            List<System.Drawing.Point> drpts = new List<System.Drawing.Point>();
            foreach (Point point in points)
            {
                System.Drawing.Point pt = new System.Drawing.Point((int)point.X, (int)point.Y);
                drpts.Add(pt);
            }
            red.Width = 10;
            g.DrawBeziers(red, drpts.ToArray());
            Image image = new Image();

            image.Source = (ImageSource)(ToBitmapSource(bitmap));

            return image;
        }

        

        

        public BitmapSource ToBitmapSource(System.Drawing.Bitmap source)
        {
            BitmapSource bitSrc = null;

            var hBitmap = source.GetHbitmap();

            try
            {
                bitSrc = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                    hBitmap,
                    IntPtr.Zero,
                    Int32Rect.Empty,
                    BitmapSizeOptions.FromEmptyOptions());
            }
            catch (System.ComponentModel.Win32Exception)
            {
                bitSrc = null;
            }
            finally
            {
            }

            return bitSrc;
        }

        public List<Point> CreatePathPoints(Point start, Point end)
        {
            Random random = new Random();
            int numberofPoints =  random.Next(8, 13);
            List<Point> points = new List<Point>();
            double dy = (end.Y - start.Y) / numberofPoints;
            double dx = (end.X - start.X) / numberofPoints;

            bool bswitch = false;

            points.Add(start);
            Point prevPoint = start;

            for (int i = 1; i < numberofPoints; i++)
            {
                Point newpt = start;
                if (bswitch)
                {
                    newpt.Offset(random.NextDouble() * (i*dx), i * dy);
                }
                else
                {
                    newpt.Offset(i * dx, random.NextDouble() * (i*dy));
                }
                

                int switchNext = random.Next(0, 2);
                if (switchNext == 1)
                    bswitch = !bswitch;

                if (((newpt.X - prevPoint.X) * dx < 0) || ((newpt.Y - prevPoint.Y) * dy < 0))
                {
                    i--;
                    continue;
                }
                prevPoint = newpt;
                points.Add(newpt);
            }

            points.Add(end);

            return points;
        }
    }
}
