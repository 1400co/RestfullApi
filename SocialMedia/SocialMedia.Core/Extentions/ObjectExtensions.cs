using System.Collections.Generic;
using System.Reflection;

public static class ObjectExtensions
{
    public static void CopyPropertiesTo<T>(this T source, T destination, List<string> excludedProperties = null)
    {
        var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

        foreach (var property in properties)
        {
            if (excludedProperties != null && excludedProperties.Contains(property.Name))
            {
                continue;
            }

            if (property.CanRead && property.CanWrite)
            {
                var value = property.GetValue(source);
                property.SetValue(destination, value);
            }
        }
    }
}