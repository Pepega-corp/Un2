using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Microsoft.Xaml.Behaviors;
using Unicon2.Fragments.Configuration.Infrastructure.ViewModel.Properties;
using Unicon2.Fragments.Configuration.ViewModel.Properties;
using Unicon2.Infrastructure.Common;
using Unicon2.Presentation.Infrastructure.ViewModels;
using Unicon2.Presentation.Infrastructure.ViewModels.Values;
using Unicon2.SharedResources.DataTemplateSelectors;

namespace Unicon2.Fragments.Configuration.Behaviors
{
    public class DynamicPropertiesDataGridBehavior : Behavior<DataGrid>
    {

        private DynamicPropertiesTable _journalDataTable;
        private ObservableCollection<List<ILocalAndDeviceValueContainingViewModel>> _collection;

        protected override void OnDetaching()
        {
            if (_journalDataTable != null) _journalDataTable.TableUpdateAction = null;
            base.OnDetaching();
        }


        public static readonly DependencyProperty RowValuesProperty =
            DependencyProperty.Register("RowValues", typeof(DynamicPropertiesTable),
                typeof(DynamicPropertiesDataGridBehavior),
                new PropertyMetadata(null, OnRowValuesPropertyChanged));

        public DynamicPropertiesTable RowValues
        {
            get { return (DynamicPropertiesTable) GetValue(RowValuesProperty); }
            set { SetValue(RowValuesProperty, value); }
        }

        private static void OnRowValuesPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            DynamicPropertiesDataGridBehavior beh = sender as DynamicPropertiesDataGridBehavior;
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
                    _collection = new ObservableCollection<List<ILocalAndDeviceValueContainingViewModel>>();

                    int rowIndex = 0;
                    foreach (string columnNameString in _journalDataTable.ColumnNamesStrings)
                    {

                        List<ILocalAndDeviceValueContainingViewModel> localAndDeviceValueContainingViewModels =
                            new List<ILocalAndDeviceValueContainingViewModel>();
                        _journalDataTable.Values.ForEach((list =>
                        {
                            if (list.Count > rowIndex)
                                localAndDeviceValueContainingViewModels.Add(list[rowIndex]);
                        }));

                        rowIndex++;
                        InsertRow(localAndDeviceValueContainingViewModels);
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

                _collection = new ObservableCollection<List<ILocalAndDeviceValueContainingViewModel>>();
               
                AssociatedObject.ItemsSource = _collection;
                _journalDataTable.Values.ForEach((list => { InsertRow(list); }));
            }

            _journalDataTable.FormattedValueViewModelAddedAction = InsertRow;
            _journalDataTable.TableUpdateAction = OnRowValuesChanged;
        }


        public static readonly DependencyProperty IsTransponedProperty =
            DependencyProperty.Register("IsTransponed", typeof(bool), typeof(DynamicPropertiesDataGridBehavior),
                new PropertyMetadata(false, OnIsTransponedPropertyChanged));

        public bool IsTransponed
        {
            get { return (bool) GetValue(IsTransponedProperty); }
            set { SetValue(IsTransponedProperty, value); }
        }

        private static void OnIsTransponedPropertyChanged(DependencyObject sender,
            DependencyPropertyChangedEventArgs args)
        {
            DynamicPropertiesDataGridBehavior beh = sender as DynamicPropertiesDataGridBehavior;
            beh.OnRowValuesChanged();
        }


        private DataGridTemplateColumn CreateGridTemplateColumn(int index, string columnName)
        {

            DataTemplate cellDataTemplate = new DataTemplate();

            var innerVisualTree = new FrameworkElementFactory(typeof(ContentControl));
            innerVisualTree.SetValue(FrameworkElement.StyleProperty, CellStyle);

            Binding b11 = new Binding(".[" + index + "]");
            //dataTemplate1.VisualTree.SetValue(ContentPresenter.HorizontalAlignmentProperty,HorizontalAlignment.Center);
            innerVisualTree.SetValue(ContentControl.ContentProperty, b11);

            DataTemplateSelector dataTemplateSelector1 = new ViewModelByStrongNameDataTemplateSelector();
            innerVisualTree.SetValue(ContentControl.ContentTemplateSelectorProperty, dataTemplateSelector1);


            cellDataTemplate.VisualTree = innerVisualTree;


            DataGridTemplateColumn dataGridTemplateColumn = new DataGridTemplateColumn
            {
                Header = columnName,
                IsReadOnly = true,
                CellTemplate = cellDataTemplate
            };

            return dataGridTemplateColumn;
        }





