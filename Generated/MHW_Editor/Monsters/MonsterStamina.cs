using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Reflection;
using MHW_Editor.Assets;
using MHW_Editor.Models;
using MHW_Template;
using MHW_Template.Models;

namespace MHW_Editor.Monsters {
    public partial class MonsterStamina : MhwMultiStructItem {
        public const ulong InitialOffset = 0;
        public const string EncryptionKey = null;

        public partial class Monster_Difficulty : MhwStructItem {
            public const ulong FixedSizeCount = 1;
            public const string DisplayName = "Monster Difficulty";

            protected uint Magic_1_raw;
            public const string Magic_1_displayName = "Magic 1";
            public const int Magic_1_sortIndex = 50;
            [SortOrder(Magic_1_sortIndex)]
            [DisplayName(Magic_1_displayName)]
            [IsReadOnly]
            public virtual uint Magic_1 {
                get => Magic_1_raw;
                set {
                    if (Magic_1_raw == value) return;
                    Magic_1_raw = value;
                    OnPropertyChanged(nameof(Magic_1));
                }
            }

            protected uint Magic_2_raw;
            public const string Magic_2_displayName = "Magic 2";
            public const int Magic_2_sortIndex = 100;
            [SortOrder(Magic_2_sortIndex)]
            [DisplayName(Magic_2_displayName)]
            [IsReadOnly]
            public virtual uint Magic_2 {
                get => Magic_2_raw;
                set {
                    if (Magic_2_raw == value) return;
                    Magic_2_raw = value;
                    OnPropertyChanged(nameof(Magic_2));
                }
            }

            protected uint Monster_Id_raw;
            public const string Monster_Id_displayName = "Monster Id";
            public const int Monster_Id_sortIndex = 150;
            [SortOrder(Monster_Id_sortIndex)]
            [DisplayName(Monster_Id_displayName)]
            [IsReadOnly]
            public virtual uint Monster_Id {
                get => Monster_Id_raw;
                set {
                    if (Monster_Id_raw == value) return;
                    Monster_Id_raw = value;
                    OnPropertyChanged(nameof(Monster_Id));
                }
            }

            protected uint Magic_3_raw;
            public const string Magic_3_displayName = "Magic 3";
            public const int Magic_3_sortIndex = 200;
            [SortOrder(Magic_3_sortIndex)]
            [DisplayName(Magic_3_displayName)]
            [IsReadOnly]
            public virtual uint Magic_3 {
                get => Magic_3_raw;
                set {
                    if (Magic_3_raw == value) return;
                    Magic_3_raw = value;
                    OnPropertyChanged(nameof(Magic_3));
                }
            }

            public static ulong GetEntryCount(List<MhwStructWrapper> data) {
                return FixedSizeCount;
            }

            public static Monster_Difficulty LoadData(BinaryReader reader) {
                var data = new Monster_Difficulty();
                data.Magic_1_raw = reader.ReadUInt32();
                data.Magic_2_raw = reader.ReadUInt32();
                data.Monster_Id_raw = reader.ReadUInt32();
                data.Magic_3_raw = reader.ReadUInt32();
                return data;
            }

            public override void WriteData(BinaryWriter writer) {
                writer.Write(Magic_1_raw);
                writer.Write(Magic_2_raw);
                writer.Write(Monster_Id_raw);
                writer.Write(Magic_3_raw);
            }
        }

        public partial class Fatigue_LR_ : MhwStructItem {
            public const ulong FixedSizeCount = 1;
            public const string DisplayName = "Fatigue (LR)";

            protected float Duration_raw;
            public const string Duration_displayName = "Duration";
            public const int Duration_sortIndex = 50;
            [SortOrder(Duration_sortIndex)]
            [DisplayName(Duration_displayName)]
            public virtual float Duration {
                get => Duration_raw;
                set {
                    if (Duration_raw == value) return;
                    Duration_raw = value;
                    OnPropertyChanged(nameof(Duration));
                }
            }

            protected uint Stamina_Min_raw;
            public const string Stamina_Min_displayName = "Stamina Min";
            public const int Stamina_Min_sortIndex = 100;
            [SortOrder(Stamina_Min_sortIndex)]
            [DisplayName(Stamina_Min_displayName)]
            public virtual uint Stamina_Min {
                get => Stamina_Min_raw;
                set {
                    if (Stamina_Min_raw == value) return;
                    Stamina_Min_raw = value;
                    OnPropertyChanged(nameof(Stamina_Min));
                }
            }

