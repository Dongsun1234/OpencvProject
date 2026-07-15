using OpenCvSharp;

namespace BlazorApp2
{
    public class LineFit
    {
        public List<Point2f> FindEdgePoints(
        Mat gray,
        Point2f lineStart,
        Point2f lineEnd,
        int caliperCount = 25,
        int searchRadius = 25,
        int caliperHalfWidth = 4,
        double minGradient = 8)
        {
            var edgePoints = new List<Point2f>();

            var tangent = Normalize(lineEnd - lineStart);
            var normal = new Point2f(-tangent.Y, tangent.X);

            for (int i = 0; i < caliperCount; i++)
            {
                float t = (i + 0.5f) / caliperCount;
                var center = lineStart * (1 - t) + lineEnd * t;

                int profileSize = searchRadius * 2 + 1;
                double[] profile = new double[profileSize];

                int widthCount = 0;

                for (int w = -caliperHalfWidth; w <= caliperHalfWidth; w++)
                {
                    var widthCenter = center + tangent * w;

                    for (int d = -searchRadius; d <= searchRadius; d++)
                    {
                        var p = widthCenter + normal * d;
                        profile[d + searchRadius] += SampleBilinear(gray, p.X, p.Y);
                    }

                    widthCount++;
                }

                for (int k = 0; k < profile.Length; k++)
                    profile[k] /= widthCount;

                int bestIndex = -1;
                double bestGradient = 0;

                for (int k = 1; k < profile.Length - 1; k++)
                {
                    double gradient = (profile[k + 1] - profile[k - 1]) * 0.5;
                    double absGradient = Math.Abs(gradient);

                    if (absGradient > bestGradient)
                    {
                        bestGradient = absGradient;
                        bestIndex = k;
                    }
                }

                if (bestIndex >= 0 && bestGradient >= minGradient)
                {
                    int edgeOffset = bestIndex - searchRadius;
                    var edgePoint = center + normal * edgeOffset;
                    edgePoints.Add(edgePoint);
                }
            }

            return edgePoints;
        }

        public (Point2f Point, Point2f Direction) FitLinePca(List<Point2f> points)
        {
            double meanX = points.Average(p => p.X);
            double meanY = points.Average(p => p.Y);

            double sxx = 0;
            double syy = 0;
            double sxy = 0;

            foreach (var p in points)
            {
                double dx = p.X - meanX;
                double dy = p.Y - meanY;

                sxx += dx * dx;
                syy += dy * dy;
                sxy += dx * dy;
            }

            double theta = 0.5 * Math.Atan2(2 * sxy, sxx - syy);

            var direction = new Point2f(
                (float)Math.Cos(theta),
                (float)Math.Sin(theta));

            return (new Point2f((float)meanX, (float)meanY), direction);
        }

        private double SampleBilinear(Mat gray, double x, double y)
        {
            x = Math.Clamp(x, 0, gray.Width - 1);
            y = Math.Clamp(y, 0, gray.Height - 1);

            int x0 = (int)Math.Floor(x);
            int y0 = (int)Math.Floor(y);
            int x1 = Math.Min(x0 + 1, gray.Width - 1);
            int y1 = Math.Min(y0 + 1, gray.Height - 1);

            double dx = x - x0;
            double dy = y - y0;

            double v00 = gray.At<byte>(y0, x0);
            double v10 = gray.At<byte>(y0, x1);
            double v01 = gray.At<byte>(y1, x0);
            double v11 = gray.At<byte>(y1, x1);

            return
                v00 * (1 - dx) * (1 - dy) +
                v10 * dx * (1 - dy) +
                v01 * (1 - dx) * dy +
                v11 * dx * dy;
        }

        private Point2f Normalize(Point2f v)
        {
            float length = MathF.Sqrt(v.X * v.X + v.Y * v.Y);
            return new Point2f(v.X / length, v.Y / length);
        }
    }
}
