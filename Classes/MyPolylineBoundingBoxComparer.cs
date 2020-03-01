using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phase_1
{
	class MyPolylineBoundingBoxComparer : IComparer<Polyline>
	{
		public int Compare(Polyline l1, Polyline l2)
		{
			//if ((l1.GetSurface()).Equals(l2.GetSurface())) return 0;
			//if (l1.GetSurface() < l2.GetSurface()) return -1;
			//if (l1.GetSurface() > l2.GetSurface()) return -1;

			return l1.GetSurface().CompareTo(l2.GetSurface());
		}
	}
}
