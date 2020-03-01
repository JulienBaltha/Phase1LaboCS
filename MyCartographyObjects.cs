using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using MathUtil;

namespace Phase_1
{
	class MyCartographyObjects
	{
		static void Main()
		{
			int choice = 0;

			List<Coordonnees> c1 = new List<Coordonnees> {  new Coordonnees(1, 0),
															new Coordonnees(2, 0),
															new Coordonnees(2.5, 1),
															new Coordonnees(3, 0),
															new Coordonnees(4, 0),
															new Coordonnees(2.5, 3) };

			List<Coordonnees> c2 = new List<Coordonnees> {  new Coordonnees(1, 0),
															new Coordonnees(5.63, 0),
															new Coordonnees(2.5, 1),
															new Coordonnees(3, 923.1),
															new Coordonnees(4, 0),
															new Coordonnees(124.8, 98.2) };

			List<Coordonnees> c3 = new List<Coordonnees> {  new Coordonnees(3, 923.1),
															new Coordonnees(5.63, 0),
															new Coordonnees(2.5, 7.253) };

			List<Coordonnees> c4 = new List<Coordonnees> {  new Coordonnees(3, 923.1),
															new Coordonnees(5.63, 0),
															new Coordonnees(2.5, 7.253) };

			List<Coordonnees> c5 = new List<Coordonnees> {  new Coordonnees(3, 923.1),
															new Coordonnees(0, 0),
															new Coordonnees(2.5, 1) };


			POI poi1 = new POI("Yeet", 54.64564, 324324.532532);
			POI poi2 = new POI("Skeet", 46.64664, 8324.23532);

			Polyline pol1 = new Polyline(c1, Colors.Red, 2);
			Polyline pol2 = new Polyline(c2, Colors.Red, 2);
			Polyline pol3 = new Polyline(c3, Colors.Red, 2);
			Polyline pol4 = new Polyline(c4, Colors.Red, 2);
			Polyline pol5 = new Polyline(c5, Colors.Red, 2);

			Polygon poly1 = new Polygon(c1, Colors.White, Colors.DarkGreen, 0.2);
			Polygon poly2 = new Polygon(c2, Colors.White, Colors.DarkGreen, 0.2);
			Polygon poly3 = new Polygon(c3, Colors.White, Colors.DarkGreen, 0.2);

			do
			{
				do
				{
					Console.Clear();
					Console.WriteLine("\t 0. Quitter");
					Console.WriteLine("\t 1. Test List");
					Console.WriteLine("\t 2. Test List Sort");
					Console.WriteLine("\t 3. Test List MyPolylineBoundingBoxComparer");
					Console.WriteLine("\t 4. Test List Find() FindAll()");
					Console.WriteLine("\t 5. Test IsPointClose");
					Console.WriteLine("\t 6. Test GetSurfaceArea de Polygon");
					Console.Write("\t > ");
					choice = Int32.Parse(Console.ReadLine());
				} while (choice < 0 || choice > 6);

				switch (choice)
				{
					case 1:
						{

							//					---AFFICHAGE DES OBJETS--

							Console.WriteLine("Coord1 : " + c1);
							Console.WriteLine();
							Console.WriteLine("Coord2 : " + c2);
							Console.WriteLine();
							Console.WriteLine("POI 1 : " + poi1 + "\nPOI 2 : " + poi2);
							Console.WriteLine();
							Console.WriteLine("Polyline 1 : " + pol1 + "\nPolyline 2 : " + pol2);
							Console.WriteLine();
							Console.WriteLine("Polygon 1 : " + poly1 + "\nPolygon 2 : " + poly2);
							Console.WriteLine();
							Console.WriteLine("Surface pol1 : " + pol1.GetSurface() + "\nSurface pol2 : " + pol2.GetSurface());

							//					---CREATION LISTE DE CartoObj AVEC LES OBJETS PRECEDENTS--

							List<CartoObj> listeObjs = new List<CartoObj> { poi1, poi2, pol1, pol2, poly1, poly2 };

							Console.ForegroundColor = ConsoleColor.Green;
							Console.WriteLine("\t\t\t\t LISTE CartoObj");
							Console.ForegroundColor = ConsoleColor.White;
							foreach (CartoObj o in listeObjs)
								Console.WriteLine("\n" + o);

							Console.ForegroundColor = ConsoleColor.Green;
							Console.WriteLine("\t\t\t\t LISTE IPointy");
							Console.ForegroundColor = ConsoleColor.White;
							foreach (CartoObj o in listeObjs)
								if (o is IPointy) Console.WriteLine("\n" + o);

							Console.ForegroundColor = ConsoleColor.Green;
							Console.WriteLine("\t\t\t\t LISTE NON IPointy");
							Console.ForegroundColor = ConsoleColor.White;
							foreach (CartoObj o in listeObjs)
								if (o is IPointy) continue;
								else Console.WriteLine("\n" + o);

							break;
						}

					case 2:
						{
							List<Polyline> listePoly = new List<Polyline> { pol1, pol2, pol3, pol4, pol5 };

							Console.ForegroundColor = ConsoleColor.Red;
							Console.WriteLine("\t\t\t\t LISTE NON TRIEE");
							Console.ForegroundColor = ConsoleColor.White;
							foreach (Polyline o in listePoly)
								Console.WriteLine("\n" + o);

							listePoly.Sort();

							Console.ForegroundColor = ConsoleColor.Green;
							Console.WriteLine("\t\t\t\t LISTE TRIEE");
							Console.ForegroundColor = ConsoleColor.White;
							foreach (Polyline o in listePoly)
								Console.WriteLine("\n" + o);

							break;
						}

					case 3:
						{
							List<Polyline> listePoly = new List<Polyline> { pol1, pol2, pol3, pol4, pol5 };

							Console.ForegroundColor = ConsoleColor.Red;
							Console.WriteLine("\t\t\t\t LISTE NON TRIEE");
							Console.ForegroundColor = ConsoleColor.White;
							foreach (Polyline o in listePoly)
								Console.WriteLine("\n" + o);

							listePoly.Sort(new MyPolylineBoundingBoxComparer());

							Console.ForegroundColor = ConsoleColor.Green;
							Console.WriteLine("\t\t\t\t LISTE TRIEE");
							Console.ForegroundColor = ConsoleColor.White;
							foreach (Polyline o in listePoly)
								Console.WriteLine("\n" + o);

							break;
						}

					case 4:
						{
							List<Polyline> listePoly = new List<Polyline> { pol1, pol2, pol3, pol4, pol5 };

							Console.WriteLine("Polyline cherchee (pol4) : " + pol4);

							Console.WriteLine("\nTrouvee : " + listePoly.Find(x => x.Id == pol4.Id));

							break;
						}

					case 5:
						{
							List<Polyline> listePoly = new List<Polyline> { pol1, pol2, pol3, pol4, pol5 };

							Polyline found = new Polyline();

							double x = 100.0;
							double y = 90.0;

							Console.WriteLine("Points recherches : " + "(" + x + " ," + y + ")");

							foreach (Polyline c in listePoly)
								if (c.IsPointClose(x, y, 10.0)) found = c;

							Console.WriteLine("\nPolyline trouvee : " + found);

							break;
						}

					case 6:
						{
							Console.WriteLine("Surface poly1 : " + poly1.GetSurfaceArea());
							Console.WriteLine("Surface poly2 : " + poly2.GetSurfaceArea());
							Console.WriteLine("Surface poly3 : " + poly3.GetSurfaceArea());
							break;
						}
				}
				Console.ForegroundColor = ConsoleColor.Gray;
				Console.WriteLine("\n\nAppuyer sur une touche pour continuer...");
				Console.ForegroundColor = ConsoleColor.White;
				Console.ReadLine();
			} while (choice != 0);
		}
	}
}
