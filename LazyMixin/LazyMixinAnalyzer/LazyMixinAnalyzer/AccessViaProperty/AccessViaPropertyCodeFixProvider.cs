using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Composition;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Rename;
using Microsoft.CodeAnalysis.Text;
using Microsoft.CodeAnalysis.Formatting;

namespace LazyMixinAnalyzer
{
    [ExportCodeFixProvider("LazyMixinAnalyzerCodeFixProvider", LanguageNames.CSharp), Shared]
    public class AccessViaPropertyCodeFixProvider : CodeFixProvider
    {
        public sealed override ImmutableArray<string> FixableDiagnosticIds => ImmutableArray.Create(AccessViaPropertyAnalyzer.DiagnosticId);

        public sealed override FixAllProvider GetFixAllProvider() => WellKnownFixAllProviders.BatchFixer;

        public override async Task RegisterCodeFixesAsync(CodeFixContext context)
        {
            var root = await context.Document.GetSyntaxRootAsync(context.CancellationToken).ConfigureAwait(false);

            // TODO: Replace the following code with your own analysis, generating a CodeAction for each fix to suggest
            var diagnostic = context.Diagnostics.First();
            var diagnosticSpan = diagnostic.Location.SourceSpan;

            // Find the type declaration identified by the diagnostic.
            var declaration = root.FindToken(diagnosticSpan.Start).Parent.AncestorsAndSelf().OfType<FieldDeclarationSyntax>().First();

            // Register a code action that will invoke the fix.
            context.RegisterCodeFix(
                CodeAction.Create("Encapsulate", c => Encapsulate(context.Document, declaration, c)),
                diagnostic);
        }

        private static readonly SyntaxToken PublicToken = SyntaxFactory.Token(SyntaxKind.PublicKeyword);
        private static readonly SyntaxToken ArrowToken = SyntaxFactory.Token(SyntaxKind.EqualsGreaterThanToken);
        private static readonly SyntaxToken SemicolonToken = SyntaxFactory.Token(SyntaxKind.SemicolonToken);

        private async Task<Document> Encapsulate(Document document, FieldDeclarationSyntax f, CancellationToken cancellationToken)
        {
            var fieldType = (GenericNameSyntax)f.Declaration.Type;
            var fieldName = f.Declaration.Variables.First().Identifier.ValueText;
            var lower = fieldName.TrimStart('_');
            var upper = char.ToUpper(lower[0]) + lower.Substring(1, lower.Length - 1);

            var elementType = fieldType.TypeArgumentList.Arguments.First();
            var oldNode = f.FirstAncestorOrSelf<ClassDeclarationSyntax>();

            var p = SyntaxFactory.PropertyDeclaration(elementType, upper)
                .WithModifiers(SyntaxTokenList.Create(PublicToken))
                .WithExpressionBody(
                    SyntaxFactory.ArrowExpressionClause(
                        ArrowToken,
                        SyntaxFactory.MemberAccessExpression(
                            SyntaxKind.SimpleMemberAccessExpression,
                            SyntaxFactory.IdentifierName(fieldName),
                            SyntaxFactory.IdentifierName("Value")
                        )
                    )
                )
                .WithSemicolonToken(SemicolonToken)
                .WithAdditionalAnnotations(Formatter.Annotation)
                ;

            var xx = p.ToString();
            var xx1 = p.GetText().ToString();

            var newNode = oldNode.InsertNodesBefore(f, new[] { p });

            //    .WithExpressionBody(SyntaxFactory.ArrowExpressionClause(

            var oldRoot = await document.GetSyntaxRootAsync(cancellationToken);
            var newRoot = oldRoot.ReplaceNode(oldNode, newNode);

            return document.WithSyntaxRoot(newRoot);
        }
    }
}