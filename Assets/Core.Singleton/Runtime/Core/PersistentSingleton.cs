// Runtime/Core/PersistentSingleton.cs
using System;
using System.Runtime.CompilerServices;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace Core.Singleton
{
    /// <summary>
    /// Base class for all persistent singletons using pure C# implementation.
    /// </summary>
    public abstract class PersistentSingleton<T> : ISingleton where T : PersistentSingleton<T>, new()
    {
        private static readonly Lazy<T> _instance = new(() => new T());
        private static readonly AsyncReactiveProperty<bool> _readyState = new AsyncReactiveProperty<bool>(false);
        private static readonly CancellationTokenSource _destroyTokenSource = new CancellationTokenSource();

        /// <summary>
        /// Gets the singleton instance.
        /// </summary>
        public static T Instance => _instance.Value;

        public static UniTask<T> WaitForReadyAsync(CancellationToken cancellation = default)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Indicates whether the singleton is initialized.
        /// </summary>
        public bool IsInitialized { get; private set; }

        /// <summary>
        /// Indicates whether the singleton is ready.
        /// </summary>
        public bool IsReady => _readyState.Value;
        

        /// <summary>
        /// Wait for the singleton instance to be created.
        /// </summary>
        public static async UniTask<T> WaitForInstanceAsync(CancellationToken cancellation = default)
        {
            if (!ApplicationQuitHelper.IsQuitting)
                return Instance;

            throw new SingletonDestroyedException(typeof(T));
        }

        /// <summary>
        /// Initialize the singleton.
        /// </summary>
        public async UniTask InitializeAsync()
        {
            if (IsInitialized) return;

            try
            {
                await InitializeInternalAsync();
                IsInitialized = true;
                _readyState.Value = true;
             //   SingletonEvents.NotifyInitialized(GetType());
            }
            catch (Exception ex)
            {
                throw new SingletonInitializationTimeoutException(typeof(T), 5f);
            }
        }

        /// <summary>
        /// Override this method to implement custom initialization logic.
        /// </summary>
        protected virtual async UniTask OnInitializeAsync()
        {
            await UniTask.CompletedTask;
        }

        /// <summary>
        /// Internal initialization logic.
        /// </summary>
        private async UniTask InitializeInternalAsync()
        {
            await OnInitializeAsync();
        }

        /// <summary>
        /// Cleanup resources when the singleton is destroyed.
        /// </summary>
        public void Cleanup()
        {
            if (IsInitialized)
            {
                _destroyTokenSource.Cancel();
                IsInitialized = false;
                _readyState.Value = false;
             //   SingletonEvents.NotifyDestroyed(GetType());
            }
        }
    }
}
