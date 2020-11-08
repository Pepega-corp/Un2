using Unicon2.Formatting.Editor.ViewModels;

namespace Unicon2.Formatting.Editor.Visitors
{
    public interface IFormatterViewModelVisitor<T>
    {
        T VisitBoolFormatter(BoolFormatterViewModel formatter);
        T VisitAsciiStringFormatter(AsciiStringFormatterViewModel formatter);
        T VisitTimeFormatter(DefaultTimeFormatterViewModel formatter);
        T VisitDirectUshortFormatter(DirectFormatterViewModel formatter);
        T VisitFormulaFormatter(FormulaFormatterViewModel formatter);
        T VisitString1251Formatter(StringFormatter1251ViewModel formatter);
        T VisitUshortToIntegerFormatter(UshortToIntegerFormatterViewModel formatter);
        T VisitDictionaryMatchFormatter(DictionaryMatchingFormatterViewModel formatter);
        T VisitBitMaskFormatter(DefaultBitMaskFormatterViewModel formatter);
    }
}