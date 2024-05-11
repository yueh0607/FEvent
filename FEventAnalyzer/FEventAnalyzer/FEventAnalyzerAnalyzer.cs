using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;

namespace FEventAnalyzer
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class WhereEventAnalyzer : DiagnosticAnalyzer
    {
        public const string DiagnosticId = "WhereEventAnalyzer";
        private const string Title = "Invalid usage of WhereEvent attribute";
        private const string MessageFormat = "The type used with WhereEvent must be an interface";
        private const string Category = "Usage";

        private static readonly DiagnosticDescriptor Rule = new DiagnosticDescriptor(DiagnosticId, Title, MessageFormat, Category, DiagnosticSeverity.Error, isEnabledByDefault: true);

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get { return ImmutableArray.Create(Rule); } }

        public override void Initialize(AnalysisContext context)
        {
            context.RegisterSyntaxNodeAction(AnalyzeNode, SyntaxKind.InvocationExpression);
        }

        private static void AnalyzeNode(SyntaxNodeAnalysisContext context)
        {
            var invocation = (InvocationExpressionSyntax)context.Node;
            var methodSymbol = context.SemanticModel.GetSymbolInfo(invocation).Symbol as IMethodSymbol;

            // Check if the invocation is a call to a method with WhereEvent attribute
            if (methodSymbol != null && methodSymbol.MethodKind == MethodKind.Ordinary && methodSymbol.IsGenericMethod)
            {
                var attributes = methodSymbol.GetAttributes();
                if (attributes.Any(attr => attr.AttributeClass.Name == "WhereEventAttribute"))
                {
                    // Check type arguments passed to the method
                    foreach (var typeArgument in methodSymbol.TypeArguments)
                    {
                        if (!typeArgument.Interfaces.Any())
                        {
                            var diagnostic = Diagnostic.Create(Rule, invocation.GetLocation());
                            context.ReportDiagnostic(diagnostic);
                            break; // Only report one diagnostic for each invocation
                        }
                    }
                }
            }
        }
    }
}
