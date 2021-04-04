using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Globalization;
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
using Unicon2.Fragments.GraphicalMenu.ViewModel;

namespace Unicon2.Fragments.GraphicalMenu.UserControls
{
    /// <summary>
    /// Interaction logic for LineGrid.xaml
    /// </summary>
    public partial class LineGrid : UserControl
    {
        public LineGrid()
        {
            InitializeComponent();
            OnCellHeightChanged();
            OnCellWidthChanged();
        }

        public static readonly DependencyProperty CellHeightProperty = DependencyProperty.Register(
            "CellHeight", typeof(int), typeof(LineGrid), new PropertyMetadata(10, OnCellHeightChanged));

        private static void OnCellHeightChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {

            (d as LineGrid).OnCellHeightChanged();
        }

        private void OnCellHeightChanged()
        {
            if (!(BaseGrid is Grid) || (int) CellHeight < 0)
                return;

            Grid grid = (Grid) BaseGrid;
            grid.RowDefinitions.Clear();

            var numOfRows = DisplayHeight / CellHeight;

            for (int i = 0; i < numOfRows; i++)
                grid.RowDefinitions.Add(
                    new RowDefinition() {Height = new GridLength(CellHeight)});

        }

        public int CellHeight
        {
            get { return (int) GetValue(CellHeightProperty); }
            set { SetValue(CellHeightProperty, value); }
        }


        public static readonly DependencyProperty CellWidthProperty = DependencyProperty.Register(
            "CellWidth", typeof(int), typeof(LineGrid), new PropertyMetadata(10, OnCellWidthChanged));

        private static void OnCellWidthChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as LineGrid).OnCellWidthChanged();
        }

        private void OnCellWidthChanged()
        {
            if (!(BaseGrid is Grid) || (int) CellWidth < 0)
                return;

            Grid grid = (Grid) BaseGrid;
            grid.ColumnDefinitions.Clear();

            var numOfRows = DisplayWidth / CellWidth;

            for (int i = 0; i < numOfRows; i++)
                grid.ColumnDefinitions.Add(
                    new ColumnDefinition() {Width = new GridLength(CellWidth)});
        }

        public int CellWidth
        {
            get { return (int) GetValue(CellWidthProperty); }
            set { SetValue(CellWidthProperty, value); }
        }

        public static readonly DependencyProperty GraphicalElementsProperty = DependencyProperty.Register(
            "GraphicalElements", typeof(ObservableCollection<GraphicalElementViewModel>), typeof(LineGrid),
            new PropertyMetadata(new ObservableCollection<GraphicalElementViewModel>(), OnGraphicalElementsChanged));

        public static readonly DependencyProperty DisplayWidthProperty = DependencyProperty.Register("DisplayWidth", typeof(int), typeof(LineGrid), new PropertyMetadata(400,OnDisplayWidthChanged));

        private static void OnDisplayWidthChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as LineGrid).Width = (double)e.NewValue;
        }

        public static readonly DependencyProperty DisplayHeightProperty = DependencyProperty.Register("DisplayHeight", typeof(int), typeof(LineGrid), new PropertyMetadata(400,OnDisplayHeightChanged));
        public static readonly DependencyProperty SlotsProperty = DependencyProperty.Register("Slots", typeof(List<GraphicalMenuSlotViewModel>), typeof(LineGrid), new PropertyMetadata(new List<GraphicalMenuSlotViewModel>(),OnSlotsChanged));

        private static void OnSlotsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as LineGrid).OnSlotsChanged();
        }

        private void OnSlotsChanged()
        {
            BaseGrid.Children.Clear();
            foreach (var slot in Slots)
            {
                var border=new Border()
                {
                    Width = CellWidth,
                    Height = CellHeight,
                    BorderThickness = new Thickness(0.5),
                    BorderBrush = Brushes.DarkGray
                };
                Grid.SetColumn(border,slot.SlotLeftOffset);
                Grid.SetRow(border, slot.SlotTopOffset);
                border.DataContext = slot;
                var binding = new Binding("IsHighlighted");
                binding.Converter=new IsHighlightedConverter();
                border.SetBinding(Border.BackgroundProperty, binding);
                BaseGrid.Children.Add(border);
            }
            
        }


        private static void OnDisplayHeightChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as LineGrid).Height = (double)e.NewValue;
        }

        private static void OnGraphicalElementsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var action = new NotifyCollectionChangedEventHandler(
                (o, args) =>
                {
                    var lineGrid = d as LineGrid;

                    if (lineGrid != null)
                    {
                        lineGrid.GraphicalElementsChanged(o, args);
                    }
                });

            if (e.OldValue != null)
            {
                var coll = (INotifyCollectionChanged) e.OldValue;
                // Unsubscribe from CollectionChanged on the old collection
                coll.CollectionChanged -= action;
            }

            if (e.NewValue != null)
            {
                var coll = (ObservableCollection<GraphicalElementViewModel>) e.NewValue;

                // Subscribe to CollectionChanged on the new collection
                coll.CollectionChanged += action;
            }
        }

        private void GraphicalElementsChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                var border = new ElementOnDisplayView();

                border.DataContext = (e.NewItems[0] as GraphicalElementViewModel);

                var relatedSlots = Slots.Where(model =>
                    model.RelatedGraphicalElementViewModel == (e.NewItems[0] as GraphicalElementViewModel)).ToList();

                var columnNum = relatedSlots.Min(model => model.SlotLeftOffset);
                var rowNum = relatedSlots.Min(model => model.SlotTopOffset);

                var columnSpan = relatedSlots.Select(model => model.SlotLeftOffset).Distinct().Count();

                var rowSpan = relatedSlots.Select(model => model.SlotTopOffset).Distinct().Count();


                Grid.SetColumn(border, columnNum);
                Grid.SetRow(border, rowNum);
                Grid.SetColumnSpan(border, columnSpan);
                Grid.SetRowSpan(border, rowSpan);
                BaseGrid.Children.Add(border);
            }

            if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                var element = (e.OldItems[0] as GraphicalElementViewModel);
                var viewToRemove=BaseGrid.Children.OfType<ElementOnDisplayView>().FirstOrDefault(view => view.DataContext == element);
                BaseGrid.Children.Remove(viewToRemove);
            }
        }


        public ObservableCollection<GraphicalElementViewModel> GraphicalElements
        {
            get { return (ObservableCollection<GraphicalElementViewModel>) GetValue(GraphicalElementsProperty); }
            set { SetValue(GraphicalElementsProperty, value); }
        }

        public int DisplayWidth
        {
            get { return (int) GetValue(DisplayWidthProperty); }
            set { SetValue(DisplayWidthProperty, value); }
        }

        public int DisplayHeight
        {
            get { return (int) GetValue(DisplayHeightProperty); }
            set { SetValue(DisplayHeightProperty, value); }
        }

        public List<GraphicalMenuSlotViewModel> Slots
        {
            get { return (List<GraphicalMenuSlotViewModel>) GetValue(SlotsProperty); }
            set { SetValue(SlotsProperty, value); }
        }
    }

    public class IsHighlightedConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((bool) value)
            {
                return Brushes.Red;
            }
            return Brushes.Transparent;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
