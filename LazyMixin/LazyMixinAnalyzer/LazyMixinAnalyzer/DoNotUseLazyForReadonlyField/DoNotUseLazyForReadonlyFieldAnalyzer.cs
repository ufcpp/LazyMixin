using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Diagnostics;

namespace LazyMixinAnalyzer
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class DoNotUseLazyForReadonlyFieldAnalyzer : DiagnosticAnalyzer
    {
        public const string DiagnosticId = "DoNotUseLazyForReadonlyField";
        internal const string Title = "Do not use LazyMixin<T> for a read-only field.";
        internal const string MessageFormat = "The type of the readonly field '{0}' is LazyMixin<T>, which is not allowed.";
        internal const string Category = "Correction";

        internal static DiagnosticDescriptor Rule = new DiagnosticDescriptor(DiagnosticId, Title, MessageFormat, Category, DiagnosticSeverity.Error, isEnabledByDefault: true);

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get { return ImmutableArray.Create(Rule); } }

        public override void Initialize(AnalysisContext context)
        {
            context.RegisterSymbolAction(AnalyzeFiedlDeclaration, SymbolKind.Field);
        }

        private void AnalyzeFiedlDeclaration(SymbolAnalysisContext context)
        {
            var f = (IFieldSymbol)context.Symbol;

            if (!f.IsReadOnly)
                return;

            var t = f.Type;

            if (t.ContainingNamespace.Name == "Laziness" && t.MetadataName == "LazyMixin`1")
            {
                var node = f.DeclaringSyntaxReferences.First().GetSyntax();
                var diagnostic = Diagnostic.Create(Rule, node.GetLocation(), f.Name);
                context.ReportDiagnostic(diagnostic);
            }
        }
    }
}
