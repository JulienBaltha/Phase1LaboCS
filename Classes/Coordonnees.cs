using System;
using System.Collections;

namespace Phase_1
{
	internal class Coordonnees : CartoObj, IComparable
	{
		#region Variables Membres
		protected double x;
		protected double y;
		#endregion

		#region Constructeurs
		public Coordonnees() : base()
		{
			x = 0;
			y = 0;
		}

		public Coordonnees(double nX, double nY) : this()
		{
			x = nX;
			y = nY;
		}
		#endregion

		#region Fonction
		public int CompareTo(object c)
		{
			Coordonnees c2 = c as Coordonnees;

			if (this.X < c2.X)					// x <
				return -1;

			if (this.X == c2.X)                 // x =
				if (this.Y < c2.Y)                  // y <
					return -1;
				else
					if (this.Y == c2.Y)				// y =
						return 0;
				else
					if (this.Y > c2.Y)				// y >
						return 1;

			if (this.X > c2.X)					// x >
				return 1;

			return -2;
		}

		public int CompareTo(int c)
		{
			if (this.X < c)                  // x <
				return -1;

			if (this.X == c)                 // x =
				if (this.Y < c)                  // y <
					return -1;
				else
					if (this.Y == c)             // y =
					return 0;
				else
					if (this.Y > c)              // y >
					return 1;

			if (this.X > c)                  // x >
				return 1;

			return -2;
		}
		#endregion

		#region Get/Set
		public double X
		{
			get { return x; }
			set { x = value; }
		}

		public double Y
		{
			get { return y; }
			set { y = value; }
		}
		#endregion

		#region Overloading
		public override string ToString()
		{
			return base.ToString() + " (" + x.ToString("#.000") + " , " + y.ToString("#.000") + ")";
		}

		public static bool operator <(Coordonnees c1, Coordonnees c2)
		{
			return c1.CompareTo(c2) < 0;
		}

		public static bool operator >(Coordonnees c1, Coordonnees c2)
		{
			return c1.CompareTo(c2) > 0;
		}

		public static bool operator <(Coordonnees c1, int c2)
		{
			return c1.CompareTo(c2) < 0;
		}

		public static bool operator >(Coordonnees c1, int c2)
		{
			return c1.CompareTo(c2) > 0;
		}

		public static Coordonnees operator *(Coordonnees c1, Coordonnees c2)
		{
			Coordonnees cT = new Coordonnees(c1.X * c2.X, c1.Y * c2.Y);

			return cT;
		}
		#endregion
	}
}