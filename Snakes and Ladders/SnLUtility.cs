using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Snakes_and_Ladders
{
    /// <summary>
    /// This class is for all the extension methods we are using in the solution.
    /// </summary>
    public static class ExtensionMethods
    {
        /// <summary>
        /// This returns the cross product of the vector on which it is called and the one vector in the parameter.
        /// </summary>
        /// <param name="vx">Main Object on which this method will be called.</param>
        /// <param name="v">The object with which the cross product is to be done.</param>
        /// <returns>The cross product value of the two vectors.</returns>
        public static double Cross(this Vector vx, Vector v)
        {
            return vx.X * v.Y - vx.Y * v.X;
        }

        private const double Epsilon = 1e-10;

        /// <summary>
        /// This checks if the double value is close to zero to the accuracy of 1e-10.
        /// </summary>
        /// <param name="d">The value to be checked.</param>
        /// <returns>True if double value is close to zero else false.</returns>
        public static bool IsZero(this double d)
        {
            return Math.Abs(d) < Epsilon;
        }
        
        /// <summary>
        /// This method provides an easy way to add Token UIElement to the canvas as there are more than one UIElement present in Token.
        /// </summary>
        /// <param name="cnv">Main canvas</param>
        /// <param name="token">The UIElement to be added.</param>
        public static void AddToken(this System.Windows.Controls.Canvas cnv, Token token)
        {
            cnv.Children.Add(token);
            cnv.Children.Add(token.InnerRing);
            cnv.Children.Add(token.OuterRing);

        }

        /// <summary>
        /// This method provides an easy way to remove Token UIElement from the canvas as there are more than one UIElement present in Token.
        /// </summary>
        /// <param name="cnv">Main canvas</param>
        /// <param name="token">The UIElement to be removed.</param>
        public static void RemoveToken(this System.Windows.Controls.Canvas cnv, Token token)
        {
            cnv.Children.Remove(token);
            cnv.Children.Remove(token.InnerRing);
            cnv.Children.Remove(token.OuterRing);
        }

        /// <summary>
        /// This method provides an easy way to add Snake UIElement to the canvas as there are more than one UIElement present in Snake.
        /// </summary>
        /// <param name="cnv">Main canvas</param>
        /// <param name="token">The UIElement to be added.</param>
        public static void AddSnake(this System.Windows.Controls.Canvas cnv, Shapes.Snake snake)
        {
            cnv.Children.Add(snake);
            cnv.Children.Add(snake.Eye1);
            cnv.Children.Add(snake.Eye2);
        }

        /// <summary>
        /// This method provides an easy way to remove Snake UIElement from the canvas as there are more than one UIElement present in Snake.
        /// </summary>
        /// <param name="cnv">Main canvas</param>
        /// <param name="token">The UIElement to be removed.</param>
        public static void RemoveSnake(this System.Windows.Controls.Canvas cnv, Shapes.Snake snake)
        {
            cnv.Children.Remove(snake);
            cnv.Children.Remove(snake.Eye1);
            cnv.Children.Remove(snake.Eye2);
        }

        /// <summary>
        /// This method is used to find the zero-based index of the key present in Dictionary<int, int>.
        /// </summary>
        /// <param name="dic">Dictionary object in which key is to be searched.</param>
        /// <param name="key">The key value to be searched.</param>
        /// <returns>Zero-based index value if the key is found, else -1.</returns>
        public static int GetIndexOfKey(this Dictionary<int, int> dic, int key)
        {
            int retVal = -1;
            foreach(KeyValuePair<int, int> itr in dic)
            {
                retVal++;
                if (itr.Key == key)
                    return retVal;

            }
            
            return -1;
        }

        /// <summary>
        /// This method is used to circulate the gameplay between players. This ensures that the chance for users is sequencially moved.
        /// </summary>
        /// <param name="ienGameToken">Current token value</param>
        /// <param name="ienGameType">Current game type</param>
        /// <returns>New token value</returns>
        public static enGameToken Next(this enGameToken ienGameToken, enGameType ienGameType)
        {
            enGameToken newtoken = enGameToken.Green;
            if (ienGameToken == enGameToken.Green)
                newtoken = enGameToken.Red;
            else if (ienGameToken == enGameToken.Red && ienGameType == enGameType.TwoPlayer)
                newtoken = enGameToken.Green;
            else if (ienGameToken == enGameToken.Red && ienGameType != enGameType.TwoPlayer)
                newtoken = enGameToken.Blue;
            else if (ienGameToken == enGameToken.Blue && ienGameType == enGameType.ThreePlayer)
                newtoken = enGameToken.Green;
            else if (ienGameToken == enGameToken.Blue && ienGameType != enGameType.ThreePlayer)
                newtoken = enGameToken.Yellow;
            else if (ienGameToken == enGameToken.Yellow)
                newtoken = enGameToken.Green;
            return newtoken;
        }

        /// <summary>
        /// This method is used to clone the Path.
        /// </summary>
        /// <param name="path">The path from which clone is to be created.</param>
        /// <returns>The cloned Path object.</returns>
        public static Path Clone(this Path path)
        {
            Path newPath = new Path();
            newPath.Data = new PathGeometry();
            (newPath.Data as PathGeometry).Figures.Add(new PathFigure());
            (newPath.Data as PathGeometry).Figures[0].StartPoint = (path.Data as PathGeometry).Figures[0].StartPoint;
            (newPath.Data as PathGeometry).Figures[0].Segments.Add(new PolyBezierSegment());
            ((newPath.Data as PathGeometry).Figures[0].Segments[0] as PolyBezierSegment).Points = new PointCollection();

            PointCollection pathPoints = ((path.Data as PathGeometry).Figures[0].Segments[0] as PolyBezierSegment).Points;

            for (int i = 0; i < pathPoints.Count; i++)
            {
                ((newPath.Data as PathGeometry).Figures[0].Segments[0] as PolyBezierSegment).Points.Add(pathPoints[i]);
            }
            newPath.Opacity = path.Opacity;
            return newPath;
        }

        /// <summary>
        /// This method creates a path with the offset values of x and y as mentioned in the call.
        /// </summary>
        /// <param name="path">The path from which the clone is to be created.</param>
        /// <param name="offsetX">Offset value for X co-ordinate.</param>
        /// <param name="offsetY">Offset value for Y co-ordinate.</param>
        /// <returns>Cloned path with the points offset by offsetX and offsetY values.</returns>
        public static Path Clone(this Path path, double offsetX, double offsetY)
        {
            Path newPath = new Path();
            newPath.Data = new PathGeometry();
            (newPath.Data as PathGeometry).Figures.Add(new PathFigure());
            (newPath.Data as PathGeometry).Figures[0].StartPoint = new Point((path.Data as PathGeometry).Figures[0].StartPoint.X + offsetX, (path.Data as PathGeometry).Figures[0].StartPoint.Y + offsetY);

            (newPath.Data as PathGeometry).Figures[0].Segments.Add(new PolyBezierSegment());
            ((newPath.Data as PathGeometry).Figures[0].Segments[0] as PolyBezierSegment).Points = new PointCollection();

            PointCollection pathPoints = ((path.Data as PathGeometry).Figures[0].Segments[0] as PolyBezierSegment).Points;

            for (int i = 0; i < pathPoints.Count; i++)
            {
                ((newPath.Data as PathGeometry).Figures[0].Segments[0] as PolyBezierSegment).Points.Add(new Point(pathPoints[i].X + offsetX, pathPoints[i].Y + offsetY));
            }
            newPath.Opacity = path.Opacity;
            return newPath;
        }

        /// <summary>
        /// This method is used to clone the Path.
        /// </summary>
        /// <param name="path">The path from which clone is to be created.</param>
        /// <param name="newPath">The cloned Path object.</param>
        public static void Clone(this Path path, ref Path newPath)
        {
            newPath.Data = new PathGeometry();
            (newPath.Data as PathGeometry).Figures.Add(new PathFigure());
            (newPath.Data as PathGeometry).Figures[0].StartPoint = (path.Data as PathGeometry).Figures[0].StartPoint;
            (newPath.Data as PathGeometry).Figures[0].Segments.Add(new PolyBezierSegment());
            ((newPath.Data as PathGeometry).Figures[0].Segments[0] as PolyBezierSegment).Points = new PointCollection();

            PointCollection pathPoints = ((path.Data as PathGeometry).Figures[0].Segments[0] as PolyBezierSegment).Points;

            for (int i = 0; i < pathPoints.Count; i++)
            {
                ((newPath.Data as PathGeometry).Figures[0].Segments[0] as PolyBezierSegment).Points.Add(pathPoints[i]);
            }
            newPath.Opacity = path.Opacity;
        }
    }

    /// <summary>
    /// This class is the utility class which contains the common/reusable functionalities that can be called from anywhere.
    /// </summary>
    public static class SnLUtility
    {
        /// <summary>
        /// Test whether two line segments intersect. If so, calculate the intersection point.
        /// <see cref="http://stackoverflow.com/a/14143738/292237"/>
        /// </summary>
        /// <param name="p">Vector to the start point of p.</param>
        /// <param name="p2">Vector to the end point of p.</param>
        /// <param name="q">Vector to the start point of q.</param>
        /// <param name="q2">Vector to the end point of q.</param>
        /// <param name="intersection">The point of intersection, if any.</param>
        /// <param name="considerOverlapAsIntersect">Do we consider overlapping lines as intersecting?
        /// </param>
        /// <returns>True if an intersection point was found.</returns>
        public static bool LineSegementsIntersect(Vector p, Vector p2, Vector q, Vector q2,
            out Vector intersection, bool considerCollinearOverlapAsIntersect = false)
        {
            intersection = new Vector();

            var r = p2 - p;
            var s = q2 - q;
            var rxs = r.Cross(s);
            var qpxr = (q - p).Cross(r);

            // If r x s = 0 and (q - p) x r = 0, then the two lines are collinear.
            if (rxs.IsZero() && qpxr.IsZero())
            {
                // 1. If either  0 <= (q - p) * r <= r * r or 0 <= (p - q) * s <= * s
                // then the two lines are overlapping,
                if (considerCollinearOverlapAsIntersect)
                    if ((0 <= (q - p) * r && (q - p) * r <= r * r) || (0 <= (p - q) * s && (p - q) * s <= s * s))
                        return true;

                // 2. If neither 0 <= (q - p) * r = r * r nor 0 <= (p - q) * s <= s * s
                // then the two lines are collinear but disjoint.
                // No need to implement this expression, as it follows from the expression above.
                return false;
            }

            // 3. If r x s = 0 and (q - p) x r != 0, then the two lines are parallel and non-intersecting.
            if (rxs.IsZero() && !qpxr.IsZero())
                return false;

            // t = (q - p) x s / (r x s)
            var t = (q - p).Cross(s) / rxs;

            // u = (q - p) x r / (r x s)

            var u = (q - p).Cross(r) / rxs;

            // 4. If r x s != 0 and 0 <= t <= 1 and 0 <= u <= 1
            // the two line segments meet at the point p + t r = q + u s.
            if (!rxs.IsZero() && (0 <= t && t <= 1) && (0 <= u && u <= 1))
            {
                // We can calculate the intersection point using either t or u.
                intersection = p + t * r;

                // An intersection was found.
                return true;
            }

            // 5. Otherwise, the two line segments are not parallel but do not intersect.
            return false;
        }


    }

    /// <summary>
    /// Enum for the types of game supported in terms of number of users.
    /// </summary>
    public enum enGameType
    {
        TwoPlayer=2,
        ThreePlayer=3,
        FourPlayer=4
    }

    /// <summary>
    /// Enum for the color of tokens which denote users too.
    /// </summary>
    public enum enGameToken
    {
        Green=0,
        Red=1,
        Blue=2,
        Yellow=3
    }

    /// <summary>
    /// This class is used to conver the Bitmap value to BitmapImage.
    /// </summary>
    public class WPFBitmapConverter
    {
        /// <summary>
        /// This method converts the Bitmap value to BitmapImage.
        /// </summary>
        /// <param name="value">The Bitmap object.</param>
        /// <returns>The BitmapImage object.</returns>
        public static object Convert(object value)
        {
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            ((System.Drawing.Bitmap)value).Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
            BitmapImage image = new BitmapImage();
            image.BeginInit();
            ms.Seek(0, System.IO.SeekOrigin.Begin);
            image.StreamSource = ms;
            image.EndInit();

            return image;
        }

    }

}
