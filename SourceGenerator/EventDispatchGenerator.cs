using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using System.Collections.Generic;
using System.Linq;
using System.Text;

[Generator]
public class SendEventExtensionGenerator : ISourceGenerator
{
    public void Initialize(GeneratorInitializationContext context)
    {
        // No initialization required for this example
    }

    public void Execute(GeneratorExecutionContext context) 
    {

        IEnumerable<SyntaxTree> syntaxTrees = context.Compilation.SyntaxTrees;

        foreach (SyntaxTree syntaxTree in syntaxTrees)
        {
            SyntaxNode root = syntaxTree.GetRoot();
            IEnumerable<InterfaceDeclarationSyntax> interfaces = root.DescendantNodes().OfType<InterfaceDeclarationSyntax>();

            foreach (InterfaceDeclarationSyntax @interface in interfaces)
            {
                if (IsSendEventInterface(@interface, out string methodName, out string parameterType,out string namespaceName))
                {
                    string extensionMethod = GenerateExtensionMethod(@interface.Identifier.ToString(), methodName, parameterType,namespaceName);
                    SourceText sourceText = SourceText.From(extensionMethod , Encoding.UTF8);
                    context.AddSource($"{@interface.Identifier.Text}Extensions.cs", sourceText);
                }
            }
        }      
    }

    private bool IsSendEventInterface(InterfaceDeclarationSyntax @interface, out string methodName, out string parameterType, out string namespaceName)
    {
        methodName = null;
        parameterType = null;
        namespaceName = null;

        // Get the namespace declaration containing the interface
        var namespaceDeclaration = @interface.FirstAncestorOrSelf<NamespaceDeclarationSyntax>();
        if (namespaceDeclaration != null)
        {
            // Get the fully qualified namespace name
            namespaceName = namespaceDeclaration.Name.ToString();
        }

        if (@interface.BaseList != null)
        {
            foreach (var baseType in @interface.BaseList.Types)
            {
                var baseInterface = baseType.Type as GenericNameSyntax;
                if (baseInterface != null && baseInterface.Identifier.Text == "ISendEvent")
                {
                    // Check if there's only one method with parameters matching ISendEvent's generic type
                    var method = (MethodDeclarationSyntax)@interface.Members.FirstOrDefault(member =>
                        member is MethodDeclarationSyntax methodDeclaration &&
                        methodDeclaration.ParameterList.Parameters.Count == baseInterface.TypeArgumentList.Arguments.Count);

                    if (method != null)
                    {
                        // Get the method name
                        methodName = method.Identifier.Text;
                        parameterType = baseInterface.TypeArgumentList.Arguments[0].ToString();
                        return true;
                    }
                }
            }
        }

        return false;
    }

    private string GenerateExtensionMethod(string interfaceName, string methodName, string parameterType,string namespaceName)
    {
        // Generate the extension method
        return $@"
using System;
using {namespaceName}; 
public static partial class {interfaceName}Extensions
{{
    public static void Send(this {interfaceName} obj, {parameterType} parameter)
    {{
        obj.{methodName}(parameter);
    }}
}}
";
    }
}
