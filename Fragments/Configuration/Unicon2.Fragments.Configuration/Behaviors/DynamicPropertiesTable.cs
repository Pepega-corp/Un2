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
            this.IsBaseNumeration = isBaseNumeration;
            this.ColumnNamesStrings = columnNamesStrings;
            this.RowHeadersStrings = rowHeadersStrings;
            this.Values = new List<List<ILocalAndDeviceValueContainingViewModel>>();
        }

        public List<string> ColumnNamesStrings { get; }
        public List<string> RowHeadersStrings { get; }
        public bool IsBaseNumeration { get; }
        public List<List<ILocalAndDeviceValueContainingViewModel>> Values { get; }

        public int GetCurrentValueCount()
        {
            return this.Values.Count;
        }

        public void AddPropertyViewModel(List<ILocalAndDeviceValueContainingViewModel> rowList)
        {
            this.Values.Add(rowList);
            this.FormattedValueViewModelAddedAction?.Invoke(rowList);
        }

        public void Reset()
        {
            this.Values.Clear();
            this.ColumnNamesStrings.Clear();
            this.RowHeadersStrings.Clear();
        }


    }
}

