using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using TestHelper;

namespace LazyMixinAnalyzer.Test
{
    [TestClass]
    public class AccessViaPropertyUnitTest : ConventionCodeFixVerifier
    {
        [TestMethod]
        public void NoDiagnositcs() => VerifyCSharpByConvention();

        [TestMethod]
        public void NoNamespace() => VerifyCSharpByConvention();

        [TestMethod]
        public void AccessViaProperty() => VerifyCSharpByConvention();

        [TestMethod]
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