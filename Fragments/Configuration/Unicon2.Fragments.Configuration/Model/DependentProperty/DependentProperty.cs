using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Unicon2.Fragments.Configuration.Infrastructure.Keys;
using Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces.DependentProperty;
using Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces.Properties;
using Unicon2.Fragments.Configuration.Model.Properties;
using Unicon2.Infrastructure.Interfaces;
using Unicon2.Infrastructure.Interfaces.Dependancy;
using Unicon2.Unity.Interfaces;

namespace Unicon2.Fragments.Configuration.Model.DependentProperty
{
    [DataContract(Namespace = "DependentPropertyNS", Name = nameof(DependentProperty), IsReference = true)]
    public class DependentProperty : DefaultProperty, IDependentProperty
    {
        private IProperty _relatedProperty;
        private IUshortsFormatter _defaultUshortsFormatter;

        public DependentProperty(Func<IRange> range) : base(range)
        {
            this.DependancyConditions = new List<IDependancyCondition>();
        }

        #region Implementation of IDependentProperty
        [DataMember]

        public List<IDependancyCondition> DependancyConditions { get; set; }
        [DataMember]

        public ConditionResultEnum ActualConditionResult { get; set; }

        public IUshortsFormatter DeviceValueUshortsFormatter { get; set; }
        public IUshortsFormatter LocalValueUshortsFormatter { get; set; }

        #endregion


        #region Overrides of DefaultProperty

        public override string StrongName => ConfigurationKeys.DEPENDENT_PROPERTY;


        private void CheckCondition(IDependancyCondition dependancyCondition, ConditionResultChangingEventArgs evArgs)
        {

            if (evArgs.ConditionResult == ConditionResultEnum.ApplyingFormatter)
            {
                if (evArgs.IsConditionTriggered)
                {

                    if (evArgs.IsLocalValueTriggered)
                    {
                        this.LocalValueUshortsFormatter = dependancyCondition.UshortsFormatter;
                    }
                    else
                    {
                        this.DeviceValueUshortsFormatter = dependancyCondition.UshortsFormatter;
                    }
                    this.ConfigurationItemChangedAction?.Invoke();
                }
                else
                {
                    if (evArgs.IsLocalValueTriggered)
                    {
                        if (this.LocalValueUshortsFormatter == dependancyCondition.UshortsFormatter)
                            this.LocalValueUshortsFormatter = this._defaultUshortsFormatter;
                    }
                    else
                    {
                        if (this.DeviceValueUshortsFormatter == dependancyCondition.UshortsFormatter)
                            this.DeviceValueUshortsFormatter = this._defaultUshortsFormatter;
                    }
                    this.ConfigurationItemChangedAction?.Invoke();
                }
            }
        }


        #region Overrides of DefaultProperty

        public override void InitializeFromContainer(ITypesContainer container)
        {
            foreach (IDependancyCondition condition in this.DependancyConditions)
            {
                condition.InitializeFromContainer(container);
            }
            base.InitializeFromContainer(container);
        }

        #endregion

        #endregion


        [OnDeserialized]
        private void OnDeserialized(StreamingContext sc)
        {
            if (this.DependancyConditions != null)
            {
                foreach (IDependancyCondition dependancyCondition in this.DependancyConditions)
                {
                    dependancyCondition.ConditionResultChangedAction += (evArgs) =>
                    {
                        this.CheckCondition(dependancyCondition, evArgs);
                    };
                }
            }
        }
    }



}
