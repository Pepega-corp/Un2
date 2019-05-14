using System;
using System.Runtime.Serialization;
using Unicon2.Infrastructure.BaseItems;
using Unicon2.Infrastructure.Interfaces;
using Unicon2.Infrastructure.Values;
using Unicon2.Unity.Interfaces;

namespace Unicon2.Formatting.Model.Base
{
    [DataContract(IsReference = true, Namespace = "UshortsFormatterBaseNS")]
    public abstract class UshortsFormatterBase : Disposable, IUshortsFormatter, IInitializableFromContainer
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

        #region Implementation of IInitializableFromContainer

        public bool IsInitialized
        {
            get { return this._isInitialized; }
        }

        public virtual void InitializeFromContainer(ITypesContainer container)
        {
            this._errorValueGettingFunc = container.Resolve<Func<IErrorValue>>();
            this._isInitialized = true;
        }



        #endregion


        #region Implementation of IStronglyNamed

        public abstract string StrongName { get; }
        #endregion

        #region Implementation of ICloneable

        public abstract object Clone();

        #endregion

        #region Implementation of INameable
        [DataMember]
        public string Name { get; set; }

        #endregion
    }

}