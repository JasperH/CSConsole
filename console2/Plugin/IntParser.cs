using System;

namespace console2.Plugin
{
    public class IntParser
    {

        public  int getTryParse(string value)
        {
            int result;
           if(!int.TryParse(value, out result))
               throw new ArgumentException();
            return result;
        }
    }
}