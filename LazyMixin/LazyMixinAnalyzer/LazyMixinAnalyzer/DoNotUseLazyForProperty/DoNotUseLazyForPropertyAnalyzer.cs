using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Diagnostics;

namespace LazyMixinAnalyzer
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class DoNotUseLazyForPropertyAnalyzer : DiagnosticAnalyzer
    {
        public const string DiagnosticId = "DoNotUseLazyForProperty";
        internal const string Title = "Do not use LazyMixin<T> for a property.";
        internal const string MessageFormat = "The type of the property '{0}' is LazyMixin<T>, which is not allowed.";
        internal const string Category = "Correction";

        internal static DiagnosticDescriptor Rule = new DiagnosticDescriptor(DiagnosticId, Title, MessageFormat, Category, DiagnosticSeverity.Error, isEnabledByDefault: true);

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Rule);

        public override void Initialize(AnalysisContext context)
        {
            context.RegisterSymbolAction(AnalyzePropertyDeclaration, SymbolKind.Property);
        }

        private void AnalyzePropertyDeclaration(SymbolAnalysisContext context)
        {
            var p = (IPropertySymbol)context.Symbol;
            var t = p.Type;

            if (t.IsTargetType())
            {
                var node = p.DeclaringSyntaxReferences.First().GetSyntax();
                var diagnostic = Diagnostic.Create(Rule, node.GetLocation(), p.Name);
                context.ReportDiagnostic(diagnostic);
            }
        }
    }
}
