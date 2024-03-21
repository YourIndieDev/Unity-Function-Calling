using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace AI
{
    public static class ContextManager
    {
        private static Dictionary<string, ContextBase> context = new Dictionary<string, ContextBase>();
        private static Dictionary<string, Dictionary<string, ContextBase>> contextsOverTime = new Dictionary<string, Dictionary<string, ContextBase>>();

        public static event Action<string> OnSerializedConext;

        public static string SerializeContextJson()//string key, T value)
        {
            var json = JsonConvert.SerializeObject(context);

            OnSerializedConext?.Invoke(json);

            return json;
        }

        public static string SerializeContextOverTimeJson()//string key, T value)
        {
            return JsonConvert.SerializeObject(contextsOverTime);
        }

        public static void ClearContextJson()
        {
            context.Clear();

            OnSerializedConext?.Invoke(null);
        }

        public static void RemoveContextJson(string key)
        {
            if (!context.ContainsKey(key)) { return; }

            context.Remove(key);
        }

        // The key could be the time it was added
        public static void AddContextsOverTime(string key)  //, Dictionary<string, ContextBase> newContext)
        {
            // Replaces the value if the key already exists, otherwise create it
            if (contextsOverTime.ContainsKey(key)) contextsOverTime[key] = context; 
            else contextsOverTime.Add(key, context);
        }

        public static void ClearContextsOverTime()
        {
            contextsOverTime.Clear();
        }

        public static void RemoveContextsOverTime(string key)
        {
            if (!contextsOverTime.ContainsKey(key)) return;

            contextsOverTime[key].Clear();
        }

        public static void CreateOrUpdateContext<T>(string key, T value)
        {
            if (context.TryGetValue(key, out var existingContext) && existingContext is Context<T> thisContext)
            {
                thisContext.SetValue(value);
            }
            else
            {
                var newContext = new Context<T>(key, value);
                //newContext.OnChanged += (k, v) => SerializeContextJson(k, v);
                context[key] = newContext;
            }
        }
    }
}
