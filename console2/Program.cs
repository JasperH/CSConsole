using System;
using System.Reflection;
using System.Windows.Forms;
using console2.Execution;
using MHGameWork.TheWizards.Utilities;


namespace console2
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main()
        {

            /*var runner = new TestRunner();
            runner.TestsAssembly = Assembly.GetExecutingAssembly();
            
            runner.Run();*/
                           
            Application.SetCompatibleTextRenderingDefault(false);
            //     printDemoMemberInfo();

                 MyConsole aConsole = new MyConsole();
            aConsole.run();
        }

        private static void printDemoMemberInfo()
        {
            MyConsole a = new MyConsole();
            var b =  a.GetType().GetMembers();
            foreach (MemberInfo memberInfo in b)
            {
                Console.Out.WriteLine("**********NEW*********");
                Console.Out.WriteLine("reflectedType: " + memberInfo.ReflectedType);
             
                Console.Out.WriteLine("memberType: "+memberInfo.MemberType);
                if (memberInfo.MemberType == MemberTypes.Method)
                    Console.Out.WriteLine(a.GetType().GetMethod(memberInfo.Name).ReturnType);
                Console.Out.WriteLine("declaringType: " + memberInfo.DeclaringType);
                Console.Out.WriteLine(memberInfo.Name);
                Console.Out.WriteLine(memberInfo.ToString());
            }
        }
    }
  
}