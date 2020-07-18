using System;
using System;
using System.Collections.Generic;
using System.Text;

using UnityEngine;

public struct NewVector3 : IComparable<NewVector3>, IEquatable<NewVector3>
{
    public float x;
    public float y;
    public float z;

    public NewVector3(float f) { x = y = z = f; }
    public NewVector3(float x, float y, float z) { this.x = x; this.y = y; this.z = z; }
    public NewVector3(float[] v2) { x = v2[0]; y = v2[1]; z = v2[2]; }
    public NewVector3(NewVector3 copy) { x = copy.x; y = copy.y; z = copy.z; }

    public NewVector3(double f) { x = y = z = (float)f; }
    public NewVector3(double x, double y, double z) { this.x = (float)x; this.y = (float)y; this.z = (float)z; }
    public NewVector3(double[] v2) { x = (float)v2[0]; y = (float)v2[1]; z = (float)v2[2]; }
    //public NewVector3(NewVector3 copy) { x = (float)copy.x; y = (float)copy.y; z = (float)copy.z; }

    static public readonly NewVector3 Zero = new NewVector3(0.0f, 0.0f, 0.0f);
    static public readonly NewVector3 One = new NewVector3(1.0f, 1.0f, 1.0f);
    static public readonly NewVector3 OneNormalized = new NewVector3(1.0f, 1.0f, 1.0f).Normalized;
    static public readonly NewVector3 Invalid = new NewVector3(float.MaxValue, float.MaxValue, float.MaxValue);
    static public readonly NewVector3 AxisX = new NewVector3(1.0f, 0.0f, 0.0f);
    static public readonly NewVector3 AxisY = new NewVector3(0.0f, 1.0f, 0.0f);
    static public readonly NewVector3 AxisZ = new NewVector3(0.0f, 0.0f, 1.0f);
    static public readonly NewVector3 MaxValue = new NewVector3(float.MaxValue, float.MaxValue, float.MaxValue);
    static public readonly NewVector3 MinValue = new NewVector3(float.MinValue, float.MinValue, float.MinValue);

    public float this[int key]
    {
        get { return (key == 0) ? x : (key == 1) ? y : z; }
        set { if (key == 0) x = value; else if (key == 1) y = value; else z = value; }
    }

    public NewVector3 xy
    {
        get { return new NewVector3(x, y,z); }
        set { x = value.x; y = value.y; }
    }
    public NewVector3 xz
    {
        get { return new NewVector3(x, y,z); }
        set { x = value.x; z = value.y; }
    }
    public NewVector3 yz
    {
        get { return new NewVector3(x,y, z); }
        set { y = value.x; z = value.y; }
    }

    public float LengthSquared
    {
        get { return x * x + y * y + z * z; }
    }
    public float Length
    {
        get { return (float)Math.Sqrt(LengthSquared); }
    }

    public float LengthL1
    {
        get { return Math.Abs(x) + Math.Abs(y) + Math.Abs(z); }
    }

    public float Max
    {
        get { return Math.Max(x, Math.Max(y, z)); }
    }
    public float Min
    {
        get { return Math.Min(x, Math.Min(y, z)); }
    }
    public float MaxAbs
    {
        get { return Math.Max(Math.Abs(x), Math.Max(Math.Abs(y), Math.Abs(z))); }
    }
    public float MinAbs
    {
        get { return Math.Min(Math.Abs(x), Math.Min(Math.Abs(y), Math.Abs(z))); }
    }


    public float Normalize(float epsilon)
    {
        float length = Length;
        if (length > epsilon)
        {
            float invLength = 1.0f / length;
            x *= invLength;
            y *= invLength;
            z *= invLength;
        }
        else
        {
            length = 0;
            x = y = z = 0;
        }
        return length;
    }
    public NewVector3 Normalized
    {
        get
        {
            float length = Length;
            
                float invLength = 1 / length;
                return new NewVector3(x * invLength, y * invLength, z * invLength);
   
        }
    }

    public bool IsNormalized
    {
        //ehhh, works?
        get { return Math.Abs((x * x + y * y + z * z) - 1) < 9990000; }
    }

    public bool IsFinite
    {
        get { float f = x + y + z; return float.IsNaN(f) == false && float.IsInfinity(f) == false; }
    }


    public void Round(int nDecimals)
    {
        x = (float)Math.Round(x, nDecimals);
        y = (float)Math.Round(y, nDecimals);
        z = (float)Math.Round(z, nDecimals);
    }


    public float Dot(NewVector3 v2)
    {
        return x * v2[0] + y * v2[1] + z * v2[2];
    }
    public static float Dot(NewVector3 v1, NewVector3 v2)
    {
        return v1.Dot(v2);
    }


    public NewVector3 Cross(NewVector3 v2)
    {
        return new NewVector3(
            y * v2.z - z * v2.y,
            z * v2.x - x * v2.z,
            x * v2.y - y * v2.x);
    }
    public static NewVector3 Cross(NewVector3 v1, NewVector3 v2)
    {
        return v1.Cross(v2);
    }

    public NewVector3 UnitCross(NewVector3 v2)
    {
        NewVector3 n = new NewVector3(
            y * v2.z - z * v2.y,
            z * v2.x - x * v2.z,
            x * v2.y - y * v2.x);
        return n;
    }

