using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Shapes;

namespace Snakes_and_Ladders
{
    public static class VectorExtension
    {
        public static double Cross(this Vector vx, Vector v)
        {
            return vx.X * v.Y - vx.Y * v.X;
        }

        private const double Epsilon = 1e-10;

        public static bool IsZero(this double d)
        {
            return Math.Abs(d) < Epsilon;
        }

        public static Point Add(this Point vp, Point p)
        {
            Point retP = new Point();
            retP.X = vp.X + p.X;
            retP.Y = vp.Y + p.Y;
            return retP;
        }

        public static Point Subtract(this Point vp, double d)
        {
            vp.X = vp.X - d;
            vp.Y = vp.Y - d;
            return vp;
        }

        public static Point Add(this Point vp, double d)
        {
            vp.X = vp.X + d;
            vp.Y = vp.Y + d;
            return vp;
        }

        public static Point DividedBy(this Point vp, double d)
        {
            vp.X = vp.X / d;
            vp.Y = vp.Y / d;
            return vp;
        }

        public static void AddSnake(this System.Windows.Controls.Canvas cnv, Shapes.Snake snake)
        {
            cnv.Children.Add(snake);
            cnv.Children.Add(snake.Eye1);
            cnv.Children.Add(snake.Eye2);
            cnv.Children.Add(snake.Tongue);
            cnv.Children.Add(snake.Tongue2);
        }

        public static void RemoveSnake(this System.Windows.Controls.Canvas cnv, Shapes.Snake snake)
        {
            cnv.Children.Remove(snake);
            cnv.Children.Remove(snake.Eye1);
            cnv.Children.Remove(snake.Eye2);
            cnv.Children.Remove(snake.Tongue);
            cnv.Children.Remove(snake.Tongue2);
        }
    }
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

        //public static Path GenerateRandomPath(Point start, Point end, double prob)
        //{
        //    Path newpath = new Path();
        //    Random rnd = new Random();

        //    int curx = startx; int cury = starty; Direction curd = Direction.Right;
        //    Direction newd = curd;

        //    while (!(curx == endx && cury == endy))
        //    {
        //        if (rnd.NextDouble() <= prob) // let's generate a turn
        //        {

        //            do
        //            {
        //                if (curx == endx) newd = GetNewDirection(Direction.Left | Direction.Down, rnd);
        //                else if (cury == endy) newd = Direction.Right;
        //                else if (curx <= 0) newd = GetNewDirection(Direction.Right | Direction.Down, rnd);
        //                else newd = GetNewDirection(Direction.Right | Direction.Down | Direction.Left, rnd);

        //            }
        //            while ((newd | curd) == (Direction.Left | Direction.Right)); // excluding going back

        //            newpath.Add(newd);
        //            curd = newd;
        //            switch (newd)
        //            {
        //                case Direction.Left:
        //                    curx--;
        //                    break;
        //                case Direction.Right:
        //                    curx++;
        //                    break;
        //                case Direction.Down:
        //                    cury++;
        //                    break;
        //            }
        //        }

        //    }


        //    return newpath;
        //}
    }
}
