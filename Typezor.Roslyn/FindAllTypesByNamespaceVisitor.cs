using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;

namespace Typezor.Metadata.Roslyn;

public class FindAllTypesByNamespaceVisitor : SymbolVisitor<IEnumerable<INamedTypeSymbol>>
{
    private readonly string[] _requiredNamespaces;

    public FindAllTypesByNamespaceVisitor(params string[] requiredNamespaces)
    {
        _requiredNamespaces = requiredNamespaces;
    }
    public override IEnumerable<INamedTypeSymbol> VisitNamespace(INamespaceSymbol symbol)
    {
        foreach (var namespaceOrTypeSymbol in symbol.GetMembers())
        {
            if (!IsInNamespace(namespaceOrTypeSymbol)) continue;
            foreach (var typeSymbol in namespaceOrTypeSymbol.Accept(this) ?? Array.Empty<INamedTypeSymbol>())
            {
                yield return typeSymbol;
            }
        }
    }

    public override IEnumerable<INamedTypeSymbol> VisitNamedType(INamedTypeSymbol symbol)
    {
        if (!IsInNamespace(symbol)) yield break;
        if (symbol.DeclaredAccessibility != Accessibility.Public) yield break;

        yield return symbol;
        foreach (var childSymbol in symbol.GetTypeMembers())
        {
            foreach (var namedTypeSymbol in base.Visit(childSymbol) ?? Array.Empty<INamedTypeSymbol>())
            {
                yield return namedTypeSymbol;
            }
        }
    }

    private bool IsInNamespace(INamespaceOrTypeSymbol sym)
    {
        var fullNamespace = sym.GetNamespace();
        return fullNamespace == null || _requiredNamespaces.Any(p => p.StartsWith(fullNamespace) || fullNamespace.StartsWith(p));
    }

    private bool IsInNamespace(INamedTypeSymbol symbol)
    {
        var typeNamespace = symbol.GetNamespace();
        return typeNamespace == null || _requiredNamespaces.Any(p => typeNamespace.StartsWith(p));
    }
}