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
        public bool CheckEquality(IterationDefinition c)
        {
            if (this.FormulaString != c.FormulaString) return false;
            if (this.ArgumentNames.Count != c.ArgumentNames.Count) return false;
            if (this.ArgumentValues.Count != c.ArgumentValues.Count) return false;
            for (int i = 0; i < this.ArgumentNames.Count; i++)
            {
                if (this.ArgumentNames[i] != c.ArgumentNames[i]) return false;
            }
            for (int i = 0; i < this.ArgumentValues.Count; i++)
            {
                if (Math.Abs(this.ArgumentValues[i] - c.ArgumentValues[i]) > 0.01) return false;
            }
            return true;
        }
    }
}