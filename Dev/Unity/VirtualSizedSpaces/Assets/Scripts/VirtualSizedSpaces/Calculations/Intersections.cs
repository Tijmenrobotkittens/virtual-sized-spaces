using System;

public class Intersections
{

	public Intvl U;
	public Intvl V;


	//Class om intersections te checken bij lopen
	public int nmbint = 0;

	private Intvl Intvl = Intvl.Zero;

	public Intersections(double u0, double u1, double v0, double v1)
	{
		U = new Intvl(u0, u1);
		V = new Intvl(v0, v1);
	}
	public Intersections(Intvl u, Intvl v)
	{
		U = u;
		V = v;
	}

	public bool Test
	{
		get { return U.a <= V.b && U.b >= V.a; }
	}


	public double GetIntersection(int i)
	{
		return Intvl[i];
	}

	public bool Find()
	{
		if (U.b < V.a || U.a > V.b)
		{
			nmbint = 0;
		}
		else if (U.b > V.a)
		{
			if (U.a < V.b)
			{
				nmbint = 2;
				Intvl.a = (U.a < V.a ? V.a : U.a);
				Intvl.b = (U.b > V.b ? V.b : U.b);
				if (Intvl.a == Intvl.b)
				{
					nmbint = 1;
				}
			}
			else
			{
				nmbint = 1;
				Intvl.a = U.a;
			}
		}
		else
		{
			nmbint = 1;
			Intvl.a = U.b;
		}

		return nmbint > 0;
	}
}
