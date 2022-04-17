using System;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace NodeScriptToBinary {

   /// <summary>
   /// 
   /// </summary>

   class Program {

      /// <summary>
      /// Program to convert old node trees (.eee, .fff) in editor script format 
      /// </summary>
      /// <param name="filename">Filename</param>
      static void Main(string[] args)
      {
         var lines = File.ReadAllLines(args[0]);

         foreach (var l in lines) {

            var indent = l.Count(c=>c=='\t');

            if (l.Trim().StartsWith("{CreateIntelligence")) {
               string defaultIntelligence = l.Substring(l.LastIndexOf(','), l.Length-l.LastIndexOf(',')-1);
               Debug.WriteLine($"Default intelligence: {defaultIntelligence}");
            }
         }
      }
   }
}
