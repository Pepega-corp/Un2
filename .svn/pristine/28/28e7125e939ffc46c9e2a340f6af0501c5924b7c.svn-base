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
            this.IsBaseNumeration = isBaseNumeration;
            this.ColumnNamesStrings = columnNamesStrings;
            this.RowHeadersStrings = rowHeadersStrings;
            this.Values = new List<List<IFormattedValueViewModel>>();
        }

        public List<string> ColumnNamesStrings { get; }
        public List<string> RowHeadersStrings { get; }
        public bool IsBaseNumeration { get; }
        public List<List<IFormattedValueViewModel>> Values { get; }

        public int GetCurrentValueCount()
        {
            return this.Values.Count;
        }

       public void AddFormattedValueViewModel(List<IFormattedValueViewModel> rowList)
        {
            this.Values.Add(rowList);
            this.FormattedValueViewModelAddedAction?.Invoke(rowList);
        }

        public void AddColumn(string columnName, List<IFormattedValueViewModel> formattedValueViewModels)
        {
            this.ColumnNamesStrings.Add(columnName);
            for (int i = 0; i < formattedValueViewModels.Count; i++)
            {
                if (this.Values.Count <= i)
                {
                    this.Values.Add(new List<IFormattedValueViewModel>());
                }
                this.Values[i].Add(formattedValueViewModels[i]);
            }
        }

        public void DeleteColumn(string columnName)
        {
            int index = this.ColumnNamesStrings.IndexOf(columnName);
            this.ColumnNamesStrings.Remove(columnName);
            this.Values.ForEach((list => list.RemoveAt(index)));
        }
        
        public void Reset()
        {
            this.Values.Clear();
            this.ColumnNamesStrings.Clear();
            this.RowHeadersStrings.Clear();
        }

        public int GetValueColumn(IFormattedValueViewModel formattedValueViewModel)
        {
            foreach (List<IFormattedValueViewModel> list in this.Values)
            {
                if (list.Contains(formattedValueViewModel)) return list.IndexOf(formattedValueViewModel);
            }
            return -1;
        }

        public int GetValueRow(IFormattedValueViewModel formattedValueViewModel)
        {
            foreach (List<IFormattedValueViewModel> list in this.Values)
            {
                if (list.Contains(formattedValueViewModel)) return this.Values.IndexOf(list);
            }
            return -1;
        }


    }
}
