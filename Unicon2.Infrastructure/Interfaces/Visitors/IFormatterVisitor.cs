namespace Unicon2.Infrastructure.Interfaces.Visitors
{
    public interface IFormatterVisitor<T>
    {
        T VisitBoolFormatter(IUshortsFormatter formatter);
        T VisitAsciiStringFormatter(IUshortsFormatter formatter);
        T VisitTimeFormatter(IUshortsFormatter formatter);
        T VisitDirectUshortFormatter(IUshortsFormatter formatter);
        T VisitFormulaFormatter(IUshortsFormatter formatter);
        T VisitString1251Formatter(IUshortsFormatter formatter);
        T VisitUshortToIntegerFormatter(IUshortsFormatter formatter);
        T VisitDictionaryMatchFormatter(IUshortsFormatter formatter);
        T VisitBitMaskFormatter(IUshortsFormatter formatter);
        T VisitMatrixFormatter(IUshortsFormatter formatter);

    }
}