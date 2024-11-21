// Runtime/Core/ISingleton.cs

using System.Threading;
using Cysharp.Threading.Tasks;

namespace Core.Singleton
{
    /// <summary>
    /// Base interface for all singletons
    /// </summary>
    public interface ISingleton
    {
        /// <summary>
        /// Whether the singleton instance exists
        /// </summary>
        bool IsInitialized { get; }

        /// <summary>
        /// Whether the singleton is fully initialized and ready
        /// </summary>
        bool IsReady { get; }

        /// <summary>
        /// Initialize the singleton
        /// </summary>
        UniTask InitializeAsync();
    }
    
}