            protected uint Stamina_Max_raw;
            public const string Stamina_Max_displayName = "Stamina Max";
            public const int Stamina_Max_sortIndex = 150;
            [SortOrder(Stamina_Max_sortIndex)]
            [DisplayName(Stamina_Max_displayName)]
            public virtual uint Stamina_Max {
                get => Stamina_Max_raw;
                set {
                    if (Stamina_Max_raw == value) return;
                    Stamina_Max_raw = value;
                    OnPropertyChanged(nameof(Stamina_Max));
                }
            }

            public static ulong GetEntryCount(List<MhwStructWrapper> data) {
                return FixedSizeCount;
            }

            public static Fatigue_LR_ LoadData(BinaryReader reader) {
                var data = new Fatigue_LR_();
                data.Duration_raw = reader.ReadSingle();
                data.Stamina_Min_raw = reader.ReadUInt32();
                data.Stamina_Max_raw = reader.ReadUInt32();
                return data;
            }

            public override void WriteData(BinaryWriter writer) {
                writer.Write(Duration_raw);
                writer.Write(Stamina_Min_raw);
                writer.Write(Stamina_Max_raw);
            }
        }

        public partial class Stamina_Count_LR_ : MhwStructItem {
            public const ulong FixedSizeCount = 1;
            public const string DisplayName = "Stamina Count (LR)";
            public const bool Hidden = true;

            protected uint Number_of_Stamina_Entries_raw;
            public const string Number_of_Stamina_Entries_displayName = "Number of Stamina Entries";
            public const int Number_of_Stamina_Entries_sortIndex = 50;
            [SortOrder(Number_of_Stamina_Entries_sortIndex)]
            [DisplayName(Number_of_Stamina_Entries_displayName)]
            [IsReadOnly]
            public virtual uint Number_of_Stamina_Entries {
                get => Number_of_Stamina_Entries_raw;
                set {
                    if (Number_of_Stamina_Entries_raw == value) return;
                    Number_of_Stamina_Entries_raw = value;
                    OnPropertyChanged(nameof(Number_of_Stamina_Entries));
                }
            }

            public static ulong GetEntryCount(List<MhwStructWrapper> data) {
                return FixedSizeCount;
            }

            public static Stamina_Count_LR_ LoadData(BinaryReader reader) {
                var data = new Stamina_Count_LR_();
                data.Number_of_Stamina_Entries_raw = reader.ReadUInt32();
                return data;
            }

            public override void WriteData(BinaryWriter writer) {
                writer.Write(Number_of_Stamina_Entries_raw);
            }
        }

        public partial class Stamina_LR_ : MhwStructItem {
            public const ulong FixedSizeCount = 0;
            public const string DisplayName = "Stamina (LR)";

            protected uint Unk_raw;
            public const string Unk_displayName = "Unk";
            public const int Unk_sortIndex = 50;
            [SortOrder(Unk_sortIndex)]
            [DisplayName(Unk_displayName)]
            public virtual uint Unk {
                get => Unk_raw;
                set {
                    if (Unk_raw == value) return;
                    Unk_raw = value;
                    OnPropertyChanged(nameof(Unk));
                }
            }

            protected uint Base_raw;
            public const string Base_displayName = "Base";
            public const int Base_sortIndex = 100;
            [SortOrder(Base_sortIndex)]
            [DisplayName(Base_displayName)]
            public virtual uint Base {
                get => Base_raw;
                set {
                    if (Base_raw == value) return;
                    Base_raw = value;
                    OnPropertyChanged(nameof(Base));
                }
            }

            public static Stamina_LR_ LoadData(BinaryReader reader) {
                var data = new Stamina_LR_();
                data.Unk_raw = reader.ReadUInt32();
                data.Base_raw = reader.ReadUInt32();
                return data;
            }

            public override void WriteData(BinaryWriter writer) {
                writer.Write(Unk_raw);
                writer.Write(Base_raw);
            }
        }

        public partial class Fatigue_HR_ : MhwStructItem {
            public const ulong FixedSizeCount = 1;
            public const string DisplayName = "Fatigue (HR)";

            protected float Duration_raw;
            public const string Duration_displayName = "Duration";
            public const int Duration_sortIndex = 50;
            [SortOrder(Duration_sortIndex)]
            [DisplayName(Duration_displayName)]
            public virtual float Duration {
                get => Duration_raw;
                set {
                    if (Duration_raw == value) return;
                    Duration_raw = value;
                    OnPropertyChanged(nameof(Duration));
                }
            }

