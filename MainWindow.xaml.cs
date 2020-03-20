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
using System.Windows.Navigation;
using System.Collections.ObjectModel;
using Microsoft.Maps.MapControl.WPF;
using MyObjLib;
using MyPersonalMapData_lib;
using MathUtil;
using Microsoft.VisualBasic.FileIO;
using System.IO;
using System.Text.RegularExpressions;
using System.Globalization;
using Microsoft.Win32;
using System.ComponentModel;

namespace Project_WPF_Mock
{
	public partial class MainWindow : Window
	{
		bool add;
		bool modify;
		bool delete;

		MyPersonalMapData userData;
		List<Coordonnees> coord;
		LocationCollection loc;

		int nbItems;
		int nbCoord;
		int nbPoi;

		int prevRadio;

		public MainWindow()
		{
			InitializeComponent();

			SolidColorBrush newBrush = new SolidColorBrush(Colors.Black);
			superCombo.SelectedColor = newBrush;

			userData = new MyPersonalMapData();
			coord = new List<Coordonnees>();
			loc = new LocationCollection();

			nbItems = 0;
			nbCoord = 0;
			nbPoi = 0;
			prevRadio = 0;

			add = true;
			modify = delete = false;
		}

		private void myMap_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
		{
			listBox.UnselectAll();
			X_box.Text = "";
			Y_box.Text = "";

			Point mousepnt = e.GetPosition(this);
			mousepnt.Y -= 47;
			mousepnt.X -= 105;
			Location mapMouse = myMap.ViewportPointToLocation(mousepnt);

			if (add)
			{
				coord.Add(new Coordonnees(mapMouse.Longitude, mapMouse.Latitude));
				loc.Add(new Location(coord[nbCoord].Y, coord[nbCoord].X));

				StatusText.Text = coord[nbCoord].ToString();

				if (!(bool)RadioPOI.IsChecked) nbCoord++;

				if ((bool)RadioLine.IsChecked)
				{
					Boutton_valider.Visibility = Visibility.Hidden;

					if (prevRadio != 1)
					{
						Desc_Box.Text = "";
						listBox.UnselectAll();
					}

					if ((Keyboard.Modifiers & ModifierKeys.Control) > 0 && coord.Count > 1)
					{
						nbItems++;
						SolidColorBrush newBrush = (SolidColorBrush)superCombo.SelectedColor;
						Polyline polyline = new Polyline(coord, newBrush.Color, 2);
						userData.Add(polyline);

						MapPolyline mapPolyline = new MapPolyline
						{
							Locations = loc,
							Stroke = newBrush,
							SnapsToDevicePixels = true
						};

						coord = new List<Coordonnees>();
						loc = new LocationCollection();
						nbCoord = 0;
						myMap.Children.Add(mapPolyline);

						if(String.IsNullOrEmpty(Desc_Box.Text))
							listBox.Items.Add("Polyline" + nbItems);
						else
							listBox.Items.Add(Desc_Box.Text);

						Desc_Box.Text = "";

						StatusText.Text = "Polyline ajouté";
					}
				}

				if ((bool)RadioGone.IsChecked)
				{
					Boutton_valider.Visibility = Visibility.Hidden;

					if (prevRadio != 2)
					{
						Desc_Box.Text = "";
						listBox.UnselectAll();
					}

					if ((Keyboard.Modifiers & ModifierKeys.Control) > 0 && coord.Count > 1)
					{
						nbItems++;
						coord.Add(coord[0]);
						loc.Add(loc[0]);
						SolidColorBrush newBrush = (SolidColorBrush)superCombo.SelectedColor;
						Polygon polygon = new Polygon(coord, newBrush.Color, newBrush.Color, 0.3);
						userData.Add(polygon);

						MapPolygon mapPolygon = new MapPolygon
						{
							Locations = loc,
							Stroke = newBrush,
							SnapsToDevicePixels = true,
						};

						mapPolygon.Fill = new SolidColorBrush(polygon.Bgcolor) { Opacity = 0.3 };

						coord = new List<Coordonnees>();
						loc = new LocationCollection();
						nbCoord = 0;
						myMap.Children.Add(mapPolygon);

						if (String.IsNullOrEmpty(Desc_Box.Text))
							listBox.Items.Add("Polygon" + nbItems);
						else
							listBox.Items.Add(Desc_Box.Text);

						Desc_Box.Text = "";
						StatusText.Text = "Polygone ajouté";
					}
				}

				if ((bool)RadioPOI.IsChecked)
				{
					if (prevRadio != 3)
					{
						Desc_Box.Text = "";
						listBox.UnselectAll();
						superCombo.Visibility = Visibility.Hidden;
					}

					Pushpin pin = new Pushpin();
					POI poi = new POI();

					pin.Location = loc[0];
					poi = new POI(Desc_Box.Text, coord[nbCoord].X, coord[nbCoord].Y);

					if (String.IsNullOrEmpty(poi.Description)) poi.Description = "POI" + nbPoi;

					userData.Add(poi);

					coord = new List<Coordonnees>();
					loc = new LocationCollection();
					nbCoord = 0;
					myMap.Children.Add(pin);

					listBox.Items.Add(poi.Description);

					Desc_Box.Text = "";
					nbPoi++;

					StatusText.Text = "POI ajouté";
				}
			}

			if(modify)
			{
				int i = 0;

				
				foreach (CartoObj o in userData.Liste)
					if (o.IsPointClose(mapMouse.Longitude, mapMouse.Latitude, 0.2)) break;
					else i++;

				listBox.SelectedIndex = i;

				Boutton_valider.Visibility = Visibility.Visible;

				if (listBox.SelectedIndex >= 0)
				{
					Desc_Box.Text = listBox.SelectedItem.ToString();
					if (userData.Liste[listBox.SelectedIndex] is POI) superCombo.Visibility = Visibility.Hidden;
				}
			}

			if (delete && userData.Liste.Count > 0)
			{
				int i = 0;

				foreach (CartoObj o in userData.Liste)
					if (o.IsPointClose(mapMouse.Longitude, mapMouse.Latitude, 0.2)) break;
					else i++;

				Console.WriteLine(i);

				if (i <= listBox.Items.Count - 1)
				{
					if(myMap.Children[i] is MapShapeBase) nbItems--;
					else
					if(myMap.Children[i] is Pushpin) nbPoi--;

					StatusText.Text = listBox.Items[i].ToString() + " supprimé";

					listBox.Items.RemoveAt(i);
					myMap.Children.RemoveAt(i);
					userData.Liste.RemoveAt(i);
					listBox.UnselectAll();
					
					Desc_Box.Text = "";
				}
				else
					StatusText.Text = "Aucun objet a proximité";
			}
		}

