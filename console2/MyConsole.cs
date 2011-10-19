using System;
using System.Windows.Forms;
using console2.Execution;
using console2.Plugin;

namespace console2
{
    public class MyConsole
    {
        private readonly Form1 myForm1;
        public readonly PluginHanddler myPluginHanddler;
        public readonly BenchObjectHandler myBenchObjectHandler;
        public readonly AutoCompleter myAutoCompleter;
        public readonly Interpreter myInterpreter;
        
        public MyConsole()
        {
            myForm1 = new Form1(this);
            myAutoCompleter = new AutoCompleter(this);
            myPluginHanddler = new PluginHanddler(this);
            myInterpreter = new Interpreter(this);
            myBenchObjectHandler = new BenchObjectHandler(this);
            myPluginHanddler.addPlugin(new GeneralPlugin(),"mainPlugin");
            myBenchObjectHandler.addObject(this,"MConsole");
            myBenchObjectHandler.addObject(true,"true");
            myBenchObjectHandler.addObject(false, "false");
            myBenchObjectHandler.addObject(new IntParser(),"intParser");
            
            myBenchObjectHandler.addObject(new TestClass(), "testObject");

        }
        /// <summary>
        /// Adds the plugin.
        /// </summary>
        /// <param name="plugin">The plugin.</param>
        /// <param name="name">The name of the plugin.</param>
        public void addPlugin(IPlugin plugin, string name)
        {
            myPluginHanddler.addPlugin(plugin, name);
        }

        public void run()
        {
            Application.EnableVisualStyles();
            Application.Run(myForm1);
        }

        public void interPrete(string code)
        {
            myInterpreter.interprete(code);
        }
        public void updateObjectBench()
        {
            ListBox getMyListBox = myForm1.getMyListBox();
            getMyListBox.DataSource = myBenchObjectHandler.getNamesBenchObjects();
            getMyListBox.Refresh();
        }
        public void removePlugin(IPlugin plugin)
        {
            throw new NotImplementedException();
        }
        public void print(string text)
        {
            myForm1.getTextBox().Text += text;
        }
        public void printnl(string text)
        {
            myForm1.getTextBox().Text += string.Format("\r\n{0}", text);
        }
        
    }
}
