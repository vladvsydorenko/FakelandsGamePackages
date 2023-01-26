using System;

namespace Xyz.Vasd.Fake
{
    public static class TypeTools
    {
        public static object CreateGeneric(Type baseType, Type valueType, params object[] args)
        {
            Type type = baseType.MakeGenericType(valueType);
            return Activator.CreateInstance(type, args);
        }
    }
}
