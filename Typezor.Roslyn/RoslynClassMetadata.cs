﻿using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;

using Typezor.Metadata.Interfaces;

namespace Typezor.Metadata.Roslyn
{
    public class RoslynClassMetadata : IClassMetadata
    {
        private readonly INamedTypeSymbol _symbol;

        private RoslynClassMetadata(INamedTypeSymbol symbol)
        {
            _symbol = symbol;
        }

        private IReadOnlyCollection<ISymbol> _members;
        private IReadOnlyCollection<ISymbol> Members
        {
            get
            {
                if (_members == null)
                {
                    _members = _symbol.GetMembers();
                }
                return _members;
            }
        }

        public string DocComment => _symbol.GetDocumentationCommentXml();
        public string Name => _symbol.Name;
        public string FullName => _symbol.ToDisplayString();
        public bool IsAbstract => _symbol.IsAbstract;
        public bool IsGeneric => _symbol.TypeParameters.Any();
        public bool IsRecord => _symbol.IsRecord;
        public string Namespace => _symbol.GetNamespace();

        public ITypeMetadata Type => RoslynTypeMetadata.FromTypeSymbol(_symbol);

        public IEnumerable<IAttributeMetadata> Attributes => RoslynAttributeMetadata.FromAttributeData(_symbol.GetAttributes());
        public IClassMetadata BaseClass => RoslynClassMetadata.FromNamedTypeSymbol(_symbol.BaseType);
        public IClassMetadata ContainingClass => RoslynClassMetadata.FromNamedTypeSymbol(_symbol.ContainingType);
        public IEnumerable<IConstantMetadata> Constants => RoslynConstantMetadata.FromFieldSymbols(Members.OfType<IFieldSymbol>());
        public IEnumerable<IDelegateMetadata> Delegates => RoslynDelegateMetadata.FromNamedTypeSymbols(Members.OfType<INamedTypeSymbol>().Where(s => s.TypeKind == TypeKind.Delegate));
        public IEnumerable<IEventMetadata> Events => RoslynEventMetadata.FromEventSymbols(Members.OfType<IEventSymbol>());
        public IEnumerable<IClassFieldMetadata> Fields => RoslynFieldMetadata.FromFieldSymbols(Members.OfType<IFieldSymbol>());
        public IEnumerable<IInterfaceMetadata> Interfaces => RoslynInterfaceMetadata.FromNamedTypeSymbols(_symbol.Interfaces);
        public IEnumerable<IMethodMetadata> Methods => RoslynMethodMetadata.FromMethodSymbols(Members.OfType<IMethodSymbol>());
        public IEnumerable<IMethodMetadata> StaticMethods => RoslynMethodMetadata.FromMethodSymbols(Members.OfType<IMethodSymbol>(), true);
        public IEnumerable<IPropertyMetadata> Properties => RoslynPropertyMetadata.FromPropertySymbol(Members.OfType<IPropertySymbol>());
        public IEnumerable<ITypeParameterMetadata> TypeParameters => RoslynTypeParameterMetadata.FromTypeParameterSymbols(_symbol.TypeParameters);
        public IEnumerable<ITypeMetadata> TypeArguments => RoslynTypeMetadata.FromTypeSymbols(_symbol.TypeArguments);
        public IEnumerable<IClassMetadata> NestedClasses => RoslynClassMetadata.FromNamedTypeSymbols(Members.OfType<INamedTypeSymbol>().Where(s => s.TypeKind == TypeKind.Class));
        public IEnumerable<IEnumMetadata> NestedEnums => RoslynEnumMetadata.FromNamedTypeSymbols(Members.OfType<INamedTypeSymbol>().Where(s => s.TypeKind == TypeKind.Enum));
        public IEnumerable<IInterfaceMetadata> NestedInterfaces => RoslynInterfaceMetadata.FromNamedTypeSymbols(Members.OfType<INamedTypeSymbol>().Where(s => s.TypeKind == TypeKind.Interface));

        internal static IClassMetadata FromNamedTypeSymbol(INamedTypeSymbol symbol)
        {
            if (symbol == null) return null;
            if (symbol.DeclaredAccessibility != Accessibility.Public || symbol.ToDisplayString() == "object") return null;

            return new RoslynClassMetadata(symbol);
        }

        public static IEnumerable<IClassMetadata> FromNamedTypeSymbols(IEnumerable<INamedTypeSymbol> symbols)
        {
            return symbols.Where(p => p.TypeKind == TypeKind.Class)
                .Where(s => s.DeclaredAccessibility == Accessibility.Public && s.ToDisplayString() != "object")
                .Select(s => new RoslynClassMetadata(s));
        }
    }
}
