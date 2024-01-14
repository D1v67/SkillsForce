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
        /// <summary>
        /// Maps data from a SqlDataReader to an object of a specified type.
        /// </summary>
        /// <typeparam name="T">Type of object to be created and mapped.</typeparam>
        /// <param name="reader">SqlDataReader containing the result set.</param>
        /// <returns>An instance of the specified type with properties mapped from the SqlDataReader.</returns>
        /// 
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


        /// <summary>
        /// Sets the value of a property on an object based on the provided property name and value.
        /// </summary>
        /// <param name="obj">Object whose property will be set.</param>
        /// <param name="propertyName">Name of the property to be set.</param>
        /// <param name="value">Value to be assigned to the property.</param>
        /// 
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
