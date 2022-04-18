using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using OpenSpaceCodeGen.AITypes;
using OpenSpaceCodeGen.Config;

namespace NodeScriptToBinary {

   struct Intelligence
   {
      public string Name;
      public Comport[] Comports;
      public string DefaultComport;

      public Intelligence(string name, Comport[] comports, string defaultComport)
      {
         this.Name = name;
         this.Comports = comports;
         this.DefaultComport = defaultComport;
      }
   }

   struct Comport
   {
      public string Name;
      public Rule[] Rules;

      public Comport(string name, Rule[] rules)
      {
         this.Name = name;
         this.Rules = rules;
      }
   }

   struct Rule
   {
      public int Index;
      public string[] Lines;

      public Rule(int index, string[] lines)
      {
         this.Index = index;
         this.Lines = lines;
      }
   }

   public class NodeScriptToBinary {

      /// <summary>
      /// Convert old node trees (.eee, .fff) to CPA script
      /// </summary>
      /// <param name="inputFile">Filename to parse</param>
      /// /// <param name="inputFile">Directory to output </param>
      public static void Convert(string inputFile, string outputDirectory)
      {
         var lines = File.ReadAllLines(inputFile);
         
         string defaultComport = string.Empty;
         string currentComport = "";
         string[] parameters = null;

         int comportCount = 0;
         int currentComportLineIndex = -1;

         List<Comport> Comports = new List<Comport>();

         for(int i=0;i<lines.Length;i++) {
            string l = lines[i];

            var indent = l.Count(c=>c=='\t');

            if (TryParseParameters(l, "CreateIntelligence:(", out parameters, out _)) {
               comportCount = int.Parse(parameters[0]);
               defaultComport = parameters[2];

               currentComportLineIndex = i + 1;

               break;
            }
            
         }

         if (currentComportLineIndex < 0) {
            throw new Exception("No comports found");
         }

         for (int comportIter = 0; comportIter < comportCount; comportIter++) {
            string comportLine = lines[currentComportLineIndex];
            if (TryParseParameters(comportLine, "CreateComport:", out parameters, out string comportName)) {
               int comportIndex = int.Parse(parameters[0]);
               int ruleCount = int.Parse(parameters[1]);
               int currentRuleLineIndex = currentComportLineIndex + 1;

               List<Rule> comportRules = new List<Rule>();

               for (int ruleIter = 0; ruleIter < ruleCount; ruleIter++) {

                  string ruleLine = lines[currentRuleLineIndex];
                  TryParseParameters(ruleLine, "CreateRule:(", out parameters, out _); // CreateRule:(ruleIndex, itemCount)

                  int ruleIndex = int.Parse(parameters[0]);
                  int numLines = int.Parse(parameters[1]);

                  string[] ruleLines = new string[numLines];
                  for (int i = 0; i < numLines; i++) {
                     ruleLines[i] = lines[currentRuleLineIndex + 1 + i];
                  }

                  currentRuleLineIndex += (numLines + 2);

                  comportRules.Add(new Rule(ruleIndex, ruleLines));
               }

               Comports.Add(new Comport(comportName, comportRules.ToArray()));
            }
         }

         Intelligence intelligence = new Intelligence(Path.GetFileNameWithoutExtension(inputFile), Comports.ToArray(), defaultComport);
         Debug.WriteLine(JsonConvert.SerializeObject(intelligence));

         var intelligenceDir = Path.Combine(outputDirectory, intelligence.Name);

         if (string.IsNullOrEmpty(outputDirectory)) {
            throw new Exception("No valid output directory");
         }

         if (Directory.Exists(outputDirectory)) {
            Directory.Delete(outputDirectory, true);
         }
         
         Directory.CreateDirectory(outputDirectory);

         foreach (var comport in intelligence.Comports) {

            var comportDirectory = Path.Combine(outputDirectory, comport.Name);
            Directory.CreateDirectory(comportDirectory);
            foreach (var rule in comport.Rules) {
               var ruleFilePath = (Path.Combine(comportDirectory, $"{rule.Index}.osb"));

               List<byte> ruleBytes = new List<byte>();

               foreach (var line in rule.Lines) {
                  ruleBytes.AddRange(CreateNodeBytesFromLine(line));
               }

               File.WriteAllBytes(ruleFilePath, ruleBytes.ToArray());
            }
         }
      }

      /// <summary>
      /// Attempts to reads a list of parameters from a string with the format prefix[out itemName](param1,param2,param3,...)
      /// </summary>
      /// <param name="parameters">The prefix to search for - end with a ( if no prefix is neeeded</param>
      /// <returns>A list of the parameters or null if no match was found</returns>
      private static bool TryParseParameters(string line, string prefix, out string[] output, out string itemName)
      {
         output = null;
         itemName = string.Empty;

         var indexOfPrefix = line.IndexOf(prefix, StringComparison.Ordinal);

         if (indexOfPrefix > 0) {

            if (!prefix.EndsWith('(')) {
               int bracketOpenIndex = line.IndexOf('(');
               itemName = line[(indexOfPrefix+prefix.Length) .. bracketOpenIndex];
            }

            var parameters = line.Substring(indexOfPrefix+prefix.Length+itemName.Length).TrimStart('(').TrimEnd(')');
            output = parameters.Split(',');
            return true;
         }

         return false;
      }

