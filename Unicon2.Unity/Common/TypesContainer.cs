using System;
using System.Collections.Generic;
using Prism.Mvvm;
using Unicon2.Unity.Interfaces;
using Unity;
using Unity.Resolution;

namespace Unicon2.Unity.Common
{
    /// <summary>
    /// Custom container for the bridge between IUnityContainer and other application modules
    /// </summary>
    public class TypesContainer : ITypesContainer
    {
        private IUnityContainer _container;

        public TypesContainer(IUnityContainer container)
        {
            _container = container;
        }

        public T Resolve<T>()
        {
            return _container.Resolve<T>();
        }

        public T Resolve<T>(params ResolverParameter[] parameters)
        {
            var resolverOverrides = new ResolverOverride[parameters.Length];
            for (var i = 0; i < parameters.Length; i++)
            {
                resolverOverrides[i] = new ParameterOverride(parameters[i].ParameterName, parameters[i].ParameterValue);
            }
            return _container.Resolve<T>(resolverOverrides);
        }

        public object Resolve(Type t)
        {
            return _container.Resolve(t);
        }

        public T Resolve<T>(string key)
        {
            return _container.Resolve<T>(key);
        }

        public T Resolve<T>(string key, params ResolverParameter[] parameters)
        {
            var resolverOverrides = new ResolverOverride[parameters.Length];
            for (var i = 0; i < parameters.Length; i++)
            {
                resolverOverrides[i] = new ParameterOverride(parameters[i].ParameterName, parameters[i].ParameterValue);
            }
            return _container.Resolve<T>(key, resolverOverrides);
        }
        
        public object Resolve(Type t, string key)
        {
            return _container.Resolve(t, key);
        }

        public IEnumerable<T> ResolveAll<T>()
        {
            return _container.ResolveAll<T>();
        }

        public void Register<T>(bool isSingleton = false)
        {
            if (isSingleton)
            {
                _container.RegisterSingleton<T>();
            }
            else
            {
                _container.RegisterType<T>();
            }
        }

        public void Register(Type t, bool isSingleton = false)
        {
            if (isSingleton)
            {
                _container.RegisterSingleton(t);
            }
            else
            {
                _container.RegisterType(t);
            }
        }

        public void Register<TFrom, TTo>(bool isSingleton = false) where TTo : TFrom
        {
            if (isSingleton)
            {
                _container.RegisterSingleton<TFrom, TTo>();
            }
            else
            {
                _container.RegisterType<TFrom, TTo>();
            }
        }

        public void Register(Type from, Type to, bool isSingleton = false)
        {
            if (isSingleton)
            {
                _container.RegisterSingleton(from, to);
            }
            else
            {
                _container.RegisterType(from, to);
            }
        }

        public void Register<TFrom, TTo>(string key, bool isSingleton = false) where TTo : TFrom
        {
            if (isSingleton)
            {
                _container.RegisterSingleton<TFrom, TTo>(key);
            }
            else
            {
                _container.RegisterType<TFrom, TTo>(key);
            }
        }

        public void Register(Type from, Type to, string key, bool isSingleton = false)
        {
            if (isSingleton)
            {
                _container.RegisterSingleton(from, to, key);
            }
            else
            {
                _container.RegisterType(from, to, key);
            }
        }

        public void RegisterInstance<T>(T instance)
        {
            _container.RegisterInstance(instance);
        }

        public void RegisterViewModel<TView, TViewModel>()
        {
            ViewModelLocationProvider.Register<TView, TViewModel>();
        }

    }
}
