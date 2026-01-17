using System;

namespace ProjectX.CodeBase.Utils
{
    public class LinkedTypeAttribute : Attribute
    {
        public readonly Type Type;

        public LinkedTypeAttribute(Type type)
        {
            Type = type;
        }
    }
}