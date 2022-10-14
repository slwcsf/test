using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Sources;

namespace com.sunlw.net.MyTask
{
    public class MyValueTaskTest
    {
        public async ValueTask<string> GetStringAsync(MyValueTaskSource<string> source, string key)
        {
            // 创建异步任务
            ValueTask<string> task = new ValueTask<string>(source, 0);
            // source.SetResult("123");
            return await task;
        }

        public async Task Test()
        {
            // 自定义状态机
            var source = new MyValueTaskSource<string>();
            await Task.Factory.StartNew(async () =>
            {
                await Task.Delay(10000);
                source.SetResult("你好");
            });
            var aa = await GetStringAsync(source, "123");
            var bb = 11;
        }
    }

    public class MyValueTaskSource<TRusult> : IValueTaskSource<TRusult>
    {
        private ManualResetValueTaskSourceCore<TRusult> _source = new ManualResetValueTaskSourceCore<TRusult>();

        #region 实现接口，告诉调用者，任务是否已经完成，以及是否有结果，是否有异常等

        // 获取结果
        public TRusult GetResult(short token)
        {
            return _source.GetResult(token);
        }

        // 获取状态，这个示例中，用不到令牌 token
        public ValueTaskSourceStatus GetStatus(short token)
        {
            return _source.GetStatus(token);
        }

        #endregion 实现接口，告诉调用者，任务是否已经完成，以及是否有结果，是否有异常等

        #region 实现状态机，能够控制此任务是否已经完成，以及是否有异常

        // 以及完成任务，并给出结果
        public void SetResult(TRusult result)
        {
            _source.SetResult(result);
        }

        // 要执行的任务出现异常
        public void SetException(Exception exception)
        {
            _source.SetException(exception);
        }

        public void OnCompleted(Action<object?> continuation, object? state, short token, ValueTaskSourceOnCompletedFlags flags)
        {
            _source.OnCompleted(continuation, state, token, flags);
        }

        #endregion 实现状态机，能够控制此任务是否已经完成，以及是否有异常
    }
}