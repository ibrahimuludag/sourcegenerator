using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace Generator;

[Generator]
public class TheGenerator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        var provider = context.SyntaxProvider.CreateSyntaxProvider(
            predicate: static(node, _) => node is ClassDeclarationSyntax,
            transform: static (ctx, _) => (ClassDeclarationSyntax)ctx.Node
            ).Where(c => c is not null);

        var compilation = context.CompilationProvider.Combine(provider.Collect());

        context.RegisterSourceOutput(compilation, Execute);
    }

    private void Execute(SourceProductionContext context, (Compilation Left, ImmutableArray<ClassDeclarationSyntax> Right) tuple)
    {
        var (compilation, list) = tuple;

        var namelist = new List<string>();

        foreach (var systax in list) {
            var symbol = compilation.GetSemanticModel(systax.SyntaxTree)
                .GetDeclaredSymbol(systax) as INamedTypeSymbol;

            namelist.Add($"\"{symbol.ToDisplayString()}\"");
        }

        var names = string.Join(",\n ", namelist);

        var theCode = $$"""
            namespace  Generator;

            public static class ClassNames {
                public static List<string> Names = new(){
                    {{ names }}
                };
            }
            """;

        context.AddSource("YourClassList.g.cs", theCode);
    }
}
