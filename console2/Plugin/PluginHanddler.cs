using System;
using System.Collections.Generic;
using System.Linq;

namespace console2.Plugin
{


    /**
     * Takes care of the types of the plugin, also having for each type a string representation.
     * 
     */
    public class PluginHanddler
    {

        /**
         * Stores the relation of types and strings of the main types
         * 
         */
       private readonly SortedDictionary<String, Type> mainTypeDictStringToTypes = new SortedDictionary<string, Type>();
       private readonly SortedSet<String> mainTypeStrings = new SortedSet<String>();

        private readonly SortedDictionary<String, IPlugin> pluginDictStringToPlugins = new SortedDictionary<string, IPlugin>();
        private readonly SortedSet<String> pluginStrings = new SortedSet<String>();
      
        private readonly MyConsole myConsole;


        /// <summary>
        /// Initializes a new instance of the <see cref="PluginHanddler"/> class.
        /// </summary>
        /// <param name="myConsole">My console.</param>
        public PluginHanddler(MyConsole myConsole)
        {
            this.myConsole = myConsole;
        }
        #region getInforMationOutOfDictionary
        public SortedSet<String> getPossibleItemsOutOfMainTypeDictStrings(string selectionString)
        {
            return mainTypeStrings.GetViewBetween(selectionString, selectionString + "zzzzzz");
        }
        public SortedSet<String> getPossibleItemsOutOfpluginDict(string selectionString)
        {
            return pluginStrings.GetViewBetween(selectionString, selectionString + "zzzzzz");
        }
        public List<Object> getPossibleItemsOutOfMainTypeDictObjects(string selectionString)
        {
            var sortedSet = mainTypeStrings.GetViewBetween(selectionString, selectionString + "zzzzzz");
           var retList = new List<Object>();
            if (sortedSet.Count == 0)
                return retList;
            foreach (var s in sortedSet)
            {
                Type type;
                mainTypeDictStringToTypes.TryGetValue(s,out type);
            retList.Add(type);
            }
           return retList;
        }
        public List<Object> getPossibleItemsOutOfpluginDictObjects(string selectionString)
        {
            var sortedSet = pluginStrings.GetViewBetween(selectionString, selectionString + "zzzzzz");
            var retList = new List<Object>();
            if (sortedSet.Count == 0)
                return retList;
            foreach (String s in sortedSet)
            {
                IPlugin plugin;
                if(!pluginDictStringToPlugins.TryGetValue(s, out plugin))
                    return retList;    /*totaal afhandelen ;)*/
                
                
                retList.Add(plugin);
            }
            return retList;
        }
        #endregion

        #region MovingPlugins


        /// <summary>
        /// Adds the plugin.
        /// </summary>
        /// <param name="plugin">The plugin.</param>
        /// <param name="name">The name.</param>
        public void addPlugin(IPlugin plugin,string name)
        {
            if (mainTypeDictStringToTypes.ContainsKey(name))
            {
                myConsole.print("plugin " + name + " allready added!");
                return;
               
            }
            if (pluginDictStringToPlugins.Select(type => type.Value).Any(value => value == plugin))
            {
                myConsole.print("plugin" + name + "allready added under other name!");
                return;
            }
            pluginStrings.Add(name);
            pluginDictStringToPlugins.Add(name, plugin);
            addMainTypes(plugin);

        }
       
        private void addMainTypes(IPlugin plugin)
        {
            foreach (var mainType in plugin.getMainTypes())
            {
                var stringOfType = getStringOfType(mainType);
                mainTypeDictStringToTypes.Add(stringOfType, mainType);
                mainTypeStrings.Add(stringOfType);
            }
        }
        
        #endregion

        /*
        public Type getType(IPlugin myPlugin, String type)
        {
            Dictionary<String, Type> lookinDict = getDictionaryOf(myPlugin);
            if (lookinDict == null)
            {
                return null;
            }
            Type retType;
            if (lookinDict.TryGetValue(type, out retType))
                return retType;
                return null;
        }

        */


        /// <summary>
        /// Gets the type of the string of.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        /// Zet het type om naar een leesbaar type.
        public static string getStringOfType(Type type)
        {
            string typeString = type.ToString();
            if (!type.IsGenericType)
            {
                return getTypeNotGeneric(typeString);
            }
            //generic
            int firstOccurence = typeString.IndexOf('´');
            string firstPart = getTypeNotGeneric(typeString.Substring(0, firstOccurence));
            int numberOfGenericParameters = 0;
            firstOccurence++;
            while ("0123456789".Contains(typeString[firstOccurence].ToString()))
            {
                int result;
                int.TryParse(typeString[firstOccurence].ToString(), out result);
                numberOfGenericParameters = 10 * numberOfGenericParameters + result;
                firstOccurence++;
            }
            string postgeneric = "<";
            numberOfGenericParameters--;
            for (int i = 0; i < numberOfGenericParameters; i++)
            {
                postgeneric = postgeneric + ",";
            }
            postgeneric = postgeneric + ">";
            return firstPart + postgeneric;
        }

        /// <summary>
        /// Return the last part of a type in string representation, removing all namespace parts.
        /// </summary>
        /// <param name="typeString">The type string .</param>
        /// <returns>the type string without namespace parts</returns>
        private static string getTypeNotGeneric(string typeString)
        {
            if (!typeString.Contains("."))
                return typeString;
            int lastIndex = typeString.LastIndexOf('.');
            return typeString.Substring(lastIndex + 1);


        }
    }
}
