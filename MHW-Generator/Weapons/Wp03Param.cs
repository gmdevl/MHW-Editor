﻿using System.Collections.Generic;
using MHW_Generator.Models;
using MHW_Template;
using MHW_Template.Struct_Generation.Multi;

namespace MHW_Generator.Weapons {
    public class Wp03Param : WpParamBase, IMultiStruct {
        private const string N = "03";

        public MultiStruct Generate() { // .w03p
            var structs = new List<MhwMultiStructData.StructData>();
            structs.AddRange(CreateBase(N));

            structs.AddRange(new List<MhwMultiStructData.StructData> {
                new MhwMultiStructData.StructData($"W{N}p Params ({++paramIndex})", new List<MhwMultiStructData.Entry> {
                    new MhwMultiStructData.Entry($"Unk {++unkIndex}", typeof(float)),
                    new MhwMultiStructData.Entry($"Unk {++unkIndex}", typeof(float)),
                    new MhwMultiStructData.Entry($"Unk {++unkIndex}", typeof(float)),
                    new MhwMultiStructData.Entry($"Unk {++unkIndex}", typeof(float)),
                    new MhwMultiStructData.Entry($"Unk {++unkIndex}", typeof(float)),
                    new MhwMultiStructData.Entry($"Unk {++unkIndex}", typeof(float)),
                    new MhwMultiStructData.Entry($"Unk {++unkIndex}", typeof(float)),
                    new MhwMultiStructData.Entry($"Unk {++unkIndex}", typeof(float)),
                    new MhwMultiStructData.Entry($"Unk {++unkIndex}", typeof(float)),
                    new MhwMultiStructData.Entry($"Unk {++unkIndex}", typeof(float)),
                    new MhwMultiStructData.Entry($"Unk {++unkIndex}", typeof(float)),
                    new MhwMultiStructData.Entry($"Unk {++unkIndex}", typeof(float)),
                    new MhwMultiStructData.Entry($"Unk {++unkIndex}", typeof(float)),
                    new MhwMultiStructData.Entry($"Unk {++unkIndex}", typeof(float)),
                    new MhwMultiStructData.Entry($"Unk {++unkIndex}", typeof(float)),
                    new MhwMultiStructData.Entry($"Unk {++unkIndex}", typeof(float)),
                    new MhwMultiStructData.Entry($"Unk {++unkIndex}", typeof(float)),
                    new MhwMultiStructData.Entry($"Unk {++unkIndex}", typeof(float)),
                    new MhwMultiStructData.Entry($"Unk {++unkIndex}", typeof(float)),
                    new MhwMultiStructData.Entry($"Unk {++unkIndex}", typeof(float)),
                    new MhwMultiStructData.Entry($"Unk {++unkIndex}", typeof(float)),
                    new MhwMultiStructData.Entry($"Unk {++unkIndex}", typeof(float)),
                    new MhwMultiStructData.Entry($"Unk {++unkIndex}", typeof(float)),
                    new MhwMultiStructData.Entry($"Unk {++unkIndex}", typeof(float)),
                    new MhwMultiStructData.Entry($"Unk {++unkIndex}", typeof(float)),
                    new MhwMultiStructData.Entry($"Unk {++unkIndex}", typeof(uint)),
                    new MhwMultiStructData.Entry($"Unk {++unkIndex}", typeof(float)),
                    new MhwMultiStructData.Entry($"Unk {++unkIndex}", typeof(float)),
                    new MhwMultiStructData.Entry($"Unk {++unkIndex}", typeof(float)),
                    new MhwMultiStructData.Entry($"Unk {++unkIndex}", typeof(float)),
                    new MhwMultiStructData.Entry($"Unk {++unkIndex}", typeof(float)),
                    new MhwMultiStructData.Entry($"Unk {++unkIndex}", typeof(float)),
                    new MhwMultiStructData.Entry($"Unk {++unkIndex}", typeof(float)),
                    new MhwMultiStructData.Entry($"Unk {++unkIndex}", typeof(float)),
                    new MhwMultiStructData.Entry($"Unk {++unkIndex}", typeof(float)),
                    new MhwMultiStructData.Entry($"Unk {++unkIndex}", typeof(float)),
                    new MhwMultiStructData.Entry($"Unk {++unkIndex}", typeof(byte)),
                    new MhwMultiStructData.Entry($"Unk {++unkIndex}", typeof(uint)),
                    new MhwMultiStructData.Entry($"Unk {++unkIndex}", typeof(float)),
                    new MhwMultiStructData.Entry($"Unk {++unkIndex}", typeof(ushort)),
                    new MhwMultiStructData.Entry($"Unk {++unkIndex}", typeof(float)),
                    new MhwMultiStructData.Entry($"Unk {++unkIndex}", typeof(float)),
                    new MhwMultiStructData.Entry($"Unk {++unkIndex}", typeof(float)),
                    new MhwMultiStructData.Entry($"Unk {++unkIndex}", typeof(float)),
                    new MhwMultiStructData.Entry($"Unk {++unkIndex}", typeof(float)),
                    new MhwMultiStructData.Entry($"Unk {++unkIndex}", typeof(float)),
                    new MhwMultiStructData.Entry($"Unk {++unkIndex}", typeof(float))
                }, 1, showVertically: true),

                new MhwMultiStructData.StructData($"Action Param {++actionParamIndex}", actionParam, 1),

                new MhwMultiStructData.StructData($"W{N}p Params ({++paramIndex})", new List<MhwMultiStructData.Entry> {
                    new MhwMultiStructData.Entry($"Unk {++unkIndex}", typeof(float)),
                    new MhwMultiStructData.Entry($"Unk {++unkIndex}", typeof(float))
                }, 1, showVertically: true),

                new MhwMultiStructData.StructData($"Action Param {++actionParamIndex}", actionParam, 1),

                new MhwMultiStructData.StructData($"W{N}p Params ({++paramIndex})", new List<MhwMultiStructData.Entry> {
                    new MhwMultiStructData.Entry($"Unk {++unkIndex}", typeof(float))
                }, 1, showVertically: true),

                new MhwMultiStructData.StructData($"Action Param {++actionParamIndex}", actionParam, 3),

                new MhwMultiStructData.StructData($"W{N}p Params ({++paramIndex})", new List<MhwMultiStructData.Entry> {
                    new MhwMultiStructData.Entry($"Unk {++unkIndex}", typeof(float))
                }, 1, showVertically: true),

                new MhwMultiStructData.StructData($"Action Param {++actionParamIndex}", actionParam, 2),

                new MhwMultiStructData.StructData($"W{N}p Params ({++paramIndex})", new List<MhwMultiStructData.Entry> {
                    new MhwMultiStructData.Entry($"Unk {++unkIndex}", typeof(float)),
                    new MhwMultiStructData.Entry($"Unk {++unkIndex}", typeof(float)),
                    new MhwMultiStructData.Entry($"Unk {++unkIndex}", typeof(float)),
                    new MhwMultiStructData.Entry($"Unk {++unkIndex}", typeof(float)),
                    new MhwMultiStructData.Entry($"Unk {++unkIndex}", typeof(float)),
                    new MhwMultiStructData.Entry($"Unk {++unkIndex}", typeof(float)),
                    new MhwMultiStructData.Entry($"Unk {++unkIndex}", typeof(uint)),
                    new MhwMultiStructData.Entry($"Unk {++unkIndex}", typeof(float)),
                    new MhwMultiStructData.Entry($"Unk {++unkIndex}", typeof(float))
                }, 1, showVertically: true),

                new MhwMultiStructData.StructData($"Action Param {++actionParamIndex}", actionParam, 1),

                new MhwMultiStructData.StructData($"W{N}p Params ({++paramIndex})", new List<MhwMultiStructData.Entry> {
                    new MhwMultiStructData.Entry($"Unk {++unkIndex}", typeof(float))
                }, 1, showVertically: true)
            });

            return new MultiStruct("MHW_Editor.Weapons", $"Wp{N}Param", new MhwMultiStructData(structs, $"w{N}p", EncryptionKeys.FILE_EXT_KEY_LOOKUP[$".w{N}p"]));
        }
    }
}