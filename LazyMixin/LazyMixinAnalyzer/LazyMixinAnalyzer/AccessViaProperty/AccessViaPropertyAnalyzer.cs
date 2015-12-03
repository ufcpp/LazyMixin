using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Diagnostics;

namespace LazyMixinAnalyzer
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class AccessViaPropertyAnalyzer : DiagnosticAnalyzer
    {
        public const string DiagnosticId = "AccessViaProperty";
        internal const string Title = "It is recommended to access a field of LazyMixin<T> via a property.";
        internal const string MessageFormat = "The field '{0}' is LazyMixin<T>, which Value can be encapsulated with a property.";
        internal const string Category = "Correction";

        internal static DiagnosticDescriptor Rule = new DiagnosticDescriptor(DiagnosticId, Title, MessageFormat, Category, DiagnosticSeverity.Info, isEnabledByDefault: true);

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Rule);

        public override void Initialize(AnalysisContext context)
        {
            context.RegisterSymbolAction(AnalyzeFieldDeclaration, SymbolKind.Field);
        }

        private void AnalyzeFieldDeclaration(SymbolAnalysisContext context)
        {
            var f = (IFieldSymbol)context.Symbol;

            var containing = f.ContainingType;

            foreach (var p in containing.GetMembers().OfType<IPropertySymbol>())
            {
                if (p.Name.ToLower() == f.Name.TrimStart('_').ToLower())
                    return;
            }

            var t = f.Type;

            if (t.IsTargetType())
            {
                var node = f.DeclaringSyntaxReferences.First().GetSyntax();
                var diagnostic = Diagnostic.Create(Rule, node.GetLocation(), f.Name);
                context.ReportDiagnostic(diagnostic);
            }
        }
    }
}
