using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAppCallBackFunc
{
    class Program
    {
        public delegate int AsyncCallbackExample(int x, int y);
        static void Main(string[] args)
        {
            ClsCallback_Invoker ObjClsCallback_Invoker = new ClsCallback_Invoker();

            //Synchronous Callback Code need to wait to complete add operation and 
            //when completed then return a callback(GetResult)

            ObjClsCallback_Invoker.Callback_Synchronous_Add
                 (new ClsCallback_Invoker.CallBack(GetResult), 10, 20);

            //Suppose we have a long running processing method and 
            //we cannot wait to complete this, then we need a callback function
            //in this case code execution immediately return to main method 
            //after calling below methods so we can continue other task while 
            //long running task completed then a callback(Waiting_For_LongRunnigTask) will be executed.

            AsyncCallbackExample objasync =
              new AsyncCallbackExample(ObjClsCallback_Invoker.Callback_Asynchronous_longRunningTask);
            objasync.BeginInvoke(20, 10, new AsyncCallback(Waiting_For_LongRunnigTask), objasync);
            Console.WriteLine("Long running Task Started and return to main method");

            Console.ReadLine();
        }

        private static void GetResult(int Result)
        {
            Console.WriteLine(Result);
        }

        private static void Waiting_For_LongRunnigTask(IAsyncResult Result)
        {
            AsyncCallbackExample obj = (AsyncCallbackExample)Result.AsyncState;

            Console.WriteLine(obj.EndInvoke(Result));
            Console.WriteLine("Task Completed");

        }
    }

    public class ClsCallback_Invoker
    {
        public delegate void CallBack(int Result);

        public void Callback_Synchronous_Add(CallBack Method, int x, int y)
        {
            CallBack ObjCallback = new CallBack(Method);
            ObjCallback(x + y);
        }

        public int Callback_Asynchronous_longRunningTask(int x, int y)
        {
            System.Threading.Thread.Sleep(12000);

            return x - y;
        }
    }
}
