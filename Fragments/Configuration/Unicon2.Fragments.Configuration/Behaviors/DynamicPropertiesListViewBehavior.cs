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
    public class DynamicPropertiesListViewBehavior : Behavior<ListView>
    {

        private DynamicPropertiesTable _journalDataTable;
        private ObservableCollection<List<ILocalAndDeviceValueContainingViewModel>> _collection;

        protected override void OnDetaching()
        {
            if (_journalDataTable != null) _journalDataTable.TableUpdateAction = null;
            base.OnDetaching();
        }

        public static readonly DependencyProperty IsTransponedProperty =
            DependencyProperty.Register("IsTransponed", typeof(bool), typeof(DynamicPropertiesListViewBehavior),
                new PropertyMetadata(false, OnIsTransponedPropertyChanged));

        public bool IsTransponed
        {
            get { return (bool) GetValue(IsTransponedProperty); }
            set { SetValue(IsTransponedProperty, value); }
        }

        private static void OnIsTransponedPropertyChanged(DependencyObject sender,
            DependencyPropertyChangedEventArgs args)
        {
            DynamicPropertiesListViewBehavior beh = sender as DynamicPropertiesListViewBehavior;
            beh.OnRowValuesChanged();
        }


        public static readonly DependencyProperty IsDeviceValuesProperty =
            DependencyProperty.Register("IsDeviceValues", typeof(bool), typeof(DynamicPropertiesListViewBehavior));

        public bool IsDeviceValues
        {
            get { return (bool) GetValue(IsDeviceValuesProperty); }
            set { SetValue(IsDeviceValuesProperty, value); }
        }

        public static readonly DependencyProperty RowValuesProperty =
            DependencyProperty.Register("RowValues", typeof(DynamicPropertiesTable),
                typeof(DynamicPropertiesListViewBehavior),
                new PropertyMetadata(null, OnRowValuesPropertyChanged));

        public DynamicPropertiesTable RowValues
        {
            get { return (DynamicPropertiesTable) GetValue(RowValuesProperty); }
            set { SetValue(RowValuesProperty, value); }
        }

        private static void OnRowValuesPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            DynamicPropertiesListViewBehavior beh = sender as DynamicPropertiesListViewBehavior;
            beh.OnRowValuesChanged();
        }

        private void OnRowValuesChanged()
        {
            if (AssociatedObject == null) return;
            if (RowValues == null) return;
            _journalDataTable = RowValues;


            if (IsTransponed)
            {
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

                _collection = new ObservableCollection<List<ILocalAndDeviceValueContainingViewModel>>();

                AssociatedObject.ItemsSource = _collection;
                _journalDataTable.Values.ForEach((list => { InsertRow(list); }));
            }

            _journalDataTable.FormattedValueViewModelAddedAction = InsertRow;
            _journalDataTable.TableUpdateAction = OnRowValuesChanged;
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
                    listToInsert.Insert(0, propertyViewModel as ILocalAndDeviceValueContainingViewModel);
                }
            }

            Application.Current.Dispatcher.Invoke(() => { _collection.Add(listToInsert); });
        }

        protected override void OnAttached()
        {
            //  this.AssociatedObject.SelectionChanged += this.OnSelectionChanged;
            //_dataTableOfRecords = new DataTable();
            OnRowValuesChanged();
            base.OnAttached();
        }
    }
}