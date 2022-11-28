using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.sunlw.net.MyTask
{
    public class ManualReset
    {
        public ManualResetEvent _resetEvent = new ManualResetEvent(true);

        public void Start()
        {
            Task task = new Task(async () =>
            {
                while (true)
                {
                    await Task.Delay(2000);
                    // 初始化为true时执行WaitOne不阻塞
                    _resetEvent.WaitOne();
                     _resetEvent.Reset();
                    // Doing something.......
                    await Task.Delay(2000);
                }
            });
            task.Start();
        }

        public void Suspend()
        {
            _resetEvent.Reset();
        }

        public void Continue()
        {
            _resetEvent.Set();
        }
    }
}