		private void Ajouter_Click(object sender, RoutedEventArgs e)
		{
			add = true;
			modify = false;
			delete = false;
		}

		private void Modifier_Click(object sender, RoutedEventArgs e)
		{
			add = false;
			modify = true;
			delete = false;
		}

		private void Supprimer_Click(object sender, RoutedEventArgs e)
		{
			add = false;
			modify = false;
			delete = true;
		}

		private void listBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			Boutton_valider.Visibility = Visibility.Visible;

			if(listBox.SelectedIndex >= 0)
			{
				Desc_Box.Text = listBox.SelectedItem.ToString();
				if (userData.Liste[listBox.SelectedIndex] is POI)
				{
					POI poi = userData.Liste[listBox.SelectedIndex] as POI;

					superCombo.Visibility = Visibility.Hidden;
					X_box.Visibility = Visibility.Visible;
					Y_box.Visibility = Visibility.Visible;

					X_box.Text = poi.X.ToString("#.00000");
					Y_box.Text = poi.Y.ToString("#.00000");
				}
				else
				{
					superCombo.Visibility = Visibility.Visible;
					X_box.Visibility = Visibility.Hidden;
					Y_box.Visibility = Visibility.Hidden;
				}
			}
		}

		private void Boutton_valider_Click(object sender, RoutedEventArgs e)
		{
			if (listBox.SelectedIndex != -1)
			{
				if (!(userData.Liste[listBox.SelectedIndex] is POI))
				{
					MapShapeBase obj = myMap.Children[listBox.SelectedIndex] as MapShapeBase;

					obj.Stroke = superCombo.SelectedColor;

					if (userData.Liste[listBox.SelectedIndex] is Polyline)
					{
						Polyline line = userData.Liste[listBox.SelectedIndex] as Polyline;
						line.Color = ((SolidColorBrush)superCombo.SelectedColor).Color;
						userData.Liste[listBox.SelectedIndex] = line;
					}

					if (userData.Liste[listBox.SelectedIndex] is Polygon)
					{
						Polygon polygon = userData.Liste[listBox.SelectedIndex] as Polygon;
						polygon.Linecolor = ((SolidColorBrush)superCombo.SelectedColor).Color;
						polygon.Bgcolor = Color.FromArgb(150, polygon.Linecolor.R, polygon.Linecolor.G, polygon.Linecolor.B);
						obj.Fill = new SolidColorBrush(polygon.Bgcolor) { Opacity = 0.3 };
						userData.Liste[listBox.SelectedIndex] = polygon;
					}

					if (!Desc_Box.Text.Equals(listBox.SelectedItem.ToString()))
					{
						int index = listBox.SelectedIndex;
						listBox.Items.RemoveAt(listBox.SelectedIndex);
						listBox.Items.Insert(index, Desc_Box.Text);
					}

				}
				else
				if (userData.Liste[listBox.SelectedIndex] is POI)
				{
					POI poi = userData.Liste[listBox.SelectedIndex] as POI;
					poi.X = Double.Parse(X_box.Text);
					poi.Y = Double.Parse(Y_box.Text);

					Location loc = new Location(Double.Parse(Y_box.Text), Double.Parse(X_box.Text));
					Pushpin pin = myMap.Children[listBox.SelectedIndex] as Pushpin;
					pin.Location = loc;
				}

				listBox.UnselectAll();
				Boutton_valider.Visibility = Visibility.Hidden;
				superCombo.Visibility = Visibility.Visible;
				Desc_Box.Text = "";
				StatusText.Text = "Objet modifié";
			}
			else
			{
				if (!String.IsNullOrEmpty(X_box.Text) && !String.IsNullOrEmpty(Y_box.Text))
				{
					Pushpin pin = new Pushpin();
					POI poi = new POI(Desc_Box.Text, Double.Parse(X_box.Text), Double.Parse(Y_box.Text));

					pin.Location = new Location(Double.Parse(Y_box.Text), Double.Parse(X_box.Text));

					userData.Add(poi);

					myMap.Children.Add(pin);

					listBox.Items.Add(poi.Description);

					Desc_Box.Text = "";
					nbPoi++;

					StatusText.Text = "POI ajouté";
				}
			}
		}

