using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Typezor.Metadata.Interfaces;

namespace Typezor.Metadata.Roslyn
{
    public class RoslynGlobalNamespaceMetadata : IFileMetadata
    {
        private readonly INamespaceSymbol _namespaceSymbol;


        public RoslynGlobalNamespaceMetadata(INamespaceSymbol namespaceSymbol)
        {
            _namespaceSymbol = namespaceSymbol;
        }

        public IEnumerable<IClassMetadata> Classes => RoslynClassMetadata.FromNamedTypeSymbols(GetNamespaceChildNodes());
        public IEnumerable<IDelegateMetadata> Delegates => RoslynDelegateMetadata.FromNamedTypeSymbols(GetNamespaceChildNodes());
        public IEnumerable<IEnumMetadata> Enums => RoslynEnumMetadata.FromNamedTypeSymbols(GetNamespaceChildNodes());
        public IEnumerable<IInterfaceMetadata> Interfaces => RoslynInterfaceMetadata.FromNamedTypeSymbols(GetNamespaceChildNodes());


        private IEnumerable<INamedTypeSymbol> GetNamespaceChildNodes()
        {
            var visitor = new FindAllTypesVisitor();
            return visitor.VisitNamespace(_namespaceSymbol);
        }
    }
}
