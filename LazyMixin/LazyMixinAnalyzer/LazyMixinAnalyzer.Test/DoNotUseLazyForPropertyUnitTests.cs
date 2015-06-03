using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestHelper;

namespace LazyMixinAnalyzer.Test
{
    [TestClass]
    public class DoNotUseLazyForPropertyUnitTest : CodeFixVerifier
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
        public void DoNotUseLazyForProperty()
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
        public LazyMixin<StringBuilder> X { get; }
    }
}";
            var expected = new DiagnosticResult
            {
                Id = DoNotUseLazyForPropertyAnalyzer.DiagnosticId,
                Message = string.Format(DoNotUseLazyForPropertyAnalyzer.MessageFormat, "X"),
                Severity = DiagnosticSeverity.Error,
                Locations =
                    new[]
                    {
                        new DiagnosticResultLocation("Test0.cs", 14, 9)
                    }
            };

            VerifyCSharpDiagnostic(test, expected);
        }

        protected override DiagnosticAnalyzer GetCSharpDiagnosticAnalyzer()
        {
            return new DoNotUseLazyForPropertyAnalyzer();
        }
    }
}