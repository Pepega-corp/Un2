using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Unicon2.Fragments.Configuration.Infrastructure.Keys;
using Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces.DependentProperty;
using Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces.Properties;
using Unicon2.Fragments.Configuration.Infrastructure.ViewModel;
using Unicon2.Fragments.Configuration.Model.Properties;
using Unicon2.Infrastructure.Interfaces;
using Unicon2.Infrastructure.Interfaces.Dependancy;
using Unicon2.Unity.Interfaces;

namespace Unicon2.Fragments.Configuration.Model.DependentProperty
{
    [JsonObject(MemberSerialization.OptIn)]
    public class DependentProperty : DefaultProperty, IDependentProperty
    {
        private IProperty _relatedProperty;
        private IUshortsFormatter _defaultUshortsFormatter;

        public DependentProperty()
        {
            DependancyConditions = new List<IDependancyCondition>();
        }

        [JsonProperty] public List<IDependancyCondition> DependancyConditions { get; set; }
        [JsonProperty] public ConditionResultEnum ActualConditionResult { get; set; }

        public IUshortsFormatter DeviceValueUshortsFormatter { get; set; }
        public IUshortsFormatter LocalValueUshortsFormatter { get; set; }

        public override T Accept<T>(IConfigurationItemVisitor<T> visitor)
        {
            return visitor.VisitDependentProperty(this);
        }
        //private void CheckCondition(IDependancyCondition dependancyCondition, ConditionResultChangingEventArgs evArgs)
        //{

        //    if (evArgs.ConditionResult == ConditionResultEnum.ApplyingFormatter)
        //    {
        //        if (evArgs.IsConditionTriggered)
        //        {

        //            if (evArgs.IsLocalValueTriggered)
        //            {
        //                this.LocalValueUshortsFormatter = dependancyCondition.UshortsFormatter;
        //            }
        //            else
        //            {
        //                this.DeviceValueUshortsFormatter = dependancyCondition.UshortsFormatter;
        //            }
        //            this.ConfigurationItemChangedAction?.Invoke();
        //        }
        //        else
        //        {
        //            if (evArgs.IsLocalValueTriggered)
        //            {
        //                if (this.LocalValueUshortsFormatter == dependancyCondition.UshortsFormatter)
        //                    this.LocalValueUshortsFormatter = this._defaultUshortsFormatter;
        //            }
        //            else
        //            {
        //                if (this.DeviceValueUshortsFormatter == dependancyCondition.UshortsFormatter)
        //                    this.DeviceValueUshortsFormatter = this._defaultUshortsFormatter;
        //            }
        //            this.ConfigurationItemChangedAction?.Invoke();
        //        }
        //    }
        //}
    }



}