﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using JetBrains.Annotations;
using MHW_Editor.Assets;
using MHW_Editor.Controls.Models;
using MHW_Editor.Gems;
using MHW_Editor.Items;
using MHW_Editor.Models;
using MHW_Editor.Skills;
using MHW_Editor.Weapons;
using MHW_Template;
using MHW_Template.Armors;
using MHW_Template.Items;
using MHW_Template.Models;
using MHW_Template.Weapons;

namespace MHW_Editor.Controls {
    public abstract partial class MhwDataGrid {
        protected static readonly Brush BACKGROUND_BRUSH = (Brush) new BrushConverter().ConvertFrom("#c0e1fb");

        protected MhwDataGrid() {
            InitializeComponent();
        }

        protected abstract void On_AutoGeneratedColumns(object sender, EventArgs e);

        protected abstract void On_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e);

        protected abstract void On_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e);

        protected abstract void On_GotFocus(object sender, RoutedEventArgs e);

        protected abstract void On_Sorting(object sender, DataGridSortingEventArgs e);

        protected abstract void On_Cell_MouseClick(object sender, MouseButtonEventArgs e);
    }

    public class MhwDataGridDynamic : MhwDataGridGeneric<dynamic> {
    }

    public class MhwDataGridGeneric<T> : MhwDataGrid, IMhwDataGrid<T> where T : class {
        private             Dictionary<string, ColumnHolder> columnMap;
        [CanBeNull] private DataGridRow                      coloredRow;
        private             bool                             isManualEditCommit;

        public MainWindow mainWindow;

        private ObservableCollection<T> items;
        public new ObservableCollection<T> Items {
            get => items;
            private set {
                columnMap  = new Dictionary<string, ColumnHolder>();
                coloredRow = null;
                items      = value;

                if (value == null) {
                    ItemsSource = null;
                    return;
                }

                // ReSharper disable once ConvertIfStatementToConditionalTernaryExpression
                if (mainWindow.targetFileType.IsGeneric(typeof(IHasCustomView<>))) {
                    ItemsSource = new ListCollectionView(((dynamic) items[0]).GetCustomView());
                } else {
                    ItemsSource = new ListCollectionView(items);
                }

                if (mainWindow.targetFileType.Is(typeof(DecoGradeLottery), typeof(DecoLottery), typeof(KulveGradeLottery), typeof(SafiItemGradeLottery))) {
                    CalculatePercents();
                }
            }
        }

        [SuppressMessage("ReSharper", "ParameterHidesMember")]
        public void SetItems(MainWindow mainWindow, ObservableCollection<T> items) {
            this.mainWindow = mainWindow;
            Items           = items;
        }

        protected override void On_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e) {
            switch (e.PropertyName) {
                case nameof(IMhwItem.Bytes):
                case nameof(IMhwItem.UniqueId):
                case nameof(Melee.GMD_Name_Index):
                case nameof(Melee.GMD_Description_Index):
                    e.Cancel = true; // Internal.
                    break;
                case nameof(IMhwItem.Raw_Data):
                    e.Cancel = !MainWindow.SHOW_RAW_BYTES; // Only for debug builds.
                    break;
                case nameof(Ranged.Barrel_Type):
                case nameof(Ranged.Deviation):
                case nameof(Ranged.Magazine_Type):
                case nameof(Ranged.Muzzle_Type):
                case nameof(Ranged.Scope_Type):
                case nameof(Ranged.Shell_Type_Id):
                    e.Cancel = mainWindow.targetFileType.Is(typeof(Bow));
                    break;
                case nameof(SkillDat.Id):
                    e.Cancel = mainWindow.targetFileType.Is(typeof(SkillDat));
                    break;
                default:
                    e.Cancel = e.PropertyName.EndsWith("Raw");
                    break;
            }

            Type sourceClassType = ((dynamic) e.PropertyDescriptor).ComponentType;

            // Cancel for _button columns as we will use a text version with onClick opening a selector.
            if (ButtonTypeInfo.TYPE_AND_NAME.ContainsKey(sourceClassType)
                && ButtonTypeInfo.TYPE_AND_NAME[sourceClassType].Contains(e.PropertyName)) {
                e.Cancel = true;
            }

            if (e.Cancel) return;

            // Create 'X' button for delete column.
            if (e.PropertyName == "Delete") {
                var col  = new DataGridTemplateColumn();
                var btn1 = new FrameworkElementFactory(typeof(Button));

                btn1.SetValue(ContentControl.ContentProperty, "X");
                btn1.AddHandler(ButtonBase.ClickEvent, new RoutedEventHandler(On_Cell_Delete_Click));

                col.CellTemplate = new DataTemplate {VisualTree = btn1};
                e.Column         = col;
            }

            if (e.PropertyName.EndsWith("_percent")) {
                var cb = new DataGridTextColumn {
                    Header = e.Column.Header,
                    Binding = new Binding(e.PropertyName) {
                        StringFormat = "{0:0.##%;-0.##%;\"\"}" // Can't be negative, but needed to hide all 0 cases.
                    },
                    CanUserSort = true,
                    IsReadOnly  = true
                };
                e.Column = cb;
            }

            if (e.PropertyType == typeof(DateTime)) {
                var cb = new DataGridTextColumn {
                    Header = e.Column.Header,
                    Binding = new Binding(e.PropertyName) {
                        StringFormat = "{0:yyyy-MM-dd}" // Can't be negative, but needed to hide all 0 cases.
                    },
                    CanUserSort = true,
                    IsReadOnly  = true
                };
                e.Column = cb;
            }

            if (e.PropertyName == "Index") {
                e.Column.IsReadOnly = true; // Do before normal readOnly checks.
            }

            e.Column.CanUserSort = true;

            // Use [DisplayName] attribute for the column header text.
            // Use [SortOrder] attribute to control the position. Generated fields have spacing so it's easy to say 'generated_field_sortOrder + 1'.
            // Use [CustomSorter] to define an IComparer class to control sorting.
            var           propertyInfo     = sourceClassType.GetProperties().FirstOrDefault(info => info.Name == e.PropertyName);
            var           displayName      = ((DisplayNameAttribute) propertyInfo?.GetCustomAttribute(typeof(DisplayNameAttribute), true))?.DisplayName;
            var           sortOrder        = ((SortOrderAttribute) propertyInfo?.GetCustomAttribute(typeof(SortOrderAttribute), true))?.sortOrder;
            var           customSorterType = ((CustomSorterAttribute) propertyInfo?.GetCustomAttribute(typeof(CustomSorterAttribute), true))?.customSorterType;
            var           isReadOnly       = (IsReadOnlyAttribute) propertyInfo?.GetCustomAttribute(typeof(IsReadOnlyAttribute), true) != null;
            ICustomSorter customSorter     = null;

            if (displayName != null) {
                if (displayName == "") { // Use empty DisplayName as a way to hide columns.
                    e.Cancel = true;
                    return;
                }

                e.Column.Header = displayName;
            }

            if (e.PropertyType.IsGenericType && e.PropertyType.GetGenericTypeDefinition() == typeof(ObservableCollection<>)) {
                var col  = new DataGridTemplateColumn();
                var btn1 = new FrameworkElementFactory(typeof(Button));

                btn1.SetValue(ContentControl.ContentProperty, "Open");
                btn1.AddHandler(ButtonBase.ClickEvent, new RoutedEventHandler((o, args) => On_Cell_Open_Click(o, args, propertyInfo, displayName)));

                col.CellTemplate = new DataTemplate {VisualTree = btn1};
                e.Column         = col;
                e.Column.Header  = displayName;
            }

            if (customSorterType != null) {
                customSorter = (ICustomSorter) Activator.CreateInstance(customSorterType);
                if (customSorter is ICustomSorterWithPropertyName csWithName) {
                    csWithName.PropertyName = e.PropertyName;
                }
            }

            if (isReadOnly) {
                e.Column.IsReadOnly = !mainWindow.unlockFields;
            }

            columnMap[e.PropertyName] = new ColumnHolder(e.Column, sortOrder ?? -1, customSorter);

            // TODO: Fix enum value display at some point.
        }

        protected override void On_AutoGeneratedColumns(object sender, EventArgs e) {
            var columns = columnMap.Values.ToList();
            columns.Sort((c1, c2) => c1.sortOrder.CompareTo(c2.sortOrder));
            for (var i = 0; i < columns.Count; i++) {
                columns[i].column.DisplayIndex = i;
            }
        }

        protected override void On_GotFocus(object sender, RoutedEventArgs e) {
            try {
                // Lookup for the source to be DataGridCell
                if (e.OriginalSource is DataGridCell cell) {
                    ColorCell(cell);

                    if (mainWindow.SingleClickToEditMode) {
                        // Needs to only happen when it's a button. If not, we stop regular fields from working.
                        if (CheckCellForButtonTypeAndHandleClick(cell)) return;

                        // We're past the _button check, now we just want to avoid a normal drop-down set to read only.
                        if (cell.IsReadOnly) return;

                        // Starts the Edit on the row;
                        BeginEdit(e);

                        if (cell.Content is ComboBox cbx) {
                            cbx.IsDropDownOpen = true;
                        }
                    }
                }
            } catch (Exception err) when (!Debugger.IsAttached) {
                MainWindow.ShowError(err, "Error Occured");
            }
        }

        private void ColorCell(DependencyObject cell) {
            if (coloredRow != null) coloredRow.Background = Brushes.White;
            coloredRow = cell.GetParent<DataGridRow>();
            // ReSharper disable once PossibleNullReferenceException
            coloredRow.Background = BACKGROUND_BRUSH;
        }

        protected override void On_Cell_MouseClick(object sender, MouseButtonEventArgs e) {
            try {
                if (sender is DataGridCell cell) {
                    // We come here on both single & double click. If we don't check for focus, this hijacks the click and prevents focusing.
                    if (e?.ClickCount == 1 && !cell.IsFocused) return;

                    CheckCellForButtonTypeAndHandleClick(cell);
                }
            } catch (Exception err) when (!Debugger.IsAttached) {
                MainWindow.ShowError(err, "Error Occured");
            }
        }

        private void On_Cell_Delete_Click(object sender, RoutedEventArgs e) {
            try {
                var obj = ((FrameworkElement) sender).DataContext;
                Items.Remove((T) obj);
            } catch (Exception err) when (!Debugger.IsAttached) {
                MainWindow.ShowError(err, "Error Occured");
            }
        }

        private void On_Cell_Open_Click(object sender, RoutedEventArgs e, PropertyInfo propertyInfo, string displayName) {
            try {
                var frameworkElement = (FrameworkElement) sender;
                var obj              = frameworkElement.DataContext;
                var list             = propertyInfo.GetGetMethod().Invoke(obj, null);
                var listType         = propertyInfo.PropertyType.GenericTypeArguments[0];
                var viewType         = typeof(SubStructViewDynamic<>).MakeGenericType(listType);
                var isReadOnly       = (bool) (listType.GetField(nameof(MhwStructDataContainer.IsAddingAllowed), BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)?.GetValue(null) ?? false);
                var subStructView    = (SubStructView) Activator.CreateInstance(viewType, mainWindow, displayName, list, isReadOnly);

                ColorCell(frameworkElement);
                subStructView.ShowDialog();
            } catch (Exception err) when (!Debugger.IsAttached) {
                MainWindow.ShowError(err, "Error Occured");
            }
        }

        private bool CheckCellForButtonTypeAndHandleClick(DataGridCell cell) {
            if (!(cell.Content is TextBlock)) return false;

            // Have to loop though our column list to file the original property name.
            return (from propertyName in columnMap.Keys.Where(key => key.Contains("_button"))
                    where cell.Column == columnMap[propertyName].column
                    select EditSelectedItemId(cell, propertyName)).FirstOrDefault();
        }

        private bool EditSelectedItemId(FrameworkElement cell, string propertyName) {
            var obj      = (IOnPropertyChanged) cell.DataContext;
            var property = obj.GetType().GetProperty(propertyName.Replace("_button", ""), BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
            Debug.Assert(property != null, nameof(property) + " != null");
            var propertyType   = property.PropertyType;
            var value          = property.GetValue(obj);
            var dataSourceType = ((DataSourceAttribute) property.GetCustomAttribute(typeof(DataSourceAttribute), true))?.dataType;
            var isReadOnly     = (IsReadOnlyAttribute) property.GetCustomAttribute(typeof(IsReadOnlyAttribute), true) != null;

            if (isReadOnly) return false;

            dynamic dataSource = dataSourceType switch {
                DataSourceType.Items => DataHelper.itemNames[MainWindow.locale],
                DataSourceType.Skills => DataHelper.skillNames[MainWindow.locale],
                DataSourceType.SkillDat => MainWindow.skillDatLookup[MainWindow.locale],
                DataSourceType.ArmorById => DataHelper.armorIdNameLookup[GetArmorType(cell)][MainWindow.locale],
                DataSourceType.ArmorByIndex => DataHelper.armorIndexNameLookup[GetArmorType(cell)][MainWindow.locale],
                DataSourceType.ArmorByIndexNeg => DataHelper.armorIndexNegNameLookup[GetArmorType(cell)][MainWindow.locale],
                DataSourceType.ArmorByFileIndexNeg => DataHelper.armorFileIndexNegNameLookup[MainWindow.locale],
                DataSourceType.WeaponsById => DataHelper.weaponIdNameLookup[GetWeaponType(cell)][MainWindow.locale],
                DataSourceType.WeaponsByIndex => DataHelper.weaponIndexNameLookup[GetWeaponType(cell)][MainWindow.locale],
                DataSourceType.EquipmentById => DataHelper.equipmentIdNameLookup[GetEquipmentType(cell)][MainWindow.locale],
                DataSourceType.Pendants => DataHelper.pendantNames[MainWindow.locale],
                DataSourceType.Monsters => DataHelper.monsterNames[MainWindow.locale],
                DataSourceType.MonstersNeg => DataHelper.monsterNamesNeg[MainWindow.locale],
                DataSourceType.ShellRecoil => ShellTable.recoilLookup,
                DataSourceType.ShellReload => ShellTable.reloadLookup,
                DataSourceType.GunnerRecoil => GunnerShoot.recoilLookup,
                DataSourceType.GunnerReload => GunnerReload.reloadLookup,
                DataSourceType.MantleByIdNeg => DataHelper.mantleNamesNeg[MainWindow.locale],
                DataSourceType.KinsectById => DataHelper.kinsectNames[MainWindow.locale],
                _ => throw new ArgumentOutOfRangeException(dataSourceType.ToString())
            };

            var getNewItemId = new GetNewItemId(value, dataSource);

            getNewItemId.ShowDialog();

            if (!getNewItemId.Cancelled) {
                property.SetValue(obj, Convert.ChangeType(getNewItemId.CurrentItem, propertyType));
                obj.OnPropertyChanged(propertyName);
            }

            return true;
        }

        private static ArmorType GetArmorType(FrameworkElement cell) {
            return ((IHasArmorType) cell.DataContext).GetArmorType();
        }

        private static WeaponType GetWeaponType(FrameworkElement cell) {
            return ((IHasWeaponType) cell.DataContext).GetWeaponType();
        }

        private static EquipmentType GetEquipmentType(FrameworkElement cell) {
            return ((IHasEquipmentType) cell.DataContext).GetEquipmentType();
        }

        protected override void On_Sorting(object sender, DataGridSortingEventArgs e) {
            try {
                // Does the column we're sorting define a custom sorter?
                var matches = columnMap.Where(pair => pair.Value.column == e.Column && pair.Value.customSorter != null).ToList();
                if (!matches.Any()) return;
                var customSorter = matches.First().Value.customSorter;

                e.Column.SortDirection = customSorter.SortDirection = e.Column.SortDirection != ListSortDirection.Ascending ? ListSortDirection.Ascending : ListSortDirection.Descending;

                var listColView = (ListCollectionView) ItemsSource;
                listColView.CustomSort = customSorter;

                e.Handled = true;
            } catch (Exception err) when (!Debugger.IsAttached) {
                MainWindow.ShowError(err, "Error Occured");
            }
        }

        protected override void On_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e) {
            try {
                // Commit as cell edit ends instead of DG waiting till we leave the row.
                if (!isManualEditCommit) {
                    isManualEditCommit = true;
                    CommitEdit(DataGridEditingUnit.Row, true);
                    isManualEditCommit = false;
                }

                CalculatePercents();
            } catch (Exception err) when (!Debugger.IsAttached) {
                MainWindow.ShowError(err, "Error Occured");
            }
        }

        private void CalculatePercents() {
            if (string.IsNullOrEmpty(mainWindow.targetFile)) return;

            if (mainWindow.targetFileType.Is(typeof(DecoLottery))) {
                var dict = new Dictionary<int, uint>();
                for (var i = 0; i <= 10; i++) {
                    dict[i] = 0;
                }

                foreach (DecoLottery item in ItemsSource) {
                    dict[0]  += item.Grade_1;
                    dict[1]  += item.Grade_2;
                    dict[2]  += item.Grade_3;
                    dict[3]  += item.Grade_4;
                    dict[4]  += item.Grade_5;
                    dict[5]  += item.Grade_6;
                    dict[6]  += item.Grade_7;
                    dict[7]  += item.Grade_8;
                    dict[8]  += item.Grade_9;
                    dict[9]  += item.Stream_R6_;
                    dict[10] += item.Stream_R8_;
                }

                foreach (DecoLottery item in ItemsSource) {
                    item.Grade_1_percent    = item.Grade_1 > 0f ? (float) item.Grade_1 / dict[0] : 0f;
                    item.Grade_2_percent    = item.Grade_2 > 0f ? (float) item.Grade_2 / dict[1] : 0f;
                    item.Grade_3_percent    = item.Grade_3 > 0f ? (float) item.Grade_3 / dict[2] : 0f;
                    item.Grade_4_percent    = item.Grade_4 > 0f ? (float) item.Grade_4 / dict[3] : 0f;
                    item.Grade_5_percent    = item.Grade_5 > 0f ? (float) item.Grade_5 / dict[4] : 0f;
                    item.Grade_6_percent    = item.Grade_6 > 0f ? (float) item.Grade_6 / dict[5] : 0f;
                    item.Grade_7_percent    = item.Grade_7 > 0f ? (float) item.Grade_7 / dict[6] : 0f;
                    item.Grade_8_percent    = item.Grade_8 > 0f ? (float) item.Grade_8 / dict[7] : 0f;
                    item.Grade_9_percent    = item.Grade_9 > 0f ? (float) item.Grade_9 / dict[8] : 0f;
                    item.Stream_R6__percent = item.Stream_R6_ > 0f ? (float) item.Stream_R6_ / dict[9] : 0f;
                    item.Stream_R8__percent = item.Stream_R8_ > 0f ? (float) item.Stream_R8_ / dict[10] : 0f;
                }
            } else if (mainWindow.targetFileType.Is(typeof(DecoGradeLottery))) {
                foreach (DecoGradeLottery item in ItemsSource) {
                    var total = item.Grade_1
                                + item.Grade_2
                                + item.Grade_3
                                + item.Grade_4
                                + item.Grade_5
                                + item.Grade_6
                                + item.Grade_7
                                + item.Grade_8
                                + item.Grade_9
                                + item.Stream_R6_
                                + item.Stream_R8_;

                    item.Grade_1_percent    = item.Grade_1 > 0f ? (float) item.Grade_1 / total : 0f;
                    item.Grade_2_percent    = item.Grade_2 > 0f ? (float) item.Grade_2 / total : 0f;
                    item.Grade_3_percent    = item.Grade_3 > 0f ? (float) item.Grade_3 / total : 0f;
                    item.Grade_4_percent    = item.Grade_4 > 0f ? (float) item.Grade_4 / total : 0f;
                    item.Grade_5_percent    = item.Grade_5 > 0f ? (float) item.Grade_5 / total : 0f;
                    item.Grade_6_percent    = item.Grade_6 > 0f ? (float) item.Grade_6 / total : 0f;
                    item.Grade_7_percent    = item.Grade_7 > 0f ? (float) item.Grade_7 / total : 0f;
                    item.Grade_8_percent    = item.Grade_8 > 0f ? (float) item.Grade_8 / total : 0f;
                    item.Grade_9_percent    = item.Grade_9 > 0f ? (float) item.Grade_9 / total : 0f;
                    item.Stream_R6__percent = item.Stream_R6_ > 0f ? (float) item.Stream_R6_ / total : 0f;
                    item.Stream_R8__percent = item.Stream_R8_ > 0f ? (float) item.Stream_R8_ / total : 0f;
                }
            } else if (mainWindow.targetFileType.Is(typeof(SafiItemGradeLottery))) {
                foreach (SafiItemGradeLottery item in ItemsSource) {
                    var total = item.Grade_1
                                + item.Grade_2
                                + item.Grade_3
                                + item.Grade_4
                                + item.Grade_5
                                + item.Grade_6
                                + item.Grade_7
                                + item.Grade_8
                                + item.Grade_9
                                + item.Grade_10
                                + item.Grade_11
                                + item.Grade_12
                                + item.Grade_13
                                + item.Grade_14
                                + item.Grade_15;

                    item.Grade_1_percent  = item.Grade_1 > 0f ? (float) item.Grade_1 / total : 0f;
                    item.Grade_2_percent  = item.Grade_2 > 0f ? (float) item.Grade_2 / total : 0f;
                    item.Grade_3_percent  = item.Grade_3 > 0f ? (float) item.Grade_3 / total : 0f;
                    item.Grade_4_percent  = item.Grade_4 > 0f ? (float) item.Grade_4 / total : 0f;
                    item.Grade_5_percent  = item.Grade_5 > 0f ? (float) item.Grade_5 / total : 0f;
                    item.Grade_6_percent  = item.Grade_6 > 0f ? (float) item.Grade_6 / total : 0f;
                    item.Grade_7_percent  = item.Grade_7 > 0f ? (float) item.Grade_7 / total : 0f;
                    item.Grade_8_percent  = item.Grade_8 > 0f ? (float) item.Grade_8 / total : 0f;
                    item.Grade_9_percent  = item.Grade_9 > 0f ? (float) item.Grade_9 / total : 0f;
                    item.Grade_10_percent = item.Grade_10 > 0f ? (float) item.Grade_10 / total : 0f;
                    item.Grade_11_percent = item.Grade_11 > 0f ? (float) item.Grade_11 / total : 0f;
                    item.Grade_12_percent = item.Grade_12 > 0f ? (float) item.Grade_12 / total : 0f;
                    item.Grade_13_percent = item.Grade_13 > 0f ? (float) item.Grade_13 / total : 0f;
                    item.Grade_14_percent = item.Grade_14 > 0f ? (float) item.Grade_14 / total : 0f;
                    item.Grade_15_percent = item.Grade_15 > 0f ? (float) item.Grade_15 / total : 0f;
                }
            } else if (mainWindow.targetFileType.Is(typeof(KulveGradeLottery))) {
                foreach (KulveGradeLottery item in ItemsSource) {
                    var total = item.Grade_1
                                + item.Grade_2
                                + item.Grade_3
                                + item.Grade_4
                                + item.Grade_5;

                    item.Grade_1_percent = item.Grade_1 > 0f ? (float) item.Grade_1 / total : 0f;
                    item.Grade_2_percent = item.Grade_2 > 0f ? (float) item.Grade_2 / total : 0f;
                    item.Grade_3_percent = item.Grade_3 > 0f ? (float) item.Grade_3 / total : 0f;
                    item.Grade_4_percent = item.Grade_4 > 0f ? (float) item.Grade_4 / total : 0f;
                    item.Grade_5_percent = item.Grade_5 > 0f ? (float) item.Grade_5 / total : 0f;
                }
            }
        }
    }
}