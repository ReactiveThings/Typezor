using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Typezor.Metadata.Interfaces;

namespace Typezor.Metadata.Roslyn
{
    public class RoslynFieldMetadata : IClassFieldMetadata
    {
        private readonly IFieldSymbol symbol;

        protected RoslynFieldMetadata(IFieldSymbol symbol)
        {
            this.symbol = symbol;
        }

        public string DocComment => symbol.GetDocumentationCommentXml();
        public string Name => symbol.Name;
        public string FullName => symbol.ToDisplayString();
        public IEnumerable<IAttributeMetadata> Attributes => RoslynAttributeMetadata.FromAttributeData(symbol.GetAttributes());
        public ITypeMetadata Type => RoslynTypeMetadata.FromTypeSymbol(symbol.Type);
        public bool IsReadOnly => symbol.IsReadOnly;

        public static IEnumerable<IClassFieldMetadata> FromFieldSymbols(IEnumerable<IFieldSymbol> symbols)
        {
            return symbols.Where(s => s.DeclaredAccessibility == Accessibility.Public && s.IsConst == false && s.IsStatic == false).Select(s => new RoslynFieldMetadata(s));
        }

        
    }
}