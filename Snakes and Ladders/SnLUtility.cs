using System;
using System.Collections;
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


        /// <summary>
        /// Converts the hashtable to dictionary.
        /// </summary>
        /// <typeparam name="TKey">Data type of the key.</typeparam>
        /// <typeparam name="TVal">Data type of the value.</typeparam>
        /// <param name="table">Input hashtable</param>
        /// <returns>Dictonary of type (TKey, TVal)</returns>
        public static Dictionary<TKey, TVal> ToDictionary<TKey, TVal>(this Hashtable table)
        {
            if (table == null)
                return null;
            else
                return table.Cast<DictionaryEntry>().ToDictionary(kvp => (TKey)kvp.Key, kvp => (TVal)kvp.Value);
        }

        /// <summary>
        /// This method is used to get the Brush object from string.
        /// Default return value is Black.
        /// Returns Cyan when you will expect aqua. Returns Magenta when you will expect Fuchsia.
        /// </summary>
        /// <param name="sColor">The string of the color.</param>
        /// <returns>The Brush with the matching string.</returns>
        public static System.Windows.Media.Brush ConvertToWindowsBrush(this string sColor)
        {
            switch (sColor)
            {
                case "#FFF0F8FF":
                    return Brushes.AliceBlue;
                case "#FFEEE8AA":
                    return Brushes.PaleGoldenrod;
                case "#FFDA70D6":
                    return Brushes.Orchid;
                case "#FFFF4500":
                    return Brushes.OrangeRed;
                case "#FFFFA500":
                    return Brushes.Orange;
                case "#FF6B8E23":
                    return Brushes.OliveDrab;
                case "#FF808000":
                    return Brushes.Olive;
                case "#FFFDF5E6":
                    return Brushes.OldLace;
                case "#FF000080":
                    return Brushes.Navy;
                case "#FFFFDEAD":
                    return Brushes.NavajoWhite;
                case "#FFFFE4B5":
                    return Brushes.Moccasin;
                case "#FFFFE4E1":
                    return Brushes.MistyRose;
                case "#FFF5FFFA":
                    return Brushes.MintCream;
                case "#FF191970":
                    return Brushes.MidnightBlue;
                case "#FFC71585":
                    return Brushes.MediumVioletRed;
                case "#FF48D1CC":
                    return Brushes.MediumTurquoise;
                case "#FF00FA9A":
                    return Brushes.MediumSpringGreen;
                case "#FF7B68EE":
                    return Brushes.MediumSlateBlue;
                case "#FF87CEFA":
                    return Brushes.LightSkyBlue;
                case "#FF778899":
                    return Brushes.LightSlateGray;
                case "#FFB0C4DE":
                    return Brushes.LightSteelBlue;
                case "#FFFFFFE0":
                    return Brushes.LightYellow;
                case "#FF00FF00":
                    return Brushes.Lime;
                case "#FF32CD32":
                    return Brushes.LimeGreen;
                case "#FF98FB98":
                    return Brushes.PaleGreen;
                case "#FFFAF0E6":
                    return Brushes.Linen;
                case "#FF800000":
                    return Brushes.Maroon;
                case "#FF66CDAA":
                    return Brushes.MediumAquamarine;
                case "#FF0000CD":
                    return Brushes.MediumBlue;
                case "#FFBA55D3":
                    return Brushes.MediumOrchid;
                case "#FF9370DB":
                    return Brushes.MediumPurple;
                case "#FF3CB371":
                    return Brushes.MediumSeaGreen;
                case "#FFFF00FF":
                    return Brushes.Magenta;
                case "#FFAFEEEE":
                    return Brushes.PaleTurquoise;
                case "#FFDB7093":
                    return Brushes.PaleVioletRed;
                case "#FFFFEFD5":
                    return Brushes.PapayaWhip;
                case "#FF708090":
                    return Brushes.SlateGray;
                case "#FFFFFAFA":
                    return Brushes.Snow;
                case "#FF00FF7F":
                    return Brushes.SpringGreen;
                case "#FF4682B4":
                    return Brushes.SteelBlue;
                case "#FFD2B48C":
                    return Brushes.Tan;
                case "#FF008080":
                    return Brushes.Teal;
                case "#FF6A5ACD":
                    return Brushes.SlateBlue;
                case "#FFD8BFD8":
                    return Brushes.Thistle;
                case "#00FFFFFF":
                    return Brushes.Transparent;
                case "#FF40E0D0":
                    return Brushes.Turquoise;
                case "#FFEE82EE":
                    return Brushes.Violet;
                case "#FFF5DEB3":
                    return Brushes.Wheat;
                case "#FFFFFFFF":
                    return Brushes.White;
                case "#FFF5F5F5":
                    return Brushes.WhiteSmoke;
                case "#FFFF6347":
                    return Brushes.Tomato;
                case "#FF20B2AA":
                    return Brushes.LightSeaGreen;
                case "#FF87CEEB":
                    return Brushes.SkyBlue;
                case "#FFA0522D":
                    return Brushes.Sienna;
                case "#FFFFDAB9":
                    return Brushes.PeachPuff;
                case "#FFCD853F":
                    return Brushes.Peru;
                case "#FFFFC0CB":
                    return Brushes.Pink;
                case "#FFDDA0DD":
                    return Brushes.Plum;
                case "#FFB0E0E6":
                    return Brushes.PowderBlue;
                case "#FF800080":
                    return Brushes.Purple;
                case "#FFC0C0C0":
                    return Brushes.Silver;
                case "#FFFF0000":
                    return Brushes.Red;
                case "#FF4169E1":
                    return Brushes.RoyalBlue;
                case "#FF8B4513":
                    return Brushes.SaddleBrown;
                case "#FFFA8072":
                    return Brushes.Salmon;
                case "#FFF4A460":
                    return Brushes.SandyBrown;
                case "#FF2E8B57":
                    return Brushes.SeaGreen;
                case "#FFFFF5EE":
                    return Brushes.SeaShell;
                case "#FFBC8F8F":
                    return Brushes.RosyBrown;
                case "#FFFFFF00":
                    return Brushes.Yellow;
                case "#FFFFA07A":
                    return Brushes.LightSalmon;
                case "#FF90EE90":
                    return Brushes.LightGreen;
                case "#FF8B0000":
                    return Brushes.DarkRed;
                case "#FF9932CC":
                    return Brushes.DarkOrchid;
                case "#FFFF8C00":
                    return Brushes.DarkOrange;
                case "#FF556B2F":
                    return Brushes.DarkOliveGreen;
                case "#FF8B008B":
                    return Brushes.DarkMagenta;
                case "#FFBDB76B":
                    return Brushes.DarkKhaki;
                case "#FF006400":
                    return Brushes.DarkGreen;
                case "#FFA9A9A9":
                    return Brushes.DarkGray;
                case "#FFB8860B":
                    return Brushes.DarkGoldenrod;
                case "#FF008B8B":
                    return Brushes.DarkCyan;
                case "#FF00008B":
                    return Brushes.DarkBlue;
                case "#FF00FFFF":
                    return Brushes.Cyan;
                case "#FFDC143C":
                    return Brushes.Crimson;
                case "#FFFFF8DC":
                    return Brushes.Cornsilk;
                case "#FF6495ED":
                    return Brushes.CornflowerBlue;
                case "#FFFF7F50":
                    return Brushes.Coral;
                case "#FFD2691E":
                    return Brushes.Chocolate;
                case "#FFFAEBD7":
                    return Brushes.AntiqueWhite;
                case "#FF7FFFD4":
                    return Brushes.Aquamarine;
                case "#FFF0FFFF":
                    return Brushes.Azure;
                case "#FFF5F5DC":
                    return Brushes.Beige;
                case "#FFFFE4C4":
                    return Brushes.Bisque;
                case "#FFE9967A":
                    return Brushes.DarkSalmon;
                case "#FF000000":
                    return Brushes.Black;
                case "#FF0000FF":
                    return Brushes.Blue;
                case "#FF8A2BE2":
                    return Brushes.BlueViolet;
                case "#FFA52A2A":
                    return Brushes.Brown;
                case "#FFDEB887":
                    return Brushes.BurlyWood;
                case "#FF5F9EA0":
                    return Brushes.CadetBlue;
                case "#FF7FFF00":
                    return Brushes.Chartreuse;
                case "#FFFFEBCD":
                    return Brushes.BlanchedAlmond;
                case "#FF8FBC8F":
                    return Brushes.DarkSeaGreen;
                case "#FF483D8B":
                    return Brushes.DarkSlateBlue;
                case "#FF2F4F4F":
                    return Brushes.DarkSlateGray;
                case "#FFFF69B4":
                    return Brushes.HotPink;
                case "#FFCD5C5C":
                    return Brushes.IndianRed;
                case "#FF4B0082":
                    return Brushes.Indigo;
                case "#FFFFFFF0":
                    return Brushes.Ivory;
                case "#FFF0E68C":
                    return Brushes.Khaki;
                case "#FFF0FFF0":
                    return Brushes.Honeydew;
                case "#FFE6E6FA":
                    return Brushes.Lavender;
                case "#FFFFF0F5":
                    return Brushes.LavenderBlush;
                case "#FFFFFACD":
                    return Brushes.LemonChiffon;
                case "#FFADD8E6":
                    return Brushes.LightBlue;
                case "#FFF08080":
                    return Brushes.LightCoral;
                case "#FFE0FFFF":
                    return Brushes.LightCyan;
                case "#FFFAFAD2":
                    return Brushes.LightGoldenrodYellow;
                case "#FFD3D3D3":
                    return Brushes.LightGray;
                case "#FF7CFC00":
                    return Brushes.LawnGreen;
                case "#FFFFB6C1":
                    return Brushes.LightPink;
                case "#FFADFF2F":
                    return Brushes.GreenYellow;
                case "#FF808080":
                    return Brushes.Gray;
                case "#FF00CED1":
                    return Brushes.DarkTurquoise;
                case "#FF9400D3":
                    return Brushes.DarkViolet;
                case "#FFFF1493":
                    return Brushes.DeepPink;
                case "#FF00BFFF":
                    return Brushes.DeepSkyBlue;
                case "#FF696969":
                    return Brushes.DimGray;
                case "#FF1E90FF":
                    return Brushes.DodgerBlue;
                case "#FF008000":
                    return Brushes.Green;
                case "#FFB22222":
                    return Brushes.Firebrick;
                case "#FF228B22":
                    return Brushes.ForestGreen;
                case "#FFDCDCDC":
                    return Brushes.Gainsboro;
                case "#FFF8F8FF":
                    return Brushes.GhostWhite;
                case "#FFFFD700":
                    return Brushes.Gold;
                case "#FFDAA520":
                    return Brushes.Goldenrod;
                case "#FFFFFAF0":
                    return Brushes.FloralWhite;
                case "#FF9ACD32":
                    return Brushes.YellowGreen;
                default:
                    return Brushes.Black;

            }
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
