using System;
using System.Collections.Generic;

namespace Unicon2.Formatting.Model
{
    public class IterationDefinition
    {
        public string FormulaString { get; set; }
        public List<string> ArgumentNames { get; set; }
        public List<double> ArgumentValues { get; set; }
        public double Result { get; set; }
        public int NumberOfSimbolsAfterComma { get; set; }
        public bool CheckEquality(IterationDefinition c)
        {
            if (FormulaString != c.FormulaString) return false;
            if (ArgumentNames.Count != c.ArgumentNames.Count) return false;
            if (ArgumentValues.Count != c.ArgumentValues.Count) return false;
            if (NumberOfSimbolsAfterComma != c.NumberOfSimbolsAfterComma) return false;
            for (int i = 0; i < ArgumentNames.Count; i++)
            {
                if (ArgumentNames[i] != c.ArgumentNames[i]) return false;
            }
            for (int i = 0; i < ArgumentValues.Count; i++)
            {
                if (Math.Abs(ArgumentValues[i] - c.ArgumentValues[i]) > 0.01) return false;
            }
            return true;
        }
    }
}