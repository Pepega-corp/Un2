using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unicon2.Presentation.Infrastructure.ViewModels;

namespace Unicon2.Fragments.Configuration.Behaviors
{
    public class DynamicPropertiesTable
    {

        public Action TableUpdateAction { get; set; }
        public Action<List<ILocalAndDeviceValueContainingViewModel>> FormattedValueViewModelAddedAction { get; set; }

        public DynamicPropertiesTable(List<string> columnNamesStrings, List<string> rowHeadersStrings,
            bool isBaseNumeration)
        {
            IsBaseNumeration = isBaseNumeration;
            ColumnNamesStrings = columnNamesStrings;
            RowHeadersStrings = rowHeadersStrings;
            Values = new List<List<ILocalAndDeviceValueContainingViewModel>>();
        }

        public List<string> ColumnNamesStrings { get; }
        public List<string> RowHeadersStrings { get; }
        public bool IsBaseNumeration { get; }
        public List<List<ILocalAndDeviceValueContainingViewModel>> Values { get; }

        public int GetCurrentValueCount()
        {
            return Values.Count;
        }

        public void AddPropertyViewModel(List<ILocalAndDeviceValueContainingViewModel> rowList)
        {
            Values.Add(rowList);
            FormattedValueViewModelAddedAction?.Invoke(rowList);
        }

        public void Reset()
        {
            Values.Clear();
            ColumnNamesStrings.Clear();
            RowHeadersStrings.Clear();
        }


    }
}

