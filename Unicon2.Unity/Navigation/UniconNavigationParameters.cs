using Prism.Regions;
using System.Collections.Generic;
using System.Linq;

namespace Unicon2.Unity.Navigation
{
    public class UniconNavigationParameters : List<UniconNavigationParameter>
    {
        public static UniconNavigationParameters FromNavigationParameters(NavigationParameters navigationParameters)
        {
            UniconNavigationParameters uniconNavigationParameters = new UniconNavigationParameters();
            foreach (KeyValuePair<string, object> keyValuePair in navigationParameters)
            {
                uniconNavigationParameters.Add(new UniconNavigationParameter(keyValuePair.Key, keyValuePair.Value));
            }

            return uniconNavigationParameters;
        }

        public NavigationParameters ToNavigationParameters()
        {
            NavigationParameters navigationParametersToRegion = new NavigationParameters();

            this.ForEach(parameter =>
            {
                navigationParametersToRegion.Add(parameter.ParameterName, parameter.Parameter);
            });
            return navigationParametersToRegion;
        }


        public T GetParameterByName<T>(string key)
        {
            UniconNavigationParameter parameterResult =
                this.FirstOrDefault((parameter => parameter.ParameterName == key));
            if (parameterResult != null)
            {
                return (T)parameterResult.Parameter;
            }

            return default(T);
        }

        public UniconNavigationParameters AddParameterByName(string key, object parameter)
        {
            if (this.Any((navigationParameter => navigationParameter.ParameterName == key))) return this;
            this.Add(new UniconNavigationParameter(key, parameter));
            return this;
        }
    }

    public class UniconNavigationParameter
    {
        public UniconNavigationParameter(string parameterName, object parameter)
        {
            this.ParameterName = parameterName;
            this.Parameter = parameter;
        }

        public string ParameterName { get; }
        public object Parameter { get; }
    }
}
