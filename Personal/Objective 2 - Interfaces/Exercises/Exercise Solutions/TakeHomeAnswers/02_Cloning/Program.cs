using System;
using System.Collections.Generic;
using System.Text;

namespace Cloning
{
    internal class Square : ICloneable
    {
        private double _side;

        internal Square(double side)
        {
            Side = side;
        }

        internal double Side
        {
            get { return _side; }
            set { _side = value; }
        }

        internal double Perimeter
        {
            get { return (Side * 4); }
        }

        internal double Area
        {
            get { return (Side * Side); }
        }

        public object Clone()
        {
            Square s = new Square(this.Side);
            return s;
        }
    }

    internal class Circle : ICloneable
    {
        private double _radius;

        internal Circle(double radius)
        {
            Radius = radius;
        }

        internal double Radius
        {
            get { return _radius; }
            set { _radius = value; }
        }

        internal double Circumference
        {
            get { return ((Radius * 2) * Math.PI); }
        }

        internal double Area
        {
            get { return ((Radius * Radius) * Math.PI); }
        }

        public object Clone()
        {
            Circle c = new Circle(this.Radius);
            return c;
        }
    }

    class TestCloning
    {
        private static object Copy(ICloneable source)
        {
            object destination = source.Clone();
            return destination;
        }

        static void Main()
        {
            // Create a Square object.
            Square s1 = new Square(21.75);
            Console.WriteLine("Original square: \n\tside = {0} \n\tperimeter = {1} \n\tarea = {2}", s1.Side, s1.Perimeter, s1.Area);

            // Copy the Square object to a new object.
            Square s2 = (Square) Copy(s1);
            Console.WriteLine("Copied square: \n\tside = {0} \n\tperimeter = {1} \n\tarea = {2}", s2.Side, s2.Perimeter, s2.Area);

            Console.WriteLine();

            // Modify the side of the second square and display them both again.
            s2.Side = 24.25;
            Console.WriteLine("Original square: \n\tside = {0} \n\tperimeter = {1} \n\tarea = {2}", s1.Side, s1.Perimeter, s1.Area);
            Console.WriteLine("Copied square: \n\tside = {0} \n\tperimeter = {1} \n\tarea = {2}", s2.Side, s2.Perimeter, s2.Area);

            Console.WriteLine();

            // Create a Circle object
            Circle c1 = new Circle(9.8723472);
            Console.WriteLine("Original circle: \n\tradius = {0} \n\tcircumference = {1} \n\tarea = {2}", c1.Radius, c1.Circumference, c1.Area);

            // Copy the Circle object to a new object.
            Circle c2 = (Circle)Copy(c1);
            Console.WriteLine("Copied circle: \n\tradius = {0} \n\tcircumference = {1} \n\tarea = {2}", c2.Radius, c2.Circumference, c2.Area);

            Console.WriteLine();

            // Modify the radius of the second circle and display them both again.
            c2.Radius = 14.172371;
            Console.WriteLine("Original circle: \n\tradius = {0} \n\tcircumference = {1} \n\tarea = {2}", c1.Radius, c1.Circumference, c1.Area);
            Console.WriteLine("Copied circle: \n\tradius = {0} \n\tcircumference = {1} \n\tarea = {2}", c2.Radius, c2.Circumference, c2.Area);
           
            Console.Write("\nPress <ENTER> to end: ");
            Console.ReadLine();
        }
    }
}
