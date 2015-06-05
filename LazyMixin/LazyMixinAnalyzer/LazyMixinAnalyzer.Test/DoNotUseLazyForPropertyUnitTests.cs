using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestHelper;

namespace LazyMixinAnalyzer.Test
{
    [TestClass]
    public class DoNotUseLazyForPropertyUnitTest : ContractCodeFixVerifier
    {
        [TestMethod]
        public void NoDiagnositcs() => VerifyDiagnostic();

        [TestMethod]
        public void DoNotUseLazyForProperty() => VerifyDiagnostic(new DiagnosticResult
        {
            Id = DoNotUseLazyForPropertyAnalyzer.DiagnosticId,
            Message = string.Format(DoNotUseLazyForPropertyAnalyzer.MessageFormat, "X"),
            Severity = DiagnosticSeverity.Error,
            Locations = new[]
            {
                new DiagnosticResultLocation("Test0.cs", 13, 9)
            }
        });

        protected override DiagnosticAnalyzer GetCSharpDiagnosticAnalyzer() => new DoNotUseLazyForPropertyAnalyzer();
    }
}