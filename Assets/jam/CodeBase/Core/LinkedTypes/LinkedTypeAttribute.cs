using System;
using System.Reflection;

namespace jam.CodeBase.Core.LinkedTypes
{
    [AttributeUsage(AttributeTargets.Field)]
    public class LinkedTypeAttribute : Attribute
    {
        public Type Type { get; }

        public LinkedTypeAttribute(Type type)
        {
            Type = type;
        }
    }
    
    public static partial class EnumExtensions
    {
        public static Type GetLinkedType(this Enum enumValue)
        {
            var fieldInfo = enumValue.GetType().GetField(enumValue.ToString());
            var attribute = fieldInfo?.GetCustomAttribute<LinkedTypeAttribute>();
            return attribute?.Type;
        }
    }

}