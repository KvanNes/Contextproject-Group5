using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using Object = UnityEngine.Object;

namespace Utilities
{
    public class Utils
	{
        public static T GetDictionaryValue<TS, T>(Dictionary<TS, T> dictionary, TS key)
        {
            T result;
            try
            {
                dictionary.TryGetValue(key, out result);
            }
            catch (Exception)
            {
                return default(T);
            }
            return result;
        }

        public static void DestroyObject(Object obj)
        {
            Object.DestroyImmediate(obj);
        }

        public static void DestroyObjects(Object[] objs)
        {
            foreach (Object o in objs)
            {
                DestroyObject(o);
            }
        }

        public static string TimeToString(double time)
        {
            int minutes = (int)Math.Floor(time / 60);
            int seconds = (int)Math.Floor(time % 60);
            return minutes.ToString("D2") + ":" + seconds.ToString("D2");
        }
    }
}