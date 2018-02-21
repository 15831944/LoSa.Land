using System;


namespace LoSa.Land.ObjectIn4
{
    public class Point
    {
        public static Point ZERO = new Point(0, "", 0, 0, 0, 0);

        public int Number { get; set; }
        public String Name { get; set; }

        public double X { get; set; }
        public double Y { get; set; }

        public double Mx { get; set; }
        public double My { get; set; }

        public Point() 
        {

        }

        public Point(double x, double y)
        {
            this.X = x;
            this.Y = y;
        }

        public Point(int number, String name, double x, double y) 
        {
            this.X = x;
            this.Y = y;
            this.Number = number;
            this.Name = name;
        }

        public Point(int number, String name, double x, double y, double mx, double my)
        {
            this.X = x;
            this.Y = y;
            this.Number = number;
            this.Name = name;
            this.Mx = mx;
            this.My = my;
        }

        public double Distance(Point point)
        {
            double dx = point.X - this.X;
            double dy = point.Y - this.Y;
            return Math.Sqrt(dx*dx + dy*dy);
        }
    }


}
