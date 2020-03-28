﻿using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Windows.Media;
using Unicon2.Fragments.Programming.Infrastructure.Model.Elements;

namespace Unicon2.Fragments.Programming.Model
{
    [DataContract(Namespace = "ConnectionNS")]
    public class Connection : IConnection
    {
        private readonly List<IConnector> _connectors;

        [DataMember] public int ConnectionNumber { get; set; }
        [DataMember] public PathGeometry Path { get; set; }
        public IConnector[] Connectors => this._connectors.ToArray();

        public Connection()
        {
            this._connectors = new List<IConnector>();
        }

        public void AddConnector(IConnector connector)
        {
            if (!this._connectors.Contains(connector))
            {
                this._connectors.Add(connector);
            }
        }

        public void RemoveConnector(IConnector connector)
        {
            if (this._connectors.Contains(connector))
            {
                this._connectors.Remove(connector);
            }
        }

        public void Clear()
        {
            this._connectors.Clear();
        }
    }
}
