using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestHelper;

namespace LazyMixinAnalyzer.Test
{
    [TestClass]
    public class DoNotUseLazyForReadonlyFieldUnitTest : CodeFixVerifier
    {
        [TestMethod]
        public void NoDiagnositcs()
        {
            var test = @"
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using Laziness;

namespace ConsoleApplication1
{
    class TypeName
    {
        public StringBuilder X => _x.Value;
        private LazyMixin<StringBuilder> _x;
    }
}";

            VerifyCSharpDiagnostic(test);
        }

        [TestMethod]
        public void DoNotUseLazyForReadonlyField()
        {
            var test = @"
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using Laziness;

namespace ConsoleApplication1
{
    class TypeName
    {
        readonly LazyMixin<StringBuilder> _x;
    }
}";
            var expected = new DiagnosticResult
            {
                Id = DoNotUseLazyForReadonlyFieldAnalyzer.DiagnosticId,
                Message = string.Format(DoNotUseLazyForReadonlyFieldAnalyzer.MessageFormat, "_x"),
                Severity = DiagnosticSeverity.Error,
                Locations =
                    new[]
                    {
                        new DiagnosticResultLocation("Test0.cs", 14, 43)
                    }
            };

            VerifyCSharpDiagnostic(test, expected);
        }

        protected override DiagnosticAnalyzer GetCSharpDiagnosticAnalyzer()
        {
            return new DoNotUseLazyForReadonlyFieldAnalyzer();
        }
    }
}