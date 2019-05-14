using System;
using System.Globalization;
using System.Windows.Controls;

namespace Oscilloscope.View.MainItem
{
    public class DoubleValidationRule : ValidationRule
    {
        public DoubleValidationRule()
        {
            this.Min = double.MinValue;
            this.Max = double.MaxValue;
        }

        public double Min { get; set; }
        public double Max { get; set; }


        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            double age;
            string strVal = (string) value;
            if (strVal == null)
            {
                return new ValidationResult(false, "Допускаются только вещественные значения");
            }
            bool result = double.TryParse(strVal, NumberStyles.AllowDecimalPoint, CultureInfo.CurrentCulture, out age);
            if (!result)
            {
                return new ValidationResult(false, "Допускаются только вещественные значения");
            }

            if ((age < this.Min) || (age > this.Max))
            {
                return new ValidationResult(false, "Пожалуйста введите значение в диапазоне: " + this.Min + " - " + this.Max + ".");
            }

            return new ValidationResult(true, null);
        }
    }


    public class IsIntValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            double age = 0;

            try
            {
                age = string.IsNullOrEmpty(value.ToString()) ? 0 : uint.Parse((string)value, NumberStyles.AllowDecimalPoint, CultureInfo.CurrentCulture);
            }
            catch (Exception e)
            {
                return new ValidationResult(false, "Неверное значение");
            }
            return new ValidationResult(true, null);
        }
    }
}