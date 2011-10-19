using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using console2.Execution.Precompilation;
using console2.Plugin;

namespace console2.Execution
{
    public class Interpreter
    {
        public class InterPreterLogger
        {
            private Interpreter myInterPreter;
            public InterPreterLogger(Interpreter theInterpreter)
            {
                myInterPreter = theInterpreter;
            }

            public void printMessage(string message)
            {
                myInterPreter.theConsole.printnl(message);
            }

            

        }


        #region fields
        public int enableLoggingLevel = 2;
        private bool enableFirstTake = true;
        private PreInterpreter myPreInterpreter;
        private InterPreterLogger myLogger;
        private MyConsole theConsole;
        private int errorCount = 0;

    

        #endregion
        #region constructor
        public Interpreter(MyConsole theConsole)
        {
            myPreInterpreter = new PreInterpreter(this);
            this.theConsole = theConsole;
            myLogger = new InterPreterLogger(this);
        }

        #endregion
        #region getterSetter
        public void setInterPretionLogging(int number)
        {
            /*number Level sheet*/
            /* UnsuscpectError  70
             * SuspectERROR:    50
             * Warning:         20
             * HighStarting     2
             * HighEnding       2
             * Starting:        1
             * passing true     1
             * parameterpass    1
             * finishing        1
            */

            enableLoggingLevel = number;
        }
        public void setTakeFirstForInterpretion(bool flag)
        {
            enableFirstTake = flag;
        }

        public int enabledLoggingLevel()
        {
            return enableLoggingLevel;
        }

        #endregion
        private void printMessageToLogger(string message, int severityLevel)
        {
            if (severityLevel >= enableLoggingLevel)
                myLogger.printMessage(message);
        }
        public void interprete(string code)
        {
            try
            {
                printMessageToLogger("Interpretion started: " + code, 2);
                string preInterpreted;
                myPreInterpreter.preInterprete(code, out preInterpreted);
                ExecutionNode currentNode = ExecutionNode.seperate(preInterpreted);
                executeBaseNode(currentNode);
            }
            catch (Exception e)
            {
                printMessageToLogger(e.Message, 70);
                theConsole.myBenchObjectHandler.addObject(e, "ExecutionError" + errorCount);
                errorCount++;
            }
            printMessageToLogger("Interpretion ended", 2);
        }

        /// <summary>
        /// Executes the base node and all afterdot nodes. Basenodes are no methods and have no parameters.
        /// </summary>
        /// <param name="firstNode">The first node.</param>
        /// <returns></returns>
        private object executeBaseNode(ExecutionNode firstNode)
        {
            var executionText = firstNode.executionText;
            printMessageToLogger("Start processing baseNode: " + firstNode.executionText, 1);
            List<Object> possibilities = theConsole.myAutoCompleter.getAllFirstBasesObjects(AutoCompleter.getFirstPartOfName((firstNode.executionText)));
            #region preInterpreterReplacement
            //DIRECT PREINTERPRETE REPLACEMENT

            if (myPreInterpreter.getReplacementStringDictionary().Keys.Count > 0 && myPreInterpreter.getReplacementStringDictionary().ContainsKey(firstNode.executionText))
            {
                object preResult;
                myPreInterpreter.getReplacementStringDictionary().TryGetValue(firstNode.executionText, out preResult);

                possibilities = new List<object>(1) { preResult };
            }
            #endregion

            //END DIRECT PREINTERPRETE REPLACEMENT
            if (possibilities.Count == 0)
            {
                printMessageToLogger("No possible base object for: " + executionText, 50);
                throw new BaseItemNotFoundException();
            }
            if (!enableFirstTake && possibilities.Count > 1)
            {
                printMessageToLogger("To many possibilities found and firstTake not enabled: " + executionText, 50);
                throw new ToManyBaseFoundException();
            }
            if (firstNode.hasPostDotNode())
            {
                printMessageToLogger(executionText + " finished & given through to: " + firstNode.getPostDotNode().executionText, 1);
                return executeDotAttachedNode(firstNode.getPostDotNode(), possibilities[0]);
            }
            printMessageToLogger(executionText + " finished", 1);
            return possibilities[0];
        }

