using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using OpenSpaceCodeGen.AITypes;
using OpenSpaceCodeGen.Util;

namespace OpenSpaceCodeGen.AIModels {
    public readonly struct AIModelMetaData
    {

        public readonly uint Offset;
        public readonly string Map;
        public readonly string Name;
        public readonly string[] InitialRules;
        public readonly string[] InitialReflexes;
        public readonly List<DsgVar> DsgVars;
        public readonly Dictionary<PointerInMap, string> OffsetToBehaviourNameMap;
        public readonly Dictionary<PointerInMap, PersoNames> OffsetToPersoNamesMap;

        public AIModelMetaData(uint offset, string map, string name, string[] initialRules, string[] initialReflexes, List<DsgVar> dsgVars, Dictionary<PointerInMap, string> offsetToBehaviourNameMap, Dictionary<PointerInMap, PersoNames> offsetToPersoNamesMap)
        {
            this.Offset = offset;
            this.Map = map;
            this.Name = name;
            this.InitialRules = initialRules;
            this.InitialReflexes = initialReflexes;
            this.DsgVars = dsgVars;
            this.OffsetToBehaviourNameMap = offsetToBehaviourNameMap;
            this.OffsetToPersoNamesMap = offsetToPersoNamesMap;
        }

        public readonly struct DsgVar {
            public readonly DsgVarType Type;
            public readonly object ModelValue;

            public DsgVar(DsgVarType type, object modelValue)
            {
                this.Type = type;
                this.ModelValue = modelValue;
            }
        }

        public readonly struct PersoNames {
            public readonly string InstanceName;
            public readonly string ModelName;
            public readonly string FamilyName;

            public PersoNames(int offset)
            {
                string offsetStr = $"0x{offset:x}";
                InstanceName = $"Instance_{offsetStr}";
                ModelName = $"AIModel_{offsetStr}";
                FamilyName = $"Family_{offsetStr}";
            }

            [JsonConstructor]
            public PersoNames(string instanceName, string modelName, string familyName)
            {
                InstanceName = instanceName;
                ModelName = modelName;
                FamilyName = familyName;
            }
        }
    }
}
