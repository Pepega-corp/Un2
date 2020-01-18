using System.Linq;
using FluentValidation;
using Unicon2.Formatting.Infrastructure.ViewModel;
using Unicon2.Infrastructure;
using Unicon2.Infrastructure.Services;

namespace Unicon2.Formatting.Editor.ViewModels.Validators
{
   public class DictionaryMatchingFormatterValidator: AbstractValidator<IDictionaryMatchingFormatterViewModel>
    {
        public DictionaryMatchingFormatterValidator(ILocalizerService localizerService)
        {
            RuleFor(model => model.KeyValuesDictionary)
                .Must(pairs => !pairs.GroupBy(pair =>pair.Key ).Any(grouping => grouping.Count()>1))
                .WithMessage(
                    localizerService.GetLocalizedString(ApplicationGlobalNames.StatusMessages.DUBLICATE_VALUES_MESSAGE));
        }
    }
}
