using System;
using System.Collections.Generic;

namespace Unicon2.Infrastructure.Services
{
    public interface ISerializerService
    {
        void AddKnownTypeForSerializationRange(IEnumerable<Type> types);
        void AddKnownTypeForSerialization(Type type);
        void AddNamespaceAttribute(string attributeName, string namespaceString);
        List<Type> GetTypesForSerialiation();

        Dictionary<string, string> GetNamespacesAttributes();
    }
}