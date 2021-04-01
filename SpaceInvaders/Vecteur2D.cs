using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpaceInvaders
{
    class Vecteur2D
    {
        public double x;
        public double y;
        public double Norme { get; }

        public Vecteur2D(double x=0, double y=0)
        {
            this.x = x;
            this.y = y;
        }
        
        public static Vecteur2D operator +(Vecteur2D v1, Vecteur2D v2)
        { return new Vecteur2D(v1.x + v2.x, v1.y + v2.y); }
        public static Vecteur2D operator -(Vecteur2D v1, Vecteur2D v2)
        { return new Vecteur2D(v1.x - v2.x, v1.y - v2.y); }
        public static Vecteur2D operator -(Vecteur2D v)
        { return new Vecteur2D(-v.x, -v.y); }
        public static Vecteur2D operator *(Vecteur2D v, double k)
        { return new Vecteur2D(k*v.x, k*v.y); }
        public static Vecteur2D operator *(double k, Vecteur2D v)
        { return new Vecteur2D(k*v.x, k*v.y); }
        public static Vecteur2D operator /(Vecteur2D v, double k)
        { return new Vecteur2D(v.x/k, v.y/k); }

    }
}
