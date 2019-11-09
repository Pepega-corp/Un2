﻿using System.Collections.ObjectModel;
using System.Windows;
using Unicon2.Fragments.Programming.Infrastructure.Model.Elements;

namespace Unicon2.Fragments.Programming.Infrastructure.ViewModels.Scheme.ElementViewModels
{
    public interface IConnectorViewModel
    {
        string Symbol {get;}

        Point ConnectorPoint { get; set; }
        ILogicElementViewModel ParentViewModel { get; }
        ObservableCollection<IConnectionViewModel> Connections { get; }
        IConnector Connector { get; }
        ConnectorType ConnectorType { get; set; }
        ConnectorOrientation Orientation { get; }
        int ConnectionNumber { get; set; }
        bool Connected { get; set; }
    }
}