        /// <summary>
        /// Executes the dot attached node and all afterdot nodes.
        /// </summary>
        /// <param name="nextNode">The next node.</param>
        /// <param name="onWhich">The on which.</param>
        /// <returns></returns>
        private object executeDotAttachedNode(ExecutionNode nextNode, object onWhich)
        {
            Type onWichType = onWhich.GetType();
            printMessageToLogger("Start processing dotNode: " + nextNode.executionText, 1);
            var parameters = getParameterObjects(nextNode);
            //NameFilter
            var possibleMembers = getPossibleMembers(nextNode, onWichType.GetMembers(), parameters);
            if (possibleMembers.Count == 0)
            {
                printMessageToLogger("No possible members for: " + nextNode.executionText, 50);
                throw new DotItemNotFoundException();
            }
            //overloadFilter
            possibleMembers = getPossibleMembersFilteringOverloadMethods(parameters, possibleMembers);
            if (!enableFirstTake && possibleMembers.Count > 1)
            {
                printMessageToLogger("To many possibilities found and firstTake not enabled: " + nextNode.executionText, 50);
                throw new ToManyDotItemsFoundException();
            }
            var chosen = possibleMembers[0];
            object result;
            switch (chosen.MemberType)
            {
                case MemberTypes.Property:

                    result = onWichType.GetProperty(chosen.Name).GetValue(chosen, new object[0]);
                    break;

                case MemberTypes.Field:
                    result = onWichType.GetField(chosen.Name).GetValue(onWhich);
                    break;
                case MemberTypes.Method:
                    //VIA methodbase
                    result = ((MethodBase)chosen).Invoke(onWhich, parameters.ToArray());
                    break;
                case MemberTypes.Constructor:
                case MemberTypes.NestedType:
                case MemberTypes.Event:
                default:
                    throw new NotImplementedException();
                    break;
            }


            if (nextNode.hasPostDotNode())
            {
                printMessageToLogger(nextNode.executionText + " finished & given through to: " + nextNode.getPostDotNode().executionText, 1);
                return executeDotAttachedNode(nextNode.getPostDotNode(), result);
            }



            printMessageToLogger(nextNode.executionText + " finished", 1);
            return result;





        }

        private List<MemberInfo> getPossibleMembersFilteringOverloadMethods(List<object> parameters, List<MemberInfo> possibleMembers)
        {
            bool hasMethods = false;
            foreach (var possibleMember in
                possibleMembers.Where(possibleMember => possibleMember.MemberType == MemberTypes.Method))
            {
                hasMethods = true;
            }
            if (hasMethods)
            {
                possibleMembers = filterOnParameterTypes(possibleMembers, parameters);
            }
            return possibleMembers;
        }



        private List<MemberInfo> filterOnParameterTypes(List<MemberInfo> memberInfos, List<Object> parameters)
        {
            var retList = new List<MemberInfo>();
            foreach (var memberInfo in memberInfos)
            {
                var isGood = true;
                var d = memberInfo.MemberType;
                if (memberInfo.MemberType.Equals(MemberTypes.Method))
                {
                    MethodInfo methodInfo = (MethodInfo)memberInfo;
                    if (parameters.Count != methodInfo.GetParameters().Count())
                        continue;

                    for (int i = 0; i < methodInfo.GetParameters().Count(); i++)
                    {
                        Type a = methodInfo.GetParameters()[i].ParameterType;
                        isGood = isGood && (a.IsInstanceOfType(parameters[i]));
                    }

                }
                if (isGood)
                    retList.Add(memberInfo);
            }
            return retList;

        }





        /// <summary> 
        /// Filter the not name like, and also if parameters are added, the not methods.
        /// </summary>
        /// <param name="nextNode">The next node.</param>
        /// <param name="initialMembers">The initial members.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns></returns>
        private List<MemberInfo> getPossibleMembers(ExecutionNode nextNode, MemberInfo[] initialMembers, List<object> parameters)
        {
            var possibleMembers = initialMembers.Where(possibleMember => possibleMember.Name.ToLower() == nextNode.executionText.ToLower()).ToList();
            if (parameters.Count > 0)
            {
                List<MemberInfo> replaceList = new List<MemberInfo>();
                foreach (var memberInfo in possibleMembers)
                {
                    if (memberInfo.MemberType == MemberTypes.Method)
                        replaceList.Add(memberInfo);
                }
                possibleMembers = replaceList;
            }
            return possibleMembers;
        }
        private List<Object> getParameterObjects(ExecutionNode myNode)
        {
            var retList = new List<object>();
            foreach (var parameterNode in
                myNode.getParameterNodes().Where(parameterNode => parameterNode.getAnteDotNode() == null))
            {
                printMessageToLogger("Parameter passing: " + parameterNode.executionText, 1);
                retList.Add(executeBaseNode(parameterNode));
            }
            return retList;
        }

    }



    public class ExecutionNode
    {
        #region fields
        public string executionText = "";

        private List<ExecutionNode> parameterNodes = new List<ExecutionNode>();
        private ExecutionNode parentNode;

        private ExecutionNode anteParallelNode;
        private ExecutionNode PostParallelNode;

        private ExecutionNode anteDotNode;
        private ExecutionNode postDotNode;

        #endregion

        #region constructor
        public ExecutionNode(ExecutionNode parentNode)
        {
            this.parentNode = parentNode;
            if (parentNode != null)
                parentNode.parameterNodes.Add(this);
        }
        #endregion

        #region getterSetter

