﻿using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using System.Collections.Generic;
using System.Linq;
using System.Text;

[Generator]
public class SendEventExtensionGenerator : ISourceGenerator
{
    public void Execute(GeneratorExecutionContext context)
    {
        // Get the syntax trees of all the source files
        IEnumerable<SyntaxTree> syntaxTrees = context.Compilation.SyntaxTrees;

        // Create a StringBuilder to store the generated source code
        StringBuilder stringBuilder = new StringBuilder();

        // Iterate through each syntax tree
        foreach (SyntaxTree syntaxTree in syntaxTrees)
        {
            // Get the root node of the syntax tree
            SyntaxNode root = syntaxTree.GetRoot();

            // Find all interface declarations in the syntax tree
            IEnumerable<InterfaceDeclarationSyntax> interfaces = root.DescendantNodes().OfType<InterfaceDeclarationSyntax>();

            // Process each interface
            foreach (InterfaceDeclarationSyntax @interface in interfaces)
            {
                // Check if the interface inherits from ISendEvent
                if (IsSendEventInterface(@interface, out string methodName, out string[] parameterTypes, out string[] parameterNames, out string namespaceName))
                {
                    string extensionMethodClass = GenerateExtensionMethodClass(@interface.Identifier.ToString(), namespaceName, methodName, parameterTypes, parameterNames);
                    stringBuilder.AppendLine(extensionMethodClass);

                    SourceText sourceText = SourceText.From(stringBuilder.ToString(), Encoding.UTF8);

                    context.AddSource($"SendEvent_{@interface.Identifier.Text}_Extensions.cs", sourceText);

                    stringBuilder.Clear();
                }
            }
        }

    }
    private bool IsSendEventInterface(InterfaceDeclarationSyntax @interface, out string methodName, out string[] parameterType, out string[] parameterNames, out string namespaceName)
    {
        methodName = null;
        parameterType = null;
        parameterNames = null;
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
                var notGenericBaseInterface = baseType.Type as IdentifierNameSyntax;
                if (baseInterface != null && baseInterface.Identifier.Text == "ISendEvent" && baseInterface?.TypeArgumentList.Arguments != null && baseInterface.TypeArgumentList.Arguments.Count > 0)
                {
                    // Get the generic type arguments of ISendEvent
                    var genericTypeArguments = baseInterface.TypeArgumentList.Arguments;

                    // Define temporary variables to store the method name, parameter type, and parameter names
                    string tempMethodName = null;
                    List<string> tempParameterType = new List<string>();
                    List<string> tempParameterNames = new List<string>();

                    // Find any method with parameters matching ISendEvent's generic type arguments
                    var matchingMethod = @interface.Members.FirstOrDefault(member =>
                    {
                        if (member is MethodDeclarationSyntax methodDeclaration)
                        {
                            var parameters = methodDeclaration.ParameterList.Parameters;
                            // Check if the number of parameters matches the number of generic type arguments
                            if (parameters.Count == genericTypeArguments.Count)
                            {
                                bool allParametersMatch = true;
                                for (int i = 0; i < parameters.Count; i++)
                                {
                                    var parameterTypeName = parameters[i].Type.ToString();
                                    if (parameterTypeName != genericTypeArguments[i].ToString())
                                    {
                                        allParametersMatch = false;
                                        break;
                                    }
                                    tempParameterNames.Add(parameters[i].Identifier.Text);
                                    tempParameterType.Add(parameterTypeName);
                                }
                                if (allParametersMatch)
                                {
                                    // Found a method with matching parameters
                                    tempMethodName = methodDeclaration.Identifier.Text;
                                    return true;
                                }
                            }
                        }
                        return false;
                    });

                    // Assign the temporary method name, parameter type, and parameter names to the output parameters
                    methodName = tempMethodName;
                    parameterType = tempParameterType.ToArray();
                    parameterNames = tempParameterNames.ToArray();

                    // If a matching method is found, return true
                    if (matchingMethod != null)
                    {
                        return true;
                    }
                }
                else if (notGenericBaseInterface != null && notGenericBaseInterface.Identifier.Text == "ISendEvent")
                {
                    string tempMethodName = null;
                    var matchingMethod = @interface.Members.FirstOrDefault(member =>
                    {
                        if (member is MethodDeclarationSyntax methodDeclaration)
                        {
                            var parameters = methodDeclaration.ParameterList.Parameters;
                            // Check if the number of parameters matches the number of generic type arguments
                            if (parameters.Count == 0)
                            {
                                // Found a method with matching parameters
                                tempMethodName = methodDeclaration.Identifier.Text;
                                return true;
                            }
                        }
                        return false;
                    });
                    methodName = tempMethodName;
                    parameterType = new string[0];
                    parameterNames = new string[0];


                    // If a matching method is found, return true
                    if (matchingMethod != null)
                    {
                        return true;
                    }
                }
            }
        }

        return false;
    }






    private string GenerateExtensionMethodClass(string interfaceName, string namespaceName, string methodName, string[] parameterTypes, string[] parameterNames)
    {
        string signature = ',' + string.Join(", ", parameterTypes.Zip(parameterNames, (type, name) => $"{type} {name}"));
        if (parameterNames.Length == 0) signature = "";

        string inputParameters = string.Join(", ", parameterNames);

        string listName = $"a1588_57_list_13845_812841";
        string objName = $"a_655__obj_333695__254";
        string publisherName = $"publisher_3_3_8184";
        string convertObjName = $"convert_a_655__obj_333695__254";
        string extensionMethodClass = $@"
using global::System;
using {namespaceName};

namespace FEvent
{{
    public static class {interfaceName}PublishExtensions
    {{
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static void Send<T>(this object {objName} {signature}) where T : {interfaceName}
        {{
            if({objName} is {interfaceName} {convertObjName})
                {convertObjName}.{methodName}({inputParameters});
        }}

        public static void SendAll<T>(this IEventPublisher {publisherName} {signature}) where T : {interfaceName}
        {{
            DynamicQueue<IEventListener> {listName} = {publisherName}.GetPublishableEvents<T>(typeof(T));
            if ({listName} != null)
            {{
                {listName}.StartEnum();
                while ({listName}.MoveNext(out IEventListener {objName}))
                {{
                    ((T){objName}).{methodName}({inputParameters});
                    {listName}.Return({objName});
                }}
                {listName}.EndEnum();
            }}
        }}
    }}
}}";

        return extensionMethodClass;
    }

    public void Initialize(GeneratorInitializationContext context)
    {

    }
}
