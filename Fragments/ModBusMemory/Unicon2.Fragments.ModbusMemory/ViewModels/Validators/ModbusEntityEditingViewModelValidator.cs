using System;
using FluentValidation;
using Unicon2.Fragments.ModbusMemory.Infrastructure.ViewModels;
using Unicon2.Infrastructure;
using Unicon2.Infrastructure.Services;

namespace Unicon2.Fragments.ModbusMemory.ViewModels.Validators
{
    public class ModbusEntityEditingViewModelValidator : AbstractValidator<IModbusEntityEditingViewModel>
    {
        public ModbusEntityEditingViewModelValidator(ILocalizerService localizerService)
        {
            RuleFor(model => model.ValueDec).Must(s =>
            {
                ushort u;
                return ushort.TryParse(s, out u);
            }).WithMessage(localizerService.GetLocalizedString(
                                ApplicationGlobalNames.ErrorMessages.VALUE_OUT_OF_RANGE_MESSAGE_KEY) + " (0-65535)");
            RuleFor(model => model.ValueHex).Must(s =>
            {
                try
                {
                    Convert.ToInt16(s, 16);
                }
                catch
                {
                    return false;
                }
                return true;
            }).WithMessage(localizerService.GetLocalizedString(
                                ApplicationGlobalNames.ErrorMessages.VALUE_OUT_OF_RANGE_MESSAGE_KEY) + " (0-65535)");
        }

    }
}