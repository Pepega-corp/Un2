using org.mariuszgromada.math.mxparser;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using Unicon2.Formatting.Infrastructure.Model;
using Unicon2.Formatting.Model.Base;
using Unicon2.Infrastructure.DeviceInterfaces;
using Unicon2.Infrastructure.Interfaces.DataOperations;
using Unicon2.Infrastructure.Interfaces.Values;
using Unicon2.Infrastructure.Values;
using Unicon2.Unity.Interfaces;
using Expression = org.mariuszgromada.math.mxparser.Expression;

namespace Unicon2.Formatting.Model
{
    [DataContract(Name = nameof(FormulaFormatter), Namespace = "FormulaFormatterNS", IsReference = true)]
    public class FormulaFormatter : UshortsFormatterBase, IFormulaFormatter, ILoadable
    {
        internal class IterationDefinition
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
        
        private Func<INumericValue> _numericValueGettingFunc;
        private ConcurrentBag<IterationDefinition> _iterationDefinitionsCache;
        private ushort _numberOfSimbolsAfterComma;

        public FormulaFormatter(Func<INumericValue> numericValueGettingFunc)
        {
            this._numericValueGettingFunc = numericValueGettingFunc;
            this.UshortFormattables = new List<IUshortFormattable>();
        }



        public override string StrongName => nameof(FormulaFormatter);

        protected override IFormattedValue OnFormatting(ushort[] ushorts)
        {
            INumericValue formattedValue = this._numericValueGettingFunc();
            IterationDefinition iterationDefinition = new IterationDefinition();
            iterationDefinition.FormulaString = this.FormulaString;

            iterationDefinition.ArgumentNames = new List<string>();
            iterationDefinition.ArgumentValues = new List<double>();
            iterationDefinition.ArgumentNames.Add("x");
            iterationDefinition.ArgumentValues.Add(ushorts[0]);
            if (this.UshortFormattables != null)
            {
                int index = 1;
                foreach (IUshortFormattable formattableUshortResource in this.UshortFormattables)
                {
                    if (formattableUshortResource is IDeviceValueContaining)
                    {
                        IFormattedValue value = formattableUshortResource.UshortsFormatter
                            .Format((formattableUshortResource as IDeviceValueContaining).DeviceUshortsValue);
                        if (value is INumericValue)
                        {
                            double num = (value as INumericValue).NumValue;
                            iterationDefinition.ArgumentNames.Add("x" + index++);
                            iterationDefinition.ArgumentValues.Add(num);
                        }
                    }
                }
            }

            formattedValue.NumValue = this.MemoizeCalculateResult(iterationDefinition);
            return formattedValue;
        }

        private double MemoizeCalculateResult(IterationDefinition iterationDefinition)
        {

            if (this._iterationDefinitionsCache == null)
            {
                this._iterationDefinitionsCache = new ConcurrentBag<IterationDefinition>();
            }
            else
            {
                IterationDefinition iteration = this._iterationDefinitionsCache.FirstOrDefault(definition => definition.CheckEquality(iterationDefinition));
                if (iteration != null)
                {
                    return iteration.Result;
                }
            }

            Expression expression = new Expression(this.FormulaString);
            List<Argument> arguments = new List<Argument>();
            for (int i = 0; i < iterationDefinition.ArgumentValues.Count; i++)
            {
                arguments.Add(new Argument(iterationDefinition.ArgumentNames[i], iterationDefinition.ArgumentValues[i]));
            }
            expression.addArguments(arguments.ToArray());
            double result = expression.calculate();
            if (Double.IsNaN(result)) throw new ArgumentException();

            result = Math.Round(result, this.NumberOfSimbolsAfterComma);

            iterationDefinition.Result = result;
            this._iterationDefinitionsCache.Add(iterationDefinition);
            return result;
        }

        public override ushort[] FormatBack(IFormattedValue formattedValue)
        {
            try
            {
                (formattedValue as INumericValue).NumValue = Math.Round((formattedValue as INumericValue).NumValue, this.NumberOfSimbolsAfterComma);
                string numstr = (formattedValue as INumericValue).NumValue.ToString().Replace(',', '.');
                Expression expression = new Expression("solve(" + "(" + this.FormulaString + ")" + "-" + numstr + ",x,0," + ushort.MaxValue + ")");

                if (this.UshortFormattables != null)
                {
                    int index = 1;
                    foreach (IUshortFormattable formattableUshortResource in this.UshortFormattables)
                    {
                        if (formattableUshortResource is IDeviceValueContaining)
                        {
                            IFormattedValue value = formattableUshortResource.UshortsFormatter.Format((formattableUshortResource as IDeviceValueContaining).DeviceUshortsValue);
                            if (value is INumericValue)
                            {
                                double num = (value as INumericValue).NumValue;
                                expression.addArguments(new Argument("x" + index++, num));
                            }
                        }
                    }
                }
                double x = expression.calculate();
                if (double.IsNaN(x)) throw new ArgumentException();

                return new[] { (ushort)x };

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public override object Clone()
        {
            FormulaFormatter cloneFormulaFormatter = new FormulaFormatter(this._numericValueGettingFunc);
            cloneFormulaFormatter.FormulaString = this.FormulaString;
            cloneFormulaFormatter.NumberOfSimbolsAfterComma = this.NumberOfSimbolsAfterComma;
            return cloneFormulaFormatter;
        }

        [DataMember(Name = nameof(FormulaString))]
        public string FormulaString { get; set; }
        [DataMember]
        public List<IUshortFormattable> UshortFormattables { get; set; }

        [DataMember]

        public ushort NumberOfSimbolsAfterComma
        {
            get { return this._numberOfSimbolsAfterComma; }
            set
            {
                this._numberOfSimbolsAfterComma = value;
                this._iterationDefinitionsCache = new ConcurrentBag<IterationDefinition>();
            }
        }


        public override void InitializeFromContainer(ITypesContainer container)
        {
            this._numericValueGettingFunc = container.Resolve(typeof(Func<INumericValue>)) as Func<INumericValue>;
            base.InitializeFromContainer(container);
        }


        private IDataProvider _dataProvider; //
        public void SetDataProvider(IDataProvider dataProvider)
        {
            this._dataProvider = dataProvider;
        }

        public async Task Load()
        {
            if (this.UshortFormattables?.Count > 0)
            {
                foreach (IUshortFormattable ushortFormattable in this.UshortFormattables)
                {
                    if ((ushortFormattable as IDeviceValueContaining)?.DeviceUshortsValue != null) return;
                    await (ushortFormattable as ILoadable)?.Load();
                }
            }
        }
    }
}