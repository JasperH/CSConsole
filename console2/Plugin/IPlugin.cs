using System;
using System.Collections.Generic;
using System.Reflection;

namespace console2.Plugin
{
    public interface IPlugin
    {
        //object invokeMethod(MethodInfo method, object target, params object[] parameters);
        //void assign(object target,object value);
        Type[] getMainTypes();
      //  Type[] getPluginExecutionTypes();
        /*iedere plugin heeft zijn types die gebruikt worden voor empty autocompletion. 
         * ieder object dat expliciet door plugin moet worden uitgevoerd moet in de pluginExecutionTypeList staan.
         */

    }

    /*class PluginExecutionType
    {
      
        private Type myType;
        private IPlugin myPlugin;
        PluginExecutionType(Type theType, IPlugin thePlugin)
        {
            myType = theType;
            myPlugin = thePlugin;
        }
        public Type getType()
        {
           return myType;
        }
        public IPlugin getPlugin()
        {
            return myPlugin;
        }
*/




    }



