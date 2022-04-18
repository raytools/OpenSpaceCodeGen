using System;
using System.Collections.Generic;
using OpenSpaceCodeGen.AITypes;
using OpenSpaceCodeGen.Config;
using OpenSpaceCodeGen.Translation;

namespace OpenSpaceCodeGen.Nodes {
    public abstract class Node
    {
        public int param;
        public short padding;
        public byte depth;
        public byte typeID;

        public Node Parent;
        public List<Node> Children = new List<Node>();
        public int Offset;

        public abstract NodeTranslator GetTranslator(CodeGenerator gen);
        protected virtual void UpdateTypeHints(CodeGenerator gen) { }

        protected Node() { }

        public static Node FromBytes(NodeSettings settings, AIType types, byte[] nodeBytes, int offset)
        {
            var typeID = nodeBytes[settings.indexOfType];
            var type = types.GetNodeType(typeID);

            Node node;

            switch (type) {
                case NodeType.KeyWord: node = new NodeKeyWord();
                    break;
                case NodeType.Condition: node = new NodeCondition();
                    break;
                case NodeType.Operator: node = new NodeOperator();
                    break;
                case NodeType.Function: node = new NodeFunction();
                    break;
                case NodeType.Procedure: node = new NodeProcedure();
                    break;
                case NodeType.MetaAction: node = new NodeMetaAction();
                    break;
                case NodeType.BeginMacro: node = new NodeBeginMacro();
                    break;
                case NodeType.EndMacro: node = new NodeEndMacro();
                    break;
                case NodeType.Field: node = new NodeField();
                    break;
                case NodeType.DsgVarRef: node = new NodeDsgVarRef();
                    break;
                case NodeType.Constant: node = new NodeConstant();
                    break;
                case NodeType.Real: node = new NodeReal();
                    break;
                case NodeType.Button: node = new NodeButton();
                    break;
                case NodeType.ConstantVector: node = new NodeConstantVector();
                    break;
                case NodeType.Vector: node = new NodeVector();
                    break;
                case NodeType.Mask: node = new NodeMask();
                    break;
                case NodeType.ModuleRef: node = new NodeModuleRef();
                    break;
                case NodeType.DsgVarId: node = new NodeDsgVarId();
                    break;
                case NodeType.String: node = new NodeString();
                    break;
                case NodeType.LipsSynchroRef: node = new NodeLipsSynchroRef();
                    break;
                case NodeType.FamilyRef: node = new NodeFamilyRef();
                    break;
                case NodeType.PersoRef: node = new NodePersoRef();
                    break;
                case NodeType.ActionRef: node = new NodeActionRef();
                    break;
                case NodeType.SuperObjectRef: node = new NodeSuperObjectRef();
                    break;
                case NodeType.WayPointRef: node = new NodeWayPointRef();
                    break;
                case NodeType.TextRef: node = new NodeTextRef();
                    break;
                case NodeType.ComportRef: node = new NodeComportRef();
                    break;
                case NodeType.SoundEventRef: node = new NodeSoundEventRef();
                    break;
                case NodeType.ObjectTableRef: node = new NodeObjectTableRef();
                    break;
                case NodeType.GameMaterialRef: node = new NodeGameMaterialRef();
                    break;
                case NodeType.ParticleGenerator: node = new NodeParticleGenerator();
                    break;
                case NodeType.VisualMaterial: node = new NodeVisualMaterial();
                    break;
                case NodeType.ModelRef: node = new NodeModelRef();
                    break;
                case NodeType.DataType42: node = new NodeType42();
                    break;
                case NodeType.CustomBits: node = new NodeCustomBits();
                    break;
                case NodeType.Caps: node = new NodeCaps();
                    break;
                case NodeType.SubRoutine: node = new NodeSubRoutine();
                    break;
                case NodeType.Null: node = new NodeNull();
                    break;
                case NodeType.GraphRef: node = new NodeGraphRef();
                    break;
                case NodeType.ConstantRef: node = new NodeConstantRef();
                    break;
                case NodeType.RealRef: node = new NodeRealRef();
                    break;
                case NodeType.SurfaceRef: node = new NodeSurfaceRef();
                    break;
                case NodeType.Way: node = new NodeWay();
                    break;
                case NodeType.DsgVar: node = new NodeDsgVar();
                    break;
                case NodeType.SectorRef: node = new NodeSectorRef();
                    break;
                case NodeType.EnvironmentRef: node = new NodeEnvironmentRef();
                    break;
                case NodeType.FontRef: node = new NodeFontRef();
                    break;
                case NodeType.Color: node = new NodeColor();
                    break;
                case NodeType.Module: node = new NodeModule();
                    break;
                case NodeType.LightInfoRef: node = new NodeLightInfoRef();
                    break;
                case NodeType.Unknown: node = new NodeUnknown();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            node.Offset = offset;
            node.param = BitConverter.ToInt32(nodeBytes, settings.indexOfParam);
            node.depth = nodeBytes[settings.indexOfIndent];
            node.typeID = typeID;

            return node;
        }

        public NodeType GetNodeType(AIType type)
        {
            return type.GetNodeType(this.typeID);
        }

        public void Visit(CodeGenerator gen)
        {
            GetTranslator(gen).Translate(this, gen);
            if (gen.AIModel != null) {
                UpdateTypeHints(gen);
            }
        }

        public abstract string ToString(CodeGenerator gen);
    }
}
