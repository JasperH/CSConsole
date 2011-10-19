using System;
using System.Reflection;

namespace console2.Plugin
{
    /// <summary>
    /// A Plugin class containing the basic types as int32, string,char.
    /// </summary>
    public class GeneralPlugin: IPlugin
    {

        
        public object invokeMethod(MethodInfo method, object target, params object[] parameters)
        {
            throw new NotImplementedException();
        }

        public object getBenchObjects(string name)
        {
            throw new NotImplementedException();
        }

        public string[] getBenchObjectNames()
        {
            throw new NotImplementedException();
        }

        public void assign(object target, object value)
        {
            throw new NotImplementedException();
        }

        public Type[] getMainTypes()
        {
           return new[]{1.GetType(),"string".GetType(),'c'.GetType()};
        }

        public Type[] getPluginExecutionTypes()
        {
            throw new NotImplementedException();
        }
    }
}