      /// <summary>
      /// This is where the magic happens
      /// </summary>
      /// <param name="line">The text line</param>
      /// <returns></returns>
      private static byte[] CreateNodeBytesFromLine(string line)
      {
         // TODO: maybe make config/aiTypes not hardcoded, but eh
         var config = NodeSettings.SettingsDefault;
         var aiTypes = AIType.R2_19980306;

         var bytes = new byte[config.sizeOfNode];

         string l = line.Trim();

         string nodeTypeName = l.Substring(0, l.IndexOf('('));
         string[] nodeParams = l.Substring(l.IndexOf('(') + 1).TrimEnd(')').Split(',');

         int.TryParse(nodeParams[1], out int indent);

         byte typeID = 0;
         int param = 0;

         if (Enum.TryParse(nodeTypeName, out NodeType nodeType)) {
            typeID = aiTypes.GetNodeTypeID(nodeType);
         } else {
            throw new ArgumentException($"Cannot find Node Type {nodeTypeName}");
         }

         switch (nodeType) {
            case NodeType.Unknown: throw new ArgumentException("Unknown NodeType");
            case NodeType.KeyWord:
               Enum.TryParse(nodeParams[0], out EnumKeyword keyword);
               param = (byte)Array.IndexOf(aiTypes.KeywordTable, keyword); break;
            case NodeType.Condition:
               param = (byte)Array.IndexOf(aiTypes.ConditionTable, nodeParams[0]); break;
            case NodeType.Operator:
               Enum.TryParse(nodeParams[0], out EnumKeyword operatorType);
               param = (byte)Array.IndexOf(aiTypes.OperatorTable, operatorType); break;
            case NodeType.Function:
               param = (byte)Array.IndexOf(aiTypes.FunctionTable, nodeParams[0]); break;
            case NodeType.Procedure:
               param = (byte)Array.IndexOf(aiTypes.ProcedureTable, nodeParams[0]); break;
            case NodeType.MetaAction:
               param = (byte)Array.IndexOf(aiTypes.MetaActionTable, nodeParams[0]); break;
            case NodeType.BeginMacro:
               break;
            case NodeType.EndMacro:
               break;
            case NodeType.Field:
               param = (byte)Array.IndexOf(aiTypes.FieldTable, nodeParams[0]); break;
               break;
            case NodeType.DsgVarRef:
               param = 0; // TODO: actual dsgvar ID's, mapping etc.
               break;
            case NodeType.Constant:
               param = int.Parse(nodeParams[0]);
               break;
            case NodeType.Real:
               param = BitConverter.ToInt32(BitConverter.GetBytes(float.Parse(nodeParams[0]))); // float bytes as int
               break;
            case NodeType.Button:
               break;
            case NodeType.ConstantVector:
               break;
            case NodeType.Vector:
               break;
            case NodeType.Mask:
               break;
            case NodeType.ModuleRef:
               break;
            case NodeType.DsgVarId:
               break;
            case NodeType.String:
               break;
            case NodeType.LipsSynchroRef:
               break;
            case NodeType.FamilyRef:
               break;
            case NodeType.PersoRef:
               break;
            case NodeType.ActionRef:
               break;
            case NodeType.SuperObjectRef:
               break;
            case NodeType.WayPointRef:
               break;
            case NodeType.TextRef:
               break;
            case NodeType.ComportRef:
               break;
            case NodeType.SoundEventRef:
               break;
            case NodeType.ObjectTableRef:
               break;
            case NodeType.GameMaterialRef:
               break;
            case NodeType.ParticleGenerator:
               break;
            case NodeType.VisualMaterial:
               break;
            case NodeType.ModelRef:
               break;
            case NodeType.DataType42:
               break;
            case NodeType.CustomBits:
               break;
            case NodeType.Caps:
               break;
            case NodeType.SubRoutine:
               break;
            case NodeType.Null:
               break;
            case NodeType.GraphRef:
               break;
            case NodeType.ConstantRef:
               break;
            case NodeType.RealRef:
               break;
            case NodeType.SurfaceRef:
               break;
            case NodeType.Way:
               break;
            case NodeType.DsgVar:
               break;
            case NodeType.SectorRef:
               break;
            case NodeType.EnvironmentRef:
               break;
            case NodeType.FontRef:
               break;
            case NodeType.Color:
               break;
            case NodeType.Module:
               break;
            case NodeType.LightInfoRef:
               break;
            default:
               throw new ArgumentOutOfRangeException();
         }

         bytes[config.indexOfIndent] = (byte)indent;
         bytes[config.indexOfType] = typeID;

         var paramBytes = BitConverter.GetBytes(param);
         bytes[config.indexOfParam + 0] = paramBytes[0];
         bytes[config.indexOfParam + 1] = paramBytes[1];
         bytes[config.indexOfParam + 2] = paramBytes[2];
         bytes[config.indexOfParam + 3] = paramBytes[3];

         return bytes;
      }
   }
}
