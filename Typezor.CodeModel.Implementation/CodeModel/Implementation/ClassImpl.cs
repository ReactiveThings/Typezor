﻿using System.Collections.Generic;
using System.Linq;

using Typezor.Metadata.Interfaces;
using static Typezor.CodeModel.Helpers;

namespace Typezor.CodeModel.Implementation
{
    public sealed class ClassImpl : Class
    {
        private readonly IClassMetadata _metadata;

        private ClassImpl(IClassMetadata metadata, Item parent)
        {
            _metadata = metadata;
            Parent = parent;
        }

        public override Item Parent { get; }

        
        public override string name => CamelCase(_metadata.Name.TrimStart('@'));
        public override string Name => _metadata.Name.TrimStart('@');
        public override string FullName => _metadata.FullName;
        public override string Namespace => _metadata.Namespace;
        public override bool IsAbstract => _metadata.IsAbstract;
        public override bool IsGeneric => _metadata.IsGeneric;

        private Type _type;
        protected override Type Type => _type ?? (_type = TypeImpl.FromMetadata(_metadata.Type, Parent));
        
        private IEnumerable<Attribute> _attributes;
        public override IEnumerable<Attribute> Attributes => _attributes ?? (_attributes = AttributeImpl.FromMetadata(_metadata.Attributes, this));

        private IEnumerable<CodeModel.Constant> _constants;
        public override IEnumerable<CodeModel.Constant> Constants => _constants ?? (_constants = ConstantImpl.FromMetadata(_metadata.Constants, this));

        private IEnumerable<CodeModel.Delegate> _delegates;
        public override IEnumerable<CodeModel.Delegate> Delegates => _delegates ?? (_delegates = DelegateImpl.FromMetadata(_metadata.Delegates, this));

        private DocComment _docComment;
        public override DocComment DocComment => _docComment ?? (_docComment = DocCommentImpl.FromXml(_metadata.DocComment, this));

        private IEnumerable<Event> _events;
        public override IEnumerable<Event> Events => _events ?? (_events = EventImpl.FromMetadata(_metadata.Events, this));

        private IEnumerable<CodeModel.Field> _fields;
        public override IEnumerable<CodeModel.Field> Fields => _fields ?? (_fields = FieldImpl.FromMetadata(_metadata.Fields, this));

        private Class _baseClass;
        public override Class BaseClass => _baseClass ?? (_baseClass = ClassImpl.FromMetadata(_metadata.BaseClass, this));

        private Class _containingClass;
        public override Class ContainingClass => _containingClass ?? (_containingClass = ClassImpl.FromMetadata(_metadata.ContainingClass, this));

        private IEnumerable<Interface> _interfaces;
        public override IEnumerable<Interface> Interfaces => _interfaces ?? (_interfaces = InterfaceImpl.FromMetadata(_metadata.Interfaces, this));

        private IEnumerable<Method> _methods;
        public override IEnumerable<Method> Methods => _methods ?? (_methods = MethodImpl.FromMetadata(_metadata.Methods, this));

        private IEnumerable<Method> _staticMethods;
        public override IEnumerable<Method> StaticMethods => _staticMethods ?? (_staticMethods = MethodImpl.FromMetadata(_metadata.StaticMethods, this));

        private IEnumerable<CodeModel.Property> _properties;
        public override IEnumerable<CodeModel.Property> Properties => _properties ?? (_properties = PropertyImpl.FromMetadata(_metadata.Properties, this));

        private IEnumerable<TypeParameter> _typeParameters;
        public override IEnumerable<TypeParameter> TypeParameters => _typeParameters ?? (_typeParameters = TypeParameterImpl.FromMetadata(_metadata.TypeParameters, this));

        private IEnumerable<CodeModel.Type> _typeArguments;
        public override IEnumerable<CodeModel.Type> TypeArguments => _typeArguments ?? (_typeArguments = TypeImpl.FromMetadata(_metadata.TypeArguments, this));

        private IEnumerable<CodeModel.Class> _nestedClasses;
        public override IEnumerable<CodeModel.Class> NestedClasses => _nestedClasses ?? (_nestedClasses = ClassImpl.FromMetadata(_metadata.NestedClasses, this));

        private IEnumerable<Enum> _nestedEnums;
        public override IEnumerable<Enum> NestedEnums => _nestedEnums ?? (_nestedEnums = EnumImpl.FromMetadata(_metadata.NestedEnums, this));

        private IEnumerable<Interface> _nestedInterfaces;
        public override IEnumerable<Interface> NestedInterfaces => _nestedInterfaces ?? (_nestedInterfaces = InterfaceImpl.FromMetadata(_metadata.NestedInterfaces, this));

        public override string ToString()
        {
            return Name;
        }

        public static IEnumerable<CodeModel.Class> FromMetadata(IEnumerable<IClassMetadata> metadata, Item parent)
        {
            return metadata.Select(c => new ClassImpl(c, parent));
        }

        public static Class FromMetadata(IClassMetadata metadata, Item parent)
        {
            return metadata == null ? null : new ClassImpl(metadata, parent);
        }
    }
}
