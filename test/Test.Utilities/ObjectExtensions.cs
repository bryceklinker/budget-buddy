using System;
using System.Collections.Generic;
using System.Reflection;

namespace BudgetBuddy.Test.Utilities
{
    public static class ObjectExtensions
    {
        public static T GetAttribute<T>(this object obj) where T : Attribute
        {
            return obj.GetType().GetTypeInfo().GetCustomAttribute<T>();
        }

        public static T GetAttribute<T>(this object obj, string method) where T : Attribute
        {
            return obj.GetType().GetMethod(method).GetCustomAttribute<T>();
        }

        public static IEnumerable<T> GetAttributes<T>(this object obj, string method) where T : Attribute
        {
            return obj.GetType().GetMethod(method).GetCustomAttributes<T>();
        }
    }
}
