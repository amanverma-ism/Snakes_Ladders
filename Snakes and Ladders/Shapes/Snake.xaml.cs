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
    /// Interaction logic for Snake.xaml
    /// </summary>
    public partial class Snake : UserControl
    {
        public Snake()
        {
            DataContext = this;
            InitializeComponent();
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
        private Path MakeBezierPath(Point[] points)
        {
            // Create a Path to hold the geometry.
            Path path = new Path();

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
                new PointCollection(points.Length - 1);
            for (int i = 1; i < points.Length; i++)
                point_collection.Add(points[i]);

            // Make a PolyBezierSegment from the points.
            PolyBezierSegment bezier_segment = new PolyBezierSegment();
            bezier_segment.Points = point_collection;

            // Add the PolyBezierSegment to othe segment collection.
            path_segment_collection.Add(bezier_segment);

            return path;
        }

        // Make a Bezier curve connecting these points.
        private Path MakeCurve(Point[] points, double tension)
        {
            if (points.Length < 2) return null;
            Point[] result_points = MakeCurvePoints(points, tension);

            // Use the points to create the path.
            return MakeBezierPath(result_points.ToArray());
        }

        // Make the curve.
        public void DrawCurve(Point start, Point end)
        {
            // Remove any previous curves.
            // Make a path.
            Point[] ptarr = CreatePathPoints(start, end).ToArray();
            SnakePath = MakeCurve(ptarr, 0.8);
            SnakePath.Stroke = Brushes.DarkRed;
            SnakePath.StrokeThickness = 10;
        }

        public List<Point> CreatePathPoints(Point start, Point end)
        {
            Random random = new Random();
            int numberofPoints = random.Next(8, 13);
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
