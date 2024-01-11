using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Common.SkillsForce.Helpers
{
    public static class DataReaderMapper
    {
        public static T MapToObject<T>(SqlDataReader reader) where T : new()
        {
            T obj = new T();

            for (int i = 0; i < reader.FieldCount; i++)
            {
                string propertyName = reader.GetName(i);
                object value = reader[i];

                SetProperty(obj, propertyName, value);
            }

            return obj;
        }

        private static void SetProperty(object obj, string propertyName, object value)
        {
            var property = obj.GetType().GetProperty(propertyName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

            if (property != null && value != DBNull.Value)
            {
                Type targetType = property.PropertyType;

                if (targetType.IsGenericType && targetType.GetGenericTypeDefinition() == typeof(Nullable<>))
                {
                    targetType = Nullable.GetUnderlyingType(targetType);
                }

                object convertedValue = Convert.ChangeType(value, targetType);
                property.SetValue(obj, convertedValue, null);
            }
        }
    }
}
