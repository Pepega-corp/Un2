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
using System.Windows.Shapes;
using Unicon2.Fragments.Measuring.Editor.ViewModel.PresentationSettings;

namespace Unicon2.Fragments.Measuring.Editor.View.PresentationSettings
{
	/// <summary>
	/// Логика взаимодействия для PresentationSettingsWindow.xaml
	/// </summary>
	public partial class PresentationSettingsWindow
	{
		private Point startPoint;
		public PresentationSettingsWindow()
		{
			InitializeComponent();
		}

		private void UIElement_OnMouseMove(object sender, MouseEventArgs e)
		{
			if (e.LeftButton == MouseButtonState.Pressed)
			{
				var presentationElementViewModel =
					((sender as FrameworkElement).DataContext as PresentationElementViewModel);
				if (!presentationElementViewModel.IsSelected)
				{
					return;
				}


				Point position = e.GetPosition(this);
				var yOffset = position.Y - startPoint.Y;
				var xOffset = position.X - startPoint.X;

				if (Math.Abs(xOffset) > SystemParameters.MinimumHorizontalDragDistance)
				{
					((sender as FrameworkElement).DataContext as PresentationElementViewModel).PositioningInfoViewModel
						.OffsetLeft += (int) (Math.Round(xOffset/5)*5);
					startPoint = e.GetPosition(null);
				}

				if (Math.Abs(yOffset) > SystemParameters.MinimumVerticalDragDistance)
				{
					((sender as FrameworkElement).DataContext as PresentationElementViewModel).PositioningInfoViewModel
						.OffsetTop += (int) (Math.Round(yOffset / 5) * 5);
					startPoint = e.GetPosition(null);
				}
			}
		}
		

		
		private void UIElement_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			startPoint = e.GetPosition(null);
		}
	}
}