    public float AngleD(NewVector3 v2)
    {
        float fDot = Dot(v2);
        return (float)(Math.Acos(fDot));
    }
    public static float AngleD(NewVector3 v1, NewVector3 v2)
    {
        return v1.AngleD(v2);
    }
    public float AngleR(NewVector3 v2)
    {
        float fDot = Dot(v2);
        return (float)(Math.Acos(fDot));
    }
    public static float AngleR(NewVector3 v1, NewVector3 v2)
    {
        return v1.AngleR(v2);
    }


    public float DistanceSquared(NewVector3 v2)
    {
        float dx = v2.x - x, dy = v2.y - y, dz = v2.z - z;
        return dx * dx + dy * dy + dz * dz;
    }
    public float Distance(NewVector3 v2)
    {
        float dx = v2.x - x, dy = v2.y - y, dz = v2.z - z;
        return (float)Math.Sqrt(dx * dx + dy * dy + dz * dz);
    }



    public void Set(NewVector3 o)
    {
        x = o[0]; y = o[1]; z = o[2];
    }
    public void Set(float fX, float fY, float fZ)
    {
        x = fX; y = fY; z = fZ;
    }
    public void Add(NewVector3 o)
    {
        x += o[0]; y += o[1]; z += o[2];
    }
    public void Subtract(NewVector3 o)
    {
        x -= o[0]; y -= o[1]; z -= o[2];
    }



    public static NewVector3 operator -(NewVector3 v)
    {
        return new NewVector3(-v.x, -v.y, -v.z);
    }

    public static NewVector3 operator *(float f, NewVector3 v)
    {
        return new NewVector3(f * v.x, f * v.y, f * v.z);
    }
    public static NewVector3 operator *(NewVector3 v, float f)
    {
        return new NewVector3(f * v.x, f * v.y, f * v.z);
    }
    public static NewVector3 operator /(NewVector3 v, float f)
    {
        return new NewVector3(v.x / f, v.y / f, v.z / f);
    }
    public static NewVector3 operator /(float f, NewVector3 v)
    {
        return new NewVector3(f / v.x, f / v.y, f / v.z);
    }

    public static NewVector3 operator *(NewVector3 a, NewVector3 b)
    {
        return new NewVector3(a.x * b.x, a.y * b.y, a.z * b.z);
    }
    public static NewVector3 operator /(NewVector3 a, NewVector3 b)
    {
        return new NewVector3(a.x / b.x, a.y / b.y, a.z / b.z);
    }


    public static NewVector3 operator +(NewVector3 v0, NewVector3 v1)
    {
        return new NewVector3(v0.x + v1.x, v0.y + v1.y, v0.z + v1.z);
    }
    public static NewVector3 operator +(NewVector3 v0, float f)
    {
        return new NewVector3(v0.x + f, v0.y + f, v0.z + f);
    }

    public static NewVector3 operator -(NewVector3 v0, NewVector3 v1)
    {
        return new NewVector3(v0.x - v1.x, v0.y - v1.y, v0.z - v1.z);
    }
    public static NewVector3 operator -(NewVector3 v0, float f)
    {
        return new NewVector3(v0.x - f, v0.y - f, v0.z - f);
    }


    public static bool operator ==(NewVector3 a, NewVector3 b)
    {
        return (a.x == b.x && a.y == b.y && a.z == b.z);
    }
    public static bool operator !=(NewVector3 a, NewVector3 b)
    {
        return (a.x != b.x || a.y != b.y || a.z != b.z);
    }
    public override bool Equals(object obj)
    {
        return this == (NewVector3)obj;
    }
    public override int GetHashCode()
    {
        unchecked // Overflow is fine, just wrap
        {
            int hash = (int)2166136261;
            // Suitable nullity checks etc, of course :)
            hash = (hash * 16777619) ^ x.GetHashCode();
            hash = (hash * 16777619) ^ y.GetHashCode();
            hash = (hash * 16777619) ^ z.GetHashCode();
            return hash;
        }
    }
    public int CompareTo(NewVector3 other)
    {
        if (x != other.x)
            return x < other.x ? -1 : 1;
        else if (y != other.y)
            return y < other.y ? -1 : 1;
        else if (z != other.z)
            return z < other.z ? -1 : 1;
        return 0;
    }
    public bool Equals(NewVector3 other)
    {
        return (x == other.x && y == other.y && z == other.z);
    }


    public bool EpsilonEqual(NewVector3 v2, float epsilon)
    {
        return (float)Math.Abs(x - v2.x) <= epsilon &&
                (float)Math.Abs(y - v2.y) <= epsilon &&
                (float)Math.Abs(z - v2.z) <= epsilon;
    }


    public static NewVector3 Lerp(NewVector3 a, NewVector3 b, float t)
    {
        float s = 1 - t;
        return new NewVector3(s * a.x + t * b.x, s * a.y + t * b.y, s * a.z + t * b.z);
    }



    public override string ToString()
    {
        return string.Format("{0:F8} {1:F8} {2:F8}", x, y, z);
    }
    public string ToString(string fmt)
    {
        return string.Format("{0} {1} {2}", x.ToString(fmt), y.ToString(fmt), z.ToString(fmt));
    }






    public static implicit operator NewVector3(UnityEngine.Vector3 v)
    {
        return new NewVector3(v.x, v.y, v.z);
    }
    public static implicit operator Vector3(NewVector3 v)
    {
        return new Vector3(v.x, v.y, v.z);
    }
    public static implicit operator Color(NewVector3 v)
    {
        return new Color(v.x, v.y, v.z, 1.0f);
    }
    public static implicit operator NewVector3(Color c)
    {
        return new NewVector3(c.r, c.g, c.b);
    }

}

