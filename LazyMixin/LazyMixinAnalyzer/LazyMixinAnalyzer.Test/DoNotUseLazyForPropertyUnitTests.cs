using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using TestHelper;
using Xunit;

namespace LazyMixinAnalyzer.Test
{
    public class DoNotUseLazyForPropertyUnitTest : ConventionCodeFixVerifier
    {
        [Fact]
        public void NoDiagnositcs() => VerifyCSharpByConvention();

        [Fact]
        public void DoNotUseLazyForProperty() => VerifyCSharpByConvention();

        protected override DiagnosticAnalyzer GetCSharpDiagnosticAnalyzer() => new DoNotUseLazyForPropertyAnalyzer();

        protected override IEnumerable<MetadataReference> References
        {
            get
            {
                foreach (var x in base.References) yield return x;
                yield return MetadataReference.CreateFromFile(typeof(Laziness.LazyMixin<>).Assembly.Location);
            }
        }
    }
}