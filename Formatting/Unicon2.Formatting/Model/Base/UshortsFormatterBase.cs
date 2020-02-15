using System;
using System.Runtime.Serialization;
using Unicon2.Infrastructure.BaseItems;
using Unicon2.Infrastructure.Interfaces;
using Unicon2.Infrastructure.Values;
using Unicon2.Unity.Interfaces;

namespace Unicon2.Formatting.Model.Base
{
    [DataContract(IsReference = true, Namespace = "UshortsFormatterBaseNS")]
    public abstract class UshortsFormatterBase : Disposable, IUshortsFormatter
    {
        protected bool _isInitialized;
        private Func<IErrorValue> _errorValueGettingFunc;

        public IFormattedValue Format(ushort[] ushorts)
        {
            try
            {
                return this.OnFormatting(ushorts);
            }
            catch (Exception e)
            {
                IErrorValue errorValue = this._errorValueGettingFunc();
                errorValue.ErrorMessage = e.Message;
                return errorValue;
            }
        }

        public abstract ushort[] FormatBack(IFormattedValue formattedValue);


        protected abstract IFormattedValue OnFormatting(ushort[] ushorts);

        public bool IsInitialized
        {
            get { return this._isInitialized; }
        }

        public virtual void InitializeFromContainer(ITypesContainer container)
        {
            this._errorValueGettingFunc = container.Resolve<Func<IErrorValue>>();
            this._isInitialized = true;
        }


        public abstract string StrongName { get; }

        public abstract object Clone();

        [DataMember]
        public string Name { get; set; }
    }

}