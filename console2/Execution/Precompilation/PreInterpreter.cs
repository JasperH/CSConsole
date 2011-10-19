using System;
using System.Collections.Generic;

namespace console2.Execution.Precompilation
{
    public class PreInterpreter
    {
        private Interpreter myInterPreter;
        private Dictionary<String, Object> replacementStringDictionary = new Dictionary<string, object>();
        public Dictionary<String,Object> getReplacementStringDictionary()
        {
            return replacementStringDictionary;
        }

        public PreInterpreter(Interpreter myInterPreter)
        {
            this.myInterPreter = myInterPreter;
        }
        public void preInterprete(string textToInterprete, out string result)
        {
            replacementStringDictionary = new Dictionary<string, object>();
            result = textToInterprete;
           
            result = stringReplacement(textToInterprete, result);
      
        
        
        
        }

        private string stringReplacement(string textToInterprete, string result)
        {
            var stringCounter = 0;
            var beginOfString = 0;
            var inString = false;
            string retString = result.Substring(0);
            for (var i = 0; i < textToInterprete.Length; i++)
            {
                if (!textToInterprete[i].Equals('"')) continue;
                if (!inString)
                    beginOfString = i;
                if (inString)
                {
                    replacementStringDictionary.Add("replacementString" + stringCounter,
                                                    result.Substring(beginOfString+1, i - beginOfString -1));
                    retString = retString.Replace(result.Substring(beginOfString, i - beginOfString+1),
                                            "replacementString" + stringCounter);
                    stringCounter++;

                }
                inString = !inString;
            }
            return retString;
        }
    }
}