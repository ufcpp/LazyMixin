using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestHelper;

namespace LazyMixinAnalyzer.Test
{
    [TestClass]
    public class DoNotUseLazyForPropertyUnitTest : ConventionCodeFixVerifier
    {
        [TestMethod]
        public void NoDiagnositcs() => VerifyCSharpByConvention();

        [TestMethod]
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