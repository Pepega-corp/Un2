using System;
using System.Collections.Generic;
using Unicon2.Unity.Common;

namespace Unicon2.Unity.Interfaces
{
    public interface ITypesContainer
    {
        T Resolve<T>();
        T Resolve<T>(params ResolverParameter[] parameters);
        object Resolve(Type t);
        T Resolve<T>(string key);
        T Resolve<T>(string key, params ResolverParameter[] parameters);
        object Resolve(Type t, string key);
        IEnumerable<T> ResolveAll<T>();

        void Register<T>(bool isSingleton = false);
        void Register(Type t, bool isSingleton = false);

        void Register<TFrom, TTo>(bool isSingleton = false) where TTo : TFrom;
        void Register(Type from, Type to, bool isSingleton = false);

        void Register<TFrom, TTo>(string key, bool isSingleton = false) where TTo : TFrom;
        void Register(Type from, Type to, string key, bool isSingleton = false);

        void RegisterInstance<T>(T instance);

        void RegisterViewModel<TView, TViewModel>();
    }
}