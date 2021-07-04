using System;
using System.Collections.Generic;
using System.Text;
using OpenSpaceCodeGen.Translation;

namespace OpenSpaceCodeGen.Util {
    public static class StringUtil {
        public static string IndentMultilineString(this string str, int spaceCount)
        {
            List<string> indentedLines = new List<string>();

            foreach (var line in str.Split(Environment.NewLine)) {
                indentedLines.Add(new string(' ', spaceCount) + line);
            }

            return string.Join(Environment.NewLine, indentedLines);
        }

        /// <summary>
        /// Repeat any string <i>repeatCount</i> amount of times.
        /// </summary>
        /// <param name="str">The input string</param>
        /// <param name="repeatCount">How often to repeat this string. A value of 1 will yield the original string as the result.</param>
        /// <returns></returns>
        public static string Repeat(this string str, int repeatCount)
        {
            if (repeatCount == 1) {
                return str;
            }

            if (repeatCount < 0) {
                throw new ArgumentOutOfRangeException(nameof(repeatCount),
                    "The repeatCount should be greater than zero!");
            }

            string result = "";
            for (int i = 0; i < repeatCount; i++) {
                result += str;
            }

            return result;
        }

    }
}
