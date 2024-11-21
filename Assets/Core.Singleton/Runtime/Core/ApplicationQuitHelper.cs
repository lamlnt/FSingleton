// Runtime/Core/ApplicationQuitHelper.cs
using UnityEngine;

namespace Core.Singleton
{
    internal static class ApplicationQuitHelper
    {
        private static bool s_isQuitting;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void Initialize()
        {
            s_isQuitting = false;
            Application.quitting += OnApplicationQuit;
        }

        private static void OnApplicationQuit()
        {
            s_isQuitting = true;
        }

        public static bool IsQuitting => s_isQuitting;
    }
}
