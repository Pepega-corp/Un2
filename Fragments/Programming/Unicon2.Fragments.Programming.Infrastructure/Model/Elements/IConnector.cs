﻿namespace Unicon2.Fragments.Programming.Infrastructure.Model.Elements
{
    public interface IConnector
    {
        ConnectorOrientation Orientation { get; }
        ConnectorType Type { get; set; }
        int ConnectionNumber { get; set; }
    }
}