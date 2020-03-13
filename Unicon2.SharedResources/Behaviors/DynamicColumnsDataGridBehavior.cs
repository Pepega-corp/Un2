using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Interactivity;
using Unicon2.Infrastructure.Common;
using Unicon2.Presentation.Infrastructure.ViewModels.Values;
using Unicon2.SharedResources.DataTemplateSelectors;

namespace Unicon2.SharedResources.Behaviors
{
    public class DynamicColumnsDataGridBehavior : Behavior<DataGrid>
    {
        private DynamicDataTable _journalDataTable;
        private ObservableCollection<List<IFormattedValueViewModel>> _collection;

        protected override void OnDetaching()
        {
            if (_journalDataTable != null) _journalDataTable.TableUpdateAction = null;
            base.OnDetaching();
        }


        public static readonly DependencyProperty RowValuesProperty =
          DependencyProperty.Register("RowValues", typeof(DynamicDataTable), typeof(DynamicColumnsDataGridBehavior),
              new PropertyMetadata(null, OnRowValuesPropertyChanged));

        public DynamicDataTable RowValues
        {
            get { return (DynamicDataTable)GetValue(RowValuesProperty); }
            set { SetValue(RowValuesProperty, value); }
        }

        private static void OnRowValuesPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            DynamicColumnsDataGridBehavior beh = sender as DynamicColumnsDataGridBehavior;
            beh.OnRowValuesChanged();
        }

        private void OnRowValuesChanged()
        {
            if (AssociatedObject == null) return;
            if (RowValues == null) return;
            _journalDataTable = RowValues;

            AssociatedObject.Columns.Clear();

            int index = 0;
            if (IsTransponed)
            {

                if (_journalDataTable.IsBaseNumeration && _journalDataTable.RowHeadersStrings != null)
                {
                    AssociatedObject.Columns.Add(CreateGridTemplateColumn(index++, "#"));
                }
                AssociatedObject.Columns.Add(CreateGridTemplateColumn(index++, ""));
                if (_journalDataTable.RowHeadersStrings != null)
                {
                    foreach (string rowHeadersString in _journalDataTable.RowHeadersStrings)
                    {
                        AssociatedObject.Columns.Add(CreateGridTemplateColumn(index, rowHeadersString));
                        index++;
                    }
                }


                if (_journalDataTable.ColumnNamesStrings != null)
                {
                    _collection = new ObservableCollection<List<IFormattedValueViewModel>>();

                    int rowIndex = 0;
                    foreach (string columnNameString in _journalDataTable.ColumnNamesStrings)
                    {

                        List<IFormattedValueViewModel> formattedValueViewModels = new List<IFormattedValueViewModel>();
                        _journalDataTable.Values.ForEach((list =>
                        {
                            if (list.Count > rowIndex)
                                formattedValueViewModels.Add(list[rowIndex]);
                        }));

                        rowIndex++;
                        InsertRow(formattedValueViewModels);
                    }
                }
                AssociatedObject.ItemsSource = _collection;
            }
            else
            {
                if (_journalDataTable.RowHeadersStrings != null)
                {
                    AssociatedObject.Columns.Add(CreateGridTemplateColumn(index++, ""));

                }
                if ((_journalDataTable.ColumnNamesStrings != null) && (_journalDataTable.IsBaseNumeration))
                {
                    AssociatedObject.Columns.Add(CreateGridTemplateColumn(index++, "№"));
                }

                if (_journalDataTable.ColumnNamesStrings != null)
                {
                    foreach (string columnName in _journalDataTable.ColumnNamesStrings)
                    {
                        AssociatedObject.Columns.Add(CreateGridTemplateColumn(index, columnName));
                        index++;
                    }
                }
                _collection = new ObservableCollection<List<IFormattedValueViewModel>>();

                AssociatedObject.ItemsSource = _collection;
                _journalDataTable.Values.ForEach((list => { InsertRow(list); }));
            }
            _journalDataTable.FormattedValueViewModelAddedAction = InsertRow;
            _journalDataTable.TableUpdateAction = OnRowValuesChanged;
        }


        public static readonly DependencyProperty IsTransponedProperty =
            DependencyProperty.Register("IsTransponed", typeof(bool), typeof(DynamicColumnsDataGridBehavior),
                new PropertyMetadata(false, OnIsTransponedPropertyChanged));

        public bool IsTransponed
        {
            get { return (bool)GetValue(IsTransponedProperty); }
            set { SetValue(IsTransponedProperty, value); }
        }

