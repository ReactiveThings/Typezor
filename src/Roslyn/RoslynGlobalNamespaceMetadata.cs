using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Typezor.Metadata.Interfaces;

namespace Typezor.Metadata.Roslyn
{
    public class RoslynGlobalNamespaceMetadata : IFileMetadata
    {
        private readonly SymbolVisitor<IEnumerable<INamedTypeSymbol>> _symbolVisitor;
        private readonly INamespaceSymbol _namespaceSymbol;


        public RoslynGlobalNamespaceMetadata(INamespaceSymbol namespaceSymbol, SymbolVisitor<IEnumerable<INamedTypeSymbol>> symbolVisitor)
        {
            _symbolVisitor = symbolVisitor;
            _namespaceSymbol = namespaceSymbol;
        }

        public IEnumerable<IClassMetadata> Classes => RoslynClassMetadata.FromNamedTypeSymbols(GetNamespaceChildNodes());
        public IEnumerable<IDelegateMetadata> Delegates => RoslynDelegateMetadata.FromNamedTypeSymbols(GetNamespaceChildNodes());
        public IEnumerable<IEnumMetadata> Enums => RoslynEnumMetadata.FromNamedTypeSymbols(GetNamespaceChildNodes());
        public IEnumerable<IInterfaceMetadata> Interfaces => RoslynInterfaceMetadata.FromNamedTypeSymbols(GetNamespaceChildNodes());

        public IFileMetadata GetTypesFromNamespace(params string[] requiredNamespaces)
        {
            return new RoslynGlobalNamespaceMetadata(_namespaceSymbol, new FindAllTypesByNamespaceVisitor(requiredNamespaces));
        }

        private IEnumerable<INamedTypeSymbol> GetNamespaceChildNodes()
        {
            return _symbolVisitor.VisitNamespace(_namespaceSymbol);
        }
    }
}
