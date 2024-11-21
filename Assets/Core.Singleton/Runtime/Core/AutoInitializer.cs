// Runtime/Core/AutoInitializer.cs
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace Core.Singleton
{
    /// <summary>
    /// Automatically initializes singleton instances marked with PersistentSingletonConfig.
    /// </summary>
    public static class AutoInitializer
    {
        private static readonly Dictionary<Type, PersistentSingletonConfig> _singletonConfigs = new();
        private static readonly HashSet<Type> _initializedSingletons = new();

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void InitializeSingletons()
        {
            // Cache all singleton types with their configuration
            CacheSingletonConfigs();

            // Initialize all singletons
            foreach (var singletonType in _singletonConfigs.Keys)
            {
                var config = _singletonConfigs[singletonType];
                if (config.AutoInitOnStartup && !_initializedSingletons.Contains(singletonType))
                {
                    InitializeSingleton(singletonType);
                }
            }
        }

        private static void CacheSingletonConfigs()
        {
            var singletonTypes = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(a => a.GetTypes())
                .Where(t => t.IsClass && !t.IsAbstract && typeof(ISingleton).IsAssignableFrom(t));

            foreach (var type in singletonTypes)
            {
                var config = type.GetCustomAttribute<PersistentSingletonConfig>();
                if (config != null)
                {
                    _singletonConfigs[type] = config;
                }
            }
        }

        private static async void InitializeSingleton(Type singletonType)
        {
            try
            {
                // Get the singleton instance
                var instance = (ISingleton)singletonType.GetProperty("Instance")?.GetValue(null);
                if (instance == null) return;

                // Get the InitializeAsync method
                var method = singletonType.GetMethod("InitializeAsync", BindingFlags.Public | BindingFlags.Instance);
                if (method != null)
                {
                    await (UniTask)method.Invoke(instance, null);
                    _initializedSingletons.Add(singletonType); // Mark as initialized
                }
            }
            catch (Exception ex)
            {
                Debug.LogError($"Failed to initialize singleton {singletonType.Name}: {ex}");
            }
        }
    }
}