            protected uint Stamina_Min_raw;
            public const string Stamina_Min_displayName = "Stamina Min";
            public const int Stamina_Min_sortIndex = 100;
            [SortOrder(Stamina_Min_sortIndex)]
            [DisplayName(Stamina_Min_displayName)]
            public virtual uint Stamina_Min {
                get => Stamina_Min_raw;
                set {
                    if (Stamina_Min_raw == value) return;
                    Stamina_Min_raw = value;
                    OnPropertyChanged(nameof(Stamina_Min));
                }
            }

            protected uint Stamina_Max_raw;
            public const string Stamina_Max_displayName = "Stamina Max";
            public const int Stamina_Max_sortIndex = 150;
            [SortOrder(Stamina_Max_sortIndex)]
            [DisplayName(Stamina_Max_displayName)]
            public virtual uint Stamina_Max {
                get => Stamina_Max_raw;
                set {
                    if (Stamina_Max_raw == value) return;
                    Stamina_Max_raw = value;
                    OnPropertyChanged(nameof(Stamina_Max));
                }
            }

            public static ulong GetEntryCount(List<MhwStructWrapper> data) {
                return FixedSizeCount;
            }

            public static Fatigue_HR_ LoadData(BinaryReader reader) {
                var data = new Fatigue_HR_();
                data.Duration_raw = reader.ReadSingle();
                data.Stamina_Min_raw = reader.ReadUInt32();
                data.Stamina_Max_raw = reader.ReadUInt32();
                return data;
            }

            public override void WriteData(BinaryWriter writer) {
                writer.Write(Duration_raw);
                writer.Write(Stamina_Min_raw);
                writer.Write(Stamina_Max_raw);
            }
        }

        public partial class Stamina_Count_HR_ : MhwStructItem {
            public const ulong FixedSizeCount = 1;
            public const string DisplayName = "Stamina Count (HR)";
            public const bool Hidden = true;

            protected uint Number_of_Stamina_Entries_raw;
            public const string Number_of_Stamina_Entries_displayName = "Number of Stamina Entries";
            public const int Number_of_Stamina_Entries_sortIndex = 50;
            [SortOrder(Number_of_Stamina_Entries_sortIndex)]
            [DisplayName(Number_of_Stamina_Entries_displayName)]
            [IsReadOnly]
            public virtual uint Number_of_Stamina_Entries {
                get => Number_of_Stamina_Entries_raw;
                set {
                    if (Number_of_Stamina_Entries_raw == value) return;
                    Number_of_Stamina_Entries_raw = value;
                    OnPropertyChanged(nameof(Number_of_Stamina_Entries));
                }
            }

            public static ulong GetEntryCount(List<MhwStructWrapper> data) {
                return FixedSizeCount;
            }

            public static Stamina_Count_HR_ LoadData(BinaryReader reader) {
                var data = new Stamina_Count_HR_();
                data.Number_of_Stamina_Entries_raw = reader.ReadUInt32();
                return data;
            }

            public override void WriteData(BinaryWriter writer) {
                writer.Write(Number_of_Stamina_Entries_raw);
            }
        }

        public partial class Stamina_HR_ : MhwStructItem {
            public const ulong FixedSizeCount = 0;
            public const string DisplayName = "Stamina (HR)";

            protected uint Unk_raw;
            public const string Unk_displayName = "Unk";
            public const int Unk_sortIndex = 50;
            [SortOrder(Unk_sortIndex)]
            [DisplayName(Unk_displayName)]
            public virtual uint Unk {
                get => Unk_raw;
                set {
                    if (Unk_raw == value) return;
                    Unk_raw = value;
                    OnPropertyChanged(nameof(Unk));
                }
            }

            protected uint Base_raw;
            public const string Base_displayName = "Base";
            public const int Base_sortIndex = 100;
            [SortOrder(Base_sortIndex)]
            [DisplayName(Base_displayName)]
            public virtual uint Base {
                get => Base_raw;
                set {
                    if (Base_raw == value) return;
                    Base_raw = value;
                    OnPropertyChanged(nameof(Base));
                }
            }

            public static Stamina_HR_ LoadData(BinaryReader reader) {
                var data = new Stamina_HR_();
                data.Unk_raw = reader.ReadUInt32();
                data.Base_raw = reader.ReadUInt32();
                return data;
            }

            public override void WriteData(BinaryWriter writer) {
                writer.Write(Unk_raw);
                writer.Write(Base_raw);
            }
        }

        public partial class Fatigue_MR_ : MhwStructItem {
            public const ulong FixedSizeCount = 1;
            public const string DisplayName = "Fatigue (MR)";

