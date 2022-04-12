using System;
using System.Collections.Generic;
using System.Diagnostics;
using OpenSpaceCodeGen.AITypes;
using OpenSpaceCodeGen.Nodes;
using OpenSpaceCodeGen.Nodes.Generic;

namespace OpenSpaceCodeGen.Translation.Context
{
    public class TypeHints
    {
        public class TypeHint
        {
            public DsgVarType ReturnType;
            public List<DsgVarType> ParameterTypes;

            public TypeHint(DsgVarType returnType, List<DsgVarType> parameterTypes)
            {
                ReturnType = returnType;
                ParameterTypes = parameterTypes;
            }

            public void InitParameters()
            {
                if (ParameterTypes == null) {
                    ParameterTypes = new List<DsgVarType>();
                }
            }

            public void SetParameterType(int index, DsgVarType type)
            {
                InitParameters();

                int paramTypeCount = ParameterTypes.Count;

                for (int i = 0; i <= (index - paramTypeCount); i++)
                    ParameterTypes.Add(DsgVarType.None);

                ParameterTypes[index] = type;
            }
        }

        public class TypeHintMap
        {
            public Dictionary<string, TypeHint> Hints = new Dictionary<string, TypeHint>();

            public TypeHint this[string name]
            {
                get
                {
                    if (!Hints.ContainsKey(name)) {
                        Hints[name] = new TypeHint(default, null);
                    }
                    return Hints[name];
                }
                set => Hints[name] = value;
            }

