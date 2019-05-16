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
    class NLPArabicTokenizer
    {
        string Sentence {get; set;}

        public NLPArabicTokenizer() { }
        public NLPArabicTokenizer(string sentence)
        {
            Sentence = sentence;
        }

        public List<string> TokenizeSentence()
        {
            List<string> tokens = new List<string>();

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

            var tokenizer = tf.getTokenizer(new StringReader(Sentence));
            const string NEWLINE_TOKEN = "*NL*";
            while (tokenizer.hasNext())
            {
                var next = tokenizer.next() as CoreLabel;
                String word = next.word();

                if (word.Equals(NEWLINE_TOKEN) == false)
                {
                    tokens.Add(word);
                }
            }

            return tokens;
        }
    }
}
