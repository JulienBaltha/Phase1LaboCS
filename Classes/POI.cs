using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phase_1
{
	class POI : Coordonnees
	{
		private string description;


		//			CONSTRUCTEURS
		public POI() : base(50.610869, 5.510435)
		{
			description = "HEPL";
		}

		public POI(string desc, double nX, double nY) : base(nX, nY)
		{
			description = desc;
		}

		//			FONCTIONS
		public Coordonnees GetCoordonnees()
		{
			return new Coordonnees(x, y);
		}

		//			GET/SET
		public string Description
		{
			get { return description; }
			set { description = value; }
		}

		//			OVERLOAD
		public override string ToString()
		{
			return "Desc : " + description + " " + base.ToString();
		}

	}
}
