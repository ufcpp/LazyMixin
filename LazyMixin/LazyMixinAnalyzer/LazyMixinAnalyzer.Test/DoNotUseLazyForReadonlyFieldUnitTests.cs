using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using System.Collections.Generic;
using TestHelper;
using Xunit;

namespace LazyMixinAnalyzer.Test
{
    public class DoNotUseLazyForReadonlyFieldUnitTest : ConventionCodeFixVerifier
    {
        [Fact]
        public void NoDiagnositcs() => VerifyCSharpByConvention();

        [Fact]
        public void DoNotUseLazyForReadonlyField() => VerifyCSharpByConvention();

        protected override DiagnosticAnalyzer GetCSharpDiagnosticAnalyzer() => new DoNotUseLazyForReadonlyFieldAnalyzer();

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