using Unicon2.Formatting.Factories;
using Unicon2.Formatting.Infrastructure.Factories;
using Unicon2.Formatting.Infrastructure.Keys;
using Unicon2.Formatting.Model;
using Unicon2.Formatting.Services;
using Unicon2.Infrastructure.Interfaces;
using Unicon2.Presentation.Infrastructure.Services.Formatting;
using Unicon2.Unity.Interfaces;

namespace Unicon2.Formatting.Module
{
    public class FormattingModule : IUnityModule
    {
        public void Initialize(ITypesContainer container)
        {
            container.Register(typeof(IUshortsFormatter), typeof(AsciiStringFormatter),
                StringKeys.ASCII_STRING_FORMATTER);
            container.Register(typeof(IUshortsFormatter), typeof(DictionaryMatchingFormatter),
                StringKeys.DICTIONARY_MATCHING_FORMATTER);
            container.Register(typeof(IUshortsFormatter), typeof(FormulaFormatter), StringKeys.FORMULA_FORMATTER);
            container.Register(typeof(IUshortsFormatter), typeof(DirectUshortFormatter),
                StringKeys.DIRECT_USHORT_FORMATTER);
            container.Register(typeof(IUshortsFormatter), typeof(BoolFormatter), StringKeys.BOOL_FORMATTER);
            container.Register(typeof(IUshortsFormatter), typeof(DefaultTimeFormatter),
                StringKeys.DEFAULT_TIME_FORMATTER);
            container.Register(typeof(IUshortsFormatter), typeof(UshortToIntegerFormatter),
                StringKeys.USHORT_TO_INTEGER_FORMATTER);
            container.Register(typeof(IUshortsFormatter), typeof(DefaultBitMaskFormatter),
                StringKeys.DEFAULT_BIT_MASK_FORMATTER);
            container.Register(typeof(IUshortsFormatter), typeof(StringFormatter1251), StringKeys.STRING_FORMATTER1251);
            container.Register(typeof(IFormatterFactory), typeof(FormatterFactory));
            container.Register(typeof(IFormattingService), typeof(FormattingService), true);

            //ISerializerService serializerService = container.Resolve<ISerializerService>();
            //serializerService.AddKnownTypeForSerializationRange(new[]
            //{
            //    typeof(FormulaFormatter),
            //    typeof(UshortsFormatterBase), typeof(StringFormatter1251),
            //    typeof(BoolFormatter), typeof(DirectUshortFormatter), typeof(DefaultBitMaskFormatter),
            //    typeof(AsciiStringFormatter), typeof(DictionaryMatchingFormatter), typeof(DefaultTimeFormatter),
            //    typeof(UshortToIntegerFormatter)
            //});
            //serializerService.AddNamespaceAttribute("formulaFormatter", "FormulaFormatterNS");
            //serializerService.AddNamespaceAttribute("boolFormatter", "BoolFormatterNS");
            //serializerService.AddNamespaceAttribute("directUshortFormatter", "DirectUshortFormatterNS");
            //serializerService.AddNamespaceAttribute("asciiStringFormatter", "AsciiStringFormatterNS");
            //serializerService.AddNamespaceAttribute("dictionaryMatchingFormatter", "DictionaryMatchingFormatterNS");
            //serializerService.AddNamespaceAttribute("defaultTimeFormatter", "DefaultTimeFormatterNS");
            //serializerService.AddNamespaceAttribute("ushortToIntegerFormatter", "UshortToIntegerFormatterNS");
            //serializerService.AddNamespaceAttribute("defaultBitMaskFormatter", "DefaultBitMaskFormatterNS");
            //serializerService.AddNamespaceAttribute("stringFormatter1251", "StringFormatter1251NS");
        }
    }
}