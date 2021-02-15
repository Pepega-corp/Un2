using System;
using System.Collections.Generic;
using System.Linq;
using Unicon2.Fragments.Configuration.Editor.Interfaces;
using Unicon2.Fragments.Configuration.Editor.ViewModels;
using Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces;
using Unicon2.Fragments.Configuration.Model;
using Unicon2.Infrastructure.Common;
using Unicon2.Infrastructure.Functional;

namespace Unicon2.Fragments.Configuration.Editor.Helpers
{
    public class BaseValuesFillHelper
    {
        public BaseValuesFillHelper()
        {

        }

        public IBaseValuesViewModel CreateBaseValuesViewModel(IConfigurationBaseValues configurationBaseValues)
        {
            if (configurationBaseValues == null)
            {
                return new BaseValuesViewModel();
            }
            var res = new BaseValuesViewModel();
            res.BaseValuesViewModels.AddCollection(configurationBaseValues.BaseValues.Select(value =>
                new BaseValueViewModel()
                {
                    Name = value.Name,
                    BaseValuesMemory =
                        Result<Dictionary<ushort, ushort>>.Create(value.LocalMemoryValues, true),
                    IsBaseValuesMemoryFilled = true
                }));
            return res;
        }

        public IConfigurationBaseValues CreateConfigurationBaseValues(IBaseValuesViewModel baseValuesViewModel)
        {
            if (baseValuesViewModel == null)
            {
                return new ConfigurationBaseValues();
            }
            var res = new ConfigurationBaseValues()
            {
                BaseValues = baseValuesViewModel.BaseValuesViewModels.Select(model => new ConfigurationBaseValue()
                        {Name = model.Name, LocalMemoryValues = model.BaseValuesMemory.Item})
                    .Cast<IConfigurationBaseValue>().ToList()
            };
            return res;
        }
    }
}