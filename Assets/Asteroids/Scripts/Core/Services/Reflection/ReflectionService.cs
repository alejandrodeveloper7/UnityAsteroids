using System;
using System.Reflection;
using UnityEngine;

namespace Asteroids.Core.Services
{
   public static class ReflectionService 
   {
        #region Functionality

        public static object GetFieldValue(object instance, string fieldName)
        {
            Type type = instance.GetType();
            FieldInfo field = type.GetField(fieldName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

            if (field == null)
            {
                Debug.LogWarning($"{typeof(ReflectionService).Name} field {fieldName} not found.");
                return null;
            }

            return field.GetValue(instance);
        }
      
        public static object GetPropertyValue(object instance, string propertyName)
        {
            Type type = instance.GetType();
            PropertyInfo property = type.GetProperty(propertyName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

            if (property == null)
            {
                Debug.LogWarning($"{typeof(ReflectionService).Name} property {propertyName} not found.");
                return null;
            }

            return property.GetValue(instance);
        }

        #endregion
    }
}