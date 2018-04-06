using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.Diagnostics;
using System.Collections.Generic;
using TestHelper;
using Xunit;

namespace LazyMixinAnalyzer.Test
{
    public class AccessViaPropertyUnitTest : ConventionCodeFixVerifier
    {
        [Fact]
        public void NoDiagnositcs() => VerifyCSharpByConvention();

        [Fact]
        public void NoNamespace() => VerifyCSharpByConvention();

        [Fact]
        public void AccessViaProperty() => VerifyCSharpByConvention();

        [Fact]
        public void AccessViaPropertyWithQualifiedName() => VerifyCSharpByConvention();

        protected override CodeFixProvider GetCSharpCodeFixProvider() => new AccessViaPropertyCodeFixProvider();

        protected override DiagnosticAnalyzer GetCSharpDiagnosticAnalyzer() => new AccessViaPropertyAnalyzer();

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