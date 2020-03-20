using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Project_WPF_Mock
{
	public delegate void ColorSelected(object sender, EventArgs e);

	public partial class Options : Window
	{
		public event ColorSelected colorSelected;

		public List<SolidColorBrush> index = new List<SolidColorBrush>();

		public Options()
		{
			InitializeComponent();
		}

		protected virtual void OnColorSelectedValider()
		{
			ColorSelected color = colorSelected;
			if (color != null) color(this, EventArgs.Empty);
			index.Clear();
			this.Close();
		}

		protected virtual void OnColorSelectedAppliquer()
		{
			ColorSelected color = colorSelected;
			if (color != null) color(this, EventArgs.Empty);
			index.Clear();
		}

		private void Valider_Click(object sender, RoutedEventArgs e)
		{
			this.index.Add((SolidColorBrush)superCombo1.SelectedColor);
			this.index.Add((SolidColorBrush)superCombo2.SelectedColor);
			OnColorSelectedValider();
		}

		private void Appliquer_Click(object sender, RoutedEventArgs e)
		{
			this.index.Add((SolidColorBrush)superCombo1.SelectedColor);
			this.index.Add((SolidColorBrush)superCombo2.SelectedColor);
			OnColorSelectedAppliquer();
		}

		private void Annuler_Click(object sender, RoutedEventArgs e)
		{
			this.Close();
		}
	}
}
