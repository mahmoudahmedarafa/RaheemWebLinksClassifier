using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using edu.stanford.nlp.international.arabic.process;
using edu.stanford.nlp.ling;
using edu.stanford.nlp.util;
using java.io;

namespace DigiSay_Task
{
    class Program
    {
        static void Main(string[] args)
        {
            string s = "جامعة الدول العربية هي منظمة تضم دولا في الشرق الأوسط وأفريقيا";

            var parameters =
                new[]
                {
                    "normArDigits", "normAlif", "normYa", "removeDiacritics", "removeTatweel", "removeProMarker",
                    "removeSegMarker", "removeMorphMarker", "removeLengthening", "atbEscaping"
                };
            var tokenizerOptions = StringUtils.argsToProperties(parameters);
            var tf = tokenizerOptions.containsKey("atb")
                ? ArabicTokenizer.atbFactory()
                : ArabicTokenizer.factory();

            foreach (String option in parameters)
            { tf.setOptions(option); }
            tf.setOptions("tokenizeNLs");

            int nLines = 0;
            int nTokens = 0;
            var tokenizer = tf.getTokenizer(new StringReader(s));
            var printSpace = false;
            const string NEWLINE_TOKEN = "*NL*";
            while (tokenizer.hasNext())
            {
                ++nTokens;
                var next = tokenizer.next() as CoreLabel;
                String word = next.word();
                if (word.Equals(NEWLINE_TOKEN))
                {
                    ++nLines;
                    printSpace = false;
                    System.Console.WriteLine();
                }
                else
                {
                    if (printSpace) System.Console.Write(" ");
                    System.Console.Write(word);
                    printSpace = true;
                }
            }
            System.Console.WriteLine("\nDone! Tokenized %d lines (%d tokens)%n", nLines, nTokens);

        }
    }
}
