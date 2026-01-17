using System;

namespace ProjectX.CodeBase.Utils
{
    public static class EnumTools
    {
        public static TEnumElement[] GetElements<TEnumElement>() where TEnumElement : Enum
        {
            return (TEnumElement[])Enum.GetValues(typeof(TEnumElement));
        }
    }
}