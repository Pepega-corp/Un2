﻿using System;
using System.Windows;
using Unicon2.Fragments.Programming.Infrastructure.Model.Elements;

namespace Unicon2.Fragments.Programming.Infrastructure.ViewModels.Scheme.ElementViewModels
{
    public interface IConnectorViewModel
    {
        event Action<Point> ConnectorPositionChanged;

        IConnector Model { get; }
        ConnectorType ConnectorType { get; set; }
        ConnectorOrientation Orientation { get; }
        string Symbol {get;}
        Point ConnectorPosition { get; set; }
        ILogicElementViewModel ParentViewModel { get; }
        IConnectionViewModel Connection { get; set; }
        int ConnectionNumber { get; set; }
        bool Connected { get; }
        bool IsDragConnection { get; set; }
        void UpdateConnectorPosition(Point deltaPosition);
    }
}