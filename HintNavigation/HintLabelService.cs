using System.Text;

namespace HintNavigation
{
    public static class HintLabelGeneration
    {
        public static char[] LabelCharacters = new[] { 'T', 'N', 'R', 'E', 'S', 'I', 'A', 'O', 'G', 'H', 'J', 'V', 'M', 'C' };

        public static char[] SecondaryCharacters = new[] { 'L', 'D' };

    /// <summary>
    /// Gets available hint strings
    /// </summary>
    /// <remarks>Adapted from vimium to give a consistent experience, see https://github.com/philc/vimium/blob/master/content_scripts/link_hints.js </remarks>
    /// <param name="hintCount">The number of hints</param>
    /// <returns>A list of hint strings</returns>
    public static IList<string> GenerateHintStrings(int hintCount)
        {
            var hintStrings = new List<string>();
            if (hintCount <= 0)
            {
                return hintStrings;
            }
            

            var digitsNeeded = (int)Math.Ceiling(Math.Log(hintCount) / Math.Log(LabelCharacters.Length));

            var wholeHintCount = (int)Math.Pow(LabelCharacters.Length, digitsNeeded);
            var shortHintCount = (wholeHintCount - hintCount) / LabelCharacters.Length;
            var longHintCount = hintCount - shortHintCount;

            var longHintPrefixCount = wholeHintCount / LabelCharacters.Length - shortHintCount;
            for (int i = 0, j = 0; i < longHintCount; ++i, ++j)
            {
                hintStrings.Add(new string(NumberToHintString(j, LabelCharacters, digitsNeeded).Reverse().ToArray()));
                if (longHintPrefixCount > 0 && (i + 1) % longHintPrefixCount == 0)
                {
                    j += shortHintCount;
                }
            }

            if (digitsNeeded > 1)
            {
                for (var i = 0; i < shortHintCount; ++i)
                {
                    hintStrings.Add(new string(NumberToHintString(i + longHintPrefixCount, LabelCharacters, digitsNeeded - 1).ToArray()));
                }
            }

            return hintStrings.ToList();
        }

        /// <summary>
        /// Converts a number like "8" into a hint string like "JK". This is used to sequentially generate all of the
        /// hint text. The hint string will be "padded with zeroes" to ensure its length is >= numHintDigits.
        /// </summary>
        /// <remarks>Adapted from vimium to give a consistent experience, see https://github.com/philc/vimium/blob/master/content_scripts/link_hints.js</remarks>
        /// <param name="number">The number</param>
        /// <param name="characterSet">The set of characters</param>
        /// <param name="noHintDigits">The number of hint digits</param>
        /// <returns>A hint string</returns>
        private static string NumberToHintString(int number, char[] characterSet, int noHintDigits = 0)
        {
            var divisor = characterSet.Length;
            var hintString = new StringBuilder();

            do
            {
                var remainder = number % divisor;
                hintString.Insert(0, characterSet[remainder]);
                number -= remainder;
                number /= (int)Math.Floor((double)divisor);
            } while (number > 0);

            // Pad the hint string we're returning so that it matches numHintDigits.
            // Note: the loop body changes hintString.length, so the original length must be cached!
            var length = hintString.Length;
            for (var i = 0; i < noHintDigits - length; ++i)
            {
                hintString.Insert(0, characterSet[0]);
            }

            return hintString.ToString();
        }
    }
}
