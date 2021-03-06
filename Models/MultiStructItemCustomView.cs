﻿using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Reflection;
using JetBrains.Annotations;
using MHW_Editor.Assets;
using MHW_Editor.Windows;
using MHW_Template;

namespace MHW_Editor.Models {
    public class MultiStructItemCustomView : MhwStructItem {
        private readonly IHasCustomView<MultiStructItemCustomView> parent;
        private readonly string                                    name;
        private readonly MethodInfo                                getMethod;
        private readonly MethodInfo                                setMethod;
        private readonly MethodInfo                                offsetGetMethod;
        private readonly Type                                      propertyType;
        private readonly bool                                      isReadOnly;

        public MultiStructItemCustomView(IHasCustomView<MultiStructItemCustomView> parent, string name, string propertyName, string offsetPropName) {
            this.parent = parent;
            this.name   = name;

            var property = parent.GetType().GetProperty(propertyName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            Debug.Assert(property != null, nameof(property) + " != null");

            propertyType = property.PropertyType;
            getMethod    = property.GetGetMethod();
            setMethod    = property.GetSetMethod();

            isReadOnly = (IsReadOnlyAttribute) property.GetCustomAttribute(typeof(IsReadOnlyAttribute), true) != null;

            property = parent.GetType().GetProperty(offsetPropName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            Debug.Assert(property != null, nameof(property) + " != null");

            offsetGetMethod = property.GetGetMethod();
        }

        [SortOrder(0)]
        [UsedImplicitly]
        public string Name {
            get {
                if (DataHelper.translations.TryGet(MainWindow.locale, null)?.ContainsKey(name) ?? false) {
                    return DataHelper.translations[MainWindow.locale][name];
                }
                return name;
            }
        }

        [DisplayName("")] // Hide it for vertical view since it will always be 0;
        public override ulong Index => base.Index;

        [SortOrder(25)]
        [DisplayName("Data")]
        public object Data {
            get {
                var value = getMethod.Invoke(parent, null);
                return Convert.ChangeType(value, typeof(object));
            }
            set {
                if (setMethod == null || isReadOnly) return;
                try {
                    var returnValue = Convert.ChangeType(value, propertyType);
                    setMethod.Invoke(parent, new[] {returnValue});
                    OnPropertyChanged(nameof(Data));
                } catch (FormatException) {
                }
            }
        }

        [SortOrder(50)]
        [DisplayName("Type")]
        [UsedImplicitly]
        public string Type {
            get {
                // ReSharper disable once SwitchExpressionHandlesSomeKnownEnumValuesWithExceptionInDefault
                return System.Type.GetTypeCode(propertyType) switch {
                    TypeCode.Single => "float",
                    TypeCode.Double => "double",
                    TypeCode.Byte => "uInt8",
                    TypeCode.SByte => "sInt8",
                    TypeCode.UInt16 => "uInt16",
                    TypeCode.Int16 => "sInt16",
                    TypeCode.UInt32 => "uInt32",
                    TypeCode.Int32 => "sInt32",
                    TypeCode.Int64 => "sInt64",
                    TypeCode.UInt64 => "sInt64",
                    TypeCode.String => "string",
                    _ => throw new ArgumentOutOfRangeException()
                };
            }
        }

        [SortOrder(75)]
        [DisplayName("Offset")]
        public long Offset => (long) offsetGetMethod.Invoke(parent, null);
    }
}