            protected float Duration_raw;
            public const string Duration_displayName = "Duration";
            public const int Duration_sortIndex = 50;
            [SortOrder(Duration_sortIndex)]
            [DisplayName(Duration_displayName)]
            public virtual float Duration {
                get => Duration_raw;
                set {
                    if (Duration_raw == value) return;
                    Duration_raw = value;
                    OnPropertyChanged(nameof(Duration));
                }
            }

            protected uint Stamina_Min_raw;
            public const string Stamina_Min_displayName = "Stamina Min";
            public const int Stamina_Min_sortIndex = 100;
            [SortOrder(Stamina_Min_sortIndex)]
            [DisplayName(Stamina_Min_displayName)]
            public virtual uint Stamina_Min {
                get => Stamina_Min_raw;
                set {
                    if (Stamina_Min_raw == value) return;
                    Stamina_Min_raw = value;
                    OnPropertyChanged(nameof(Stamina_Min));
                }
            }

            protected uint Stamina_Max_raw;
            public const string Stamina_Max_displayName = "Stamina Max";
            public const int Stamina_Max_sortIndex = 150;
            [SortOrder(Stamina_Max_sortIndex)]
            [DisplayName(Stamina_Max_displayName)]
            public virtual uint Stamina_Max {
                get => Stamina_Max_raw;
                set {
                    if (Stamina_Max_raw == value) return;
                    Stamina_Max_raw = value;
                    OnPropertyChanged(nameof(Stamina_Max));
                }
            }

            public static ulong GetEntryCount(List<MhwStructWrapper> data) {
                return FixedSizeCount;
            }

            public static Fatigue_MR_ LoadData(BinaryReader reader) {
                var data = new Fatigue_MR_();
                data.Duration_raw = reader.ReadSingle();
                data.Stamina_Min_raw = reader.ReadUInt32();
                data.Stamina_Max_raw = reader.ReadUInt32();
                return data;
            }

            public override void WriteData(BinaryWriter writer) {
                writer.Write(Duration_raw);
                writer.Write(Stamina_Min_raw);
                writer.Write(Stamina_Max_raw);
            }
        }

        public partial class Stamina_Count_MR_ : MhwStructItem {
            public const ulong FixedSizeCount = 1;
            public const string DisplayName = "Stamina Count (MR)";
            public const bool Hidden = true;

            protected uint Number_of_Stamina_Entries_raw;
            public const string Number_of_Stamina_Entries_displayName = "Number of Stamina Entries";
            public const int Number_of_Stamina_Entries_sortIndex = 50;
            [SortOrder(Number_of_Stamina_Entries_sortIndex)]
            [DisplayName(Number_of_Stamina_Entries_displayName)]
            [IsReadOnly]
            public virtual uint Number_of_Stamina_Entries {
                get => Number_of_Stamina_Entries_raw;
                set {
                    if (Number_of_Stamina_Entries_raw == value) return;
                    Number_of_Stamina_Entries_raw = value;
                    OnPropertyChanged(nameof(Number_of_Stamina_Entries));
                }
            }

            public static ulong GetEntryCount(List<MhwStructWrapper> data) {
                return FixedSizeCount;
            }

            public static Stamina_Count_MR_ LoadData(BinaryReader reader) {
                var data = new Stamina_Count_MR_();
                data.Number_of_Stamina_Entries_raw = reader.ReadUInt32();
                return data;
            }

            public override void WriteData(BinaryWriter writer) {
                writer.Write(Number_of_Stamina_Entries_raw);
            }
        }

        public partial class Stamina_MR_ : MhwStructItem {
            public const ulong FixedSizeCount = 0;
            public const string DisplayName = "Stamina (MR)";

            protected uint Unk_raw;
            public const string Unk_displayName = "Unk";
            public const int Unk_sortIndex = 50;
            [SortOrder(Unk_sortIndex)]
            [DisplayName(Unk_displayName)]
            public virtual uint Unk {
                get => Unk_raw;
                set {
                    if (Unk_raw == value) return;
                    Unk_raw = value;
                    OnPropertyChanged(nameof(Unk));
                }
            }

            protected uint Base_raw;
            public const string Base_displayName = "Base";
            public const int Base_sortIndex = 100;
            [SortOrder(Base_sortIndex)]
            [DisplayName(Base_displayName)]
            public virtual uint Base {
                get => Base_raw;
                set {
                    if (Base_raw == value) return;
                    Base_raw = value;
                    OnPropertyChanged(nameof(Base));
                }
            }

            public static Stamina_MR_ LoadData(BinaryReader reader) {
                var data = new Stamina_MR_();
                data.Unk_raw = reader.ReadUInt32();
                data.Base_raw = reader.ReadUInt32();
                return data;
            }

