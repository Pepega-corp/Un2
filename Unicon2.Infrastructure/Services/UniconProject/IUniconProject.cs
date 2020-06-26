using System;
using System.Collections.Generic;
using Unicon2.Infrastructure.Interfaces;
using Unicon2.Infrastructure.Interfaces.DataOperations;

namespace Unicon2.Infrastructure.Services.UniconProject
{
    /// <summary>
    /// Проект уникона
    /// </summary>
    public interface IUniconProject : IDisposable
    {
        /// <summary>
        /// Все подключаемые устройства
        /// </summary>
        List<IConnectable> ConnectableItems { get; set; }

        bool IsProjectSaved { get; }
        string Name { get; set; }
        string ProjectPath { get; set; }

        string LayoutString { get; set; }
    }
}