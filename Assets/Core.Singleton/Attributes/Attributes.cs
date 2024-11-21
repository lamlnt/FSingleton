using System;

namespace Core.Singleton
{
    /// <summary>
    /// Configuration for singleton behavior
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public sealed class PersistentSingletonConfig : Attribute
    {
        /// <summary>
        /// Whether to use thread-safe initialization
        /// </summary>
        public bool ThreadSafe { get; }

        /// <summary>
        /// When to initialize the singleton
        /// </summary>
        public InitializationTiming InitTiming { get; }

        /// <summary>
        /// Timeout for initialization in seconds
        /// </summary>
        public float InitTimeout { get; }

        /// <summary>
        /// Whether to automatically initialize on startup
        /// </summary>
        public bool AutoInitOnStartup { get; }

        public PersistentSingletonConfig(
            bool threadSafe = true,
            InitializationTiming timing = InitializationTiming.Lazy,
            float timeout = 5f,
            bool autoInitOnStartup = false)
        {
            ThreadSafe = threadSafe;
            InitTiming = timing;
            InitTimeout = timeout;
            AutoInitOnStartup = autoInitOnStartup;
        }
    }

    /// <summary>
    /// Marks a field to be included in serialized data
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public sealed class SerializeDataAttribute : Attribute
    {
        /// <summary>
        /// Category for grouping in editor
        /// </summary>
        public string Category { get; }

        /// <summary>
        /// Description for documentation
        /// </summary>
        public string Description { get; }

        /// <summary>
        /// Whether the field is required
        /// </summary>
        public bool Required { get; }

        public SerializeDataAttribute(
            string category = "Default",
            string description = "",
            bool required = false)
        {
            Category = category;
            Description = description;
            Required = required;
        }
    }

    /// <summary>
    /// Marks a field as required for validation
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public class RequiredAttribute : Attribute { }

    /// <summary>
    /// Marks a field as having a value range
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public class RangeAttribute : Attribute
    {
        public object Min { get; }
        public object Max { get; }

        public RangeAttribute(object min, object max)
        {
            Min = min;
            Max = max;
        }
    }

    /// <summary>
    /// Initialization timing options
    /// </summary>
    public enum InitializationTiming
    {
        /// <summary>
        /// Initialize when first accessed
        /// </summary>
        Lazy,

        /// <summary>
        /// Initialize immediately when created
        /// </summary>
        Immediate
    }
}
