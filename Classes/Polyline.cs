using MathUtil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;

namespace Phase_1
{
	class Polyline : CartoObj, IPointy, IComparable<Polyline>, IEquatable<Polyline>
	{
		#region Variables Membres
		private List<Coordonnees> coord;
		private Color color;
		private int girth;
		private int nbPoints;
		#endregion

		#region Constructeurs
		public Polyline() : base()
		{
			coord = new List<Coordonnees>();
			color = Colors.Lime;
			girth = 1;
			nbPoints = coord.Count;
		}

		public Polyline(List<Coordonnees> coor, Color col, int gir) : this()
		{
			coord = coor;
			color = col;
			girth = gir;
			nbPoints = coord.Count;
		}
		#endregion

		#region Fonctions
		public List<Coordonnees> GetCoordonnees()
		{
			return coord;
		}
		public int GetNbPoints()
		{
			nbPoints = coord.Count;
			return coord.Count;
		}
		public override bool IsPointClose(double x, double y, double precision)
		{
			bool ret = false;

			List<Coordonnees> oTemp = this.GetCoordonnees();

			double xMin = double.MaxValue, xMax = double.MinValue;
			double yMin = double.MaxValue, yMax = double.MinValue;

			foreach (Coordonnees co in oTemp)
			{
				if (co.X < xMin) xMin = co.X;
				if (co.X > xMax) xMax = co.X;

				if (co.Y < yMin) yMin = co.Y;
				if (co.Y > yMax) yMax = co.Y;
			}

			if (x >= xMin - precision && x <= xMax + precision && y >= yMin - precision && y <= yMax + precision) ret = true;

			return ret;
		}
		
		public double GetSurface()
		{
			PolyMath lib = new PolyMath();
			List<Coordonnees> oTemp = this.GetCoordonnees();

			double xMin = double.MaxValue, xMax = double.MinValue;
			double yMin = double.MaxValue, yMax = double.MinValue;

			foreach (Coordonnees co in oTemp)
			{
				if (co.X < xMin) xMin = co.X;
				if (co.X > xMax) xMax = co.X;

				if (co.Y < yMin) yMin = co.Y;
				if (co.Y > yMax) yMax = co.Y;
			}

			return (lib.GetDistPP(xMin, yMin, xMax, yMin) * lib.GetDistPP(xMin, yMin, xMin, yMax));
		}
		public override void Draw()
		{
			Console.WriteLine(this.ToString());
		}

		public int CompareTo(Polyline l2)
		{
			PolyMath lib = new PolyMath();

			List<Coordonnees> cTemp = new List<Coordonnees>();
			cTemp = (GetCoordonnees()).ToList();
			cTemp.Sort();

			double taille1 = 0;

			for (int i = 0; i < cTemp.Count - 1; i++)
				taille1 += lib.GetDistPP(cTemp[i].X, cTemp[i].Y, cTemp[i + 1].X, cTemp[i + 1].Y);

			cTemp = (l2.GetCoordonnees()).ToList();
			cTemp.Sort();

			double taille2 = 0;

			for (int i = 0; i < cTemp.Count - 1; i++)
				taille2 += lib.GetDistPP(cTemp[i].X, cTemp[i].Y, cTemp[i + 1].X, cTemp[i + 1].Y);

			if (taille1.Equals(taille2)) return 0;
			if (taille1 > taille2) return 1;
			if (taille1 < taille2) return -1;

			return -2;
		}
		#endregion

		#region Overloading
		public override string ToString()
		{
			string coor = "";

			for(int i = 0; i < coord.Count; i++)
				coor += coord[i].ToString();

			return base.ToString() + " Couleur :" + color + " Epaisseur :" + girth + "px" + " nBPoint : " + nbPoints + "\nCoord : { " + coor + " }" + "\tSruface : " + this.GetSurface();
		}

		public static bool operator <(Polyline c1, Polyline c2)
		{
			return c1.CompareTo(c2) < 0;
		}

		public static bool operator >(Polyline c1, Polyline c2)
		{
			return c1.CompareTo(c2) > 0;
		}

		public static bool operator ==(Polyline c1, Polyline c2)
		{
			return c1.CompareTo(c2) == 0;
		}

		public static bool operator !=(Polyline c1, Polyline c2)
		{
			return c1.CompareTo(c2) != 0;
		}

		public bool Equals(Polyline c2)
		{
			if (c2 == null) return false;
			return this.CompareTo(c2) == 0;
		}
		#endregion

		#region Get/Set
		public int NbPoints
		{
			get { return nbPoints; }
		}
		#endregion
	}
}
