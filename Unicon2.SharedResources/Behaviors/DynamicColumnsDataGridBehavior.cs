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

        #region Overrides of Behavior

        protected override void OnDetaching()
        {
            if (_journalDataTable != null) _journalDataTable.TableUpdateAction = null;
            base.OnDetaching();
        }

        #endregion
        #region RowValues dp

       

        public static readonly DependencyProperty RowValuesProperty =
          DependencyProperty.Register("RowValues", typeof(DynamicDataTable), typeof(DynamicColumnsDataGridBehavior),
              new PropertyMetadata(null, OnRowValuesPropertyChanged));

        public DynamicDataTable RowValues
        {
            get { return (DynamicDataTable)this.GetValue(RowValuesProperty); }
            set { this.SetValue(RowValuesProperty, value); }
        }

        private static void OnRowValuesPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            DynamicColumnsDataGridBehavior beh = sender as DynamicColumnsDataGridBehavior;
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
                    this._collection = new ObservableCollection<List<IFormattedValueViewModel>>();

                    int rowIndex = 0;
                    foreach (string columnNameString in this._journalDataTable.ColumnNamesStrings)
                    {
                        List<IFormattedValueViewModel> formattedValueViewModels = new List<IFormattedValueViewModel>();
                        this._journalDataTable.Values.ForEach((list => formattedValueViewModels.Add(list[rowIndex])));

                        rowIndex++;
                        this.InsertRow(formattedValueViewModels);
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
                this._collection = new ObservableCollection<List<IFormattedValueViewModel>>();

                this.AssociatedObject.ItemsSource = this._collection;
                this._journalDataTable.Values.ForEach((list => { this.InsertRow(list); }));
            }
            this._journalDataTable.FormattedValueViewModelAddedAction = this.InsertRow;
            this._journalDataTable.TableUpdateAction = this.OnRowValuesChanged;
        }



        #endregion


        #region IsTransponed dp

        public static readonly DependencyProperty IsTransponedProperty =
            DependencyProperty.Register("IsTransponed", typeof(bool), typeof(DynamicColumnsDataGridBehavior),
                new PropertyMetadata(false, OnIsTransponedPropertyChanged));

        public bool IsTransponed
        {
            get { return (bool)this.GetValue(IsTransponedProperty); }
            set { this.SetValue(IsTransponedProperty, value); }
        }

        private static void OnIsTransponedPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            DynamicColumnsDataGridBehavior beh = sender as DynamicColumnsDataGridBehavior;
            beh.OnRowValuesChanged();
        }





        #endregion

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

            if (this.IsTransponed)
            {
                if (this._journalDataTable.ColumnNamesStrings != null)
                {
                    IStringValueViewModel stringValueViewModel =
                     StaticContainer.Container.Resolve<IStringValueViewModel>();
                    stringValueViewModel.StringValue = this._journalDataTable.ColumnNamesStrings[this._collection.Count];
                    listToInsert.Insert(0, stringValueViewModel);
                }
                if ((this._journalDataTable.IsBaseNumeration) && (this._journalDataTable.RowHeadersStrings != null))
                {
                    INumericValueViewModel numericValueViewModel =
                        StaticContainer.Container.Resolve<INumericValueViewModel>();
                    numericValueViewModel.NumValue = (this._collection.Count + 1).ToString();
                    listToInsert.Insert(0, numericValueViewModel);
                }
            }
            else
            {
                if ((this._journalDataTable.IsBaseNumeration) && this._journalDataTable.ColumnNamesStrings != null)
                {
                    INumericValueViewModel numericValueViewModel =
                       StaticContainer.Container.Resolve<INumericValueViewModel>();
                    numericValueViewModel.NumValue = (this._collection.Count + 1).ToString();
                    listToInsert.Insert(0, numericValueViewModel);
                }
                if ((this._journalDataTable.RowHeadersStrings != null))
                {
                    IStringValueViewModel stringValueViewModel =
                        StaticContainer.Container.Resolve<IStringValueViewModel>();
                    if (this._journalDataTable.RowHeadersStrings.Count > this._collection.Count)
                    {
                        stringValueViewModel.StringValue = this._journalDataTable.RowHeadersStrings[this._collection.Count];
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
                this._collection.Add(listToInsert);
            });
        }

        public static readonly DependencyProperty SelectedIndexesProperty =
            DependencyProperty.Register("SelectedIndexes", typeof(List<int>), typeof(DynamicColumnsDataGridBehavior),
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


        #region Overrides of Behavior

        protected override void OnAttached()
        {
            this.AssociatedObject.SelectionChanged += this.OnSelectionChanged;
            //  _dataTableOfRecords = new DataTable();
            this.OnRowValuesChanged();
            base.OnAttached();
        }
        
        #endregion
    }
}