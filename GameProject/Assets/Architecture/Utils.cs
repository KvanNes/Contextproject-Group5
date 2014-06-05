using UnityEngine;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;

public class Utils {

    public static T getDictionaryValue<S, T>(Dictionary<S, T> dictionary, S key) {
        T result;
        try {
            dictionary.TryGetValue(key, out result);
        } catch(Exception) {
            return default(T);
        }
        return result;
    }
    
    // Gebaseerd op: http://stackoverflow.com/a/80467

    /// <summary>
    /// Returns all types in the current AppDomain implementing the interface or inheriting the type. 
    /// </summary>
    public static IEnumerable<Type> TypesImplementingInterface(Type desiredType) {
        return AppDomain
            .CurrentDomain
            .GetAssemblies()
            .SelectMany(assembly => assembly.GetTypes())
            .Where(type => desiredType.IsAssignableFrom(type) && desiredType != type);
    }
}
