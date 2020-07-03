using System;

public struct Intvl
{

    //Type intval voor het houden van de values voor intersections
    public double a;
    public double b;

    public Intvl(double f) { a = b = f; }
    public Intvl(double x, double y) { this.a = x; this.b = y; }
    public Intvl(double[] v2) { a = v2[0]; b = v2[1]; }
    public Intvl(float f) { a = b = f; }
    public Intvl(float x, float y) { this.a = x; this.b = y; }
    public Intvl(float[] v2) { a = v2[0]; b = v2[1]; }
    public Intvl(Intvl copy) { a = copy.a; b = copy.b; }


    static public readonly Intvl Zero = new Intvl(0.0f, 0.0f);
    static public readonly Intvl Empty = new Intvl(double.MaxValue, -double.MaxValue);
    static public readonly Intvl Infinite = new Intvl(-double.MaxValue, double.MaxValue);


    public static Intvl Unsorted(double x, double y)
    {
        return (x < y) ? new Intvl(x, y) : new Intvl(y, x);
    }

    public double this[int key]
    {
        get { return (key == 0) ? a : b; }
        set { if (key == 0) a = value; else b = value; }
    }


    public double LengthSquared
    {
        get { return (a - b) * (a - b); }
    }
    public double Length
    {
        get { return b - a; }
    }
    public bool IsConstant
    {
        get { return b == a; }
    }

    public double Center
    {
        get { return (b + a) * 0.5; }
    }

    public void Contain(double d)
    {
        if (d < a)
            a = d;
        if (d > b)
            b = d;
    }

    public bool Contains(double d)
    {
        return d >= a && d <= b;
    }


    public bool Overlaps(Intvl o)
    {
        return !(o.a > b || o.b < a);
    }

    //Ik denk dat het zo moet wrken... maybe
    public double SquaredDist(Intvl o)
    {
        if (b < o.a)
            return (o.a - b) * (o.a - b);
        else if (a > o.b)
            return (a - o.b) * (a - o.b);
        else
            return 0;
    }
    public double Dist(Intvl o)
    {
        if (b < o.a)
            return o.a - b;
        else if (a > o.b)
            return a - o.b;
        else
            return 0;
    }

    public Intvl IntersectionWith(ref Intvl o)
    {
        if (o.a > b || o.b < a)
            return Intvl.Empty;
        return new Intvl(Math.Max(a, o.a), Math.Min(b, o.b));
    }

    public double Clamp(double f)
    {
        return (f < a) ? a : (f > b) ? b : f;
    }

    public double Interpolate(double t)
    {
        return (1 - t) * a + (t) * b;
    }

    //Dit is een beetje kuttig
    public double GetT(double value)
    {
        if (value <= a) return 0;
        else if (value >= b) return 1;
        else if (a == b) return 0.5;
        else return (value - a) / (b - a);
    }

    public void Set(Intvl o)
    {
        a = o.a; b = o.b;
    }
    public void Set(double fA, double fB)
    {
        a = fA; b = fB;
    }



    public static Intvl operator -(Intvl v)
    {
        return new Intvl(-v.a, -v.b);
    }


    public static Intvl operator +(Intvl a, double f)
    {
        return new Intvl(a.a + f, a.b + f);
    }
    public static Intvl operator -(Intvl a, double f)
    {
        return new Intvl(a.a - f, a.b - f);
    }

    public static Intvl operator *(Intvl a, double f)
    {
        return new Intvl(a.a * f, a.b * f);
    }


    public override string ToString()
    {
        return string.Format("[{0:F8},{1:F8}]", a, b);
    }


}