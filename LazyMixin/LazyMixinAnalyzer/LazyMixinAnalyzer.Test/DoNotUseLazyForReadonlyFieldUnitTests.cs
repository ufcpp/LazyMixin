using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestHelper;

namespace LazyMixinAnalyzer.Test
{
    [TestClass]
    public class DoNotUseLazyForReadonlyFieldUnitTest : ContractCodeFixVerifier
    {
        [TestMethod]
        public void NoDiagnositcs() => VerifyDiagnostic();

        [TestMethod]
        public void DoNotUseLazyForReadonlyField() => VerifyDiagnostic(new DiagnosticResult
        {
            Id = DoNotUseLazyForReadonlyFieldAnalyzer.DiagnosticId,
            Message = string.Format(DoNotUseLazyForReadonlyFieldAnalyzer.MessageFormat, "_x"),
            Severity = DiagnosticSeverity.Error,
            Locations = new[]
            {
                new DiagnosticResultLocation("Test0.cs", 13, 43)
            }
        });

        protected override DiagnosticAnalyzer GetCSharpDiagnosticAnalyzer() => new DoNotUseLazyForReadonlyFieldAnalyzer();
    }
}