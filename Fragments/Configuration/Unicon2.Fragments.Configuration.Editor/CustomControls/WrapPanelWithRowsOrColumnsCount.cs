namespace Unicon2.Fragments.Configuration.Editor.CustomControls
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;

    public class WrapPanelWithRowsOrColumnsCount : WrapPanel
    {
        public static readonly DependencyProperty RowsOrColumnsCountProperty =
            DependencyProperty.Register("RowsOrColumnsCount", typeof(int), typeof(WrapPanelWithRowsOrColumnsCount), new PropertyMetadata(int.MaxValue));

        public int RowsOrColumnsCount
        {
            get { return (int)this.GetValue(RowsOrColumnsCountProperty); }
            set { this.SetValue(RowsOrColumnsCountProperty, Math.Max(value, 1)); }
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            if (this.Children.Count > 0)
            {
                Size newAvailableSize;

                if (this.Orientation == Orientation.Horizontal)
                {
                    double suitableWidth = this.EstimateSuitableRowOrColumnLength(this.Children.Cast<UIElement>(),
                                                                        true,
                                                                        availableSize,
                                                                        this.RowsOrColumnsCount);

                    newAvailableSize =
                        double.IsNaN(suitableWidth) || suitableWidth <= 0
                            ? availableSize
                            : new Size(Math.Min(suitableWidth, availableSize.Width), availableSize.Height);
                }
                else
                {
                    double suitableHeigth = this.EstimateSuitableRowOrColumnLength(this.Children.Cast<UIElement>(),
                                                                        false,
                                                                        availableSize,
                                                                        this.RowsOrColumnsCount);
                    newAvailableSize =
                        double.IsNaN(suitableHeigth) || suitableHeigth <= 0
                            ? availableSize
                            : new Size(availableSize.Width, Math.Min(suitableHeigth, availableSize.Height));
                }

                return base.MeasureOverride(newAvailableSize);
            }
            else
            {
                return base.MeasureOverride(availableSize);
            }
        }

        private double EstimateSuitableRowOrColumnLength(IEnumerable<UIElement> elements,
                                                            bool trueRowsFalseColumns,
                                                            Size availableSize,
                                                            int rowsOrColumnsCount)
        {
            List<UIElement> elementsList = elements.ToList();

            List<double> desiredLengths = elementsList.Select(el => this.DesiredLength(el, availableSize, trueRowsFalseColumns)).ToList();

            double maxLength = desiredLengths.Where(length => !double.IsNaN(length)).Concat(new[] { 0.0 }).Max();

            if (maxLength <= 0.0)
            {
                return double.NaN;
            }

            List<double> desiredLengthsRepaired = desiredLengths.Select(length => double.IsNaN(length) ? maxLength : length).ToList();

            double totalDesiredLength = desiredLengthsRepaired.Sum();

            int maxCount = Math.Min(rowsOrColumnsCount, elementsList.Count);

            double suitableRowOrColumnLength = totalDesiredLength / maxCount;

            double nextLengthIncrement;

            while (this.CountRowsOrColumnsNumber(desiredLengthsRepaired, suitableRowOrColumnLength, out nextLengthIncrement) > maxCount)
            {
                suitableRowOrColumnLength += nextLengthIncrement;
            }

            suitableRowOrColumnLength = Math.Max(suitableRowOrColumnLength, desiredLengthsRepaired.Max());

            return suitableRowOrColumnLength;
        }

        private int CountRowsOrColumnsNumber(List<double> desiredLengths, double rowOrColumnLengthLimit, out double nextLengthIncrement)
        {
            int rowOrColumnCount = 1;
            double currentCumulativeLength = 0;
            bool nextNewRowOrColumn = false;

            double minimalIncrement = double.MaxValue;

            foreach (double desiredLength in desiredLengths)
            {
                if (nextNewRowOrColumn)
                {
                    rowOrColumnCount++;
                    currentCumulativeLength = 0;
                    nextNewRowOrColumn = false;
                }

                if (currentCumulativeLength + desiredLength > rowOrColumnLengthLimit)
                {
                    minimalIncrement = Math.Min(minimalIncrement,
                                                currentCumulativeLength + desiredLength - rowOrColumnLengthLimit);

                    if (currentCumulativeLength == 0)
                    {
                        nextNewRowOrColumn = true;
                        currentCumulativeLength = 0;
                    }
                    else
                    {
                        rowOrColumnCount++;
                        currentCumulativeLength = desiredLength;
                    }
                }
                else
                {
                    currentCumulativeLength += desiredLength;
                }
            }

            nextLengthIncrement = minimalIncrement != double.MaxValue ? minimalIncrement : 1;

            return rowOrColumnCount;
        }

        private double DesiredLength(UIElement el, Size availableSize, bool trueRowsFalseColumns)
        {
            el.Measure(availableSize);
            Size next = el.DesiredSize;

            double length = trueRowsFalseColumns ? next.Width : next.Height;

            if (double.IsInfinity(length) || double.IsNaN(length))
            {
                return double.NaN;
            }
            else
            {
                return length;
            }
        }
    }
}
