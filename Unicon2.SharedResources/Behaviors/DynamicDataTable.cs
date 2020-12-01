using System;
using System.Collections.Generic;
using Unicon2.Presentation.Infrastructure.ViewModels.Values;

namespace Unicon2.SharedResources.Behaviors
{
    public class DynamicDataTable
    {
        public Action TableUpdateAction { get; set; }
        public Action<List<IFormattedValueViewModel>> FormattedValueViewModelAddedAction { get; set; }

        public DynamicDataTable(List<string> columnNamesStrings, List<string> rowHeadersStrings, bool isBaseNumeration)
        {
            IsBaseNumeration = isBaseNumeration;
            ColumnNamesStrings = columnNamesStrings;
            RowHeadersStrings = rowHeadersStrings;
            Values = new List<List<IFormattedValueViewModel>>();
        }

        public List<string> ColumnNamesStrings { get; }
        public List<string> RowHeadersStrings { get; }
        public bool IsBaseNumeration { get; }
        public List<List<IFormattedValueViewModel>> Values { get; }


        public int GetCurrentValueCount()
        {
            return Values.Count;
        }

       public void AddFormattedValueViewModel(List<IFormattedValueViewModel> rowList)
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
