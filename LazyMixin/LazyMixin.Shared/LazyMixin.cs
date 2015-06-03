using System;

namespace Laziness
{
    /// <summary>
    /// Simple implementation for lazy initialization without thread-safety.
    /// </summary>
    /// <remarks>
    /// This struct is simple and efficient in memory usage, but has many restriction because of the C# struct charactaristics.
    /// - Do not use for a property. It makes a copied value.
    /// - Do not use for a readonly field. It also makes a copied value.
    ///
    /// It is thus highly recommended to use this struct with the LazyMixinAnalyzer.
    /// </remarks>
    /// <example><![CDATA[
    /// // Do not
    /// public Lazy<T> X { get; }
    /// 
    /// // Do not
    /// public Lazy<T> X => _x.Value;
    /// private readonly Lazy<T> _x;
    /// 
    /// // Recommended
    /// public Lazy<T> X => _x.Value;
    /// private Lazy<T> _x;
    /// ]]></example>
    /// <typeparam name="T"></typeparam>
    public struct LazyMixin<T> : IDisposable
        where T : class, new()
    {
        private T _value;

        /// <summary>
        /// Gets the lazily initialized value.
        /// </summary>
        public T Value => _value ?? (_value = new T());

        /// <summary>
        /// Invokes value.Dispose if _value != null, then resets value to null.
        /// </summary>
        /// <remarks>
        /// This struct can be resused. If you use the Value after invoking the Dispose, the Value is re-initialized.
        /// </remarks>
        public void Dispose()
        {
            if (_value != null)
            {
                (_value as IDisposable)?.Dispose();
                _value = null;
            }
        }

        internal bool IsInitialized => _value != null;
    }
}
