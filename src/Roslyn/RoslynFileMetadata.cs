using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

using Typezor.Metadata.Interfaces;

namespace Typezor.Metadata.Roslyn
{
    public class RoslynFileMetadata : IFileMetadata
    {
        private readonly SyntaxNode _root;
        private readonly SemanticModel _semanticModel;

        public RoslynFileMetadata(SemanticModel semanticModel)
        {
            _semanticModel = semanticModel;
            _root = _semanticModel.SyntaxTree.GetRoot();
        }

        public IEnumerable<IClassMetadata> Classes => RoslynClassMetadata.FromNamedTypeSymbols(GetNamespaceChildNodes<ClassDeclarationSyntax>());
        public IEnumerable<IDelegateMetadata> Delegates => RoslynDelegateMetadata.FromNamedTypeSymbols(GetNamespaceChildNodes<DelegateDeclarationSyntax>());
        public IEnumerable<IEnumMetadata> Enums => RoslynEnumMetadata.FromNamedTypeSymbols(GetNamespaceChildNodes<EnumDeclarationSyntax>());
        public IEnumerable<IInterfaceMetadata> Interfaces => RoslynInterfaceMetadata.FromNamedTypeSymbols(GetNamespaceChildNodes<InterfaceDeclarationSyntax>(), this);


        private IEnumerable<INamedTypeSymbol> GetNamespaceChildNodes<T>() where T : SyntaxNode
        {
            var symbols = _root.ChildNodes().OfType<T>().Concat(
                _root.ChildNodes().OfType<NamespaceDeclarationSyntax>().SelectMany(n => n.ChildNodes().OfType<T>()))
                .Select(c => _semanticModel.GetDeclaredSymbol(c) as INamedTypeSymbol);

            return symbols;
        }
    }
}
