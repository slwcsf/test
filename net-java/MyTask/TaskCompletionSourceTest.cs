using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.sunlw.net.MyTask
{
    public class TaskCompletionSourceTest
    {
        public async Task Test()
        {
            TaskCompletionSource<bool> tcs = new();
            await Task.Factory.StartNew(async () => await ChoiceTicket(tcs));
            await Task.Factory.StartNew(async () => await PayForMoney(tcs));
        }

        public async Task<bool> ChoiceTicket(TaskCompletionSource<bool> tcs)
        {

            Console.WriteLine("选购火车票");
            Console.WriteLine("跳转到到支付页面");

            await tcs.Task;
            Console.WriteLine("订票完成");
            return tcs.Task.Result;
        }

        //选择支付页面
        public async Task PayForMoney(TaskCompletionSource<bool> tcs)
        {

            await Task.Delay(3000).ContinueWith((t) => Console.WriteLine("选购支付宝支付"));

            tcs.SetResult(true);
            Console.WriteLine("支付完成");
            Console.WriteLine("跳到火车票软件完成");
        }
    }
}