            /*public void Update(string name, Action<TypeHint> modifyHint)
            {
                var h = this[name];
                modifyHint.Invoke(h);
                this[name] = h;
            }*/
            public void UpdateParams(string func, List<Node> parameters, CodeGenerator gen)
            {
                int i = 0;

                this[func].InitParameters();

                foreach (var p in parameters) {

                    DsgVarType type = default;

                    switch (p) {
                        case NodeActionRef nodeActionRef:			type = DsgVarType.Action; break;
                        case NodeBeginMacro nodeBeginMacro: break;
                        case NodeButton nodeButton:					type = DsgVarType.Input; break;
                        case NodeCaps nodeCaps: 					type = DsgVarType.Caps; break;
                        case NodeColor nodeColor: break;
                        case NodeComportRef nodeComportRef: 		type = DsgVarType.Comport; break;
                        case NodeCondition nodeCondition:

                            type = gen.Context.TypeHints.ConditionTypeHints[nodeCondition.ToString(gen)].ReturnType;
                            break;
                        case NodeConstant nodeConstant: 			type = DsgVarType.Int; break;
                        case NodeConstantRef nodeConstantRef: 		type = DsgVarType.Int; break;
                        case NodeConstantVector nodeConstantVector: type = DsgVarType.Vector; break;
                        case NodeCustomBits nodeCustomBits: 		type = DsgVarType.Caps; break;
                        case NodeDsgVar nodeDsgVar: break;
                        case NodeDsgVarId nodeDsgVarId: break;
                        case NodeDsgVarRef nodeDsgVarRef:

                            // Make sure the dsgvar belongs to the AI Model itself, so no ultra operators or shit like that
                            if (!(nodeDsgVarRef.Parent is NodeDsgVarRef || nodeDsgVarRef.Parent is NodeOperator)) {
                                type = gen.AIModel.MetaData.DsgVars[nodeDsgVarRef.param].Type;
                            }
                            break;
                        case NodeEndMacro nodeEndMacro: break;
                        case NodeEnvironmentRef nodeEnvironmentRef: break;
                        case NodeFamilyRef nodeFamilyRef: break;
                        case NodeField nodeField:
                            type = gen.Context.TypeHints.FieldTypeHints[nodeField.ToString(gen)].ReturnType; break;
                        case NodeFontRef nodeFontRef: break;
                        case NodeFunction nodeFunction:				
                            type = gen.Context.TypeHints.FunctionTypeHints[nodeFunction.ToString(gen)].ReturnType; break;
                        case NodeGameMaterialRef nodeGameMaterialRef: break;
                        case NodeGraphRef nodeGraphRef: break;
                        case NodeKeyWord nodeKeyWord:
                            var kw = nodeKeyWord.GetKeyword(gen);
                            switch (kw) {
                                case EnumKeyword.Me:            type = DsgVarType.Perso; break;
                                case EnumKeyword.MainActor:     type = DsgVarType.Perso; break;
                                case EnumKeyword.Nobody:        type = DsgVarType.Perso; break;
                                case EnumKeyword.NoInput:       type = DsgVarType.Input; break;
                                case EnumKeyword.NoSoundEvent:  type = DsgVarType.SoundEvent; break;
                                case EnumKeyword.NoSuperObject: type = DsgVarType.SuperObject; break;
                                case EnumKeyword.Nowhere:       type = DsgVarType.WayPoint; break;
                                case EnumKeyword.NoSOLinksWord: type = DsgVarType.SOLinks; break;
                                case EnumKeyword.EmptyText:     type = DsgVarType.Text; break;
                                case EnumKeyword.CapsNull:      type = DsgVarType.Caps; break;
                                case EnumKeyword.NoGraph:       type = DsgVarType.Graph; break;
                                case EnumKeyword.NoAction:      type = DsgVarType.Action; break;
                                case EnumKeyword.NoGMT:         type = DsgVarType.GameMaterial; break;
                                case EnumKeyword.NoVMT:         type = DsgVarType.VisualMaterial; break;
                                case EnumKeyword.Noway:         type = DsgVarType.Way; break;
                            }

                            break;
                        case NodeLightInfoRef nodeLightInfoRef: break;
                        case NodeLipsSynchroRef nodeLipsSynchroRef: break;
                        case NodeMask nodeMask: break;
                        case NodeMetaAction nodeMetaAction:
                            //type = gen.Context.TypeHints.met[nodeMetaAction.ToString(gen)].ReturnType; break;
                        case NodeModelRef nodeModelRef: break;
                        case NodeModule nodeModule: break;
                        case NodeModuleRef nodeModuleRef: 			type = DsgVarType.Int; break;
                        case NodeNull nodeNull: break;
                        case NodeObjectTableRef nodeObjectTableRef: type = DsgVarType.ObjectList; break;
                        case NodeOperator nodeOperator: break;
                        case NodeParticleGenerator nodeParticleGenerator: break;
                        case NodePersoRef nodePersoRef: 			type = DsgVarType.Perso; break;
                        case NodeProcedure nodeProcedure:
                            type = gen.Context.TypeHints.ProcedureTypeHints[nodeProcedure.ToString(gen)].ReturnType;
							break;
                        case NodeReal nodeReal: 					type = DsgVarType.Float; break;
                        case NodeRealRef nodeRealRef: 				type = DsgVarType.Float; break;
                        case NodeSectorRef nodeSectorRef: break;
                        case NodeSoundEventRef nodeSoundEventRef: break;
                        case NodeString nodeString: 				type = DsgVarType.Text; break;
                        case NodeSubRoutine nodeSubRoutine: break;
                        case NodeSuperObjectRef nodeSuperObjectRef: type = DsgVarType.SuperObject; break;
                        case NodeSurfaceRef nodeSurfaceRef: break;
                        case NodeTextRef nodeTextRef: 				type = DsgVarType.Text; break;
                        case NodeType42 nodeType42: break;
                        case NodeUnknown nodeUnknown: break;
                        case NodeVector nodeVector: 				type = DsgVarType.Vector; break;
                        case NodeVisualMaterial nodeVisualMaterial: type = DsgVarType.VisualMaterial; break;
                        case NodeWay nodeWay: 						type = DsgVarType.Way; break;
                        case NodeWayPointRef nodeWayPointRef: 		type = DsgVarType.WayPoint; break;
                    }

                    this[func].SetParameterType(i, type);

                    i++;
                }
            }
        }

        public TypeHintMap ProcedureTypeHints;
        public TypeHintMap FunctionTypeHints;
        public TypeHintMap FieldTypeHints;
        public TypeHintMap ConditionTypeHints;

        public TypeHints()
        {
            ProcedureTypeHints = new TypeHintMap();
            FunctionTypeHints = new TypeHintMap();
            FieldTypeHints = new TypeHintMap();
            ConditionTypeHints = new TypeHintMap();
        }
    }
}