using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;


namespace console2
{
    public class AutoCompleter
    {

        private readonly MyConsole myconsole;

        public AutoCompleter(MyConsole myconsole)
        {
            this.myconsole = myconsole;
        }

        /// <summary>
        /// Autocompletes text.
        /// </summary>
        /// <param name="sourceText">The source text.</param>
        /// <param name="word">The word.</param>
        /// <param name="cursorPosition">The cursor position.</param>
        /// <param name="newPosition">The new position.</param>
        /// <returns></returns>
        public string autoCompleteText(string sourceText, string word, int cursorPosition, out int newPosition)
        {

            newPosition = cursorPosition;
            var listedItemsOfText = getListedItemsOfText(sourceText, cursorPosition);
            var retString = sourceText;
            if (listedItemsOfText.Length > 0)
            {
                var lengthOfOldWord = listedItemsOfText[listedItemsOfText.Length - 1].Length;
                newPosition -= lengthOfOldWord;
                retString = retString.Remove(cursorPosition - lengthOfOldWord, lengthOfOldWord);
                cursorPosition -= lengthOfOldWord;
            }
            newPosition += word.Length;
            return retString.Insert(cursorPosition, word);
        }
        public string autoCompleteText(string sourceText, int cursorPosition, out int newPosition)
        {

            var possibilities = getCompletionItemsOfText(sourceText, cursorPosition);
            if (possibilities.Length == 0)
            {
                newPosition = cursorPosition;
                return sourceText;
            }
            return autoCompleteText(sourceText, possibilities[0], cursorPosition, out newPosition);


        }

        /// <summary>
        /// Gets the completion items of the given text.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="position">The position.</param>
        /// <returns></returns>
        public string[] getCompletionItemsOfText(string text, int position)
        {
            
            var listedItems = getListedItemsOfText(text, position);
            Console.Out.WriteLine("COMPLETIONSTART");
            foreach (var listedItem in listedItems)
            {
                Console.Out.WriteLine(listedItem);
            }
            Console.Out.WriteLine("COMPLETIONEND");
            var retItems = getPossibleItemsOutOfTextItems(listedItems);
            foreach (var retItem in retItems)
            {
                Console.Out.WriteLine(retItem);
            }
            return retItems;
        }

        #region SUBgetCompletionItemsOfText

