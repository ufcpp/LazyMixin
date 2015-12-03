using Microsoft.CodeAnalysis;

namespace LazyMixinAnalyzer
{
    static class TypeExtensions
    {
        /// <summary>
        /// Check <paramref name="t"/> is a target type of this code analyzer/code generator, the Laziness.LazyMixin{T}.
        /// This uses only its name, loosely typed.
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public static bool IsTargetType(this ITypeSymbol t) => t.ContainingNamespace?.Name == "Laziness" && t.MetadataName == "LazyMixin`1";
    }
}
