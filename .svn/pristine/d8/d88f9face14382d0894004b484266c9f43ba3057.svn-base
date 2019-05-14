using System;
using System.Collections.Generic;
using Prism.Mvvm;
using Prism.Unity;
using Unicon2.Unity.Interfaces;
using Unity;

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
            this._container = container;
        }

        public T Resolve<T>()
        {
            return this._container.Resolve<T>();
        }

        public object Resolve(Type t)
        {
            return this._container.Resolve(t);
        }

        public T Resolve<T>(string key)
        {
            return this._container.Resolve<T>(key);
        }

        public object Resolve(Type t, string key)
        {
            return this._container.Resolve(t, key);
        }

        public IEnumerable<T> ResolveAll<T>()
        {
            return this._container.ResolveAll<T>();
        }

        public void Register<T>(bool isSingleton = false)
        {
            if (isSingleton)
            {
                this._container.RegisterSingleton<T>();
            }
            else
            {
                this._container.RegisterType<T>();
            }
        }

        public void Register(Type t, bool isSingleton = false)
        {
            if (isSingleton)
            {
                this._container.RegisterSingleton(t);
            }
            else
            {
                this._container.RegisterType(t);
            }
        }

        public void Register<TFrom, TTo>(bool isSingleton = false) where TTo : TFrom
        {
            if (isSingleton)
            {
                this._container.RegisterSingleton<TFrom, TTo>();
            }
            else
            {
                this._container.RegisterType<TFrom, TTo>();
            }
        }

        public void Register(Type from, Type to, bool isSingleton = false)
        {
            if (isSingleton)
            {
                this._container.RegisterSingleton(from, to);
            }
            else
            {
                this._container.RegisterType(from, to);
            }
        }

        public void Register<TFrom, TTo>(string key, bool isSingleton = false) where TTo : TFrom
        {
            if (isSingleton)
            {
                this._container.RegisterSingleton<TFrom, TTo>(key);
            }
            else
            {
                this._container.RegisterType<TFrom, TTo>(key);
            }
        }

        public void Register(Type from, Type to, string key, bool isSingleton = false)
        {
            if (isSingleton)
            {
                this._container.RegisterSingleton(from, to, key);
            }
            else
            {
                this._container.RegisterType(from, to, key);
            }
        }

        public void RegisterInstance<T>(T instance)
        {
            this._container.RegisterInstance(instance);
        }

        public void RegisterViewModel<TView, TViewModel>()
        {
            ViewModelLocationProvider.Register<TView, TViewModel>();
        }

        public void RegisterForNavigation<TView>(string key)
        {
            this._container.RegisterTypeForNavigation<TView>(key);
        }
    }
}