        private static void OnIsTransponedPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            DynamicColumnsDataGridBehavior beh = sender as DynamicColumnsDataGridBehavior;
            beh.OnRowValuesChanged();
        }


        private DataGridTemplateColumn CreateGridTemplateColumn(int index, string columnName)
        {
            DataTemplate dataTemplate1 = new DataTemplate();
            dataTemplate1.VisualTree = new FrameworkElementFactory(typeof(ContentPresenter));
            Binding b11 = new Binding(".[" + index + "]");
            //dataTemplate1.VisualTree.SetValue(ContentPresenter.HorizontalAlignmentProperty,HorizontalAlignment.Center);
            dataTemplate1.VisualTree.SetValue(ContentPresenter.ContentProperty, b11);
            DataTemplateSelector dataTemplateSelector1 = new ViewModelByStrongNameDataTemplateSelector();
            dataTemplate1.VisualTree.SetValue(ContentPresenter.ContentTemplateSelectorProperty, dataTemplateSelector1);


            DataGridTemplateColumn dataGridTemplateColumn = new DataGridTemplateColumn
            {
                Header = columnName,
                IsReadOnly = true,
                CellTemplate = dataTemplate1
            };
            return dataGridTemplateColumn;
        }

        private void InsertRow(IEnumerable<IFormattedValueViewModel> formattedValueViewModels)
        {
            List<IFormattedValueViewModel> listToInsert = new List<IFormattedValueViewModel>(formattedValueViewModels);

            if (IsTransponed)
            {
                if (_journalDataTable.ColumnNamesStrings != null)
                {
                    IStringValueViewModel stringValueViewModel =
                     StaticContainer.Container.Resolve<IStringValueViewModel>();
                    stringValueViewModel.StringValue = _journalDataTable.ColumnNamesStrings[_collection.Count];
                    listToInsert.Insert(0, stringValueViewModel);
                }
                if ((_journalDataTable.IsBaseNumeration) && (_journalDataTable.RowHeadersStrings != null))
                {
                    INumericValueViewModel numericValueViewModel =
                        StaticContainer.Container.Resolve<INumericValueViewModel>();
                    numericValueViewModel.NumValue = (_collection.Count + 1).ToString();
                    listToInsert.Insert(0, numericValueViewModel);
                }
            }
            else
            {
                if ((_journalDataTable.IsBaseNumeration) && _journalDataTable.ColumnNamesStrings != null)
                {
                    INumericValueViewModel numericValueViewModel =
                       StaticContainer.Container.Resolve<INumericValueViewModel>();
                    numericValueViewModel.NumValue = (_collection.Count + 1).ToString();
                    listToInsert.Insert(0, numericValueViewModel);
                }
                if ((_journalDataTable.RowHeadersStrings != null))
                {
                    IStringValueViewModel stringValueViewModel =
                        StaticContainer.Container.Resolve<IStringValueViewModel>();
                    if (_journalDataTable.RowHeadersStrings.Count > _collection.Count)
                    {
                        stringValueViewModel.StringValue = _journalDataTable.RowHeadersStrings[_collection.Count];
                    }
                    else
                    {
                        stringValueViewModel.StringValue = string.Empty;
                    }
                    listToInsert.Insert(0, stringValueViewModel);
                }
            }

            Application.Current.Dispatcher.Invoke(() =>
            {
                _collection.Add(listToInsert);
            });
        }

        public static readonly DependencyProperty SelectedIndexesProperty =
            DependencyProperty.Register("SelectedIndexes", typeof(List<int>), typeof(DynamicColumnsDataGridBehavior),
                new FrameworkPropertyMetadata(null) { BindsTwoWayByDefault = true });

        public List<int> SelectedIndexes
        {
            get { return (List<int>)GetValue(SelectedIndexesProperty); }
            set { SetValue(SelectedIndexesProperty, value); }
        }


        private void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems != null && e.AddedItems.Count > 0 && SelectedIndexes != null)
            {
                foreach (List<IFormattedValueViewModel> formattedValueViewModels in e.AddedItems)
                {
                    int selectedIndex;
                    if (int.TryParse((formattedValueViewModels[0] as INumericValueViewModel).NumValue,
                        out selectedIndex))
                    {
                        SelectedIndexes.Add(selectedIndex);

                    }
                }
            }

            if (e.RemovedItems != null && e.RemovedItems.Count > 0 && SelectedIndexes != null)
            {
                foreach (List<IFormattedValueViewModel> formattedValueViewModels in e.RemovedItems)
                {
                    int selectedIndex;
                    if (int.TryParse((formattedValueViewModels[0] as INumericValueViewModel).NumValue,
                        out selectedIndex))
                    {
                        SelectedIndexes.Remove(selectedIndex);

                    }
                }
            }
        }


        protected override void OnAttached()
        {
            AssociatedObject.SelectionChanged += OnSelectionChanged;
            //  _dataTableOfRecords = new DataTable();
            OnRowValuesChanged();
            base.OnAttached();
        }
    }
}