		private void POI_Import_Click(object sender, RoutedEventArgs e)
		{
			string filename = "";

			Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();

			dlg.DefaultExt = ".csv";
			dlg.Filter = "CSV Files (*.csv)|*.csv";

			Nullable<bool> result = dlg.ShowDialog();

			if (result == true)
			{
				filename = dlg.FileName;

				using (TextFieldParser parser = new TextFieldParser(@filename))
				{
					parser.TextFieldType = FieldType.Delimited;
					parser.SetDelimiters(";");

					POI poi = new POI();

					while (!parser.EndOfData)
					{
						string[] fields = parser.ReadFields();

						for (int i = 2; i <= fields.Length ; i += 3)
						{
							poi = new POI(fields[i], Double.Parse(fields[i - 1]), Double.Parse(fields[i - 2]));

							Pushpin pin = new Pushpin();
							pin.Location = new Location(Double.Parse(fields[i - 2]), Double.Parse(fields[i - 1]));

							myMap.Children.Add(pin);
							userData.Add(poi);

							listBox.Items.Add(poi.Description);

							nbPoi++;
						}
					}
				}
			}
		}

		private void POI_Export_Click(object sender, RoutedEventArgs e)
		{
			Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();

			dlg.DefaultExt = ".csv";
			dlg.Filter = "CSV Files (*.csv)|*.csv";

			Nullable<bool> result = dlg.ShowDialog();

			if (result == true)
			{
				string filename = dlg.FileName;

				StreamWriter sw = new StreamWriter(filename, false);

				foreach(POI poi in userData.Liste)
				{
					sw.WriteLine(poi.Y + ";" + poi.X + ";" + poi.Description + ";");
				}

				sw.Close();
			}
		}

		private void Trajet_Export_Click(object sender, RoutedEventArgs e)
		{
			Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();

			dlg.DefaultExt = ".csv";
			dlg.Filter = "CSV Files (*.csv)|*.csv";

			Nullable<bool> result = dlg.ShowDialog();

			if (result == true)
			{
				string filename = dlg.FileName;

				StreamWriter sw = new StreamWriter(filename, false);

				foreach (CartoObj o in userData.Liste)
				{
					if (o is POI)
					{
						POI poi = o as POI;
						sw.WriteLine(poi.Y + ";" + poi.X + ";" + poi.Description + ";");
					}

					if (o is Polyline)
					{
						Polyline line = o as Polyline;

						foreach (Coordonnees coord in line.GetCoordonnees())
						{
							sw.WriteLine(coord.Y + ";" + coord.X + ";");
						}
					}
				}
				sw.Close();
			}
		}

