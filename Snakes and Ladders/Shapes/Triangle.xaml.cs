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
    /// Interaction logic for Triangle.xaml
    /// </summary>
    public partial class Triangle : UserControl
    {
        private double _canvasWidth;
        private double _canvasHeight;
        private Brush _fillColor;
        public Brush FillColor
        {
            get { return _fillColor; }
            set
            {
                _fillColor = value;
            }
        }
        public Triangle()
        {
            InitializeComponent();
            DataContext = this;
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

        public void DrawTriangle(Point p1, Point p2, Point p3)
        {
            // Create a Path to hold the geometry.

            // Add a PathGeometry.
            PathGeometry path_geometry = new PathGeometry();
            tPath.Data = path_geometry;

            // Create a PathFigure.
            PathFigure path_figure = new PathFigure();
            path_geometry.Figures.Add(path_figure);

            // Start at the first point.
            path_figure.StartPoint = p1;

            // Create a PathSegmentCollection.
            PathSegmentCollection path_segment_collection =
                new PathSegmentCollection();
            path_figure.Segments = path_segment_collection;

            // Add the rest of the points to a PointCollection.
            PointCollection point_collection =
                new PointCollection(2);
            point_collection.Add(p2);
            point_collection.Add(p3);

            // Make a PolyLineSegment from the points.
            PolyLineSegment polyline_segment = new PolyLineSegment();
            polyline_segment.Points = point_collection;

            // Add the PolyLineSegment to othe segment collection.
            path_segment_collection.Add(polyline_segment);
        }

        public void ResizeTriangle(double width, double height)
        {
            (tPath.Data as PathGeometry).Figures[0].StartPoint = new Point(((tPath.Data as PathGeometry).Figures[0].StartPoint.X / _canvasWidth) * width, ((tPath.Data as PathGeometry).Figures[0].StartPoint.Y / _canvasHeight) * height);

            PointCollection pathPoints = ((tPath.Data as PathGeometry).Figures[0].Segments[0] as PolyLineSegment).Points;

            for (int i = 0; i < pathPoints.Count; i++)
            {
                pathPoints[i] = new Point((pathPoints[i].X / _canvasWidth) * width, (pathPoints[i].Y / _canvasHeight) * height);
            }
            _canvasWidth = width;
            _canvasHeight = height;
        }
    }
}
