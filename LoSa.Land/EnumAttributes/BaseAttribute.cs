using LoSa.Land.Service;
using System;
using System.Linq;

namespace LoSa.Land.EnumAttributes
{
    public class MessageConstructingRectangle : BaseAttribute
    {
        public MessageConstructingRectangle(string value)
            : base(value)
        {

        }
    }

    public interface IFormatSDR
    {
        object GetValue();
    }

    public class FormatSDR20 : BaseAttribute, IFormatSDR
    {
        public FormatSDR20(int value)
            : base(value)
        {

        }
    }

    public class FormatSDR33 : BaseAttribute, IFormatSDR
    {
        public FormatSDR33(int value)
            : base(value)
        {

        }
    }

    public static class EnumExtensionMethods
    {
        public static string GetMessage(this Enum enumItem)
        {
            return enumItem.GetAttributeValueEnum(typeof(MessageConstructingRectangle), string.Empty);
        }
    }

    public static class ClassExtensionMethods
    {
        public static int GeNumberCharactersSDR20(this Type typeItem)
        {
            return  typeItem.GetAttributeValueClass(typeof(FormatSDR20), 0);
        }

        public static int GeNumberCharactersSDR33(this Type typeItem)
        {
            return typeItem.GetAttributeValueClass(typeof(FormatSDR33), 0);
        }
    }

    public abstract class BaseAttribute : Attribute
    {
        private readonly object _value;
        public BaseAttribute(object value) { this._value = value; }

        public object GetValue() { return this._value; }
    }

    public static class EnumAttributesBaseLogic
    {
        public static VAL GetAttributeValueEnum<ENUM, VAL>(this ENUM enumItem, Type attributeType, VAL defaultValue)
        {
            var attribute = enumItem.GetType().GetField(enumItem.ToString())
                .GetCustomAttributes(attributeType, true)
                .Where(a => a is BaseAttribute)
                .Select(a => (BaseAttribute)a)
                .FirstOrDefault();

            return attribute == null ? defaultValue : (VAL)attribute.GetValue();
        }
    }

    public static class ClassAttributesBaseLogic
    {
        public static VAL GetAttributeValueClass<CLASS, VAL>(this CLASS classItem, Type attributeType, VAL defaultValue)
        {
            var attribute = classItem.GetType().GetField(classItem.ToString())
                .GetCustomAttributes(attributeType, true)
                .Where(a => a is BaseAttribute)
                .Select(a => (BaseAttribute)a)
                .FirstOrDefault();

            return attribute == null ? defaultValue : (VAL)attribute.GetValue();
        }
    }
}