		private void Trajet_Import_Click(object sender, RoutedEventArgs e)
		{
			string filename = "";

			Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();

			dlg.DefaultExt = ".csv";
			dlg.Filter = "CSV Files (*.csv)|*.csv";

			Nullable<bool> result = dlg.ShowDialog();

			if (result == true)
			{
				filename = dlg.FileName;

				using (TextFieldParser parser = new TextFieldParser(@filename))
				{
					parser.TextFieldType = FieldType.Delimited;
					parser.SetDelimiters(";");

					POI poi = new POI();
					Polyline line = new Polyline();
					List<Coordonnees> c = new List<Coordonnees>();

					while (!parser.EndOfData)
					{
						string[] fields = parser.ReadFields();

						for (int i = 2; i <= fields.Length; i += 3)
						{
							poi = new POI(fields[i], Double.Parse(fields[i - 1]), Double.Parse(fields[i - 2]));

							Pushpin pin = new Pushpin();
							pin.Location = new Location(Double.Parse(fields[i - 2]), Double.Parse(fields[i - 1]));

							if (String.IsNullOrEmpty(poi.Description))
							{
								c.Add(new Coordonnees(poi.X, poi.Y));
								loc.Add(new Location(poi.Y, poi.X));
								nbItems++;
							}
							else
							{
								myMap.Children.Add(pin);
								userData.Add(poi);
								listBox.Items.Add(poi.Description);
								nbPoi++;
							}

							i += 3;
						}
					}

					if (loc.Count() > 0)
					{
						line = new Polyline(c, Colors.Black, 1);

						MapPolyline mapPolyline = new MapPolyline
						{
							Locations = loc,
							Stroke = new SolidColorBrush(Colors.Black),
							SnapsToDevicePixels = true
						};

						loc = new LocationCollection();
						myMap.Children.Add(mapPolyline);

						userData.Add(line);
						listBox.Items.Add("Polyline" + nbItems);
					}
				}
			}
		}

		private void New_Click(object sender, RoutedEventArgs e)
		{
			nbItems = 0;
			nbPoi = 0;
			nbCoord = 0;

			userData.ResetCollection();
			myMap.Children.Clear();
			listBox.Items.Clear();
		}

		private void Exit_Click(object sender, RoutedEventArgs e)
		{
			System.Windows.Application.Current.Shutdown();
		}
		
		private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
		{
			Regex regex = new Regex("[^0-9.,-]+");
			e.Handled = regex.IsMatch(e.Text);
		}

		private void RadioPOI_Checked(object sender, RoutedEventArgs e)
		{
			listBox.UnselectAll();
			X_box.Visibility = Visibility.Visible;
			Y_box.Visibility = Visibility.Visible;
			superCombo.Visibility = Visibility.Hidden;
			Boutton_valider.Visibility = Visibility.Visible;
		}

		private void RadioLine_Checked(object sender, RoutedEventArgs e)
		{
			listBox.UnselectAll();
			X_box.Visibility = Visibility.Hidden;
			Y_box.Visibility = Visibility.Hidden;
			superCombo.Visibility = Visibility.Visible;
		}

		private void RadioGone_Checked(object sender, RoutedEventArgs e)
		{
			listBox.UnselectAll();
			X_box.Visibility = Visibility.Hidden;
			Y_box.Visibility = Visibility.Hidden;
			superCombo.Visibility = Visibility.Visible;
		}

		private void AboutBox_Click(object sender, RoutedEventArgs e)
		{
			MessageBoxResult result = MessageBox.Show("Balthazar Julien\n2225\nApplication BingMaps 2020",
													  "About this app",
													  MessageBoxButton.OK);
		}

		private void Options_Click(object sender, RoutedEventArgs e)
		{
			Options dlg = new Options();

			dlg.Owner = this;
			dlg.colorSelected += new ColorSelected(dlg_ColorSelected);

			dlg.Show();
		}

		private void dlg_ColorSelected(object sender, EventArgs e)
		{
			Options dlg = (Options)sender;

			this.listBox.Background = (Brush)dlg.index[0];
			this.listBox.Foreground = (Brush)dlg.index[1];
			this.listBox.Focus();

			StatusText.Text = "Couleurs de la listeBox modifiées en " + ((Brush)dlg.index[0]).ToString() + " et " + ((Brush)dlg.index[1]).ToString();
		}
	}
}