        private static string[] getListedItemsOfText(string text, int position)
        {
            if (position == 0)
                return new[] { "" };
            var lastChar = text[position - 1];
            if (lastChar == '<' || lastChar == ',' || lastChar == ' ' || lastChar == '(')
                return new[] { "" };
            var i = position - 1;
            i = getStartIndex(text, i, 0);
            var useFullString = text.Substring(i, position - i);

            return getListOfSubString(useFullString);
            /*
            int genericDepth = getGenericDepth(position, text);
            if(genericDepth != 0)
            */
        }
        #region SUBgetListedItemsOfText
        private static string[] getListOfSubString(string useFullString)
        {
            var retList = new List<string>();
            var length = useFullString.Length;
            var startWord = 0;
            var bracketDepth = 0;
            for (var j = 0; j < length; j++)
            {
                if (useFullString[j] == '(')
                    bracketDepth++;
                if (useFullString[j] == ')')
                    bracketDepth--;
                if (bracketDepth == 0 && useFullString[j] == '.')
                {
                    retList.Add(useFullString.Substring(startWord, j - startWord).Trim());
                    startWord = j + 1;
                }
            }
            
            if (startWord < length)
            {
                retList.Add(useFullString.Substring(startWord, length - startWord).Trim());
            }

            if (useFullString[useFullString.Length - 1] == '.')
                retList.Add("");

            return retList.ToArray();
        }
        private static int getStartIndex(string text, int i, int normalParenThesis)
        {
            while (i >= 0 && ((normalParenThesis > 0) || text[i] != '('))
            {
                var theChar = text[i];
                if (theChar == '(')
                    normalParenThesis--;
                if (theChar == ')')
                    normalParenThesis++;
                if (theChar == ' ' && normalParenThesis == 0)
                {
                    break;
                }
                i--;
            }
            i++;
            return i;
        }
        #endregion
        private string[] getPossibleItemsOutOfTextItems(string[] listedItems)
        {
            if (listedItems.Length == 0)
                return new string[0];
            if (listedItems.Length == 1)
                return getAllFirstBasesString(listedItems[0]).ToArray();
            var length = listedItems.Length;
            var nextObjects = getAllFirstBasesObjects(listedItems[0]);
            if (nextObjects.Count == 0)
                return new string[0];
            var nextTypes = new List<Type>(1) { nextObjects[0].GetType() };
            for (var i = 1; i < length - 1; i++)
            {
                nextTypes = getNextTypes(nextTypes[0], listedItems[i]);
                if (nextTypes.Count == 0)
                    return new string[0];
            }
            return getNextStrings(nextTypes[0], listedItems[length - 1]);

        }
        #region SUBgetNextStrings
        public static string[] getNextStrings(Type previousType, string nextString)
        {
            nextString = getFirstPartOfName(nextString);
            var nextNames = new List<String>();
            foreach (var memberInfo in
                previousType.GetMembers().Where(memberInfo => memberInfo.Name.ToLower().StartsWith(nextString.ToLower())))
            {
                switch (memberInfo.MemberType)
                {
                    case MemberTypes.Constructor:
                        break;
                    case MemberTypes.Property:
                    case MemberTypes.Field:
                        nextNames.Add(memberInfo.Name);
                        break;
                    case MemberTypes.Method:
                        if (!nextNames.Contains(memberInfo.Name + "()"))
                            nextNames.Add(memberInfo.Name + "()");
                        break;
                    default:
                        break;
                }
            }
            return nextNames.ToArray();
        }
        private List<Type> getNextTypes(Type previousType, string nextString)
        {
            nextString = getFirstPartOfName(nextString);
            List<Type> nextTypes = new List<Type>();
            foreach (MemberInfo memberInfo in
                previousType.GetMembers().Where(memberInfo => memberInfo.Name.ToLower().StartsWith(nextString.ToLower())))
            {
                try
                {
                    switch (memberInfo.MemberType)
                    {

                        case MemberTypes.Constructor:
                            break;

                        case MemberTypes.Field:
                            Type fieldType = previousType.GetField(memberInfo.Name).FieldType;
                            if (!nextTypes.Contains(fieldType))
                                nextTypes.Add(fieldType);
                            break;
                        case MemberTypes.Property:
                            Type propertyType = previousType.GetProperty(memberInfo.Name).PropertyType;
                            if (!nextTypes.Contains(propertyType))
                            nextTypes.Add(propertyType);
                            break;
                        case MemberTypes.Method:
                            Type returnType = previousType.GetMethod(memberInfo.Name).ReturnType;
                            if(!nextTypes.Contains(returnType))
                            nextTypes.Add(returnType);
                            break;
                        default:
                            break;
                    }
                }
                catch (AmbiguousMatchException aMe)
                {
                    Console.Out.WriteLine("******aME occured!!!!********");
                    Console.Out.WriteLine(aMe.Message);
                }
            }
            return nextTypes;
        }
        public static string getFirstPartOfName(string nextString)
        {
            int firstOccurenceNormalBracket = nextString.IndexOf('(');
            int firstOccurenceXBracket = nextString.IndexOf('<');
            int end = nextString.Length;
            if (firstOccurenceXBracket != -1)
            {
                end = firstOccurenceXBracket;
            }
            if (firstOccurenceNormalBracket != -1)
            {
                end = Math.Min(firstOccurenceNormalBracket, end);
            }

            nextString = nextString.Substring(0, end);
            return nextString;
        }
        private List<String> getAllFirstBasesString(string selectionString)
        {
            List<String> retString = new List<string>();
            retString.AddRange(myconsole.myPluginHanddler.getPossibleItemsOutOfMainTypeDictStrings(selectionString));
            retString.AddRange(myconsole.myPluginHanddler.getPossibleItemsOutOfpluginDict(selectionString));
            retString.AddRange(myconsole.myBenchObjectHandler.getNamesMatchBenchObjects(selectionString));
            return retString;
        }
        public  List<Object> getAllFirstBasesObjects(string selectionString)
        {
            List<Object> retObjects;
            retObjects = myconsole.myPluginHanddler.getPossibleItemsOutOfpluginDictObjects(selectionString);
            retObjects.AddRange(myconsole.myPluginHanddler.getPossibleItemsOutOfMainTypeDictObjects(selectionString));
            retObjects.AddRange(myconsole.myBenchObjectHandler.getObjectsMatchBenchObjects(selectionString));
            return retObjects;

        }
        #endregion
        #endregion


    }
}