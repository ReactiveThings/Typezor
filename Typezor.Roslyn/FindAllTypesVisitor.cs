using System;
using System.Collections.Generic;
using Microsoft.CodeAnalysis;

namespace Typezor.Metadata.Roslyn
{
    public class FindAllTypesVisitor : SymbolVisitor<IEnumerable<INamedTypeSymbol>>
    {
        public override IEnumerable<INamedTypeSymbol> VisitNamespace(INamespaceSymbol symbol)
        {
            foreach (INamespaceOrTypeSymbol sym in symbol.GetMembers())
            {
                foreach (var typeSymbol in sym.Accept(this) ?? Array.Empty<INamedTypeSymbol>())
                {
                    yield return typeSymbol;
                }
            }
        }

        public override IEnumerable<INamedTypeSymbol> VisitNamedType(INamedTypeSymbol symbol)
        {
            yield return symbol;
            foreach (var childSymbol in symbol.GetTypeMembers())
            {
                foreach (var namedTypeSymbol in base.Visit(childSymbol) ?? Array.Empty<INamedTypeSymbol>())
                {
                    yield return namedTypeSymbol;
                }
            }
        }
    }
}