        public bool hasPostDotNode()
        {
            return (getPostDotNode() != null);
        }
        public ExecutionNode getParentNode()
        {
            return parentNode;
        }
        public List<ExecutionNode> getParameterNodes()
        {
            return parameterNodes;
        }
        public ExecutionNode getAnteParallelNode()
        {
            return anteParallelNode;
        }
        public ExecutionNode getPostParallelNode()
        {
            return PostParallelNode;
        }
        public ExecutionNode getPostDotNode()
        {
            return postDotNode;
        }
        public ExecutionNode getAnteDotNode()
        {
            return anteDotNode;
        }

        public void removeParameterNode(ExecutionNode executionNode)
        {
            if (executionNode == null)
                return;
            if (parameterNodes.Contains(executionNode))
            {
                parameterNodes.Remove(executionNode);
                if (executionNode.parentNode == this)
                {
                    executionNode.parentNode = null;
                }
            }
        }


        #endregion

        /// <summary>
        /// Seperates the specified text, by walking the tree.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <returns></returns>
        public static ExecutionNode seperate(string text)
        {
            String usingText = text.Trim();
            usingText = removeUselesWhite(usingText);
            ExecutionNode rootNode = new ExecutionNode(null);
            try
            {
                ExecutionNode currentNode = rootNode;
                int currentCharacter = 0;
                for (int i = 0; i < usingText.Length; i++)
                {
                    Char c = usingText[i];

                    switch (c)
                    {
                        case '(':
                            currentNode.executionText = usingText.Substring(currentCharacter, i - currentCharacter).Trim();
                            currentCharacter = i + 1;
                            ExecutionNode nextNode = new ExecutionNode(currentNode);
                            currentNode = nextNode;
                            break;
                        case ')':
                            if (currentNode.executionText.Equals(""))
                                currentNode.executionText = usingText.Substring(currentCharacter, i - currentCharacter).Trim();
                            if (currentNode.executionText.Equals(""))
                            {
                                ExecutionNode papa = currentNode.parentNode;
                                currentNode.parentNode.removeParameterNode(currentNode);
                                currentNode = papa;
                                currentCharacter = i + 1;
                                break;
                            }
                            currentNode = currentNode.parentNode;
                            currentCharacter = i + 1;
                            break;
                        case ' ':
                            if (currentNode.executionText.Equals(""))
                            {
                                if (!usingText.Substring(currentCharacter, i - currentCharacter).Trim().Equals(""))
                                    currentNode.executionText = usingText.Substring(currentCharacter, i - currentCharacter).Trim();
                            }
                            currentCharacter = i + 1;
                            currentNode.PostParallelNode = new ExecutionNode(currentNode.parentNode);
                            currentNode.PostParallelNode.anteParallelNode = currentNode;
                            currentNode = currentNode.PostParallelNode;
                            break;
                        case '.':
                            if (currentNode.executionText.Equals(""))
                                currentNode.executionText = usingText.Substring(currentCharacter, i - currentCharacter).Trim();
                            currentCharacter = i + 1;
                            currentNode.postDotNode = new ExecutionNode(currentNode.parentNode);
                            currentNode.postDotNode.anteDotNode = currentNode;
                            currentNode = currentNode.postDotNode;
                            break;
                        case ',':
                            if (currentNode.executionText.Equals(""))
                                currentNode.executionText = usingText.Substring(currentCharacter, i - currentCharacter).Trim();
                            currentCharacter = i + 1;
                            ExecutionNode myNextNode = new ExecutionNode(currentNode.parentNode);
                            currentNode = myNextNode;
                            break;

                        case ';':
                            if (currentNode.executionText.Equals(""))
                                currentNode.executionText = usingText.Substring(currentCharacter, i - currentCharacter).Trim();
                            break;
                    }
                }
            }
            catch (NullReferenceException nRE)
            { throw new ArgumentException("ILLEGAL AMOUNT OF BRACKETS!"); }

            return rootNode;
        }
        private static string removeUselesWhite(string text)
        {

            bool[] toDelete = new bool[text.Length];
            int dimLenth = text.Length - 1;
            for (int i = 0; i < text.Length; i++)
            {//first beforeBracket and komma...
                int opposite = dimLenth - i;
                if (i != 0 && (text[opposite] == ' ') && (text[opposite + 1].Equals(')') || text[opposite + 1].Equals(',') || text[opposite + 1].Equals(';') || toDelete[opposite + 1] == true))
                    toDelete[opposite] = true;
                //after komma...
                if (i != 0 && (text[i] == ' ') && (text[i - 1].Equals(',') || text[i - 1].Equals('(')))
                    toDelete[i] = true;

            }
            string retText = "";
            for (int i = 0; i < text.Length; i++)
            {
                if (!toDelete[i])
                    retText += text[i];
            }

            return retText;
        }

    }
    public class InterPreterException : Exception
    {


    }

    public class DotItemNotFoundException : InterPreterException { }
    public class ToManyDotItemsFoundException : InterPreterException { }
    public class ToManyBaseFoundException : InterPreterException { }
    public class BaseItemNotFoundException : InterPreterException
    {


    }

}
