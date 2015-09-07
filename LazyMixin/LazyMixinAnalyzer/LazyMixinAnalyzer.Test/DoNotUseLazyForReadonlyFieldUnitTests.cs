using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using TestHelper;

namespace LazyMixinAnalyzer.Test
{
    [TestClass]
    public class DoNotUseLazyForReadonlyFieldUnitTest : ConventionCodeFixVerifier
    {
        [TestMethod]
        public void NoDiagnositcs() => VerifyCSharpByConvention();

        [TestMethod]
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