        private void InsertRow(IEnumerable<ILocalAndDeviceValueContainingViewModel> formattedValueViewModels)
        {
            List<ILocalAndDeviceValueContainingViewModel> listToInsert =
                new List<ILocalAndDeviceValueContainingViewModel>(formattedValueViewModels);

            if (IsTransponed)
            {
                if (_journalDataTable.ColumnNamesStrings != null)
                {
                    IStringValueViewModel stringValueViewModel =
                        StaticContainer.Container.Resolve<IStringValueViewModel>();
                    stringValueViewModel.StringValue =
                        _journalDataTable.ColumnNamesStrings[_collection.Count];
                    IPropertyViewModel propertyViewModel = new RuntimePropertyViewModel();
                    (propertyViewModel as ILocalAndDeviceValueContainingViewModel).DeviceValue = stringValueViewModel;
                    listToInsert.Insert(0, propertyViewModel as ILocalAndDeviceValueContainingViewModel);
                }

                //if ((this._journalDataTable.IsBaseNumeration) && (this._journalDataTable.RowHeadersStrings != null))
                //{
                //    INumericValueViewModel numericValueViewModel =
                //        StaticContainer.Container.Resolve<INumericValueViewModel>();
                //    numericValueViewModel.NumValue = (this._collection.Count + 1).ToString();
                //    listToInsert.Insert(0, numericValueViewModel);
                //}
            }
            else
            {
                //if ((this._journalDataTable.IsBaseNumeration) && this._journalDataTable.ColumnNamesStrings != null)
                //{
                //    INumericValueViewModel numericValueViewModel =
                //        StaticContainer.Container.Resolve<INumericValueViewModel>();
                //    numericValueViewModel.NumValue = (this._collection.Count + 1).ToString();
                //    listToInsert.Insert(0, numericValueViewModel);
                //}

                if ((_journalDataTable.RowHeadersStrings != null))
                {
                    IStringValueViewModel stringValueViewModel =
                        StaticContainer.Container.Resolve<IStringValueViewModel>();
                    if (_journalDataTable.RowHeadersStrings.Count > _collection.Count)
                    {
                        stringValueViewModel.StringValue =
                            _journalDataTable.RowHeadersStrings[_collection.Count];
                    }
                    else
                    {
                        stringValueViewModel.StringValue = string.Empty;
                    }

                    IPropertyViewModel propertyViewModel = new RuntimePropertyViewModel();
                    (propertyViewModel as ILocalAndDeviceValueContainingViewModel).DeviceValue = stringValueViewModel;
                    (propertyViewModel as ILocalAndDeviceValueContainingViewModel).LocalValue = stringValueViewModel;

                    listToInsert.Insert(0, propertyViewModel as ILocalAndDeviceValueContainingViewModel);
                }
            }

            Application.Current.Dispatcher.Invoke(() => { _collection.Add(listToInsert); });
        }

        public static readonly DependencyProperty SelectedIndexesProperty =
            DependencyProperty.Register("SelectedIndexes", typeof(List<int>), typeof(DynamicPropertiesDataGridBehavior),
                new FrameworkPropertyMetadata(null) {BindsTwoWayByDefault = true});

        public List<int> SelectedIndexes
        {
            get { return (List<int>) GetValue(SelectedIndexesProperty); }
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

        public static readonly DependencyProperty CellStyleProperty =
            DependencyProperty.Register("CellStyle", typeof(Style), typeof(DynamicPropertiesGridViewBehavior));

        public Style CellStyle
        {
            get { return (Style) GetValue(CellStyleProperty); }
            set { SetValue(CellStyleProperty, value); }
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