            public override void WriteData(BinaryWriter writer) {
                writer.Write(Unk_raw);
                writer.Write(Base_raw);
            }
        }

        public static void SaveData(List<MhwStructWrapper> data, string targetFile) {
            SaveData(data, targetFile, EncryptionKey);
        }

        public static List<MhwStructWrapper> LoadData(string targetFile) {
            using var reader = new BinaryReader(OpenFile(targetFile, EncryptionKey));
            var data = new List<MhwStructWrapper>();
            var Monster_Difficulty_list = new List<object>();
            for (ulong i = 0; i < Monster_Difficulty.GetEntryCount(data); i++) {
                var item = Monster_Difficulty.LoadData(reader);
                item.index = i;
                Monster_Difficulty_list.Add(item);
            }
            data.Add(new MhwStructWrapper(Monster_Difficulty_list, typeof(Monster_Difficulty)));
            var Fatigue_LR__list = new List<object>();
            for (ulong i = 0; i < Fatigue_LR_.GetEntryCount(data); i++) {
                var item = Fatigue_LR_.LoadData(reader);
                item.index = i;
                Fatigue_LR__list.Add(item);
            }
            data.Add(new MhwStructWrapper(Fatigue_LR__list, typeof(Fatigue_LR_)));
            var Stamina_Count_LR__list = new List<object>();
            for (ulong i = 0; i < Stamina_Count_LR_.GetEntryCount(data); i++) {
                var item = Stamina_Count_LR_.LoadData(reader);
                item.index = i;
                Stamina_Count_LR__list.Add(item);
            }
            data.Add(new MhwStructWrapper(Stamina_Count_LR__list, typeof(Stamina_Count_LR_)));
            var Stamina_LR__list = new List<object>();
            for (ulong i = 0; i < Stamina_LR_.GetEntryCount(data); i++) {
                var item = Stamina_LR_.LoadData(reader);
                item.index = i;
                Stamina_LR__list.Add(item);
            }
            data.Add(new MhwStructWrapper(Stamina_LR__list, typeof(Stamina_LR_)));
            var Fatigue_HR__list = new List<object>();
            for (ulong i = 0; i < Fatigue_HR_.GetEntryCount(data); i++) {
                var item = Fatigue_HR_.LoadData(reader);
                item.index = i;
                Fatigue_HR__list.Add(item);
            }
            data.Add(new MhwStructWrapper(Fatigue_HR__list, typeof(Fatigue_HR_)));
            var Stamina_Count_HR__list = new List<object>();
            for (ulong i = 0; i < Stamina_Count_HR_.GetEntryCount(data); i++) {
                var item = Stamina_Count_HR_.LoadData(reader);
                item.index = i;
                Stamina_Count_HR__list.Add(item);
            }
            data.Add(new MhwStructWrapper(Stamina_Count_HR__list, typeof(Stamina_Count_HR_)));
            var Stamina_HR__list = new List<object>();
            for (ulong i = 0; i < Stamina_HR_.GetEntryCount(data); i++) {
                var item = Stamina_HR_.LoadData(reader);
                item.index = i;
                Stamina_HR__list.Add(item);
            }
            data.Add(new MhwStructWrapper(Stamina_HR__list, typeof(Stamina_HR_)));
            var Fatigue_MR__list = new List<object>();
            for (ulong i = 0; i < Fatigue_MR_.GetEntryCount(data); i++) {
                var item = Fatigue_MR_.LoadData(reader);
                item.index = i;
                Fatigue_MR__list.Add(item);
            }
            data.Add(new MhwStructWrapper(Fatigue_MR__list, typeof(Fatigue_MR_)));
            var Stamina_Count_MR__list = new List<object>();
            for (ulong i = 0; i < Stamina_Count_MR_.GetEntryCount(data); i++) {
                var item = Stamina_Count_MR_.LoadData(reader);
                item.index = i;
                Stamina_Count_MR__list.Add(item);
            }
            data.Add(new MhwStructWrapper(Stamina_Count_MR__list, typeof(Stamina_Count_MR_)));
            var Stamina_MR__list = new List<object>();
            for (ulong i = 0; i < Stamina_MR_.GetEntryCount(data); i++) {
                var item = Stamina_MR_.LoadData(reader);
                item.index = i;
                Stamina_MR__list.Add(item);
            }
            data.Add(new MhwStructWrapper(Stamina_MR__list, typeof(Stamina_MR_)));
            return data;
        }
    }
}