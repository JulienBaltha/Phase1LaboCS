using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phase_1
{
	public abstract class CartoObj : IIsPointClose
	{
		#region Variables Membres
		static int nbCartes;
		private int id;
		#endregion

		#region Constructeurs
		public CartoObj()
		{
			id = nbCartes++;
		}
		#endregion

		#region Fonctions
		public virtual void Draw()
		{
			Console.WriteLine(this.ToString());
		}

		public virtual bool IsPointClose(double x, double y, double precision)
		{
			return false;
		}
		#endregion

		#region Overloading
		public override string ToString()
		{
			return "ID : " + id;
		}
		#endregion

		#region Get/Set
		public int Id { get => id; set => id = value; }
		#endregion
	}
}
