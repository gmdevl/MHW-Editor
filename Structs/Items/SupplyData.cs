﻿using MHW_Editor.Models;

namespace MHW_Editor.Structs.Items {
    public partial class SupplyData : MhwMultiStructFile<SupplyData> {
        public partial class Item_Box_Scaling {
            public string Name {
                get {
                    return Index switch {
                        0 => "1 Player",
                        1 => "2 Players",
                        2 => "3/4 Players",
                        _ => "Unknown"
                    };
                }
            }
        }

        public partial class Ammo_Box_Scaling {
            public string Name {
                get {
                    return Index switch {
                        0 => "1 Player",
                        1 => "2 Players",
                        2 => "3/4 Players",
                        _ => "Unknown"
                    };
                }
            }
        }
    }
}