using System;
using System.Collections.Generic;
using System.Linq;

namespace console2.Plugin
{
    public class BenchObjectHandler
    {
        private MyConsole theConsole;
        private Dictionary<String, Object> benchObjects = new Dictionary<string, object>();
        public BenchObjectHandler(MyConsole theConsole)
        {
            this.theConsole = theConsole;
        }
        public Object getBObject(string name)
        {
            Object retObject = null;
            benchObjects.TryGetValue(name, out retObject);
            return retObject;
        }
        public void addObject(Object myObject, string name)
        {
            try
            {
                benchObjects.Add(name, myObject);
       
            }
            catch (Exception e)
            {
                theConsole.printnl("Error at adding object "+ name + ", aka  " + myObject.ToString());
                theConsole.printnl(e.Message);
            }
            theConsole.updateObjectBench();
        }
        public void removeObject(string name)
        {
            try
            {

                benchObjects.Remove(name);
            }
            catch(Exception e)
            {
                theConsole.printnl("Error at removing object " + name);
                theConsole.printnl(e.Message);
       
            }
            theConsole.updateObjectBench();
        }
        public List<String> getNamesMatchBenchObjects(string selectionString)
        {
            var lower = selectionString.ToLower();
            return benchObjects.Keys.Where(key => key.ToLower().StartsWith(lower)).ToList();
        }
        public List<Object> getObjectsMatchBenchObjects(string selectionString)
        {
            var lower = selectionString.ToLower();
            return (from keyValuePair in benchObjects
                    where keyValuePair.Key.ToLower().StartsWith(lower)
                    select keyValuePair.Value).ToList();
        }

        public List<String> getNamesBenchObjects()
        {
            return benchObjects.Keys.ToList();
        }
    }
}
