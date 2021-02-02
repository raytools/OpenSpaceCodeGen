using System;
using System.Collections.Generic;
using System.Text;
using OpenSpaceCodeGen.AITypes;
using OpenSpaceCodeGen.Nodes;

namespace OpenSpaceCodeGen.Translation.CFamily {

    public static class KeywordTranslation{

        private static TranslateAction[] IndentedChildren(params TranslateAction[] pre)
        {
            var actions = new List<TranslateAction>();
            actions.AddRange(pre);
            actions.AddRange(new TranslateAction[]
            {
                TranslateAction.NextLine,
                "{", TranslateAction.NextLine,
                TranslateAction.Indent,
                TranslateAction.VisitChildren(),
                TranslateAction.Unindent,
                "}", TranslateAction.NextLine
            });

            return actions.ToArray();
        }

        public static NodeTranslator TranslateKeyword(NodeKeyWord node, CodeGenerator gen)
        {
            int randomizer;
            var keyword = node.GetKeyword(gen);
            switch (keyword) {
                case EnumKeyword.If:
                    return NodeTranslator.Sequence("if (", 0,")");
                case EnumKeyword.IfNot:
                    return NodeTranslator.Sequence("if (!(",0,"))");
                case EnumKeyword.If2:
                case EnumKeyword.If4:
                case EnumKeyword.If8:
                case EnumKeyword.If16:
                case EnumKeyword.If32:
                case EnumKeyword.If64:
                    randomizer = int.Parse(keyword.ToString().Substring(2));
                    return NodeTranslator.Sequence(
                        $"if (globalRandomizer%{randomizer}==0 && (", TranslateAction.VisitChildren(),"))");
                case EnumKeyword.IfNot2:
                case EnumKeyword.IfNot4:
                case EnumKeyword.IfNot8:
                case EnumKeyword.IfNot16:
                case EnumKeyword.IfNot32:
                case EnumKeyword.IfNot64:
                    randomizer = int.Parse(keyword.ToString().Substring(2));
                    return NodeTranslator.Sequence($"if (globalRandomizer%{randomizer}==0 && !(","))");
                case EnumKeyword.IfDebug:
                    return NodeTranslator.Sequence(IndentedChildren("if (false /* debug */)"));
                case EnumKeyword.IfNotU64:
                    return NodeTranslator.Sequence(IndentedChildren("if (true /* NOT U64 */)"));
                case EnumKeyword.Then:
                    return NodeTranslator.Sequence(IndentedChildren());
                case EnumKeyword.Else:
                    return NodeTranslator.Sequence(IndentedChildren(
                        "else"));
                case EnumKeyword.Goto:
                case EnumKeyword.EngineGoto:
                    return NodeTranslator.Sequence("goto");
                case EnumKeyword.Me: return NodeTranslator.Sequence("this");
                case EnumKeyword.MainActor: return NodeTranslator.Sequence("MainActor");
                case EnumKeyword.Nobody:
                case EnumKeyword.NoInput:
                case EnumKeyword.NoSoundEvent:
                case EnumKeyword.NoSuperObject:
                case EnumKeyword.Nowhere:
                case EnumKeyword.NoSOLinksWord:
                case EnumKeyword.NoGraph:
                case EnumKeyword.NoAction:
                case EnumKeyword.NoGMT:
                case EnumKeyword.NoVMT:
                case EnumKeyword.Noway:
                    return NodeTranslator.Sequence(keyword.ToString());

                case EnumKeyword.EmptyText: return NodeTranslator.Sequence("\"\"");
                case EnumKeyword.CapsNull: return NodeTranslator.Sequence("null");
                case EnumKeyword.While:
                case EnumKeyword.BeginWhile: return NodeTranslator.Sequence("while (");
                case EnumKeyword.EndWhile: return NodeTranslator.Sequence("}");
                case EnumKeyword.World: return NodeTranslator.Sequence("World");

                default:
                    throw new ArgumentOutOfRangeException(nameof(keyword), keyword, null);
            }
        }

    }

}
