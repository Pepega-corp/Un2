using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Interactivity;
using Unicon2.Fragments.Configuration.Infrastructure.ViewModel.Properties;
using Unicon2.Fragments.Configuration.ViewModel.Properties;
using Unicon2.Infrastructure.Common;
using Unicon2.Presentation.Infrastructure.Factories;
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
            DependencyProperty.Register("RowValues", typeof(DynamicPropertiesTable), typeof(DynamicPropertiesDataGridBehavior),
                new PropertyMetadata(null, OnRowValuesPropertyChanged));

        public DynamicPropertiesTable RowValues
        {
            get { return (DynamicPropertiesTable)this.GetValue(RowValuesProperty); }
            set { this.SetValue(RowValuesProperty, value); }
        }

        private static void OnRowValuesPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            DynamicPropertiesDataGridBehavior beh = sender as DynamicPropertiesDataGridBehavior;
            beh.OnRowValuesChanged();
        }

        private void OnRowValuesChanged()
        {
            if (this.AssociatedObject == null) return;
            if (this.RowValues == null) return;
            this._journalDataTable = this.RowValues;

            this.AssociatedObject.Columns.Clear();

            int index = 0;
            if (this.IsTransponed)
            {

                if (this._journalDataTable.IsBaseNumeration && this._journalDataTable.RowHeadersStrings != null)
                {
                    this.AssociatedObject.Columns.Add(this.CreateGridTemplateColumn(index++, "#"));
                }

                this.AssociatedObject.Columns.Add(this.CreateGridTemplateColumn(index++, ""));
                if (this._journalDataTable.RowHeadersStrings != null)
                {
                    foreach (string rowHeadersString in this._journalDataTable.RowHeadersStrings)
                    {
                        this.AssociatedObject.Columns.Add(this.CreateGridTemplateColumn(index, rowHeadersString));
                        index++;
                    }
                }


                if (this._journalDataTable.ColumnNamesStrings != null)
                {
                    this._collection = new ObservableCollection<List<ILocalAndDeviceValueContainingViewModel>>();

                    int rowIndex = 0;
                    foreach (string columnNameString in this._journalDataTable.ColumnNamesStrings)
                    {

                        List<ILocalAndDeviceValueContainingViewModel> localAndDeviceValueContainingViewModels = new List<ILocalAndDeviceValueContainingViewModel>();
                        this._journalDataTable.Values.ForEach((list =>
                        {
                            if (list.Count > rowIndex)
                                localAndDeviceValueContainingViewModels.Add(list[rowIndex]);
                        }));

                        rowIndex++;
                        this.InsertRow(localAndDeviceValueContainingViewModels);
                    }
                }

                this.AssociatedObject.ItemsSource = this._collection;
            }
            else
            {
                if (this._journalDataTable.RowHeadersStrings != null)
                {
                    this.AssociatedObject.Columns.Add(this.CreateGridTemplateColumn(index++, ""));

                }

                if ((this._journalDataTable.ColumnNamesStrings != null) && (this._journalDataTable.IsBaseNumeration))
                {
                    this.AssociatedObject.Columns.Add(this.CreateGridTemplateColumn(index++, "№"));
                }

                if (this._journalDataTable.ColumnNamesStrings != null)
                {
                    foreach (string columnName in this._journalDataTable.ColumnNamesStrings)
                    {
                        this.AssociatedObject.Columns.Add(this.CreateGridTemplateColumn(index, columnName));
                        index++;
                    }
                }

                this._collection = new ObservableCollection<List<ILocalAndDeviceValueContainingViewModel>>();
                try
                {
                    AssociatedObject.Items.Clear();

                }
                catch (Exception e)
                {
                }
                this.AssociatedObject.ItemsSource = this._collection;
                this._journalDataTable.Values.ForEach((list => { this.InsertRow(list); }));
            }

            this._journalDataTable.FormattedValueViewModelAddedAction = this.InsertRow;
            this._journalDataTable.TableUpdateAction = this.OnRowValuesChanged;
        }


        public static readonly DependencyProperty IsTransponedProperty =
            DependencyProperty.Register("IsTransponed", typeof(bool), typeof(DynamicPropertiesDataGridBehavior),
                new PropertyMetadata(false, OnIsTransponedPropertyChanged));

        public bool IsTransponed
        {
            get { return (bool)this.GetValue(IsTransponedProperty); }
            set { this.SetValue(IsTransponedProperty, value); }
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
            List<ILocalAndDeviceValueContainingViewModel> listToInsert = new List<ILocalAndDeviceValueContainingViewModel>(formattedValueViewModels);

            if (this.IsTransponed)
            {
                if (this._journalDataTable.ColumnNamesStrings != null)
                {
                    IStringValueViewModel stringValueViewModel =
                        StaticContainer.Container.Resolve<IStringValueViewModel>();
                    stringValueViewModel.StringValue =
                        this._journalDataTable.ColumnNamesStrings[this._collection.Count];
                    IPropertyViewModel propertyViewModel = new RuntimePropertyViewModel(StaticContainer.Container, StaticContainer.Container.Resolve<IValueViewModelFactory>());
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

                if ((this._journalDataTable.RowHeadersStrings != null))
                {
                    IStringValueViewModel stringValueViewModel =
                        StaticContainer.Container.Resolve<IStringValueViewModel>();
                    if (this._journalDataTable.RowHeadersStrings.Count > this._collection.Count)
                    {
                        stringValueViewModel.StringValue =
                            this._journalDataTable.RowHeadersStrings[this._collection.Count];
                    }
                    else
                    {
                        stringValueViewModel.StringValue = string.Empty;
                    }
                    IPropertyViewModel propertyViewModel = new RuntimePropertyViewModel(StaticContainer.Container, StaticContainer.Container.Resolve<IValueViewModelFactory>());
                    (propertyViewModel as ILocalAndDeviceValueContainingViewModel).DeviceValue = stringValueViewModel;
                    (propertyViewModel as ILocalAndDeviceValueContainingViewModel).LocalValue = stringValueViewModel;

                    listToInsert.Insert(0, propertyViewModel as ILocalAndDeviceValueContainingViewModel);
                }
            }

            Application.Current.Dispatcher.Invoke(() => { this._collection.Add(listToInsert); });
        }

        public static readonly DependencyProperty SelectedIndexesProperty =
            DependencyProperty.Register("SelectedIndexes", typeof(List<int>), typeof(DynamicPropertiesDataGridBehavior),
                new FrameworkPropertyMetadata(null) { BindsTwoWayByDefault = true });

        public List<int> SelectedIndexes
        {
            get { return (List<int>)this.GetValue(SelectedIndexesProperty); }
            set { this.SetValue(SelectedIndexesProperty, value); }
        }


        private void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems != null && e.AddedItems.Count > 0 && this.SelectedIndexes != null)
            {
                foreach (List<IFormattedValueViewModel> formattedValueViewModels in e.AddedItems)
                {
                    int selectedIndex;
                    if (int.TryParse((formattedValueViewModels[0] as INumericValueViewModel).NumValue,
                        out selectedIndex))
                    {
                        this.SelectedIndexes.Add(selectedIndex);

                    }
                }
            }

            if (e.RemovedItems != null && e.RemovedItems.Count > 0 && this.SelectedIndexes != null)
            {
                foreach (List<IFormattedValueViewModel> formattedValueViewModels in e.RemovedItems)
                {
                    int selectedIndex;
                    if (int.TryParse((formattedValueViewModels[0] as INumericValueViewModel).NumValue,
                        out selectedIndex))
                    {
                        this.SelectedIndexes.Remove(selectedIndex);

                    }
                }
            }
        }
        public static readonly DependencyProperty CellStyleProperty =
            DependencyProperty.Register("CellStyle", typeof(Style), typeof(DynamicPropertiesGridViewBehavior));

        public Style CellStyle
        {
            get { return (Style)this.GetValue(CellStyleProperty); }
            set { this.SetValue(CellStyleProperty, value); }
        }

        protected override void OnAttached()
        {
            this.AssociatedObject.SelectionChanged += this.OnSelectionChanged;
            //  _dataTableOfRecords = new DataTable();
            this.OnRowValuesChanged();
            base.OnAttached();
        }
    }
}