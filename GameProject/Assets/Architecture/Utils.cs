using UnityEngine;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;

public class Utils {
    
    public static Texture2D LoadTexture(String path) {
        return (Texture2D) Resources.Load<Texture2D>(path);
    }

    public static Vector2 RotatedTranslate(Vector2 start, Vector2 delta, float angle) {
        return Rotate(start + delta, start, angle);
    }

    public static Vector2 Rotate(Vector2 point, Vector2 midpoint, float angle) {
        return new Vector2(
            point.x * Mathf.Cos(angle) - point.y * Mathf.Sin(angle),
            point.y * Mathf.Cos(angle) + point.x * Mathf.Sin(angle)
        );
    }

    public static Texture2D LoadTexture(String path) {
        return (Texture2D) Resources.Load<Texture2D>(path);
    }

    public static T getDictionaryValue<S, T>(Dictionary<S, T> dictionary, S key) {
        T result;
        try {
            dictionary.TryGetValue(key, out result);
        } catch(Exception) {
            return default(T);
        }
        return result;
    }
    
    // Copies (clones) a Quaternion.
    public static Quaternion copy(Quaternion v) {
        return new Quaternion(v.x, v.y, v.z, v.w);
    }
    
    // Copies (clones) a Vector3.
    public static Vector3 copy(Vector3 v) {
        return new Vector3(v.x, v.y, v.z);
    }
    
    public static float forceInInterval(float x, float min, float max) {
        return Mathf.Min(Mathf.Max(min, x), max);
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
