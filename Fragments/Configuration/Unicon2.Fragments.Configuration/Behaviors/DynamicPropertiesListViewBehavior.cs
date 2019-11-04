using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interactivity;
using Unicon2.Fragments.Configuration.Infrastructure.ViewModel.Properties;
using Unicon2.Fragments.Configuration.ViewModel.Properties;
using Unicon2.Infrastructure.Common;
using Unicon2.Presentation.Infrastructure.Factories;
using Unicon2.Presentation.Infrastructure.ViewModels;
using Unicon2.Presentation.Infrastructure.ViewModels.Values;

namespace Unicon2.Fragments.Configuration.Behaviors
{
   public class DynamicPropertiesListViewBehavior:Behavior<ListView>
    {

        private DynamicPropertiesTable _journalDataTable;
        private ObservableCollection<List<ILocalAndDeviceValueContainingViewModel>> _collection;

        #region Overrides of Behavior

        protected override void OnDetaching()
        {
            if (_journalDataTable != null) _journalDataTable.TableUpdateAction = null;
            base.OnDetaching();
        }

        #endregion
        #region IsTransponed dp

        public static readonly DependencyProperty IsTransponedProperty =
            DependencyProperty.Register("IsTransponed", typeof(bool), typeof(DynamicPropertiesListViewBehavior),
                new PropertyMetadata(false, OnIsTransponedPropertyChanged));

        public bool IsTransponed
        {
            get { return (bool)this.GetValue(IsTransponedProperty); }
            set { this.SetValue(IsTransponedProperty, value); }
        }

        private static void OnIsTransponedPropertyChanged(DependencyObject sender,
            DependencyPropertyChangedEventArgs args)
        {
            DynamicPropertiesListViewBehavior beh = sender as DynamicPropertiesListViewBehavior;
            beh.OnRowValuesChanged();
        }


        #endregion
        #region RowValues dp

        public static readonly DependencyProperty IsDeviceValuesProperty =
            DependencyProperty.Register("IsDeviceValues", typeof(bool), typeof(DynamicPropertiesListViewBehavior));

        public bool IsDeviceValues
        {
            get { return (bool)this.GetValue(IsDeviceValuesProperty); }
            set { this.SetValue(IsDeviceValuesProperty, value); }
        }

        public static readonly DependencyProperty RowValuesProperty =
            DependencyProperty.Register("RowValues", typeof(DynamicPropertiesTable), typeof(DynamicPropertiesListViewBehavior),
                new PropertyMetadata(null, OnRowValuesPropertyChanged));

        public DynamicPropertiesTable RowValues
        {
            get { return (DynamicPropertiesTable)this.GetValue(RowValuesProperty); }
            set { this.SetValue(RowValuesProperty, value); }
        }

        private static void OnRowValuesPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            DynamicPropertiesListViewBehavior beh = sender as DynamicPropertiesListViewBehavior;
            beh.OnRowValuesChanged();
        }
        #endregion
        private void OnRowValuesChanged()
        {
            if (this.AssociatedObject == null) return;
            if (this.RowValues == null) return;
            this._journalDataTable = this.RowValues;

           
            int index = 0;
            if (this.IsTransponed)
            {
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

                this._collection = new ObservableCollection<List<ILocalAndDeviceValueContainingViewModel>>();

                this.AssociatedObject.ItemsSource = this._collection;
                this._journalDataTable.Values.ForEach((list => { this.InsertRow(list); }));
            }

            this._journalDataTable.FormattedValueViewModelAddedAction = this.InsertRow;
            this._journalDataTable.TableUpdateAction = this.OnRowValuesChanged;
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
                    listToInsert.Insert(0, propertyViewModel as ILocalAndDeviceValueContainingViewModel);
                }
            }

            Application.Current.Dispatcher.Invoke(() => { this._collection.Add(listToInsert); });
        }

        #region Overrides of Behavior

        protected override void OnAttached()
        {
            //  this.AssociatedObject.SelectionChanged += this.OnSelectionChanged;
              //_dataTableOfRecords = new DataTable();
            this.OnRowValuesChanged();
            base.OnAttached();
        }

        #endregion
    }
}
