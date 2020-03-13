using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Interactivity;
using System.Windows.Markup;
using Unicon2.Fragments.Configuration.Infrastructure.ViewModel.Properties;
using Unicon2.Fragments.Configuration.ViewModel.Properties;
using Unicon2.Infrastructure.Common;
using Unicon2.Presentation.Infrastructure.Factories;
using Unicon2.Presentation.Infrastructure.ViewModels;
using Unicon2.Presentation.Infrastructure.ViewModels.Values;
using Unicon2.SharedResources.DataTemplateSelectors;

namespace Unicon2.Fragments.Configuration.Behaviors
{
    public class DynamicPropertiesGridViewBehavior : Behavior<GridView>
    {

        private DynamicPropertiesTable _journalDataTable;

        protected override void OnDetaching()
        {
            if (_journalDataTable != null) _journalDataTable.TableUpdateAction = null;
            base.OnDetaching();
        }

        public static readonly DependencyProperty IsDeviceValuesProperty =
            DependencyProperty.Register("IsDeviceValues", typeof(bool), typeof(DynamicPropertiesGridViewBehavior));

        public bool IsDeviceValues
        {
            get { return (bool) GetValue(IsDeviceValuesProperty); }
            set { SetValue(IsDeviceValuesProperty, value); }
        }

        public static readonly DependencyProperty RowValuesProperty =
            DependencyProperty.Register("RowValues", typeof(DynamicPropertiesTable),
                typeof(DynamicPropertiesGridViewBehavior),
                new PropertyMetadata(null, OnRowValuesPropertyChanged));

        public DynamicPropertiesTable RowValues
        {
            get { return (DynamicPropertiesTable) GetValue(RowValuesProperty); }
            set { SetValue(RowValuesProperty, value); }
        }

        private static void OnRowValuesPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            DynamicPropertiesGridViewBehavior beh = sender as DynamicPropertiesGridViewBehavior;
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
            }

            _journalDataTable.TableUpdateAction = OnRowValuesChanged;
        }


        public static readonly DependencyProperty IsTransponedProperty =
            DependencyProperty.Register("IsTransponed", typeof(bool), typeof(DynamicPropertiesGridViewBehavior),
                new PropertyMetadata(false, OnIsTransponedPropertyChanged));

        public bool IsTransponed
        {
            get { return (bool) GetValue(IsTransponedProperty); }
            set { SetValue(IsTransponedProperty, value); }
        }

        private static void OnIsTransponedPropertyChanged(DependencyObject sender,
            DependencyPropertyChangedEventArgs args)
        {
            DynamicPropertiesGridViewBehavior beh = sender as DynamicPropertiesGridViewBehavior;
            beh.OnRowValuesChanged();
        }


        private GridViewColumn CreateGridTemplateColumn(int index, string columnName)
        {
            // DataTemplate dataTemplate1 = new DataTemplate();
            // dataTemplate1.VisualTree = new FrameworkElementFactory(typeof(ContentPresenter));
            //var contentPresenterHigh = new ContentPresenter();

            // if (IsDeviceValues)
            // {
            //     Binding b11 = new Binding(".[" + index + "].DeviceValue");
            //     //dataTemplate1.VisualTree.SetValue(ContentPresenter.HorizontalAlignmentProperty,HorizontalAlignment.Center);
            //     dataTemplate1.VisualTree.SetValue(ContentPresenter.ContentProperty, b11);
            // }
            // else
            // {
            //     Binding b11 = new Binding(".[" + index + "].LocalValue");
            //     //dataTemplate1.VisualTree.SetValue(ContentPresenter.HorizontalAlignmentProperty,HorizontalAlignment.Center);
            //     dataTemplate1.VisualTree.SetValue(ContentPresenter.ContentProperty, b11);
            // }
            // DataTemplateSelector dataTemplateSelector1 = new ViewModelByStrongNameDataTemplateSelector();
            // dataTemplate1.VisualTree.SetValue(ContentPresenter.ContentTemplateSelectorProperty, dataTemplateSelector1);

            DataTemplate cellDataTemplate = new DataTemplate();




            var innerVisualTree = new FrameworkElementFactory(typeof(ContentControl));
            innerVisualTree.SetValue(FrameworkElement.StyleProperty, CellStyle);
            if (IsDeviceValues)
            {
                Binding b11 = new Binding(".[" + index + "].DeviceValue");
                //dataTemplate1.VisualTree.SetValue(ContentPresenter.HorizontalAlignmentProperty,HorizontalAlignment.Center);
                innerVisualTree.SetValue(ContentControl.ContentProperty, b11);
            }
            else
            {
                Binding b11 = new Binding(".[" + index + "].LocalValue");
                //dataTemplate1.VisualTree.SetValue(ContentPresenter.HorizontalAlignmentProperty,HorizontalAlignment.Center);
                innerVisualTree.SetValue(ContentControl.ContentProperty, b11);
            }

            DataTemplateSelector dataTemplateSelector1 = new ViewModelByStrongNameDataTemplateSelector();
            innerVisualTree.SetValue(ContentControl.ContentTemplateSelectorProperty, dataTemplateSelector1);


            cellDataTemplate.VisualTree = innerVisualTree;


            GridViewColumn dataGridTemplateColumn = new GridViewColumn
            {
                Header = columnName,
                //IsReadOnly = true,
                CellTemplate = cellDataTemplate
            };

            return dataGridTemplateColumn;
        }

        public static readonly DependencyProperty CellStyleProperty =
            DependencyProperty.Register("CellStyle", typeof(Style), typeof(DynamicPropertiesGridViewBehavior));

        public Style CellStyle
        {
            get { return (Style) GetValue(CellStyleProperty); }
            set { SetValue(CellStyleProperty, value); }
        }

        public static readonly DependencyProperty SelectedIndexesProperty =
            DependencyProperty.Register("SelectedIndexes", typeof(List<int>), typeof(DynamicPropertiesGridViewBehavior),
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


        protected override void OnAttached()
        {
            //  this.AssociatedObject.SelectionChanged += this.OnSelectionChanged;
            //  _dataTableOfRecords = new DataTable();
            OnRowValuesChanged();
            base.OnAttached();
        }
    }

    public static class ExtensionMethods
    {
        public static T XamlClone<T>(this T original)
            where T : class
        {
            if (original == null)
                return null;

            object clone;
            using (var stream = new MemoryStream())
            {
                XamlWriter.Save(original, stream);
                stream.Seek(0, SeekOrigin.Begin);
                clone = XamlReader.Load(stream);
            }

            if (clone is T)
                return (T) clone;
            else
                return null;
        }
    }
}