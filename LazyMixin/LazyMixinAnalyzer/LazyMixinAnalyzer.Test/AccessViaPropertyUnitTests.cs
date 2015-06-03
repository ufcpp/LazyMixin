using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestHelper;

namespace LazyMixinAnalyzer.Test
{
    [TestClass]
    public class AccessViaPropertyUnitTest : CodeFixVerifier
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
        public void AccessViaProperty()
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
        private LazyMixin<StringBuilder> _x;
    }
}";
            var expected = new DiagnosticResult
            {
                Id = AccessViaPropertyAnalyzer.DiagnosticId,
                Message = string.Format(AccessViaPropertyAnalyzer.MessageFormat, "_x"),
                Severity = DiagnosticSeverity.Info,
                Locations =
                    new[]
                    {
                        new DiagnosticResultLocation("Test0.cs", 14, 42)
                    }
            };

            VerifyCSharpDiagnostic(test, expected);

            var fixtest = @"
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
            VerifyCSharpFix(test, fixtest);
        }

        protected override CodeFixProvider GetCSharpCodeFixProvider()
        {
            return new AccessViaPropertyCodeFixProvider();
        }

        protected override DiagnosticAnalyzer GetCSharpDiagnosticAnalyzer()
        {
            return new AccessViaPropertyAnalyzer();
        }
    }
}