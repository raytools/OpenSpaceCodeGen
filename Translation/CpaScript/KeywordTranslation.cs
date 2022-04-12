using System;
using System.Collections.Generic;
using System.Text;
using OpenSpaceCodeGen.AITypes;
using OpenSpaceCodeGen.Nodes;

namespace OpenSpaceCodeGen.Translation.CpaScript {

    public static class KeywordTranslation{

        private static TranslateAction[] IndentedChildren(params TranslateAction[] pre)
        {
            var actions = new List<TranslateAction>();
            actions.AddRange(pre);
            actions.AddRange(new TranslateAction[]
            {
                TranslateAction.NextLine,
                TranslateAction.Indent,
                TranslateAction.VisitChildren(),
                TranslateAction.Unindent,
                EnumKeyword.EndIf.ToString(), TranslateAction.NextLine
            });

            return actions.ToArray();
        }

        public static NodeTranslator TranslateKeyword(NodeKeyWord node, CodeGenerator gen, LanguageTranslationCpaScript translation)
        {
            int randomizer;
            var keyword = node.GetKeyword(gen);
            switch (keyword) {
                case EnumKeyword.If:
                case EnumKeyword.IfNot:
                case EnumKeyword.If2:
                case EnumKeyword.If4:
                case EnumKeyword.If8:
                case EnumKeyword.If16:
                case EnumKeyword.If32:
                case EnumKeyword.If64:
                case EnumKeyword.IfNot2:
                case EnumKeyword.IfNot4:
                case EnumKeyword.IfNot8:
                case EnumKeyword.IfNot16:
                case EnumKeyword.IfNot32:
                case EnumKeyword.IfNot64:
                case EnumKeyword.IfDebug:
                case EnumKeyword.IfNotU64:

                   return NodeTranslator.Sequence($"{keyword} ", 0);
                case EnumKeyword.Then:
                   return NodeTranslator.Sequence(IndentedChildren(
                      $" {keyword}"));
               case EnumKeyword.Else:

                  List<TranslateAction> actions = new List<TranslateAction>();
                  actions.Add(TranslateAction.RemoveLastLine()); // Remove the EndIf when we're using else
                  actions.Add(TranslateAction.RemoveLastLine()); // ..
                  actions.Add(TranslateAction.NextLine);
                  actions.AddRange(IndentedChildren($"{keyword}"));

                  return NodeTranslator.Sequence(actions.ToArray());
                case EnumKeyword.Goto:
                case EnumKeyword.EngineGoto:
                case EnumKeyword.Me:
                case EnumKeyword.MainActor:
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
                case EnumKeyword.EmptyText:
                case EnumKeyword.CapsNull:
                case EnumKeyword.While:
                case EnumKeyword.BeginWhile:
                case EnumKeyword.EndWhile:
                case EnumKeyword.World:

                   return NodeTranslator.Sequence(keyword.ToString());
            case EnumKeyword.Vector:
            case EnumKeyword.ConstantVector: // TODO: untested
               return NodeTranslator.Sequence(keyword.ToString(), "(", TranslateAction.VisitChildren(","), ")");
                default:
                    throw new ArgumentOutOfRangeException(nameof(keyword), keyword, null);
            }
        }

    }

}
