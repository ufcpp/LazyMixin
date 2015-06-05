using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestHelper;

namespace LazyMixinAnalyzer.Test
{
    [TestClass]
    public class AccessViaPropertyUnitTest : ContractCodeFixVerifier
    {
        [TestMethod]
        public void NoDiagnositcs() => VerifyDiagnostic();

        [TestMethod]
        public void AccessViaProperty() => VerifyCodeFix(new DiagnosticResult
        {
            Id = AccessViaPropertyAnalyzer.DiagnosticId,
            Message = string.Format(AccessViaPropertyAnalyzer.MessageFormat, "_x"),
            Severity = DiagnosticSeverity.Info,
            Locations = new[]
            {
                new DiagnosticResultLocation("Test0.cs", 13, 42)
            }
        });

        protected override CodeFixProvider GetCSharpCodeFixProvider() => new AccessViaPropertyCodeFixProvider();

        protected override DiagnosticAnalyzer GetCSharpDiagnosticAnalyzer() => new AccessViaPropertyAnalyzer();
    }
}