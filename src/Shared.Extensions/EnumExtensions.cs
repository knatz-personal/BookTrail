using System.ComponentModel;
using System.Reflection;
using System.Text;

namespace Shared.Extensions
{
    public static class EnumExtensions
    {
        public static string GetDescription(this Enum value)
        {
            Type type = value.GetType();
            string name = Enum.GetName(type, value);
            if (name != null)
            {
                FieldInfo field = type.GetField(name);
                if (field != null)
                {
                    if (Attribute.GetCustomAttribute(field,
                            typeof(DescriptionAttribute)) is DescriptionAttribute attr)
                    {
                        return attr.Description;
                    }
                }
            }

            return null;
        }

        public static string GetDisplayName(this Enum value)
        {
            Type type = value.GetType();
            string name = Enum.GetName(type, value);
            if (name != null)
            {
                FieldInfo field = type.GetField(name);
                if (field != null)
                {
                    if (Attribute.GetCustomAttribute(field,
                            typeof(DisplayNameAttribute)) is DisplayNameAttribute attr)
                    {
                        return attr.DisplayName;
                    }
                }
            }

            return null;
        }

        public static TEnum? ToEnumValue<TEnum>(this int? nullableInt) where TEnum : struct
        {
            if (nullableInt.HasValue && Enum.IsDefined(typeof(TEnum), nullableInt))
            {
                return (TEnum)Enum.ToObject(typeof(TEnum), nullableInt);
            }

            return null;
        }

        /// <summary>
        ///     Converts an enum to a JSON array of objects containing id and text properties.
        /// </summary>
        /// <param name="enumValue">The enum value to convert.</param>
        /// <returns>A JSON string representation of the enum.</returns>
        /// <exception cref="ArgumentNullException">Thrown when enumValue is null.</exception>
        public static string ToJson(this Enum enumValue)
        {
            ArgumentNullException.ThrowIfNull(enumValue);

            StringBuilder sb = new();
            List<KeyValuePair<string, int>> results =
                Enum.GetValues(enumValue.GetType()).Cast<object>()
                    .ToDictionary(value => ((Enum)value).GetDescription(), value => (int)value).ToList();

            sb.Append('[');

            for (int i = 0; i < results.Count; i++)
            {
                KeyValuePair<string, int> item = results[i];
                sb.Append($"{{\"id\": {item.Value}, \"text\": \"{item.Key}\"}}");

                if (i < results.Count - 1)
                {
                    sb.Append(',');
                }
            }

            sb.Append(']');
            return sb.ToString();
        }
    }
}