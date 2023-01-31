using System;

namespace Xyz.Vasd.Fake.Data.Database
{
    internal static class TypeTools
    {
        public static object CreateGeneric(Type baseType, Type valueType, params object[] args)
        {
            Type type = baseType.MakeGenericType(valueType);
            return Activator.CreateInstance(type, args);
        }
    }
}
