using System.ComponentModel;

namespace CoinDropGamble.Helpers
{
public static class EnumHelper
    {
        public static string GetDescription(Enum value)
        {
            // Get the field information for the enum value
            var field = value.GetType().GetField(value.ToString());

            // Get the description attribute applied to the enum field
            var attribute = (DescriptionAttribute)Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute));

            // If a description is found, return it, otherwise return the enum value as a string
            return attribute != null ? attribute.Description : value.ToString();
        }
    }
}