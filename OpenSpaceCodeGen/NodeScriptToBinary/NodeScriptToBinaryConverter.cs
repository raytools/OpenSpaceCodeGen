using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using OpenSpaceCodeGen.AIModels;
using OpenSpaceCodeGen.AITypes;
using OpenSpaceCodeGen.Config;
using OpenSpaceCodeGen.Translation.Context;
using OpenSpaceCodeGen.Util;

namespace OpenSpaceCodeGen.NodeScriptToBinary {

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
      public bool IsSchedule;

      public Rule(int index, string[] lines, bool isSchedule)
      {
         this.Index = index;
         this.Lines = lines;
         this.IsSchedule = isSchedule;
      }

   }

   public class NodeScriptToBinaryConverter
   {
      private NodeSettings nodeSettings;
      private string inputDirectory;
      private string outputDirectory;

      private Dictionary<uint, string> hashToNameMap;

      class DsgVarsAndConstants {
         public List<string> dsgVars;
         public List<string> constants;

         public DsgVarsAndConstants()
         {
            dsgVars = new List<string>();
            constants = new List<string>();
         }
      }

      private DsgVarsAndConstants dsgVarsAndConstants;

      public NodeScriptToBinaryConverter(NodeSettings nodeSettings, string inputDirectory, string outputDirectory)
      {
         this.nodeSettings = nodeSettings;
         this.inputDirectory = inputDirectory;
         this.outputDirectory = outputDirectory;

         hashToNameMap = new Dictionary<uint, string>();

         dsgVarsAndConstants = new DsgVarsAndConstants();
      }

      public void ConvertAll()
      {
         var files = Directory.GetFiles(inputDirectory);
         foreach (var f in files) {
            if (Path.GetExtension(f) == ".eee") {
               Convert(f, "rule");
            }
            if (Path.GetExtension(f) == ".fff") {
               Convert(f, "reflex");
            }
         }

         File.WriteAllText(Path.Combine(outputDirectory, "pointers.json"), JsonConvert.SerializeObject(hashToNameMap));

         // TODO: fill this in?
         var aiModel = new AIModelMetaData(0, "", new DirectoryInfo(inputDirectory).Name, Array.Empty<string>(),
            Array.Empty<string>(), new List<AIModelMetaData.DsgVar>(), new Dictionary<PointerInMap, string>(),
            new Dictionary<PointerInMap, AIModelMetaData.PersoNames>());

         File.WriteAllText(Path.Combine(outputDirectory, "aimodel.json"), JsonConvert.SerializeObject(aiModel));

         File.WriteAllText(Path.Combine(outputDirectory, "dsgVarsAndConstants.json"), JsonConvert.SerializeObject(dsgVarsAndConstants));
      }

      /// <summary>
      /// Convert old node trees (.eee, .fff) to CPA script
      /// </summary>
      /// <param name="inputFile">Filename to parse</param>
      /// <param name="subdirectory">Subdirectory to output to</param>
      private void Convert(string inputFile, string subdirectory)
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

            Debug.WriteLine($"comportLine @{currentComportLineIndex} = {comportLine}");

            if (TryParseParameters(comportLine, "CreateComport:", out parameters, out string comportName)) {

               int comportIndex = int.Parse(parameters[0]);
               int ruleCount = int.Parse(parameters[1]);
               int currentRuleLineIndex = currentComportLineIndex + 1;

               // Schedule has an extra rule at the start that's not included in the ruleCount
               if (lines[currentComportLineIndex + 1].Trim().StartsWith("{CreateSchedule:")) {
                  ruleCount += 1;
               }

               List<Rule> comportRules = new List<Rule>();

               for (int ruleIter = 0; ruleIter < ruleCount; ruleIter++) {

                  string ruleLine = lines[currentRuleLineIndex];

                  int ruleIndex = 0;
                  int numLines = 0;

                  bool isSchedule = false;

                  if (TryParseParameters(ruleLine, "CreateRule:(", out parameters, out _)) {
                     // CreateRule:(ruleIndex, itemCount)

                     ruleIndex = int.Parse(parameters[0]);
                     numLines = int.Parse(parameters[1]);

                  } else if (TryParseParameters(ruleLine, "CreateSchedule:(", out parameters, out _)) {
                     // CreateSchedule:(ruleIndex, itemCount)

                     ruleIndex = int.Parse(parameters[0]);
                     numLines = int.Parse(parameters[1]);

                     isSchedule = true;
                  }

                  string[] ruleLines = new string[numLines];
                  for (int i = 0; i < numLines; i++) {
                     ruleLines[i] = lines[currentRuleLineIndex + 1 + i];
                  }

                  currentRuleLineIndex += (numLines + 2);

                  comportRules.Add(new Rule(ruleIndex, ruleLines, isSchedule));
               }

               Comports.Add(new Comport(comportName, comportRules.ToArray()));

               currentComportLineIndex = currentRuleLineIndex + 1;
            }
         }

         Intelligence intelligence = new Intelligence(Path.GetFileNameWithoutExtension(inputFile), Comports.ToArray(), defaultComport);
         Debug.WriteLine(JsonConvert.SerializeObject(intelligence));
         
         if (string.IsNullOrEmpty(outputDirectory)) {
            throw new Exception("No valid output directory");
         }

         if (!Directory.Exists(outputDirectory)) {
            Directory.CreateDirectory(outputDirectory);
         }

         AIModelMetaData metaData = new AIModelMetaData(0, "", intelligence.Name, null, null, null, null, null);
         File.WriteAllText(Path.Combine(outputDirectory, "aimodel.json"), JsonConvert.SerializeObject(metaData));

         foreach (var comport in intelligence.Comports) {

            var comportDirectory = Path.Combine(outputDirectory, subdirectory, comport.Name);
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

      private void AddHashName(int hash, string name)
      {
         if (!hashToNameMap.ContainsKey((uint)hash)) {
            hashToNameMap.Add((uint)hash, name);
         }
      }

      /// <summary>
      /// This is where the magic happens
      /// </summary>
      /// <param name="line">The text line</param>
      /// <returns></returns>
      private byte[] CreateNodeBytesFromLine(string line)
      {
         // TODO: maybe make config/aiTypes not hardcoded, but eh
         var aiTypes = AIType.R2_19980306;

         var bytes = new byte[nodeSettings.sizeOfNode];

         string l = line.Trim();

         string nodeTypeName = l.Substring(0, l.IndexOf('('));
         string[] nodeParams = l.Substring(l.IndexOf('(') + 1).TrimEnd(')').Split(',');

         int.TryParse(nodeParams[1], out int indent);

         byte typeID = 0;
         int param = -1;

         if (Enum.TryParse(nodeTypeName, out NodeType nodeType)) {
            typeID = aiTypes.GetNodeTypeID(nodeType);
         } else {
            throw new ArgumentException($"Cannot find Node Type {nodeTypeName}");
         }

         void AssertParamFound()
         {
            if (param == -1) throw new ArgumentException($"Unknown param for \"{nodeParams[0]}\"");
         }

         switch (nodeType) {
            case NodeType.Unknown: throw new ArgumentException("Unknown NodeType");
            case NodeType.KeyWord:

               if (Enum.TryParse(nodeParams[0], out EnumKeyword keyword)) {
                  param = Array.IndexOf(aiTypes.KeywordTable, keyword);
               }

               AssertParamFound(); break;

            case NodeType.Condition:
               param = Array.IndexOf(aiTypes.ConditionTable, nodeParams[0]);
               AssertParamFound(); break;

            case NodeType.Operator:

               bool parsed = Enum.TryParse(nodeParams[0], out EnumOperator operatorType);
               
               bool dotOp = true;

               if (nodeParams[0].Trim('"') == ".X") {
                  operatorType = EnumOperator.Dot_X;
               } else if (nodeParams[0].Trim('"') == ".Y") {
                  operatorType = EnumOperator.Dot_Y;
               } else if (nodeParams[0].Trim('"') == ".Z") {
                  operatorType = EnumOperator.Dot_Z;
               } else if (nodeParams[0].Trim('"') == ".X:=") {
                  operatorType = EnumOperator.Dot_X_Assign;
               } else if (nodeParams[0].Trim('"') == ".Y:=") {
                  operatorType = EnumOperator.Dot_Y_Assign;
               } else if (nodeParams[0].Trim('"') == ".Z:=") {
                  operatorType = EnumOperator.Dot_Z_Assign;
               } else {
                  dotOp = false;
               }

               if (parsed || dotOp) {
                  param = Array.IndexOf(aiTypes.OperatorTable, operatorType);
               }

               AssertParamFound(); break;

            case NodeType.Function:
               param = Array.IndexOf(aiTypes.FunctionTable, nodeParams[0]);
               AssertParamFound(); break;

            case NodeType.Procedure:
               param = Array.IndexOf(aiTypes.ProcedureTable, nodeParams[0]);
               AssertParamFound(); break;

            case NodeType.MetaAction:
               param = Array.IndexOf(aiTypes.MetaActionTable, nodeParams[0]);
               AssertParamFound(); break;

            case NodeType.BeginMacro:
               break;
            case NodeType.EndMacro:
               break;
            case NodeType.Field:

               param = Array.IndexOf(aiTypes.FieldTable, nodeParams[0]);
               AssertParamFound(); break;

            case NodeType.ActionRef:
            case NodeType.ComportRef:
            case NodeType.DsgVarRef:
            case NodeType.PersoRef:
            case NodeType.ConstantRef:
            case NodeType.RealRef:
               var trim = nodeParams[0].Trim('"');
               var split = trim.Split(":");

               param = split[1].GetHashCode();
               AddHashName(param, split[1]);

               if (nodeType == NodeType.ConstantRef || nodeType == NodeType.RealRef) {
                  if (!dsgVarsAndConstants.constants.Contains(split[1])) {
                     dsgVarsAndConstants.constants.Add(split[1]);
                  }
               }

               if (nodeType == NodeType.DsgVarRef) {
                  if (!dsgVarsAndConstants.dsgVars.Contains(split[1])) {
                     dsgVarsAndConstants.dsgVars.Add(split[1]);
                  }
               }

               break;
            case NodeType.Constant:
               param = int.Parse(nodeParams[0]);
               break;
            case NodeType.Real:
               param = GetRealParam(nodeParams[0]);
               break;
            case NodeType.Button:
               param = nodeParams[0].GetHashCode();
               AddHashName(param, nodeParams[0]);
               break;
            case NodeType.ConstantVector:
            case NodeType.Vector:
               param = 0;
               break;
            case NodeType.Mask:
               param = int.Parse(nodeParams[0]);
               break;
            case NodeType.DsgVarId:
               param = int.Parse(nodeParams[0]);
               break;
            case NodeType.DataType42:
               break;
            case NodeType.CustomBits:
               param = int.Parse(nodeParams[0]);
               break;
            case NodeType.Caps:
               param = int.Parse(nodeParams[0]);
               break;
            default:
               param = nodeParams[0].GetHashCode();
               AddHashName(param, nodeParams[0]);
               break;
         }

         bytes[nodeSettings.indexOfIndent] = (byte)indent;
         bytes[nodeSettings.indexOfType] = typeID;

         var paramBytes = BitConverter.GetBytes(param);
         bytes[nodeSettings.indexOfParam + 0] = paramBytes[0];
         bytes[nodeSettings.indexOfParam + 1] = paramBytes[1];
         bytes[nodeSettings.indexOfParam + 2] = paramBytes[2];
         bytes[nodeSettings.indexOfParam + 3] = paramBytes[3];

         return bytes;
      }

      private static int GetRealParam(string nodeParam)
      {
         return BitConverter.ToInt32(BitConverter.GetBytes(float.Parse(nodeParam.Trim('"')))); // float bytes as int
      }
   }
}
