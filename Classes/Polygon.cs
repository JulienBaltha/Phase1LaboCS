using System;
using System.Collections.Generic;
using System.Windows.Media;

namespace Phase_1
{
	class Polygon : CartoObj, IPointy
	{
		#region Variables Membres
		private List<Coordonnees> coord;
		private Color BgColor;
		private Color LineColor;
		private double opacity;
		private int nbPoints;
		#endregion

		#region Constructeurs
		public Polygon() : base()
		{
			coord = new List<Coordonnees>();
			BgColor = Colors.Cyan;
			LineColor = Colors.DarkSeaGreen;
			opacity = 1;
			nbPoints = coord.Count;
		}

		public Polygon(List<Coordonnees> coor, Color Bg, Color Line, double op) : base()
		{
			coord = coor;
			BgColor = Bg;
			LineColor = Line;
			opacity = op;
		}

		public Polygon(List<Coordonnees> coor, double op) : base()
		{
			coord = coor;
			opacity = op;
		}
		#endregion

		#region Fonctions
		public List<Coordonnees> GetCoordonnees()
		{
			nbPoints = coord.Count;
			return coord;
		}

		public override void Draw()
		{
			Console.WriteLine(this.ToString());
		}

		public int GetNbPoints()
		{
			nbPoints = coord.Count;
			return coord.Count;
		}

		public void AddCoord(double x, double y)
		{
			Coordonnees c = new Coordonnees(x, y);
			coord.Add(c);
		}

		public void AddCoord(Coordonnees c)
		{
			coord.Add(c);
		}

		public override bool IsPointClose(double x, double y, double precision)
		{
			//								V1
			bool ret = false;

			List<Coordonnees> oTemp = this.GetCoordonnees();

			int j = oTemp.Count - 1;
			for (int i = 0; i < oTemp.Count; i++)
			{
				if (oTemp[i].Y < y && oTemp[j].Y >= y || oTemp[j].Y < y && oTemp[i].Y >= y)
				{
					if (oTemp[i].X + (y - oTemp[i].Y) / (oTemp[j].Y - oTemp[i].Y) * (oTemp[j].X - oTemp[i].X) < x)
					{
						ret = !ret;
					}
				}
				j = i;
			}

			return ret;

			//								V2 MR. WAGNER
			/*List<Coordonnees> ListeCoord = coord;
			ListeCoord.Sort();

			double maxX = double.MinValue;
			double minX = double.MaxValue;

			double maxY = double.MinValue;
			double minY = double.MaxValue;

			foreach (Coordonnees co in ListeCoord)
			{
				if (maxX < co.X) maxX = co.X;
				if (minX > co.X) minX = co.X;

				if (maxY < co.Y) maxY = co.Y;
				if (minY > co.Y) minY = co.Y;
			}
				

			Coordonnees[] rect = { new Coordonnees(minX, maxY), new Coordonnees(maxX, maxY), new Coordonnees(maxX, minY), new Coordonnees(minX, minY)};

			Coordonnees Cab = new Coordonnees(); //(yB−yA)x + (xA−xB)y + (xByA−xAyB)
			Coordonnees Cac = new Coordonnees(); //(yC−yA)x + (xA−xC)y + (xCyA−xAyC)
			Coordonnees Cbc = new Coordonnees(); //(yC−yB)x + (xB−xC)y + (xCyB−xByC)

			Coordonnees a = new Coordonnees();
			Coordonnees b = new Coordonnees();
			Coordonnees c = new Coordonnees();

			Coordonnees p = new Coordonnees(x, y);

			for(int i = 0; i < ListeCoord.Count; i++)
			{
				a = ListeCoord[i];

				if (i + 1 > ListeCoord.Count) b = ListeCoord[(i-ListeCoord.Count()) + 1]; 
				else b = ListeCoord[i + 1];

				if (i + 2 > ListeCoord.Count) c = ListeCoord[(i - ListeCoord.Count()) + 2];
				else c = ListeCoord[i + 2];

				Cab.X = (b.Y - a.Y);
				Cab.Y = -((a.X - b.X) + ((b.X * a.Y) - (a.X * b.Y)));

				Cac.X = (c.Y - a.Y);
				Cac.Y = -((a.X - c.X) + ((c.X*a.Y) - (b.X*c.Y)));

				Cbc.X = (c.Y - b.Y);
				Cbc.Y = -((b.X - c.X) + ((c.Y*b.Y) - (b.Y*c.Y)));

				Console.WriteLine(Cab);
				Console.WriteLine(Cac);
				Console.WriteLine(Cbc);

				Console.WriteLine("NbTour : " + i + "\tCount : " + ListeCoord.Count);
				Console.WriteLine("p*Cab : " + ((p.X * Cab.X) + (p.Y * Cab.Y)));
				Console.WriteLine("p*Cac : " + ((p.X * Cac.X) + (p.Y * Cac.Y)));
				Console.WriteLine("p*Cbc : " + ((p.X * Cbc.X) + (p.Y * Cbc.Y)));

				if ((Math.Sign(((p.X * Cab.X) + (p.Y * Cab.Y))) ==  Math.Sign(x) && Math.Sign(((p.X * Cab.X) + (p.Y * Cab.Y))) == Math.Sign(y)) ||
					(Math.Sign(((p.X * Cac.X) + (p.Y * Cac.Y))) == Math.Sign(x) && Math.Sign(((p.X * Cac.X) + (p.Y * Cac.Y))) == Math.Sign(y)) ||
					(Math.Sign(((p.X * Cbc.X) + (p.Y * Cbc.Y))) == Math.Sign(x) && Math.Sign(((p.X * Cbc.X) + (p.Y * Cbc.Y))) == Math.Sign(y)))
						return false;
			}

			return true;*/
		}

		public double GetSurfaceArea()
		{
			// Add the first point to the end.
			int num_points = coord.Count - 1;
			Coordonnees[] pts = new Coordonnees[num_points];
			pts = coord.ToArray();

			// Get the areas.
			double area = 0;
			for (int i = 0; i < num_points; i++)
				area += (pts[i + 1].X - pts[i].X) * (pts[i + 1].Y + pts[i].Y) / 2;

			// Return the result.
			return Math.Abs(area);
		}
		#endregion

		#region Overloading
		public override string ToString()
		{
			string coor = "";

			for (int i = 0; i < coord.Count; i++)
				coor += coord[i].ToString();

			return base.ToString() + " Couleur Bg :" + BgColor + " Couleur Ligne :" + LineColor + " Opacite :" + opacity + "px" + " nBPoint : " + nbPoints + "\nCoord : { " + coor + " }";
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
