using System;
using System.Reflection;
using System.Windows;
using Unicon2.Infrastructure.Services;

namespace Unicon2.Services
{
    public class ApplicationXamlResourcesService : IXamlResourcesService
    {
        public void AddResourceAsGlobal(string filePath, Assembly sourceAssembly)
        {
            var uriPath = $"pack://application:,,,/{sourceAssembly.GetName().Name};component/{filePath}";

            var packUri = new Uri(uriPath, UriKind.Absolute);
            if (packUri == null) throw new ArgumentNullException("packUri");

            Application.Current.Resources.MergedDictionaries.Add(new ResourceDictionary
            {
                Source = packUri
            });
        }
    }
}
