using System.ComponentModel;

namespace Unicon2.Fragments.Programming.Infrastructure
{
    public enum ConnectorOrientation
    {
        NONE,
        LEFT,
        RIGHT
    }

    public enum ConnectorType
    {
        [Description("Прямой")]
        DIRECT,
        [Description("Инверсный")]
        INVERS
    }
}