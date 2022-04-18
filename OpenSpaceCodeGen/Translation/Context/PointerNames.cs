using System;
using System.Collections.Generic;
using System.Text;
using OpenSpaceCodeGen.Util;

namespace OpenSpaceCodeGen.Translation.Context {
    public class PointerNames {


        public Dictionary<PointerInMap, string> Map { get; }

        public PointerNames()
        {
            Map = new Dictionary<PointerInMap, string>();
        }

        public string this[PointerInMap pointer]
        {
            get
            {
                if (Map.ContainsKey(pointer)) {
                    return Map[pointer];
                }

                return pointer.ToString();
            }
            set
            {
                if (!Map.ContainsKey(pointer)) {
                    Map.Add(pointer, value);
                } else {
                    Map[pointer] = value;
                }
            }
        }
    }
}
