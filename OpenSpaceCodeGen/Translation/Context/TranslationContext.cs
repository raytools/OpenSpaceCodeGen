using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using OpenSpaceCodeGen.AIModels;
using OpenSpaceCodeGen.Translation.Context;
using OpenSpaceCodeGen.Util;

namespace OpenSpaceCodeGen.Translation {
    public class TranslationContext
    {
        public AIModel Model { get; }
        public PointerNames PointerNames { get; }
        public Dictionary<PointerInMap, AIModelMetaData.PersoNames> PersoModelNames { get; }
        
        public bool UseDsgVarNames = false;

        public TypeHints TypeHints;

        public TranslationContext()
        {
            PointerNames = new PointerNames();
            PersoModelNames = new Dictionary<PointerInMap, AIModelMetaData.PersoNames>();
            TypeHints = new TypeHints();
        }

        public void ReadPointerNames(string map, string stringsJson)
        {
            var json = File.ReadAllText(stringsJson);
            Dictionary<uint, string> strings = JsonConvert.DeserializeObject<Dictionary<uint, string>>(json);
            foreach (var kv in strings) {
                var ptr = new PointerInMap(map, kv.Key);
                if (!PointerNames.Map.ContainsKey(ptr)) {
                    PointerNames.Map.Add(ptr, $"{kv.Value}");
                }
            }
        }
    }
}
