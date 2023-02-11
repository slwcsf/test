using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.sunlw.net.MyTask
{
    public class LockTest
    {
        // private static object lockObj = new object();
        private object lockObj = new object();

        public int name { get; set; }

        public async Task Test()
        {
            await Task.Delay(500);
            lock (lockObj)
            {
                Thread.Sleep(3000);
                Console.WriteLine(name);
            }
        }
    }
}